using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8
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

		public ulong Flags;

		public ulong MaxSessionCount;

		public ulong SessionCount;

		public ulong MaxRwSessionCount;

		public ulong RwSessionCount;

		public ulong MaxPinLen;

		public ulong MinPinLen;

		public ulong TotalPublicMemory;

		public ulong FreePublicMemory;

		public ulong TotalPrivateMemory;

		public ulong FreePrivateMemory;

		public CK_VERSION HardwareVersion;

		public CK_VERSION FirmwareVersion;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] UtcTime;
	}
}
