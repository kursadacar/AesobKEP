namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.IsisMtt
{
	internal abstract class IsisMttObjectIdentifiers
	{
		public static readonly DerObjectIdentifier IdIsisMtt = new DerObjectIdentifier("1.3.36.8");

		public static readonly DerObjectIdentifier IdIsisMttCP;

		public static readonly DerObjectIdentifier IdIsisMttCPAccredited;

		public static readonly DerObjectIdentifier IdIsisMttAT;

		public static readonly DerObjectIdentifier IdIsisMttATDateOfCertGen;

		public static readonly DerObjectIdentifier IdIsisMttATProcuration;

		public static readonly DerObjectIdentifier IdIsisMttATAdmission;

		public static readonly DerObjectIdentifier IdIsisMttATMonetaryLimit;

		public static readonly DerObjectIdentifier IdIsisMttATDeclarationOfMajority;

		public static readonly DerObjectIdentifier IdIsisMttATIccsn;

		public static readonly DerObjectIdentifier IdIsisMttATPKReference;

		public static readonly DerObjectIdentifier IdIsisMttATRestriction;

		public static readonly DerObjectIdentifier IdIsisMttATRetrieveIfAllowed;

		public static readonly DerObjectIdentifier IdIsisMttATRequestedCertificate;

		public static readonly DerObjectIdentifier IdIsisMttATNamingAuthorities;

		public static readonly DerObjectIdentifier IdIsisMttATCertInDirSince;

		public static readonly DerObjectIdentifier IdIsisMttATCertHash;

		public static readonly DerObjectIdentifier IdIsisMttATNameAtBirth;

		public static readonly DerObjectIdentifier IdIsisMttATAdditionalInformation;

		public static readonly DerObjectIdentifier IdIsisMttATLiabilityLimitationFlag;

		static IsisMttObjectIdentifiers()
		{
			DerObjectIdentifier idIsisMtt = IdIsisMtt;
			IdIsisMttCP = new DerObjectIdentifier(((idIsisMtt != null) ? idIsisMtt.ToString() : null) + ".1");
			DerObjectIdentifier idIsisMttCP = IdIsisMttCP;
			IdIsisMttCPAccredited = new DerObjectIdentifier(((idIsisMttCP != null) ? idIsisMttCP.ToString() : null) + ".1");
			DerObjectIdentifier idIsisMtt2 = IdIsisMtt;
			IdIsisMttAT = new DerObjectIdentifier(((idIsisMtt2 != null) ? idIsisMtt2.ToString() : null) + ".3");
			DerObjectIdentifier idIsisMttAT = IdIsisMttAT;
			IdIsisMttATDateOfCertGen = new DerObjectIdentifier(((idIsisMttAT != null) ? idIsisMttAT.ToString() : null) + ".1");
			DerObjectIdentifier idIsisMttAT2 = IdIsisMttAT;
			IdIsisMttATProcuration = new DerObjectIdentifier(((idIsisMttAT2 != null) ? idIsisMttAT2.ToString() : null) + ".2");
			DerObjectIdentifier idIsisMttAT3 = IdIsisMttAT;
			IdIsisMttATAdmission = new DerObjectIdentifier(((idIsisMttAT3 != null) ? idIsisMttAT3.ToString() : null) + ".3");
			DerObjectIdentifier idIsisMttAT4 = IdIsisMttAT;
			IdIsisMttATMonetaryLimit = new DerObjectIdentifier(((idIsisMttAT4 != null) ? idIsisMttAT4.ToString() : null) + ".4");
			DerObjectIdentifier idIsisMttAT5 = IdIsisMttAT;
			IdIsisMttATDeclarationOfMajority = new DerObjectIdentifier(((idIsisMttAT5 != null) ? idIsisMttAT5.ToString() : null) + ".5");
			DerObjectIdentifier idIsisMttAT6 = IdIsisMttAT;
			IdIsisMttATIccsn = new DerObjectIdentifier(((idIsisMttAT6 != null) ? idIsisMttAT6.ToString() : null) + ".6");
			DerObjectIdentifier idIsisMttAT7 = IdIsisMttAT;
			IdIsisMttATPKReference = new DerObjectIdentifier(((idIsisMttAT7 != null) ? idIsisMttAT7.ToString() : null) + ".7");
			DerObjectIdentifier idIsisMttAT8 = IdIsisMttAT;
			IdIsisMttATRestriction = new DerObjectIdentifier(((idIsisMttAT8 != null) ? idIsisMttAT8.ToString() : null) + ".8");
			DerObjectIdentifier idIsisMttAT9 = IdIsisMttAT;
			IdIsisMttATRetrieveIfAllowed = new DerObjectIdentifier(((idIsisMttAT9 != null) ? idIsisMttAT9.ToString() : null) + ".9");
			DerObjectIdentifier idIsisMttAT10 = IdIsisMttAT;
			IdIsisMttATRequestedCertificate = new DerObjectIdentifier(((idIsisMttAT10 != null) ? idIsisMttAT10.ToString() : null) + ".10");
			DerObjectIdentifier idIsisMttAT11 = IdIsisMttAT;
			IdIsisMttATNamingAuthorities = new DerObjectIdentifier(((idIsisMttAT11 != null) ? idIsisMttAT11.ToString() : null) + ".11");
			DerObjectIdentifier idIsisMttAT12 = IdIsisMttAT;
			IdIsisMttATCertInDirSince = new DerObjectIdentifier(((idIsisMttAT12 != null) ? idIsisMttAT12.ToString() : null) + ".12");
			DerObjectIdentifier idIsisMttAT13 = IdIsisMttAT;
			IdIsisMttATCertHash = new DerObjectIdentifier(((idIsisMttAT13 != null) ? idIsisMttAT13.ToString() : null) + ".13");
			DerObjectIdentifier idIsisMttAT14 = IdIsisMttAT;
			IdIsisMttATNameAtBirth = new DerObjectIdentifier(((idIsisMttAT14 != null) ? idIsisMttAT14.ToString() : null) + ".14");
			DerObjectIdentifier idIsisMttAT15 = IdIsisMttAT;
			IdIsisMttATAdditionalInformation = new DerObjectIdentifier(((idIsisMttAT15 != null) ? idIsisMttAT15.ToString() : null) + ".15");
			IdIsisMttATLiabilityLimitationFlag = new DerObjectIdentifier("0.2.262.1.10.12.0");
		}
	}
}
