using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_ECDH1_DERIVE_PARAMS
	{
		public uint Kdf;

		public uint SharedDataLen;

		public IntPtr SharedData;

		public uint PublicDataLen;

		public IntPtr PublicData;
	}
}
