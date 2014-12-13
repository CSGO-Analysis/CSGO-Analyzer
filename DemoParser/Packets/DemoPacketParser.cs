using DemoParser_Core.Messages;
using DemoParser_Core.Packets.Handlers;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DemoParser_Core.Packets
{
	public class DemoPacketParser
    {
		private static readonly Dictionary<string, Type> MessagesCSVC = (
			from type in Assembly.GetExecutingAssembly().GetTypes()
			where type.FullName.Contains("Messages.CSVCMsg_")
			select type).ToDictionary(type => type.FullName);

		private static readonly Dictionary<string, Type> MessagesCNET = (
			from type in Assembly.GetExecutingAssembly().GetTypes()
			where type.FullName.Contains("Messages.CNETMsg_")
			select type).ToDictionary(type => type.FullName);

		// TODO Thread-safe
		private static GameEventHandler gameEventHandler = new GameEventHandler();

		private static readonly Dictionary<Type, IMessageParser> ParseType = new Dictionary<Type, IMessageParser> {
			//{ typeof(CSVCMsg_CreateStringTable), new GenericCreateStringTablesHandler() },
			{ typeof(CSVCMsg_CreateStringTable), new CreateStringTableUserInfoHandler() },
			{ typeof(CSVCMsg_UpdateStringTable), new UpdateStringTableUserInfoHandler() },
			{ typeof(CSVCMsg_GameEventList), gameEventHandler },
			{ typeof(CSVCMsg_GameEvent), gameEventHandler },
			//{ typeof(CSVCMsg_UserMessage), new UserMessageHandler() },
			{ typeof(CSVCMsg_PacketEntities), new PacketEntitiesHandler() }
		};

		/// <summary>
		/// Read protobuf messages
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="parser"></param>
		public void ParsePacket(Stream stream, DemoParser parser)
        {
			var reader = new BinaryReader(stream);

			while (stream.Position < stream.Length)
            {
                int cmd = reader.ReadVarInt32();

                Type toParse = null;

                if (Enum.IsDefined(typeof(SVC_Messages), cmd))
                {
                    SVC_Messages msg = (SVC_Messages)cmd;
					toParse = MessagesCSVC["DemoParser_Core.Messages.CSVCMsg_" + msg.ToString().Substring(4)];
                }
                else if (Enum.IsDefined(typeof(NET_Messages), cmd))
                {
                    NET_Messages msg = (NET_Messages)cmd;
					toParse = MessagesCNET["DemoParser_Core.Messages.CNETMsg_" + msg.ToString().Substring(4)];
                }

                if (toParse == null)  
                {
                    reader.ReadBytes(reader.ReadVarInt32());
                    continue;
                }

                IExtensible message = reader.ReadProtobufMessage(toParse, ProtoBuf.PrefixStyle.Base128);

				//This method apply message only to Handler able to deal with that type of message
				if (ParseType.ContainsKey(toParse))
				{
					ParseType[toParse].TryApplyMessage(message, parser);
				}
			}
        }
    }
}
