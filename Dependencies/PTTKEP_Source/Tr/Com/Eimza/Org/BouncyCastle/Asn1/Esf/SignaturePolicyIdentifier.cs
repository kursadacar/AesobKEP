using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Esf
{
	internal class SignaturePolicyIdentifier : Asn1Encodable, IAsn1Choice
	{
		private readonly SignaturePolicyId sigPolicy;

		public SignaturePolicyId SignaturePolicyId
		{
			get
			{
				return sigPolicy;
			}
		}

		public static SignaturePolicyIdentifier GetInstance(object obj)
		{
			if (obj == null || obj is SignaturePolicyIdentifier)
			{
				return (SignaturePolicyIdentifier)obj;
			}
			if (obj is SignaturePolicyId)
			{
				return new SignaturePolicyIdentifier((SignaturePolicyId)obj);
			}
			if (obj is Asn1Null)
			{
				return new SignaturePolicyIdentifier();
			}
			throw new ArgumentException("Unknown object in 'SignaturePolicyIdentifier' factory: " + obj.GetType().Name, "obj");
		}

		public SignaturePolicyIdentifier()
		{
			sigPolicy = null;
		}

		public SignaturePolicyIdentifier(SignaturePolicyId signaturePolicyId)
		{
			if (signaturePolicyId == null)
			{
				throw new ArgumentNullException("signaturePolicyId");
			}
			sigPolicy = signaturePolicyId;
		}

		public override Asn1Object ToAsn1Object()
		{
			if (sigPolicy != null)
			{
				return sigPolicy.ToAsn1Object();
			}
			return DerNull.Instance;
		}
	}
}
