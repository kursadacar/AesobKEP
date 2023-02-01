using System;
using System.Collections.ObjectModel;
using System.Net.Mail;

namespace Tr.Com.Eimza.Smime
{
	internal class SMailAddressCollection : Collection<SMailAddress>
	{
		public void Add(string addresses)
		{
			foreach (MailAddress item in new MailAddressCollection { addresses })
			{
				Add(new SMailAddress(item.Address));
			}
		}

		protected override void InsertItem(int index, SMailAddress item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.InsertItem(index, item);
		}

		protected override void SetItem(int index, SMailAddress item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.SetItem(index, item);
		}
	}
}
