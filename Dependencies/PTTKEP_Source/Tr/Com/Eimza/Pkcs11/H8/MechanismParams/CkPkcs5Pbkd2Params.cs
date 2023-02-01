using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkPkcs5Pbkd2Params : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_PKCS5_PBKD2_PARAMS _lowLevelStruct;

		public CkPkcs5Pbkd2Params(ulong saltSource, byte[] saltSourceData, ulong iterations, ulong prf, byte[] prfData, byte[] password)
		{
			_lowLevelStruct.SaltSource = 0uL;
			_lowLevelStruct.SaltSourceData = IntPtr.Zero;
			_lowLevelStruct.SaltSourceDataLen = 0uL;
			_lowLevelStruct.Iterations = 0uL;
			_lowLevelStruct.Prf = 0uL;
			_lowLevelStruct.PrfData = IntPtr.Zero;
			_lowLevelStruct.PrfDataLen = 0uL;
			_lowLevelStruct.Password = IntPtr.Zero;
			_lowLevelStruct.PasswordLen = 0uL;
			_lowLevelStruct.SaltSource = saltSource;
			if (saltSourceData != null)
			{
				_lowLevelStruct.SaltSourceData = UnmanagedMemory.Allocate(saltSourceData.Length);
				UnmanagedMemory.Write(_lowLevelStruct.SaltSourceData, saltSourceData);
				_lowLevelStruct.SaltSourceDataLen = Convert.ToUInt64(saltSourceData.Length);
			}
			_lowLevelStruct.Iterations = iterations;
			_lowLevelStruct.Prf = prf;
			if (prfData != null)
			{
				_lowLevelStruct.PrfData = UnmanagedMemory.Allocate(prfData.Length);
				UnmanagedMemory.Write(_lowLevelStruct.PrfData, prfData);
				_lowLevelStruct.PrfDataLen = Convert.ToUInt64(prfData.Length);
			}
			if (password != null)
			{
				_lowLevelStruct.Password = UnmanagedMemory.Allocate(password.Length);
				UnmanagedMemory.Write(_lowLevelStruct.Password, password);
				_lowLevelStruct.PasswordLen = Convert.ToUInt64(password.Length);
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
				UnmanagedMemory.Free(ref _lowLevelStruct.SaltSourceData);
				_lowLevelStruct.SaltSourceDataLen = 0uL;
				UnmanagedMemory.Free(ref _lowLevelStruct.PrfData);
				_lowLevelStruct.PrfDataLen = 0uL;
				UnmanagedMemory.Free(ref _lowLevelStruct.Password);
				_lowLevelStruct.PasswordLen = 0uL;
				_disposed = true;
			}
		}

		~CkPkcs5Pbkd2Params()
		{
			Dispose(false);
		}
	}
}
