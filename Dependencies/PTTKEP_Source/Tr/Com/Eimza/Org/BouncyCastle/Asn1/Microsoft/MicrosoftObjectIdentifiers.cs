namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Microsoft
{
	internal abstract class MicrosoftObjectIdentifiers
	{
		public static readonly DerObjectIdentifier Microsoft = new DerObjectIdentifier("1.3.6.1.4.1.311");

		public static readonly DerObjectIdentifier MicrosoftCertTemplateV1;

		public static readonly DerObjectIdentifier MicrosoftCAVersion;

		public static readonly DerObjectIdentifier MicrosoftPrevCACertHash;

		public static readonly DerObjectIdentifier MicrosoftCertTemplateV2;

		public static readonly DerObjectIdentifier MicrosoftAppPolicies;

		static MicrosoftObjectIdentifiers()
		{
			DerObjectIdentifier microsoft = Microsoft;
			MicrosoftCertTemplateV1 = new DerObjectIdentifier(((microsoft != null) ? microsoft.ToString() : null) + ".20.2");
			DerObjectIdentifier microsoft2 = Microsoft;
			MicrosoftCAVersion = new DerObjectIdentifier(((microsoft2 != null) ? microsoft2.ToString() : null) + ".21.1");
			DerObjectIdentifier microsoft3 = Microsoft;
			MicrosoftPrevCACertHash = new DerObjectIdentifier(((microsoft3 != null) ? microsoft3.ToString() : null) + ".21.2");
			DerObjectIdentifier microsoft4 = Microsoft;
			MicrosoftCertTemplateV2 = new DerObjectIdentifier(((microsoft4 != null) ? microsoft4.ToString() : null) + ".21.7");
			DerObjectIdentifier microsoft5 = Microsoft;
			MicrosoftAppPolicies = new DerObjectIdentifier(((microsoft5 != null) ? microsoft5.ToString() : null) + ".21.10");
		}
	}
}
