using System;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters
{
	internal class ParametersWithRandom : ICipherParameters
	{
		private readonly ICipherParameters parameters;

		private readonly SecureRandom random;

		public SecureRandom Random
		{
			get
			{
				return random;
			}
		}

		public ICipherParameters Parameters
		{
			get
			{
				return parameters;
			}
		}

		public ParametersWithRandom(ICipherParameters parameters, SecureRandom random)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("random");
			}
			if (random == null)
			{
				throw new ArgumentNullException("random");
			}
			this.parameters = parameters;
			this.random = random;
		}

		public ParametersWithRandom(ICipherParameters parameters)
			: this(parameters, new SecureRandom())
		{
		}

		[Obsolete("Use Random property instead")]
		public SecureRandom GetRandom()
		{
			return Random;
		}
	}
}
