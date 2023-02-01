using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Math.EC;
using Tr.Com.Eimza.Org.BouncyCastle.Math.EC.Multiplier;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Signers
{
	internal class ECGost3410Signer : IDsa
	{
		private ECKeyParameters key;

		private SecureRandom random;

		public string AlgorithmName
		{
			get
			{
				return "ECGOST3410";
			}
		}

		public void Init(bool forSigning, ICipherParameters parameters)
		{
			if (forSigning)
			{
				if (parameters is ParametersWithRandom)
				{
					ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
					random = parametersWithRandom.Random;
					parameters = parametersWithRandom.Parameters;
				}
				else
				{
					random = new SecureRandom();
				}
				if (!(parameters is ECPrivateKeyParameters))
				{
					throw new InvalidKeyException("EC private key required for signing");
				}
				key = (ECPrivateKeyParameters)parameters;
			}
			else
			{
				if (!(parameters is ECPublicKeyParameters))
				{
					throw new InvalidKeyException("EC public key required for verification");
				}
				key = (ECPublicKeyParameters)parameters;
			}
		}

		public BigInteger[] GenerateSignature(byte[] message)
		{
			byte[] array = new byte[message.Length];
			for (int i = 0; i != array.Length; i++)
			{
				array[i] = message[array.Length - 1 - i];
			}
			BigInteger val = new BigInteger(1, array);
			ECDomainParameters parameters = key.Parameters;
			BigInteger n = parameters.N;
			BigInteger d = ((ECPrivateKeyParameters)key).D;
			BigInteger bigInteger = null;
			ECMultiplier eCMultiplier = CreateBasePointMultiplier();
			BigInteger bigInteger3;
			while (true)
			{
				BigInteger bigInteger2 = new BigInteger(n.BitLength, random);
				if (bigInteger2.SignValue == 0)
				{
					continue;
				}
				bigInteger3 = eCMultiplier.Multiply(parameters.G, bigInteger2).Normalize().AffineXCoord.ToBigInteger().Mod(n);
				if (bigInteger3.SignValue != 0)
				{
					bigInteger = bigInteger2.Multiply(val).Add(d.Multiply(bigInteger3)).Mod(n);
					if (bigInteger.SignValue != 0)
					{
						break;
					}
				}
			}
			return new BigInteger[2] { bigInteger3, bigInteger };
		}

		public bool VerifySignature(byte[] message, BigInteger r, BigInteger s)
		{
			byte[] array = new byte[message.Length];
			for (int i = 0; i != array.Length; i++)
			{
				array[i] = message[array.Length - 1 - i];
			}
			BigInteger bigInteger = new BigInteger(1, array);
			BigInteger n = key.Parameters.N;
			if (r.CompareTo(BigInteger.One) < 0 || r.CompareTo(n) >= 0)
			{
				return false;
			}
			if (s.CompareTo(BigInteger.One) < 0 || s.CompareTo(n) >= 0)
			{
				return false;
			}
			BigInteger val = bigInteger.ModInverse(n);
			BigInteger a = s.Multiply(val).Mod(n);
			BigInteger b = n.Subtract(r).Multiply(val).Mod(n);
			ECPoint g = key.Parameters.G;
			ECPoint q = ((ECPublicKeyParameters)key).Q;
			ECPoint eCPoint = ECAlgorithms.SumOfTwoMultiplies(g, a, q, b).Normalize();
			if (eCPoint.IsInfinity)
			{
				return false;
			}
			return eCPoint.AffineXCoord.ToBigInteger().Mod(n).Equals(r);
		}

		protected virtual ECMultiplier CreateBasePointMultiplier()
		{
			return new FixedPointCombMultiplier();
		}
	}
}
