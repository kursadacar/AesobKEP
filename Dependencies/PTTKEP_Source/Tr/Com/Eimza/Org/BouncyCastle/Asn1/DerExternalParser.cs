namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1
{
	internal class DerExternalParser : Asn1Encodable
	{
		private readonly Asn1StreamParser _parser;

		public DerExternalParser(Asn1StreamParser parser)
		{
			_parser = parser;
		}

		public IAsn1Convertible ReadObject()
		{
			return _parser.ReadObject();
		}

		public override Asn1Object ToAsn1Object()
		{
			return new DerExternal(_parser.ReadVector());
		}
	}
}
