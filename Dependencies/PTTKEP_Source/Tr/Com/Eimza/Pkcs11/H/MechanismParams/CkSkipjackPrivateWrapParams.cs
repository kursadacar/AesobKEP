using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkSkipjackPrivateWrapParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkSkipjackPrivateWrapParams _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkSkipjackPrivateWrapParams _params8;

		public CkSkipjackPrivateWrapParams(byte[] password, byte[] publicData, byte[] randomA, byte[] primeP, byte[] baseG, byte[] subprimeQ)
		{
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkSkipjackPrivateWrapParams(password, publicData, randomA, primeP, baseG, subprimeQ);
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkSkipjackPrivateWrapParams(password, publicData, randomA, primeP, baseG, subprimeQ);
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

		~CkSkipjackPrivateWrapParams()
		{
			Dispose(false);
		}
	}
}
