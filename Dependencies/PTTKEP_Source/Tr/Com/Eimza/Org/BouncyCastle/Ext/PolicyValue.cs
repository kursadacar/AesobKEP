namespace Tr.Com.Eimza.Org.BouncyCastle.Ext
{
	internal class PolicyValue
	{
		private readonly string signaturePolicyId;

		private readonly SignaturePolicy policy;

		public PolicyValue(string signaturePolicyId)
		{
			this.signaturePolicyId = signaturePolicyId;
			policy = SignaturePolicy.EXPLICIT;
		}

		public PolicyValue()
		{
			policy = SignaturePolicy.IMPLICIT;
		}

		public virtual string GetSignaturePolicyId()
		{
			return signaturePolicyId;
		}

		public virtual SignaturePolicy GetPolicy()
		{
			return policy;
		}

		public override string ToString()
		{
			if (policy == SignaturePolicy.EXPLICIT)
			{
				return signaturePolicyId;
			}
			return policy.ToString();
		}
	}
}
