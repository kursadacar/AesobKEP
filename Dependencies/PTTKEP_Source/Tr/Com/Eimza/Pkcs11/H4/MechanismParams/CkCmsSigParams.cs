using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkCmsSigParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private CK_CMS_SIG_PARAMS _lowLevelStruct;

		public CkCmsSigParams(ObjectHandle certificateHandle, uint? signingMechanism, uint? digestMechanism, string contentType, byte[] requestedAttributes, byte[] requiredAttributes)
		{
			_lowLevelStruct.CertificateHandle = 0u;
			_lowLevelStruct.SigningMechanism = IntPtr.Zero;
			_lowLevelStruct.DigestMechanism = IntPtr.Zero;
			_lowLevelStruct.ContentType = IntPtr.Zero;
			_lowLevelStruct.RequestedAttributes = IntPtr.Zero;
			_lowLevelStruct.RequestedAttributesLen = 0u;
			_lowLevelStruct.RequiredAttributes = IntPtr.Zero;
			_lowLevelStruct.RequiredAttributesLen = 0u;
			if (certificateHandle == null)
			{
				throw new ArgumentNullException("certificateHandle");
			}
			_lowLevelStruct.CertificateHandle = certificateHandle.ObjectId;
			if (signingMechanism.HasValue)
			{
				byte[] array = ConvertUtils.UIntToBytes(signingMechanism.Value);
				_lowLevelStruct.SigningMechanism = UnmanagedMemory.Allocate(array.Length);
				UnmanagedMemory.Write(_lowLevelStruct.SigningMechanism, array);
			}
			if (digestMechanism.HasValue)
			{
				byte[] array2 = ConvertUtils.UIntToBytes(digestMechanism.Value);
				_lowLevelStruct.DigestMechanism = UnmanagedMemory.Allocate(array2.Length);
				UnmanagedMemory.Write(_lowLevelStruct.DigestMechanism, array2);
			}
			if (contentType != null)
			{
				byte[] array3 = ConvertUtils.Utf8StringToBytes(contentType);
				Array.Resize(ref array3, array3.Length + 1);
				array3[array3.Length - 1] = 0;
				_lowLevelStruct.ContentType = UnmanagedMemory.Allocate(array3.Length);
				UnmanagedMemory.Write(_lowLevelStruct.ContentType, array3);
			}
			if (requestedAttributes != null)
			{
				_lowLevelStruct.RequestedAttributes = UnmanagedMemory.Allocate(requestedAttributes.Length);
				UnmanagedMemory.Write(_lowLevelStruct.RequestedAttributes, requestedAttributes);
				_lowLevelStruct.RequestedAttributesLen = Convert.ToUInt32(requestedAttributes.Length);
			}
			if (requiredAttributes != null)
			{
				_lowLevelStruct.RequiredAttributes = UnmanagedMemory.Allocate(requiredAttributes.Length);
				UnmanagedMemory.Write(_lowLevelStruct.RequiredAttributes, requiredAttributes);
				_lowLevelStruct.RequiredAttributesLen = Convert.ToUInt32(requiredAttributes.Length);
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
				UnmanagedMemory.Free(ref _lowLevelStruct.SigningMechanism);
				UnmanagedMemory.Free(ref _lowLevelStruct.DigestMechanism);
				UnmanagedMemory.Free(ref _lowLevelStruct.ContentType);
				UnmanagedMemory.Free(ref _lowLevelStruct.RequestedAttributes);
				_lowLevelStruct.RequestedAttributesLen = 0u;
				UnmanagedMemory.Free(ref _lowLevelStruct.RequiredAttributes);
				_lowLevelStruct.RequiredAttributesLen = 0u;
				_disposed = true;
			}
		}

		~CkCmsSigParams()
		{
			Dispose(false);
		}
	}
}
