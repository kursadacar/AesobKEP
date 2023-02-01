using System;
using System.Collections.Generic;
using System.Linq;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Security.Certificates;
using Tr.Com.Eimza.Org.BouncyCastle.Tsp;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ext
{
	internal class TimestampToken
	{
		public enum TimestampType
		{
			CONTENT_TIMESTAMP,
			INDIVIDUAL_CONTENT_TIMESTAMP,
			SIGNATURE_TIMESTAMP,
			VALIDATION_DATA_REFSONLY_TIMESTAMP,
			VALIDATION_DATA_TIMESTAMP,
			ARCHIVE_TIMESTAMP
		}

		private readonly TimeStampToken timeStamp;

		private readonly TimestampType timeStampType;

		public TimestampToken(TimeStampToken timeStamp)
		{
			this.timeStamp = timeStamp;
		}

		public TimestampToken(TimeStampToken timeStamp, TimestampType type)
		{
			this.timeStamp = timeStamp;
			timeStampType = type;
		}

		public virtual X509Name GetSignerSubjectName()
		{
			foreach (X509Certificate item in (IEnumerable<X509Certificate>)GetWrappedCertificateSource().GetCertificates())
			{
				if (timeStamp.SignerID.Match(item))
				{
					return item.SubjectDN;
				}
			}
			return null;
		}

		public virtual bool IsSignedBy(X509Certificate potentialIssuer)
		{
			try
			{
				timeStamp.Validate(potentialIssuer);
				return true;
			}
			catch (CertificateExpiredException)
			{
				return false;
			}
			catch (CertificateNotYetValidException)
			{
				return false;
			}
			catch (TspValidationException)
			{
				return false;
			}
			catch (TspException)
			{
				return false;
			}
		}

		public virtual CadesCertificateSource GetWrappedCertificateSource()
		{
			return new CadesCertificateSource(timeStamp.ToCmsSignedData());
		}

		public virtual TimestampType GetTimeStampType()
		{
			return timeStampType;
		}

		public virtual TimeStampToken GetTimeStamp()
		{
			return timeStamp;
		}

		public virtual bool MatchData(byte[] data)
		{
			return DigestUtilities.CalculateDigest(timeStamp.TimeStampInfo.HashAlgorithm.ObjectID.Id, data).SequenceEqual(timeStamp.TimeStampInfo.GetMessageImprintDigest());
		}

		public virtual DateTime GetGenTimeDate()
		{
			return timeStamp.TimeStampInfo.GenTime;
		}
	}
}
