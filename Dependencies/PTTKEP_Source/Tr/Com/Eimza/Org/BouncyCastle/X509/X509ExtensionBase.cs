using System;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Collections;

namespace Tr.Com.Eimza.Org.BouncyCastle.X509
{
	internal abstract class X509ExtensionBase : IX509Extension
	{
		protected abstract X509Extensions GetX509Extensions();

		protected virtual ISet GetExtensionOids(bool critical)
		{
			X509Extensions x509Extensions = GetX509Extensions();
			if (x509Extensions != null)
			{
				HashSet hashSet = new HashSet();
				{
					foreach (DerObjectIdentifier extensionOid in x509Extensions.ExtensionOids)
					{
						if (x509Extensions.GetExtension(extensionOid).IsCritical == critical)
						{
							hashSet.Add(extensionOid.Id);
						}
					}
					return hashSet;
				}
			}
			return null;
		}

		public virtual ISet GetNonCriticalExtensionOids()
		{
			return GetExtensionOids(false);
		}

		public virtual ISet GetCriticalExtensionOids()
		{
			return GetExtensionOids(true);
		}

		[Obsolete("Use version taking a DerObjectIdentifier instead")]
		public Asn1OctetString GetExtensionValue(string oid)
		{
			return GetExtensionValue(new DerObjectIdentifier(oid));
		}

		public virtual Asn1OctetString GetExtensionValue(DerObjectIdentifier oid)
		{
			X509Extensions x509Extensions = GetX509Extensions();
			if (x509Extensions != null)
			{
				X509Extension extension = x509Extensions.GetExtension(oid);
				if (extension != null)
				{
					return extension.Value;
				}
			}
			return null;
		}

		public virtual X509Extension GetExtension(DerObjectIdentifier oid)
		{
			X509Extensions x509Extensions = GetX509Extensions();
			if (x509Extensions != null)
			{
				return x509Extensions.GetExtension(oid);
			}
			return null;
		}

		public virtual byte[] GetExtensionOctets(DerObjectIdentifier oid)
		{
			X509Extensions x509Extensions = GetX509Extensions();
			if (x509Extensions != null)
			{
				X509Extension extension = x509Extensions.GetExtension(oid);
				if (extension != null)
				{
					return extension.Value.GetOctets();
				}
			}
			return null;
		}
	}
}
