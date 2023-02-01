using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkCamelliaCbcEncryptDataParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_CAMELLIA_CBC_ENCRYPT_DATA_PARAMS _lowLevelStruct;

		public CkCamelliaCbcEncryptDataParams(byte[] iv, byte[] data)
		{
			_lowLevelStruct.Iv = new byte[16];
			_lowLevelStruct.Data = IntPtr.Zero;
			_lowLevelStruct.Length = 0uL;
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
				_lowLevelStruct.Length = Convert.ToUInt64(data.Length);
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
				_lowLevelStruct.Length = 0uL;
				_disposed = true;
			}
		}

		~CkCamelliaCbcEncryptDataParams()
		{
			Dispose(false);
		}
	}
}
