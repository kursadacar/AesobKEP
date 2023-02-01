using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkAriaCbcEncryptDataParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_ARIA_CBC_ENCRYPT_DATA_PARAMS _lowLevelStruct;

		public CkAriaCbcEncryptDataParams(byte[] iv, byte[] data)
		{
			_lowLevelStruct.Iv = new byte[16];
			_lowLevelStruct.Data = IntPtr.Zero;
			_lowLevelStruct.Length = 0u;
			if (iv == null)
			{
				throw new ArgumentNullException("iv");
			}
			if (iv.Length != 16)
			{
				throw new ArgumentOutOfRangeException("iv", "Array has to be 16 bytes long");
			}
			Array.Copy(iv, _lowLevelStruct.Iv, iv.Length);
			if (data != null)
			{
				_lowLevelStruct.Data = UnmanagedMemory.Allocate(data.Length);
				UnmanagedMemory.Write(_lowLevelStruct.Data, data);
				_lowLevelStruct.Length = Convert.ToUInt32(data.Length);
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
				UnmanagedMemory.Free(ref _lowLevelStruct.Data);
				_lowLevelStruct.Length = 0u;
				_disposed = true;
			}
		}

		~CkAriaCbcEncryptDataParams()
		{
			Dispose(false);
		}
	}
}
