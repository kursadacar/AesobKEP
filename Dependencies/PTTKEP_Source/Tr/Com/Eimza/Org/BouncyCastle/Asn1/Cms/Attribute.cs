using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms
{
	internal class Attribute : Asn1Encodable
	{
		private readonly DerObjectIdentifier attrType;

		private readonly Asn1Set attrValues;

		public DerObjectIdentifier AttrType
		{
			get
			{
				return attrType;
			}
		}

		public Asn1Set AttrValues
		{
			get
			{
				return attrValues;
			}
		}

		public static Attribute GetInstance(object obj)
		{
			if (obj == null || obj is Attribute)
			{
				return (Attribute)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new Attribute((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + obj.GetType().Name, "obj");
		}

		public Attribute(Asn1Sequence seq)
		{
			attrType = (DerObjectIdentifier)seq[0];
			attrValues = (Asn1Set)seq[1];
		}

		public Attribute(DerObjectIdentifier attrType, Asn1Set attrValues)
		{
			this.attrType = attrType;
			this.attrValues = attrValues;
		}

		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(attrType, attrValues);
		}
	}
}
