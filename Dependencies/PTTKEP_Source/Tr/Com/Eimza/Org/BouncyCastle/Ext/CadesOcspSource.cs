using System;
using System.Collections;
using System.Collections.Generic;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Esf;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ocsp;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs;
using Tr.Com.Eimza.Org.BouncyCastle.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Ocsp;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ext
{
	internal class CadesOcspSource
	{
		private readonly CmsSignedData cmsSignedData;

		private readonly SignerInformation signerInformation;

		public CadesOcspSource(byte[] encodedCMS)
			: this(new CmsSignedData(encodedCMS))
		{
		}

		public CadesOcspSource(CmsSignedData cmsSignedData)
		{
			IEnumerator enumerator = cmsSignedData.GetSignerInfos().GetSigners().GetEnumerator();
			enumerator.MoveNext();
			this.cmsSignedData = cmsSignedData;
			signerInformation = (SignerInformation)enumerator.Current;
		}

		public CadesOcspSource(CmsSignedData cmsSignedData, SignerInformation signerInformation)
		{
			this.cmsSignedData = cmsSignedData;
			this.signerInformation = signerInformation;
		}

		public List<OcspResponse> GetOCSPResponsesFromSignature()
		{
			List<OcspResponse> list = new List<OcspResponse>();
			if (signerInformation != null && signerInformation.UnsignedAttributes != null && signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationValues] != null)
			{
				RevocationValues instance = RevocationValues.GetInstance(signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationValues].AttrValues[0]);
				if (instance.GetOcspVals() != null)
				{
					BasicOcspResponse[] ocspVals = instance.GetOcspVals();
					for (int i = 0; i < ocspVals.Length; i++)
					{
						BasicOcspResp basicOcspResp = new BasicOcspResp(ocspVals[i]);
						list.Add(basicOcspResp.ToOcspResponse());
					}
				}
			}
			return list;
		}
	}
}
