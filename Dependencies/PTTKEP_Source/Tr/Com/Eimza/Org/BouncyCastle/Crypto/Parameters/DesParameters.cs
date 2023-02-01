using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters
{
	internal class DesParameters : KeyParameter
	{
		public const int DesKeyLength = 8;

		private const int N_DES_WEAK_KEYS = 16;

		private static readonly byte[] DES_weak_keys = new byte[128]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 31, 31,
			31, 31, 14, 14, 14, 14, 224, 224, 224, 224,
			241, 241, 241, 241, 254, 254, 254, 254, 254, 254,
			254, 254, 1, 254, 1, 254, 1, 254, 1, 254,
			31, 224, 31, 224, 14, 241, 14, 241, 1, 224,
			1, 224, 1, 241, 1, 241, 31, 254, 31, 254,
			14, 254, 14, 254, 1, 31, 1, 31, 1, 14,
			1, 14, 224, 254, 224, 254, 241, 254, 241, 254,
			254, 1, 254, 1, 254, 1, 254, 1, 224, 31,
			224, 31, 241, 14, 241, 14, 224, 1, 224, 1,
			241, 1, 241, 1, 254, 31, 254, 31, 254, 14,
			254, 14, 31, 1, 31, 1, 14, 1, 14, 1,
			254, 224, 254, 224, 254, 241, 254, 241
		};

		public DesParameters(byte[] key)
			: base(key)
		{
			if (IsWeakKey(key))
			{
				throw new ArgumentException("attempt to create weak DES key");
			}
		}

		public DesParameters(byte[] key, int keyOff, int keyLen)
			: base(key, keyOff, keyLen)
		{
			if (IsWeakKey(key, keyOff))
			{
				throw new ArgumentException("attempt to create weak DES key");
			}
		}

		public static bool IsWeakKey(byte[] key, int offset)
		{
			if (key.Length - offset < 8)
			{
				throw new ArgumentException("key material too short.");
			}
			for (int i = 0; i < 16; i++)
			{
				bool flag = false;
				for (int j = 0; j < 8; j++)
				{
					if (key[j + offset] != DES_weak_keys[i * 8 + j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsWeakKey(byte[] key)
		{
			return IsWeakKey(key, 0);
		}

		public static void SetOddParity(byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i++)
			{
				int num = bytes[i];
				bytes[i] = (byte)(((uint)num & 0xFEu) | (((uint)((num >> 1) ^ (num >> 2) ^ (num >> 3) ^ (num >> 4) ^ (num >> 5) ^ (num >> 6) ^ (num >> 7)) ^ 1u) & 1u));
			}
		}
	}
}
