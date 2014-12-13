﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoParser_Core.Packets.Handlers
{
    class GenericCreateStringTablesHandler : IMessageParser
    {
        public bool TryApplyMessage(ProtoBuf.IExtensible message, DemoParser parser)
        {
			if (message is Messages.CSVCMsg_CreateStringTable) {
				parser.StringTablesParser.stringTables.Add((Messages.CSVCMsg_CreateStringTable)message);
				return true;
			}

			return false;
        }

		public int Priority { get { return 0; } }
    }
}
