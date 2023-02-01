using System.Collections.Generic;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Sec;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X9;

namespace Tr.Com.Eimza.Org.BouncyCastle.Tools
{
	internal class BouncyProviderUtil
	{
		private static readonly Dictionary<int, DerObjectIdentifier> curveOidDic;

		static BouncyProviderUtil()
		{
			curveOidDic = new Dictionary<int, DerObjectIdentifier>();
			curveOidDic[163] = X9ObjectIdentifiers.C2Pnb163v3;
			curveOidDic[176] = X9ObjectIdentifiers.C2Pnb176w1;
			curveOidDic[191] = X9ObjectIdentifiers.C2Tnb191v3;
			curveOidDic[208] = X9ObjectIdentifiers.C2Pnb208w1;
			curveOidDic[239] = X9ObjectIdentifiers.C2Tnb239v3;
			curveOidDic[272] = X9ObjectIdentifiers.C2Pnb272w1;
			curveOidDic[359] = X9ObjectIdentifiers.C2Tnb359v1;
			curveOidDic[368] = X9ObjectIdentifiers.C2Pnb368w1;
			curveOidDic[256] = X9ObjectIdentifiers.Prime256v1;
			curveOidDic[384] = SecObjectIdentifiers.SecP384r1;
		}

		public static DerObjectIdentifier GetCurveOID(int aCurveLength)
		{
			DerObjectIdentifier value = null;
			if (curveOidDic.TryGetValue(aCurveLength, out value))
			{
				return value;
			}
			return null;
		}
	}
}
