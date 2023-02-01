using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class AuthenticatedEventArgs : EventArgs
	{
		private string _username;

		private string _password;

		private string _host;

		private string _serverResponse;

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

		public string ServerResponse
		{
			get
			{
				return _serverResponse;
			}
		}

		public AuthenticatedEventArgs(string username, string password, string host, string serverResponse)
		{
			_username = username;
			_password = password;
			_host = host;
			_serverResponse = serverResponse;
		}

		public AuthenticatedEventArgs(string username, string password, string serverResponse)
		{
			_username = username;
			_password = password;
			_host = "unknown";
			_serverResponse = serverResponse;
		}
	}
}
