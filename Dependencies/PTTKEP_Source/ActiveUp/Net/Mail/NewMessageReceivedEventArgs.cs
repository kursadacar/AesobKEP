using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class NewMessageReceivedEventArgs : EventArgs
	{
		private int _messageCount;

		public int MessageCount
		{
			get
			{
				return _messageCount;
			}
		}

		public NewMessageReceivedEventArgs(int messageCount)
		{
			_messageCount = messageCount;
		}
	}
}
