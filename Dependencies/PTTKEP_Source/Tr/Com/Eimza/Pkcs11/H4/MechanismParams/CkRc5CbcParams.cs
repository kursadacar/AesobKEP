using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkRc5CbcParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_RC5_CBC_PARAMS _lowLevelStruct;

		public CkRc5CbcParams(uint wordsize, uint rounds, byte[] iv)
		{
			_lowLevelStruct.Wordsize = 0u;
			_lowLevelStruct.Rounds = 0u;
			_lowLevelStruct.Iv = IntPtr.Zero;
			_lowLevelStruct.IvLen = 0u;
			_lowLevelStruct.Wordsize = wordsize;
			_lowLevelStruct.Rounds = rounds;
			if (iv != null)
			{
				_lowLevelStruct.Iv = UnmanagedMemory.Allocate(iv.Length);
				UnmanagedMemory.Write(_lowLevelStruct.Iv, iv);
				_lowLevelStruct.IvLen = Convert.ToUInt32(iv.Length);
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
				UnmanagedMemory.Free(ref _lowLevelStruct.Iv);
				_lowLevelStruct.IvLen = 0u;
				_disposed = true;
			}
		}

		~CkRc5CbcParams()
		{
			Dispose(false);
		}
	}
}
