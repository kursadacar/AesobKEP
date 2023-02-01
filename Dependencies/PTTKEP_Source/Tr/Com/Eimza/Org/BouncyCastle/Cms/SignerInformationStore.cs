using System.Collections;
using System.Collections.Generic;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class SignerInformationStore
	{
		private readonly IList all;

		private readonly IDictionary table = Platform.CreateHashtable();

		public int Count
		{
			get
			{
				return all.Count;
			}
		}

		public SignerInformationStore(ICollection signerInfos)
		{
			foreach (SignerInformation signerInfo in signerInfos)
			{
				SignerID signerID = signerInfo.SignerID;
				IList list = (IList)table[signerID];
				if (list == null)
				{
					list = (IList)(table[signerID] = Platform.CreateArrayList(1));
				}
				list.Add(signerInfo);
			}
			all = Platform.CreateArrayList(signerInfos);
		}

		public SignerInformation GetFirstSigner(SignerID selector)
		{
			IList list = (IList)table[selector];
			if (list != null)
			{
				return (SignerInformation)list[0];
			}
			return null;
		}

		public ICollection GetSigners()
		{
			return Platform.CreateArrayList(all);
		}

		public List<SignerInformation> GetSignerList()
		{
			List<SignerInformation> list = new List<SignerInformation>();
			for (int i = 0; i < all.Count; i++)
			{
				SignerInformation item = all[i] as SignerInformation;
				list.Add(item);
			}
			return list;
		}

		public ICollection GetSigners(SignerID selector)
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
