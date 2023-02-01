using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Engines
{
	internal class NullEngine : IBlockCipher
	{
		private bool initialised;

		private const int BlockSize = 1;

		public string AlgorithmName
		{
			get
			{
				return "Null";
			}
		}

		public bool IsPartialBlockOkay
		{
			get
			{
				return true;
			}
		}

		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			initialised = true;
		}

		public int GetBlockSize()
		{
			return 1;
		}

		public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (!initialised)
			{
				throw new InvalidOperationException("Null engine not initialised");
			}
			if (inOff + 1 > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			if (outOff + 1 > output.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			for (int i = 0; i < 1; i++)
			{
				output[outOff + i] = input[inOff + i];
			}
			return 1;
		}

		public void Reset()
		{
		}
	}
}
