using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4;

namespace Tr.Com.Eimza.Pkcs11.H4
{
	internal class Mechanism : IDisposable
	{
		private bool _disposed;

		private CK_MECHANISM _ckMechanism;

		private IMechanismParams _mechanismParams;

		internal CK_MECHANISM CkMechanism
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return _ckMechanism;
			}
		}

		public uint Type
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return _ckMechanism.Mechanism;
			}
		}

		public Mechanism(uint type)
		{
			_ckMechanism = CkmUtils.CreateMechanism(type);
		}

		public Mechanism(CKM type)
		{
			_ckMechanism = CkmUtils.CreateMechanism(type);
		}

		public Mechanism(uint type, byte[] parameter)
		{
			_ckMechanism = CkmUtils.CreateMechanism(type, parameter);
		}

		public Mechanism(CKM type, byte[] parameter)
		{
			_ckMechanism = CkmUtils.CreateMechanism(type, parameter);
		}

		public Mechanism(uint type, IMechanismParams parameter)
		{
			if (parameter == null)
			{
				throw new ArgumentNullException("parameter");
			}
			_mechanismParams = parameter;
			object parameterStructure = _mechanismParams.ToMarshalableStructure();
			_ckMechanism = CkmUtils.CreateMechanism(type, parameterStructure);
		}

		public Mechanism(CKM type, IMechanismParams parameter)
		{
			if (parameter == null)
			{
				throw new ArgumentNullException("parameter");
			}
			_mechanismParams = parameter;
			object parameterStructure = _mechanismParams.ToMarshalableStructure();
			_ckMechanism = CkmUtils.CreateMechanism(type, parameterStructure);
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
				UnmanagedMemory.Free(ref _ckMechanism.Parameter);
				_ckMechanism.ParameterLen = 0u;
				_disposed = true;
			}
		}

		~Mechanism()
		{
			Dispose(false);
		}
	}
}
