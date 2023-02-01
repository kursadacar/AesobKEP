using System;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Math.EC;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Agreement
{
	internal class ECMqvBasicAgreement : IBasicAgreement
	{
		protected internal MqvPrivateParameters privParams;

		public virtual void Init(ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			privParams = (MqvPrivateParameters)parameters;
		}

		public virtual int GetFieldSize()
		{
			return (privParams.StaticPrivateKey.Parameters.Curve.FieldSize + 7) / 8;
		}

		public virtual BigInteger CalculateAgreement(ICipherParameters pubKey)
		{
			MqvPublicParameters mqvPublicParameters = (MqvPublicParameters)pubKey;
			ECPrivateKeyParameters staticPrivateKey = privParams.StaticPrivateKey;
			ECPoint eCPoint = CalculateMqvAgreement(staticPrivateKey.Parameters, staticPrivateKey, privParams.EphemeralPrivateKey, privParams.EphemeralPublicKey, mqvPublicParameters.StaticPublicKey, mqvPublicParameters.EphemeralPublicKey).Normalize();
			if (eCPoint.IsInfinity)
			{
				throw new InvalidOperationException("Infinity is not a valid agreement value for MQV");
			}
			return eCPoint.AffineXCoord.ToBigInteger();
		}

		private static ECPoint CalculateMqvAgreement(ECDomainParameters parameters, ECPrivateKeyParameters d1U, ECPrivateKeyParameters d2U, ECPublicKeyParameters Q2U, ECPublicKeyParameters Q1V, ECPublicKeyParameters Q2V)
		{
			BigInteger n = parameters.N;
			int num = (n.BitLength + 1) / 2;
			BigInteger m = BigInteger.One.ShiftLeft(num);
			ECCurve curve = parameters.Curve;
			ECPoint[] array = new ECPoint[3]
			{
				ECAlgorithms.ImportPoint(curve, (Q2U == null) ? parameters.G.Multiply(d2U.D) : Q2U.Q),
				ECAlgorithms.ImportPoint(curve, Q1V.Q),
				ECAlgorithms.ImportPoint(curve, Q2V.Q)
			};
			curve.NormalizeAll(array);
			ECPoint eCPoint = array[0];
			ECPoint p = array[1];
			ECPoint eCPoint2 = array[2];
			BigInteger val = eCPoint.AffineXCoord.ToBigInteger().Mod(m).SetBit(num);
			BigInteger val2 = d1U.D.Multiply(val).Add(d2U.D).Mod(n);
			BigInteger bigInteger = eCPoint2.AffineXCoord.ToBigInteger().Mod(m).SetBit(num);
			BigInteger bigInteger2 = parameters.H.Multiply(val2).Mod(n);
			return ECAlgorithms.SumOfTwoMultiplies(p, bigInteger.Multiply(bigInteger2).Mod(n), eCPoint2, bigInteger2);
		}
	}
}
