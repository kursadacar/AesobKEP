using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509
{
	internal class KeyUsage : DerBitString
	{
		public const int DigitalSignature = 128;

		public const int NonRepudiation = 64;

		public const int KeyEncipherment = 32;

		public const int DataEncipherment = 16;

		public const int KeyAgreement = 8;

		public const int KeyCertSign = 4;

		public const int CrlSign = 2;

		public const int EncipherOnly = 1;

		public const int DecipherOnly = 32768;

		public static readonly DerObjectIdentifier KeyUsageID = new DerObjectIdentifier("2.5.29.15");

		public bool IsDigitalSignature
		{
			get
			{
				try
				{
					if (KeyUsageList != null && KeyUsageList.Length != 0)
					{
						return KeyUsageList[0];
					}
				}
				catch (Exception)
				{
					return false;
				}
				return false;
			}
		}

		public bool IsKeyEncipherment
		{
			get
			{
				try
				{
					if (KeyUsageList != null && KeyUsageList.Length != 0)
					{
						return KeyUsageList[2];
					}
				}
				catch (Exception)
				{
					return false;
				}
				return false;
			}
		}

		public bool IsDataEncipherment
		{
			get
			{
				try
				{
					if (KeyUsageList != null && KeyUsageList.Length != 0)
					{
						return KeyUsageList[3];
					}
				}
				catch (Exception)
				{
					return false;
				}
				return false;
			}
		}

		public bool[] KeyUsageList
		{
			get
			{
				byte[] bytes = GetBytes();
				int num = bytes.Length * 8 - base.PadBits;
				bool[] array = new bool[(num < 9) ? 9 : num];
				for (int i = 0; i != num; i++)
				{
					array[i] = (bytes[i / 8] & (128 >> i % 8)) != 0;
				}
				return array;
			}
		}

		public new static KeyUsage GetInstance(object obj)
		{
			if (obj is KeyUsage)
			{
				return (KeyUsage)obj;
			}
			if (obj is X509Extension)
			{
				return GetInstance(X509Extension.ConvertValueToObject((X509Extension)obj));
			}
			return new KeyUsage(DerBitString.GetInstance(obj));
		}

		public KeyUsage(int usage)
			: base(DerBitString.GetBytes(usage), DerBitString.GetPadBits(usage))
		{
		}

		private KeyUsage(DerBitString usage)
			: base(usage.GetBytes(), usage.PadBits)
		{
		}

		public override string ToString()
		{
			byte[] bytes = GetBytes();
			if (bytes.Length == 1)
			{
				return "KeyUsage: 0x" + (bytes[0] & 0xFF).ToString("X");
			}
			return "KeyUsage: 0x" + (((bytes[1] & 0xFF) << 8) | (bytes[0] & 0xFF)).ToString("X");
		}
	}
}
