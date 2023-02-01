using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkKeaDeriveParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_KEA_DERIVE_PARAMS _lowLevelStruct;

		public CkKeaDeriveParams(bool isSender, byte[] randomA, byte[] randomB, byte[] publicData)
		{
			_lowLevelStruct.IsSender = false;
			_lowLevelStruct.RandomLen = 0uL;
			_lowLevelStruct.RandomA = IntPtr.Zero;
			_lowLevelStruct.RandomB = IntPtr.Zero;
			_lowLevelStruct.PublicDataLen = 0uL;
			_lowLevelStruct.PublicData = IntPtr.Zero;
			_lowLevelStruct.IsSender = isSender;
			if (randomA != null && randomB != null && randomA.Length != randomB.Length)
			{
				throw new ArgumentException("Length of randomA has to be the same as length of randomB");
			}
			if (randomA != null)
			{
				_lowLevelStruct.RandomA = UnmanagedMemory.Allocate(randomA.Length);
				UnmanagedMemory.Write(_lowLevelStruct.RandomA, randomA);
				_lowLevelStruct.RandomLen = Convert.ToUInt64(randomA.Length);
			}
			if (randomB != null)
			{
				_lowLevelStruct.RandomB = UnmanagedMemory.Allocate(randomB.Length);
				UnmanagedMemory.Write(_lowLevelStruct.RandomB, randomB);
				_lowLevelStruct.RandomLen = Convert.ToUInt64(randomB.Length);
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
				UnmanagedMemory.Free(ref _lowLevelStruct.RandomA);
				UnmanagedMemory.Free(ref _lowLevelStruct.RandomB);
				_lowLevelStruct.RandomLen = 0uL;
				UnmanagedMemory.Free(ref _lowLevelStruct.PublicData);
				_lowLevelStruct.PublicDataLen = 0uL;
				_disposed = true;
			}
		}

		~CkKeaDeriveParams()
		{
			Dispose(false);
		}
	}
}
