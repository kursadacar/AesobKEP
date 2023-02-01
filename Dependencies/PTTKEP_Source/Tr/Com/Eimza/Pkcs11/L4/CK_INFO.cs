using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_INFO
	{
		public CK_VERSION CryptokiVersion;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public byte[] ManufacturerId;

		public uint Flags;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public byte[] LibraryDescription;

		public CK_VERSION LibraryVersion;
	}
}
