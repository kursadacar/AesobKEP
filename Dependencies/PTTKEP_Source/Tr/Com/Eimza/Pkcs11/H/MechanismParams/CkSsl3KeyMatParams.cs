using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkSsl3KeyMatParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkSsl3KeyMatParams _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkSsl3KeyMatParams _params8;

		private bool _returnedKeyMaterialLeftInstance;

		private CkSsl3KeyMatOut _returnedKeyMaterial;

		private CkSsl3RandomData _randomInfo;

		public CkSsl3KeyMatOut ReturnedKeyMaterial
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (_returnedKeyMaterial == null)
				{
					if (UnmanagedLong.Size == 4)
					{
						_returnedKeyMaterial = new CkSsl3KeyMatOut(_params4.ReturnedKeyMaterial);
					}
					else
					{
						_returnedKeyMaterial = new CkSsl3KeyMatOut(_params8.ReturnedKeyMaterial);
					}
					_returnedKeyMaterialLeftInstance = true;
				}
				return _returnedKeyMaterial;
			}
		}

		public CkSsl3KeyMatParams(ulong macSizeInBits, ulong keySizeInBits, ulong ivSizeInBits, bool isExport, CkSsl3RandomData randomInfo)
		{
			if (randomInfo == null)
			{
				throw new ArgumentNullException("randomInfo");
			}
			_randomInfo = randomInfo;
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkSsl3KeyMatParams(Convert.ToUInt32(macSizeInBits), Convert.ToUInt32(keySizeInBits), Convert.ToUInt32(ivSizeInBits), isExport, _randomInfo._params4);
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkSsl3KeyMatParams(macSizeInBits, keySizeInBits, ivSizeInBits, isExport, _randomInfo._params8);
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
				if (!_returnedKeyMaterialLeftInstance && _returnedKeyMaterial != null)
				{
					_returnedKeyMaterial.Dispose();
					_returnedKeyMaterial = null;
				}
			}
			_disposed = true;
		}

		~CkSsl3KeyMatParams()
		{
			Dispose(false);
		}
	}
}
