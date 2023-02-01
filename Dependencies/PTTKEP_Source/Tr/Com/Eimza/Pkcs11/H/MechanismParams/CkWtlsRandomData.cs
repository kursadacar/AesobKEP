using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkWtlsRandomData : IMechanismParams, IDisposable
	{
		private bool _disposed;

		internal Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkWtlsRandomData _params4;

		internal Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkWtlsRandomData _params8;

		public CkWtlsRandomData(byte[] clientRandom, byte[] serverRandom)
		{
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkWtlsRandomData(clientRandom, serverRandom);
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkWtlsRandomData(clientRandom, serverRandom);
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

		~CkWtlsRandomData()
		{
			Dispose(false);
		}
	}
}
