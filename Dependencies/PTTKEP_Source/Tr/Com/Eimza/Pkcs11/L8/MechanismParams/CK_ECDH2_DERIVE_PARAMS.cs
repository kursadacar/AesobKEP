using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_ECDH2_DERIVE_PARAMS
	{
		public ulong Kdf;

		public ulong SharedDataLen;

		public IntPtr SharedData;

		public ulong PublicDataLen;

		public IntPtr PublicData;

		public ulong PrivateDataLen;

		public ulong PrivateData;

		public ulong PublicDataLen2;

		public IntPtr PublicData2;
	}
}
