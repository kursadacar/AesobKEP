using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1
{
	internal class BerSetGenerator : BerGenerator
	{
		public BerSetGenerator(Stream outStream)
			: base(outStream)
		{
			WriteBerHeader(49);
		}

		public BerSetGenerator(Stream outStream, int tagNo, bool isExplicit)
			: base(outStream, tagNo, isExplicit)
		{
			WriteBerHeader(49);
		}
	}
}
