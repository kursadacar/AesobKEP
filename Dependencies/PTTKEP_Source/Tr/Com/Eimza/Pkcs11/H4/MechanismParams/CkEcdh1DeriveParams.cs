using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkEcdh1DeriveParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_ECDH1_DERIVE_PARAMS _lowLevelStruct;

		public CkEcdh1DeriveParams(uint kdf, byte[] sharedData, byte[] publicData)
		{
			_lowLevelStruct.Kdf = 0u;
			_lowLevelStruct.SharedDataLen = 0u;
			_lowLevelStruct.SharedData = IntPtr.Zero;
			_lowLevelStruct.PublicDataLen = 0u;
			_lowLevelStruct.PublicData = IntPtr.Zero;
			_lowLevelStruct.Kdf = kdf;
			if (sharedData != null)
			{
				_lowLevelStruct.SharedData = UnmanagedMemory.Allocate(sharedData.Length);
				UnmanagedMemory.Write(_lowLevelStruct.SharedData, sharedData);
				_lowLevelStruct.SharedDataLen = Convert.ToUInt32(sharedData.Length);
			}
			if (publicData != null)
			{
				_lowLevelStruct.PublicData = UnmanagedMemory.Allocate(publicData.Length);
				UnmanagedMemory.Write(_lowLevelStruct.PublicData, publicData);
				_lowLevelStruct.PublicDataLen = Convert.ToUInt32(publicData.Length);
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
				_lowLevelStruct.SharedDataLen = 0u;
				UnmanagedMemory.Free(ref _lowLevelStruct.PublicData);
				_lowLevelStruct.PublicDataLen = 0u;
				_disposed = true;
			}
		}

		~CkEcdh1DeriveParams()
		{
			Dispose(false);
		}
	}
}
