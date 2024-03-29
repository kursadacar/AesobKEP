using System;
using System.Security.Cryptography;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Prng
{
	internal class CryptoApiRandomGenerator : IRandomGenerator
	{
		private readonly RandomNumberGenerator rndProv;

		public CryptoApiRandomGenerator()
			: this(new RNGCryptoServiceProvider())
		{
		}

		public CryptoApiRandomGenerator(RandomNumberGenerator rng)
		{
			rndProv = rng;
		}

		public virtual void AddSeedMaterial(byte[] seed)
		{
		}

		public virtual void AddSeedMaterial(long seed)
		{
		}

		public virtual void NextBytes(byte[] bytes)
		{
			rndProv.GetBytes(bytes);
		}

		public virtual void NextBytes(byte[] bytes, int start, int len)
		{
			if (start < 0)
			{
				throw new ArgumentException("Start offset cannot be negative", "start");
			}
			if (bytes.Length < start + len)
			{
				throw new ArgumentException("Byte array too small for requested offset and length");
			}
			if (bytes.Length == len && start == 0)
			{
				NextBytes(bytes);
				return;
			}
			byte[] array = new byte[len];
			NextBytes(array);
			Array.Copy(array, 0, bytes, start, len);
		}
	}
}
