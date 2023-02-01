using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms
{
	internal class IssuerAndSerialNumber : Asn1Encodable
	{
		private readonly X509Name name;

		private readonly DerInteger serialNumber;

		public X509Name Name
		{
			get
			{
				return name;
			}
		}

		public DerInteger SerialNumber
		{
			get
			{
				return serialNumber;
			}
		}

		public static IssuerAndSerialNumber GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			IssuerAndSerialNumber issuerAndSerialNumber = obj as IssuerAndSerialNumber;
			if (issuerAndSerialNumber != null)
			{
				return issuerAndSerialNumber;
			}
			return new IssuerAndSerialNumber(Asn1Sequence.GetInstance(obj));
		}

		public IssuerAndSerialNumber(Asn1Sequence seq)
		{
			name = X509Name.GetInstance(seq[0]);
			serialNumber = (DerInteger)seq[1];
		}

		public IssuerAndSerialNumber(X509Name name, BigInteger serialNumber)
		{
			this.name = name;
			this.serialNumber = new DerInteger(serialNumber);
		}

		public IssuerAndSerialNumber(X509Name name, DerInteger serialNumber)
		{
			this.name = name;
			this.serialNumber = serialNumber;
		}

		public IssuerAndSerialNumber(X509Certificate certificate)
		{
			name = certificate.IssuerDN;
			serialNumber = new DerInteger(certificate.SerialNumber);
		}

		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(name, serialNumber);
		}
	}
}
