namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ocsp
{
	internal abstract class OcspObjectIdentifiers
	{
		internal const string PkixOcspId = "1.3.6.1.5.5.7.48.1";

		public static readonly DerObjectIdentifier PkixOcsp = new DerObjectIdentifier("1.3.6.1.5.5.7.48.1");

		public static readonly DerObjectIdentifier PkixOcspBasic = new DerObjectIdentifier("1.3.6.1.5.5.7.48.1.1");

		public static readonly DerObjectIdentifier PkixOcspNonce;

		public static readonly DerObjectIdentifier PkixOcspCrl;

		public static readonly DerObjectIdentifier PkixOcspResponse;

		public static readonly DerObjectIdentifier PkixOcspNocheck;

		public static readonly DerObjectIdentifier PkixOcspArchiveCutoff;

		public static readonly DerObjectIdentifier PkixOcspServiceLocator;

		static OcspObjectIdentifiers()
		{
			DerObjectIdentifier pkixOcsp = PkixOcsp;
			PkixOcspNonce = new DerObjectIdentifier(((pkixOcsp != null) ? pkixOcsp.ToString() : null) + ".2");
			DerObjectIdentifier pkixOcsp2 = PkixOcsp;
			PkixOcspCrl = new DerObjectIdentifier(((pkixOcsp2 != null) ? pkixOcsp2.ToString() : null) + ".3");
			DerObjectIdentifier pkixOcsp3 = PkixOcsp;
			PkixOcspResponse = new DerObjectIdentifier(((pkixOcsp3 != null) ? pkixOcsp3.ToString() : null) + ".4");
			DerObjectIdentifier pkixOcsp4 = PkixOcsp;
			PkixOcspNocheck = new DerObjectIdentifier(((pkixOcsp4 != null) ? pkixOcsp4.ToString() : null) + ".5");
			DerObjectIdentifier pkixOcsp5 = PkixOcsp;
			PkixOcspArchiveCutoff = new DerObjectIdentifier(((pkixOcsp5 != null) ? pkixOcsp5.ToString() : null) + ".6");
			DerObjectIdentifier pkixOcsp6 = PkixOcsp;
			PkixOcspServiceLocator = new DerObjectIdentifier(((pkixOcsp6 != null) ? pkixOcsp6.ToString() : null) + ".7");
		}
	}
}
