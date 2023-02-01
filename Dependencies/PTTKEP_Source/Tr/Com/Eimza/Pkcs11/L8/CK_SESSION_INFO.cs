using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_SESSION_INFO
	{
		public ulong SlotId;

		public ulong State;

		public ulong Flags;

		public ulong DeviceError;
	}
}
