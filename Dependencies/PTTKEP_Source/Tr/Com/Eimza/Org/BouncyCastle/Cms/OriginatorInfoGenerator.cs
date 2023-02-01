using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.X509;
using Tr.Com.Eimza.Org.BouncyCastle.X509.Store;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class OriginatorInfoGenerator
	{
		private readonly IList origCerts;

		private readonly IList origCrls;

		public OriginatorInfoGenerator(X509Certificate origCert)
		{
			origCerts = Platform.CreateArrayList(1);
			origCrls = null;
			origCerts.Add(origCert.CertificateStructure);
		}

		public OriginatorInfoGenerator(IX509Store origCerts, IX509Store origCrls = null)
		{
			this.origCerts = CmsUtilities.GetCertificatesFromStore(origCerts);
			this.origCrls = ((origCrls == null) ? null : CmsUtilities.GetCrlsFromStore(origCrls));
		}

		public virtual OriginatorInfo Generate()
		{
			Asn1Set certs = CmsUtilities.CreateDerSetFromList(origCerts);
			Asn1Set crls = ((origCrls == null) ? null : CmsUtilities.CreateDerSetFromList(origCrls));
			return new OriginatorInfo(certs, crls);
		}
	}
}
