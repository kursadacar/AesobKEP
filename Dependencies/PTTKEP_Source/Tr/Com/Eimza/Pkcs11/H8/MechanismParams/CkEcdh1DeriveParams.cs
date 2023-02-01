using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkEcdh1DeriveParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_ECDH1_DERIVE_PARAMS _lowLevelStruct;

		public CkEcdh1DeriveParams(ulong kdf, byte[] sharedData, byte[] publicData)
		{
			_lowLevelStruct.Kdf = 0uL;
			_lowLevelStruct.SharedDataLen = 0uL;
			_lowLevelStruct.SharedData = IntPtr.Zero;
			_lowLevelStruct.PublicDataLen = 0uL;
			_lowLevelStruct.PublicData = IntPtr.Zero;
			_lowLevelStruct.Kdf = kdf;
			if (sharedData != null)
			{
				_lowLevelStruct.SharedData = UnmanagedMemory.Allocate(sharedData.Length);
				UnmanagedMemory.Write(_lowLevelStruct.SharedData, sharedData);
				_lowLevelStruct.SharedDataLen = Convert.ToUInt64(sharedData.Length);
			}
			if (publicData != null)
			{
				_lowLevelStruct.PublicData = UnmanagedMemory.Allocate(publicData.Length);
				UnmanagedMemory.Write(_lowLevelStruct.PublicData, publicData);
				_lowLevelStruct.PublicDataLen = Convert.ToUInt64(publicData.Length);
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
				UnmanagedMemory.Free(ref _lowLevelStruct.SharedData);
				_lowLevelStruct.SharedDataLen = 0uL;
				UnmanagedMemory.Free(ref _lowLevelStruct.PublicData);
				_lowLevelStruct.PublicDataLen = 0uL;
				_disposed = true;
			}
		}

		~CkEcdh1DeriveParams()
		{
			Dispose(false);
		}
	}
}
