using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkEcdh2DeriveParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkEcdh2DeriveParams _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkEcdh2DeriveParams _params8;

		public CkEcdh2DeriveParams(ulong kdf, byte[] sharedData, byte[] publicData, ulong privateDataLen, ObjectHandle privateData, byte[] publicData2)
		{
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkEcdh2DeriveParams(Convert.ToUInt32(kdf), sharedData, publicData, Convert.ToUInt32(privateDataLen), privateData.ObjectHandle4, publicData2);
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkEcdh2DeriveParams(kdf, sharedData, publicData, privateDataLen, privateData.ObjectHandle8, publicData2);
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

		~CkEcdh2DeriveParams()
		{
			Dispose(false);
		}
	}
}
