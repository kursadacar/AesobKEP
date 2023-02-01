using System;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Date;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.Sig
{
	internal class SignatureCreationTime : SignatureSubpacket
	{
		protected static byte[] TimeToBytes(DateTime time)
		{
			long num = DateTimeUtilities.DateTimeToUnixMs(time) / 1000;
			return new byte[4]
			{
				(byte)(num >> 24),
				(byte)(num >> 16),
				(byte)(num >> 8),
				(byte)num
			};
		}

		public SignatureCreationTime(bool critical, byte[] data)
			: base(SignatureSubpacketTag.CreationTime, critical, data)
		{
		}

		public SignatureCreationTime(bool critical, DateTime date)
			: base(SignatureSubpacketTag.CreationTime, critical, TimeToBytes(date))
		{
		}

		public DateTime GetTime()
		{
			return DateTimeUtilities.UnixMsToDateTime((long)(uint)((data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3]) * 1000L);
		}
	}
}
