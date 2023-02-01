using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkSkipjackPrivateWrapParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_SKIPJACK_PRIVATE_WRAP_PARAMS _lowLevelStruct;

		public CkSkipjackPrivateWrapParams(byte[] password, byte[] publicData, byte[] randomA, byte[] primeP, byte[] baseG, byte[] subprimeQ)
		{
			_lowLevelStruct.PasswordLen = 0uL;
			_lowLevelStruct.Password = IntPtr.Zero;
			_lowLevelStruct.PublicDataLen = 0uL;
			_lowLevelStruct.PublicData = IntPtr.Zero;
			_lowLevelStruct.PAndGLen = 0uL;
			_lowLevelStruct.QLen = 0uL;
			_lowLevelStruct.RandomLen = 0uL;
			_lowLevelStruct.RandomA = IntPtr.Zero;
			_lowLevelStruct.PrimeP = IntPtr.Zero;
			_lowLevelStruct.BaseG = IntPtr.Zero;
			_lowLevelStruct.SubprimeQ = IntPtr.Zero;
			if (password != null)
			{
				_lowLevelStruct.Password = UnmanagedMemory.Allocate(password.Length);
				UnmanagedMemory.Write(_lowLevelStruct.Password, password);
				_lowLevelStruct.PasswordLen = Convert.ToUInt64(password.Length);
			}
			if (publicData != null)
			{
				_lowLevelStruct.PublicData = UnmanagedMemory.Allocate(publicData.Length);
				UnmanagedMemory.Write(_lowLevelStruct.PublicData, publicData);
				_lowLevelStruct.PublicDataLen = Convert.ToUInt64(publicData.Length);
			}
			if (randomA != null)
			{
				_lowLevelStruct.RandomA = UnmanagedMemory.Allocate(randomA.Length);
				UnmanagedMemory.Write(_lowLevelStruct.RandomA, randomA);
				_lowLevelStruct.RandomLen = Convert.ToUInt64(randomA.Length);
			}
			if (primeP != null && baseG != null && primeP.Length != baseG.Length)
			{
				throw new ArgumentException("Length of primeP has to be the same as length of baseG");
			}
			if (primeP != null)
			{
				_lowLevelStruct.PrimeP = UnmanagedMemory.Allocate(primeP.Length);
				UnmanagedMemory.Write(_lowLevelStruct.PrimeP, primeP);
				_lowLevelStruct.PAndGLen = Convert.ToUInt64(primeP.Length);
			}
			if (baseG != null)
			{
				_lowLevelStruct.BaseG = UnmanagedMemory.Allocate(baseG.Length);
				UnmanagedMemory.Write(_lowLevelStruct.BaseG, baseG);
				_lowLevelStruct.PAndGLen = Convert.ToUInt64(baseG.Length);
			}
			if (subprimeQ != null)
			{
				_lowLevelStruct.SubprimeQ = UnmanagedMemory.Allocate(subprimeQ.Length);
				UnmanagedMemory.Write(_lowLevelStruct.SubprimeQ, subprimeQ);
				_lowLevelStruct.QLen = Convert.ToUInt64(subprimeQ.Length);
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
				UnmanagedMemory.Free(ref _lowLevelStruct.Password);
				_lowLevelStruct.PasswordLen = 0uL;
				UnmanagedMemory.Free(ref _lowLevelStruct.PublicData);
				_lowLevelStruct.PublicDataLen = 0uL;
				UnmanagedMemory.Free(ref _lowLevelStruct.RandomA);
				_lowLevelStruct.RandomLen = 0uL;
				UnmanagedMemory.Free(ref _lowLevelStruct.PrimeP);
				UnmanagedMemory.Free(ref _lowLevelStruct.BaseG);
				_lowLevelStruct.PAndGLen = 0uL;
				UnmanagedMemory.Free(ref _lowLevelStruct.SubprimeQ);
				_lowLevelStruct.QLen = 0uL;
				_disposed = true;
			}
		}

		~CkSkipjackPrivateWrapParams()
		{
			Dispose(false);
		}
	}
}
