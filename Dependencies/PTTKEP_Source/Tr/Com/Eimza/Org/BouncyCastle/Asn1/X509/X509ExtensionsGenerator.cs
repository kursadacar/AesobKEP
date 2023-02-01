using System;
using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509
{
	internal class X509ExtensionsGenerator
	{
		private IDictionary extensions = Platform.CreateHashtable();

		private IList extOrdering = Platform.CreateArrayList();

		public bool IsEmpty
		{
			get
			{
				return extOrdering.Count < 1;
			}
		}

		public void Reset()
		{
			extensions = Platform.CreateHashtable();
			extOrdering = Platform.CreateArrayList();
		}

		public void AddExtension(DerObjectIdentifier oid, bool critical, Asn1Encodable extValue)
		{
			byte[] derEncoded;
			try
			{
				derEncoded = extValue.GetDerEncoded();
			}
			catch (Exception ex)
			{
				throw new ArgumentException("error encoding value: " + ((ex != null) ? ex.ToString() : null));
			}
			AddExtension(oid, critical, derEncoded);
		}

		public void AddExtension(DerObjectIdentifier oid, bool critical, byte[] extValue)
		{
			if (extensions.Contains(oid))
			{
				throw new ArgumentException("extension " + ((oid != null) ? oid.ToString() : null) + " already added");
			}
			extOrdering.Add(oid);
			extensions.Add(oid, new X509Extension(critical, new DerOctetString(extValue)));
		}

		public X509Extensions Generate()
		{
			return new X509Extensions(extOrdering, extensions);
		}
	}
}
