using System;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ocsp;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Date;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ocsp
{
	internal class SingleResp : X509ExtensionBase
	{
		internal readonly SingleResponse resp;

		public DateTime ThisUpdate
		{
			get
			{
				return resp.ThisUpdate.ToDateTime();
			}
		}

		public DateTimeObject NextUpdate
		{
			get
			{
				if (resp.NextUpdate != null)
				{
					return new DateTimeObject(resp.NextUpdate.ToDateTime());
				}
				return null;
			}
		}

		public X509Extensions SingleExtensions
		{
			get
			{
				return resp.SingleExtensions;
			}
		}

		public SingleResp(SingleResponse resp)
		{
			this.resp = resp;
		}

		public CertificateID GetCertID()
		{
			return new CertificateID(resp.CertId);
		}

		public object GetCertStatus()
		{
			CertStatus certStatus = resp.CertStatus;
			if (certStatus.TagNo == 0)
			{
				return null;
			}
			if (certStatus.TagNo == 1)
			{
				return new RevokedStatus(RevokedInfo.GetInstance(certStatus.Status));
			}
			return new UnknownStatus();
		}

		protected override X509Extensions GetX509Extensions()
		{
			return SingleExtensions;
		}
	}
}
