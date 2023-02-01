using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkWtlsKeyMatOut : IDisposable
	{
		private bool _disposed;

		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkWtlsKeyMatOut _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkWtlsKeyMatOut _params8;

		public ObjectHandle MacSecret
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (UnmanagedLong.Size != 4)
				{
					return new ObjectHandle(_params8.MacSecret);
				}
				return new ObjectHandle(_params4.MacSecret);
			}
		}

		public ObjectHandle Key
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (UnmanagedLong.Size != 4)
				{
					return new ObjectHandle(_params8.Key);
				}
				return new ObjectHandle(_params4.Key);
			}
		}

		public byte[] IV
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (UnmanagedLong.Size != 4)
				{
					return _params8.IV;
				}
				return _params4.IV;
			}
		}

		internal CkWtlsKeyMatOut(Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkWtlsKeyMatOut ckWtlsKeyMatOut)
		{
			if (ckWtlsKeyMatOut == null)
			{
				throw new ArgumentNullException("ckWtlsKeyMatOut");
			}
			_params4 = ckWtlsKeyMatOut;
		}

		internal CkWtlsKeyMatOut(Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkWtlsKeyMatOut ckWtlsKeyMatOut)
		{
			if (ckWtlsKeyMatOut == null)
			{
				throw new ArgumentNullException("ckWtlsKeyMatOut");
			}
			_params8 = ckWtlsKeyMatOut;
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

		~CkWtlsKeyMatOut()
		{
			Dispose(false);
		}
	}
}
