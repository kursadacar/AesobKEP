using System;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms
{
	internal class OriginatorPublicKey : Asn1Encodable
	{
		private AlgorithmIdentifier algorithm;

		private DerBitString publicKey;

		public AlgorithmIdentifier Algorithm
		{
			get
			{
				return algorithm;
			}
		}

		public DerBitString PublicKey
		{
			get
			{
				return publicKey;
			}
		}

		public OriginatorPublicKey(AlgorithmIdentifier algorithm, byte[] publicKey)
		{
			this.algorithm = algorithm;
			this.publicKey = new DerBitString(publicKey);
		}

		public OriginatorPublicKey(Asn1Sequence seq)
		{
			algorithm = AlgorithmIdentifier.GetInstance(seq[0]);
			publicKey = (DerBitString)seq[1];
		}

		public static OriginatorPublicKey GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		public static OriginatorPublicKey GetInstance(object obj)
		{
			if (obj == null || obj is OriginatorPublicKey)
			{
				return (OriginatorPublicKey)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OriginatorPublicKey((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid OriginatorPublicKey: " + obj.GetType().Name);
		}

		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(algorithm, publicKey);
		}
	}
}
