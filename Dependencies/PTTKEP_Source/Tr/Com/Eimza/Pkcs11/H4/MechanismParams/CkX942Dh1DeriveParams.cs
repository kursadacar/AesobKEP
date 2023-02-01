using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkX942Dh1DeriveParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_X9_42_DH1_DERIVE_PARAMS _lowLevelStruct;

		public CkX942Dh1DeriveParams(uint kdf, byte[] otherInfo, byte[] publicData)
		{
			_lowLevelStruct.Kdf = 0u;
			_lowLevelStruct.OtherInfoLen = 0u;
			_lowLevelStruct.OtherInfo = IntPtr.Zero;
			_lowLevelStruct.PublicDataLen = 0u;
			_lowLevelStruct.PublicData = IntPtr.Zero;
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
				_disposed = true;
			}
		}

		~CkX942Dh1DeriveParams()
		{
			Dispose(false);
		}
	}
}
