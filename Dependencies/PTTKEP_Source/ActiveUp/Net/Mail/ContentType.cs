using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class ContentType : StructuredHeaderField
	{
		private string _mimeType = "text/plain";

		public string Type
		{
			get
			{
				return _mimeType.Split('/')[0];
			}
			set
			{
				_mimeType = value + "/" + SubType;
			}
		}

		public string SubType
		{
			get
			{
				return _mimeType.Split('/')[1];
			}
			set
			{
				_mimeType = Type + "/" + value;
			}
		}

		public string MimeType
		{
			get
			{
				return _mimeType;
			}
			set
			{
				_mimeType = value;
			}
		}

		public override string ToString()
		{
			string empty = string.Empty;
			empty = empty + "Content-Type: " + MimeType;
			string[] allKeys = base.Parameters.AllKeys;
			foreach (string text in allKeys)
			{
				string empty2 = string.Empty;
				empty2 = ((!text.Equals("boundary")) ? base.Parameters[text] : ("\"" + base.Parameters[text] + "\""));
				empty = empty + ";\r\n\t" + text + "=" + empty2;
			}
			return empty;
		}
	}
}
