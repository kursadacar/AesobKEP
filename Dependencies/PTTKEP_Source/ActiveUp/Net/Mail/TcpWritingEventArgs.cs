using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class TcpWritingEventArgs : EventArgs
	{
		private string _data;

		public string Command
		{
			get
			{
				return _data;
			}
		}

		public TcpWritingEventArgs(string data)
		{
			_data = data;
		}
	}
}
