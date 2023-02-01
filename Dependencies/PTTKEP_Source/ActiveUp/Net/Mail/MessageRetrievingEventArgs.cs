using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class MessageRetrievingEventArgs : EventArgs
	{
		private int _index;

		private int _totalCount;

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

		public MessageRetrievingEventArgs(int index, int totalCount)
		{
			_index = index;
			_totalCount = totalCount;
		}

		public MessageRetrievingEventArgs(int index)
		{
			_index = index;
			_totalCount = -1;
		}
	}
}
