using System.Runtime.InteropServices;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L8
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate CKR C_GenerateKeyDelegate(ulong session, ref CK_MECHANISM mechanism, CK_ATTRIBUTE[] template, ulong count, ref ulong key);
}
