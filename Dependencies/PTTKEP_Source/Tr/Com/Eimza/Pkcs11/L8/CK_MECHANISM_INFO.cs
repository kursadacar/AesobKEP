using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_MECHANISM_INFO
	{
		public ulong MinKeySize;

		public ulong MaxKeySize;

		public ulong Flags;
	}
}
