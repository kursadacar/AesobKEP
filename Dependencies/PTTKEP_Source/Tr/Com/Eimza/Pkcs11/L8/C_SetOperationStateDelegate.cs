using System.Runtime.InteropServices;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L8
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate CKR C_SetOperationStateDelegate(ulong session, byte[] operationState, ulong operationStateLen, ulong encryptionKey, ulong authenticationKey);
}
