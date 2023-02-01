using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4;

namespace Tr.Com.Eimza.Pkcs11.H4
{
	internal class SessionInfo
	{
		private uint _sessionId;

		private uint _slotId;

		private CKS _state;

		private SessionFlags _sessionFlags;

		private uint _deviceError;

		public uint SessionId
		{
			get
			{
				return _sessionId;
			}
		}

		public uint SlotId
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

		public uint DeviceError
		{
			get
			{
				return _deviceError;
			}
		}

		internal SessionInfo(uint sessionId, CK_SESSION_INFO ck_session_info)
		{
			_sessionId = sessionId;
			_slotId = ck_session_info.SlotId;
			_state = (CKS)ck_session_info.State;
			_sessionFlags = new SessionFlags(ck_session_info.Flags);
			_deviceError = ck_session_info.DeviceError;
		}
	}
}
