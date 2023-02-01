using System;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto
{
	internal class CipherKeyGenerator
	{
		protected internal SecureRandom random;

		protected internal int strength;

		private bool uninitialised = true;

		private int defaultStrength;

		public int DefaultStrength
		{
			get
			{
				return defaultStrength;
			}
		}

		public CipherKeyGenerator()
		{
		}

		internal CipherKeyGenerator(int defaultStrength)
		{
			if (defaultStrength < 1)
			{
				throw new ArgumentException("strength must be a positive value", "defaultStrength");
			}
			this.defaultStrength = defaultStrength;
		}

		public void Init(KeyGenerationParameters parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			uninitialised = false;
			engineInit(parameters);
		}

		protected virtual void engineInit(KeyGenerationParameters parameters)
		{
			random = parameters.Random;
			strength = (parameters.Strength + 7) / 8;
		}

		public byte[] GenerateKey()
		{
			if (uninitialised)
			{
				if (defaultStrength < 1)
				{
					throw new InvalidOperationException("Generator has not been initialised");
				}
				uninitialised = false;
				engineInit(new KeyGenerationParameters(new SecureRandom(), defaultStrength));
			}
			return engineGenerateKey();
		}

		protected virtual byte[] engineGenerateKey()
		{
			return random.GenerateSeed(strength);
		}
	}
}
