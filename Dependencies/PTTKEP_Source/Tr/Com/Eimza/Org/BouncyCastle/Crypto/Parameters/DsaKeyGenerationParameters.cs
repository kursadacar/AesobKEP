using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters
{
	internal class DsaKeyGenerationParameters : KeyGenerationParameters
	{
		private readonly DsaParameters parameters;

		public DsaParameters Parameters
		{
			get
			{
				return parameters;
			}
		}

		public DsaKeyGenerationParameters(SecureRandom random, DsaParameters parameters)
			: base(random, parameters.P.BitLength - 1)
		{
			this.parameters = parameters;
		}
	}
}
