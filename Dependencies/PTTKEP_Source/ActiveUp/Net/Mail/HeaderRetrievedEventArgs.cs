using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class HeaderRetrievedEventArgs : EventArgs
	{
		private byte[] _data;

		private int _index;

		private int _totalCount = -1;

		public Header Header
		{
			get
			{
				return Parser.ParseHeader(_data);
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

		public HeaderRetrievedEventArgs(byte[] data, int index, int totalCount)
		{
			_index = index;
			_totalCount = totalCount;
			_data = data;
		}

		public HeaderRetrievedEventArgs(byte[] data, int index)
		{
			_index = index;
			_data = data;
			_totalCount = -1;
		}
	}
}
