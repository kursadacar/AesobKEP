using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkWtlsPrfParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_WTLS_PRF_PARAMS _lowLevelStruct;

		public byte[] Output
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				int size = UnmanagedMemory.SizeOf(typeof(uint));
				uint value = ConvertUtils.BytesToUInt(UnmanagedMemory.Read(_lowLevelStruct.OutputLen, size));
				return UnmanagedMemory.Read(_lowLevelStruct.Output, Convert.ToInt32(value));
			}
		}

		public CkWtlsPrfParams(uint digestMechanism, byte[] seed, byte[] label, uint outputLen)
		{
			_lowLevelStruct.DigestMechanism = 0u;
			_lowLevelStruct.Seed = IntPtr.Zero;
			_lowLevelStruct.SeedLen = 0u;
			_lowLevelStruct.Label = IntPtr.Zero;
			_lowLevelStruct.LabelLen = 0u;
			_lowLevelStruct.Output = IntPtr.Zero;
			_lowLevelStruct.OutputLen = IntPtr.Zero;
			_lowLevelStruct.DigestMechanism = digestMechanism;
			if (seed != null)
			{
				_lowLevelStruct.Seed = UnmanagedMemory.Allocate(seed.Length);
				UnmanagedMemory.Write(_lowLevelStruct.Seed, seed);
				_lowLevelStruct.SeedLen = Convert.ToUInt32(seed.Length);
			}
			if (label != null)
			{
				_lowLevelStruct.Label = UnmanagedMemory.Allocate(label.Length);
				UnmanagedMemory.Write(_lowLevelStruct.Label, label);
				_lowLevelStruct.LabelLen = Convert.ToUInt32(label.Length);
			}
			if (outputLen < 1)
			{
				throw new ArgumentException("Value has to be positive number", "outputLen");
			}
			_lowLevelStruct.Output = UnmanagedMemory.Allocate(Convert.ToInt32(outputLen));
			byte[] array = ConvertUtils.UIntToBytes(outputLen);
			_lowLevelStruct.OutputLen = UnmanagedMemory.Allocate(array.Length);
			UnmanagedMemory.Write(_lowLevelStruct.OutputLen, array);
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
				UnmanagedMemory.Free(ref _lowLevelStruct.Seed);
				_lowLevelStruct.SeedLen = 0u;
				UnmanagedMemory.Free(ref _lowLevelStruct.Label);
				_lowLevelStruct.LabelLen = 0u;
				UnmanagedMemory.Free(ref _lowLevelStruct.Output);
				UnmanagedMemory.Free(ref _lowLevelStruct.OutputLen);
				_disposed = true;
			}
		}

		~CkWtlsPrfParams()
		{
			Dispose(false);
		}
	}
}
