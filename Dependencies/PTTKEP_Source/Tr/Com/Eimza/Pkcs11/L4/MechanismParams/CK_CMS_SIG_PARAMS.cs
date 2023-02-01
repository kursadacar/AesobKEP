using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_CMS_SIG_PARAMS
	{
		public uint CertificateHandle;

		public IntPtr SigningMechanism;

		public IntPtr DigestMechanism;

		public IntPtr ContentType;

		public IntPtr RequestedAttributes;

		public uint RequestedAttributesLen;

		public IntPtr RequiredAttributes;

		public uint RequiredAttributesLen;
	}
}
