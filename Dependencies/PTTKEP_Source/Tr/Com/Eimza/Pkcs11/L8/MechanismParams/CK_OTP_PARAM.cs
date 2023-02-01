using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_OTP_PARAM
	{
		public ulong Type;

		public IntPtr Value;

		public ulong ValueLen;
	}
}
