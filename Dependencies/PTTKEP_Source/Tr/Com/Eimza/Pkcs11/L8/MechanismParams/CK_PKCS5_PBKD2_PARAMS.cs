using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_PKCS5_PBKD2_PARAMS
	{
		public ulong SaltSource;

		public IntPtr SaltSourceData;

		public ulong SaltSourceDataLen;

		public ulong Iterations;

		public ulong Prf;

		public IntPtr PrfData;

		public ulong PrfDataLen;

		public IntPtr Password;

		public ulong PasswordLen;
	}
}
