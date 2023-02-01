using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_SESSION_INFO
	{
		public uint SlotId;

		public uint State;

		public uint Flags;

		public uint DeviceError;
	}
}
