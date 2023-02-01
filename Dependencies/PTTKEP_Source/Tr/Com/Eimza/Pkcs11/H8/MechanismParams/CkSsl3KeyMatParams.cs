using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkSsl3KeyMatParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_SSL3_KEY_MAT_PARAMS _lowLevelStruct;

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
				UnmanagedMemory.Read(_lowLevelStruct.ReturnedKeyMaterial, _returnedKeyMaterial._lowLevelStruct);
				_returnedKeyMaterialLeftInstance = true;
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
			if (ivSizeInBits % 8uL != 0L)
			{
				throw new ArgumentException("Value has to be a multiple of 8", "ivSizeInBits");
			}
			_returnedKeyMaterial = new CkSsl3KeyMatOut(ivSizeInBits / 8uL);
			_lowLevelStruct.MacSizeInBits = macSizeInBits;
			_lowLevelStruct.KeySizeInBits = keySizeInBits;
			_lowLevelStruct.IVSizeInBits = ivSizeInBits;
			_lowLevelStruct.IsExport = isExport;
			_lowLevelStruct.RandomInfo = (CK_SSL3_RANDOM_DATA)_randomInfo.ToMarshalableStructure();
			_lowLevelStruct.ReturnedKeyMaterial = UnmanagedMemory.Allocate(UnmanagedMemory.SizeOf(typeof(CK_SSL3_KEY_MAT_OUT)));
			UnmanagedMemory.Write(_lowLevelStruct.ReturnedKeyMaterial, _returnedKeyMaterial._lowLevelStruct);
		}

		public object ToMarshalableStructure()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			return _lowLevelStruct;
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
				if (disposing && !_returnedKeyMaterialLeftInstance && _returnedKeyMaterial != null)
				{
					_returnedKeyMaterial.Dispose();
					_returnedKeyMaterial = null;
				}
				UnmanagedMemory.Free(ref _lowLevelStruct.ReturnedKeyMaterial);
				_disposed = true;
			}
		}

		~CkSsl3KeyMatParams()
		{
			Dispose(false);
		}
	}
}
