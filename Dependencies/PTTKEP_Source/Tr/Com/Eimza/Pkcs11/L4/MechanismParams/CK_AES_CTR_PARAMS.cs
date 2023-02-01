using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_AES_CTR_PARAMS
	{
		public uint CounterBits;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] Cb;
	}
}
