using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class MessageSendingEventArgs : EventArgs
	{
		private Message _message;

		public Message Message
		{
			get
			{
				return _message;
			}
		}

		public MessageSendingEventArgs(Message message)
		{
			_message = message;
		}
	}
}
