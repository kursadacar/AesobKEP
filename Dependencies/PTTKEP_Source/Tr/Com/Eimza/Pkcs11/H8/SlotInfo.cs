using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8;

namespace Tr.Com.Eimza.Pkcs11.H8
{
	internal class SlotInfo
	{
		private ulong _slotId;

		private string _slotDescription;

		private string _manufacturerId;

		private SlotFlags _slotFlags;

		private string _hardwareVersion;

		private string _firmwareVersion;

		public ulong SlotId
		{
			get
			{
				return _slotId;
			}
		}

		public string SlotDescription
		{
			get
			{
				return _slotDescription;
			}
		}

		public string ManufacturerId
		{
			get
			{
				return _manufacturerId;
			}
		}

		public SlotFlags SlotFlags
		{
			get
			{
				return _slotFlags;
			}
		}

		public string HardwareVersion
		{
			get
			{
				return _hardwareVersion;
			}
		}

		public string FirmwareVersion
		{
			get
			{
				return _firmwareVersion;
			}
		}

		internal SlotInfo(ulong slotId, CK_SLOT_INFO ck_slot_info)
		{
			_slotId = slotId;
			_slotDescription = ConvertUtils.BytesToUtf8String(ck_slot_info.SlotDescription, true);
			_manufacturerId = ConvertUtils.BytesToUtf8String(ck_slot_info.ManufacturerId, true);
			_slotFlags = new SlotFlags(ck_slot_info.Flags);
			_hardwareVersion = ConvertUtils.CkVersionToString(ck_slot_info.HardwareVersion);
			_firmwareVersion = ConvertUtils.CkVersionToString(ck_slot_info.FirmwareVersion);
		}
	}
}
