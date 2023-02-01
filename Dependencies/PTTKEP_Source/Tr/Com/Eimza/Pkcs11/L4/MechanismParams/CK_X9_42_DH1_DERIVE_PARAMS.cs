using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_X9_42_DH1_DERIVE_PARAMS
	{
		public uint Kdf;

		public uint OtherInfoLen;

		public IntPtr OtherInfo;

		public uint PublicDataLen;

		public IntPtr PublicData;
	}
}
