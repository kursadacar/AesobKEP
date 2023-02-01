using System;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Digests;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Signers
{
	internal class PreComputedSigner : ISigner
	{
		private readonly IDigest digest;

		private byte[] currentSignature;

		public byte[] PreComputedSignature { get; set; }

		public string AlgorithmName { get; set; }

		public PreComputedSigner(byte[] preComputedSignature, string AlgorithmName = "NONE")
		{
			PreComputedSignature = preComputedSignature;
			digest = new NullDigest();
			this.AlgorithmName = AlgorithmName;
		}

		public PreComputedSigner()
			: this(new byte[0])
		{
		}

		public void Init(bool forSigning, ICipherParameters parameters)
		{
			Reset();
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
			if (PreComputedSignature.Length != 0)
			{
				currentSignature = PreComputedSignature;
				return PreComputedSignature;
			}
			byte[] output = new byte[digest.GetDigestSize()];
			digest.DoFinal(output, 0);
			currentSignature = output;
			return currentSignature;
		}

		public byte[] CurrentSignature()
		{
			return currentSignature;
		}

		public bool VerifySignature(byte[] signature)
		{
			throw new NotImplementedException();
		}

		public void Reset()
		{
			currentSignature = null;
			digest.Reset();
		}
	}
}
