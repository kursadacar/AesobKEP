using System;
using System.Runtime.InteropServices;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L4
{
	internal class Pkcs11 : IDisposable
	{
		private bool _disposed;

		private IntPtr _libraryHandle = IntPtr.Zero;

		private CK_FUNCTION_LIST _functionList;

		public Pkcs11(string libraryPath)
		{
			if (libraryPath == null)
			{
				throw new ArgumentNullException("libraryPath");
			}
			_libraryHandle = UnmanagedLibrary.Load(libraryPath);
			C_GetFunctionList(out _functionList);
		}

		public Pkcs11(string libraryPath, bool useGetFunctionList)
		{
			if (libraryPath == null)
			{
				throw new ArgumentNullException("libraryPath");
			}
			_libraryHandle = UnmanagedLibrary.Load(libraryPath);
			if (useGetFunctionList)
			{
				C_GetFunctionList(out _functionList);
			}
			else
			{
				GetFunctionList(out _functionList);
			}
		}

		private void Release()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (_libraryHandle != IntPtr.Zero)
			{
				UnmanagedLibrary.Unload(_libraryHandle);
				_libraryHandle = IntPtr.Zero;
			}
		}

		public CKR C_Initialize(CK_C_INITIALIZE_ARGS initArgs)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_InitializeDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_Initialize, typeof(C_InitializeDelegate)))(initArgs);
		}

		public CKR C_Finalize(IntPtr reserved)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_FinalizeDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_Finalize, typeof(C_FinalizeDelegate)))(reserved);
		}

		public CKR C_GetInfo(ref CK_INFO info)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_GetInfoDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_GetInfo, typeof(C_GetInfoDelegate)))(ref info);
		}

		private void C_GetFunctionList(out CK_FUNCTION_LIST functionList)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			C_GetFunctionListDelegate obj = (C_GetFunctionListDelegate)Marshal.GetDelegateForFunctionPointer(UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_GetFunctionList"), typeof(C_GetFunctionListDelegate));
			IntPtr functionList2 = IntPtr.Zero;
			if (obj(out functionList2) != 0 || functionList2 == IntPtr.Zero)
			{
				throw new Exception("Unable to call C_GetFunctionList");
			}
			PlatformID platform = Environment.OSVersion.Platform;
			if (platform == PlatformID.Unix || platform == PlatformID.MacOSX)
			{
				functionList = ((CK_FUNCTION_LIST_UNIX)UnmanagedMemory.Read(functionList2, typeof(CK_FUNCTION_LIST_UNIX))).ConvertToCkFunctionList();
			}
			else
			{
				functionList = (CK_FUNCTION_LIST)UnmanagedMemory.Read(functionList2, typeof(CK_FUNCTION_LIST));
			}
		}

		private void GetFunctionList(out CK_FUNCTION_LIST functionList)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			CK_FUNCTION_LIST cK_FUNCTION_LIST = default(CK_FUNCTION_LIST);
			cK_FUNCTION_LIST.C_Initialize = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_Initialize");
			cK_FUNCTION_LIST.C_Finalize = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_Finalize");
			cK_FUNCTION_LIST.C_GetInfo = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_GetInfo");
			cK_FUNCTION_LIST.C_GetFunctionList = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_GetFunctionList");
			cK_FUNCTION_LIST.C_GetSlotList = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_GetSlotList");
			cK_FUNCTION_LIST.C_GetSlotInfo = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_GetSlotInfo");
			cK_FUNCTION_LIST.C_GetTokenInfo = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_GetTokenInfo");
			cK_FUNCTION_LIST.C_GetMechanismList = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_GetMechanismList");
			cK_FUNCTION_LIST.C_GetMechanismInfo = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_GetMechanismInfo");
			cK_FUNCTION_LIST.C_InitToken = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_InitToken");
			cK_FUNCTION_LIST.C_InitPIN = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_InitPIN");
			cK_FUNCTION_LIST.C_SetPIN = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_SetPIN");
			cK_FUNCTION_LIST.C_OpenSession = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_OpenSession");
			cK_FUNCTION_LIST.C_CloseSession = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_CloseSession");
			cK_FUNCTION_LIST.C_CloseAllSessions = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_CloseAllSessions");
			cK_FUNCTION_LIST.C_GetSessionInfo = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_GetSessionInfo");
			cK_FUNCTION_LIST.C_GetOperationState = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_GetOperationState");
			cK_FUNCTION_LIST.C_SetOperationState = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_SetOperationState");
			cK_FUNCTION_LIST.C_Login = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_Login");
			cK_FUNCTION_LIST.C_Logout = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_Logout");
			cK_FUNCTION_LIST.C_CreateObject = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_CreateObject");
			cK_FUNCTION_LIST.C_CopyObject = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_CopyObject");
			cK_FUNCTION_LIST.C_DestroyObject = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_DestroyObject");
			cK_FUNCTION_LIST.C_GetObjectSize = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_GetObjectSize");
			cK_FUNCTION_LIST.C_GetAttributeValue = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_GetAttributeValue");
			cK_FUNCTION_LIST.C_SetAttributeValue = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_SetAttributeValue");
			cK_FUNCTION_LIST.C_FindObjectsInit = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_FindObjectsInit");
			cK_FUNCTION_LIST.C_FindObjects = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_FindObjects");
			cK_FUNCTION_LIST.C_FindObjectsFinal = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_FindObjectsFinal");
			cK_FUNCTION_LIST.C_EncryptInit = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_EncryptInit");
			cK_FUNCTION_LIST.C_Encrypt = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_Encrypt");
			cK_FUNCTION_LIST.C_EncryptUpdate = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_EncryptUpdate");
			cK_FUNCTION_LIST.C_EncryptFinal = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_EncryptFinal");
			cK_FUNCTION_LIST.C_DecryptInit = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_DecryptInit");
			cK_FUNCTION_LIST.C_Decrypt = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_Decrypt");
			cK_FUNCTION_LIST.C_DecryptUpdate = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_DecryptUpdate");
			cK_FUNCTION_LIST.C_DecryptFinal = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_DecryptFinal");
			cK_FUNCTION_LIST.C_DigestInit = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_DigestInit");
			cK_FUNCTION_LIST.C_Digest = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_Digest");
			cK_FUNCTION_LIST.C_DigestUpdate = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_DigestUpdate");
			cK_FUNCTION_LIST.C_DigestKey = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_DigestKey");
			cK_FUNCTION_LIST.C_DigestFinal = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_DigestFinal");
			cK_FUNCTION_LIST.C_SignInit = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_SignInit");
			cK_FUNCTION_LIST.C_Sign = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_Sign");
			cK_FUNCTION_LIST.C_SignUpdate = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_SignUpdate");
			cK_FUNCTION_LIST.C_SignFinal = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_SignFinal");
			cK_FUNCTION_LIST.C_SignRecoverInit = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_SignRecoverInit");
			cK_FUNCTION_LIST.C_SignRecover = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_SignRecover");
			cK_FUNCTION_LIST.C_VerifyInit = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_VerifyInit");
			cK_FUNCTION_LIST.C_Verify = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_Verify");
			cK_FUNCTION_LIST.C_VerifyUpdate = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_VerifyUpdate");
			cK_FUNCTION_LIST.C_VerifyFinal = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_VerifyFinal");
			cK_FUNCTION_LIST.C_VerifyRecoverInit = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_VerifyRecoverInit");
			cK_FUNCTION_LIST.C_VerifyRecover = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_VerifyRecover");
			cK_FUNCTION_LIST.C_DigestEncryptUpdate = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_DigestEncryptUpdate");
			cK_FUNCTION_LIST.C_DecryptDigestUpdate = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_DecryptDigestUpdate");
			cK_FUNCTION_LIST.C_SignEncryptUpdate = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_SignEncryptUpdate");
			cK_FUNCTION_LIST.C_DecryptVerifyUpdate = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_DecryptVerifyUpdate");
			cK_FUNCTION_LIST.C_GenerateKey = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_GenerateKey");
			cK_FUNCTION_LIST.C_GenerateKeyPair = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_GenerateKeyPair");
			cK_FUNCTION_LIST.C_WrapKey = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_WrapKey");
			cK_FUNCTION_LIST.C_UnwrapKey = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_UnwrapKey");
			cK_FUNCTION_LIST.C_DeriveKey = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_DeriveKey");
			cK_FUNCTION_LIST.C_SeedRandom = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_SeedRandom");
			cK_FUNCTION_LIST.C_GenerateRandom = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_GenerateRandom");
			cK_FUNCTION_LIST.C_GetFunctionStatus = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_GetFunctionStatus");
			cK_FUNCTION_LIST.C_CancelFunction = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_CancelFunction");
			cK_FUNCTION_LIST.C_WaitForSlotEvent = UnmanagedLibrary.GetFunctionPointer(_libraryHandle, "C_WaitForSlotEvent");
			functionList = cK_FUNCTION_LIST;
		}

		public CKR C_GetSlotList(bool tokenPresent, uint[] slotList, ref uint count)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_GetSlotListDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_GetSlotList, typeof(C_GetSlotListDelegate)))(tokenPresent, slotList, ref count);
		}

		public CKR C_GetSlotInfo(uint slotId, ref CK_SLOT_INFO info)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_GetSlotInfoDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_GetSlotInfo, typeof(C_GetSlotInfoDelegate)))(slotId, ref info);
		}

		public CKR C_GetTokenInfo(uint slotId, ref CK_TOKEN_INFO info)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_GetTokenInfoDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_GetTokenInfo, typeof(C_GetTokenInfoDelegate)))(slotId, ref info);
		}

		public CKR C_GetMechanismList(uint slotId, CKM[] mechanismList, ref uint count)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			uint[] array = null;
			if (mechanismList != null)
			{
				array = new uint[mechanismList.Length];
			}
			CKR result = ((C_GetMechanismListDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_GetMechanismList, typeof(C_GetMechanismListDelegate)))(slotId, array, ref count);
			if (mechanismList != null)
			{
				for (int i = 0; i < mechanismList.Length; i++)
				{
					mechanismList[i] = (CKM)array[i];
				}
			}
			return result;
		}

		public CKR C_GetMechanismInfo(uint slotId, CKM type, ref CK_MECHANISM_INFO info)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_GetMechanismInfoDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_GetMechanismInfo, typeof(C_GetMechanismInfoDelegate)))(slotId, (uint)type, ref info);
		}

		public CKR C_InitToken(uint slotId, byte[] pin, uint pinLen, byte[] label)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_InitTokenDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_InitToken, typeof(C_InitTokenDelegate)))(slotId, pin, pinLen, label);
		}

		public CKR C_InitPIN(uint session, byte[] pin, uint pinLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_InitPINDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_InitPIN, typeof(C_InitPINDelegate)))(session, pin, pinLen);
		}

		public CKR C_SetPIN(uint session, byte[] oldPin, uint oldPinLen, byte[] newPin, uint newPinLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_SetPINDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_SetPIN, typeof(C_SetPINDelegate)))(session, oldPin, oldPinLen, newPin, newPinLen);
		}

		public CKR C_OpenSession(uint slotId, uint flags, IntPtr application, IntPtr notify, ref uint session)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_OpenSessionDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_OpenSession, typeof(C_OpenSessionDelegate)))(slotId, flags, application, notify, ref session);
		}

		public CKR C_CloseSession(uint session)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_CloseSessionDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_CloseSession, typeof(C_CloseSessionDelegate)))(session);
		}

		public CKR C_CloseAllSessions(uint slotId)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_CloseAllSessionsDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_CloseAllSessions, typeof(C_CloseAllSessionsDelegate)))(slotId);
		}

		public CKR C_GetSessionInfo(uint session, ref CK_SESSION_INFO info)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_GetSessionInfoDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_GetSessionInfo, typeof(C_GetSessionInfoDelegate)))(session, ref info);
		}

		public CKR C_GetOperationState(uint session, byte[] operationState, ref uint operationStateLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_GetOperationStateDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_GetOperationState, typeof(C_GetOperationStateDelegate)))(session, operationState, ref operationStateLen);
		}

		public CKR C_SetOperationState(uint session, byte[] operationState, uint operationStateLen, uint encryptionKey, uint authenticationKey)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_SetOperationStateDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_SetOperationState, typeof(C_SetOperationStateDelegate)))(session, operationState, operationStateLen, encryptionKey, authenticationKey);
		}

		public CKR C_Login(uint session, CKU userType, byte[] pin, uint pinLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_LoginDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_Login, typeof(C_LoginDelegate)))(session, (uint)userType, pin, pinLen);
		}

		public CKR C_Logout(uint session)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_LogoutDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_Logout, typeof(C_LogoutDelegate)))(session);
		}

		public CKR C_CreateObject(uint session, CK_ATTRIBUTE[] template, uint count, ref uint objectId)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_CreateObjectDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_CreateObject, typeof(C_CreateObjectDelegate)))(session, template, count, ref objectId);
		}

		public CKR C_CopyObject(uint session, uint objectId, CK_ATTRIBUTE[] template, uint count, ref uint newObjectId)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_CopyObjectDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_CopyObject, typeof(C_CopyObjectDelegate)))(session, objectId, template, count, ref newObjectId);
		}

		public CKR C_DestroyObject(uint session, uint objectId)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_DestroyObjectDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_DestroyObject, typeof(C_DestroyObjectDelegate)))(session, objectId);
		}

		public CKR C_GetObjectSize(uint session, uint objectId, ref uint size)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_GetObjectSizeDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_GetObjectSize, typeof(C_GetObjectSizeDelegate)))(session, objectId, ref size);
		}

		public CKR C_GetAttributeValue(uint session, uint objectId, CK_ATTRIBUTE[] template, uint count)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_GetAttributeValueDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_GetAttributeValue, typeof(C_GetAttributeValueDelegate)))(session, objectId, template, count);
		}

		public CKR C_SetAttributeValue(uint session, uint objectId, CK_ATTRIBUTE[] template, uint count)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_SetAttributeValueDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_SetAttributeValue, typeof(C_SetAttributeValueDelegate)))(session, objectId, template, count);
		}

		public CKR C_FindObjectsInit(uint session, CK_ATTRIBUTE[] template, uint count)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_FindObjectsInitDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_FindObjectsInit, typeof(C_FindObjectsInitDelegate)))(session, template, count);
		}

		public CKR C_FindObjects(uint session, uint[] objectId, uint maxObjectCount, ref uint objectCount)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_FindObjectsDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_FindObjects, typeof(C_FindObjectsDelegate)))(session, objectId, maxObjectCount, ref objectCount);
		}

		public CKR C_FindObjectsFinal(uint session)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_FindObjectsFinalDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_FindObjectsFinal, typeof(C_FindObjectsFinalDelegate)))(session);
		}

		public CKR C_EncryptInit(uint session, ref CK_MECHANISM mechanism, uint key)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_EncryptInitDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_EncryptInit, typeof(C_EncryptInitDelegate)))(session, ref mechanism, key);
		}

		public CKR C_Encrypt(uint session, byte[] data, uint dataLen, byte[] encryptedData, ref uint encryptedDataLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_EncryptDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_Encrypt, typeof(C_EncryptDelegate)))(session, data, dataLen, encryptedData, ref encryptedDataLen);
		}

		public CKR C_EncryptUpdate(uint session, byte[] part, uint partLen, byte[] encryptedPart, ref uint encryptedPartLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_EncryptUpdateDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_EncryptUpdate, typeof(C_EncryptUpdateDelegate)))(session, part, partLen, encryptedPart, ref encryptedPartLen);
		}

		public CKR C_EncryptFinal(uint session, byte[] lastEncryptedPart, ref uint lastEncryptedPartLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_EncryptFinalDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_EncryptFinal, typeof(C_EncryptFinalDelegate)))(session, lastEncryptedPart, ref lastEncryptedPartLen);
		}

		public CKR C_DecryptInit(uint session, ref CK_MECHANISM mechanism, uint key)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_DecryptInitDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_DecryptInit, typeof(C_DecryptInitDelegate)))(session, ref mechanism, key);
		}

		public CKR C_Decrypt(uint session, byte[] encryptedData, uint encryptedDataLen, byte[] data, ref uint dataLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_DecryptDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_Decrypt, typeof(C_DecryptDelegate)))(session, encryptedData, encryptedDataLen, data, ref dataLen);
		}

		public CKR C_DecryptUpdate(uint session, byte[] encryptedPart, uint encryptedPartLen, byte[] part, ref uint partLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_DecryptUpdateDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_DecryptUpdate, typeof(C_DecryptUpdateDelegate)))(session, encryptedPart, encryptedPartLen, part, ref partLen);
		}

		public CKR C_DecryptFinal(uint session, byte[] lastPart, ref uint lastPartLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_DecryptFinalDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_DecryptFinal, typeof(C_DecryptFinalDelegate)))(session, lastPart, ref lastPartLen);
		}

		public CKR C_DigestInit(uint session, ref CK_MECHANISM mechanism)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_DigestInitDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_DigestInit, typeof(C_DigestInitDelegate)))(session, ref mechanism);
		}

		public CKR C_Digest(uint session, byte[] data, uint dataLen, byte[] digest, ref uint digestLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_DigestDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_Digest, typeof(C_DigestDelegate)))(session, data, dataLen, digest, ref digestLen);
		}

		public CKR C_DigestUpdate(uint session, byte[] part, uint partLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_DigestUpdateDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_DigestUpdate, typeof(C_DigestUpdateDelegate)))(session, part, partLen);
		}

		public CKR C_DigestKey(uint session, uint key)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_DigestKeyDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_DigestKey, typeof(C_DigestKeyDelegate)))(session, key);
		}

		public CKR C_DigestFinal(uint session, byte[] digest, ref uint digestLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_DigestFinalDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_DigestFinal, typeof(C_DigestFinalDelegate)))(session, digest, ref digestLen);
		}

		public CKR C_SignInit(uint session, ref CK_MECHANISM mechanism, uint key)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_SignInitDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_SignInit, typeof(C_SignInitDelegate)))(session, ref mechanism, key);
		}

		public CKR C_Sign(uint session, byte[] data, uint dataLen, byte[] signature, ref uint signatureLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_SignDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_Sign, typeof(C_SignDelegate)))(session, data, dataLen, signature, ref signatureLen);
		}

		public CKR C_SignUpdate(uint session, byte[] part, uint partLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_SignUpdateDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_SignUpdate, typeof(C_SignUpdateDelegate)))(session, part, partLen);
		}

		public CKR C_SignFinal(uint session, byte[] signature, ref uint signatureLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_SignFinalDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_SignFinal, typeof(C_SignFinalDelegate)))(session, signature, ref signatureLen);
		}

		public CKR C_SignRecoverInit(uint session, ref CK_MECHANISM mechanism, uint key)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_SignRecoverInitDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_SignRecoverInit, typeof(C_SignRecoverInitDelegate)))(session, ref mechanism, key);
		}

		public CKR C_SignRecover(uint session, byte[] data, uint dataLen, byte[] signature, ref uint signatureLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_SignRecoverDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_SignRecover, typeof(C_SignRecoverDelegate)))(session, data, dataLen, signature, ref signatureLen);
		}

		public CKR C_VerifyInit(uint session, ref CK_MECHANISM mechanism, uint key)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_VerifyInitDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_VerifyInit, typeof(C_VerifyInitDelegate)))(session, ref mechanism, key);
		}

		public CKR C_Verify(uint session, byte[] data, uint dataLen, byte[] signature, uint signatureLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_VerifyDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_Verify, typeof(C_VerifyDelegate)))(session, data, dataLen, signature, signatureLen);
		}

		public CKR C_VerifyUpdate(uint session, byte[] part, uint partLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_VerifyUpdateDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_VerifyUpdate, typeof(C_VerifyUpdateDelegate)))(session, part, partLen);
		}

		public CKR C_VerifyFinal(uint session, byte[] signature, uint signatureLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_VerifyFinalDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_VerifyFinal, typeof(C_VerifyFinalDelegate)))(session, signature, signatureLen);
		}

		public CKR C_VerifyRecoverInit(uint session, ref CK_MECHANISM mechanism, uint key)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_VerifyRecoverInitDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_VerifyRecoverInit, typeof(C_VerifyRecoverInitDelegate)))(session, ref mechanism, key);
		}

		public CKR C_VerifyRecover(uint session, byte[] signature, uint signatureLen, byte[] data, ref uint dataLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_VerifyRecoverDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_VerifyRecover, typeof(C_VerifyRecoverDelegate)))(session, signature, signatureLen, data, ref dataLen);
		}

		public CKR C_DigestEncryptUpdate(uint session, byte[] part, uint partLen, byte[] encryptedPart, ref uint encryptedPartLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_DigestEncryptUpdateDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_DigestEncryptUpdate, typeof(C_DigestEncryptUpdateDelegate)))(session, part, partLen, encryptedPart, ref encryptedPartLen);
		}

		public CKR C_DecryptDigestUpdate(uint session, byte[] encryptedPart, uint encryptedPartLen, byte[] part, ref uint partLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_DecryptDigestUpdateDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_DecryptDigestUpdate, typeof(C_DecryptDigestUpdateDelegate)))(session, encryptedPart, encryptedPartLen, part, ref partLen);
		}

		public CKR C_SignEncryptUpdate(uint session, byte[] part, uint partLen, byte[] encryptedPart, ref uint encryptedPartLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_SignEncryptUpdateDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_SignEncryptUpdate, typeof(C_SignEncryptUpdateDelegate)))(session, part, partLen, encryptedPart, ref encryptedPartLen);
		}

		public CKR C_DecryptVerifyUpdate(uint session, byte[] encryptedPart, uint encryptedPartLen, byte[] part, ref uint partLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_DecryptVerifyUpdateDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_DecryptVerifyUpdate, typeof(C_DecryptVerifyUpdateDelegate)))(session, encryptedPart, encryptedPartLen, part, ref partLen);
		}

		public CKR C_GenerateKey(uint session, ref CK_MECHANISM mechanism, CK_ATTRIBUTE[] template, uint count, ref uint key)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_GenerateKeyDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_GenerateKey, typeof(C_GenerateKeyDelegate)))(session, ref mechanism, template, count, ref key);
		}

		public CKR C_GenerateKeyPair(uint session, ref CK_MECHANISM mechanism, CK_ATTRIBUTE[] publicKeyTemplate, uint publicKeyAttributeCount, CK_ATTRIBUTE[] privateKeyTemplate, uint privateKeyAttributeCount, ref uint publicKey, ref uint privateKey)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_GenerateKeyPairDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_GenerateKeyPair, typeof(C_GenerateKeyPairDelegate)))(session, ref mechanism, publicKeyTemplate, publicKeyAttributeCount, privateKeyTemplate, privateKeyAttributeCount, ref publicKey, ref privateKey);
		}

		public CKR C_WrapKey(uint session, ref CK_MECHANISM mechanism, uint wrappingKey, uint key, byte[] wrappedKey, ref uint wrappedKeyLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_WrapKeyDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_WrapKey, typeof(C_WrapKeyDelegate)))(session, ref mechanism, wrappingKey, key, wrappedKey, ref wrappedKeyLen);
		}

		public CKR C_UnwrapKey(uint session, ref CK_MECHANISM mechanism, uint unwrappingKey, byte[] wrappedKey, uint wrappedKeyLen, CK_ATTRIBUTE[] template, uint attributeCount, ref uint key)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_UnwrapKeyDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_UnwrapKey, typeof(C_UnwrapKeyDelegate)))(session, ref mechanism, unwrappingKey, wrappedKey, wrappedKeyLen, template, attributeCount, ref key);
		}

		public CKR C_DeriveKey(uint session, ref CK_MECHANISM mechanism, uint baseKey, CK_ATTRIBUTE[] template, uint attributeCount, ref uint key)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_DeriveKeyDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_DeriveKey, typeof(C_DeriveKeyDelegate)))(session, ref mechanism, baseKey, template, attributeCount, ref key);
		}

		public CKR C_SeedRandom(uint session, byte[] seed, uint seedLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_SeedRandomDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_SeedRandom, typeof(C_SeedRandomDelegate)))(session, seed, seedLen);
		}

		public CKR C_GenerateRandom(uint session, byte[] randomData, uint randomLen)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_GenerateRandomDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_GenerateRandom, typeof(C_GenerateRandomDelegate)))(session, randomData, randomLen);
		}

		public CKR C_GetFunctionStatus(uint session)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_GetFunctionStatusDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_GetFunctionStatus, typeof(C_GetFunctionStatusDelegate)))(session);
		}

		public CKR C_CancelFunction(uint session)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_CancelFunctionDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_CancelFunction, typeof(C_CancelFunctionDelegate)))(session);
		}

		public CKR C_WaitForSlotEvent(uint flags, ref uint slot, IntPtr reserved)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return ((C_WaitForSlotEventDelegate)Marshal.GetDelegateForFunctionPointer(_functionList.C_WaitForSlotEvent, typeof(C_WaitForSlotEventDelegate)))(flags, ref slot, reserved);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				Release();
				_disposed = true;
			}
		}

		~Pkcs11()
		{
			Dispose(false);
		}
	}
}
