using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkSsl3MasterKeyDeriveParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkSsl3MasterKeyDeriveParams _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkSsl3MasterKeyDeriveParams _params8;

		private CkSsl3RandomData _randomInfo;

		public CkVersion Version
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (UnmanagedLong.Size == 4)
				{
					if (_params4.Version != null)
					{
						return new CkVersion(_params4.Version);
					}
					return null;
				}
				if (_params8.Version != null)
				{
					return new CkVersion(_params8.Version);
				}
				return null;
			}
		}

		public CkSsl3MasterKeyDeriveParams(CkSsl3RandomData randomInfo, bool dh)
		{
			if (randomInfo == null)
			{
				throw new ArgumentNullException("randomInfo");
			}
			_randomInfo = randomInfo;
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkSsl3MasterKeyDeriveParams(_randomInfo._params4, dh);
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkSsl3MasterKeyDeriveParams(_randomInfo._params8, dh);
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

		~CkSsl3MasterKeyDeriveParams()
		{
			Dispose(false);
		}
	}
}
