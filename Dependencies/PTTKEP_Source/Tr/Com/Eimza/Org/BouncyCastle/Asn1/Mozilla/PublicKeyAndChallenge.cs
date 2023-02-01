using System;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Mozilla
{
	internal class PublicKeyAndChallenge : Asn1Encodable
	{
		private Asn1Sequence pkacSeq;

		private SubjectPublicKeyInfo spki;

		private DerIA5String challenge;

		public SubjectPublicKeyInfo SubjectPublicKeyInfo
		{
			get
			{
				return spki;
			}
		}

		public DerIA5String Challenge
		{
			get
			{
				return challenge;
			}
		}

		public static PublicKeyAndChallenge GetInstance(object obj)
		{
			if (obj is PublicKeyAndChallenge)
			{
				return (PublicKeyAndChallenge)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PublicKeyAndChallenge((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in 'PublicKeyAndChallenge' factory : " + obj.GetType().Name + ".");
		}

		public PublicKeyAndChallenge(Asn1Sequence seq)
		{
			pkacSeq = seq;
			spki = SubjectPublicKeyInfo.GetInstance(seq[0]);
			challenge = DerIA5String.GetInstance(seq[1]);
		}

		public override Asn1Object ToAsn1Object()
		{
			return pkacSeq;
		}
	}
}
