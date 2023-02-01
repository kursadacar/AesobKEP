using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkPkcs5Pbkd2Params : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_PKCS5_PBKD2_PARAMS _lowLevelStruct;

		public CkPkcs5Pbkd2Params(uint saltSource, byte[] saltSourceData, uint iterations, uint prf, byte[] prfData, byte[] password)
		{
			_lowLevelStruct.SaltSource = 0u;
			_lowLevelStruct.SaltSourceData = IntPtr.Zero;
			_lowLevelStruct.SaltSourceDataLen = 0u;
			_lowLevelStruct.Iterations = 0u;
			_lowLevelStruct.Prf = 0u;
			_lowLevelStruct.PrfData = IntPtr.Zero;
			_lowLevelStruct.PrfDataLen = 0u;
			_lowLevelStruct.Password = IntPtr.Zero;
			_lowLevelStruct.PasswordLen = 0u;
			_lowLevelStruct.SaltSource = saltSource;
			if (saltSourceData != null)
			{
				_lowLevelStruct.SaltSourceData = UnmanagedMemory.Allocate(saltSourceData.Length);
				UnmanagedMemory.Write(_lowLevelStruct.SaltSourceData, saltSourceData);
				_lowLevelStruct.SaltSourceDataLen = Convert.ToUInt32(saltSourceData.Length);
			}
			_lowLevelStruct.Iterations = iterations;
			_lowLevelStruct.Prf = prf;
			if (prfData != null)
			{
				_lowLevelStruct.PrfData = UnmanagedMemory.Allocate(prfData.Length);
				UnmanagedMemory.Write(_lowLevelStruct.PrfData, prfData);
				_lowLevelStruct.PrfDataLen = Convert.ToUInt32(prfData.Length);
			}
			if (password != null)
			{
				_lowLevelStruct.Password = UnmanagedMemory.Allocate(password.Length);
				UnmanagedMemory.Write(_lowLevelStruct.Password, password);
				_lowLevelStruct.PasswordLen = Convert.ToUInt32(password.Length);
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
				_lowLevelStruct.SaltSourceDataLen = 0u;
				UnmanagedMemory.Free(ref _lowLevelStruct.PrfData);
				_lowLevelStruct.PrfDataLen = 0u;
				UnmanagedMemory.Free(ref _lowLevelStruct.Password);
				_lowLevelStruct.PasswordLen = 0u;
				_disposed = true;
			}
		}

		~CkPkcs5Pbkd2Params()
		{
			Dispose(false);
		}
	}
}
