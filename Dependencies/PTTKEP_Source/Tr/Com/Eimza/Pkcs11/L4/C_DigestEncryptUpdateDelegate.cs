using System.Runtime.InteropServices;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L4
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate CKR C_DigestEncryptUpdateDelegate(uint session, byte[] part, uint partLen, byte[] encryptedPart, ref uint encryptedPartLen);
}
