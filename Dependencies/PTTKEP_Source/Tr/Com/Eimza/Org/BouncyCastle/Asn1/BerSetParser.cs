namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1
{
	internal class BerSetParser : Asn1SetParser, IAsn1Convertible
	{
		private readonly Asn1StreamParser _parser;

		internal BerSetParser(Asn1StreamParser parser)
		{
			_parser = parser;
		}

		public IAsn1Convertible ReadObject()
		{
			return _parser.ReadObject();
		}

		public Asn1Object ToAsn1Object()
		{
			return new BerSet(_parser.ReadVector(), false);
		}
	}
}
