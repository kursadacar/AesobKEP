using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_ECMQV_DERIVE_PARAMS
	{
		public uint Kdf;

		public uint SharedDataLen;

		public IntPtr SharedData;

		public uint PublicDataLen;

		public IntPtr PublicData;

		public uint PrivateDataLen;

		public uint PrivateData;

		public uint PublicDataLen2;

		public IntPtr PublicData2;

		public uint PublicKey;
	}
}
