using DemoParser_Core.Messages;
using DemoParser_Core.StringTables;
using ProtoBuf;

namespace DemoParser_Core.Packets.Handlers
{
	/// <summary>
	/// Used to parse userinfo data : create PlayerInfo and add them to demoParser.RawPlayers
	/// </summary>
    class CreateStringTableUserInfoHandler : IMessageParser
    {
        public bool TryApplyMessage(IExtensible message, DemoParser parser)
        {
			var create = message as CSVCMsg_CreateStringTable;

			if (create != null)
				parser.stringTables.Add((Messages.CSVCMsg_CreateStringTable)message);

			if ((create == null) || (create.name != "userinfo"))
				return false;

            StringTableParser.ParseStringTableUpdate(create, parser);

			return true;
        }

		public int Priority { get { return 0; } }
    }
}
