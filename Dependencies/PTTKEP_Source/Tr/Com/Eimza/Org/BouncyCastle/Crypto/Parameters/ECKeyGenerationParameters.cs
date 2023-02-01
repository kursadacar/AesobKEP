using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters
{
	internal class ECKeyGenerationParameters : KeyGenerationParameters
	{
		private readonly ECDomainParameters domainParams;

		private readonly DerObjectIdentifier publicKeyParamSet;

		public ECDomainParameters DomainParameters
		{
			get
			{
				return domainParams;
			}
		}

		public DerObjectIdentifier PublicKeyParamSet
		{
			get
			{
				return publicKeyParamSet;
			}
		}

		public ECKeyGenerationParameters(ECDomainParameters domainParameters, SecureRandom random)
			: base(random, domainParameters.N.BitLength)
		{
			domainParams = domainParameters;
		}

		public ECKeyGenerationParameters(DerObjectIdentifier publicKeyParamSet, SecureRandom random)
			: this(ECKeyParameters.LookupParameters(publicKeyParamSet), random)
		{
			this.publicKeyParamSet = publicKeyParamSet;
		}
	}
}
