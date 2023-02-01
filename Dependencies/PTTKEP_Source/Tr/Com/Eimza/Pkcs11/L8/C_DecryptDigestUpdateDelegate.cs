using System.Runtime.InteropServices;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L8
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate CKR C_DecryptDigestUpdateDelegate(ulong session, byte[] encryptedPart, ulong encryptedPartLen, byte[] part, ref ulong partLen);
}
