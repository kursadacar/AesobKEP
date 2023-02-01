using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class TcpWrittenEventArgs : EventArgs
	{
		private string _data;

		public string Command
		{
			get
			{
				return _data;
			}
		}

		public TcpWrittenEventArgs(string data)
		{
			_data = data;
		}
	}
}
