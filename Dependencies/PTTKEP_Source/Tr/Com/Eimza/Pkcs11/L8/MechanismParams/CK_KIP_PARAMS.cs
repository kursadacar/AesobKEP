using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_KIP_PARAMS
	{
		public IntPtr Mechanism;

		public ulong Key;

		public IntPtr Seed;

		public ulong SeedLen;
	}
}
