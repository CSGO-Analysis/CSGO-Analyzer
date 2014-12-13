using DemoParser_Core.Messages;
using DemoParser_Core.StringTables;

namespace DemoParser_Core.Packets.Handlers
{
    class UpdateStringTableUserInfoHandler : IMessageParser
    {
		public bool TryApplyMessage(ProtoBuf.IExtensible message, DemoParser parser)
        {
			var stringTableUpdate = message as CSVCMsg_UpdateStringTable;
			if ((stringTableUpdate == null) || (parser.StringTablesParser.stringTables[stringTableUpdate.table_id].name != "userinfo"))
				return false;

			CSVCMsg_CreateStringTable stringTableCreate = parser.StringTablesParser.stringTables[stringTableUpdate.table_id];
            stringTableCreate.num_entries = stringTableUpdate.num_changed_entries;
            stringTableCreate.string_data = stringTableUpdate.string_data;

			parser.StringTablesParser.ParseStringTableMessage(stringTableCreate, parser);

			return true;
        }

		public int Priority { get { return 0; } }
    }
}
