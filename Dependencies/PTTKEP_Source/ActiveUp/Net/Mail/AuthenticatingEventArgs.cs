using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class AuthenticatingEventArgs : EventArgs
	{
		private string _username;

		private string _password;

		private string _host;

		public string Username
		{
			get
			{
				return _username;
			}
		}

		public string Password
		{
			get
			{
				return _password;
			}
		}

		public string Host
		{
			get
			{
				return _host;
			}
		}

		public AuthenticatingEventArgs(string username, string password, string host)
		{
			_username = username;
			_password = password;
			_host = host;
		}

		public AuthenticatingEventArgs(string username, string password)
		{
			_username = username;
			_password = password;
			_host = "unkwown";
		}
	}
}
