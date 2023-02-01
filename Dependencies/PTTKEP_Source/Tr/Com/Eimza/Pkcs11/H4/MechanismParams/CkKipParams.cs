using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkKipParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_KIP_PARAMS _lowLevelStruct;

		public CkKipParams(uint? mechanism, ObjectHandle key, byte[] seed)
		{
			_lowLevelStruct.Mechanism = IntPtr.Zero;
			_lowLevelStruct.Key = 0u;
			_lowLevelStruct.Seed = IntPtr.Zero;
			_lowLevelStruct.SeedLen = 0u;
			if (mechanism.HasValue)
			{
				byte[] array = ConvertUtils.UIntToBytes(mechanism.Value);
				_lowLevelStruct.Mechanism = UnmanagedMemory.Allocate(array.Length);
				UnmanagedMemory.Write(_lowLevelStruct.Mechanism, array);
			}
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			_lowLevelStruct.Key = key.ObjectId;
			if (seed != null)
			{
				_lowLevelStruct.Seed = UnmanagedMemory.Allocate(seed.Length);
				UnmanagedMemory.Write(_lowLevelStruct.Seed, seed);
				_lowLevelStruct.SeedLen = Convert.ToUInt32(seed.Length);
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
				UnmanagedMemory.Free(ref _lowLevelStruct.Mechanism);
				UnmanagedMemory.Free(ref _lowLevelStruct.Seed);
				_lowLevelStruct.SeedLen = 0u;
				_disposed = true;
			}
		}

		~CkKipParams()
		{
			Dispose(false);
		}
	}
}
