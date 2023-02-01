using System.Runtime.InteropServices;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L8
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate CKR C_VerifyRecoverDelegate(ulong session, byte[] signature, ulong signatureLen, byte[] data, ref ulong dataLen);
}
