using System;
using Tr.Com.Eimza.Org.BouncyCastle.Math;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg
{
	internal class MPInteger : BcpgObject
	{
		private readonly BigInteger val;

		public BigInteger Value
		{
			get
			{
				return val;
			}
		}

		public MPInteger(BcpgInputStream bcpgIn)
		{
			if (bcpgIn == null)
			{
				throw new ArgumentNullException("bcpgIn");
			}
			byte[] array = new byte[(((bcpgIn.ReadByte() << 8) | bcpgIn.ReadByte()) + 7) / 8];
			bcpgIn.ReadFully(array);
			val = new BigInteger(1, array);
		}

		public MPInteger(BigInteger val)
		{
			if (val == null)
			{
				throw new ArgumentNullException("val");
			}
			if (val.SignValue < 0)
			{
				throw new ArgumentException("Values must be positive", "val");
			}
			this.val = val;
		}

		public override void Encode(BcpgOutputStream bcpgOut)
		{
			bcpgOut.WriteShort((short)val.BitLength);
			bcpgOut.Write(val.ToByteArrayUnsigned());
		}

		internal static void Encode(BcpgOutputStream bcpgOut, BigInteger val)
		{
			bcpgOut.WriteShort((short)val.BitLength);
			bcpgOut.Write(val.ToByteArrayUnsigned());
		}
	}
}
