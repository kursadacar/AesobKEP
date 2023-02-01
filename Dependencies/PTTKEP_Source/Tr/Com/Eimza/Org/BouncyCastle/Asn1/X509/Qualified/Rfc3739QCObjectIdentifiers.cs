namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509.Qualified
{
	internal sealed class Rfc3739QCObjectIdentifiers
	{
		public static readonly DerObjectIdentifier IdQcs = new DerObjectIdentifier("1.3.6.1.5.5.7.11");

		public static readonly DerObjectIdentifier IdQcsPkixQCSyntaxV1;

		public static readonly DerObjectIdentifier IdQcsPkixQCSyntaxV2;

		private Rfc3739QCObjectIdentifiers()
		{
		}

		static Rfc3739QCObjectIdentifiers()
		{
			DerObjectIdentifier idQcs = IdQcs;
			IdQcsPkixQCSyntaxV1 = new DerObjectIdentifier(((idQcs != null) ? idQcs.ToString() : null) + ".1");
			DerObjectIdentifier idQcs2 = IdQcs;
			IdQcsPkixQCSyntaxV2 = new DerObjectIdentifier(((idQcs2 != null) ? idQcs2.ToString() : null) + ".2");
		}
	}
}
