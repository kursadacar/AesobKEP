using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Modes.Gcm
{
	internal class BasicGcmMultiplier : IGcmMultiplier
	{
		private byte[] H;

		public void Init(byte[] H)
		{
			this.H = Arrays.Clone(H);
		}

		public void MultiplyH(byte[] x)
		{
			GcmUtilities.Multiply(x, H);
		}
	}
}
