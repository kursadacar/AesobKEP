using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkWtlsKeyMatParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_WTLS_KEY_MAT_PARAMS _lowLevelStruct;

		private bool _returnedKeyMaterialLeftInstance;

		private CkWtlsKeyMatOut _returnedKeyMaterial;

		private CkWtlsRandomData _randomInfo;

		public CkWtlsKeyMatOut ReturnedKeyMaterial
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

		public CkWtlsKeyMatParams(uint digestMechanism, uint macSizeInBits, uint keySizeInBits, uint ivSizeInBits, uint sequenceNumber, bool isExport, CkWtlsRandomData randomInfo)
		{
			if (randomInfo == null)
			{
				throw new ArgumentNullException("randomInfo");
			}
			_randomInfo = randomInfo;
			if (ivSizeInBits % 8u != 0)
			{
				throw new ArgumentException("Value has to be a multiple of 8", "ivSizeInBits");
			}
			_returnedKeyMaterial = new CkWtlsKeyMatOut(ivSizeInBits / 8u);
			_lowLevelStruct.DigestMechanism = digestMechanism;
			_lowLevelStruct.MacSizeInBits = macSizeInBits;
			_lowLevelStruct.KeySizeInBits = keySizeInBits;
			_lowLevelStruct.IVSizeInBits = ivSizeInBits;
			_lowLevelStruct.SequenceNumber = sequenceNumber;
			_lowLevelStruct.IsExport = isExport;
			_lowLevelStruct.RandomInfo = (CK_WTLS_RANDOM_DATA)_randomInfo.ToMarshalableStructure();
			_lowLevelStruct.ReturnedKeyMaterial = UnmanagedMemory.Allocate(UnmanagedMemory.SizeOf(typeof(CK_WTLS_KEY_MAT_OUT)));
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

		~CkWtlsKeyMatParams()
		{
			Dispose(false);
		}
	}
}
