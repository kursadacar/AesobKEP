using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkWtlsMasterKeyDeriveParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_WTLS_MASTER_KEY_DERIVE_PARAMS _lowLevelStruct;

		private CkWtlsRandomData _randomInfo;

		public CkVersion Version
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				CkVersion result = null;
				if (_lowLevelStruct.Version != IntPtr.Zero)
				{
					CK_VERSION cK_VERSION = default(CK_VERSION);
					UnmanagedMemory.Read(_lowLevelStruct.Version, cK_VERSION);
					result = new CkVersion(cK_VERSION.Major[0], cK_VERSION.Minor[0]);
				}
				return result;
			}
		}

		public CkWtlsMasterKeyDeriveParams(ulong digestMechanism, CkWtlsRandomData randomInfo, bool dh)
		{
			if (randomInfo == null)
			{
				throw new ArgumentNullException("randomInfo");
			}
			_randomInfo = randomInfo;
			_lowLevelStruct.DigestMechanism = digestMechanism;
			_lowLevelStruct.RandomInfo = (CK_WTLS_RANDOM_DATA)_randomInfo.ToMarshalableStructure();
			_lowLevelStruct.Version = (dh ? IntPtr.Zero : UnmanagedMemory.Allocate(UnmanagedMemory.SizeOf(typeof(CK_VERSION))));
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
				UnmanagedMemory.Free(ref _lowLevelStruct.Version);
				_disposed = true;
			}
		}

		~CkWtlsMasterKeyDeriveParams()
		{
			Dispose(false);
		}
	}
}
