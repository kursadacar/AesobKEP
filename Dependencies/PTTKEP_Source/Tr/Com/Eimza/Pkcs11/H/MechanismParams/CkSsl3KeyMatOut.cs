using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkSsl3KeyMatOut : IDisposable
	{
		private bool _disposed;

		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkSsl3KeyMatOut _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkSsl3KeyMatOut _params8;

		public ObjectHandle ClientMacSecret
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (UnmanagedLong.Size != 4)
				{
					return new ObjectHandle(_params8.ClientMacSecret);
				}
				return new ObjectHandle(_params4.ClientMacSecret);
			}
		}

		public ObjectHandle ServerMacSecret
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (UnmanagedLong.Size != 4)
				{
					return new ObjectHandle(_params8.ServerMacSecret);
				}
				return new ObjectHandle(_params4.ServerMacSecret);
			}
		}

		public ObjectHandle ClientKey
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (UnmanagedLong.Size != 4)
				{
					return new ObjectHandle(_params8.ClientKey);
				}
				return new ObjectHandle(_params4.ClientKey);
			}
		}

		public ObjectHandle ServerKey
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (UnmanagedLong.Size != 4)
				{
					return new ObjectHandle(_params8.ServerKey);
				}
				return new ObjectHandle(_params4.ServerKey);
			}
		}

		public byte[] IVClient
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (UnmanagedLong.Size != 4)
				{
					return _params8.IVClient;
				}
				return _params4.IVClient;
			}
		}

		public byte[] IVServer
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (UnmanagedLong.Size != 4)
				{
					return _params8.IVServer;
				}
				return _params4.IVServer;
			}
		}

		internal CkSsl3KeyMatOut(Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkSsl3KeyMatOut ckSsl3KeyMatOut)
		{
			if (ckSsl3KeyMatOut == null)
			{
				throw new ArgumentNullException("ckSsl3KeyMatOut");
			}
			_params4 = ckSsl3KeyMatOut;
		}

		internal CkSsl3KeyMatOut(Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkSsl3KeyMatOut ckSsl3KeyMatOut)
		{
			if (ckSsl3KeyMatOut == null)
			{
				throw new ArgumentNullException("ckSsl3KeyMatOut");
			}
			_params8 = ckSsl3KeyMatOut;
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

		~CkSsl3KeyMatOut()
		{
			Dispose(false);
		}
	}
}
