using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal class CK_C_INITIALIZE_ARGS
	{
		public IntPtr CreateMutex = IntPtr.Zero;

		public IntPtr DestroyMutex = IntPtr.Zero;

		public IntPtr LockMutex = IntPtr.Zero;

		public IntPtr UnlockMutex = IntPtr.Zero;

		public ulong Flags;

		public IntPtr Reserved = IntPtr.Zero;
	}
}
