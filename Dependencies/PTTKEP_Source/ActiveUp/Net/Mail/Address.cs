using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class Address
	{
		private string _email;

		private string _name;

		private string _charset;

		public string Email
		{
			get
			{
				return _email;
			}
			set
			{
				_email = value.Trim();
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value.Trim();
			}
		}

		public string Merged
		{
			get
			{
				string empty = string.Empty;
				string empty2 = string.Empty;
				empty2 = ((!string.IsNullOrEmpty(Charset)) ? Codec.RFC2047Encode(Name, Charset) : Name);
				if (Name.Length > 0)
				{
					empty = empty + "\"" + empty2 + "\" ";
					return empty + "<" + Email + ">";
				}
				return empty + Email;
			}
		}

		public string Link
		{
			get
			{
				string empty = string.Empty;
				if (Name.Length > 0)
				{
					empty = empty + "<a href=\"mailto:" + Email + "\">";
					return empty + Name + "</a>";
				}
				empty = empty + "<a href=\"mailto:" + Email + "\">";
				return empty + Email + "</a>";
			}
		}

		public string Charset
		{
			get
			{
				return _charset;
			}
			set
			{
				_charset = value;
			}
		}

		public Address()
		{
			Email = string.Empty;
			Name = string.Empty;
		}

		public Address(string email)
		{
			Address address = Parser.ParseAddress(email);
			Email = address.Email;
			Name = address.Name;
		}

		public Address(string email, string name)
		{
			Email = email;
			Name = name;
		}

		public Address(string email, string name, string charset)
		{
			Email = email;
			Name = name;
			Charset = charset;
		}

		public override string ToString()
		{
			return Merged;
		}
	}
}
