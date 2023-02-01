namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.Sig
{
	internal class KeyExpirationTime : SignatureSubpacket
	{
		public long Time
		{
			get
			{
				return ((long)(data[0] & 0xFF) << 24) | ((long)(data[1] & 0xFF) << 16) | ((long)(data[2] & 0xFF) << 8) | ((long)data[3] & 0xFFL);
			}
		}

		protected static byte[] TimeToBytes(long t)
		{
			return new byte[4]
			{
				(byte)(t >> 24),
				(byte)(t >> 16),
				(byte)(t >> 8),
				(byte)t
			};
		}

		public KeyExpirationTime(bool critical, byte[] data)
			: base(SignatureSubpacketTag.KeyExpireTime, critical, data)
		{
		}

		public KeyExpirationTime(bool critical, long seconds)
			: base(SignatureSubpacketTag.KeyExpireTime, critical, TimeToBytes(seconds))
		{
		}
	}
}
