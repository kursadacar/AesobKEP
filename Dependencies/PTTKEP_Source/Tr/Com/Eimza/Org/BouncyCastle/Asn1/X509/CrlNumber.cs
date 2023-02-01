using Tr.Com.Eimza.Org.BouncyCastle.Math;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509
{
	internal class CrlNumber : DerInteger
	{
		public BigInteger Number
		{
			get
			{
				return base.PositiveValue;
			}
		}

		public CrlNumber(BigInteger number)
			: base(number)
		{
		}

		public override string ToString()
		{
			BigInteger number = Number;
			return "CRLNumber: " + ((number != null) ? number.ToString() : null);
		}
	}
}
