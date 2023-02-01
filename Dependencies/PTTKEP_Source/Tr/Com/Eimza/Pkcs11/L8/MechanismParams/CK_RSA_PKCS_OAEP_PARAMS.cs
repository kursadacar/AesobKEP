using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_RSA_PKCS_OAEP_PARAMS
	{
		public ulong HashAlg;

		public ulong Mgf;

		public ulong Source;

		public IntPtr SourceData;

		public ulong SourceDataLen;
	}
}
