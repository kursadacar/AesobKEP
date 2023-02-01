using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class Server
	{
		private string _host;

		private string _username;

		private string _password;

		private int _port;

		private bool _requiresAuthentication;

		private EncryptionType _encType;

		public string Username
		{
			get
			{
				return _username;
			}
			set
			{
				_username = value;
			}
		}

		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
			}
		}

		public string Host
		{
			get
			{
				return _host;
			}
			set
			{
				_host = value;
			}
		}

		public int Port
		{
			get
			{
				return _port;
			}
			set
			{
				_port = value;
			}
		}

		public bool RequiresAuthentication
		{
			get
			{
				return _requiresAuthentication;
			}
			set
			{
				_requiresAuthentication = value;
			}
		}

		public EncryptionType ServerEncryptionType
		{
			get
			{
				return _encType;
			}
			set
			{
				_encType = value;
			}
		}

		public Server()
		{
			Host = "127.0.0.1";
			Port = 0;
			Username = string.Empty;
			Password = string.Empty;
		}

		public Server(string host, int port)
		{
			Host = host;
			Port = port;
			Username = string.Empty;
			Password = string.Empty;
		}

		public Server(string host, int port, string username, string password)
		{
			Host = host;
			Port = port;
			Username = username;
			Password = password;
		}

		public Server(string host, int port, string username, string password, bool RequiresAuthentication, EncryptionType EncType)
		{
			Host = host;
			Port = port;
			Username = username;
			Password = password;
			_requiresAuthentication = RequiresAuthentication;
			_encType = EncType;
		}
	}
}
