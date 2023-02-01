using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_WTLS_KEY_MAT_PARAMS
	{
		public ulong DigestMechanism;

		public ulong MacSizeInBits;

		public ulong KeySizeInBits;

		public ulong IVSizeInBits;

		public ulong SequenceNumber;

		public bool IsExport;

		public CK_WTLS_RANDOM_DATA RandomInfo;

		public IntPtr ReturnedKeyMaterial;
	}
}
