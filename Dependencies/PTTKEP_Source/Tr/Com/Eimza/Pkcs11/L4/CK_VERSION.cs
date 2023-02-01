using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_VERSION
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
		public byte[] Major;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
		public byte[] Minor;
	}
}
