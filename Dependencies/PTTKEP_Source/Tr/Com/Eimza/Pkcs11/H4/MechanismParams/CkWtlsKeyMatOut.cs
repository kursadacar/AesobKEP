using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkWtlsKeyMatOut : IDisposable
	{
		private bool _disposed;

		internal CK_WTLS_KEY_MAT_OUT _lowLevelStruct;

		private uint _ivLength;

		public ObjectHandle MacSecret
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return new ObjectHandle(_lowLevelStruct.MacSecret);
			}
		}

		public ObjectHandle Key
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return new ObjectHandle(_lowLevelStruct.Key);
			}
		}

		public byte[] IV
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (_ivLength >= 1)
				{
					return UnmanagedMemory.Read(_lowLevelStruct.IV, Convert.ToInt32(_ivLength));
				}
				return null;
			}
		}

		internal CkWtlsKeyMatOut(uint ivLength)
		{
			_lowLevelStruct.MacSecret = 0u;
			_lowLevelStruct.Key = 0u;
			_lowLevelStruct.IV = IntPtr.Zero;
			_ivLength = ivLength;
			if (_ivLength != 0)
			{
				_lowLevelStruct.IV = UnmanagedMemory.Allocate(Convert.ToInt32(_ivLength));
			}
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
				UnmanagedMemory.Free(ref _lowLevelStruct.IV);
				_disposed = true;
			}
		}

		~CkWtlsKeyMatOut()
		{
			Dispose(false);
		}
	}
}
