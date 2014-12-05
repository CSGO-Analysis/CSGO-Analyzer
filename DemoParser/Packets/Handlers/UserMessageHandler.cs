using DemoParser_Core.Messages;

namespace DemoParser_Core.Packets.Handlers
{
	public class UserMessageHandler : IMessageParser
	{
		public bool TryApplyMessage (ProtoBuf.IExtensible message, DemoParser parser)
		{
			CSVCMsg_UserMessage userMessage = message as CSVCMsg_UserMessage;
			if (userMessage == null)
				return false;

			var messageType = (Messages.ECstrike15UserMessages)userMessage.msg_type;

			return true;
		}

		public int Priority { get { return 0; } }
	}
}

