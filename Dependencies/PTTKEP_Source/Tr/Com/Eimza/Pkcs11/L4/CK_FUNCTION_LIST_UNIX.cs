using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct CK_FUNCTION_LIST_UNIX
	{
		internal CK_VERSION version;

		internal IntPtr C_Initialize;

		internal IntPtr C_Finalize;

		internal IntPtr C_GetInfo;

		internal IntPtr C_GetFunctionList;

		internal IntPtr C_GetSlotList;

		internal IntPtr C_GetSlotInfo;

		internal IntPtr C_GetTokenInfo;

		internal IntPtr C_GetMechanismList;

		internal IntPtr C_GetMechanismInfo;

		internal IntPtr C_InitToken;

		internal IntPtr C_InitPIN;

		internal IntPtr C_SetPIN;

		internal IntPtr C_OpenSession;

		internal IntPtr C_CloseSession;

		internal IntPtr C_CloseAllSessions;

		internal IntPtr C_GetSessionInfo;

		internal IntPtr C_GetOperationState;

		internal IntPtr C_SetOperationState;

		internal IntPtr C_Login;

		internal IntPtr C_Logout;

		internal IntPtr C_CreateObject;

		internal IntPtr C_CopyObject;

		internal IntPtr C_DestroyObject;

		internal IntPtr C_GetObjectSize;

		internal IntPtr C_GetAttributeValue;

		internal IntPtr C_SetAttributeValue;

		internal IntPtr C_FindObjectsInit;

		internal IntPtr C_FindObjects;

		internal IntPtr C_FindObjectsFinal;

		internal IntPtr C_EncryptInit;

		internal IntPtr C_Encrypt;

		internal IntPtr C_EncryptUpdate;

		internal IntPtr C_EncryptFinal;

		internal IntPtr C_DecryptInit;

		internal IntPtr C_Decrypt;

		internal IntPtr C_DecryptUpdate;

		internal IntPtr C_DecryptFinal;

		internal IntPtr C_DigestInit;

		internal IntPtr C_Digest;

		internal IntPtr C_DigestUpdate;

		internal IntPtr C_DigestKey;

		internal IntPtr C_DigestFinal;

		internal IntPtr C_SignInit;

		internal IntPtr C_Sign;

		internal IntPtr C_SignUpdate;

		internal IntPtr C_SignFinal;

		internal IntPtr C_SignRecoverInit;

		internal IntPtr C_SignRecover;

		internal IntPtr C_VerifyInit;

		internal IntPtr C_Verify;

		internal IntPtr C_VerifyUpdate;

		internal IntPtr C_VerifyFinal;

		internal IntPtr C_VerifyRecoverInit;

		internal IntPtr C_VerifyRecover;

		internal IntPtr C_DigestEncryptUpdate;

		internal IntPtr C_DecryptDigestUpdate;

		internal IntPtr C_SignEncryptUpdate;

		internal IntPtr C_DecryptVerifyUpdate;

		internal IntPtr C_GenerateKey;

		internal IntPtr C_GenerateKeyPair;

		internal IntPtr C_WrapKey;

		internal IntPtr C_UnwrapKey;

		internal IntPtr C_DeriveKey;

		internal IntPtr C_SeedRandom;

		internal IntPtr C_GenerateRandom;

		internal IntPtr C_GetFunctionStatus;

		internal IntPtr C_CancelFunction;

		internal IntPtr C_WaitForSlotEvent;

		public CK_FUNCTION_LIST ConvertToCkFunctionList()
		{
			CK_FUNCTION_LIST result = default(CK_FUNCTION_LIST);
			result.version = version;
			result.C_Initialize = C_Initialize;
			result.C_Finalize = C_Finalize;
			result.C_GetInfo = C_GetInfo;
			result.C_GetFunctionList = C_GetFunctionList;
			result.C_GetSlotList = C_GetSlotList;
			result.C_GetSlotInfo = C_GetSlotInfo;
			result.C_GetTokenInfo = C_GetTokenInfo;
			result.C_GetMechanismList = C_GetMechanismList;
			result.C_GetMechanismInfo = C_GetMechanismInfo;
			result.C_InitToken = C_InitToken;
			result.C_InitPIN = C_InitPIN;
			result.C_SetPIN = C_SetPIN;
			result.C_OpenSession = C_OpenSession;
			result.C_CloseSession = C_CloseSession;
			result.C_CloseAllSessions = C_CloseAllSessions;
			result.C_GetSessionInfo = C_GetSessionInfo;
			result.C_GetOperationState = C_GetOperationState;
			result.C_SetOperationState = C_SetOperationState;
			result.C_Login = C_Login;
			result.C_Logout = C_Logout;
			result.C_CreateObject = C_CreateObject;
			result.C_CopyObject = C_CopyObject;
			result.C_DestroyObject = C_DestroyObject;
			result.C_GetObjectSize = C_GetObjectSize;
			result.C_GetAttributeValue = C_GetAttributeValue;
			result.C_SetAttributeValue = C_SetAttributeValue;
			result.C_FindObjectsInit = C_FindObjectsInit;
			result.C_FindObjects = C_FindObjects;
			result.C_FindObjectsFinal = C_FindObjectsFinal;
			result.C_EncryptInit = C_EncryptInit;
			result.C_Encrypt = C_Encrypt;
			result.C_EncryptUpdate = C_EncryptUpdate;
			result.C_EncryptFinal = C_EncryptFinal;
			result.C_DecryptInit = C_DecryptInit;
			result.C_Decrypt = C_Decrypt;
			result.C_DecryptUpdate = C_DecryptUpdate;
			result.C_DecryptFinal = C_DecryptFinal;
			result.C_DigestInit = C_DigestInit;
			result.C_Digest = C_Digest;
			result.C_DigestUpdate = C_DigestUpdate;
			result.C_DigestKey = C_DigestKey;
			result.C_DigestFinal = C_DigestFinal;
			result.C_SignInit = C_SignInit;
			result.C_Sign = C_Sign;
			result.C_SignUpdate = C_SignUpdate;
			result.C_SignFinal = C_SignFinal;
			result.C_SignRecoverInit = C_SignRecoverInit;
			result.C_SignRecover = C_SignRecover;
			result.C_VerifyInit = C_VerifyInit;
			result.C_Verify = C_Verify;
			result.C_VerifyUpdate = C_VerifyUpdate;
			result.C_VerifyFinal = C_VerifyFinal;
			result.C_VerifyRecoverInit = C_VerifyRecoverInit;
			result.C_VerifyRecover = C_VerifyRecover;
			result.C_DigestEncryptUpdate = C_DigestEncryptUpdate;
			result.C_DecryptDigestUpdate = C_DecryptDigestUpdate;
			result.C_SignEncryptUpdate = C_SignEncryptUpdate;
			result.C_DecryptVerifyUpdate = C_DecryptVerifyUpdate;
			result.C_GenerateKey = C_GenerateKey;
			result.C_GenerateKeyPair = C_GenerateKeyPair;
			result.C_WrapKey = C_WrapKey;
			result.C_UnwrapKey = C_UnwrapKey;
			result.C_DeriveKey = C_DeriveKey;
			result.C_SeedRandom = C_SeedRandom;
			result.C_GenerateRandom = C_GenerateRandom;
			result.C_GetFunctionStatus = C_GetFunctionStatus;
			result.C_CancelFunction = C_CancelFunction;
			result.C_WaitForSlotEvent = C_WaitForSlotEvent;
			return result;
		}
	}
}
