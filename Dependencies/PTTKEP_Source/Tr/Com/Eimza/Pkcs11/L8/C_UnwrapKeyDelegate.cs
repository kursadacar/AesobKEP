using System.Runtime.InteropServices;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L8
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate CKR C_UnwrapKeyDelegate(ulong session, ref CK_MECHANISM mechanism, ulong unwrappingKey, byte[] wrappedKey, ulong wrappedKeyLen, CK_ATTRIBUTE[] template, ulong attributeCount, ref ulong key);
}
