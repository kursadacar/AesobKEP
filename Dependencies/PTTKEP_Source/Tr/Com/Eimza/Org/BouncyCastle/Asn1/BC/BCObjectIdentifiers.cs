namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.BC
{
	internal abstract class BCObjectIdentifiers
	{
		public static readonly DerObjectIdentifier bc = new DerObjectIdentifier("1.3.6.1.4.1.22554");

		public static readonly DerObjectIdentifier bc_pbe;

		public static readonly DerObjectIdentifier bc_pbe_sha1;

		public static readonly DerObjectIdentifier bc_pbe_sha256;

		public static readonly DerObjectIdentifier bc_pbe_sha384;

		public static readonly DerObjectIdentifier bc_pbe_sha512;

		public static readonly DerObjectIdentifier bc_pbe_sha224;

		public static readonly DerObjectIdentifier bc_pbe_sha1_pkcs5;

		public static readonly DerObjectIdentifier bc_pbe_sha1_pkcs12;

		public static readonly DerObjectIdentifier bc_pbe_sha256_pkcs5;

		public static readonly DerObjectIdentifier bc_pbe_sha256_pkcs12;

		public static readonly DerObjectIdentifier bc_pbe_sha1_pkcs12_aes128_cbc;

		public static readonly DerObjectIdentifier bc_pbe_sha1_pkcs12_aes192_cbc;

		public static readonly DerObjectIdentifier bc_pbe_sha1_pkcs12_aes256_cbc;

		public static readonly DerObjectIdentifier bc_pbe_sha256_pkcs12_aes128_cbc;

		public static readonly DerObjectIdentifier bc_pbe_sha256_pkcs12_aes192_cbc;

		public static readonly DerObjectIdentifier bc_pbe_sha256_pkcs12_aes256_cbc;

		static BCObjectIdentifiers()
		{
			DerObjectIdentifier derObjectIdentifier = bc;
			bc_pbe = new DerObjectIdentifier(((derObjectIdentifier != null) ? derObjectIdentifier.ToString() : null) + ".1");
			DerObjectIdentifier derObjectIdentifier2 = bc_pbe;
			bc_pbe_sha1 = new DerObjectIdentifier(((derObjectIdentifier2 != null) ? derObjectIdentifier2.ToString() : null) + ".1");
			DerObjectIdentifier derObjectIdentifier3 = bc_pbe;
			bc_pbe_sha256 = new DerObjectIdentifier(((derObjectIdentifier3 != null) ? derObjectIdentifier3.ToString() : null) + ".2.1");
			DerObjectIdentifier derObjectIdentifier4 = bc_pbe;
			bc_pbe_sha384 = new DerObjectIdentifier(((derObjectIdentifier4 != null) ? derObjectIdentifier4.ToString() : null) + ".2.2");
			DerObjectIdentifier derObjectIdentifier5 = bc_pbe;
			bc_pbe_sha512 = new DerObjectIdentifier(((derObjectIdentifier5 != null) ? derObjectIdentifier5.ToString() : null) + ".2.3");
			DerObjectIdentifier derObjectIdentifier6 = bc_pbe;
			bc_pbe_sha224 = new DerObjectIdentifier(((derObjectIdentifier6 != null) ? derObjectIdentifier6.ToString() : null) + ".2.4");
			DerObjectIdentifier derObjectIdentifier7 = bc_pbe_sha1;
			bc_pbe_sha1_pkcs5 = new DerObjectIdentifier(((derObjectIdentifier7 != null) ? derObjectIdentifier7.ToString() : null) + ".1");
			DerObjectIdentifier derObjectIdentifier8 = bc_pbe_sha1;
			bc_pbe_sha1_pkcs12 = new DerObjectIdentifier(((derObjectIdentifier8 != null) ? derObjectIdentifier8.ToString() : null) + ".2");
			DerObjectIdentifier derObjectIdentifier9 = bc_pbe_sha256;
			bc_pbe_sha256_pkcs5 = new DerObjectIdentifier(((derObjectIdentifier9 != null) ? derObjectIdentifier9.ToString() : null) + ".1");
			DerObjectIdentifier derObjectIdentifier10 = bc_pbe_sha256;
			bc_pbe_sha256_pkcs12 = new DerObjectIdentifier(((derObjectIdentifier10 != null) ? derObjectIdentifier10.ToString() : null) + ".2");
			DerObjectIdentifier derObjectIdentifier11 = bc_pbe_sha1_pkcs12;
			bc_pbe_sha1_pkcs12_aes128_cbc = new DerObjectIdentifier(((derObjectIdentifier11 != null) ? derObjectIdentifier11.ToString() : null) + ".1.2");
			DerObjectIdentifier derObjectIdentifier12 = bc_pbe_sha1_pkcs12;
			bc_pbe_sha1_pkcs12_aes192_cbc = new DerObjectIdentifier(((derObjectIdentifier12 != null) ? derObjectIdentifier12.ToString() : null) + ".1.22");
			DerObjectIdentifier derObjectIdentifier13 = bc_pbe_sha1_pkcs12;
			bc_pbe_sha1_pkcs12_aes256_cbc = new DerObjectIdentifier(((derObjectIdentifier13 != null) ? derObjectIdentifier13.ToString() : null) + ".1.42");
			DerObjectIdentifier derObjectIdentifier14 = bc_pbe_sha256_pkcs12;
			bc_pbe_sha256_pkcs12_aes128_cbc = new DerObjectIdentifier(((derObjectIdentifier14 != null) ? derObjectIdentifier14.ToString() : null) + ".1.2");
			DerObjectIdentifier derObjectIdentifier15 = bc_pbe_sha256_pkcs12;
			bc_pbe_sha256_pkcs12_aes192_cbc = new DerObjectIdentifier(((derObjectIdentifier15 != null) ? derObjectIdentifier15.ToString() : null) + ".1.22");
			DerObjectIdentifier derObjectIdentifier16 = bc_pbe_sha256_pkcs12;
			bc_pbe_sha256_pkcs12_aes256_cbc = new DerObjectIdentifier(((derObjectIdentifier16 != null) ? derObjectIdentifier16.ToString() : null) + ".1.42");
		}
	}
}
