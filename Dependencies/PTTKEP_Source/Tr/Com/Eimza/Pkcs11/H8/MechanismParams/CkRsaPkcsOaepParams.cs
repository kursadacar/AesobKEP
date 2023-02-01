using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkRsaPkcsOaepParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_RSA_PKCS_OAEP_PARAMS _lowLevelStruct;

		public CkRsaPkcsOaepParams(ulong hashAlg, ulong mgf, ulong source, byte[] sourceData)
		{
			_lowLevelStruct.HashAlg = 0uL;
			_lowLevelStruct.Mgf = 0uL;
			_lowLevelStruct.Source = 0uL;
			_lowLevelStruct.SourceData = IntPtr.Zero;
			_lowLevelStruct.SourceDataLen = 0uL;
			_lowLevelStruct.HashAlg = hashAlg;
			_lowLevelStruct.Mgf = mgf;
			_lowLevelStruct.Source = source;
			if (sourceData != null)
			{
				_lowLevelStruct.SourceData = UnmanagedMemory.Allocate(sourceData.Length);
				UnmanagedMemory.Write(_lowLevelStruct.SourceData, sourceData);
				_lowLevelStruct.SourceDataLen = Convert.ToUInt64(sourceData.Length);
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
				_lowLevelStruct.SourceDataLen = 0uL;
				_disposed = true;
			}
		}

		~CkRsaPkcsOaepParams()
		{
			Dispose(false);
		}
	}
}
