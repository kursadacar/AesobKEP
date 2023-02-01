using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_PBE_PARAMS
	{
		public IntPtr InitVector;

		public IntPtr Password;

		public ulong PasswordLen;

		public IntPtr Salt;

		public ulong SaltLen;

		public ulong Iteration;
	}
}
