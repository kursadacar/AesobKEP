using System;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Crmf;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cmp
{
	internal class RevAnnContent : Asn1Encodable
	{
		private readonly PkiStatusEncodable status;

		private readonly CertId certId;

		private readonly DerGeneralizedTime willBeRevokedAt;

		private readonly DerGeneralizedTime badSinceDate;

		private readonly X509Extensions crlDetails;

		public virtual PkiStatusEncodable Status
		{
			get
			{
				return status;
			}
		}

		public virtual CertId CertID
		{
			get
			{
				return certId;
			}
		}

		public virtual DerGeneralizedTime WillBeRevokedAt
		{
			get
			{
				return willBeRevokedAt;
			}
		}

		public virtual DerGeneralizedTime BadSinceDate
		{
			get
			{
				return badSinceDate;
			}
		}

		public virtual X509Extensions CrlDetails
		{
			get
			{
				return crlDetails;
			}
		}

		private RevAnnContent(Asn1Sequence seq)
		{
			status = PkiStatusEncodable.GetInstance(seq[0]);
			certId = CertId.GetInstance(seq[1]);
			willBeRevokedAt = DerGeneralizedTime.GetInstance(seq[2]);
			badSinceDate = DerGeneralizedTime.GetInstance(seq[3]);
			if (seq.Count > 4)
			{
				crlDetails = X509Extensions.GetInstance(seq[4]);
			}
		}

		public static RevAnnContent GetInstance(object obj)
		{
			if (obj is RevAnnContent)
			{
				return (RevAnnContent)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new RevAnnContent((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + obj.GetType().Name, "obj");
		}

		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(status, certId, willBeRevokedAt, badSinceDate);
			asn1EncodableVector.AddOptional(crlDetails);
			return new DerSequence(asn1EncodableVector);
		}
	}
}
