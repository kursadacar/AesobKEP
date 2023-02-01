using System;
using System.Collections.Generic;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkOtpParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkOtpParams _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkOtpParams _params8;

		public CkOtpParams(List<CkOtpParam> parameters)
		{
			if (UnmanagedLong.Size == 4)
			{
				List<Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkOtpParam> list = new List<Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkOtpParam>();
				if (parameters != null && parameters.Count > 0)
				{
					for (int i = 0; i < parameters.Count; i++)
					{
						list.Add(parameters[i]._params4);
					}
				}
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkOtpParams(list);
				return;
			}
			List<Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkOtpParam> list2 = new List<Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkOtpParam>();
			if (parameters != null && parameters.Count > 0)
			{
				for (int j = 0; j < parameters.Count; j++)
				{
					list2.Add(parameters[j]._params8);
				}
			}
			_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkOtpParams(list2);
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

		~CkOtpParams()
		{
			Dispose(false);
		}
	}
}
