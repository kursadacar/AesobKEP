using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_TOKEN_INFO
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public byte[] Label;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public byte[] ManufacturerId;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] Model;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] SerialNumber;

		public uint Flags;

		public uint MaxSessionCount;

		public uint SessionCount;

		public uint MaxRwSessionCount;

		public uint RwSessionCount;

		public uint MaxPinLen;

		public uint MinPinLen;

		public uint TotalPublicMemory;

		public uint FreePublicMemory;

		public uint TotalPrivateMemory;

		public uint FreePrivateMemory;

		public CK_VERSION HardwareVersion;

		public CK_VERSION FirmwareVersion;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] UtcTime;
	}
}
