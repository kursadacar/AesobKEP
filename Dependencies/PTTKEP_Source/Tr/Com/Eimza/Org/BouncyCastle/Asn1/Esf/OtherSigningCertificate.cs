using System;
using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Collections;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Esf
{
	internal class OtherSigningCertificate : Asn1Encodable
	{
		private readonly Asn1Sequence certs;

		private readonly Asn1Sequence policies;

		public static OtherSigningCertificate GetInstance(object obj)
		{
			if (obj == null || obj is OtherSigningCertificate)
			{
				return (OtherSigningCertificate)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OtherSigningCertificate((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'OtherSigningCertificate' factory: " + obj.GetType().Name, "obj");
		}

		private OtherSigningCertificate(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count < 1 || seq.Count > 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			certs = Asn1Sequence.GetInstance(seq[0].ToAsn1Object());
			if (seq.Count > 1)
			{
				policies = Asn1Sequence.GetInstance(seq[1].ToAsn1Object());
			}
		}

		public OtherSigningCertificate(params OtherCertID[] certs)
			: this(certs, (PolicyInformation[])null)
		{
		}

		public OtherSigningCertificate(OtherCertID[] certs, params PolicyInformation[] policies)
		{
			if (certs == null)
			{
				throw new ArgumentNullException("certs");
			}
			Asn1Encodable[] v = certs;
			this.certs = new DerSequence(v);
			if (policies != null)
			{
				v = policies;
				this.policies = new DerSequence(v);
			}
		}

		public OtherSigningCertificate(IEnumerable certs)
			: this(certs, null)
		{
		}

		public OtherSigningCertificate(IEnumerable certs, IEnumerable policies)
		{
			if (certs == null)
			{
				throw new ArgumentNullException("certs");
			}
			if (!CollectionUtilities.CheckElementsAreOfType(certs, typeof(OtherCertID)))
			{
				throw new ArgumentException("Must contain only 'OtherCertID' objects", "certs");
			}
			this.certs = new DerSequence(Asn1EncodableVector.FromEnumerable(certs));
			if (policies != null)
			{
				if (!CollectionUtilities.CheckElementsAreOfType(policies, typeof(PolicyInformation)))
				{
					throw new ArgumentException("Must contain only 'PolicyInformation' objects", "policies");
				}
				this.policies = new DerSequence(Asn1EncodableVector.FromEnumerable(policies));
			}
		}

		public OtherCertID[] GetCerts()
		{
			OtherCertID[] array = new OtherCertID[certs.Count];
			for (int i = 0; i < certs.Count; i++)
			{
				array[i] = OtherCertID.GetInstance(certs[i].ToAsn1Object());
			}
			return array;
		}

		public PolicyInformation[] GetPolicies()
		{
			if (policies == null)
			{
				return null;
			}
			PolicyInformation[] array = new PolicyInformation[policies.Count];
			for (int i = 0; i < policies.Count; i++)
			{
				array[i] = PolicyInformation.GetInstance(policies[i].ToAsn1Object());
			}
			return array;
		}

		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(certs);
			if (policies != null)
			{
				asn1EncodableVector.Add(policies);
			}
			return new DerSequence(asn1EncodableVector);
		}
	}
}
