using System;
using System.Collections.ObjectModel;

namespace Tr.Com.Eimza.Smime
{
	internal class SMailAttachmentCollection : Collection<SMailAttachment>
	{
		internal SMailAttachmentCollection()
		{
		}

		protected override void InsertItem(int index, SMailAttachment item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.InsertItem(index, item);
		}

		protected override void SetItem(int index, SMailAttachment item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.SetItem(index, item);
		}
	}
}
