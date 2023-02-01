using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H
{
    internal class SessionFlags
	{
		private Tr.Com.Eimza.Pkcs11.H4.SessionFlags _sessionFlags4;

		private Tr.Com.Eimza.Pkcs11.H8.SessionFlags _sessionFlags8;

		public ulong Flags
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _sessionFlags8.Flags;
				}
				return _sessionFlags4.Flags;
			}
		}

		public bool RwSession
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _sessionFlags8.RwSession;
				}
				return _sessionFlags4.RwSession;
			}
		}

		public bool SerialSession
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _sessionFlags8.SerialSession;
				}
				return _sessionFlags4.SerialSession;
			}
		}

		internal SessionFlags(Tr.Com.Eimza.Pkcs11.H4.SessionFlags sessionFlags)
		{
			if (sessionFlags == null)
			{
				throw new ArgumentNullException("sessionFlags");
			}
			_sessionFlags4 = sessionFlags;
		}

		internal SessionFlags(Tr.Com.Eimza.Pkcs11.H8.SessionFlags sessionFlags)
		{
			if (sessionFlags == null)
			{
				throw new ArgumentNullException("sessionFlags");
			}
			_sessionFlags8 = sessionFlags;
		}
	}
}
