using System;
using System.Runtime.InteropServices;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L8
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate CKR C_OpenSessionDelegate(ulong slotId, ulong flags, IntPtr application, IntPtr notify, ref ulong session);
}
