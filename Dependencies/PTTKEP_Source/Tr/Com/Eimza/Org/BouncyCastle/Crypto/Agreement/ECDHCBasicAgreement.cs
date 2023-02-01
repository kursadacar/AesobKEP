using System;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Math.EC;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Agreement
{
	internal class ECDHCBasicAgreement : IBasicAgreement
	{
		private ECPrivateKeyParameters key;

		public virtual void Init(ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			key = (ECPrivateKeyParameters)parameters;
		}

		public virtual int GetFieldSize()
		{
			return (key.Parameters.Curve.FieldSize + 7) / 8;
		}

		public virtual BigInteger CalculateAgreement(ICipherParameters pubKey)
		{
			ECPublicKeyParameters obj = (ECPublicKeyParameters)pubKey;
			ECDomainParameters parameters = obj.Parameters;
			BigInteger b = parameters.H.Multiply(key.D).Mod(parameters.N);
			ECPoint eCPoint = obj.Q.Multiply(b).Normalize();
			if (eCPoint.IsInfinity)
			{
				throw new InvalidOperationException("Infinity is not a valid agreement value for ECDHC");
			}
			return eCPoint.AffineXCoord.ToBigInteger();
		}
	}
}
