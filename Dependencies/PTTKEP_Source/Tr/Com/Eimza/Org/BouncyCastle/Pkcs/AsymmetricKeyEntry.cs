using System;
using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Pkcs
{
	internal class AsymmetricKeyEntry : Pkcs12Entry
	{
		private readonly AsymmetricKeyParameter key;

		public AsymmetricKeyParameter Key
		{
			get
			{
				return key;
			}
		}

		public AsymmetricKeyEntry(AsymmetricKeyParameter key)
			: base(Platform.CreateHashtable())
		{
			this.key = key;
		}

		[Obsolete]
		public AsymmetricKeyEntry(AsymmetricKeyParameter key, Hashtable attributes)
			: base(attributes)
		{
			this.key = key;
		}

		public AsymmetricKeyEntry(AsymmetricKeyParameter key, IDictionary attributes)
			: base(attributes)
		{
			this.key = key;
		}

		public override bool Equals(object obj)
		{
			AsymmetricKeyEntry asymmetricKeyEntry = obj as AsymmetricKeyEntry;
			if (asymmetricKeyEntry == null)
			{
				return false;
			}
			return key.Equals(asymmetricKeyEntry.key);
		}

		public override int GetHashCode()
		{
			return ~key.GetHashCode();
		}
	}
}
