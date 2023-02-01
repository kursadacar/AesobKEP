using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_KEA_DERIVE_PARAMS
	{
		public bool IsSender;

		public ulong RandomLen;

		public IntPtr RandomA;

		public IntPtr RandomB;

		public ulong PublicDataLen;

		public IntPtr PublicData;
	}
}
