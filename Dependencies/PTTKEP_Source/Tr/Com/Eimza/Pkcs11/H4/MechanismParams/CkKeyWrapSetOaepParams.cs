using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkKeyWrapSetOaepParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_KEY_WRAP_SET_OAEP_PARAMS _lowLevelStruct;

		public CkKeyWrapSetOaepParams(byte bc, byte[] x)
		{
			_lowLevelStruct.BC = 0;
			_lowLevelStruct.X = IntPtr.Zero;
			_lowLevelStruct.XLen = 0u;
			_lowLevelStruct.BC = bc;
			if (x != null)
			{
				_lowLevelStruct.X = UnmanagedMemory.Allocate(x.Length);
				UnmanagedMemory.Write(_lowLevelStruct.X, x);
				_lowLevelStruct.XLen = Convert.ToUInt32(x.Length);
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
				UnmanagedMemory.Free(ref _lowLevelStruct.X);
				_lowLevelStruct.XLen = 0u;
				_disposed = true;
			}
		}

		~CkKeyWrapSetOaepParams()
		{
			Dispose(false);
		}
	}
}
