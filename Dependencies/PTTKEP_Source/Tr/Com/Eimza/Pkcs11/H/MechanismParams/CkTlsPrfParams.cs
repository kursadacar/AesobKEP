using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkTlsPrfParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkTlsPrfParams _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkTlsPrfParams _params8;

		public byte[] Output
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (UnmanagedLong.Size != 4)
				{
					return _params8.Output;
				}
				return _params4.Output;
			}
		}

		public CkTlsPrfParams(byte[] seed, byte[] label, ulong outputLen)
		{
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkTlsPrfParams(seed, label, Convert.ToUInt32(outputLen));
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkTlsPrfParams(seed, label, outputLen);
			}
		}

		public object ToMarshalableStructure()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				return _params4.ToMarshalableStructure();
			}
			return _params8.ToMarshalableStructure();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}
			if (disposing)
			{
				if (_params4 != null)
				{
					_params4.Dispose();
					_params4 = null;
				}
				if (_params8 != null)
				{
					_params8.Dispose();
					_params8 = null;
				}
			}
			_disposed = true;
		}

		~CkTlsPrfParams()
		{
			Dispose(false);
		}
	}
}
