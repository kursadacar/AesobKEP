using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Paddings
{
	internal class Pkcs7Padding : IBlockCipherPadding
	{
		public string PaddingName
		{
			get
			{
				return "PKCS7";
			}
		}

		public void Init(SecureRandom random)
		{
		}

		public int AddPadding(byte[] input, int inOff)
		{
			byte b = (byte)(input.Length - inOff);
			while (inOff < input.Length)
			{
				input[inOff] = b;
				inOff++;
			}
			return b;
		}

		public int PadCount(byte[] input)
		{
			int num = input[input.Length - 1];
			if (num < 1 || num > input.Length)
			{
				throw new InvalidCipherTextException("pad block corrupted");
			}
			for (int i = 1; i <= num; i++)
			{
				if (input[input.Length - i] != num)
				{
					throw new InvalidCipherTextException("pad block corrupted");
				}
			}
			return num;
		}
	}
}
