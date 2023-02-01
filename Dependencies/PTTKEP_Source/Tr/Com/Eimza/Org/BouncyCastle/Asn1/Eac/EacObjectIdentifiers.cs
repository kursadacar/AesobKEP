namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Eac
{
	internal abstract class EacObjectIdentifiers
	{
		public static readonly DerObjectIdentifier bsi_de = new DerObjectIdentifier("0.4.0.127.0.7");

		public static readonly DerObjectIdentifier id_PK;

		public static readonly DerObjectIdentifier id_PK_DH;

		public static readonly DerObjectIdentifier id_PK_ECDH;

		public static readonly DerObjectIdentifier id_CA;

		public static readonly DerObjectIdentifier id_CA_DH;

		public static readonly DerObjectIdentifier id_CA_DH_3DES_CBC_CBC;

		public static readonly DerObjectIdentifier id_CA_ECDH;

		public static readonly DerObjectIdentifier id_CA_ECDH_3DES_CBC_CBC;

		public static readonly DerObjectIdentifier id_TA;

		public static readonly DerObjectIdentifier id_TA_RSA;

		public static readonly DerObjectIdentifier id_TA_RSA_v1_5_SHA_1;

		public static readonly DerObjectIdentifier id_TA_RSA_v1_5_SHA_256;

		public static readonly DerObjectIdentifier id_TA_RSA_PSS_SHA_1;

		public static readonly DerObjectIdentifier id_TA_RSA_PSS_SHA_256;

		public static readonly DerObjectIdentifier id_TA_ECDSA;

		public static readonly DerObjectIdentifier id_TA_ECDSA_SHA_1;

		public static readonly DerObjectIdentifier id_TA_ECDSA_SHA_224;

		public static readonly DerObjectIdentifier id_TA_ECDSA_SHA_256;

		public static readonly DerObjectIdentifier id_TA_ECDSA_SHA_384;

		public static readonly DerObjectIdentifier id_TA_ECDSA_SHA_512;

		static EacObjectIdentifiers()
		{
			DerObjectIdentifier derObjectIdentifier = bsi_de;
			id_PK = new DerObjectIdentifier(((derObjectIdentifier != null) ? derObjectIdentifier.ToString() : null) + ".2.2.1");
			DerObjectIdentifier derObjectIdentifier2 = id_PK;
			id_PK_DH = new DerObjectIdentifier(((derObjectIdentifier2 != null) ? derObjectIdentifier2.ToString() : null) + ".1");
			DerObjectIdentifier derObjectIdentifier3 = id_PK;
			id_PK_ECDH = new DerObjectIdentifier(((derObjectIdentifier3 != null) ? derObjectIdentifier3.ToString() : null) + ".2");
			DerObjectIdentifier derObjectIdentifier4 = bsi_de;
			id_CA = new DerObjectIdentifier(((derObjectIdentifier4 != null) ? derObjectIdentifier4.ToString() : null) + ".2.2.3");
			DerObjectIdentifier derObjectIdentifier5 = id_CA;
			id_CA_DH = new DerObjectIdentifier(((derObjectIdentifier5 != null) ? derObjectIdentifier5.ToString() : null) + ".1");
			DerObjectIdentifier derObjectIdentifier6 = id_CA_DH;
			id_CA_DH_3DES_CBC_CBC = new DerObjectIdentifier(((derObjectIdentifier6 != null) ? derObjectIdentifier6.ToString() : null) + ".1");
			DerObjectIdentifier derObjectIdentifier7 = id_CA;
			id_CA_ECDH = new DerObjectIdentifier(((derObjectIdentifier7 != null) ? derObjectIdentifier7.ToString() : null) + ".2");
			DerObjectIdentifier derObjectIdentifier8 = id_CA_ECDH;
			id_CA_ECDH_3DES_CBC_CBC = new DerObjectIdentifier(((derObjectIdentifier8 != null) ? derObjectIdentifier8.ToString() : null) + ".1");
			DerObjectIdentifier derObjectIdentifier9 = bsi_de;
			id_TA = new DerObjectIdentifier(((derObjectIdentifier9 != null) ? derObjectIdentifier9.ToString() : null) + ".2.2.2");
			DerObjectIdentifier derObjectIdentifier10 = id_TA;
			id_TA_RSA = new DerObjectIdentifier(((derObjectIdentifier10 != null) ? derObjectIdentifier10.ToString() : null) + ".1");
			DerObjectIdentifier derObjectIdentifier11 = id_TA_RSA;
			id_TA_RSA_v1_5_SHA_1 = new DerObjectIdentifier(((derObjectIdentifier11 != null) ? derObjectIdentifier11.ToString() : null) + ".1");
			DerObjectIdentifier derObjectIdentifier12 = id_TA_RSA;
			id_TA_RSA_v1_5_SHA_256 = new DerObjectIdentifier(((derObjectIdentifier12 != null) ? derObjectIdentifier12.ToString() : null) + ".2");
			DerObjectIdentifier derObjectIdentifier13 = id_TA_RSA;
			id_TA_RSA_PSS_SHA_1 = new DerObjectIdentifier(((derObjectIdentifier13 != null) ? derObjectIdentifier13.ToString() : null) + ".3");
			DerObjectIdentifier derObjectIdentifier14 = id_TA_RSA;
			id_TA_RSA_PSS_SHA_256 = new DerObjectIdentifier(((derObjectIdentifier14 != null) ? derObjectIdentifier14.ToString() : null) + ".4");
			DerObjectIdentifier derObjectIdentifier15 = id_TA;
			id_TA_ECDSA = new DerObjectIdentifier(((derObjectIdentifier15 != null) ? derObjectIdentifier15.ToString() : null) + ".2");
			DerObjectIdentifier derObjectIdentifier16 = id_TA_ECDSA;
			id_TA_ECDSA_SHA_1 = new DerObjectIdentifier(((derObjectIdentifier16 != null) ? derObjectIdentifier16.ToString() : null) + ".1");
			DerObjectIdentifier derObjectIdentifier17 = id_TA_ECDSA;
			id_TA_ECDSA_SHA_224 = new DerObjectIdentifier(((derObjectIdentifier17 != null) ? derObjectIdentifier17.ToString() : null) + ".2");
			DerObjectIdentifier derObjectIdentifier18 = id_TA_ECDSA;
			id_TA_ECDSA_SHA_256 = new DerObjectIdentifier(((derObjectIdentifier18 != null) ? derObjectIdentifier18.ToString() : null) + ".3");
			DerObjectIdentifier derObjectIdentifier19 = id_TA_ECDSA;
			id_TA_ECDSA_SHA_384 = new DerObjectIdentifier(((derObjectIdentifier19 != null) ? derObjectIdentifier19.ToString() : null) + ".4");
			DerObjectIdentifier derObjectIdentifier20 = id_TA_ECDSA;
			id_TA_ECDSA_SHA_512 = new DerObjectIdentifier(((derObjectIdentifier20 != null) ? derObjectIdentifier20.ToString() : null) + ".5");
		}
	}
}
