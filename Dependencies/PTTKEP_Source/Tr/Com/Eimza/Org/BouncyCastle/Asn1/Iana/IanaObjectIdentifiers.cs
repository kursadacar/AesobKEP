namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Iana
{
	internal abstract class IanaObjectIdentifiers
	{
		public static readonly DerObjectIdentifier IsakmpOakley = new DerObjectIdentifier("1.3.6.1.5.5.8.1");

		public static readonly DerObjectIdentifier HmacMD5;

		public static readonly DerObjectIdentifier HmacSha1;

		public static readonly DerObjectIdentifier HmacTiger;

		public static readonly DerObjectIdentifier HmacRipeMD160;

		static IanaObjectIdentifiers()
		{
			DerObjectIdentifier isakmpOakley = IsakmpOakley;
			HmacMD5 = new DerObjectIdentifier(((isakmpOakley != null) ? isakmpOakley.ToString() : null) + ".1");
			DerObjectIdentifier isakmpOakley2 = IsakmpOakley;
			HmacSha1 = new DerObjectIdentifier(((isakmpOakley2 != null) ? isakmpOakley2.ToString() : null) + ".2");
			DerObjectIdentifier isakmpOakley3 = IsakmpOakley;
			HmacTiger = new DerObjectIdentifier(((isakmpOakley3 != null) ? isakmpOakley3.ToString() : null) + ".3");
			DerObjectIdentifier isakmpOakley4 = IsakmpOakley;
			HmacRipeMD160 = new DerObjectIdentifier(((isakmpOakley4 != null) ? isakmpOakley4.ToString() : null) + ".4");
		}
	}
}
