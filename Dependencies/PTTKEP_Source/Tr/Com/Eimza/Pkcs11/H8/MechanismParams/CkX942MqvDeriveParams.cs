using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkX942MqvDeriveParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_X9_42_MQV_DERIVE_PARAMS _lowLevelStruct;

		public CkX942MqvDeriveParams(ulong kdf, byte[] otherInfo, byte[] publicData, ulong privateDataLen, ObjectHandle privateData, byte[] publicData2, ObjectHandle publicKey)
		{
			_lowLevelStruct.Kdf = 0uL;
			_lowLevelStruct.OtherInfoLen = 0uL;
			_lowLevelStruct.OtherInfo = IntPtr.Zero;
			_lowLevelStruct.PublicDataLen = 0uL;
			_lowLevelStruct.PublicData = IntPtr.Zero;
			_lowLevelStruct.PrivateDataLen = 0uL;
			_lowLevelStruct.PrivateData = 0uL;
			_lowLevelStruct.PublicDataLen2 = 0uL;
			_lowLevelStruct.PublicData2 = IntPtr.Zero;
			_lowLevelStruct.PublicKey = 0uL;
			_lowLevelStruct.Kdf = kdf;
			if (otherInfo != null)
			{
				_lowLevelStruct.OtherInfo = UnmanagedMemory.Allocate(otherInfo.Length);
				UnmanagedMemory.Write(_lowLevelStruct.OtherInfo, otherInfo);
				_lowLevelStruct.OtherInfoLen = Convert.ToUInt64(otherInfo.Length);
			}
			if (publicData != null)
			{
				_lowLevelStruct.PublicData = UnmanagedMemory.Allocate(publicData.Length);
				UnmanagedMemory.Write(_lowLevelStruct.PublicData, publicData);
				_lowLevelStruct.PublicDataLen = Convert.ToUInt64(publicData.Length);
			}
			_lowLevelStruct.PrivateDataLen = privateDataLen;
			if (privateData == null)
			{
				throw new ArgumentNullException("privateData");
			}
			_lowLevelStruct.PrivateData = privateData.ObjectId;
			if (publicData2 != null)
			{
				_lowLevelStruct.PublicData2 = UnmanagedMemory.Allocate(publicData2.Length);
				UnmanagedMemory.Write(_lowLevelStruct.PublicData2, publicData2);
				_lowLevelStruct.PublicDataLen2 = Convert.ToUInt64(publicData2.Length);
			}
			if (publicKey == null)
			{
				throw new ArgumentNullException("publicKey");
			}
			_lowLevelStruct.PublicKey = publicKey.ObjectId;
		}

		public object ToMarshalableStructure()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return _lowLevelStruct;
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
				UnmanagedMemory.Free(ref _lowLevelStruct.OtherInfo);
				_lowLevelStruct.OtherInfoLen = 0uL;
				UnmanagedMemory.Free(ref _lowLevelStruct.PublicData);
				_lowLevelStruct.PublicDataLen = 0uL;
				UnmanagedMemory.Free(ref _lowLevelStruct.PublicData2);
				_lowLevelStruct.PublicDataLen2 = 0uL;
				_disposed = true;
			}
		}

		~CkX942MqvDeriveParams()
		{
			Dispose(false);
		}
	}
}
