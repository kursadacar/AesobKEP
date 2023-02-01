using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class MessageSentEventArgs : EventArgs
	{
		private Message _message;

		public Message Message
		{
			get
			{
				return _message;
			}
		}

		public MessageSentEventArgs(Message message)
		{
			_message = message;
		}
	}
}
