using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkSsl3KeyMatOut : IDisposable
	{
		private bool _disposed;

		internal CK_SSL3_KEY_MAT_OUT _lowLevelStruct;

		private uint _ivLength;

		public ObjectHandle ClientMacSecret
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return new ObjectHandle(_lowLevelStruct.ClientMacSecret);
			}
		}

		public ObjectHandle ServerMacSecret
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return new ObjectHandle(_lowLevelStruct.ServerMacSecret);
			}
		}

		public ObjectHandle ClientKey
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return new ObjectHandle(_lowLevelStruct.ClientKey);
			}
		}

		public ObjectHandle ServerKey
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return new ObjectHandle(_lowLevelStruct.ServerKey);
			}
		}

		public byte[] IVClient
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (_ivLength >= 1)
				{
					return UnmanagedMemory.Read(_lowLevelStruct.IVClient, Convert.ToInt32(_ivLength));
				}
				return null;
			}
		}

		public byte[] IVServer
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (_ivLength >= 1)
				{
					return UnmanagedMemory.Read(_lowLevelStruct.IVServer, Convert.ToInt32(_ivLength));
				}
				return null;
			}
		}

		internal CkSsl3KeyMatOut(uint ivLength)
		{
			_lowLevelStruct.ClientMacSecret = 0u;
			_lowLevelStruct.ServerMacSecret = 0u;
			_lowLevelStruct.ClientKey = 0u;
			_lowLevelStruct.ServerKey = 0u;
			_lowLevelStruct.IVClient = IntPtr.Zero;
			_lowLevelStruct.IVServer = IntPtr.Zero;
			_ivLength = ivLength;
			if (_ivLength != 0)
			{
				_lowLevelStruct.IVClient = UnmanagedMemory.Allocate(Convert.ToInt32(_ivLength));
				_lowLevelStruct.IVServer = UnmanagedMemory.Allocate(Convert.ToInt32(_ivLength));
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
				UnmanagedMemory.Free(ref _lowLevelStruct.IVClient);
				UnmanagedMemory.Free(ref _lowLevelStruct.IVServer);
				_disposed = true;
			}
		}

		~CkSsl3KeyMatOut()
		{
			Dispose(false);
		}
	}
}
