using System;
using System.Collections.ObjectModel;
using System.Net.Mail;

namespace Tr.Com.Eimza.SecureMail
{
	internal class SecureMailAddressCollection : Collection<SecureMailAddress>
	{
		public void Add(string addresses)
		{
			foreach (MailAddress item in new MailAddressCollection { addresses })
			{
				Add(new SecureMailAddress(item.Address));
			}
		}

		protected override void InsertItem(int index, SecureMailAddress item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.InsertItem(index, item);
		}

		protected override void SetItem(int index, SecureMailAddress item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.SetItem(index, item);
		}
	}
}
