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
	public static class DemoPacketParser
    {
		private static readonly IEnumerable<IMessageParser> Parsers = (
			from type in Assembly.GetExecutingAssembly().GetTypes()
			where type.GetInterfaces().Contains(typeof(IMessageParser))
			let parser = (IMessageParser)type.GetConstructor(new Type[0]).Invoke(null)
			orderby -parser.Priority
			select parser).ToArray();

		private static readonly Dictionary<string, Type> MessagesCSVC = (
			from type in Assembly.GetExecutingAssembly().GetTypes()
			where type.FullName.Contains("Messages.CSVCMsg_")
			select type).ToDictionary(type => type.FullName);

		private static readonly Dictionary<string, Type> MessagesCNET = (
			from type in Assembly.GetExecutingAssembly().GetTypes()
			where type.FullName.Contains("Messages.CNETMsg_")
			select type).ToDictionary(type => type.FullName);

		private static readonly Dictionary<Type, IMessageParser> ParseType = new Dictionary<Type, IMessageParser> {
			//{ typeof(CSVCMsg_CreateStringTable), new GenericCreateStringTablesHandler() },
			{ typeof(CSVCMsg_CreateStringTable), new CreateStringTableUserInfoHandler() },
			{ typeof(CSVCMsg_UpdateStringTable), new UpdateStringTableUserInfoHandler() },
			{ typeof(CSVCMsg_GameEventList), new GameEventHandler() },
			{ typeof(CSVCMsg_GameEvent), new GameEventHandler() },
			//{ typeof(CSVCMsg_UserMessage), new UserMessageHandler() },
			{ typeof(CSVCMsg_PacketEntities), new PacketEntitiesHandler() }
		};

		/// <summary>
		/// Read protobuf messages
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="demo"></param>
		public static void ParsePacket(Stream stream, DemoParser demo)
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
					ParseType[toParse].TryApplyMessage(message, demo);
				}

				//This method give packet to all parsers (without determining if its a packet usable by the parser)
                /*foreach (IMessageParser parser in Parsers)
					if (parser.TryApplyMessage(message, demo) && (parser.Priority > 0))
						break;
				*/
			}
        }
    }
}
