using System;
using System.Collections.Generic;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4;

namespace Tr.Com.Eimza.Pkcs11.H4
{
	internal class Slot
	{
		private Tr.Com.Eimza.Pkcs11.L4.Pkcs11 _p11;

		private uint _slotId;

		public uint SlotId
		{
			get
			{
				return _slotId;
			}
		}

		internal Slot(Tr.Com.Eimza.Pkcs11.L4.Pkcs11 pkcs11, uint slotId)
		{
			if (pkcs11 == null)
			{
				throw new ArgumentNullException("pkcs11");
			}
			_p11 = pkcs11;
			_slotId = slotId;
		}

		public SlotInfo GetSlotInfo()
		{
			CK_SLOT_INFO info = default(CK_SLOT_INFO);
			CKR cKR = _p11.C_GetSlotInfo(_slotId, ref info);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_GetSlotInfo", cKR);
			}
			return new SlotInfo(_slotId, info);
		}

		public TokenInfo GetTokenInfo()
		{
			CK_TOKEN_INFO info = default(CK_TOKEN_INFO);
			CKR cKR = _p11.C_GetTokenInfo(_slotId, ref info);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_GetTokenInfo", cKR);
			}
			return new TokenInfo(_slotId, info);
		}

		public List<CKM> GetMechanismList()
		{
			uint count = 0u;
			CKR cKR = _p11.C_GetMechanismList(_slotId, null, ref count);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_GetMechanismList", cKR);
			}
			if (count < 1)
			{
				return new List<CKM>();
			}
			CKM[] array = new CKM[count];
			cKR = _p11.C_GetMechanismList(_slotId, array, ref count);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_GetMechanismList", cKR);
			}
			if (array.Length != count)
			{
				Array.Resize(ref array, Convert.ToInt32(count));
			}
			return new List<CKM>(array);
		}

		public MechanismInfo GetMechanismInfo(CKM mechanism)
		{
			CK_MECHANISM_INFO info = default(CK_MECHANISM_INFO);
			CKR cKR = _p11.C_GetMechanismInfo(_slotId, mechanism, ref info);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_GetMechanismInfo", cKR);
			}
			return new MechanismInfo(mechanism, info);
		}

		public void InitToken(string soPin, string label)
		{
			byte[] array = null;
			uint pinLen = 0u;
			if (soPin != null)
			{
				array = ConvertUtils.Utf8StringToBytes(soPin);
				pinLen = Convert.ToUInt32(array.Length);
			}
			byte[] label2 = ConvertUtils.Utf8StringToBytes(label, 32, 32);
			CKR cKR = _p11.C_InitToken(_slotId, array, pinLen, label2);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_InitToken", cKR);
			}
		}

		public void InitToken(byte[] soPin, byte[] label)
		{
			byte[] pin = null;
			uint pinLen = 0u;
			if (soPin != null)
			{
				pin = soPin;
				pinLen = Convert.ToUInt32(soPin.Length);
			}
			byte[] array = new byte[32];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 32;
			}
			if (label != null)
			{
				if (label.Length > 32)
				{
					throw new Exception("Label too long");
				}
				Array.Copy(label, 0, array, 0, label.Length);
			}
			CKR cKR = _p11.C_InitToken(_slotId, pin, pinLen, array);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_InitToken", cKR);
			}
		}

		public Session OpenSession(bool readOnly)
		{
			uint num = 4u;
			if (!readOnly)
			{
				num |= 2u;
			}
			uint session = 0u;
			CKR cKR = _p11.C_OpenSession(_slotId, num, IntPtr.Zero, IntPtr.Zero, ref session);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_OpenSession", cKR);
			}
			return new Session(_p11, session);
		}

		public void CloseSession(Session session)
		{
			if (session == null)
			{
				throw new ArgumentNullException("session");
			}
			session.CloseSession();
		}

		public void CloseAllSessions()
		{
			CKR cKR = _p11.C_CloseAllSessions(_slotId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_CloseAllSessions", cKR);
			}
		}
	}
}
