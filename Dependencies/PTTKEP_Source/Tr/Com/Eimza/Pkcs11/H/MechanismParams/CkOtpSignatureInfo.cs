using System;
using System.Collections.Generic;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkOtpSignatureInfo : IDisposable
	{
		private bool _disposed;

		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkOtpSignatureInfo _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkOtpSignatureInfo _params8;

		private bool _paramsLeftInstance;

		private List<CkOtpParam> _params = new List<CkOtpParam>();

		public IList<CkOtpParam> Params
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				_paramsLeftInstance = true;
				return _params.AsReadOnly();
			}
		}

		public CkOtpSignatureInfo(byte[] signature)
		{
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkOtpSignatureInfo(signature);
				IList<Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkOtpParam> @params = _params4.Params;
				for (int i = 0; i < @params.Count; i++)
				{
					_params.Add(new CkOtpParam(@params[i]));
				}
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkOtpSignatureInfo(signature);
				IList<Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkOtpParam> params2 = _params8.Params;
				for (int j = 0; j < params2.Count; j++)
				{
					_params.Add(new CkOtpParam(params2[j]));
				}
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
				if (!_paramsLeftInstance)
				{
					for (int i = 0; i < _params.Count; i++)
					{
						if (_params[i] != null)
						{
							_params[i].Dispose();
							_params[i] = null;
						}
					}
				}
			}
			_disposed = true;
		}

		~CkOtpSignatureInfo()
		{
			Dispose(false);
		}
	}
}
