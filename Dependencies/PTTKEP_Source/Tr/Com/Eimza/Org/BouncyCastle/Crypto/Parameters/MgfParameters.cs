using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters
{
	internal class MgfParameters : IDerivationParameters
	{
		private readonly byte[] seed;

		public MgfParameters(byte[] seed)
			: this(seed, 0, seed.Length)
		{
		}

		public MgfParameters(byte[] seed, int off, int len)
		{
			this.seed = new byte[len];
			Array.Copy(seed, off, this.seed, 0, len);
		}

		public byte[] GetSeed()
		{
			return (byte[])seed.Clone();
		}
	}
}
