using System.Net.Mail;

namespace Tr.Com.Eimza.Smime
{
	internal class SMailAddress
	{
		public string Address
		{
			get
			{
				return InternalMailAddress.Address;
			}
		}

		public string DisplayName
		{
			get
			{
				return InternalMailAddress.DisplayName;
			}
		}

		public string Host
		{
			get
			{
				return InternalMailAddress.Host;
			}
		}

		public string User
		{
			get
			{
				return InternalMailAddress.User;
			}
		}

		internal MailAddress InternalMailAddress { get; private set; }

		public SMailAddress(string address)
		{
			InternalMailAddress = new MailAddress(address);
		}

		public SMailAddress(string address, string displayName)
		{
			InternalMailAddress = new MailAddress(address, displayName);
		}
	}
}
