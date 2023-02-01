using System;
using System.Collections.ObjectModel;

namespace Tr.Com.Eimza.SecureMail
{
	internal class SecureAttachmentCollection : Collection<SecureAttachment>
	{
		internal SecureAttachmentCollection()
		{
		}

		protected override void InsertItem(int index, SecureAttachment item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.InsertItem(index, item);
		}

		protected override void SetItem(int index, SecureAttachment item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.SetItem(index, item);
		}
	}
}
