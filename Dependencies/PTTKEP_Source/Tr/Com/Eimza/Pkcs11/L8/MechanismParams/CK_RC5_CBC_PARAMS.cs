using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_RC5_CBC_PARAMS
	{
		public ulong Wordsize;

		public ulong Rounds;

		public IntPtr Iv;

		public ulong IvLen;
	}
}
