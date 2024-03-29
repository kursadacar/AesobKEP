using System;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Digests;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Macs
{
	internal class SkeinMac : IMac
	{
		public const int SKEIN_256 = 256;

		public const int SKEIN_512 = 512;

		public const int SKEIN_1024 = 1024;

		private readonly SkeinEngine engine;

		public string AlgorithmName
		{
			get
			{
				return "Skein-MAC-" + engine.BlockSize * 8 + "-" + engine.OutputSize * 8;
			}
		}

		public SkeinMac(int stateSizeBits, int digestSizeBits)
		{
			engine = new SkeinEngine(stateSizeBits, digestSizeBits);
		}

		public SkeinMac(SkeinMac mac)
		{
			engine = new SkeinEngine(mac.engine);
		}

		public void Init(ICipherParameters parameters)
		{
			SkeinParameters skeinParameters;
			if (parameters is SkeinParameters)
			{
				skeinParameters = (SkeinParameters)parameters;
			}
			else
			{
				if (!(parameters is KeyParameter))
				{
					throw new ArgumentException("Invalid parameter passed to Skein MAC init - " + parameters.GetType().Name);
				}
				skeinParameters = new SkeinParameters.Builder().SetKey(((KeyParameter)parameters).GetKey()).Build();
			}
			if (skeinParameters.GetKey() == null)
			{
				throw new ArgumentException("Skein MAC requires a key parameter.");
			}
			engine.Init(skeinParameters);
		}

		public int GetMacSize()
		{
			return engine.OutputSize;
		}

		public void Reset()
		{
			engine.Reset();
		}

		public void Update(byte inByte)
		{
			engine.Update(inByte);
		}

		public void BlockUpdate(byte[] input, int inOff, int len)
		{
			engine.Update(input, inOff, len);
		}

		public int DoFinal(byte[] output, int outOff)
		{
			return engine.DoFinal(output, outOff);
		}
	}
}
