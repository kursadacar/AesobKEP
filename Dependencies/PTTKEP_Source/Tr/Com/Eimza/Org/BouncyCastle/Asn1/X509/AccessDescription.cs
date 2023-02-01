using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509
{
	internal class AccessDescription : Asn1Encodable
	{
		public static readonly DerObjectIdentifier IdADOcsp = new DerObjectIdentifier("1.3.6.1.5.5.7.48.1");

		public static readonly DerObjectIdentifier IdADCAIssuers = new DerObjectIdentifier("1.3.6.1.5.5.7.48.2");

		public static readonly DerObjectIdentifier IdADTimeStamp = new DerObjectIdentifier("1.3.6.1.5.5.7.48.3");

		public static readonly DerObjectIdentifier IdADVCS = new DerObjectIdentifier("1.3.6.1.5.5.7.48.4");

		private readonly DerObjectIdentifier accessMethod;

		private readonly GeneralName accessLocation;

		public DerObjectIdentifier AccessMethod
		{
			get
			{
				return accessMethod;
			}
		}

		public GeneralName AccessLocation
		{
			get
			{
				return accessLocation;
			}
		}

		public static AccessDescription GetInstance(object obj)
		{
			if (obj is AccessDescription)
			{
				return (AccessDescription)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new AccessDescription((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + obj.GetType().Name, "obj");
		}

		private AccessDescription(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("wrong number of elements in sequence");
			}
			accessMethod = DerObjectIdentifier.GetInstance(seq[0]);
			accessLocation = GeneralName.GetInstance(seq[1]);
		}

		public AccessDescription(DerObjectIdentifier oid, GeneralName location)
		{
			accessMethod = oid;
			accessLocation = location;
		}

		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(accessMethod, accessLocation);
		}

		public override string ToString()
		{
			return "AccessDescription: Oid(" + accessMethod.Id + ")";
		}
	}
}
