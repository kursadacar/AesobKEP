using System;
using System.Collections.Generic;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkOtpParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_OTP_PARAMS _lowLevelStruct;

		public CkOtpParams(List<CkOtpParam> parameters)
		{
			_lowLevelStruct.Params = IntPtr.Zero;
			_lowLevelStruct.Count = 0uL;
			if (parameters != null && parameters.Count > 0)
			{
				int num = UnmanagedMemory.SizeOf(typeof(CK_OTP_PARAM));
				_lowLevelStruct.Params = UnmanagedMemory.Allocate(num * parameters.Count);
				_lowLevelStruct.Count = Convert.ToUInt64(parameters.Count);
				for (int i = 0; i < parameters.Count; i++)
				{
					UnmanagedMemory.Write(new IntPtr(_lowLevelStruct.Params.ToInt64() + i * num), parameters[i].ToMarshalableStructure());
				}
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
				UnmanagedMemory.Free(ref _lowLevelStruct.Params);
				_lowLevelStruct.Count = 0uL;
				_disposed = true;
			}
		}

		~CkOtpParams()
		{
			Dispose(false);
		}
	}
}
