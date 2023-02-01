using System;
using System.Linq;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Esf;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Security.Certificates;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ext
{
	internal class CrlRef
	{
		private X509Name crlIssuer;

		private DateTime crlIssuedTime;

		private BigInteger crlNumber;

		private string algorithm;

		private byte[] digestValue;

		public CrlValidatedID CrlValidatedId { get; private set; }

		public CrlRef()
		{
		}

		public CrlRef(CrlValidatedID cmsRef)
		{
			try
			{
				CrlValidatedId = cmsRef;
				crlIssuer = cmsRef.CrlIdentifier.CrlIssuer;
				crlIssuedTime = cmsRef.CrlIdentifier.CrlIssuedTime;
				crlNumber = cmsRef.CrlIdentifier.CrlNumber;
				algorithm = cmsRef.CrlHash.HashAlgorithm.ObjectID.Id;
				digestValue = cmsRef.CrlHash.GetHashValue();
			}
			catch (CmsException e)
			{
				throw new CmsException(e);
			}
		}

		public virtual bool Match(X509Crl crl)
		{
			try
			{
				byte[] second = DigestUtilities.CalculateDigest(algorithm, crl.GetEncoded());
				return digestValue.SequenceEqual(second);
			}
			catch (NoSuchAlgorithmException e)
			{
				throw new CmsException("Maybe BouncyCastle provider is not installed ?", e);
			}
			catch (CrlException e2)
			{
				throw new CmsException(e2);
			}
		}

		public virtual X509Name GetCrlIssuer()
		{
			return crlIssuer;
		}

		public virtual void SetCrlIssuer(X509Name crlIssuer)
		{
			this.crlIssuer = crlIssuer;
		}

		public virtual DateTime GetCrlIssuedTime()
		{
			return crlIssuedTime;
		}

		public virtual void SetCrlIssuedTime(DateTime crlIssuedTime)
		{
			this.crlIssuedTime = crlIssuedTime;
		}

		public virtual BigInteger GetCrlNumber()
		{
			return crlNumber;
		}

		public virtual void SetCrlNumber(BigInteger crlNumber)
		{
			this.crlNumber = crlNumber;
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
