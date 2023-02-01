using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkPbeParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_PBE_PARAMS _lowLevelStruct;

		public CkPbeParams(byte[] initVector, byte[] password, byte[] salt, ulong iteration)
		{
			_lowLevelStruct.InitVector = IntPtr.Zero;
			_lowLevelStruct.Password = IntPtr.Zero;
			_lowLevelStruct.PasswordLen = 0uL;
			_lowLevelStruct.Salt = IntPtr.Zero;
			_lowLevelStruct.SaltLen = 0uL;
			_lowLevelStruct.Iteration = 0uL;
			if (initVector != null)
			{
				if (initVector.Length != 8)
				{
					throw new ArgumentOutOfRangeException("initVector", "Array has to be 8 bytes long");
				}
				_lowLevelStruct.InitVector = UnmanagedMemory.Allocate(initVector.Length);
				UnmanagedMemory.Write(_lowLevelStruct.InitVector, initVector);
			}
			if (password != null)
			{
				_lowLevelStruct.Password = UnmanagedMemory.Allocate(password.Length);
				UnmanagedMemory.Write(_lowLevelStruct.Password, password);
				_lowLevelStruct.PasswordLen = Convert.ToUInt64(password.Length);
			}
			if (salt != null)
			{
				_lowLevelStruct.Salt = UnmanagedMemory.Allocate(salt.Length);
				UnmanagedMemory.Write(_lowLevelStruct.Salt, salt);
				_lowLevelStruct.SaltLen = Convert.ToUInt64(salt.Length);
			}
			_lowLevelStruct.Iteration = iteration;
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
				UnmanagedMemory.Free(ref _lowLevelStruct.InitVector);
				UnmanagedMemory.Free(ref _lowLevelStruct.Password);
				_lowLevelStruct.PasswordLen = 0uL;
				UnmanagedMemory.Free(ref _lowLevelStruct.Salt);
				_lowLevelStruct.SaltLen = 0uL;
				_disposed = true;
			}
		}

		~CkPbeParams()
		{
			Dispose(false);
		}
	}
}
