using System.Runtime.InteropServices;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L8
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate CKR C_WrapKeyDelegate(ulong session, ref CK_MECHANISM mechanism, ulong wrappingKey, ulong key, byte[] wrappedKey, ref ulong wrappedKeyLen);
}
