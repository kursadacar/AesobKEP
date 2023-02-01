using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_WTLS_KEY_MAT_OUT
	{
		public ulong MacSecret;

		public ulong Key;

		public IntPtr IV;
	}
}
