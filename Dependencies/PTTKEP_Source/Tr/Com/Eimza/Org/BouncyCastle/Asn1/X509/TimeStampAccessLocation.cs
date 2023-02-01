using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509
{
	internal class TimeStampAccessLocation : Asn1Encodable
	{
		private readonly DerInteger id;

		private readonly DerTaggedObject tsaUrl;

		public virtual DerInteger Id
		{
			get
			{
				return id;
			}
		}

		public virtual DerTaggedObject TsaURL
		{
			get
			{
				return tsaUrl;
			}
		}

		public TimeStampAccessLocation(DerInteger tsaID, DerTaggedObject tsaURL)
		{
			id = tsaID;
			tsaUrl = tsaURL;
		}

		private TimeStampAccessLocation(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			id = DerInteger.GetInstance(seq[0]);
			tsaUrl = Asn1TaggedObject.GetInstance(seq[1]) as DerTaggedObject;
		}

		public static TimeStampAccessLocation GetInstance(object obj)
		{
			if (obj is TimeStampAccessLocation)
			{
				return (TimeStampAccessLocation)obj;
			}
			if (obj == null)
			{
				return null;
			}
			return new TimeStampAccessLocation(Asn1Sequence.GetInstance(obj));
		}

		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(id, tsaUrl);
		}
	}
}
