using System;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ocsp
{
	internal class RevokedInfo : Asn1Encodable
	{
		private readonly DerGeneralizedTime revocationTime;

		private readonly CrlReason revocationReason;

		public DerGeneralizedTime RevocationTime
		{
			get
			{
				return revocationTime;
			}
		}

		public CrlReason RevocationReason
		{
			get
			{
				return revocationReason;
			}
		}

		public static RevokedInfo GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		public static RevokedInfo GetInstance(object obj)
		{
			if (obj == null || obj is RevokedInfo)
			{
				return (RevokedInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new RevokedInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + obj.GetType().Name, "obj");
		}

		public RevokedInfo(DerGeneralizedTime revocationTime)
			: this(revocationTime, null)
		{
		}

		public RevokedInfo(DerGeneralizedTime revocationTime, CrlReason revocationReason)
		{
			if (revocationTime == null)
			{
				throw new ArgumentNullException("revocationTime");
			}
			this.revocationTime = revocationTime;
			this.revocationReason = revocationReason;
		}

		private RevokedInfo(Asn1Sequence seq)
		{
			revocationTime = (DerGeneralizedTime)seq[0];
			if (seq.Count > 1)
			{
				revocationReason = new CrlReason(DerEnumerated.GetInstance((Asn1TaggedObject)seq[1], true));
			}
		}

		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(revocationTime);
			if (revocationReason != null)
			{
				asn1EncodableVector.Add(new DerTaggedObject(true, 0, revocationReason));
			}
			return new DerSequence(asn1EncodableVector);
		}
	}
}
