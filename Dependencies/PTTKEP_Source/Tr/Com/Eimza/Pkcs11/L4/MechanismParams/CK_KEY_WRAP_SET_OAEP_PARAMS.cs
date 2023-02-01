using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_KEY_WRAP_SET_OAEP_PARAMS
	{
		public byte BC;

		public IntPtr X;

		public uint XLen;
	}
}
