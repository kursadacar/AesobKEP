using System;
using System.Collections.Specialized;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class UsenetXrefList
	{
		private string _host;

		private NameValueCollection _groups = new NameValueCollection();

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

		public NameValueCollection Groups
		{
			get
			{
				return _groups;
			}
			set
			{
				_groups = value;
			}
		}
	}
}
