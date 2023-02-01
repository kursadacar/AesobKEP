using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_RC5_CBC_PARAMS
	{
		public uint Wordsize;

		public uint Rounds;

		public IntPtr Iv;

		public uint IvLen;
	}
}
