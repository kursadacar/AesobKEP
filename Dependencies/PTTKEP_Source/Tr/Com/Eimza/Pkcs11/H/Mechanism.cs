using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H
{
    internal class Mechanism : IDisposable
	{
		private bool _disposed;

		private Tr.Com.Eimza.Pkcs11.H4.Mechanism _mechanism4;

		private Tr.Com.Eimza.Pkcs11.H8.Mechanism _mechanism8;

		internal Tr.Com.Eimza.Pkcs11.H4.Mechanism Mechanism4
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return _mechanism4;
			}
		}

		internal Tr.Com.Eimza.Pkcs11.H8.Mechanism Mechanism8
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return _mechanism8;
			}
		}

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
					return _mechanism8.Type;
				}
				return _mechanism4.Type;
			}
		}

		internal Mechanism(Tr.Com.Eimza.Pkcs11.H4.Mechanism mechanism)
		{
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			_mechanism4 = mechanism;
		}

		internal Mechanism(Tr.Com.Eimza.Pkcs11.H8.Mechanism mechanism)
		{
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			_mechanism8 = mechanism;
		}

		public Mechanism(ulong type)
		{
			if (UnmanagedLong.Size == 4)
			{
				_mechanism4 = new Tr.Com.Eimza.Pkcs11.H4.Mechanism(Convert.ToUInt32(type));
			}
			else
			{
				_mechanism8 = new Tr.Com.Eimza.Pkcs11.H8.Mechanism(type);
			}
		}

		public Mechanism(CKM type)
		{
			if (UnmanagedLong.Size == 4)
			{
				_mechanism4 = new Tr.Com.Eimza.Pkcs11.H4.Mechanism(type);
			}
			else
			{
				_mechanism8 = new Tr.Com.Eimza.Pkcs11.H8.Mechanism(type);
			}
		}

		public Mechanism(ulong type, byte[] parameter)
		{
			if (UnmanagedLong.Size == 4)
			{
				_mechanism4 = new Tr.Com.Eimza.Pkcs11.H4.Mechanism(Convert.ToUInt32(type), parameter);
			}
			else
			{
				_mechanism8 = new Tr.Com.Eimza.Pkcs11.H8.Mechanism(type, parameter);
			}
		}

		public Mechanism(CKM type, byte[] parameter)
		{
			if (UnmanagedLong.Size == 4)
			{
				_mechanism4 = new Tr.Com.Eimza.Pkcs11.H4.Mechanism(type, parameter);
			}
			else
			{
				_mechanism8 = new Tr.Com.Eimza.Pkcs11.H8.Mechanism(type, parameter);
			}
		}

		public Mechanism(ulong type, IMechanismParams parameter)
		{
			if (parameter == null)
			{
				throw new ArgumentNullException("parameter");
			}
			if (UnmanagedLong.Size == 4)
			{
				_mechanism4 = new Tr.Com.Eimza.Pkcs11.H4.Mechanism(Convert.ToUInt32(type), parameter);
			}
			else
			{
				_mechanism8 = new Tr.Com.Eimza.Pkcs11.H8.Mechanism(type, parameter);
			}
		}

		public Mechanism(CKM type, IMechanismParams parameter)
		{
			if (parameter == null)
			{
				throw new ArgumentNullException("parameter");
			}
			if (UnmanagedLong.Size == 4)
			{
				_mechanism4 = new Tr.Com.Eimza.Pkcs11.H4.Mechanism(type, parameter);
			}
			else
			{
				_mechanism8 = new Tr.Com.Eimza.Pkcs11.H8.Mechanism(type, parameter);
			}
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
				if (_mechanism4 != null)
				{
					_mechanism4.Dispose();
					_mechanism4 = null;
				}
				if (_mechanism8 != null)
				{
					_mechanism8.Dispose();
					_mechanism8 = null;
				}
			}
			_disposed = true;
		}

		~Mechanism()
		{
			Dispose(false);
		}
	}
}
