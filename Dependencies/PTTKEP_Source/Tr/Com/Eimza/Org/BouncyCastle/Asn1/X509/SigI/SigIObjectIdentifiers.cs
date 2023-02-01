namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509.SigI
{
	internal sealed class SigIObjectIdentifiers
	{
		public static readonly DerObjectIdentifier IdSigI = new DerObjectIdentifier("1.3.36.8");

		public static readonly DerObjectIdentifier IdSigIKP;

		public static readonly DerObjectIdentifier IdSigICP;

		public static readonly DerObjectIdentifier IdSigION;

		public static readonly DerObjectIdentifier IdSigIKPDirectoryService;

		public static readonly DerObjectIdentifier IdSigIONPersonalData;

		public static readonly DerObjectIdentifier IdSigICPSigConform;

		private SigIObjectIdentifiers()
		{
		}

		static SigIObjectIdentifiers()
		{
			DerObjectIdentifier idSigI = IdSigI;
			IdSigIKP = new DerObjectIdentifier(((idSigI != null) ? idSigI.ToString() : null) + ".2");
			DerObjectIdentifier idSigI2 = IdSigI;
			IdSigICP = new DerObjectIdentifier(((idSigI2 != null) ? idSigI2.ToString() : null) + ".1");
			DerObjectIdentifier idSigI3 = IdSigI;
			IdSigION = new DerObjectIdentifier(((idSigI3 != null) ? idSigI3.ToString() : null) + ".4");
			DerObjectIdentifier idSigIKP = IdSigIKP;
			IdSigIKPDirectoryService = new DerObjectIdentifier(((idSigIKP != null) ? idSigIKP.ToString() : null) + ".1");
			DerObjectIdentifier idSigION = IdSigION;
			IdSigIONPersonalData = new DerObjectIdentifier(((idSigION != null) ? idSigION.ToString() : null) + ".1");
			DerObjectIdentifier idSigICP = IdSigICP;
			IdSigICPSigConform = new DerObjectIdentifier(((idSigICP != null) ? idSigICP.ToString() : null) + ".1");
		}
	}
}
