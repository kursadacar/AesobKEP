using System.Runtime.InteropServices;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L4
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate CKR C_GetAttributeValueDelegate(uint session, uint objectId, [In][Out] CK_ATTRIBUTE[] template, uint count);
}
