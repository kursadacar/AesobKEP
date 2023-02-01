using System;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto
{
	internal class KeyGenerationParameters
	{
		private SecureRandom random;

		private int strength;

		public SecureRandom Random
		{
			get
			{
				return random;
			}
		}

		public int Strength
		{
			get
			{
				return strength;
			}
		}

		public KeyGenerationParameters(SecureRandom random, int strength)
		{
			if (random == null)
			{
				throw new ArgumentNullException("random");
			}
			if (strength < 1)
			{
				throw new ArgumentException("strength must be a positive value", "strength");
			}
			this.random = random;
			this.strength = strength;
		}
	}
}
