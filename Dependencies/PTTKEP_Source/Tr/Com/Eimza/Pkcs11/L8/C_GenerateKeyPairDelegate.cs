using System.Runtime.InteropServices;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L8
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate CKR C_GenerateKeyPairDelegate(ulong session, ref CK_MECHANISM mechanism, CK_ATTRIBUTE[] publicKeyTemplate, ulong publicKeyAttributeCount, CK_ATTRIBUTE[] privateKeyTemplate, ulong privateKeyAttributeCount, ref ulong publicKey, ref ulong privateKey);
}
