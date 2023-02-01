using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkSkipjackRelayxParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_SKIPJACK_RELAYX_PARAMS _lowLevelStruct;

		public CkSkipjackRelayxParams(byte[] oldWrappedX, byte[] oldPassword, byte[] oldPublicData, byte[] oldRandomA, byte[] newPassword, byte[] newPublicData, byte[] newRandomA)
		{
			_lowLevelStruct.OldWrappedXLen = 0u;
			_lowLevelStruct.OldWrappedX = IntPtr.Zero;
			_lowLevelStruct.OldPasswordLen = 0u;
			_lowLevelStruct.OldPassword = IntPtr.Zero;
			_lowLevelStruct.OldPublicDataLen = 0u;
			_lowLevelStruct.OldPublicData = IntPtr.Zero;
			_lowLevelStruct.OldRandomLen = 0u;
			_lowLevelStruct.OldRandomA = IntPtr.Zero;
			_lowLevelStruct.NewPasswordLen = 0u;
			_lowLevelStruct.NewPassword = IntPtr.Zero;
			_lowLevelStruct.NewPublicDataLen = 0u;
			_lowLevelStruct.NewPublicData = IntPtr.Zero;
			_lowLevelStruct.NewRandomLen = 0u;
			_lowLevelStruct.NewRandomA = IntPtr.Zero;
			if (oldWrappedX != null)
			{
				_lowLevelStruct.OldWrappedX = UnmanagedMemory.Allocate(oldWrappedX.Length);
				UnmanagedMemory.Write(_lowLevelStruct.OldWrappedX, oldWrappedX);
				_lowLevelStruct.OldWrappedXLen = Convert.ToUInt32(oldWrappedX.Length);
			}
			if (oldPassword != null)
			{
				_lowLevelStruct.OldPassword = UnmanagedMemory.Allocate(oldPassword.Length);
				UnmanagedMemory.Write(_lowLevelStruct.OldPassword, oldPassword);
				_lowLevelStruct.OldPasswordLen = Convert.ToUInt32(oldPassword.Length);
			}
			if (oldPublicData != null)
			{
				_lowLevelStruct.OldPublicData = UnmanagedMemory.Allocate(oldPublicData.Length);
				UnmanagedMemory.Write(_lowLevelStruct.OldPublicData, oldPublicData);
				_lowLevelStruct.OldPublicDataLen = Convert.ToUInt32(oldPublicData.Length);
			}
			if (oldRandomA != null)
			{
				_lowLevelStruct.OldRandomA = UnmanagedMemory.Allocate(oldRandomA.Length);
				UnmanagedMemory.Write(_lowLevelStruct.OldRandomA, oldRandomA);
				_lowLevelStruct.OldRandomLen = Convert.ToUInt32(oldRandomA.Length);
			}
			if (newPassword != null)
			{
				_lowLevelStruct.NewPassword = UnmanagedMemory.Allocate(newPassword.Length);
				UnmanagedMemory.Write(_lowLevelStruct.NewPassword, newPassword);
				_lowLevelStruct.NewPasswordLen = Convert.ToUInt32(newPassword.Length);
			}
			if (newPublicData != null)
			{
				_lowLevelStruct.NewPublicData = UnmanagedMemory.Allocate(newPublicData.Length);
				UnmanagedMemory.Write(_lowLevelStruct.NewPublicData, newPublicData);
				_lowLevelStruct.NewPublicDataLen = Convert.ToUInt32(newPublicData.Length);
			}
			if (newRandomA != null)
			{
				_lowLevelStruct.NewRandomA = UnmanagedMemory.Allocate(newRandomA.Length);
				UnmanagedMemory.Write(_lowLevelStruct.NewRandomA, newRandomA);
				_lowLevelStruct.NewRandomLen = Convert.ToUInt32(newRandomA.Length);
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
				UnmanagedMemory.Free(ref _lowLevelStruct.OldWrappedX);
				_lowLevelStruct.OldWrappedXLen = 0u;
				UnmanagedMemory.Free(ref _lowLevelStruct.OldPassword);
				_lowLevelStruct.OldPasswordLen = 0u;
				UnmanagedMemory.Free(ref _lowLevelStruct.OldPublicData);
				_lowLevelStruct.OldPublicDataLen = 0u;
				UnmanagedMemory.Free(ref _lowLevelStruct.OldRandomA);
				_lowLevelStruct.OldRandomLen = 0u;
				UnmanagedMemory.Free(ref _lowLevelStruct.NewPassword);
				_lowLevelStruct.NewPasswordLen = 0u;
				UnmanagedMemory.Free(ref _lowLevelStruct.NewPublicData);
				_lowLevelStruct.NewPublicDataLen = 0u;
				UnmanagedMemory.Free(ref _lowLevelStruct.NewRandomA);
				_lowLevelStruct.NewRandomLen = 0u;
				_disposed = true;
			}
		}

		~CkSkipjackRelayxParams()
		{
			Dispose(false);
		}
	}
}
