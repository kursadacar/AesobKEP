using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkSsl3RandomData : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_SSL3_RANDOM_DATA _lowLevelStruct;

		public CkSsl3RandomData(byte[] clientRandom, byte[] serverRandom)
		{
			_lowLevelStruct.ClientRandom = IntPtr.Zero;
			_lowLevelStruct.ClientRandomLen = 0u;
			_lowLevelStruct.ServerRandom = IntPtr.Zero;
			_lowLevelStruct.ServerRandomLen = 0u;
			if (clientRandom != null)
			{
				_lowLevelStruct.ClientRandom = UnmanagedMemory.Allocate(clientRandom.Length);
				UnmanagedMemory.Write(_lowLevelStruct.ClientRandom, clientRandom);
				_lowLevelStruct.ClientRandomLen = Convert.ToUInt32(clientRandom.Length);
			}
			if (serverRandom != null)
			{
				_lowLevelStruct.ServerRandom = UnmanagedMemory.Allocate(serverRandom.Length);
				UnmanagedMemory.Write(_lowLevelStruct.ServerRandom, serverRandom);
				_lowLevelStruct.ServerRandomLen = Convert.ToUInt32(serverRandom.Length);
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
				UnmanagedMemory.Free(ref _lowLevelStruct.ClientRandom);
				_lowLevelStruct.ClientRandomLen = 0u;
				UnmanagedMemory.Free(ref _lowLevelStruct.ServerRandom);
				_lowLevelStruct.ServerRandomLen = 0u;
				_disposed = true;
			}
		}

		~CkSsl3RandomData()
		{
			Dispose(false);
		}
	}
}
