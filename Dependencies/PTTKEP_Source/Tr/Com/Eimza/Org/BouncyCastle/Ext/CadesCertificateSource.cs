using System.Collections;
using System.Collections.Generic;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Security.Certificates;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ext
{
	internal class CadesCertificateSource
	{
		private readonly CmsSignedData cmsSignedData;

		private readonly SignerInformation signerInformation;

		private readonly bool onlyExtended = true;

		public CadesCertificateSource(CmsSignedData cms)
		{
			IEnumerator enumerator = cms.GetSignerInfos().GetSigners().GetEnumerator();
			enumerator.MoveNext();
			cmsSignedData = cms;
			signerInformation = (SignerInformation)enumerator.Current;
			onlyExtended = false;
		}

		public CadesCertificateSource(CmsSignedData cms, SignerInformation signerInformation, bool onlyExtended)
		{
			cmsSignedData = cms;
			this.signerInformation = signerInformation;
			this.onlyExtended = onlyExtended;
		}

		public List<X509Certificate> GetCertificates()
		{
			List<X509Certificate> list = new List<X509Certificate>();
			try
			{
				if (!onlyExtended)
				{
					foreach (X509Certificate match in cmsSignedData.GetCertificates("Collection").GetMatches(null))
					{
						if (!list.Contains(match))
						{
							list.Add(match);
						}
					}
				}
				if (signerInformation != null)
				{
					if (signerInformation.UnsignedAttributes != null)
					{
						if (signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsCertValues] != null)
						{
							DerSequence derSequence = (DerSequence)signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsCertValues].AttrValues[0];
							for (int i = 0; i < derSequence.Count; i++)
							{
								X509Certificate item2 = new X509Certificate(X509CertificateStructure.GetInstance(derSequence[i]));
								if (!list.Contains(item2))
								{
									list.Add(item2);
								}
							}
							return list;
						}
						return list;
					}
					return list;
				}
				return list;
			}
			catch (CertificateParsingException e)
			{
				throw new CmsException(e);
			}
		}
	}
}
