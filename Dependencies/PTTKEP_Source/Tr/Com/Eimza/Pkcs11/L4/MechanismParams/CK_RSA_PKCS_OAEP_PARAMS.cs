using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_RSA_PKCS_OAEP_PARAMS
	{
		public uint HashAlg;

		public uint Mgf;

		public uint Source;

		public IntPtr SourceData;

		public uint SourceDataLen;
	}
}
