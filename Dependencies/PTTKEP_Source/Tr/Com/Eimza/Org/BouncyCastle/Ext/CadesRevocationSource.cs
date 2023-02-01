using System.Collections;
using System.Collections.Generic;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Esf;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs;
using Tr.Com.Eimza.Org.BouncyCastle.Cms;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ext
{
	internal class CadesRevocationSource
	{
		private readonly CmsSignedData cmsSignedData;

		private readonly SignerInformation signerInformation;

		public CadesRevocationSource(byte[] encodedCMS)
			: this(new CmsSignedData(encodedCMS))
		{
		}

		public CadesRevocationSource(CmsSignedData cmsSignedData)
		{
			IEnumerator enumerator = cmsSignedData.GetSignerInfos().GetSigners().GetEnumerator();
			enumerator.MoveNext();
			this.cmsSignedData = cmsSignedData;
			signerInformation = (SignerInformation)enumerator.Current;
		}

		public CadesRevocationSource(CmsSignedData cmsSignedData, SignerInformation signerInformation)
		{
			this.cmsSignedData = cmsSignedData;
			this.signerInformation = signerInformation;
		}

		public List<RevocationValues> GetRevocationValues()
		{
			List<RevocationValues> list = new List<RevocationValues>();
			if (signerInformation != null && signerInformation.UnsignedAttributes != null && signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationValues] != null)
			{
				RevocationValues instance = RevocationValues.GetInstance(signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationValues].AttrValues[0]);
				list.Add(instance);
			}
			return list;
		}
	}
}
