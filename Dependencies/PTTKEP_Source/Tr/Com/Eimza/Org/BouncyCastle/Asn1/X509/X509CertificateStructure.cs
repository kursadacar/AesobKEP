using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509
{
	internal class X509CertificateStructure : Asn1Encodable
	{
		private readonly TbsCertificateStructure tbsCert;

		private readonly AlgorithmIdentifier sigAlgID;

		private readonly DerBitString sig;

		public TbsCertificateStructure TbsCertificate
		{
			get
			{
				return tbsCert;
			}
		}

		public int Version
		{
			get
			{
				return tbsCert.Version;
			}
		}

		public DerInteger SerialNumber
		{
			get
			{
				return tbsCert.SerialNumber;
			}
		}

		public X509Name Issuer
		{
			get
			{
				return tbsCert.Issuer;
			}
		}

		public Time StartDate
		{
			get
			{
				return tbsCert.StartDate;
			}
		}

		public Time EndDate
		{
			get
			{
				return tbsCert.EndDate;
			}
		}

		public X509Name Subject
		{
			get
			{
				return tbsCert.Subject;
			}
		}

		public SubjectPublicKeyInfo SubjectPublicKeyInfo
		{
			get
			{
				return tbsCert.SubjectPublicKeyInfo;
			}
		}

		public AlgorithmIdentifier SignatureAlgorithm
		{
			get
			{
				return sigAlgID;
			}
		}

		public DerBitString Signature
		{
			get
			{
				return sig;
			}
		}

		public static X509CertificateStructure GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		public static X509CertificateStructure GetInstance(object obj)
		{
			if (obj is X509CertificateStructure)
			{
				return (X509CertificateStructure)obj;
			}
			if (obj == null)
			{
				return null;
			}
			return new X509CertificateStructure(Asn1Sequence.GetInstance(obj));
		}

		public X509CertificateStructure(TbsCertificateStructure tbsCert, AlgorithmIdentifier sigAlgID, DerBitString sig)
		{
			if (tbsCert == null)
			{
				throw new ArgumentNullException("tbsCert");
			}
			if (sigAlgID == null)
			{
				throw new ArgumentNullException("sigAlgID");
			}
			if (sig == null)
			{
				throw new ArgumentNullException("sig");
			}
			this.tbsCert = tbsCert;
			this.sigAlgID = sigAlgID;
			this.sig = sig;
		}

		private X509CertificateStructure(Asn1Sequence seq)
		{
			if (seq.Count != 3)
			{
				throw new ArgumentException("sequence wrong size for a certificate", "seq");
			}
			tbsCert = TbsCertificateStructure.GetInstance(seq[0]);
			sigAlgID = AlgorithmIdentifier.GetInstance(seq[1]);
			sig = DerBitString.GetInstance(seq[2]);
		}

		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(tbsCert, sigAlgID, sig);
		}
	}
}
