using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkWtlsRandomData : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_WTLS_RANDOM_DATA _lowLevelStruct;

		public CkWtlsRandomData(byte[] clientRandom, byte[] serverRandom)
		{
			_lowLevelStruct.ClientRandom = IntPtr.Zero;
			_lowLevelStruct.ClientRandomLen = 0uL;
			_lowLevelStruct.ServerRandom = IntPtr.Zero;
			_lowLevelStruct.ServerRandomLen = 0uL;
			if (clientRandom != null)
			{
				_lowLevelStruct.ClientRandom = UnmanagedMemory.Allocate(clientRandom.Length);
				UnmanagedMemory.Write(_lowLevelStruct.ClientRandom, clientRandom);
				_lowLevelStruct.ClientRandomLen = Convert.ToUInt64(clientRandom.Length);
			}
			if (serverRandom != null)
			{
				_lowLevelStruct.ServerRandom = UnmanagedMemory.Allocate(serverRandom.Length);
				UnmanagedMemory.Write(_lowLevelStruct.ServerRandom, serverRandom);
				_lowLevelStruct.ServerRandomLen = Convert.ToUInt64(serverRandom.Length);
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
				_lowLevelStruct.ClientRandomLen = 0uL;
				UnmanagedMemory.Free(ref _lowLevelStruct.ServerRandom);
				_lowLevelStruct.ServerRandomLen = 0uL;
				_disposed = true;
			}
		}

		~CkWtlsRandomData()
		{
			Dispose(false);
		}
	}
}
