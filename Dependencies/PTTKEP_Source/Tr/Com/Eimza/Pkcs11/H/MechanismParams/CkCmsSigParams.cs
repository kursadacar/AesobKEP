using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkCmsSigParams : IMechanismParams, IDisposable
	{
		private bool _disposed;

		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkCmsSigParams _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkCmsSigParams _params8;

		public CkCmsSigParams(ObjectHandle certificateHandle, ulong? signingMechanism, ulong? digestMechanism, string contentType, byte[] requestedAttributes, byte[] requiredAttributes)
		{
			if (UnmanagedLong.Size == 4)
			{
				uint? signingMechanism2 = ((!signingMechanism.HasValue) ? null : new uint?(Convert.ToUInt32(signingMechanism.Value)));
				uint? digestMechanism2 = ((!digestMechanism.HasValue) ? null : new uint?(Convert.ToUInt32(digestMechanism.Value)));
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkCmsSigParams(certificateHandle.ObjectHandle4, signingMechanism2, digestMechanism2, contentType, requestedAttributes, requiredAttributes);
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkCmsSigParams(certificateHandle.ObjectHandle8, signingMechanism, digestMechanism, contentType, requestedAttributes, requiredAttributes);
			}
		}

		public object ToMarshalableStructure()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				return _params4.ToMarshalableStructure();
			}
			return _params8.ToMarshalableStructure();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}
			if (disposing)
			{
				if (_params4 != null)
				{
					_params4.Dispose();
					_params4 = null;
				}
				if (_params8 != null)
				{
					_params8.Dispose();
					_params8 = null;
				}
			}
			_disposed = true;
		}

		~CkCmsSigParams()
		{
			Dispose(false);
		}
	}
}
