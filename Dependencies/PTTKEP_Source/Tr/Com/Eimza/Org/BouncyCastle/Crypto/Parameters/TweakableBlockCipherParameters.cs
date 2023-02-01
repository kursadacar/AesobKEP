using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters
{
	internal class TweakableBlockCipherParameters : ICipherParameters
	{
		private readonly byte[] tweak;

		private readonly KeyParameter key;

		public KeyParameter Key
		{
			get
			{
				return key;
			}
		}

		public byte[] Tweak
		{
			get
			{
				return tweak;
			}
		}

		public TweakableBlockCipherParameters(KeyParameter key, byte[] tweak)
		{
			this.key = key;
			this.tweak = Arrays.Clone(tweak);
		}
	}
}
