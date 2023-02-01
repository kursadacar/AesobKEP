using System;
using System.Collections.Generic;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H
{
    internal class Slot
	{
		private Tr.Com.Eimza.Pkcs11.H4.Slot _slot4;

		private Tr.Com.Eimza.Pkcs11.H8.Slot _slot8;

		public ulong SlotId
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _slot8.SlotId;
				}
				return _slot4.SlotId;
			}
		}

		internal Slot(Tr.Com.Eimza.Pkcs11.H4.Slot slot)
		{
			if (slot == null)
			{
				throw new ArgumentNullException("slot");
			}
			_slot4 = slot;
		}

		internal Slot(Tr.Com.Eimza.Pkcs11.H8.Slot slot)
		{
			if (slot == null)
			{
				throw new ArgumentNullException("slot");
			}
			_slot8 = slot;
		}

		public SlotInfo GetSlotInfo()
		{
			if (UnmanagedLong.Size == 4)
			{
				return new SlotInfo(_slot4.GetSlotInfo());
			}
			return new SlotInfo(_slot8.GetSlotInfo());
		}

		public TokenInfo GetTokenInfo()
		{
			if (UnmanagedLong.Size == 4)
			{
				return new TokenInfo(_slot4.GetTokenInfo());
			}
			return new TokenInfo(_slot8.GetTokenInfo());
		}

		public List<CKM> GetMechanismList()
		{
			if (UnmanagedLong.Size == 4)
			{
				return _slot4.GetMechanismList();
			}
			return _slot8.GetMechanismList();
		}

		public MechanismInfo GetMechanismInfo(CKM mechanism)
		{
			if (UnmanagedLong.Size == 4)
			{
				return new MechanismInfo(_slot4.GetMechanismInfo(mechanism));
			}
			return new MechanismInfo(_slot8.GetMechanismInfo(mechanism));
		}

		public void InitToken(string soPin, string label)
		{
			if (UnmanagedLong.Size == 4)
			{
				_slot4.InitToken(soPin, label);
			}
			else
			{
				_slot8.InitToken(soPin, label);
			}
		}

		public void InitToken(byte[] soPin, byte[] label)
		{
			if (UnmanagedLong.Size == 4)
			{
				_slot4.InitToken(soPin, label);
			}
			else
			{
				_slot8.InitToken(soPin, label);
			}
		}

		public Session OpenSession(bool readOnly)
		{
			if (UnmanagedLong.Size == 4)
			{
				return new Session(_slot4.OpenSession(readOnly));
			}
			return new Session(_slot8.OpenSession(readOnly));
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
			if (UnmanagedLong.Size == 4)
			{
				_slot4.CloseAllSessions();
			}
			else
			{
				_slot8.CloseAllSessions();
			}
		}
	}
}
