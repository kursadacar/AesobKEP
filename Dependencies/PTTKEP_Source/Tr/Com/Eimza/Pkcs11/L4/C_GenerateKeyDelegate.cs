using System.Runtime.InteropServices;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L4
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate CKR C_GenerateKeyDelegate(uint session, ref CK_MECHANISM mechanism, CK_ATTRIBUTE[] template, uint count, ref uint key);
}
