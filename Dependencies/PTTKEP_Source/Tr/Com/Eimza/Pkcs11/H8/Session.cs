using System;
using System.Collections.Generic;
using System.IO;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8;

namespace Tr.Com.Eimza.Pkcs11.H8
{
	internal class Session : IDisposable
	{
		private bool _disposed;

		private Tr.Com.Eimza.Pkcs11.L8.Pkcs11 _p11;

		private ulong _sessionId;

		public ulong SessionId
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return _sessionId;
			}
		}

		internal Session(Tr.Com.Eimza.Pkcs11.L8.Pkcs11 pkcs11, ulong sessionId)
		{
			if (pkcs11 == null)
			{
				throw new ArgumentNullException("pkcs11");
			}
			if (sessionId == 0L)
			{
				throw new ArgumentException("Invalid handle specified", "sessionId");
			}
			_p11 = pkcs11;
			_sessionId = sessionId;
		}

		public void CloseSession()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			CKR cKR = _p11.C_CloseSession(_sessionId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_CloseSession", cKR);
			}
			_sessionId = 0uL;
		}

		public void InitPin(string userPin)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			byte[] array = null;
			ulong pinLen = 0uL;
			if (userPin != null)
			{
				array = ConvertUtils.Utf8StringToBytes(userPin);
				pinLen = Convert.ToUInt64(array.Length);
			}
			CKR cKR = _p11.C_InitPIN(_sessionId, array, pinLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_InitPIN", cKR);
			}
		}

		public void InitPin(byte[] userPin)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			byte[] pin = null;
			ulong pinLen = 0uL;
			if (userPin != null)
			{
				pin = userPin;
				pinLen = Convert.ToUInt64(userPin.Length);
			}
			CKR cKR = _p11.C_InitPIN(_sessionId, pin, pinLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_InitPIN", cKR);
			}
		}

		public void SetPin(string oldPin, string newPin)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			byte[] array = null;
			ulong oldPinLen = 0uL;
			if (oldPin != null)
			{
				array = ConvertUtils.Utf8StringToBytes(oldPin);
				oldPinLen = Convert.ToUInt64(array.Length);
			}
			byte[] array2 = null;
			ulong newPinLen = 0uL;
			if (newPin != null)
			{
				array2 = ConvertUtils.Utf8StringToBytes(newPin);
				newPinLen = Convert.ToUInt64(array2.Length);
			}
			CKR cKR = _p11.C_SetPIN(_sessionId, array, oldPinLen, array2, newPinLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_SetPIN", cKR);
			}
		}

		public void SetPin(byte[] oldPin, byte[] newPin)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			byte[] oldPin2 = null;
			ulong oldPinLen = 0uL;
			if (oldPin != null)
			{
				oldPin2 = oldPin;
				oldPinLen = Convert.ToUInt64(oldPin.Length);
			}
			byte[] newPin2 = null;
			ulong newPinLen = 0uL;
			if (newPin != null)
			{
				newPin2 = newPin;
				newPinLen = Convert.ToUInt64(newPin.Length);
			}
			CKR cKR = _p11.C_SetPIN(_sessionId, oldPin2, oldPinLen, newPin2, newPinLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_SetPIN", cKR);
			}
		}

		public SessionInfo GetSessionInfo()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			CK_SESSION_INFO info = default(CK_SESSION_INFO);
			CKR cKR = _p11.C_GetSessionInfo(_sessionId, ref info);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_GetSessionInfo", cKR);
			}
			return new SessionInfo(_sessionId, info);
		}

		public byte[] GetOperationState()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			ulong operationStateLen = 0uL;
			CKR cKR = _p11.C_GetOperationState(_sessionId, null, ref operationStateLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_GetOperationState", cKR);
			}
			byte[] array = new byte[operationStateLen];
			cKR = _p11.C_GetOperationState(_sessionId, array, ref operationStateLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_GetOperationState", cKR);
			}
			return array;
		}

		public void SetOperationState(byte[] state, ObjectHandle encryptionKey, ObjectHandle authenticationKey)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (state == null)
			{
				throw new ArgumentNullException("state");
			}
			if (encryptionKey == null)
			{
				throw new ArgumentNullException("encryptionKey");
			}
			if (authenticationKey == null)
			{
				throw new ArgumentNullException("authenticationKey");
			}
			CKR cKR = _p11.C_SetOperationState(_sessionId, state, Convert.ToUInt64(state.Length), encryptionKey.ObjectId, authenticationKey.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_SetOperationState", cKR);
			}
		}

		public void Login(CKU userType, string pin)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			byte[] array = null;
			ulong pinLen = 0uL;
			if (pin != null)
			{
				array = ConvertUtils.Utf8StringToBytes(pin);
				pinLen = Convert.ToUInt64(array.Length);
			}
			CKR cKR = _p11.C_Login(_sessionId, userType, array, pinLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_Login", cKR);
			}
		}

		public void Login(CKU userType, byte[] pin)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			byte[] pin2 = null;
			ulong pinLen = 0uL;
			if (pin != null)
			{
				pin2 = pin;
				pinLen = Convert.ToUInt64(pin.Length);
			}
			CKR cKR = _p11.C_Login(_sessionId, userType, pin2, pinLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_Login", cKR);
			}
		}

		public void Logout()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			CKR cKR = _p11.C_Logout(_sessionId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_Logout", cKR);
			}
		}

		public ObjectHandle CreateObject(List<ObjectAttribute> attributes)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			ulong objectId = 0uL;
			CK_ATTRIBUTE[] array = null;
			ulong num = 0uL;
			if (attributes != null)
			{
				num = Convert.ToUInt64(attributes.Count);
				array = new CK_ATTRIBUTE[num];
				for (int i = 0; i < Convert.ToInt32(num); i++)
				{
					array[i] = attributes[i].CkAttribute;
				}
			}
			CKR cKR = _p11.C_CreateObject(_sessionId, array, num, ref objectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_CreateObject", cKR);
			}
			return new ObjectHandle(objectId);
		}

		public ObjectHandle CopyObject(ObjectHandle objectHandle, List<ObjectAttribute> attributes)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (objectHandle == null)
			{
				throw new ArgumentNullException("objectHandle");
			}
			ulong newObjectId = 0uL;
			CK_ATTRIBUTE[] array = null;
			ulong num = 0uL;
			if (attributes != null)
			{
				num = Convert.ToUInt64(attributes.Count);
				array = new CK_ATTRIBUTE[num];
				for (int i = 0; i < Convert.ToInt32(num); i++)
				{
					array[i] = attributes[i].CkAttribute;
				}
			}
			CKR cKR = _p11.C_CopyObject(_sessionId, objectHandle.ObjectId, array, num, ref newObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_CopyObject", cKR);
			}
			return new ObjectHandle(newObjectId);
		}

		public void DestroyObject(ObjectHandle objectHandle)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (objectHandle == null)
			{
				throw new ArgumentNullException("objectHandle");
			}
			CKR cKR = _p11.C_DestroyObject(_sessionId, objectHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DestroyObject", cKR);
			}
		}

		public ulong GetObjectSize(ObjectHandle objectHandle)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (objectHandle == null)
			{
				throw new ArgumentNullException("objectHandle");
			}
			ulong size = 0uL;
			CKR cKR = _p11.C_GetObjectSize(_sessionId, objectHandle.ObjectId, ref size);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_GetObjectSize", cKR);
			}
			return size;
		}

		public List<ObjectAttribute> GetAttributeValue(ObjectHandle objectHandle, List<CKA> attributes)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (objectHandle == null)
			{
				throw new ArgumentNullException("objectHandle");
			}
			if (attributes == null)
			{
				throw new ArgumentNullException("attributes");
			}
			if (attributes.Count < 1)
			{
				throw new ArgumentException("No attributes specified", "attributes");
			}
			List<ulong> list = new List<ulong>();
			foreach (CKA attribute in attributes)
			{
				list.Add(Convert.ToUInt64((uint)attribute));
			}
			return GetAttributeValue(objectHandle, list);
		}

		public List<ObjectAttribute> GetAttributeValue(ObjectHandle objectHandle, List<ulong> attributes)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (objectHandle == null)
			{
				throw new ArgumentNullException("objectHandle");
			}
			if (attributes == null)
			{
				throw new ArgumentNullException("attributes");
			}
			if (attributes.Count < 1)
			{
				throw new ArgumentException("No attributes specified", "attributes");
			}
			CK_ATTRIBUTE[] array = new CK_ATTRIBUTE[attributes.Count];
			for (int i = 0; i < attributes.Count; i++)
			{
				array[i] = CkaUtils.CreateAttribute(attributes[i]);
			}
			CKR cKR = _p11.C_GetAttributeValue(_sessionId, objectHandle.ObjectId, array, Convert.ToUInt64(array.Length));
			if (cKR != 0 && cKR != CKR.CKR_ATTRIBUTE_SENSITIVE && cKR != CKR.CKR_ATTRIBUTE_TYPE_INVALID)
			{
				throw new Pkcs11Exception("C_GetAttributeValue", cKR);
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].valueLen != ulong.MaxValue)
				{
					array[j].value = UnmanagedMemory.Allocate(Convert.ToInt32(array[j].valueLen));
				}
			}
			_p11.C_GetAttributeValue(_sessionId, objectHandle.ObjectId, array, Convert.ToUInt64(array.Length));
			if (cKR != 0 && cKR != CKR.CKR_ATTRIBUTE_SENSITIVE && cKR != CKR.CKR_ATTRIBUTE_TYPE_INVALID)
			{
				throw new Pkcs11Exception("C_GetAttributeValue", cKR);
			}
			List<ObjectAttribute> list = new List<ObjectAttribute>();
			for (int k = 0; k < array.Length; k++)
			{
				list.Add(new ObjectAttribute(array[k]));
			}
			return list;
		}

		public void SetAttributeValue(ObjectHandle objectHandle, List<ObjectAttribute> attributes)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (objectHandle == null)
			{
				throw new ArgumentNullException("objectHandle");
			}
			if (attributes == null)
			{
				throw new ArgumentNullException("attributes");
			}
			if (attributes.Count < 1)
			{
				throw new ArgumentException("No attributes specified", "attributes");
			}
			CK_ATTRIBUTE[] array = new CK_ATTRIBUTE[attributes.Count];
			for (int i = 0; i < attributes.Count; i++)
			{
				array[i] = attributes[i].CkAttribute;
			}
			CKR cKR = _p11.C_SetAttributeValue(_sessionId, objectHandle.ObjectId, array, Convert.ToUInt64(array.Length));
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_SetAttributeValue", cKR);
			}
		}

		public void FindObjectsInit(List<ObjectAttribute> attributes)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			CK_ATTRIBUTE[] array = null;
			ulong num = 0uL;
			if (attributes != null)
			{
				num = Convert.ToUInt64(attributes.Count);
				array = new CK_ATTRIBUTE[num];
				for (int i = 0; i < Convert.ToInt32(num); i++)
				{
					array[i] = attributes[i].CkAttribute;
				}
			}
			CKR cKR = _p11.C_FindObjectsInit(_sessionId, array, num);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_FindObjectsInit", cKR);
			}
		}

		public List<ObjectHandle> FindObjects(int objectCount)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			List<ObjectHandle> list = new List<ObjectHandle>();
			ulong[] array = new ulong[objectCount];
			ulong objectCount2 = 0uL;
			CKR cKR = _p11.C_FindObjects(_sessionId, array, Convert.ToUInt64(objectCount), ref objectCount2);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_FindObjects", cKR);
			}
			for (int i = 0; i < Convert.ToInt32(objectCount2); i++)
			{
				list.Add(new ObjectHandle(array[i]));
			}
			return list;
		}

		public void FindObjectsFinal()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			CKR cKR = _p11.C_FindObjectsFinal(_sessionId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_FindObjectsFinal", cKR);
			}
		}

		public List<ObjectHandle> FindAllObjects(List<ObjectAttribute> attributes)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			List<ObjectHandle> list = new List<ObjectHandle>();
			CK_ATTRIBUTE[] array = null;
			ulong num = 0uL;
			if (attributes != null)
			{
				num = Convert.ToUInt64(attributes.Count);
				array = new CK_ATTRIBUTE[num];
				for (int i = 0; i < Convert.ToInt32(num); i++)
				{
					array[i] = attributes[i].CkAttribute;
				}
			}
			CKR cKR = _p11.C_FindObjectsInit(_sessionId, array, num);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_FindObjectsInit", cKR);
			}
			ulong num2 = 256uL;
			ulong[] array2 = new ulong[num2];
			ulong objectCount = num2;
			while (objectCount == num2)
			{
				cKR = _p11.C_FindObjects(_sessionId, array2, num2, ref objectCount);
				if (cKR != 0)
				{
					throw new Pkcs11Exception("C_FindObjects", cKR);
				}
				for (int j = 0; j < Convert.ToInt32(objectCount); j++)
				{
					list.Add(new ObjectHandle(array2[j]));
				}
			}
			cKR = _p11.C_FindObjectsFinal(_sessionId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_FindObjectsFinal", cKR);
			}
			return list;
		}

		public byte[] Encrypt(Mechanism mechanism, ObjectHandle keyHandle, byte[] data)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CKR cKR = _p11.C_EncryptInit(_sessionId, ref mechanism2, keyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_EncryptInit", cKR);
			}
			ulong encryptedDataLen = 0uL;
			cKR = _p11.C_Encrypt(_sessionId, data, Convert.ToUInt64(data.Length), null, ref encryptedDataLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_Encrypt", cKR);
			}
			byte[] array = new byte[encryptedDataLen];
			cKR = _p11.C_Encrypt(_sessionId, data, Convert.ToUInt64(data.Length), array, ref encryptedDataLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_Encrypt", cKR);
			}
			if (array.Length != Convert.ToInt32(encryptedDataLen))
			{
				Array.Resize(ref array, Convert.ToInt32(encryptedDataLen));
			}
			return array;
		}

		public void Encrypt(Mechanism mechanism, ObjectHandle keyHandle, Stream inputStream, Stream outputStream)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			Encrypt(mechanism, keyHandle, inputStream, outputStream, 4096);
		}

		public void Encrypt(Mechanism mechanism, ObjectHandle keyHandle, Stream inputStream, Stream outputStream, int bufferLength)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (bufferLength < 1)
			{
				throw new ArgumentException("Value has to be positive number", "bufferLength");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CKR cKR = _p11.C_EncryptInit(_sessionId, ref mechanism2, keyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_EncryptInit", cKR);
			}
			byte[] array = new byte[bufferLength];
			byte[] array2 = new byte[bufferLength];
			ulong num = Convert.ToUInt64(array2.Length);
			int num2 = 0;
			while ((num2 = inputStream.Read(array, 0, array.Length)) > 0)
			{
				num = Convert.ToUInt64(array2.Length);
				cKR = _p11.C_EncryptUpdate(_sessionId, array, Convert.ToUInt64(num2), array2, ref num);
				if (cKR != 0)
				{
					throw new Pkcs11Exception("C_EncryptUpdate", cKR);
				}
				outputStream.Write(array2, 0, Convert.ToInt32(num));
			}
			byte[] array3 = null;
			ulong lastEncryptedPartLen = 0uL;
			cKR = _p11.C_EncryptFinal(_sessionId, null, ref lastEncryptedPartLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_EncryptFinal", cKR);
			}
			array3 = new byte[lastEncryptedPartLen];
			cKR = _p11.C_EncryptFinal(_sessionId, array3, ref lastEncryptedPartLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_EncryptFinal", cKR);
			}
			if (lastEncryptedPartLen != 0)
			{
				outputStream.Write(array3, 0, Convert.ToInt32(lastEncryptedPartLen));
			}
		}

		public byte[] Decrypt(Mechanism mechanism, ObjectHandle keyHandle, byte[] encryptedData)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (encryptedData == null)
			{
				throw new ArgumentNullException("encryptedData");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CKR cKR = _p11.C_DecryptInit(_sessionId, ref mechanism2, keyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DecryptInit", cKR);
			}
			ulong dataLen = 0uL;
			cKR = _p11.C_Decrypt(_sessionId, encryptedData, Convert.ToUInt64(encryptedData.Length), null, ref dataLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_Decrypt", cKR);
			}
			byte[] array = new byte[dataLen];
			cKR = _p11.C_Decrypt(_sessionId, encryptedData, Convert.ToUInt64(encryptedData.Length), array, ref dataLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_Decrypt", cKR);
			}
			if (array.Length != Convert.ToInt32(dataLen))
			{
				Array.Resize(ref array, Convert.ToInt32(dataLen));
			}
			return array;
		}

		public void Decrypt(Mechanism mechanism, ObjectHandle keyHandle, Stream inputStream, Stream outputStream)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			Decrypt(mechanism, keyHandle, inputStream, outputStream, 4096);
		}

		public void Decrypt(Mechanism mechanism, ObjectHandle keyHandle, Stream inputStream, Stream outputStream, int bufferLength)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (bufferLength < 1)
			{
				throw new ArgumentException("Value has to be positive number", "bufferLength");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CKR cKR = _p11.C_DecryptInit(_sessionId, ref mechanism2, keyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DecryptInit", cKR);
			}
			byte[] array = new byte[bufferLength];
			byte[] array2 = new byte[bufferLength];
			ulong num = Convert.ToUInt64(array2.Length);
			int num2 = 0;
			while ((num2 = inputStream.Read(array, 0, array.Length)) > 0)
			{
				num = Convert.ToUInt64(array2.Length);
				cKR = _p11.C_DecryptUpdate(_sessionId, array, Convert.ToUInt64(num2), array2, ref num);
				if (cKR != 0)
				{
					throw new Pkcs11Exception("C_DecryptUpdate", cKR);
				}
				outputStream.Write(array2, 0, Convert.ToInt32(num));
			}
			byte[] array3 = null;
			ulong lastPartLen = 0uL;
			cKR = _p11.C_DecryptFinal(_sessionId, null, ref lastPartLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DecryptFinal", cKR);
			}
			array3 = new byte[lastPartLen];
			cKR = _p11.C_DecryptFinal(_sessionId, array3, ref lastPartLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DecryptFinal", cKR);
			}
			if (lastPartLen != 0)
			{
				outputStream.Write(array3, 0, Convert.ToInt32(lastPartLen));
			}
		}

		public byte[] DigestKey(Mechanism mechanism, ObjectHandle keyHandle)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CKR cKR = _p11.C_DigestInit(_sessionId, ref mechanism2);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DigestInit", cKR);
			}
			cKR = _p11.C_DigestKey(_sessionId, keyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DigestKey", cKR);
			}
			ulong digestLen = 0uL;
			cKR = _p11.C_DigestFinal(_sessionId, null, ref digestLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DigestFinal", cKR);
			}
			byte[] array = new byte[digestLen];
			cKR = _p11.C_DigestFinal(_sessionId, array, ref digestLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DigestFinal", cKR);
			}
			if (array.Length != Convert.ToInt32(digestLen))
			{
				Array.Resize(ref array, Convert.ToInt32(digestLen));
			}
			return array;
		}

		public byte[] Digest(Mechanism mechanism, byte[] data)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CKR cKR = _p11.C_DigestInit(_sessionId, ref mechanism2);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DigestInit", cKR);
			}
			ulong digestLen = 0uL;
			cKR = _p11.C_Digest(_sessionId, data, Convert.ToUInt64(data.Length), null, ref digestLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_Digest", cKR);
			}
			byte[] array = new byte[digestLen];
			cKR = _p11.C_Digest(_sessionId, data, Convert.ToUInt64(data.Length), array, ref digestLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_Digest", cKR);
			}
			if (array.Length != Convert.ToInt32(digestLen))
			{
				Array.Resize(ref array, Convert.ToInt32(digestLen));
			}
			return array;
		}

		public byte[] Digest(Mechanism mechanism, Stream inputStream)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			return Digest(mechanism, inputStream, 4096);
		}

		public byte[] Digest(Mechanism mechanism, Stream inputStream, int bufferLength)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (bufferLength < 1)
			{
				throw new ArgumentException("Value has to be positive number", "bufferLength");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CKR cKR = _p11.C_DigestInit(_sessionId, ref mechanism2);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DigestInit", cKR);
			}
			byte[] array = new byte[bufferLength];
			int num = 0;
			while ((num = inputStream.Read(array, 0, array.Length)) > 0)
			{
				cKR = _p11.C_DigestUpdate(_sessionId, array, Convert.ToUInt64(num));
				if (cKR != 0)
				{
					throw new Pkcs11Exception("C_DigestUpdate", cKR);
				}
			}
			ulong digestLen = 0uL;
			cKR = _p11.C_DigestFinal(_sessionId, null, ref digestLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DigestFinal", cKR);
			}
			byte[] array2 = new byte[digestLen];
			cKR = _p11.C_DigestFinal(_sessionId, array2, ref digestLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DigestFinal", cKR);
			}
			if (array2.Length != Convert.ToInt32(digestLen))
			{
				Array.Resize(ref array2, Convert.ToInt32(digestLen));
			}
			return array2;
		}

		public byte[] Sign(Mechanism mechanism, ObjectHandle keyHandle, byte[] data)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CKR cKR = _p11.C_SignInit(_sessionId, ref mechanism2, keyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_SignInit", cKR);
			}
			ulong signatureLen = 0uL;
			cKR = _p11.C_Sign(_sessionId, data, Convert.ToUInt64(data.Length), null, ref signatureLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_Sign", cKR);
			}
			byte[] array = new byte[signatureLen];
			cKR = _p11.C_Sign(_sessionId, data, Convert.ToUInt64(data.Length), array, ref signatureLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_Sign", cKR);
			}
			if (array.Length != Convert.ToInt32(signatureLen))
			{
				Array.Resize(ref array, Convert.ToInt32(signatureLen));
			}
			return array;
		}

		public byte[] Sign(Mechanism mechanism, ObjectHandle keyHandle, Stream inputStream)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			return Sign(mechanism, keyHandle, inputStream, 4096);
		}

		public byte[] Sign(Mechanism mechanism, ObjectHandle keyHandle, Stream inputStream, int bufferLength)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (bufferLength < 1)
			{
				throw new ArgumentException("Value has to be positive number", "bufferLength");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CKR cKR = _p11.C_SignInit(_sessionId, ref mechanism2, keyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_SignInit", cKR);
			}
			byte[] array = new byte[bufferLength];
			int num = 0;
			while ((num = inputStream.Read(array, 0, array.Length)) > 0)
			{
				cKR = _p11.C_SignUpdate(_sessionId, array, Convert.ToUInt64(num));
				if (cKR != 0)
				{
					throw new Pkcs11Exception("C_SignUpdate", cKR);
				}
			}
			ulong signatureLen = 0uL;
			cKR = _p11.C_SignFinal(_sessionId, null, ref signatureLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_SignFinal", cKR);
			}
			byte[] array2 = new byte[signatureLen];
			cKR = _p11.C_SignFinal(_sessionId, array2, ref signatureLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_SignFinal", cKR);
			}
			if (array2.Length != Convert.ToInt32(signatureLen))
			{
				Array.Resize(ref array2, Convert.ToInt32(signatureLen));
			}
			return array2;
		}

		public byte[] SignRecover(Mechanism mechanism, ObjectHandle keyHandle, byte[] data)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CKR cKR = _p11.C_SignRecoverInit(_sessionId, ref mechanism2, keyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_SignRecoverInit", cKR);
			}
			ulong signatureLen = 0uL;
			cKR = _p11.C_SignRecover(_sessionId, data, Convert.ToUInt64(data.Length), null, ref signatureLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_SignRecover", cKR);
			}
			byte[] array = new byte[signatureLen];
			cKR = _p11.C_SignRecover(_sessionId, data, Convert.ToUInt64(data.Length), array, ref signatureLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_SignRecover", cKR);
			}
			if (array.Length != Convert.ToInt32(signatureLen))
			{
				Array.Resize(ref array, Convert.ToInt32(signatureLen));
			}
			return array;
		}

		public void Verify(Mechanism mechanism, ObjectHandle keyHandle, byte[] data, byte[] signature, out bool isValid)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (signature == null)
			{
				throw new ArgumentNullException("signature");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CKR cKR = _p11.C_VerifyInit(_sessionId, ref mechanism2, keyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_VerifyInit", cKR);
			}
			cKR = _p11.C_Verify(_sessionId, data, Convert.ToUInt64(data.Length), signature, Convert.ToUInt64(signature.Length));
			switch (cKR)
			{
			case CKR.CKR_OK:
				isValid = true;
				break;
			case CKR.CKR_SIGNATURE_INVALID:
				isValid = false;
				break;
			default:
				throw new Pkcs11Exception("C_Verify", cKR);
			}
		}

		public void Verify(Mechanism mechanism, ObjectHandle keyHandle, Stream inputStream, byte[] signature, out bool isValid)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (signature == null)
			{
				throw new ArgumentNullException("signature");
			}
			Verify(mechanism, keyHandle, inputStream, signature, out isValid, 4096);
		}

		public void Verify(Mechanism mechanism, ObjectHandle keyHandle, Stream inputStream, byte[] signature, out bool isValid, int bufferLength)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (signature == null)
			{
				throw new ArgumentNullException("signature");
			}
			if (bufferLength < 1)
			{
				throw new ArgumentException("Value has to be positive number", "bufferLength");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CKR cKR = _p11.C_VerifyInit(_sessionId, ref mechanism2, keyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_VerifyInit", cKR);
			}
			byte[] array = new byte[bufferLength];
			int num = 0;
			while ((num = inputStream.Read(array, 0, array.Length)) > 0)
			{
				cKR = _p11.C_VerifyUpdate(_sessionId, array, Convert.ToUInt64(num));
				if (cKR != 0)
				{
					throw new Pkcs11Exception("C_VerifyUpdate", cKR);
				}
			}
			cKR = _p11.C_VerifyFinal(_sessionId, signature, Convert.ToUInt64(signature.Length));
			switch (cKR)
			{
			case CKR.CKR_OK:
				isValid = true;
				break;
			case CKR.CKR_SIGNATURE_INVALID:
				isValid = false;
				break;
			default:
				throw new Pkcs11Exception("C_VerifyFinal", cKR);
			}
		}

		public byte[] VerifyRecover(Mechanism mechanism, ObjectHandle keyHandle, byte[] signature, out bool isValid)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (signature == null)
			{
				throw new ArgumentNullException("signature");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CKR cKR = _p11.C_VerifyRecoverInit(_sessionId, ref mechanism2, keyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_VerifyRecoverInit", cKR);
			}
			ulong dataLen = 0uL;
			cKR = _p11.C_VerifyRecover(_sessionId, signature, Convert.ToUInt64(signature.Length), null, ref dataLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_VerifyRecover", cKR);
			}
			byte[] array = new byte[dataLen];
			cKR = _p11.C_VerifyRecover(_sessionId, signature, Convert.ToUInt64(signature.Length), array, ref dataLen);
			switch (cKR)
			{
			case CKR.CKR_OK:
				isValid = true;
				break;
			case CKR.CKR_SIGNATURE_INVALID:
				isValid = false;
				break;
			default:
				throw new Pkcs11Exception("C_VerifyRecover", cKR);
			}
			if (array.Length != Convert.ToInt32(dataLen))
			{
				Array.Resize(ref array, Convert.ToInt32(dataLen));
			}
			return array;
		}

		public void DigestEncrypt(Mechanism digestingMechanism, Mechanism encryptionMechanism, ObjectHandle keyHandle, byte[] data, out byte[] digest, out byte[] encryptedData)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (digestingMechanism == null)
			{
				throw new ArgumentNullException("digestingMechanism");
			}
			if (encryptionMechanism == null)
			{
				throw new ArgumentNullException("encryptionMechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			using (MemoryStream inputStream = new MemoryStream(data))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					digest = DigestEncrypt(digestingMechanism, encryptionMechanism, keyHandle, inputStream, memoryStream);
					encryptedData = memoryStream.ToArray();
				}
			}
		}

		public byte[] DigestEncrypt(Mechanism digestingMechanism, Mechanism encryptionMechanism, ObjectHandle keyHandle, Stream inputStream, Stream outputStream)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (digestingMechanism == null)
			{
				throw new ArgumentNullException("digestingMechanism");
			}
			if (encryptionMechanism == null)
			{
				throw new ArgumentNullException("encryptionMechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			return DigestEncrypt(digestingMechanism, encryptionMechanism, keyHandle, inputStream, outputStream, 4096);
		}

		public byte[] DigestEncrypt(Mechanism digestingMechanism, Mechanism encryptionMechanism, ObjectHandle keyHandle, Stream inputStream, Stream outputStream, int bufferLength)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (digestingMechanism == null)
			{
				throw new ArgumentNullException("digestingMechanism");
			}
			if (encryptionMechanism == null)
			{
				throw new ArgumentNullException("encryptionMechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (bufferLength < 1)
			{
				throw new ArgumentException("Value has to be positive number", "bufferLength");
			}
			CK_MECHANISM mechanism = digestingMechanism.CkMechanism;
			CKR cKR = _p11.C_DigestInit(_sessionId, ref mechanism);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DigestInit", cKR);
			}
			CK_MECHANISM mechanism2 = encryptionMechanism.CkMechanism;
			cKR = _p11.C_EncryptInit(_sessionId, ref mechanism2, keyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_EncryptInit", cKR);
			}
			byte[] array = new byte[bufferLength];
			byte[] array2 = new byte[bufferLength];
			ulong num = Convert.ToUInt64(array2.Length);
			int num2 = 0;
			while ((num2 = inputStream.Read(array, 0, array.Length)) > 0)
			{
				num = Convert.ToUInt64(array2.Length);
				cKR = _p11.C_DigestEncryptUpdate(_sessionId, array, Convert.ToUInt64(num2), array2, ref num);
				if (cKR != 0)
				{
					throw new Pkcs11Exception("C_DigestEncryptUpdate", cKR);
				}
				outputStream.Write(array2, 0, Convert.ToInt32(num));
			}
			byte[] array3 = null;
			ulong lastEncryptedPartLen = 0uL;
			cKR = _p11.C_EncryptFinal(_sessionId, null, ref lastEncryptedPartLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_EncryptFinal", cKR);
			}
			array3 = new byte[lastEncryptedPartLen];
			cKR = _p11.C_EncryptFinal(_sessionId, array3, ref lastEncryptedPartLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_EncryptFinal", cKR);
			}
			if (lastEncryptedPartLen != 0)
			{
				outputStream.Write(array3, 0, Convert.ToInt32(lastEncryptedPartLen));
			}
			ulong digestLen = 0uL;
			cKR = _p11.C_DigestFinal(_sessionId, null, ref digestLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DigestFinal", cKR);
			}
			byte[] array4 = new byte[digestLen];
			cKR = _p11.C_DigestFinal(_sessionId, array4, ref digestLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DigestFinal", cKR);
			}
			if (array4.Length != Convert.ToInt32(digestLen))
			{
				Array.Resize(ref array4, Convert.ToInt32(digestLen));
			}
			return array4;
		}

		public void DecryptDigest(Mechanism digestingMechanism, Mechanism decryptionMechanism, ObjectHandle keyHandle, byte[] data, out byte[] digest, out byte[] decryptedData)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (digestingMechanism == null)
			{
				throw new ArgumentNullException("digestingMechanism");
			}
			if (decryptionMechanism == null)
			{
				throw new ArgumentNullException("decryptionMechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			using (MemoryStream inputStream = new MemoryStream(data))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					digest = DecryptDigest(digestingMechanism, decryptionMechanism, keyHandle, inputStream, memoryStream);
					decryptedData = memoryStream.ToArray();
				}
			}
		}

		public byte[] DecryptDigest(Mechanism digestingMechanism, Mechanism decryptionMechanism, ObjectHandle keyHandle, Stream inputStream, Stream outputStream)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (digestingMechanism == null)
			{
				throw new ArgumentNullException("digestingMechanism");
			}
			if (decryptionMechanism == null)
			{
				throw new ArgumentNullException("decryptionMechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			return DecryptDigest(digestingMechanism, decryptionMechanism, keyHandle, inputStream, outputStream, 4096);
		}

		public byte[] DecryptDigest(Mechanism digestingMechanism, Mechanism decryptionMechanism, ObjectHandle keyHandle, Stream inputStream, Stream outputStream, int bufferLength)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (digestingMechanism == null)
			{
				throw new ArgumentNullException("digestingMechanism");
			}
			if (decryptionMechanism == null)
			{
				throw new ArgumentNullException("decryptionMechanism");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (bufferLength < 1)
			{
				throw new ArgumentException("Value has to be positive number", "bufferLength");
			}
			CK_MECHANISM mechanism = digestingMechanism.CkMechanism;
			CKR cKR = _p11.C_DigestInit(_sessionId, ref mechanism);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DigestInit", cKR);
			}
			CK_MECHANISM mechanism2 = decryptionMechanism.CkMechanism;
			cKR = _p11.C_DecryptInit(_sessionId, ref mechanism2, keyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DecryptInit", cKR);
			}
			byte[] array = new byte[bufferLength];
			byte[] array2 = new byte[bufferLength];
			ulong num = Convert.ToUInt64(array2.Length);
			int num2 = 0;
			while ((num2 = inputStream.Read(array, 0, array.Length)) > 0)
			{
				num = Convert.ToUInt64(array2.Length);
				cKR = _p11.C_DecryptDigestUpdate(_sessionId, array, Convert.ToUInt64(num2), array2, ref num);
				if (cKR != 0)
				{
					throw new Pkcs11Exception("C_DecryptDigestUpdate", cKR);
				}
				outputStream.Write(array2, 0, Convert.ToInt32(num));
			}
			byte[] array3 = null;
			ulong lastPartLen = 0uL;
			cKR = _p11.C_DecryptFinal(_sessionId, null, ref lastPartLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DecryptFinal", cKR);
			}
			array3 = new byte[lastPartLen];
			cKR = _p11.C_DecryptFinal(_sessionId, array3, ref lastPartLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DecryptFinal", cKR);
			}
			if (lastPartLen != 0)
			{
				outputStream.Write(array3, 0, Convert.ToInt32(lastPartLen));
			}
			ulong digestLen = 0uL;
			cKR = _p11.C_DigestFinal(_sessionId, null, ref digestLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DigestFinal", cKR);
			}
			byte[] array4 = new byte[digestLen];
			cKR = _p11.C_DigestFinal(_sessionId, array4, ref digestLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DigestFinal", cKR);
			}
			if (array4.Length != Convert.ToInt32(digestLen))
			{
				Array.Resize(ref array4, Convert.ToInt32(digestLen));
			}
			return array4;
		}

		public void SignEncrypt(Mechanism signingMechanism, ObjectHandle signingKeyHandle, Mechanism encryptionMechanism, ObjectHandle encryptionKeyHandle, byte[] data, out byte[] signature, out byte[] encryptedData)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (signingMechanism == null)
			{
				throw new ArgumentNullException("signingMechanism");
			}
			if (signingKeyHandle == null)
			{
				throw new ArgumentNullException("signingKeyHandle");
			}
			if (encryptionMechanism == null)
			{
				throw new ArgumentNullException("encryptionMechanism");
			}
			if (encryptionKeyHandle == null)
			{
				throw new ArgumentNullException("encryptionKeyHandle");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			using (MemoryStream inputStream = new MemoryStream(data))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					signature = SignEncrypt(signingMechanism, signingKeyHandle, encryptionMechanism, encryptionKeyHandle, inputStream, memoryStream);
					encryptedData = memoryStream.ToArray();
				}
			}
		}

		public byte[] SignEncrypt(Mechanism signingMechanism, ObjectHandle signingKeyHandle, Mechanism encryptionMechanism, ObjectHandle encryptionKeyHandle, Stream inputStream, Stream outputStream)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (signingMechanism == null)
			{
				throw new ArgumentNullException("signingMechanism");
			}
			if (signingKeyHandle == null)
			{
				throw new ArgumentNullException("signingKeyHandle");
			}
			if (encryptionMechanism == null)
			{
				throw new ArgumentNullException("encryptionMechanism");
			}
			if (encryptionKeyHandle == null)
			{
				throw new ArgumentNullException("encryptionKeyHandle");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			return SignEncrypt(signingMechanism, signingKeyHandle, encryptionMechanism, encryptionKeyHandle, inputStream, outputStream, 4096);
		}

		public byte[] SignEncrypt(Mechanism signingMechanism, ObjectHandle signingKeyHandle, Mechanism encryptionMechanism, ObjectHandle encryptionKeyHandle, Stream inputStream, Stream outputStream, int bufferLength)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (signingMechanism == null)
			{
				throw new ArgumentNullException("signingMechanism");
			}
			if (signingKeyHandle == null)
			{
				throw new ArgumentNullException("signingKeyHandle");
			}
			if (encryptionMechanism == null)
			{
				throw new ArgumentNullException("encryptionMechanism");
			}
			if (encryptionKeyHandle == null)
			{
				throw new ArgumentNullException("encryptionKeyHandle");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (bufferLength < 1)
			{
				throw new ArgumentException("Value has to be positive number", "bufferLength");
			}
			CK_MECHANISM mechanism = signingMechanism.CkMechanism;
			CKR cKR = _p11.C_SignInit(_sessionId, ref mechanism, signingKeyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_SignInit", cKR);
			}
			CK_MECHANISM mechanism2 = encryptionMechanism.CkMechanism;
			cKR = _p11.C_EncryptInit(_sessionId, ref mechanism2, encryptionKeyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_EncryptInit", cKR);
			}
			byte[] array = new byte[bufferLength];
			byte[] array2 = new byte[bufferLength];
			ulong num = Convert.ToUInt64(array2.Length);
			int num2 = 0;
			while ((num2 = inputStream.Read(array, 0, array.Length)) > 0)
			{
				num = Convert.ToUInt64(array2.Length);
				cKR = _p11.C_SignEncryptUpdate(_sessionId, array, Convert.ToUInt64(num2), array2, ref num);
				if (cKR != 0)
				{
					throw new Pkcs11Exception("C_SignEncryptUpdate", cKR);
				}
				outputStream.Write(array2, 0, Convert.ToInt32(num));
			}
			byte[] array3 = null;
			ulong lastEncryptedPartLen = 0uL;
			cKR = _p11.C_EncryptFinal(_sessionId, null, ref lastEncryptedPartLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_EncryptFinal", cKR);
			}
			array3 = new byte[lastEncryptedPartLen];
			cKR = _p11.C_EncryptFinal(_sessionId, array3, ref lastEncryptedPartLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_EncryptFinal", cKR);
			}
			if (lastEncryptedPartLen != 0)
			{
				outputStream.Write(array3, 0, Convert.ToInt32(lastEncryptedPartLen));
			}
			ulong signatureLen = 0uL;
			cKR = _p11.C_SignFinal(_sessionId, null, ref signatureLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_SignFinal", cKR);
			}
			byte[] array4 = new byte[signatureLen];
			cKR = _p11.C_SignFinal(_sessionId, array4, ref signatureLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_SignFinal", cKR);
			}
			if (array4.Length != Convert.ToInt32(signatureLen))
			{
				Array.Resize(ref array4, Convert.ToInt32(signatureLen));
			}
			return array4;
		}

		public void DecryptVerify(Mechanism verificationMechanism, ObjectHandle verificationKeyHandle, Mechanism decryptionMechanism, ObjectHandle decryptionKeyHandle, byte[] data, byte[] signature, out byte[] decryptedData, out bool isValid)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (verificationMechanism == null)
			{
				throw new ArgumentNullException("verificationMechanism");
			}
			if (verificationKeyHandle == null)
			{
				throw new ArgumentNullException("verificationKeyHandle");
			}
			if (decryptionMechanism == null)
			{
				throw new ArgumentNullException("decryptionMechanism");
			}
			if (decryptionKeyHandle == null)
			{
				throw new ArgumentNullException("decryptionKeyHandle");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (signature == null)
			{
				throw new ArgumentNullException("signature");
			}
			using (MemoryStream inputStream = new MemoryStream(data))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					DecryptVerify(verificationMechanism, verificationKeyHandle, decryptionMechanism, decryptionKeyHandle, inputStream, memoryStream, signature, out isValid);
					decryptedData = memoryStream.ToArray();
				}
			}
		}

		public void DecryptVerify(Mechanism verificationMechanism, ObjectHandle verificationKeyHandle, Mechanism decryptionMechanism, ObjectHandle decryptionKeyHandle, Stream inputStream, Stream outputStream, byte[] signature, out bool isValid)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (verificationMechanism == null)
			{
				throw new ArgumentNullException("verificationMechanism");
			}
			if (verificationKeyHandle == null)
			{
				throw new ArgumentNullException("verificationKeyHandle");
			}
			if (decryptionMechanism == null)
			{
				throw new ArgumentNullException("decryptionMechanism");
			}
			if (decryptionKeyHandle == null)
			{
				throw new ArgumentNullException("decryptionKeyHandle");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (signature == null)
			{
				throw new ArgumentNullException("signature");
			}
			DecryptVerify(verificationMechanism, verificationKeyHandle, decryptionMechanism, decryptionKeyHandle, inputStream, outputStream, signature, out isValid, 4096);
		}

		public void DecryptVerify(Mechanism verificationMechanism, ObjectHandle verificationKeyHandle, Mechanism decryptionMechanism, ObjectHandle decryptionKeyHandle, Stream inputStream, Stream outputStream, byte[] signature, out bool isValid, int bufferLength)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (verificationMechanism == null)
			{
				throw new ArgumentNullException("verificationMechanism");
			}
			if (verificationKeyHandle == null)
			{
				throw new ArgumentNullException("verificationKeyHandle");
			}
			if (decryptionMechanism == null)
			{
				throw new ArgumentNullException("decryptionMechanism");
			}
			if (decryptionKeyHandle == null)
			{
				throw new ArgumentNullException("decryptionKeyHandle");
			}
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (signature == null)
			{
				throw new ArgumentNullException("signature");
			}
			if (bufferLength < 1)
			{
				throw new ArgumentException("Value has to be positive number", "bufferLength");
			}
			CK_MECHANISM mechanism = verificationMechanism.CkMechanism;
			CKR cKR = _p11.C_VerifyInit(_sessionId, ref mechanism, verificationKeyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_VerifyInit", cKR);
			}
			CK_MECHANISM mechanism2 = decryptionMechanism.CkMechanism;
			cKR = _p11.C_DecryptInit(_sessionId, ref mechanism2, decryptionKeyHandle.ObjectId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DecryptInit", cKR);
			}
			byte[] array = new byte[bufferLength];
			byte[] array2 = new byte[bufferLength];
			ulong num = Convert.ToUInt64(array2.Length);
			int num2 = 0;
			while ((num2 = inputStream.Read(array, 0, array.Length)) > 0)
			{
				num = Convert.ToUInt64(array2.Length);
				cKR = _p11.C_DecryptVerifyUpdate(_sessionId, array, Convert.ToUInt64(num2), array2, ref num);
				if (cKR != 0)
				{
					throw new Pkcs11Exception("C_DecryptVerifyUpdate", cKR);
				}
				outputStream.Write(array2, 0, Convert.ToInt32(num));
			}
			byte[] array3 = null;
			ulong lastPartLen = 0uL;
			cKR = _p11.C_DecryptFinal(_sessionId, null, ref lastPartLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DecryptFinal", cKR);
			}
			array3 = new byte[lastPartLen];
			cKR = _p11.C_DecryptFinal(_sessionId, array3, ref lastPartLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DecryptFinal", cKR);
			}
			if (lastPartLen != 0)
			{
				outputStream.Write(array3, 0, Convert.ToInt32(lastPartLen));
			}
			cKR = _p11.C_VerifyFinal(_sessionId, signature, Convert.ToUInt64(signature.Length));
			switch (cKR)
			{
			case CKR.CKR_OK:
				isValid = true;
				break;
			case CKR.CKR_SIGNATURE_INVALID:
				isValid = false;
				break;
			default:
				throw new Pkcs11Exception("C_VerifyFinal", cKR);
			}
		}

		public ObjectHandle GenerateKey(Mechanism mechanism, List<ObjectAttribute> attributes)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CK_ATTRIBUTE[] array = null;
			ulong num = 0uL;
			if (attributes != null)
			{
				num = Convert.ToUInt64(attributes.Count);
				array = new CK_ATTRIBUTE[num];
				for (int i = 0; i < Convert.ToInt32(num); i++)
				{
					array[i] = attributes[i].CkAttribute;
				}
			}
			ulong key = 0uL;
			CKR cKR = _p11.C_GenerateKey(_sessionId, ref mechanism2, array, num, ref key);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_GenerateKey", cKR);
			}
			return new ObjectHandle(key);
		}

		public void GenerateKeyPair(Mechanism mechanism, List<ObjectAttribute> publicKeyAttributes, List<ObjectAttribute> privateKeyAttributes, out ObjectHandle publicKeyHandle, out ObjectHandle privateKeyHandle)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CK_ATTRIBUTE[] array = null;
			ulong num = 0uL;
			if (publicKeyAttributes != null)
			{
				num = Convert.ToUInt64(publicKeyAttributes.Count);
				array = new CK_ATTRIBUTE[num];
				for (int i = 0; i < Convert.ToInt32(num); i++)
				{
					array[i] = publicKeyAttributes[i].CkAttribute;
				}
			}
			CK_ATTRIBUTE[] array2 = null;
			ulong num2 = 0uL;
			if (privateKeyAttributes != null)
			{
				num2 = Convert.ToUInt64(privateKeyAttributes.Count);
				array2 = new CK_ATTRIBUTE[num2];
				for (int j = 0; j < Convert.ToInt32(num2); j++)
				{
					array2[j] = privateKeyAttributes[j].CkAttribute;
				}
			}
			ulong publicKey = 0uL;
			ulong privateKey = 0uL;
			CKR cKR = _p11.C_GenerateKeyPair(_sessionId, ref mechanism2, array, num, array2, num2, ref publicKey, ref privateKey);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_GenerateKeyPair", cKR);
			}
			publicKeyHandle = new ObjectHandle(publicKey);
			privateKeyHandle = new ObjectHandle(privateKey);
		}

		public byte[] WrapKey(Mechanism mechanism, ObjectHandle wrappingKeyHandle, ObjectHandle keyHandle)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (wrappingKeyHandle == null)
			{
				throw new ArgumentNullException("wrappingKeyHandle");
			}
			if (keyHandle == null)
			{
				throw new ArgumentNullException("keyHandle");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			ulong wrappedKeyLen = 0uL;
			CKR cKR = _p11.C_WrapKey(_sessionId, ref mechanism2, wrappingKeyHandle.ObjectId, keyHandle.ObjectId, null, ref wrappedKeyLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_WrapKey", cKR);
			}
			byte[] array = new byte[wrappedKeyLen];
			cKR = _p11.C_WrapKey(_sessionId, ref mechanism2, wrappingKeyHandle.ObjectId, keyHandle.ObjectId, array, ref wrappedKeyLen);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_WrapKey", cKR);
			}
			if (array.Length != Convert.ToInt32(wrappedKeyLen))
			{
				Array.Resize(ref array, Convert.ToInt32(wrappedKeyLen));
			}
			return array;
		}

		public ObjectHandle UnwrapKey(Mechanism mechanism, ObjectHandle unwrappingKeyHandle, byte[] wrappedKey, List<ObjectAttribute> attributes)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (unwrappingKeyHandle == null)
			{
				throw new ArgumentNullException("unwrappingKeyHandle");
			}
			if (wrappedKey == null)
			{
				throw new ArgumentNullException("wrappedKey");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CK_ATTRIBUTE[] array = null;
			ulong attributeCount = 0uL;
			if (attributes != null)
			{
				array = new CK_ATTRIBUTE[attributes.Count];
				for (int i = 0; i < attributes.Count; i++)
				{
					array[i] = attributes[i].CkAttribute;
				}
				attributeCount = Convert.ToUInt64(attributes.Count);
			}
			ulong key = 0uL;
			CKR cKR = _p11.C_UnwrapKey(_sessionId, ref mechanism2, unwrappingKeyHandle.ObjectId, wrappedKey, Convert.ToUInt64(wrappedKey.Length), array, attributeCount, ref key);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_UnwrapKey", cKR);
			}
			return new ObjectHandle(key);
		}

		public ObjectHandle DeriveKey(Mechanism mechanism, ObjectHandle baseKeyHandle, List<ObjectAttribute> attributes)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			if (baseKeyHandle == null)
			{
				throw new ArgumentNullException("baseKeyHandle");
			}
			CK_MECHANISM mechanism2 = mechanism.CkMechanism;
			CK_ATTRIBUTE[] array = null;
			ulong attributeCount = 0uL;
			if (attributes != null)
			{
				array = new CK_ATTRIBUTE[attributes.Count];
				for (int i = 0; i < attributes.Count; i++)
				{
					array[i] = attributes[i].CkAttribute;
				}
				attributeCount = Convert.ToUInt64(attributes.Count);
			}
			ulong key = 0uL;
			CKR cKR = _p11.C_DeriveKey(_sessionId, ref mechanism2, baseKeyHandle.ObjectId, array, attributeCount, ref key);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_DeriveKey", cKR);
			}
			return new ObjectHandle(key);
		}

		public void SeedRandom(byte[] seed)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (seed == null)
			{
				throw new ArgumentNullException("seed");
			}
			CKR cKR = _p11.C_SeedRandom(_sessionId, seed, Convert.ToUInt64(seed.Length));
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_SeedRandom", cKR);
			}
		}

		public byte[] GenerateRandom(int length)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (length < 1)
			{
				throw new ArgumentException("Value has to be positive number", "length");
			}
			byte[] array = new byte[length];
			CKR cKR = _p11.C_GenerateRandom(_sessionId, array, Convert.ToUInt64(length));
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_GenerateRandom", cKR);
			}
			return array;
		}

		public void GetFunctionStatus()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			CKR cKR = _p11.C_GetFunctionStatus(_sessionId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_GetFunctionStatus", cKR);
			}
		}

		public void CancelFunction()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			CKR cKR = _p11.C_CancelFunction(_sessionId);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_CancelFunction", cKR);
			}
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
				if (disposing && _sessionId != 0L)
				{
					CloseSession();
				}
				_disposed = true;
			}
		}

		~Session()
		{
			Dispose(false);
		}
	}
}
