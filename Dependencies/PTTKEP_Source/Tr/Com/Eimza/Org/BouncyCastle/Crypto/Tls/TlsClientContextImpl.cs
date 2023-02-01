using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class TlsClientContextImpl : AbstractTlsContext, TlsClientContext, TlsContext
	{
		public override bool IsServer
		{
			get
			{
				return false;
			}
		}

		internal TlsClientContextImpl(SecureRandom secureRandom, SecurityParameters securityParameters)
			: base(secureRandom, securityParameters)
		{
		}
	}
}
