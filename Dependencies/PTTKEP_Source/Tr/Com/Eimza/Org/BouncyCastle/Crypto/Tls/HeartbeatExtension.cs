using System;
using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class HeartbeatExtension
	{
		protected readonly byte mMode;

		public virtual byte Mode
		{
			get
			{
				return mMode;
			}
		}

		public HeartbeatExtension(byte mode)
		{
			if (!HeartbeatMode.IsValid(mode))
			{
				throw new ArgumentException("not a valid HeartbeatMode value", "mode");
			}
			mMode = mode;
		}

		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteUint8(mMode, output);
		}

		public static HeartbeatExtension Parse(Stream input)
		{
			byte num = TlsUtilities.ReadUint8(input);
			if (!HeartbeatMode.IsValid(num))
			{
				throw new TlsFatalAlert(47);
			}
			return new HeartbeatExtension(num);
		}
	}
}
