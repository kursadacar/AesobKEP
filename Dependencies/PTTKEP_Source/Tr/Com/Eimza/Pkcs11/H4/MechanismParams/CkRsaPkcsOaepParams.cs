using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkRsaPkcsOaepParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_RSA_PKCS_OAEP_PARAMS _lowLevelStruct;

		public CkRsaPkcsOaepParams(uint hashAlg, uint mgf, uint source, byte[] sourceData)
		{
			_lowLevelStruct.HashAlg = 0u;
			_lowLevelStruct.Mgf = 0u;
			_lowLevelStruct.Source = 0u;
			_lowLevelStruct.SourceData = IntPtr.Zero;
			_lowLevelStruct.SourceDataLen = 0u;
			_lowLevelStruct.HashAlg = hashAlg;
			_lowLevelStruct.Mgf = mgf;
			_lowLevelStruct.Source = source;
			if (sourceData != null)
			{
				_lowLevelStruct.SourceData = UnmanagedMemory.Allocate(sourceData.Length);
				UnmanagedMemory.Write(_lowLevelStruct.SourceData, sourceData);
				_lowLevelStruct.SourceDataLen = Convert.ToUInt32(sourceData.Length);
			}
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
				UnmanagedMemory.Free(ref _lowLevelStruct.SourceData);
				_lowLevelStruct.SourceDataLen = 0u;
				_disposed = true;
			}
		}

		~CkRsaPkcsOaepParams()
		{
			Dispose(false);
		}
	}
}
