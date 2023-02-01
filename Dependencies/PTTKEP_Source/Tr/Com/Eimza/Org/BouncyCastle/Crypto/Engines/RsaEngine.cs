using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Engines
{
	internal class RsaEngine : IAsymmetricBlockCipher
	{
		private RsaCoreEngine core;

		public string AlgorithmName
		{
			get
			{
				return "RSA";
			}
		}

		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (core == null)
			{
				core = new RsaCoreEngine();
			}
			core.Init(forEncryption, parameters);
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
			if (core == null)
			{
				throw new InvalidOperationException("RSA engine not initialised");
			}
			return core.ConvertOutput(core.ProcessBlock(core.ConvertInput(inBuf, inOff, inLen)));
		}
	}
}
