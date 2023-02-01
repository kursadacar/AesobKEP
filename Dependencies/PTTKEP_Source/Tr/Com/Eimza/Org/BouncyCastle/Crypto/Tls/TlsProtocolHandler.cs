using System;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	[Obsolete("Use 'TlsClientProtocol' instead")]
	internal class TlsProtocolHandler : TlsClientProtocol
	{
		public TlsProtocolHandler(Stream stream, SecureRandom secureRandom)
			: base(stream, stream, secureRandom)
		{
		}

		public TlsProtocolHandler(Stream input, Stream output, SecureRandom secureRandom)
			: base(input, output, secureRandom)
		{
		}
	}
}
