using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H
{
    internal class SlotFlags
	{
		private Tr.Com.Eimza.Pkcs11.H4.SlotFlags _slotFlags4;

		private Tr.Com.Eimza.Pkcs11.H8.SlotFlags _slotFlags8;

		public ulong Flags
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _slotFlags8.Flags;
				}
				return _slotFlags4.Flags;
			}
		}

		public bool TokenPresent
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _slotFlags8.TokenPresent;
				}
				return _slotFlags4.TokenPresent;
			}
		}

		public bool RemovableDevice
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _slotFlags8.RemovableDevice;
				}
				return _slotFlags4.RemovableDevice;
			}
		}

		public bool HardwareSlot
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _slotFlags8.HardwareSlot;
				}
				return _slotFlags4.HardwareSlot;
			}
		}

		internal SlotFlags(Tr.Com.Eimza.Pkcs11.H4.SlotFlags slotFlags)
		{
			if (slotFlags == null)
			{
				throw new ArgumentNullException("slotFlags");
			}
			_slotFlags4 = slotFlags;
		}

		internal SlotFlags(Tr.Com.Eimza.Pkcs11.H8.SlotFlags slotFlags)
		{
			if (slotFlags == null)
			{
				throw new ArgumentNullException("slotFlags");
			}
			_slotFlags8 = slotFlags;
		}
	}
}
