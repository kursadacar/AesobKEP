using System;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ocsp;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class CertificateStatus
	{
		protected readonly byte mStatusType;

		protected readonly object mResponse;

		public virtual byte StatusType
		{
			get
			{
				return mStatusType;
			}
		}

		public virtual object Response
		{
			get
			{
				return mResponse;
			}
		}

		public CertificateStatus(byte statusType, object response)
		{
			if (!IsCorrectType(statusType, response))
			{
				throw new ArgumentException("not an instance of the correct type", "response");
			}
			mStatusType = statusType;
			mResponse = response;
		}

		public virtual OcspResponse GetOcspResponse()
		{
			if (!IsCorrectType(1, mResponse))
			{
				throw new InvalidOperationException("'response' is not an OcspResponse");
			}
			return (OcspResponse)mResponse;
		}

		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteUint8(mStatusType, output);
			if (mStatusType == 1)
			{
				TlsUtilities.WriteOpaque24(((OcspResponse)mResponse).GetEncoded("DER"), output);
				return;
			}
			throw new TlsFatalAlert(80);
		}

		public static CertificateStatus Parse(Stream input)
		{
			byte num = TlsUtilities.ReadUint8(input);
			if (num == 1)
			{
				object instance = OcspResponse.GetInstance(TlsUtilities.ReadDerObject(TlsUtilities.ReadOpaque24(input)));
				return new CertificateStatus(num, instance);
			}
			throw new TlsFatalAlert(50);
		}

		protected static bool IsCorrectType(byte statusType, object response)
		{
			if (statusType == 1)
			{
				return response is OcspResponse;
			}
			throw new ArgumentException("unsupported value", "statusType");
		}
	}
}
