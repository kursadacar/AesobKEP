using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Smime
{
	internal class SmimeCapabilitiesAttribute : AttributeX509
	{
		public SmimeCapabilitiesAttribute(SmimeCapabilityVector capabilities)
			: base(SmimeAttributes.SmimeCapabilities, new DerSet(new DerSequence(capabilities.ToAsn1EncodableVector())))
		{
		}
	}
}
