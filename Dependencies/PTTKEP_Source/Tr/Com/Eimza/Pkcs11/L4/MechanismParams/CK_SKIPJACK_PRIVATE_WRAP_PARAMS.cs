using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_SKIPJACK_PRIVATE_WRAP_PARAMS
	{
		public uint PasswordLen;

		public IntPtr Password;

		public uint PublicDataLen;

		public IntPtr PublicData;

		public uint PAndGLen;

		public uint QLen;

		public uint RandomLen;

		public IntPtr RandomA;

		public IntPtr PrimeP;

		public IntPtr BaseG;

		public IntPtr SubprimeQ;
	}
}
