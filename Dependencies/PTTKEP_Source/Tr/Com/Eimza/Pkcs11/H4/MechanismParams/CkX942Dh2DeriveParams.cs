using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkX942Dh2DeriveParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_X9_42_DH2_DERIVE_PARAMS _lowLevelStruct;

		public CkX942Dh2DeriveParams(uint kdf, byte[] otherInfo, byte[] publicData, uint privateDataLen, ObjectHandle privateData, byte[] publicData2)
		{
			_lowLevelStruct.Kdf = 0u;
			_lowLevelStruct.OtherInfoLen = 0u;
			_lowLevelStruct.OtherInfo = IntPtr.Zero;
			_lowLevelStruct.PublicDataLen = 0u;
			_lowLevelStruct.PublicData = IntPtr.Zero;
			_lowLevelStruct.PrivateDataLen = 0u;
			_lowLevelStruct.PrivateData = 0u;
			_lowLevelStruct.PublicDataLen2 = 0u;
			_lowLevelStruct.PublicData2 = IntPtr.Zero;
			_lowLevelStruct.Kdf = kdf;
			if (otherInfo != null)
			{
				_lowLevelStruct.OtherInfo = UnmanagedMemory.Allocate(otherInfo.Length);
				UnmanagedMemory.Write(_lowLevelStruct.OtherInfo, otherInfo);
				_lowLevelStruct.OtherInfoLen = Convert.ToUInt32(otherInfo.Length);
			}
			if (publicData != null)
			{
				_lowLevelStruct.PublicData = UnmanagedMemory.Allocate(publicData.Length);
				UnmanagedMemory.Write(_lowLevelStruct.PublicData, publicData);
				_lowLevelStruct.PublicDataLen = Convert.ToUInt32(publicData.Length);
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
				_lowLevelStruct.PublicDataLen2 = Convert.ToUInt32(publicData2.Length);
			}
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
				_lowLevelStruct.OtherInfoLen = 0u;
				UnmanagedMemory.Free(ref _lowLevelStruct.PublicData);
				_lowLevelStruct.PublicDataLen = 0u;
				UnmanagedMemory.Free(ref _lowLevelStruct.PublicData2);
				_lowLevelStruct.PublicDataLen2 = 0u;
				_disposed = true;
			}
		}

		~CkX942Dh2DeriveParams()
		{
			Dispose(false);
		}
	}
}
