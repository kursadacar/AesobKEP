using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H
{
    internal class SessionInfo
	{
		private Tr.Com.Eimza.Pkcs11.H4.SessionInfo _sessionInfo4;

		private Tr.Com.Eimza.Pkcs11.H8.SessionInfo _sessionInfo8;

		private SessionFlags _sessionFlags;

		public ulong SessionId
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _sessionInfo8.SessionId;
				}
				return _sessionInfo4.SessionId;
			}
		}

		public ulong SlotId
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _sessionInfo8.SlotId;
				}
				return _sessionInfo4.SlotId;
			}
		}

		public CKS State
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _sessionInfo8.State;
				}
				return _sessionInfo4.State;
			}
		}

		public SessionFlags SessionFlags
		{
			get
			{
				if (_sessionFlags == null)
				{
					_sessionFlags = ((UnmanagedLong.Size == 4) ? new SessionFlags(_sessionInfo4.SessionFlags) : new SessionFlags(_sessionInfo8.SessionFlags));
				}
				return _sessionFlags;
			}
		}

		public ulong DeviceError
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _sessionInfo8.DeviceError;
				}
				return _sessionInfo4.DeviceError;
			}
		}

		internal SessionInfo(Tr.Com.Eimza.Pkcs11.H4.SessionInfo sessionInfo)
		{
			if (sessionInfo == null)
			{
				throw new ArgumentNullException("sessionInfo");
			}
			_sessionInfo4 = sessionInfo;
		}

		internal SessionInfo(Tr.Com.Eimza.Pkcs11.H8.SessionInfo sessionInfo)
		{
			if (sessionInfo == null)
			{
				throw new ArgumentNullException("sessionInfo");
			}
			_sessionInfo8 = sessionInfo;
		}
	}
}
