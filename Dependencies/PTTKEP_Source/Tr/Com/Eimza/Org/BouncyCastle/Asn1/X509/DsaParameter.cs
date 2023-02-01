using System;
using Tr.Com.Eimza.Org.BouncyCastle.Math;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509
{
	internal class DsaParameter : Asn1Encodable
	{
		internal readonly DerInteger p;

		internal readonly DerInteger q;

		internal readonly DerInteger g;

		public BigInteger P
		{
			get
			{
				return p.PositiveValue;
			}
		}

		public BigInteger Q
		{
			get
			{
				return q.PositiveValue;
			}
		}

		public BigInteger G
		{
			get
			{
				return g.PositiveValue;
			}
		}

		public static DsaParameter GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		public static DsaParameter GetInstance(object obj)
		{
			if (obj == null || obj is DsaParameter)
			{
				return (DsaParameter)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new DsaParameter((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid DsaParameter: " + obj.GetType().Name);
		}

		public DsaParameter(BigInteger p, BigInteger q, BigInteger g)
		{
			this.p = new DerInteger(p);
			this.q = new DerInteger(q);
			this.g = new DerInteger(g);
		}

		private DsaParameter(Asn1Sequence seq)
		{
			if (seq.Count != 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			p = DerInteger.GetInstance(seq[0]);
			q = DerInteger.GetInstance(seq[1]);
			g = DerInteger.GetInstance(seq[2]);
		}

		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(p, q, g);
		}
	}
}
