using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_WTLS_MASTER_KEY_DERIVE_PARAMS
	{
		public ulong DigestMechanism;

		public CK_WTLS_RANDOM_DATA RandomInfo;

		public IntPtr Version;
	}
}
