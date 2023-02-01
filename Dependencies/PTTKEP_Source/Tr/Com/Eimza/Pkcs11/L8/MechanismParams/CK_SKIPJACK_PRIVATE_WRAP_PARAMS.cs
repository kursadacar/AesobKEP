using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_SKIPJACK_PRIVATE_WRAP_PARAMS
	{
		public ulong PasswordLen;

		public IntPtr Password;

		public ulong PublicDataLen;

		public IntPtr PublicData;

		public ulong PAndGLen;

		public ulong QLen;

		public ulong RandomLen;

		public IntPtr RandomA;

		public IntPtr PrimeP;

		public IntPtr BaseG;

		public IntPtr SubprimeQ;
	}
}
