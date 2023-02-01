using Tr.Com.Eimza.Org.BouncyCastle.Asn1;

namespace Tr.Com.Eimza.Org.BouncyCastle.OID
{
	internal class CmpValues
	{
		public static readonly int[] id_PasswordBasedMac;

		public static readonly int[] id_HMAC_SHA1;

		public static readonly int[] id_DHBasedMac;

		public static readonly int[] id_it_suppLangTags;

		public static readonly int[] id_pkix;

		public static readonly int[] id_it;

		public static readonly byte[] Rtkey;

		public static readonly DerObjectIdentifier Id_PasswordBasedMac_OID;

		public static readonly DerObjectIdentifier Id_HMAC_SHA1_OID;

		public static readonly DerObjectIdentifier Id_DHBasedMac_OID;

		public static readonly DerObjectIdentifier Id_it_suppLangTags_OID;

		public static readonly DerObjectIdentifier Id_pkix_OID;

		public static readonly DerObjectIdentifier Id_it_OID;

		static CmpValues()
		{
			id_PasswordBasedMac = new int[7] { 1, 2, 840, 113533, 7, 66, 13 };
			id_HMAC_SHA1 = new int[9] { 1, 3, 6, 1, 5, 5, 8, 1, 2 };
			id_DHBasedMac = new int[7] { 1, 2, 840, 113533, 7, 66, 30 };
			id_it_suppLangTags = new int[9] { 1, 3, 6, 1, 5, 5, 7, 4, 16 };
			id_pkix = new int[7] { 1, 3, 6, 1, 5, 5, 7 };
			id_it = new int[8] { 1, 3, 6, 1, 5, 5, 7, 4 };
			Rtkey = new byte[64]
			{
				199, 250, 42, 193, 222, 129, 245, 135, 112, 219,
				37, 225, 123, 188, 32, 254, 233, 79, 169, 247,
				139, 48, 242, 58, 73, 151, 82, 255, 228, 140,
				94, 83, 63, 23, 143, 95, 78, 135, 145, 193,
				96, 49, 176, 208, 181, 222, 83, 20, 61, 99,
				114, 240, 17, 142, 63, 89, 143, 134, 12, 121,
				145, 165, 35, 118
			};
			Id_PasswordBasedMac_OID = OIDUtil.ConvertIdentifier(id_PasswordBasedMac);
			Id_HMAC_SHA1_OID = OIDUtil.ConvertIdentifier(id_HMAC_SHA1);
			Id_DHBasedMac_OID = OIDUtil.ConvertIdentifier(id_DHBasedMac);
			Id_it_suppLangTags_OID = OIDUtil.ConvertIdentifier(id_it_suppLangTags);
			Id_pkix_OID = OIDUtil.ConvertIdentifier(id_pkix);
			Id_it_OID = OIDUtil.ConvertIdentifier(id_it);
		}
	}
}
