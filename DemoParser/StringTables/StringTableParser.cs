using DemoParser_Core.Streams.BitStream;
using DemoParser_Core.Entities;
using DemoParser_Core.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using DemoParser_Core.Events;

namespace DemoParser_Core.StringTables
{
    public static class StringTableParser
    {
		public static void ParsePacket(Stream stream, DemoParser parser)
        {
			using (IBitStream reader = BitStreamUtil.Create(stream)) {
				int numTables = reader.ReadByte();

				for (int i = 0; i < numTables; i++) {
					string tableName = reader.ReadString();

					ParseStringTable(reader, tableName, parser);
				}
			}
        }

		public static void ParseStringTable(IBitStream reader, string tableName, DemoParser parser)
        {
			int numStrings = (int)reader.ReadInt(16);

            for (int i = 0; i < numStrings; i++)
            {
                string stringName = reader.ReadString();

                if (stringName.Length >= 100)
                    throw new Exception("Roy said I should throw this.");

                if (reader.ReadBit())
                {
                    int userDataSize = (int)reader.ReadInt(16);

                    byte[] data = reader.ReadBytes(userDataSize);

					switch (tableName)
					{
						case "userinfo":
							PlayerInfo info = PlayerInfo.ParseFrom(new BinaryReader(new MemoryStream(data)));
							UpdatePlayers(info, parser);
							break;
						case "instancebaseline": // TODO implement
							//int classid = int.Parse(stringName);
							//parser.instanceBaseline[classid] = data;
							break;
					}
                }
            }

            // Client side stuff
	        if ( reader.ReadBit() )
	        {
		        int numstrings = (int)reader.ReadInt(16);
		        for ( int i = 0 ; i < numstrings; i++ )
		        {
			        reader.ReadString(); // stringname

			        if ( reader.ReadBit() )
			        {
				        int userDataSize = ( int )reader.ReadInt(16);

				        reader.ReadBytes( userDataSize );

			        }
			        else
			        {
			        }
		        }
	        }
        }

		public static void ParseStringTableMessage(CSVCMsg_CreateStringTable table, DemoParser parser)
		{
			using (IBitStream reader = BitStreamUtil.Create(table.string_data))
			{
				if (reader.ReadBit())
					throw new NotImplementedException("Encoded with dictionaries, unable to decode");

				int nTemp = table.max_entries;
				int nEntryBits = 0;
				while ((nTemp >>= 1) != 0)
					++nEntryBits;


				List<string> history = new List<string>();

				int lastEntry = -1;

				for (int i = 0; i < table.num_entries; i++)
				{
					int entryIndex = lastEntry + 1;
					// read in the entity-index
					if (!reader.ReadBit())
					{
						entryIndex = (int)reader.ReadInt(nEntryBits);
					}

					lastEntry = entryIndex;

					// Read the name of the string into entry.
					string entry = "";
					if (entryIndex < 0 || entryIndex >= table.max_entries)
					{
						throw new InvalidDataException("bogus string index");
					}

					if (reader.ReadBit())
					{
						bool substringcheck = reader.ReadBit();

						if (substringcheck)
						{
							int index = (int)reader.ReadInt(5);
							int bytestocopy = (int)reader.ReadInt(5);

							entry = history[index].Substring(0, bytestocopy);

							entry += reader.ReadString(1024);
						}
						else
						{
							entry = reader.ReadString(1024);
						}
					}

					if (entry == null)
						entry = "";

					if (history.Count > 31)
						history.RemoveAt(0);

					// Read in the user data.
					byte[] userdata = new byte[0];
					if (reader.ReadBit())
					{
						if (table.user_data_fixed_size)
						{
							userdata = reader.ReadBits(table.user_data_size_bits);
						}
						else
						{
							int bytesToRead = (int)reader.ReadInt(14);

							userdata = reader.ReadBytes(bytesToRead);
						}
					}

					if (userdata.Length == 0)
						break;

					// Now we'll parse the players out of it.
					BinaryReader playerReader = new BinaryReader(new MemoryStream(userdata));
					PlayerInfo info = PlayerInfo.ParseFrom(playerReader);

					UpdatePlayers(info, parser);
				}
			}
		}

		private static void UpdatePlayers(PlayerInfo playerInfo, DemoParser parser)
		{
			if (!parser.players.ContainsKey(playerInfo.UserID))
			{
				Player player = new Player(playerInfo);
				parser.players[playerInfo.UserID] = player;
				parser.EventsManager.RaisePlayerParsed(new PlayerParsedEventArgs(player));
			}
			else
			{
				parser.players[playerInfo.UserID].Update(playerInfo);
			}
		}
    }
}
