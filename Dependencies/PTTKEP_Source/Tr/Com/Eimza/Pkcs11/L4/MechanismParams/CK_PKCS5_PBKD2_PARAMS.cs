using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_PKCS5_PBKD2_PARAMS
	{
		public uint SaltSource;

		public IntPtr SaltSourceData;

		public uint SaltSourceDataLen;

		public uint Iterations;

		public uint Prf;

		public IntPtr PrfData;

		public uint PrfDataLen;

		public IntPtr Password;

		public uint PasswordLen;
	}
}
