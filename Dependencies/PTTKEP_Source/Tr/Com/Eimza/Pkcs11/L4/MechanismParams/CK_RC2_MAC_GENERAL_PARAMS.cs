using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_RC2_MAC_GENERAL_PARAMS
	{
		public uint EffectiveBits;

		public uint MacLength;
	}
}
