using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkKeyDerivationStringData : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_KEY_DERIVATION_STRING_DATA _lowLevelStruct;

		public CkKeyDerivationStringData(byte[] data)
		{
			_lowLevelStruct.Data = IntPtr.Zero;
			_lowLevelStruct.Len = 0u;
			if (data != null)
			{
				_lowLevelStruct.Data = UnmanagedMemory.Allocate(data.Length);
				UnmanagedMemory.Write(_lowLevelStruct.Data, data);
				_lowLevelStruct.Len = Convert.ToUInt32(data.Length);
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
				_lowLevelStruct.Len = 0u;
				_disposed = true;
			}
		}

		~CkKeyDerivationStringData()
		{
			Dispose(false);
		}
	}
}
