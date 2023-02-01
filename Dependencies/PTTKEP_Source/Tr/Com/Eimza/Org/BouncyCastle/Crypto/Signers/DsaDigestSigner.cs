using System;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Signers
{
	internal class DsaDigestSigner : ISigner
	{
		private readonly IDigest digest;

		private readonly IDsa dsaSigner;

		private bool forSigning;

		private byte[] currentSignature;

		public string AlgorithmName
		{
			get
			{
				return digest.AlgorithmName + "with" + dsaSigner.AlgorithmName;
			}
		}

		public DsaDigestSigner(IDsa signer, IDigest digest)
		{
			this.digest = digest;
			dsaSigner = signer;
		}

		public void Init(bool forSigning, ICipherParameters parameters)
		{
			this.forSigning = forSigning;
			AsymmetricKeyParameter asymmetricKeyParameter = ((!(parameters is ParametersWithRandom)) ? ((AsymmetricKeyParameter)parameters) : ((AsymmetricKeyParameter)((ParametersWithRandom)parameters).Parameters));
			if (forSigning && !asymmetricKeyParameter.IsPrivate)
			{
				throw new InvalidKeyException("Signing Requires Private Key.");
			}
			if (!forSigning && asymmetricKeyParameter.IsPrivate)
			{
				throw new InvalidKeyException("Verification Requires Public Key.");
			}
			Reset();
			dsaSigner.Init(forSigning, parameters);
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
				throw new InvalidOperationException("DSADigestSigner not initialised for signature generation.");
			}
			byte[] array = new byte[digest.GetDigestSize()];
			digest.DoFinal(array, 0);
			BigInteger[] array2 = dsaSigner.GenerateSignature(array);
			currentSignature = DerEncode(array2[0], array2[1]);
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
				throw new InvalidOperationException("DSADigestSigner not initialised for verification");
			}
			byte[] array = new byte[digest.GetDigestSize()];
			digest.DoFinal(array, 0);
			try
			{
				BigInteger[] array2 = DerDecode(signature);
				return dsaSigner.VerifySignature(array, array2[0], array2[1]);
			}
			catch (IOException)
			{
				return false;
			}
		}

		public void Reset()
		{
			currentSignature = null;
			digest.Reset();
		}

		private byte[] DerEncode(BigInteger r, BigInteger s)
		{
			return new DerSequence(new DerInteger(r), new DerInteger(s)).GetDerEncoded();
		}

		private BigInteger[] DerDecode(byte[] encoding)
		{
			Asn1Sequence asn1Sequence = (Asn1Sequence)Asn1Object.FromByteArray(encoding);
			return new BigInteger[2]
			{
				((DerInteger)asn1Sequence[0]).Value,
				((DerInteger)asn1Sequence[1]).Value
			};
		}
	}
}
