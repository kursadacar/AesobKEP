using Tr.Com.Eimza.Org.BouncyCastle.Asn1;

namespace Tr.Com.Eimza.Org.BouncyCastle.OID
{
	internal class AesValues
	{
		public static readonly int[] csor;

		public static readonly int[] nistAlgorithms;

		public static readonly int[] aes;

		public static readonly int[] id_aes128_ECB;

		public static readonly int[] id_aes128_CBC;

		public static readonly int[] id_aes128_OFB;

		public static readonly int[] id_aes128_CFB;

		public static readonly int[] id_aes192_ECB;

		public static readonly int[] id_aes192_CBC;

		public static readonly int[] id_aes192_OFB;

		public static readonly int[] id_aes192_CFB;

		public static readonly int[] id_aes256_ECB;

		public static readonly int[] id_aes256_CBC;

		public static readonly int[] id_aes256_OFB;

		public static readonly int[] id_aes256_CFB;

		public static readonly int[] id_aes128_wrap;

		public static readonly int[] id_aes192_wrap;

		public static readonly int[] id_aes256_wrap;

		public static readonly byte[] Rtkey;

		public static readonly DerObjectIdentifier CSOR_OID;

		public static readonly DerObjectIdentifier NistAlgorithms_OID;

		public static readonly DerObjectIdentifier Aes_OID;

		public static readonly DerObjectIdentifier Id_aes128_ECB_OID;

		public static readonly DerObjectIdentifier Id_aes128_CBC_OID;

		public static readonly DerObjectIdentifier Id_aes128_OFB_OID;

		public static readonly DerObjectIdentifier Id_aes128_CFB_OID;

		public static readonly DerObjectIdentifier Id_aes192_ECB_OID;

		public static readonly DerObjectIdentifier Id_aes192_CBC_OID;

		public static readonly DerObjectIdentifier Id_aes192_OFB_OID;

		public static readonly DerObjectIdentifier Id_aes192_CFB_OID;

		public static readonly DerObjectIdentifier Id_aes256_ECB_OID;

		public static readonly DerObjectIdentifier Id_aes256_CBC_OID;

		public static readonly DerObjectIdentifier Id_aes256_OFB_OID;

		public static readonly DerObjectIdentifier Id_aes256_CFB_OID;

		public static readonly DerObjectIdentifier Id_aes128_wrap_OID;

		public static readonly DerObjectIdentifier Id_aes192_wrap_OID;

		public static readonly DerObjectIdentifier Id_aes256_wrap_OID;

		static AesValues()
		{
			csor = new int[6] { 2, 16, 840, 1, 101, 3 };
			nistAlgorithms = new int[7] { 2, 16, 840, 1, 101, 3, 4 };
			aes = new int[8] { 2, 16, 840, 1, 101, 3, 4, 1 };
			id_aes128_ECB = new int[9] { 2, 16, 840, 1, 101, 3, 4, 1, 1 };
			id_aes128_CBC = new int[9] { 2, 16, 840, 1, 101, 3, 4, 1, 2 };
			id_aes128_OFB = new int[9] { 2, 16, 840, 1, 101, 3, 4, 1, 3 };
			id_aes128_CFB = new int[9] { 2, 16, 840, 1, 101, 3, 4, 1, 4 };
			id_aes192_ECB = new int[9] { 2, 16, 840, 1, 101, 3, 4, 1, 21 };
			id_aes192_CBC = new int[9] { 2, 16, 840, 1, 101, 3, 4, 1, 22 };
			id_aes192_OFB = new int[9] { 2, 16, 840, 1, 101, 3, 4, 1, 23 };
			id_aes192_CFB = new int[9] { 2, 16, 840, 1, 101, 3, 4, 1, 24 };
			id_aes256_ECB = new int[9] { 2, 16, 840, 1, 101, 3, 4, 1, 41 };
			id_aes256_CBC = new int[9] { 2, 16, 840, 1, 101, 3, 4, 1, 42 };
			id_aes256_OFB = new int[9] { 2, 16, 840, 1, 101, 3, 4, 1, 43 };
			id_aes256_CFB = new int[9] { 2, 16, 840, 1, 101, 3, 4, 1, 44 };
			id_aes128_wrap = new int[9] { 2, 16, 840, 1, 101, 3, 4, 1, 5 };
			id_aes192_wrap = new int[9] { 2, 16, 840, 1, 101, 3, 4, 1, 25 };
			id_aes256_wrap = new int[9] { 2, 16, 840, 1, 101, 3, 4, 1, 45 };
			Rtkey = new byte[64]
			{
				199, 250, 42, 193, 222, 129, 245, 135, 112, 219,
				37, 225, 123, 188, 32, 254, 233, 79, 169, 247,
				139, 48, 242, 58, 34, 86, 69, 212, 218, 41,
				18, 227, 63, 23, 143, 95, 78, 135, 145, 193,
				96, 49, 176, 208, 181, 222, 83, 20, 61, 99,
				114, 240, 17, 142, 63, 89, 143, 134, 12, 121,
				145, 165, 35, 118
			};
			CSOR_OID = OIDUtil.ConvertIdentifier(csor);
			NistAlgorithms_OID = OIDUtil.ConvertIdentifier(nistAlgorithms);
			Aes_OID = OIDUtil.ConvertIdentifier(aes);
			Id_aes128_ECB_OID = OIDUtil.ConvertIdentifier(id_aes128_ECB);
			Id_aes128_CBC_OID = OIDUtil.ConvertIdentifier(id_aes128_CBC);
			Id_aes128_OFB_OID = OIDUtil.ConvertIdentifier(id_aes128_OFB);
			Id_aes128_CFB_OID = OIDUtil.ConvertIdentifier(id_aes128_CFB);
			Id_aes192_ECB_OID = OIDUtil.ConvertIdentifier(id_aes192_ECB);
			Id_aes192_CBC_OID = OIDUtil.ConvertIdentifier(id_aes192_CBC);
			Id_aes192_OFB_OID = OIDUtil.ConvertIdentifier(id_aes192_OFB);
			Id_aes192_CFB_OID = OIDUtil.ConvertIdentifier(id_aes192_CFB);
			Id_aes256_ECB_OID = OIDUtil.ConvertIdentifier(id_aes256_ECB);
			Id_aes256_CBC_OID = OIDUtil.ConvertIdentifier(id_aes256_CBC);
			Id_aes256_OFB_OID = OIDUtil.ConvertIdentifier(id_aes256_OFB);
			Id_aes256_CFB_OID = OIDUtil.ConvertIdentifier(id_aes256_CFB);
			Id_aes128_wrap_OID = OIDUtil.ConvertIdentifier(id_aes128_wrap);
			Id_aes192_wrap_OID = OIDUtil.ConvertIdentifier(id_aes192_wrap);
			Id_aes256_wrap_OID = OIDUtil.ConvertIdentifier(id_aes256_wrap);
		}
	}
}
