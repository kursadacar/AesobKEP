using System;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Signers
{
	internal class GenericSigner : ISigner
	{
		private readonly IAsymmetricBlockCipher engine;

		private readonly IDigest digest;

		private bool forSigning;

		private byte[] currentSignature;

		public string AlgorithmName
		{
			get
			{
				return "Generic(" + engine.AlgorithmName + "/" + digest.AlgorithmName + ")";
			}
		}

		public GenericSigner(IAsymmetricBlockCipher engine, IDigest digest)
		{
			this.engine = engine;
			this.digest = digest;
		}

		public void Init(bool forSigning, ICipherParameters parameters)
		{
			this.forSigning = forSigning;
			AsymmetricKeyParameter asymmetricKeyParameter = ((!(parameters is ParametersWithRandom)) ? ((AsymmetricKeyParameter)parameters) : ((AsymmetricKeyParameter)((ParametersWithRandom)parameters).Parameters));
			if (forSigning && !asymmetricKeyParameter.IsPrivate)
			{
				throw new InvalidKeyException("Signing requires private key.");
			}
			if (!forSigning && asymmetricKeyParameter.IsPrivate)
			{
				throw new InvalidKeyException("Verification requires public key.");
			}
			Reset();
			engine.Init(forSigning, parameters);
		}

		public void Update(byte input)
		{
			digest.Update(input);
		}

		public void BlockUpdate(byte[] input, int inOff, int length)
		{
			digest.BlockUpdate(input, inOff, length);
		}

		public byte[] GenerateSignature()
		{
			if (!forSigning)
			{
				throw new InvalidOperationException("GenericSigner not initialised for signature generation.");
			}
			byte[] array = new byte[digest.GetDigestSize()];
			digest.DoFinal(array, 0);
			currentSignature = engine.ProcessBlock(array, 0, array.Length);
			return currentSignature;
		}

		public byte[] CurrentSignature()
		{
			return currentSignature;
		}

		public bool VerifySignature(byte[] signature)
		{
			if (forSigning)
			{
				throw new InvalidOperationException("GenericSigner not initialised for verification");
			}
			byte[] array = new byte[digest.GetDigestSize()];
			digest.DoFinal(array, 0);
			try
			{
				byte[] array2 = engine.ProcessBlock(signature, 0, signature.Length);
				if (array2.Length < array.Length)
				{
					byte[] array3 = new byte[array.Length];
					Array.Copy(array2, 0, array3, array3.Length - array2.Length, array2.Length);
					array2 = array3;
				}
				return Arrays.ConstantTimeAreEqual(array2, array);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void Reset()
		{
			digest.Reset();
		}
	}
}
