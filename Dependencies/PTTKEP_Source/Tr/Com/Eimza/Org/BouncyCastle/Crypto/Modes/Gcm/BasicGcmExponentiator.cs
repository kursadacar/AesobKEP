using System;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Modes.Gcm
{
	internal class BasicGcmExponentiator : IGcmExponentiator
	{
		private byte[] x;

		public void Init(byte[] x)
		{
			this.x = Arrays.Clone(x);
		}

		public void ExponentiateX(long pow, byte[] output)
		{
			byte[] array = GcmUtilities.OneAsBytes();
			if (pow > 0)
			{
				byte[] array2 = Arrays.Clone(x);
				do
				{
					if ((pow & 1) != 0L)
					{
						GcmUtilities.Multiply(array, array2);
					}
					GcmUtilities.Multiply(array2, array2);
					pow >>= 1;
				}
				while (pow > 0);
			}
			Array.Copy(array, 0, output, 0, 16);
		}
	}
}
