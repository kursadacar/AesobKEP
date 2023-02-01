using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_RC2_CBC_PARAMS
	{
		public uint EffectiveBits;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] Iv;
	}
}
