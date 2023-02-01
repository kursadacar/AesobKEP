using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_OTP_PARAM
	{
		public uint Type;

		public IntPtr Value;

		public uint ValueLen;
	}
}
