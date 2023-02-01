using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1
{
	internal interface Asn1OctetStringParser : IAsn1Convertible
	{
		Stream GetOctetStream();
	}
}
