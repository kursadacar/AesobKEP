using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H
{
    internal class SlotInfo
	{
		private Tr.Com.Eimza.Pkcs11.H4.SlotInfo _slotInfo4;

		private Tr.Com.Eimza.Pkcs11.H8.SlotInfo _slotInfo8;

		private SlotFlags _slotFlags;

		public ulong SlotId
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _slotInfo8.SlotId;
				}
				return _slotInfo4.SlotId;
			}
		}

		public string SlotDescription
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _slotInfo8.SlotDescription;
				}
				return _slotInfo4.SlotDescription;
			}
		}

		public string ManufacturerId
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _slotInfo8.ManufacturerId;
				}
				return _slotInfo4.ManufacturerId;
			}
		}

		public SlotFlags SlotFlags
		{
			get
			{
				if (_slotFlags == null)
				{
					_slotFlags = ((UnmanagedLong.Size == 4) ? new SlotFlags(_slotInfo4.SlotFlags) : new SlotFlags(_slotInfo8.SlotFlags));
				}
				return _slotFlags;
			}
		}

		public string HardwareVersion
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _slotInfo8.HardwareVersion;
				}
				return _slotInfo4.HardwareVersion;
			}
		}

		public string FirmwareVersion
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _slotInfo8.FirmwareVersion;
				}
				return _slotInfo4.FirmwareVersion;
			}
		}

		internal SlotInfo(Tr.Com.Eimza.Pkcs11.H4.SlotInfo slotInfo)
		{
			if (slotInfo == null)
			{
				throw new ArgumentNullException("slotInfo");
			}
			_slotInfo4 = slotInfo;
		}

		internal SlotInfo(Tr.Com.Eimza.Pkcs11.H8.SlotInfo slotInfo)
		{
			if (slotInfo == null)
			{
				throw new ArgumentNullException("slotInfo");
			}
			_slotInfo8 = slotInfo;
		}
	}
}
