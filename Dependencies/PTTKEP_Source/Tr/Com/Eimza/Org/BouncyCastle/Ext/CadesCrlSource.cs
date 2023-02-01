using System.Collections;
using System.Collections.Generic;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Esf;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Security.Certificates;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ext
{
	internal class CadesCrlSource
	{
		private readonly CmsSignedData cmsSignedData;

		private readonly SignerInformation signerInformation;

		public CadesCrlSource(byte[] encodedCMS)
			: this(new CmsSignedData(encodedCMS))
		{
		}

		public CadesCrlSource(CmsSignedData cmsSignedData)
		{
			IEnumerator enumerator = cmsSignedData.GetSignerInfos().GetSigners().GetEnumerator();
			enumerator.MoveNext();
			this.cmsSignedData = cmsSignedData;
			signerInformation = (SignerInformation)enumerator.Current;
		}

		public CadesCrlSource(CmsSignedData cmsSignedData, SignerInformation signerInformation)
		{
			this.cmsSignedData = cmsSignedData;
			this.signerInformation = signerInformation;
		}

		public List<X509Crl> GetCRLsFromSignature()
		{
			List<X509Crl> list = new List<X509Crl>();
			try
			{
				foreach (X509Crl match in cmsSignedData.GetCrls("Collection").GetMatches(null))
				{
					list.Add(match);
				}
				if (signerInformation != null)
				{
					if (signerInformation.UnsignedAttributes != null)
					{
						if (signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationValues] != null)
						{
							RevocationValues instance = RevocationValues.GetInstance(signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationValues].AttrValues[0]);
							if (instance.GetCrlVals() != null)
							{
								CertificateList[] crlVals = instance.GetCrlVals();
								for (int i = 0; i < crlVals.Length; i++)
								{
									X509Crl item2 = new X509Crl(crlVals[i]);
									list.Add(item2);
								}
								return list;
							}
							return list;
						}
						return list;
					}
					return list;
				}
				return list;
			}
			catch (CrlException e)
			{
				throw new CmsException(e);
			}
		}
	}
}
