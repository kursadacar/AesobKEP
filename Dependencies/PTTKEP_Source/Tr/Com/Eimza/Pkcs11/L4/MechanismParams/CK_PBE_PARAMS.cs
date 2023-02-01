using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_PBE_PARAMS
	{
		public IntPtr InitVector;

		public IntPtr Password;

		public uint PasswordLen;

		public IntPtr Salt;

		public uint SaltLen;

		public uint Iteration;
	}
}
