using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkOtpParam : IMechanismParams, IDisposable
	{
		private bool _disposed;

		internal Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkOtpParam _params4;

		internal Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkOtpParam _params8;

		public ulong Type
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (UnmanagedLong.Size != 4)
				{
					return _params8.Type;
				}
				return _params4.Type;
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
				if (UnmanagedLong.Size != 4)
				{
					return _params8.Value;
				}
				return _params4.Value;
			}
		}

		public CkOtpParam(ulong type, byte[] value)
		{
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkOtpParam(Convert.ToUInt32(type), value);
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkOtpParam(type, value);
			}
		}

		internal CkOtpParam(Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkOtpParam ckOtpParam)
		{
			if (ckOtpParam == null)
			{
				throw new ArgumentNullException("ckOtpParam");
			}
			_params4 = ckOtpParam;
		}

		internal CkOtpParam(Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkOtpParam ckOtpParam)
		{
			if (ckOtpParam == null)
			{
				throw new ArgumentNullException("ckOtpParam");
			}
			_params8 = ckOtpParam;
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

		~CkOtpParam()
		{
			Dispose(false);
		}
	}
}
