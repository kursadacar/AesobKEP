using System;
using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1
{
	internal class DerOctetStringParser : Asn1OctetStringParser, IAsn1Convertible
	{
		private readonly DefiniteLengthInputStream stream;

		internal DerOctetStringParser(DefiniteLengthInputStream stream)
		{
			this.stream = stream;
		}

		public Stream GetOctetStream()
		{
			return stream;
		}

		public Asn1Object ToAsn1Object()
		{
			try
			{
				return new DerOctetString(stream.ToArray());
			}
			catch (IOException ex)
			{
				throw new InvalidOperationException("IOException converting stream to byte array: " + ex.Message, ex);
			}
		}
	}
}
