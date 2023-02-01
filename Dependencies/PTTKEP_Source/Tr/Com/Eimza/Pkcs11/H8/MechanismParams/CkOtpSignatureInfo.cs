using System;
using System.Collections.Generic;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkOtpSignatureInfo : IDisposable
	{
		private bool _disposed;

		private CK_OTP_SIGNATURE_INFO _lowLevelStruct;

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
			if (signature == null)
			{
				throw new ArgumentNullException("signature");
			}
			IntPtr memory = IntPtr.Zero;
			try
			{
				memory = UnmanagedMemory.Allocate(signature.Length);
				UnmanagedMemory.Write(memory, signature);
				UnmanagedMemory.Read(memory, _lowLevelStruct);
			}
			finally
			{
				UnmanagedMemory.Free(ref memory);
			}
			int num = UnmanagedMemory.SizeOf(typeof(CK_OTP_PARAM));
			for (int i = 0; i < Convert.ToInt32(_lowLevelStruct.Count); i++)
			{
				IntPtr memory2 = new IntPtr(_lowLevelStruct.Params.ToInt64() + i * num);
				CK_OTP_PARAM cK_OTP_PARAM = default(CK_OTP_PARAM);
				UnmanagedMemory.Read(memory2, cK_OTP_PARAM);
				ulong type = cK_OTP_PARAM.Type;
				byte[] value = UnmanagedMemory.Read(cK_OTP_PARAM.Value, Convert.ToInt32(cK_OTP_PARAM.ValueLen));
				_params.Add(new CkOtpParam(type, value));
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
			if (disposing && !_paramsLeftInstance)
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
			_disposed = true;
		}

		~CkOtpSignatureInfo()
		{
			Dispose(false);
		}
	}
}
