using System;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Crmf;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cmp
{
	internal class RevRepContentBuilder
	{
		private readonly Asn1EncodableVector status = new Asn1EncodableVector();

		private readonly Asn1EncodableVector revCerts = new Asn1EncodableVector();

		private readonly Asn1EncodableVector crls = new Asn1EncodableVector();

		public virtual RevRepContentBuilder Add(PkiStatusInfo status)
		{
			this.status.Add(status);
			return this;
		}

		public virtual RevRepContentBuilder Add(PkiStatusInfo status, CertId certId)
		{
			if (this.status.Count != revCerts.Count)
			{
				throw new InvalidOperationException("status and revCerts sequence must be in common order");
			}
			this.status.Add(status);
			revCerts.Add(certId);
			return this;
		}

		public virtual RevRepContentBuilder AddCrl(CertificateList crl)
		{
			crls.Add(crl);
			return this;
		}

		public virtual RevRepContent Build()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector();
			asn1EncodableVector.Add(new DerSequence(status));
			if (revCerts.Count != 0)
			{
				asn1EncodableVector.Add(new DerTaggedObject(true, 0, new DerSequence(revCerts)));
			}
			if (crls.Count != 0)
			{
				asn1EncodableVector.Add(new DerTaggedObject(true, 1, new DerSequence(crls)));
			}
			return RevRepContent.GetInstance(new DerSequence(asn1EncodableVector));
		}
	}
}
