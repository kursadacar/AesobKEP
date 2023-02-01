using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_SSL3_KEY_MAT_OUT
	{
		public ulong ClientMacSecret;

		public ulong ServerMacSecret;

		public ulong ClientKey;

		public ulong ServerKey;

		public IntPtr IVClient;

		public IntPtr IVServer;
	}
}
