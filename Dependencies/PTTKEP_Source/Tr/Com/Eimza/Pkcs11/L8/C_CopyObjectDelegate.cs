using System.Runtime.InteropServices;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L8
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate CKR C_CopyObjectDelegate(ulong session, ulong objectId, CK_ATTRIBUTE[] template, ulong count, ref ulong newObjectId);
}
