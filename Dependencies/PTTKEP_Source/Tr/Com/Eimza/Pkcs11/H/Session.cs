using System;
using System.Collections.Generic;
using System.IO;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H
{
    internal class Session : IDisposable
	{
		private bool _disposed;

		private Tr.Com.Eimza.Pkcs11.H4.Session _session4;

		private Tr.Com.Eimza.Pkcs11.H8.Session _session8;

		public ulong SessionId
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (UnmanagedLong.Size != 4)
				{
					return _session8.SessionId;
				}
				return _session4.SessionId;
			}
		}

		internal Session(Tr.Com.Eimza.Pkcs11.H4.Session session)
		{
			if (session == null)
			{
				throw new ArgumentNullException("session");
			}
			_session4 = session;
		}

		internal Session(Tr.Com.Eimza.Pkcs11.H8.Session session)
		{
			if (session == null)
			{
				throw new ArgumentNullException("session");
			}
			_session8 = session;
		}

		public void CloseSession()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				_session4.CloseSession();
			}
			else
			{
				_session8.CloseSession();
			}
		}

		public void InitPin(string userPin)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				_session4.InitPin(userPin);
			}
			else
			{
				_session8.InitPin(userPin);
			}
		}

		public void InitPin(byte[] userPin)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				_session4.InitPin(userPin);
			}
			else
			{
				_session8.InitPin(userPin);
			}
		}

		public void SetPin(string oldPin, string newPin)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				_session4.SetPin(oldPin, newPin);
			}
			else
			{
				_session8.SetPin(oldPin, newPin);
			}
		}

		public void SetPin(byte[] oldPin, byte[] newPin)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				_session4.SetPin(oldPin, newPin);
			}
			else
			{
				_session8.SetPin(oldPin, newPin);
			}
		}

		public SessionInfo GetSessionInfo()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				return new SessionInfo(_session4.GetSessionInfo());
			}
			return new SessionInfo(_session8.GetSessionInfo());
		}

		public byte[] GetOperationState()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				return _session4.GetOperationState();
			}
			return _session8.GetOperationState();
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
			if (UnmanagedLong.Size == 4)
			{
				_session4.SetOperationState(state, encryptionKey.ObjectHandle4, authenticationKey.ObjectHandle4);
			}
			else
			{
				_session8.SetOperationState(state, encryptionKey.ObjectHandle8, authenticationKey.ObjectHandle8);
			}
		}

		public void Login(CKU userType, string pin)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				_session4.Login(userType, pin);
			}
			else
			{
				_session8.Login(userType, pin);
			}
		}

		public void Login(CKU userType, byte[] pin)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				_session4.Login(userType, pin);
			}
			else
			{
				_session8.Login(userType, pin);
			}
		}

		public void Logout()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				_session4.Logout();
			}
			else
			{
				_session8.Logout();
			}
		}

		public ObjectHandle CreateObject(List<ObjectAttribute> attributes)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				List<Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute> attributes2 = ObjectAttribute.ConvertToH4List(attributes);
				return new ObjectHandle(_session4.CreateObject(attributes2));
			}
			List<Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute> attributes3 = ObjectAttribute.ConvertToH8List(attributes);
			return new ObjectHandle(_session8.CreateObject(attributes3));
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
			if (UnmanagedLong.Size == 4)
			{
				List<Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute> attributes2 = ObjectAttribute.ConvertToH4List(attributes);
				return new ObjectHandle(_session4.CopyObject(objectHandle.ObjectHandle4, attributes2));
			}
			List<Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute> attributes3 = ObjectAttribute.ConvertToH8List(attributes);
			return new ObjectHandle(_session8.CopyObject(objectHandle.ObjectHandle8, attributes3));
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
			if (UnmanagedLong.Size == 4)
			{
				_session4.DestroyObject(objectHandle.ObjectHandle4);
			}
			else
			{
				_session8.DestroyObject(objectHandle.ObjectHandle8);
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
			if (UnmanagedLong.Size == 4)
			{
				return _session4.GetObjectSize(objectHandle.ObjectHandle4);
			}
			return _session8.GetObjectSize(objectHandle.ObjectHandle8);
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
			if (UnmanagedLong.Size == 4)
			{
				List<uint> list = new List<uint>();
				for (int i = 0; i < attributes.Count; i++)
				{
					list.Add(Convert.ToUInt32(attributes[i]));
				}
				return ObjectAttribute.ConvertFromH4List(_session4.GetAttributeValue(objectHandle.ObjectHandle4, list));
			}
			return ObjectAttribute.ConvertFromH8List(_session8.GetAttributeValue(objectHandle.ObjectHandle8, attributes));
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
			if (UnmanagedLong.Size == 4)
			{
				List<Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute> attributes2 = ObjectAttribute.ConvertToH4List(attributes);
				_session4.SetAttributeValue(objectHandle.ObjectHandle4, attributes2);
			}
			else
			{
				List<Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute> attributes3 = ObjectAttribute.ConvertToH8List(attributes);
				_session8.SetAttributeValue(objectHandle.ObjectHandle8, attributes3);
			}
		}

		public void FindObjectsInit(List<ObjectAttribute> attributes)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				List<Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute> attributes2 = ObjectAttribute.ConvertToH4List(attributes);
				_session4.FindObjectsInit(attributes2);
			}
			else
			{
				List<Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute> attributes3 = ObjectAttribute.ConvertToH8List(attributes);
				_session8.FindObjectsInit(attributes3);
			}
		}

		public List<ObjectHandle> FindObjects(int objectCount)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				return ObjectHandle.ConvertFromH4List(_session4.FindObjects(objectCount));
			}
			return ObjectHandle.ConvertFromH8List(_session8.FindObjects(objectCount));
		}

		public void FindObjectsFinal()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				_session4.FindObjectsFinal();
			}
			else
			{
				_session8.FindObjectsFinal();
			}
		}

		public List<ObjectHandle> FindAllObjects(List<ObjectAttribute> attributes)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				List<Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute> attributes2 = ObjectAttribute.ConvertToH4List(attributes);
				return ObjectHandle.ConvertFromH4List(_session4.FindAllObjects(attributes2));
			}
			List<Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute> attributes3 = ObjectAttribute.ConvertToH8List(attributes);
			return ObjectHandle.ConvertFromH8List(_session8.FindAllObjects(attributes3));
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
			if (UnmanagedLong.Size == 4)
			{
				return _session4.Encrypt(mechanism.Mechanism4, keyHandle.ObjectHandle4, data);
			}
			return _session8.Encrypt(mechanism.Mechanism8, keyHandle.ObjectHandle8, data);
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
			if (UnmanagedLong.Size == 4)
			{
				_session4.Encrypt(mechanism.Mechanism4, keyHandle.ObjectHandle4, inputStream, outputStream, bufferLength);
			}
			else
			{
				_session8.Encrypt(mechanism.Mechanism8, keyHandle.ObjectHandle8, inputStream, outputStream, bufferLength);
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
			if (UnmanagedLong.Size == 4)
			{
				return _session4.Decrypt(mechanism.Mechanism4, keyHandle.ObjectHandle4, encryptedData);
			}
			return _session8.Decrypt(mechanism.Mechanism8, keyHandle.ObjectHandle8, encryptedData);
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
			if (UnmanagedLong.Size == 4)
			{
				_session4.Decrypt(mechanism.Mechanism4, keyHandle.ObjectHandle4, inputStream, outputStream, bufferLength);
			}
			else
			{
				_session8.Decrypt(mechanism.Mechanism8, keyHandle.ObjectHandle8, inputStream, outputStream, bufferLength);
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
			if (UnmanagedLong.Size == 4)
			{
				return _session4.DigestKey(mechanism.Mechanism4, keyHandle.ObjectHandle4);
			}
			return _session8.DigestKey(mechanism.Mechanism8, keyHandle.ObjectHandle8);
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
			if (UnmanagedLong.Size == 4)
			{
				return _session4.Digest(mechanism.Mechanism4, data);
			}
			return _session8.Digest(mechanism.Mechanism8, data);
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
			if (UnmanagedLong.Size == 4)
			{
				return _session4.Digest(mechanism.Mechanism4, inputStream, bufferLength);
			}
			return _session8.Digest(mechanism.Mechanism8, inputStream, bufferLength);
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
			if (UnmanagedLong.Size == 4)
			{
				return _session4.Sign(mechanism.Mechanism4, keyHandle.ObjectHandle4, data);
			}
			return _session8.Sign(mechanism.Mechanism8, keyHandle.ObjectHandle8, data);
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
			if (UnmanagedLong.Size == 4)
			{
				return _session4.Sign(mechanism.Mechanism4, keyHandle.ObjectHandle4, inputStream, bufferLength);
			}
			return _session8.Sign(mechanism.Mechanism8, keyHandle.ObjectHandle8, inputStream, bufferLength);
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
			if (UnmanagedLong.Size == 4)
			{
				return _session4.SignRecover(mechanism.Mechanism4, keyHandle.ObjectHandle4, data);
			}
			return _session8.SignRecover(mechanism.Mechanism8, keyHandle.ObjectHandle8, data);
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
			if (UnmanagedLong.Size == 4)
			{
				_session4.Verify(mechanism.Mechanism4, keyHandle.ObjectHandle4, data, signature, out isValid);
			}
			else
			{
				_session8.Verify(mechanism.Mechanism8, keyHandle.ObjectHandle8, data, signature, out isValid);
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
			if (UnmanagedLong.Size == 4)
			{
				_session4.Verify(mechanism.Mechanism4, keyHandle.ObjectHandle4, inputStream, signature, out isValid, bufferLength);
			}
			else
			{
				_session8.Verify(mechanism.Mechanism8, keyHandle.ObjectHandle8, inputStream, signature, out isValid, bufferLength);
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
			if (UnmanagedLong.Size == 4)
			{
				return _session4.VerifyRecover(mechanism.Mechanism4, keyHandle.ObjectHandle4, signature, out isValid);
			}
			return _session8.VerifyRecover(mechanism.Mechanism8, keyHandle.ObjectHandle8, signature, out isValid);
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
			if (UnmanagedLong.Size == 4)
			{
				return _session4.DigestEncrypt(digestingMechanism.Mechanism4, encryptionMechanism.Mechanism4, keyHandle.ObjectHandle4, inputStream, outputStream, bufferLength);
			}
			return _session8.DigestEncrypt(digestingMechanism.Mechanism8, encryptionMechanism.Mechanism8, keyHandle.ObjectHandle8, inputStream, outputStream, bufferLength);
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
			if (UnmanagedLong.Size == 4)
			{
				return _session4.DigestEncrypt(digestingMechanism.Mechanism4, decryptionMechanism.Mechanism4, keyHandle.ObjectHandle4, inputStream, outputStream, bufferLength);
			}
			return _session8.DigestEncrypt(digestingMechanism.Mechanism8, decryptionMechanism.Mechanism8, keyHandle.ObjectHandle8, inputStream, outputStream, bufferLength);
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
			if (UnmanagedLong.Size == 4)
			{
				return _session4.SignEncrypt(signingMechanism.Mechanism4, signingKeyHandle.ObjectHandle4, encryptionMechanism.Mechanism4, encryptionKeyHandle.ObjectHandle4, inputStream, outputStream, bufferLength);
			}
			return _session8.SignEncrypt(signingMechanism.Mechanism8, signingKeyHandle.ObjectHandle8, encryptionMechanism.Mechanism8, encryptionKeyHandle.ObjectHandle8, inputStream, outputStream, bufferLength);
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
			if (UnmanagedLong.Size == 4)
			{
				_session4.DecryptVerify(verificationMechanism.Mechanism4, verificationKeyHandle.ObjectHandle4, decryptionMechanism.Mechanism4, decryptionKeyHandle.ObjectHandle4, inputStream, outputStream, signature, out isValid, bufferLength);
			}
			else
			{
				_session8.DecryptVerify(verificationMechanism.Mechanism8, verificationKeyHandle.ObjectHandle8, decryptionMechanism.Mechanism8, decryptionKeyHandle.ObjectHandle8, inputStream, outputStream, signature, out isValid, bufferLength);
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
			if (UnmanagedLong.Size == 4)
			{
				List<Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute> attributes2 = ObjectAttribute.ConvertToH4List(attributes);
				return new ObjectHandle(_session4.GenerateKey(mechanism.Mechanism4, attributes2));
			}
			List<Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute> attributes3 = ObjectAttribute.ConvertToH8List(attributes);
			return new ObjectHandle(_session8.GenerateKey(mechanism.Mechanism8, attributes3));
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
			if (UnmanagedLong.Size == 4)
			{
				List<Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute> publicKeyAttributes2 = ObjectAttribute.ConvertToH4List(publicKeyAttributes);
				List<Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute> privateKeyAttributes2 = ObjectAttribute.ConvertToH4List(privateKeyAttributes);
				Tr.Com.Eimza.Pkcs11.H4.ObjectHandle publicKeyHandle2 = null;
				Tr.Com.Eimza.Pkcs11.H4.ObjectHandle privateKeyHandle2 = null;
				_session4.GenerateKeyPair(mechanism.Mechanism4, publicKeyAttributes2, privateKeyAttributes2, out publicKeyHandle2, out privateKeyHandle2);
				publicKeyHandle = new ObjectHandle(publicKeyHandle2);
				privateKeyHandle = new ObjectHandle(privateKeyHandle2);
			}
			else
			{
				List<Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute> publicKeyAttributes3 = ObjectAttribute.ConvertToH8List(publicKeyAttributes);
				List<Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute> privateKeyAttributes3 = ObjectAttribute.ConvertToH8List(privateKeyAttributes);
				Tr.Com.Eimza.Pkcs11.H8.ObjectHandle publicKeyHandle3 = null;
				Tr.Com.Eimza.Pkcs11.H8.ObjectHandle privateKeyHandle3 = null;
				_session8.GenerateKeyPair(mechanism.Mechanism8, publicKeyAttributes3, privateKeyAttributes3, out publicKeyHandle3, out privateKeyHandle3);
				publicKeyHandle = new ObjectHandle(publicKeyHandle3);
				privateKeyHandle = new ObjectHandle(privateKeyHandle3);
			}
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
			if (UnmanagedLong.Size == 4)
			{
				return _session4.WrapKey(mechanism.Mechanism4, wrappingKeyHandle.ObjectHandle4, keyHandle.ObjectHandle4);
			}
			return _session8.WrapKey(mechanism.Mechanism8, wrappingKeyHandle.ObjectHandle8, keyHandle.ObjectHandle8);
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
			if (UnmanagedLong.Size == 4)
			{
				List<Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute> attributes2 = ObjectAttribute.ConvertToH4List(attributes);
				return new ObjectHandle(_session4.UnwrapKey(mechanism.Mechanism4, unwrappingKeyHandle.ObjectHandle4, wrappedKey, attributes2));
			}
			List<Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute> attributes3 = ObjectAttribute.ConvertToH8List(attributes);
			return new ObjectHandle(_session8.UnwrapKey(mechanism.Mechanism8, unwrappingKeyHandle.ObjectHandle8, wrappedKey, attributes3));
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
			if (UnmanagedLong.Size == 4)
			{
				List<Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute> attributes2 = ObjectAttribute.ConvertToH4List(attributes);
				return new ObjectHandle(_session4.DeriveKey(mechanism.Mechanism4, baseKeyHandle.ObjectHandle4, attributes2));
			}
			List<Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute> attributes3 = ObjectAttribute.ConvertToH8List(attributes);
			return new ObjectHandle(_session8.DeriveKey(mechanism.Mechanism8, baseKeyHandle.ObjectHandle8, attributes3));
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
			if (UnmanagedLong.Size == 4)
			{
				_session4.SeedRandom(seed);
			}
			else
			{
				_session8.SeedRandom(seed);
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
			if (UnmanagedLong.Size == 4)
			{
				return _session4.GenerateRandom(length);
			}
			return _session8.GenerateRandom(length);
		}

		public void GetFunctionStatus()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				_session4.GetFunctionStatus();
			}
			else
			{
				_session8.GetFunctionStatus();
			}
		}

		public void CancelFunction()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				_session4.CancelFunction();
			}
			else
			{
				_session8.CancelFunction();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}
			if (disposing)
			{
				if (_session4 != null)
				{
					_session4.Dispose();
					_session4 = null;
				}
				if (_session8 != null)
				{
					_session8.Dispose();
					_session8 = null;
				}
			}
			_disposed = true;
		}

		~Session()
		{
			Dispose(false);
		}
	}
}
