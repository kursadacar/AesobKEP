using System;
using Tr.Com.Eimza.Org.BouncyCastle.Math.EC.Abc;

namespace Tr.Com.Eimza.Org.BouncyCastle.Math.EC.Multiplier
{
	internal class WTauNafMultiplier : AbstractECMultiplier
	{
		internal static readonly string PRECOMP_NAME = "bc_wtnaf";

		protected override ECPoint MultiplyPositive(ECPoint point, BigInteger k)
		{
			if (!(point is F2mPoint))
			{
				throw new ArgumentException("Only F2mPoint can be used in WTauNafMultiplier");
			}
			F2mPoint f2mPoint = (F2mPoint)point;
			F2mCurve f2mCurve = (F2mCurve)f2mPoint.Curve;
			int m = f2mCurve.M;
			sbyte a = (sbyte)f2mCurve.A.ToBigInteger().IntValue;
			sbyte mu = f2mCurve.GetMu();
			BigInteger[] si = f2mCurve.GetSi();
			ZTauElement lambda = Tnaf.PartModReduction(k, m, a, si, mu, 10);
			return MultiplyWTnaf(f2mPoint, lambda, f2mCurve.GetPreCompInfo(f2mPoint, PRECOMP_NAME), a, mu);
		}

		private F2mPoint MultiplyWTnaf(F2mPoint p, ZTauElement lambda, PreCompInfo preCompInfo, sbyte a, sbyte mu)
		{
			ZTauElement[] alpha = ((a == 0) ? Tnaf.Alpha0 : Tnaf.Alpha1);
			BigInteger tw = Tnaf.GetTw(mu, 4);
			sbyte[] u = Tnaf.TauAdicWNaf(mu, lambda, 4, BigInteger.ValueOf(16L), tw, alpha);
			return MultiplyFromWTnaf(p, u, preCompInfo);
		}

		private static F2mPoint MultiplyFromWTnaf(F2mPoint p, sbyte[] u, PreCompInfo preCompInfo)
		{
			F2mCurve f2mCurve = (F2mCurve)p.Curve;
			sbyte a = (sbyte)f2mCurve.A.ToBigInteger().IntValue;
			F2mPoint[] preComp;
			if (preCompInfo == null || !(preCompInfo is WTauNafPreCompInfo))
			{
				preComp = Tnaf.GetPreComp(p, a);
				WTauNafPreCompInfo wTauNafPreCompInfo = new WTauNafPreCompInfo();
				wTauNafPreCompInfo.PreComp = preComp;
				f2mCurve.SetPreCompInfo(p, PRECOMP_NAME, wTauNafPreCompInfo);
			}
			else
			{
				preComp = ((WTauNafPreCompInfo)preCompInfo).PreComp;
			}
			F2mPoint f2mPoint = (F2mPoint)f2mCurve.Infinity;
			for (int num = u.Length - 1; num >= 0; num--)
			{
				f2mPoint = Tnaf.Tau(f2mPoint);
				sbyte b = u[num];
				if (b != 0)
				{
					f2mPoint = ((b <= 0) ? f2mPoint.SubtractSimple(preComp[-b]) : f2mPoint.AddSimple(preComp[b]));
				}
			}
			return f2mPoint;
		}
	}
}
