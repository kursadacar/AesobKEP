using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class HeaderRetrievingEventArgs : EventArgs
	{
		private int _index;

		private int _totalCount = -1;

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

		public HeaderRetrievingEventArgs(int index, int totalCount)
		{
			_index = index;
			_totalCount = totalCount;
		}

		public HeaderRetrievingEventArgs(int index)
		{
			_index = index;
		}
	}
}
