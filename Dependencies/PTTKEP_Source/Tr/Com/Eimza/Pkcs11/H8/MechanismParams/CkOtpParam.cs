using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkOtpParam : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_OTP_PARAM _lowLevelStruct;

		public ulong Type
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return _lowLevelStruct.Type;
			}
		}

		public byte[] Value
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (!(_lowLevelStruct.Value == IntPtr.Zero))
				{
					return UnmanagedMemory.Read(_lowLevelStruct.Value, Convert.ToInt32(_lowLevelStruct.ValueLen));
				}
				return null;
			}
		}

		public CkOtpParam(ulong type, byte[] value)
		{
			_lowLevelStruct.Type = 0uL;
			_lowLevelStruct.Value = IntPtr.Zero;
			_lowLevelStruct.ValueLen = 0uL;
			_lowLevelStruct.Type = type;
			if (value != null)
			{
				_lowLevelStruct.Value = UnmanagedMemory.Allocate(value.Length);
				UnmanagedMemory.Write(_lowLevelStruct.Value, value);
				_lowLevelStruct.ValueLen = Convert.ToUInt64(value.Length);
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
				UnmanagedMemory.Free(ref _lowLevelStruct.Value);
				_lowLevelStruct.ValueLen = 0uL;
				_disposed = true;
			}
		}

		~CkOtpParam()
		{
			Dispose(false);
		}
	}
}
