using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_RSA_PKCS_PSS_PARAMS
	{
		public ulong HashAlg;

		public ulong Mgf;

		public ulong Len;
	}
}
