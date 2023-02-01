namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.TeleTrust
{
	internal sealed class TeleTrusTObjectIdentifiers
	{
		public static readonly DerObjectIdentifier TeleTrusTAlgorithm = new DerObjectIdentifier("1.3.36.3");

		public static readonly DerObjectIdentifier RipeMD160;

		public static readonly DerObjectIdentifier RipeMD128;

		public static readonly DerObjectIdentifier RipeMD256;

		public static readonly DerObjectIdentifier TeleTrusTRsaSignatureAlgorithm;

		public static readonly DerObjectIdentifier RsaSignatureWithRipeMD160;

		public static readonly DerObjectIdentifier RsaSignatureWithRipeMD128;

		public static readonly DerObjectIdentifier RsaSignatureWithRipeMD256;

		public static readonly DerObjectIdentifier ECSign;

		public static readonly DerObjectIdentifier ECSignWithSha1;

		public static readonly DerObjectIdentifier ECSignWithRipeMD160;

		public static readonly DerObjectIdentifier EccBrainpool;

		public static readonly DerObjectIdentifier EllipticCurve;

		public static readonly DerObjectIdentifier VersionOne;

		public static readonly DerObjectIdentifier BrainpoolP160R1;

		public static readonly DerObjectIdentifier BrainpoolP160T1;

		public static readonly DerObjectIdentifier BrainpoolP192R1;

		public static readonly DerObjectIdentifier BrainpoolP192T1;

		public static readonly DerObjectIdentifier BrainpoolP224R1;

		public static readonly DerObjectIdentifier BrainpoolP224T1;

		public static readonly DerObjectIdentifier BrainpoolP256R1;

		public static readonly DerObjectIdentifier BrainpoolP256T1;

		public static readonly DerObjectIdentifier BrainpoolP320R1;

		public static readonly DerObjectIdentifier BrainpoolP320T1;

		public static readonly DerObjectIdentifier BrainpoolP384R1;

		public static readonly DerObjectIdentifier BrainpoolP384T1;

		public static readonly DerObjectIdentifier BrainpoolP512R1;

		public static readonly DerObjectIdentifier BrainpoolP512T1;

		private TeleTrusTObjectIdentifiers()
		{
		}

		static TeleTrusTObjectIdentifiers()
		{
			DerObjectIdentifier teleTrusTAlgorithm = TeleTrusTAlgorithm;
			RipeMD160 = new DerObjectIdentifier(((teleTrusTAlgorithm != null) ? teleTrusTAlgorithm.ToString() : null) + ".2.1");
			DerObjectIdentifier teleTrusTAlgorithm2 = TeleTrusTAlgorithm;
			RipeMD128 = new DerObjectIdentifier(((teleTrusTAlgorithm2 != null) ? teleTrusTAlgorithm2.ToString() : null) + ".2.2");
			DerObjectIdentifier teleTrusTAlgorithm3 = TeleTrusTAlgorithm;
			RipeMD256 = new DerObjectIdentifier(((teleTrusTAlgorithm3 != null) ? teleTrusTAlgorithm3.ToString() : null) + ".2.3");
			DerObjectIdentifier teleTrusTAlgorithm4 = TeleTrusTAlgorithm;
			TeleTrusTRsaSignatureAlgorithm = new DerObjectIdentifier(((teleTrusTAlgorithm4 != null) ? teleTrusTAlgorithm4.ToString() : null) + ".3.1");
			DerObjectIdentifier teleTrusTRsaSignatureAlgorithm = TeleTrusTRsaSignatureAlgorithm;
			RsaSignatureWithRipeMD160 = new DerObjectIdentifier(((teleTrusTRsaSignatureAlgorithm != null) ? teleTrusTRsaSignatureAlgorithm.ToString() : null) + ".2");
			DerObjectIdentifier teleTrusTRsaSignatureAlgorithm2 = TeleTrusTRsaSignatureAlgorithm;
			RsaSignatureWithRipeMD128 = new DerObjectIdentifier(((teleTrusTRsaSignatureAlgorithm2 != null) ? teleTrusTRsaSignatureAlgorithm2.ToString() : null) + ".3");
			DerObjectIdentifier teleTrusTRsaSignatureAlgorithm3 = TeleTrusTRsaSignatureAlgorithm;
			RsaSignatureWithRipeMD256 = new DerObjectIdentifier(((teleTrusTRsaSignatureAlgorithm3 != null) ? teleTrusTRsaSignatureAlgorithm3.ToString() : null) + ".4");
			DerObjectIdentifier teleTrusTAlgorithm5 = TeleTrusTAlgorithm;
			ECSign = new DerObjectIdentifier(((teleTrusTAlgorithm5 != null) ? teleTrusTAlgorithm5.ToString() : null) + ".3.2");
			DerObjectIdentifier eCSign = ECSign;
			ECSignWithSha1 = new DerObjectIdentifier(((eCSign != null) ? eCSign.ToString() : null) + ".1");
			DerObjectIdentifier eCSign2 = ECSign;
			ECSignWithRipeMD160 = new DerObjectIdentifier(((eCSign2 != null) ? eCSign2.ToString() : null) + ".2");
			DerObjectIdentifier teleTrusTAlgorithm6 = TeleTrusTAlgorithm;
			EccBrainpool = new DerObjectIdentifier(((teleTrusTAlgorithm6 != null) ? teleTrusTAlgorithm6.ToString() : null) + ".3.2.8");
			DerObjectIdentifier eccBrainpool = EccBrainpool;
			EllipticCurve = new DerObjectIdentifier(((eccBrainpool != null) ? eccBrainpool.ToString() : null) + ".1");
			DerObjectIdentifier ellipticCurve = EllipticCurve;
			VersionOne = new DerObjectIdentifier(((ellipticCurve != null) ? ellipticCurve.ToString() : null) + ".1");
			DerObjectIdentifier versionOne = VersionOne;
			BrainpoolP160R1 = new DerObjectIdentifier(((versionOne != null) ? versionOne.ToString() : null) + ".1");
			DerObjectIdentifier versionOne2 = VersionOne;
			BrainpoolP160T1 = new DerObjectIdentifier(((versionOne2 != null) ? versionOne2.ToString() : null) + ".2");
			DerObjectIdentifier versionOne3 = VersionOne;
			BrainpoolP192R1 = new DerObjectIdentifier(((versionOne3 != null) ? versionOne3.ToString() : null) + ".3");
			DerObjectIdentifier versionOne4 = VersionOne;
			BrainpoolP192T1 = new DerObjectIdentifier(((versionOne4 != null) ? versionOne4.ToString() : null) + ".4");
			DerObjectIdentifier versionOne5 = VersionOne;
			BrainpoolP224R1 = new DerObjectIdentifier(((versionOne5 != null) ? versionOne5.ToString() : null) + ".5");
			DerObjectIdentifier versionOne6 = VersionOne;
			BrainpoolP224T1 = new DerObjectIdentifier(((versionOne6 != null) ? versionOne6.ToString() : null) + ".6");
			DerObjectIdentifier versionOne7 = VersionOne;
			BrainpoolP256R1 = new DerObjectIdentifier(((versionOne7 != null) ? versionOne7.ToString() : null) + ".7");
			DerObjectIdentifier versionOne8 = VersionOne;
			BrainpoolP256T1 = new DerObjectIdentifier(((versionOne8 != null) ? versionOne8.ToString() : null) + ".8");
			DerObjectIdentifier versionOne9 = VersionOne;
			BrainpoolP320R1 = new DerObjectIdentifier(((versionOne9 != null) ? versionOne9.ToString() : null) + ".9");
			DerObjectIdentifier versionOne10 = VersionOne;
			BrainpoolP320T1 = new DerObjectIdentifier(((versionOne10 != null) ? versionOne10.ToString() : null) + ".10");
			DerObjectIdentifier versionOne11 = VersionOne;
			BrainpoolP384R1 = new DerObjectIdentifier(((versionOne11 != null) ? versionOne11.ToString() : null) + ".11");
			DerObjectIdentifier versionOne12 = VersionOne;
			BrainpoolP384T1 = new DerObjectIdentifier(((versionOne12 != null) ? versionOne12.ToString() : null) + ".12");
			DerObjectIdentifier versionOne13 = VersionOne;
			BrainpoolP512R1 = new DerObjectIdentifier(((versionOne13 != null) ? versionOne13.ToString() : null) + ".13");
			DerObjectIdentifier versionOne14 = VersionOne;
			BrainpoolP512T1 = new DerObjectIdentifier(((versionOne14 != null) ? versionOne14.ToString() : null) + ".14");
		}
	}
}
