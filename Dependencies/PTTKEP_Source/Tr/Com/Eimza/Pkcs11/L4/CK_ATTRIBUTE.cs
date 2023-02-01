using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_ATTRIBUTE
	{
		public uint type;

		public IntPtr value;

		public uint valueLen;
	}
}
