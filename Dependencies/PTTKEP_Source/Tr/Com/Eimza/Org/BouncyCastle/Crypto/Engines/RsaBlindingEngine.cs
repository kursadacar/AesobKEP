using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Engines
{
	internal class RsaBlindingEngine : IAsymmetricBlockCipher
	{
		private readonly RsaCoreEngine core = new RsaCoreEngine();

		private RsaKeyParameters key;

		private BigInteger blindingFactor;

		private bool forEncryption;

		public string AlgorithmName
		{
			get
			{
				return "RSA";
			}
		}

		public void Init(bool forEncryption, ICipherParameters param)
		{
			RsaBlindingParameters rsaBlindingParameters = ((!(param is ParametersWithRandom)) ? ((RsaBlindingParameters)param) : ((RsaBlindingParameters)((ParametersWithRandom)param).Parameters));
			core.Init(forEncryption, rsaBlindingParameters.PublicKey);
			this.forEncryption = forEncryption;
			key = rsaBlindingParameters.PublicKey;
			blindingFactor = rsaBlindingParameters.BlindingFactor;
		}

		public int GetInputBlockSize()
		{
			return core.GetInputBlockSize();
		}

		public int GetOutputBlockSize()
		{
			return core.GetOutputBlockSize();
		}

		public byte[] ProcessBlock(byte[] inBuf, int inOff, int inLen)
		{
			BigInteger bigInteger = core.ConvertInput(inBuf, inOff, inLen);
			bigInteger = ((!forEncryption) ? UnblindMessage(bigInteger) : BlindMessage(bigInteger));
			return core.ConvertOutput(bigInteger);
		}

		private BigInteger BlindMessage(BigInteger msg)
		{
			BigInteger bigInteger = blindingFactor;
			bigInteger = msg.Multiply(bigInteger.ModPow(key.Exponent, key.Modulus));
			return bigInteger.Mod(key.Modulus);
		}

		private BigInteger UnblindMessage(BigInteger blindedMsg)
		{
			BigInteger modulus = key.Modulus;
			BigInteger val = blindingFactor.ModInverse(modulus);
			return blindedMsg.Multiply(val).Mod(modulus);
		}
	}
}
