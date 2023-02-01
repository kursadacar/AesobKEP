using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Esf;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ess;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ocsp;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs;
using Tr.Com.Eimza.Org.BouncyCastle.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Tsp;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Date;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ext
{
    internal class CadesSignature
	{
		private readonly CmsSignedData cmsSignedData;

		private readonly SignerInformation signerInformation;

		public CadesSignature(byte[] data)
			: this(new CmsSignedData(data))
		{
		}

		public CadesSignature(CmsSignedData cms)
		{
			IEnumerator enumerator = cms.GetSignerInfos().GetSigners().GetEnumerator();
			enumerator.MoveNext();
			cmsSignedData = cms;
			signerInformation = (SignerInformation)enumerator.Current;
		}

		public CadesSignature(CmsSignedData cmsSignedData, SignerInformation signerInformation)
		{
			this.cmsSignedData = cmsSignedData;
			this.signerInformation = signerInformation;
		}

		public CadesSignature(CmsSignedData cms, SignerID id)
			: this(cms, cms.GetSignerInfos().GetFirstSigner(id))
		{
		}

		public virtual CadesCertificateSource GetCertificateSource()
		{
			return new CadesCertificateSource(cmsSignedData, signerInformation, false);
		}

		public virtual CadesCertificateSource GetExtendedCertificateSource()
		{
			return new CadesCertificateSource(cmsSignedData, signerInformation, true);
		}

		public virtual CadesRevocationSource GetRevocationSource()
		{
			return new CadesRevocationSource(cmsSignedData, signerInformation);
		}

		public virtual CadesCrlSource GetCRLSource()
		{
			return new CadesCrlSource(cmsSignedData, signerInformation);
		}

		public virtual CadesOcspSource GetOCSPSource()
		{
			return new CadesOcspSource(cmsSignedData, signerInformation);
		}

		public virtual X509Certificate GetSigningCertificate()
		{
			foreach (X509Certificate item in (IEnumerable<X509Certificate>)GetCertificates())
			{
				if (signerInformation.SignerID.Match(item))
				{
					return item;
				}
				if (signerInformation.SignerID.Issuer.Equivalent(item.IssuerDN) && signerInformation.SignerID.SerialNumber.Equals(item.SerialNumber))
				{
					return item;
				}
			}
			return null;
		}

		public virtual List<X509Certificate> GetCertificates()
		{
			return GetCertificateSource().GetCertificates();
		}

		public virtual PolicyValue GetPolicyId()
		{
			if (signerInformation.SignedAttributes == null)
			{
				return null;
			}
			Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute attribute = signerInformation.SignedAttributes[PkcsObjectIdentifiers.IdAAEtsSigPolicyID];
			if (attribute == null)
			{
				return null;
			}
			if (attribute.AttrValues[0] is DerNull)
			{
				return new PolicyValue();
			}
			SignaturePolicyId signaturePolicyId = null;
			signaturePolicyId = SignaturePolicyId.GetInstance(attribute.AttrValues[0]);
			if (signaturePolicyId == null)
			{
				return new PolicyValue();
			}
			return new PolicyValue(signaturePolicyId.SigPolicyIdentifier.Id);
		}

		public virtual DateTimeObject GetSigningTime()
		{
			if (signerInformation.SignedAttributes != null && signerInformation.SignedAttributes[PkcsObjectIdentifiers.Pkcs9AtSigningTime] != null)
			{
				Asn1Set attrValues = signerInformation.SignedAttributes[PkcsObjectIdentifiers.Pkcs9AtSigningTime].AttrValues;
				try
				{
					object obj = attrValues[0];
					if (obj is DerUtcTime)
					{
						return new DateTimeObject(((DerUtcTime)obj).ToDateTime());
					}
					if (obj is Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509.Time)
					{
						return new DateTimeObject(((Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509.Time)obj).ToDateTime());
					}
				}
				catch (Exception)
				{
					return null;
				}
			}
			return null;
		}

		public virtual CmsSignedData GetCmsSignedData()
		{
			return cmsSignedData;
		}

		public virtual string GetLocation()
		{
			return null;
		}

		public virtual string[] GetClaimedSignerRoles()
		{
			if (signerInformation.SignedAttributes == null)
			{
				return null;
			}
			Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute attribute = signerInformation.SignedAttributes[PkcsObjectIdentifiers.IdAAEtsSignerAttr];
			if (attribute == null)
			{
				return null;
			}
			SignerAttribute instance = SignerAttribute.GetInstance(attribute.AttrValues[0]);
			if (instance == null)
			{
				return null;
			}
			string[] array = new string[instance.ClaimedAttributes.Count];
			for (int i = 0; i < instance.ClaimedAttributes.Count; i++)
			{
				if (instance.ClaimedAttributes[i] is DerOctetString)
				{
					array[i] = Encoding.UTF8.GetString(((DerOctetString)instance.ClaimedAttributes[i]).GetOctets());
				}
				else
				{
					array[i] = instance.ClaimedAttributes[i].ToString();
				}
			}
			return array;
		}

		public virtual string GetContentHint()
		{
			if (signerInformation.SignedAttributes == null)
			{
				return null;
			}
			Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute attribute = signerInformation.SignedAttributes[PkcsObjectIdentifiers.IdAAContentHint];
			if (attribute == null)
			{
				return null;
			}
			ContentHints instance = ContentHints.GetInstance(attribute.AttrValues[0]);
			if (instance != null && instance.ContentDescription.GetString() != null)
			{
				return instance.ContentDescription.GetString();
			}
			return null;
		}

		private List<TimestampToken> GetTimestampList(DerObjectIdentifier attrType, TimestampToken.TimestampType timestampType)
		{
			if (signerInformation.UnsignedAttributes != null)
			{
				Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute attribute = signerInformation.UnsignedAttributes[attrType];
				if (attribute == null)
				{
					return null;
				}
				List<TimestampToken> list = new List<TimestampToken>();
				Asn1Encodable[] array = attribute.AttrValues.ToArray();
				foreach (Asn1Encodable asn1Encodable in array)
				{
					try
					{
						TimeStampToken timeStamp = new TimeStampToken(new CmsSignedData(asn1Encodable.GetDerEncoded()));
						list.Add(new TimestampToken(timeStamp, timestampType));
					}
					catch (Exception e)
					{
						throw new CmsException("Parsing error", e);
					}
				}
				return list;
			}
			return null;
		}

		private List<TimestampToken> GetTimestampListFromSignedAttr(DerObjectIdentifier attrType, TimestampToken.TimestampType timestampType)
		{
			if (signerInformation.SignedAttributes != null)
			{
				Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute attribute = signerInformation.SignedAttributes[attrType];
				if (attribute == null)
				{
					return null;
				}
				List<TimestampToken> list = new List<TimestampToken>();
				Asn1Encodable[] array = attribute.AttrValues.ToArray();
				foreach (Asn1Encodable asn1Encodable in array)
				{
					try
					{
						TimeStampToken timeStamp = new TimeStampToken(new CmsSignedData(asn1Encodable.GetDerEncoded()));
						list.Add(new TimestampToken(timeStamp, timestampType));
					}
					catch (Exception e)
					{
						throw new CmsException("Parsing error", e);
					}
				}
				return list;
			}
			return null;
		}

		public virtual List<TimestampToken> GetContentTimestamps()
		{
			return GetTimestampListFromSignedAttr(PkcsObjectIdentifiers.IdAAEtsContentTimestamp, TimestampToken.TimestampType.CONTENT_TIMESTAMP);
		}

		public virtual List<TimestampToken> GetSignatureTimestamps()
		{
			return GetTimestampList(PkcsObjectIdentifiers.IdAASignatureTimeStampToken, TimestampToken.TimestampType.SIGNATURE_TIMESTAMP);
		}

		public virtual List<TimestampToken> GetTimestampsX1()
		{
			return GetTimestampList(PkcsObjectIdentifiers.IdAAEtsEscTimeStamp, TimestampToken.TimestampType.VALIDATION_DATA_TIMESTAMP);
		}

		public virtual List<TimestampToken> GetTimestampsX2()
		{
			return GetTimestampList(PkcsObjectIdentifiers.IdAAEtsCertCrlTimestamp, TimestampToken.TimestampType.VALIDATION_DATA_REFSONLY_TIMESTAMP);
		}

		public virtual List<TimestampToken> GetArchiveTimestamps()
		{
			return GetTimestampList(PkcsObjectIdentifiers.IdAAEtsArchiveTimestamp, TimestampToken.TimestampType.ARCHIVE_TIMESTAMP);
		}

		public virtual List<TimestampToken> GetArchiveTimestampsV2()
		{
			return GetTimestampList(PkcsObjectIdentifiers.IdAAEtsArchiveTimestampV2, TimestampToken.TimestampType.ARCHIVE_TIMESTAMP);
		}

		public virtual string GetSignatureAlgorithm()
		{
			return signerInformation.EncryptionAlgOid;
		}

		public virtual string GetContentType()
		{
			return signerInformation.ContentType.ToString();
		}

		public virtual SignerInformation GetSignerInformation()
		{
			return signerInformation;
		}

		public virtual List<CadesSignature> GetCounterSignatures()
		{
			List<CadesSignature> list = new List<CadesSignature>();
			foreach (SignerInformation signer in this.signerInformation.GetCounterSignatures().GetSigners())
			{
				CadesSignature item = new CadesSignature(cmsSignedData, signer.SignerID);
				list.Add(item);
			}
			return list;
		}

		public virtual List<CertificateRef> GetCertificateRefs()
		{
			List<CertificateRef> list = new List<CertificateRef>();
			if (signerInformation.UnsignedAttributes != null)
			{
				Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute attribute = signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsCertificateRefs];
				if (attribute != null && attribute.AttrValues.Count > 0)
				{
					DerSequence derSequence = (DerSequence)attribute.AttrValues[0];
					for (int i = 0; i < derSequence.Count; i++)
					{
						CertificateRef item = new CertificateRef(Tr.Com.Eimza.Org.BouncyCastle.Asn1.Esf.OtherCertID.GetInstance(derSequence[i]));
						list.Add(item);
					}
				}
			}
			return list;
		}

		public virtual List<CrlOcspRef> GetCrlOcspRefs()
		{
			List<CrlOcspRef> list = new List<CrlOcspRef>();
			if (signerInformation.UnsignedAttributes != null)
			{
				Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute attribute = signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationRefs];
				if (attribute != null && attribute.AttrValues.Count > 0)
				{
					DerSequence derSequence = (DerSequence)attribute.AttrValues[0];
					for (int i = 0; i < derSequence.Count; i++)
					{
						CrlOcspRef instance = CrlOcspRef.GetInstance(derSequence[i]);
						list.Add(instance);
					}
				}
			}
			return list;
		}

		public virtual List<CrlRef> GetCRLRefs()
		{
			List<CrlRef> list = new List<CrlRef>();
			if (signerInformation.UnsignedAttributes != null)
			{
				Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute attribute = signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationRefs];
				if (attribute != null && attribute.AttrValues.Count > 0)
				{
					DerSequence derSequence = (DerSequence)attribute.AttrValues[0];
					for (int i = 0; i < derSequence.Count; i++)
					{
						CrlOcspRef instance = CrlOcspRef.GetInstance(derSequence[i]);
						if (instance.CrlIDs != null)
						{
							CrlValidatedID[] crls = instance.CrlIDs.GetCrls();
							foreach (CrlValidatedID cmsRef in crls)
							{
								list.Add(new CrlRef(cmsRef));
							}
						}
					}
				}
			}
			return list;
		}

		public virtual List<OcspRef> GetOCSPRefs()
		{
			List<OcspRef> list = new List<OcspRef>();
			if (signerInformation.UnsignedAttributes != null)
			{
				Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute attribute = signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationRefs];
				if (attribute != null && attribute.AttrValues.Count > 0)
				{
					DerSequence derSequence = (DerSequence)attribute.AttrValues[0];
					for (int i = 0; i < derSequence.Count; i++)
					{
						CrlOcspRef instance = CrlOcspRef.GetInstance(derSequence[i]);
						if (instance.OcspIDs != null)
						{
							OcspResponsesID[] ocspResponses = instance.OcspIDs.GetOcspResponses();
							foreach (OcspResponsesID ocsp in ocspResponses)
							{
								list.Add(new OcspRef(ocsp, true));
							}
						}
					}
				}
			}
			return list;
		}

		public virtual List<X509Crl> GetCRLs()
		{
			return GetCRLSource().GetCRLsFromSignature();
		}

		public virtual List<OcspResponse> GetOCSPs()
		{
			return GetOCSPSource().GetOCSPResponsesFromSignature();
		}

		public virtual List<RevocationValues> GetRevocationValues()
		{
			return GetRevocationSource().GetRevocationValues();
		}

		public virtual byte[] GetSignatureTimestampData()
		{
			return signerInformation.GetSignature();
		}

		public virtual byte[] GetTimestampX1Data()
		{
			try
			{
				MemoryStream memoryStream = new MemoryStream();
				BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
				binaryWriter.Write(signerInformation.GetSignature());
				if (signerInformation.UnsignedAttributes != null)
				{
					binaryWriter.Write(signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAASignatureTimeStampToken].AttrType.GetDerEncoded());
					binaryWriter.Write(signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAASignatureTimeStampToken].AttrValues.GetDerEncoded());
				}
				binaryWriter.Write(GetTimestampX2Data());
				return memoryStream.ToArray();
			}
			catch (IOException e)
			{
				throw new CmsException(e);
			}
		}

		public virtual byte[] GetTimestampX2Data()
		{
			try
			{
				MemoryStream memoryStream = new MemoryStream();
				BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
				if (signerInformation.UnsignedAttributes != null)
				{
					binaryWriter.Write(signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsCertificateRefs].AttrType.GetDerEncoded());
					binaryWriter.Write(signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsCertificateRefs].AttrValues.GetDerEncoded());
					binaryWriter.Write(signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationRefs].AttrType.GetDerEncoded());
					binaryWriter.Write(signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationRefs].AttrValues.GetDerEncoded());
				}
				return memoryStream.ToArray();
			}
			catch (IOException e)
			{
				throw new CmsException(e);
			}
		}
	}
}
