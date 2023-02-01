using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters
{
	internal class DHKeyGenerationParameters : KeyGenerationParameters
	{
		private readonly DHParameters parameters;

		public DHParameters Parameters
		{
			get
			{
				return parameters;
			}
		}

		public DHKeyGenerationParameters(SecureRandom random, DHParameters parameters)
			: base(random, GetStrength(parameters))
		{
			this.parameters = parameters;
		}

		internal static int GetStrength(DHParameters parameters)
		{
			if (parameters.L == 0)
			{
				return parameters.P.BitLength;
			}
			return parameters.L;
		}
	}
}
