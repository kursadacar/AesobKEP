namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters
{
	internal class MqvPublicParameters : ICipherParameters
	{
		private readonly ECPublicKeyParameters staticPublicKey;

		private readonly ECPublicKeyParameters ephemeralPublicKey;

		public ECPublicKeyParameters StaticPublicKey
		{
			get
			{
				return staticPublicKey;
			}
		}

		public ECPublicKeyParameters EphemeralPublicKey
		{
			get
			{
				return ephemeralPublicKey;
			}
		}

		public MqvPublicParameters(ECPublicKeyParameters staticPublicKey, ECPublicKeyParameters ephemeralPublicKey)
		{
			this.staticPublicKey = staticPublicKey;
			this.ephemeralPublicKey = ephemeralPublicKey;
		}
	}
}
