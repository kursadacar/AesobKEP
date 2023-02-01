namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Nist
{
	internal sealed class NistObjectIdentifiers
	{
		public static readonly DerObjectIdentifier NistAlgorithm = new DerObjectIdentifier("2.16.840.1.101.3.4");

		public static readonly DerObjectIdentifier HashAlgs = NistAlgorithm.Branch("2");

		public static readonly DerObjectIdentifier IdSha256 = HashAlgs.Branch("1");

		public static readonly DerObjectIdentifier IdSha384 = HashAlgs.Branch("2");

		public static readonly DerObjectIdentifier IdSha512 = HashAlgs.Branch("3");

		public static readonly DerObjectIdentifier IdSha224 = HashAlgs.Branch("4");

		public static readonly DerObjectIdentifier IdSha512_224 = HashAlgs.Branch("5");

		public static readonly DerObjectIdentifier IdSha512_256 = HashAlgs.Branch("6");

		public static readonly DerObjectIdentifier Aes;

		public static readonly DerObjectIdentifier IdAes128Ecb;

		public static readonly DerObjectIdentifier IdAes128Cbc;

		public static readonly DerObjectIdentifier IdAes128Ofb;

		public static readonly DerObjectIdentifier IdAes128Cfb;

		public static readonly DerObjectIdentifier IdAes128Wrap;

		public static readonly DerObjectIdentifier IdAes128Gcm;

		public static readonly DerObjectIdentifier IdAes128Ccm;

		public static readonly DerObjectIdentifier IdAes192Ecb;

		public static readonly DerObjectIdentifier IdAes192Cbc;

		public static readonly DerObjectIdentifier IdAes192Ofb;

		public static readonly DerObjectIdentifier IdAes192Cfb;

		public static readonly DerObjectIdentifier IdAes192Wrap;

		public static readonly DerObjectIdentifier IdAes192Gcm;

		public static readonly DerObjectIdentifier IdAes192Ccm;

		public static readonly DerObjectIdentifier IdAes256Ecb;

		public static readonly DerObjectIdentifier IdAes256Cbc;

		public static readonly DerObjectIdentifier IdAes256Ofb;

		public static readonly DerObjectIdentifier IdAes256Cfb;

		public static readonly DerObjectIdentifier IdAes256Wrap;

		public static readonly DerObjectIdentifier IdAes256Gcm;

		public static readonly DerObjectIdentifier IdAes256Ccm;

		public static readonly DerObjectIdentifier IdDsaWithSha2;

		public static readonly DerObjectIdentifier DsaWithSha224;

		public static readonly DerObjectIdentifier DsaWithSha256;

		public static readonly DerObjectIdentifier DsaWithSha384;

		public static readonly DerObjectIdentifier DsaWithSha512;

		private NistObjectIdentifiers()
		{
		}

		static NistObjectIdentifiers()
		{
			DerObjectIdentifier nistAlgorithm = NistAlgorithm;
			Aes = new DerObjectIdentifier(((nistAlgorithm != null) ? nistAlgorithm.ToString() : null) + ".1");
			DerObjectIdentifier aes = Aes;
			IdAes128Ecb = new DerObjectIdentifier(((aes != null) ? aes.ToString() : null) + ".1");
			DerObjectIdentifier aes2 = Aes;
			IdAes128Cbc = new DerObjectIdentifier(((aes2 != null) ? aes2.ToString() : null) + ".2");
			DerObjectIdentifier aes3 = Aes;
			IdAes128Ofb = new DerObjectIdentifier(((aes3 != null) ? aes3.ToString() : null) + ".3");
			DerObjectIdentifier aes4 = Aes;
			IdAes128Cfb = new DerObjectIdentifier(((aes4 != null) ? aes4.ToString() : null) + ".4");
			DerObjectIdentifier aes5 = Aes;
			IdAes128Wrap = new DerObjectIdentifier(((aes5 != null) ? aes5.ToString() : null) + ".5");
			DerObjectIdentifier aes6 = Aes;
			IdAes128Gcm = new DerObjectIdentifier(((aes6 != null) ? aes6.ToString() : null) + ".6");
			DerObjectIdentifier aes7 = Aes;
			IdAes128Ccm = new DerObjectIdentifier(((aes7 != null) ? aes7.ToString() : null) + ".7");
			DerObjectIdentifier aes8 = Aes;
			IdAes192Ecb = new DerObjectIdentifier(((aes8 != null) ? aes8.ToString() : null) + ".21");
			DerObjectIdentifier aes9 = Aes;
			IdAes192Cbc = new DerObjectIdentifier(((aes9 != null) ? aes9.ToString() : null) + ".22");
			DerObjectIdentifier aes10 = Aes;
			IdAes192Ofb = new DerObjectIdentifier(((aes10 != null) ? aes10.ToString() : null) + ".23");
			DerObjectIdentifier aes11 = Aes;
			IdAes192Cfb = new DerObjectIdentifier(((aes11 != null) ? aes11.ToString() : null) + ".24");
			DerObjectIdentifier aes12 = Aes;
			IdAes192Wrap = new DerObjectIdentifier(((aes12 != null) ? aes12.ToString() : null) + ".25");
			DerObjectIdentifier aes13 = Aes;
			IdAes192Gcm = new DerObjectIdentifier(((aes13 != null) ? aes13.ToString() : null) + ".26");
			DerObjectIdentifier aes14 = Aes;
			IdAes192Ccm = new DerObjectIdentifier(((aes14 != null) ? aes14.ToString() : null) + ".27");
			DerObjectIdentifier aes15 = Aes;
			IdAes256Ecb = new DerObjectIdentifier(((aes15 != null) ? aes15.ToString() : null) + ".41");
			DerObjectIdentifier aes16 = Aes;
			IdAes256Cbc = new DerObjectIdentifier(((aes16 != null) ? aes16.ToString() : null) + ".42");
			DerObjectIdentifier aes17 = Aes;
			IdAes256Ofb = new DerObjectIdentifier(((aes17 != null) ? aes17.ToString() : null) + ".43");
			DerObjectIdentifier aes18 = Aes;
			IdAes256Cfb = new DerObjectIdentifier(((aes18 != null) ? aes18.ToString() : null) + ".44");
			DerObjectIdentifier aes19 = Aes;
			IdAes256Wrap = new DerObjectIdentifier(((aes19 != null) ? aes19.ToString() : null) + ".45");
			DerObjectIdentifier aes20 = Aes;
			IdAes256Gcm = new DerObjectIdentifier(((aes20 != null) ? aes20.ToString() : null) + ".46");
			DerObjectIdentifier aes21 = Aes;
			IdAes256Ccm = new DerObjectIdentifier(((aes21 != null) ? aes21.ToString() : null) + ".47");
			DerObjectIdentifier nistAlgorithm2 = NistAlgorithm;
			IdDsaWithSha2 = new DerObjectIdentifier(((nistAlgorithm2 != null) ? nistAlgorithm2.ToString() : null) + ".3");
			DerObjectIdentifier idDsaWithSha = IdDsaWithSha2;
			DsaWithSha224 = new DerObjectIdentifier(((idDsaWithSha != null) ? idDsaWithSha.ToString() : null) + ".1");
			DerObjectIdentifier idDsaWithSha2 = IdDsaWithSha2;
			DsaWithSha256 = new DerObjectIdentifier(((idDsaWithSha2 != null) ? idDsaWithSha2.ToString() : null) + ".2");
			DerObjectIdentifier idDsaWithSha3 = IdDsaWithSha2;
			DsaWithSha384 = new DerObjectIdentifier(((idDsaWithSha3 != null) ? idDsaWithSha3.ToString() : null) + ".3");
			DerObjectIdentifier idDsaWithSha4 = IdDsaWithSha2;
			DsaWithSha512 = new DerObjectIdentifier(((idDsaWithSha4 != null) ? idDsaWithSha4.ToString() : null) + ".4");
		}
	}
}
