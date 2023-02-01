using System;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Modes;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Macs
{
	internal class GMac : IMac
	{
		private readonly GcmBlockCipher cipher;

		private readonly int macSizeBits;

		public string AlgorithmName
		{
			get
			{
				return cipher.GetUnderlyingCipher().AlgorithmName + "-GMAC";
			}
		}

		public GMac(GcmBlockCipher cipher)
			: this(cipher, 128)
		{
		}

		public GMac(GcmBlockCipher cipher, int macSizeBits)
		{
			this.cipher = cipher;
			this.macSizeBits = macSizeBits;
		}

		public void Init(ICipherParameters parameters)
		{
			if (parameters is ParametersWithIV)
			{
				ParametersWithIV obj = (ParametersWithIV)parameters;
				byte[] iV = obj.GetIV();
				KeyParameter key = (KeyParameter)obj.Parameters;
				cipher.Init(true, new AeadParameters(key, macSizeBits, iV));
				return;
			}
			throw new ArgumentException("GMAC requires ParametersWithIV");
		}

		public int GetMacSize()
		{
			return macSizeBits / 8;
		}

		public void Update(byte input)
		{
			cipher.ProcessAadByte(input);
		}

		public void BlockUpdate(byte[] input, int inOff, int len)
		{
			cipher.ProcessAadBytes(input, inOff, len);
		}

		public int DoFinal(byte[] output, int outOff)
		{
			try
			{
				return cipher.DoFinal(output, outOff);
			}
			catch (InvalidCipherTextException ex)
			{
				throw new InvalidOperationException(ex.ToString());
			}
		}

		public void Reset()
		{
			cipher.Reset();
		}
	}
}
