using System;
using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class CertificateStatusRequest
	{
		protected readonly byte mStatusType;

		protected readonly object mRequest;

		public virtual byte StatusType
		{
			get
			{
				return mStatusType;
			}
		}

		public virtual object Request
		{
			get
			{
				return mRequest;
			}
		}

		public CertificateStatusRequest(byte statusType, object request)
		{
			if (!IsCorrectType(statusType, request))
			{
				throw new ArgumentException("not an instance of the correct type", "request");
			}
			mStatusType = statusType;
			mRequest = request;
		}

		public virtual OcspStatusRequest GetOcspStatusRequest()
		{
			if (!IsCorrectType(1, mRequest))
			{
				throw new InvalidOperationException("'request' is not an OCSPStatusRequest");
			}
			return (OcspStatusRequest)mRequest;
		}

		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteUint8(mStatusType, output);
			if (mStatusType == 1)
			{
				((OcspStatusRequest)mRequest).Encode(output);
				return;
			}
			throw new TlsFatalAlert(80);
		}

		public static CertificateStatusRequest Parse(Stream input)
		{
			byte num = TlsUtilities.ReadUint8(input);
			if (num == 1)
			{
				object request = OcspStatusRequest.Parse(input);
				return new CertificateStatusRequest(num, request);
			}
			throw new TlsFatalAlert(50);
		}

		protected static bool IsCorrectType(byte statusType, object request)
		{
			if (statusType == 1)
			{
				return request is OcspStatusRequest;
			}
			throw new ArgumentException("unsupported value", "statusType");
		}
	}
}
