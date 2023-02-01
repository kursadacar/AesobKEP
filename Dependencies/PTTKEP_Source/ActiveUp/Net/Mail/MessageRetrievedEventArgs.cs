using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class MessageRetrievedEventArgs : EventArgs
	{
		private byte[] _data;

		private int _index;

		private int _totalCount;

		public Message Message
		{
			get
			{
				return Parser.ParseMessage(_data);
			}
		}

		public int MessageIndex
		{
			get
			{
				return _index;
			}
		}

		public int TotalCount
		{
			get
			{
				return _totalCount;
			}
		}

		public MessageRetrievedEventArgs(byte[] data, int index, int totalCount)
		{
			_data = data;
			_index = index;
			_totalCount = totalCount;
		}

		public MessageRetrievedEventArgs(byte[] data, int index)
		{
			_data = data;
			_index = index;
			_totalCount = -1;
		}
	}
}
