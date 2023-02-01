using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters
{
	internal class ElGamalKeyGenerationParameters : KeyGenerationParameters
	{
		private readonly ElGamalParameters parameters;

		public ElGamalParameters Parameters
		{
			get
			{
				return parameters;
			}
		}

		public ElGamalKeyGenerationParameters(SecureRandom random, ElGamalParameters parameters)
			: base(random, GetStrength(parameters))
		{
			this.parameters = parameters;
		}

		internal static int GetStrength(ElGamalParameters parameters)
		{
			if (parameters.L == 0)
			{
				return parameters.P.BitLength;
			}
			return parameters.L;
		}
	}
}
