using System;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Math.EC;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Agreement
{
	internal class ECDHBasicAgreement : IBasicAgreement
	{
		protected internal ECPrivateKeyParameters privKey;

		public virtual void Init(ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			privKey = (ECPrivateKeyParameters)parameters;
		}

		public virtual int GetFieldSize()
		{
			return (privKey.Parameters.Curve.FieldSize + 7) / 8;
		}

		public virtual BigInteger CalculateAgreement(ICipherParameters pubKey)
		{
			ECPoint eCPoint = ((ECPublicKeyParameters)pubKey).Q.Multiply(privKey.D).Normalize();
			if (eCPoint.IsInfinity)
			{
				throw new InvalidOperationException("Infinity is not a valid agreement value for ECDH");
			}
			return eCPoint.AffineXCoord.ToBigInteger();
		}
	}
}
