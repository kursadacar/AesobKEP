namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Misc
{
	internal abstract class MiscObjectIdentifiers
	{
		public static readonly DerObjectIdentifier Netscape = new DerObjectIdentifier("2.16.840.1.113730.1");

		public static readonly DerObjectIdentifier NetscapeCertType;

		public static readonly DerObjectIdentifier NetscapeBaseUrl;

		public static readonly DerObjectIdentifier NetscapeRevocationUrl;

		public static readonly DerObjectIdentifier NetscapeCARevocationUrl;

		public static readonly DerObjectIdentifier NetscapeRenewalUrl;

		public static readonly DerObjectIdentifier NetscapeCAPolicyUrl;

		public static readonly DerObjectIdentifier NetscapeSslServerName;

		public static readonly DerObjectIdentifier NetscapeCertComment;

		internal const string Verisign = "2.16.840.1.113733.1";

		public static readonly DerObjectIdentifier VerisignCzagExtension;

		public static readonly DerObjectIdentifier VerisignDnbDunsNumber;

		public static readonly string Novell;

		public static readonly DerObjectIdentifier NovellSecurityAttribs;

		public static readonly string Entrust;

		public static readonly DerObjectIdentifier EntrustVersionExtension;

		static MiscObjectIdentifiers()
		{
			DerObjectIdentifier netscape = Netscape;
			NetscapeCertType = new DerObjectIdentifier(((netscape != null) ? netscape.ToString() : null) + ".1");
			DerObjectIdentifier netscape2 = Netscape;
			NetscapeBaseUrl = new DerObjectIdentifier(((netscape2 != null) ? netscape2.ToString() : null) + ".2");
			DerObjectIdentifier netscape3 = Netscape;
			NetscapeRevocationUrl = new DerObjectIdentifier(((netscape3 != null) ? netscape3.ToString() : null) + ".3");
			DerObjectIdentifier netscape4 = Netscape;
			NetscapeCARevocationUrl = new DerObjectIdentifier(((netscape4 != null) ? netscape4.ToString() : null) + ".4");
			DerObjectIdentifier netscape5 = Netscape;
			NetscapeRenewalUrl = new DerObjectIdentifier(((netscape5 != null) ? netscape5.ToString() : null) + ".7");
			DerObjectIdentifier netscape6 = Netscape;
			NetscapeCAPolicyUrl = new DerObjectIdentifier(((netscape6 != null) ? netscape6.ToString() : null) + ".8");
			DerObjectIdentifier netscape7 = Netscape;
			NetscapeSslServerName = new DerObjectIdentifier(((netscape7 != null) ? netscape7.ToString() : null) + ".12");
			DerObjectIdentifier netscape8 = Netscape;
			NetscapeCertComment = new DerObjectIdentifier(((netscape8 != null) ? netscape8.ToString() : null) + ".13");
			VerisignCzagExtension = new DerObjectIdentifier("2.16.840.1.113733.1.6.3");
			VerisignDnbDunsNumber = new DerObjectIdentifier("2.16.840.1.113733.1.6.15");
			Novell = "2.16.840.1.113719";
			NovellSecurityAttribs = new DerObjectIdentifier(Novell + ".1.9.4.1");
			Entrust = "1.2.840.113533.7";
			EntrustVersionExtension = new DerObjectIdentifier(Entrust + ".65.0");
		}
	}
}
