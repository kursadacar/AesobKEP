using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_MECHANISM
	{
		public ulong Mechanism;

		public IntPtr Parameter;

		public ulong ParameterLen;
	}
}
