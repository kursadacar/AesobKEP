namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509
{
	internal abstract class X509ObjectIdentifiers
	{
		internal const string ID = "2.5.4";

		public static readonly DerObjectIdentifier CommonName = new DerObjectIdentifier("2.5.4.3");

		public static readonly DerObjectIdentifier CountryName = new DerObjectIdentifier("2.5.4.6");

		public static readonly DerObjectIdentifier LocalityName = new DerObjectIdentifier("2.5.4.7");

		public static readonly DerObjectIdentifier StateOrProvinceName = new DerObjectIdentifier("2.5.4.8");

		public static readonly DerObjectIdentifier Organization = new DerObjectIdentifier("2.5.4.10");

		public static readonly DerObjectIdentifier OrganizationalUnitName = new DerObjectIdentifier("2.5.4.11");

		public static readonly DerObjectIdentifier id_at_telephoneNumber = new DerObjectIdentifier("2.5.4.20");

		public static readonly DerObjectIdentifier id_at_name = new DerObjectIdentifier("2.5.4.41");

		public static readonly DerObjectIdentifier IdSha1 = new DerObjectIdentifier("1.3.14.3.2.26");

		public static readonly DerObjectIdentifier IdSha256 = new DerObjectIdentifier("2.16.840.1.101.3.4.2.1");

		public static readonly DerObjectIdentifier RipeMD160 = new DerObjectIdentifier("1.3.36.3.2.1");

		public static readonly DerObjectIdentifier RipeMD160WithRsaEncryption = new DerObjectIdentifier("1.3.36.3.3.1.2");

		public static readonly DerObjectIdentifier IdEARsa = new DerObjectIdentifier("2.5.8.1.1");

		public static readonly DerObjectIdentifier IdPkix = new DerObjectIdentifier("1.3.6.1.5.5.7");

		public static readonly DerObjectIdentifier IdPE;

		public static readonly DerObjectIdentifier IdAD;

		public static readonly DerObjectIdentifier IdADCAIssuers;

		public static readonly DerObjectIdentifier IdADOcsp;

		public static readonly DerObjectIdentifier OcspAccessMethod;

		public static readonly DerObjectIdentifier CrlAccessMethod;

		static X509ObjectIdentifiers()
		{
			DerObjectIdentifier idPkix = IdPkix;
			IdPE = new DerObjectIdentifier(((idPkix != null) ? idPkix.ToString() : null) + ".1");
			DerObjectIdentifier idPkix2 = IdPkix;
			IdAD = new DerObjectIdentifier(((idPkix2 != null) ? idPkix2.ToString() : null) + ".48");
			DerObjectIdentifier idAD = IdAD;
			IdADCAIssuers = new DerObjectIdentifier(((idAD != null) ? idAD.ToString() : null) + ".2");
			DerObjectIdentifier idAD2 = IdAD;
			IdADOcsp = new DerObjectIdentifier(((idAD2 != null) ? idAD2.ToString() : null) + ".1");
			OcspAccessMethod = IdADOcsp;
			CrlAccessMethod = IdADCAIssuers;
		}
	}
}
