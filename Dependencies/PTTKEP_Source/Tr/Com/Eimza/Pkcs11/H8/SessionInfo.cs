using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8;

namespace Tr.Com.Eimza.Pkcs11.H8
{
	internal class SessionInfo
	{
		private ulong _sessionId;

		private ulong _slotId;

		private CKS _state;

		private SessionFlags _sessionFlags;

		private ulong _deviceError;

		public ulong SessionId
		{
			get
			{
				return _sessionId;
			}
		}

		public ulong SlotId
		{
			get
			{
				return _slotId;
			}
		}

		public CKS State
		{
			get
			{
				return _state;
			}
		}

		public SessionFlags SessionFlags
		{
			get
			{
				return _sessionFlags;
			}
		}

		public ulong DeviceError
		{
			get
			{
				return _deviceError;
			}
		}

		internal SessionInfo(ulong sessionId, CK_SESSION_INFO ck_session_info)
		{
			_sessionId = sessionId;
			_slotId = ck_session_info.SlotId;
			_state = (CKS)ck_session_info.State;
			_sessionFlags = new SessionFlags(ck_session_info.Flags);
			_deviceError = ck_session_info.DeviceError;
		}
	}
}
