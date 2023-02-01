using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_SLOT_INFO
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public byte[] SlotDescription;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public byte[] ManufacturerId;

		public uint Flags;

		public CK_VERSION HardwareVersion;

		public CK_VERSION FirmwareVersion;
	}
}
