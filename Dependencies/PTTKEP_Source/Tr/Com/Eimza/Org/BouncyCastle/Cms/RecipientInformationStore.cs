using System.Collections;
using System.Collections.Generic;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class RecipientInformationStore
	{
		private readonly IList all;

		private readonly IDictionary table = Platform.CreateHashtable();

		public RecipientInformation this[RecipientID selector]
		{
			get
			{
				return GetFirstRecipient(selector);
			}
		}

		public int Count
		{
			get
			{
				return all.Count;
			}
		}

		public RecipientInformationStore(ICollection recipientInfos)
		{
			foreach (RecipientInformation recipientInfo in recipientInfos)
			{
				RecipientID recipientID = recipientInfo.RecipientID;
				IList list = (IList)table[recipientID];
				if (list == null)
				{
					list = (IList)(table[recipientID] = Platform.CreateArrayList(1));
				}
				list.Add(recipientInfo);
			}
			all = Platform.CreateArrayList(recipientInfos);
		}

		public RecipientInformation GetFirstRecipient(RecipientID selector)
		{
			IList list = (IList)table[selector];
			if (list != null)
			{
				return (RecipientInformation)list[0];
			}
			return null;
		}

		public ICollection GetRecipients()
		{
			return Platform.CreateArrayList(all);
		}

		public List<RecipientInformation> GetRecipientList()
		{
			List<RecipientInformation> list = new List<RecipientInformation>();
			for (int i = 0; i < all.Count; i++)
			{
				RecipientInformation item = all[i] as RecipientInformation;
				list.Add(item);
			}
			return list;
		}

		public ICollection GetRecipients(RecipientID selector)
		{
			IList list = (IList)table[selector];
			if (list != null)
			{
				return Platform.CreateArrayList(list);
			}
			return Platform.CreateArrayList();
		}
	}
}
