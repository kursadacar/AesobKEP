using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class TcpReadEventArgs : EventArgs
	{
		private string _data;

		public string Response
		{
			get
			{
				return _data;
			}
		}

		public TcpReadEventArgs(string data)
		{
			_data = data;
		}
	}
}
