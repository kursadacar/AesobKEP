using System.Runtime.InteropServices;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L4
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate CKR C_GetMechanismInfoDelegate(uint slotId, uint type, ref CK_MECHANISM_INFO info);
}
