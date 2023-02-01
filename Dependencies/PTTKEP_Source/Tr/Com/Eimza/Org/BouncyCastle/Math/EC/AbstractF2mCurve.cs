using System;
using Tr.Com.Eimza.Org.BouncyCastle.Math.Field;

namespace Tr.Com.Eimza.Org.BouncyCastle.Math.EC
{
	internal abstract class AbstractF2mCurve : ECCurve
	{
		private static IFiniteField BuildField(int m, int k1, int k2, int k3)
		{
			if (k1 == 0)
			{
				throw new ArgumentException("k1 must be > 0");
			}
			if (k2 == 0)
			{
				if (k3 != 0)
				{
					throw new ArgumentException("k3 must be 0 if k2 == 0");
				}
				return FiniteFields.GetBinaryExtensionField(new int[3] { 0, k1, m });
			}
			if (k2 <= k1)
			{
				throw new ArgumentException("k2 must be > k1");
			}
			if (k3 <= k2)
			{
				throw new ArgumentException("k3 must be > k2");
			}
			return FiniteFields.GetBinaryExtensionField(new int[5] { 0, k1, k2, k3, m });
		}

		protected AbstractF2mCurve(int m, int k1, int k2, int k3)
			: base(BuildField(m, k1, k2, k3))
		{
		}
	}
}
