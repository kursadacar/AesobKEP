using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Prng;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal interface TlsContext
	{
		IRandomGenerator NonceRandomGenerator { get; }

		SecureRandom SecureRandom { get; }

		SecurityParameters SecurityParameters { get; }

		bool IsServer { get; }

		ProtocolVersion ClientVersion { get; }

		ProtocolVersion ServerVersion { get; }

		TlsSession ResumableSession { get; }

		object UserObject { get; set; }

		byte[] ExportKeyingMaterial(string asciiLabel, byte[] context_value, int length);
	}
}
