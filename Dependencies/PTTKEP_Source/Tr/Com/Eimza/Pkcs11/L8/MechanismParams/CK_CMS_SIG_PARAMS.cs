using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_CMS_SIG_PARAMS
	{
		public ulong CertificateHandle;

		public IntPtr SigningMechanism;

		public IntPtr DigestMechanism;

		public IntPtr ContentType;

		public IntPtr RequestedAttributes;

		public ulong RequestedAttributesLen;

		public IntPtr RequiredAttributes;

		public ulong RequiredAttributesLen;
	}
}
