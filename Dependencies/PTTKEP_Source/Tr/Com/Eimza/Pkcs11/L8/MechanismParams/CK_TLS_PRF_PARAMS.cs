using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_TLS_PRF_PARAMS
	{
		public IntPtr Seed;

		public ulong SeedLen;

		public IntPtr Label;

		public ulong LabelLen;

		public IntPtr Output;

		public IntPtr OutputLen;
	}
}
