namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509.Qualified
{
	internal abstract class EtsiQCObjectIdentifiers
	{
		public static readonly DerObjectIdentifier IdEtsiQcs = new DerObjectIdentifier("0.4.0.1862.1");

		public static readonly DerObjectIdentifier IdEtsiQcsQcCompliance;

		public static readonly DerObjectIdentifier IdEtsiQcsLimitValue;

		public static readonly DerObjectIdentifier IdEtsiQcsRetentionPeriod;

		public static readonly DerObjectIdentifier IdEtsiQcsQcSscd;

		static EtsiQCObjectIdentifiers()
		{
			DerObjectIdentifier idEtsiQcs = IdEtsiQcs;
			IdEtsiQcsQcCompliance = new DerObjectIdentifier(((idEtsiQcs != null) ? idEtsiQcs.ToString() : null) + ".1");
			DerObjectIdentifier idEtsiQcs2 = IdEtsiQcs;
			IdEtsiQcsLimitValue = new DerObjectIdentifier(((idEtsiQcs2 != null) ? idEtsiQcs2.ToString() : null) + ".2");
			DerObjectIdentifier idEtsiQcs3 = IdEtsiQcs;
			IdEtsiQcsRetentionPeriod = new DerObjectIdentifier(((idEtsiQcs3 != null) ? idEtsiQcs3.ToString() : null) + ".3");
			DerObjectIdentifier idEtsiQcs4 = IdEtsiQcs;
			IdEtsiQcsQcSscd = new DerObjectIdentifier(((idEtsiQcs4 != null) ? idEtsiQcs4.ToString() : null) + ".4");
		}
	}
}
