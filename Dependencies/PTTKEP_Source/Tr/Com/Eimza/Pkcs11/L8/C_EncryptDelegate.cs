using System.Runtime.InteropServices;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L8
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate CKR C_EncryptDelegate(ulong session, byte[] data, ulong dataLen, byte[] encryptedData, ref ulong encryptedDataLen);
}
