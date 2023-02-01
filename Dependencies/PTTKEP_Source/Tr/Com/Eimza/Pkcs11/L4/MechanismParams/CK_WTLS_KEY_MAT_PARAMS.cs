using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_WTLS_KEY_MAT_PARAMS
	{
		public uint DigestMechanism;

		public uint MacSizeInBits;

		public uint KeySizeInBits;

		public uint IVSizeInBits;

		public uint SequenceNumber;

		public bool IsExport;

		public CK_WTLS_RANDOM_DATA RandomInfo;

		public IntPtr ReturnedKeyMaterial;
	}
}
