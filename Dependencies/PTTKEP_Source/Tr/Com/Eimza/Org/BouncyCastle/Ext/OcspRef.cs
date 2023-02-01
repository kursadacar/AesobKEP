using System;
using System.IO;
using System.Linq;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Esf;
using Tr.Com.Eimza.Org.BouncyCastle.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Ocsp;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ext
{
	internal class OcspRef
	{
		private string algorithm;

		private DateTime ocspProduceTime;

		private byte[] digestValue;

		private readonly bool matchOnlyBasicOCSPResponse;

		public OcspResponsesID OcspResponsesId { get; private set; }

		public OcspRef(OcspResponsesID ocsp, bool matchOnlyBasicOCSPResponse)
			: this(ocsp.OcspRepHash.HashAlgorithm.ObjectID.Id, ocsp.OcspRepHash.GetHashValue(), ocsp.OcspIdentifier.ProducedAt, matchOnlyBasicOCSPResponse)
		{
			OcspResponsesId = ocsp;
		}

		public OcspRef(string algorithm, byte[] digestValue, bool matchOnlyBasicOCSPResponse)
		{
			this.algorithm = algorithm;
			this.digestValue = digestValue;
			this.matchOnlyBasicOCSPResponse = matchOnlyBasicOCSPResponse;
		}

		public OcspRef(string algorithm, byte[] digestValue, DateTime ocspProduceTime, bool matchOnlyBasicOCSPResponse)
		{
			this.algorithm = algorithm;
			this.digestValue = digestValue;
			this.ocspProduceTime = ocspProduceTime;
			this.matchOnlyBasicOCSPResponse = matchOnlyBasicOCSPResponse;
		}

		public virtual bool Match(BasicOcspResp ocspResp)
		{
			try
			{
				IDigest digest = DigestUtilities.GetDigest(algorithm);
				byte[] array = ((!matchOnlyBasicOCSPResponse) ? ocspResp.ToOcspResp().GetEncoded() : ocspResp.GetEncoded());
				digest.BlockUpdate(array, 0, array.Length);
				byte[] second = DigestUtilities.DoFinal(digest);
				return digestValue.SequenceEqual(second);
			}
			catch (NoSuchAlgorithmException e)
			{
				throw new CmsException("Maybe BouncyCastle provider is not installed ?", e);
			}
			catch (IOException e2)
			{
				throw new CmsException(e2);
			}
		}

		public virtual DateTime GetOcspProduceAtTime()
		{
			return ocspProduceTime;
		}

		public virtual void SetOcspProduceAtTime(DateTime ocspProduceTime)
		{
			this.ocspProduceTime = ocspProduceTime;
		}

		public virtual string GetAlgorithm()
		{
			return algorithm;
		}

		public virtual void SetAlgorithm(string algorithm)
		{
			this.algorithm = algorithm;
		}

		public virtual byte[] GetDigestValue()
		{
			return digestValue;
		}

		public virtual void SetDigestValue(byte[] digestValue)
		{
			this.digestValue = digestValue;
		}
	}
}
