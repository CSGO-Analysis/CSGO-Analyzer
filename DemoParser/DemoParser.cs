using DemoParser_Core.DataTables;
using DemoParser_Core.Entities;
using DemoParser_Core.Events;
using DemoParser_Core.Messages;
using DemoParser_Core.Packets;
using DemoParser_Core.StringTables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DemoParser_Core
{
	public class DemoParser
	{
		internal const string DEMO_HEADER_ID = "HL2DEMO";
		internal const int DEMO_PROTOCOL = 4;

		
		private BinaryReader reader;

		internal DemoPacketParser DemoPacketsParser;
		internal DataTableParser DataTablesParser;
		internal StringTableParser StringTablesParser;

		public EventsManager EventsManager { get; private set; }

		public DemoHeader Header { get; private set; }

		internal Dictionary<int, Entity> entities = new Dictionary<int, Entity>(); // Key = entityId
		internal Dictionary<int, Team> teams = new Dictionary<int, Team>(3); // Key = teamNum
		internal Dictionary<int, Player> players = new Dictionary<int, Player>(16); // Key = userId

		public IReadOnlyList<Team> Teams { get { return teams.Values.ToList(); } }
		public IReadOnlyList<Player> Players { get { return players.Values.ToList(); } }

		public int CurrentTick { get; private set; }
		public double CurrentTime { get { return (Header != null) ? (this.CurrentTick * Header.TickTime) : 0; } }


		public DemoParser(Stream input)
		{
			reader = new BinaryReader(input);

			DemoPacketsParser = new DemoPacketParser();
			DataTablesParser = new DataTableParser();
			StringTablesParser = new StringTableParser();

			EventsManager = new EventsManager();
		}

		public void ParseDemo(bool fullParse)
		{
			ParseHeader();

			if (fullParse)
			{
				while (ParseNextTick()) { }
			}
		}

		private void ParseHeader()
		{
			var header = DemoHeader.ParseFrom(reader);

			if (header.Filestamp != DEMO_HEADER_ID)
				throw new NotSupportedException("Invalid file type - expecting " + DEMO_HEADER_ID);

			if (header.DemoProtocol != DEMO_PROTOCOL)
				throw new NotSupportedException("Invalid demo protocol - expecting " + DEMO_PROTOCOL);

			Header = header;

			this.EventsManager.RaiseHeaderParsed(new HeaderParsedEventArgs(header));
		}

		public bool ParseNextTick()
		{
			DemoMessageType messageType = (DemoMessageType)reader.ReadByte();

			this.CurrentTick = reader.ReadInt32();
			reader.ReadByte(); // player slot

			switch (messageType)
			{
				case DemoMessageType.Synctick:
					break;
				case DemoMessageType.Stop:
					return false;
				case DemoMessageType.ConsoleCommand:
					using (var volvo = reader.ReadVolvoPacket())
					break;
				case DemoMessageType.DataTables:
					using (var volvo = reader.ReadVolvoPacket())
						DataTablesParser.ParsePacket(volvo);
					break;
				case DemoMessageType.StringTables:
					using (var volvo = reader.ReadVolvoPacket())
						StringTablesParser.ParsePacket(volvo, this);
					break;
				case DemoMessageType.UserCommand:
					reader.ReadInt32();
					using (var volvo = reader.ReadVolvoPacket())
					break;
				case DemoMessageType.Signon:
				case DemoMessageType.Packet:
					CommandInfo.Parse(reader);
					reader.ReadInt32(); // SeqNrIn
					reader.ReadInt32(); // SeqNrOut

					using (var volvo = reader.ReadVolvoPacket())
						DemoPacketsParser.ParsePacket(volvo, this);
					break;
				default:
					throw new Exception("Can't handle MessageType " + messageType);
			}

			this.EventsManager.RaiseTickDone();

			return true;
		}
	}
}
