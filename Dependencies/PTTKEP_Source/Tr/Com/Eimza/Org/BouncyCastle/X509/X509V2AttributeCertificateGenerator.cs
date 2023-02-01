using System;
using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Security.Certificates;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.X509
{
	internal class X509V2AttributeCertificateGenerator
	{
		private readonly X509ExtensionsGenerator extGenerator = new X509ExtensionsGenerator();

		private V2AttributeCertificateInfoGenerator acInfoGen;

		private DerObjectIdentifier sigOID;

		private AlgorithmIdentifier sigAlgId;

		private string signatureAlgorithm;

		public IEnumerable SignatureAlgNames
		{
			get
			{
				return X509Utilities.GetAlgNames();
			}
		}

		public X509V2AttributeCertificateGenerator()
		{
			acInfoGen = new V2AttributeCertificateInfoGenerator();
		}

		public void Reset()
		{
			acInfoGen = new V2AttributeCertificateInfoGenerator();
			extGenerator.Reset();
		}

		public void SetHolder(AttributeCertificateHolder holder)
		{
			acInfoGen.SetHolder(holder.holder);
		}

		public void SetIssuer(AttributeCertificateIssuer issuer)
		{
			acInfoGen.SetIssuer(AttCertIssuer.GetInstance(issuer.form));
		}

		public void SetSerialNumber(BigInteger serialNumber)
		{
			acInfoGen.SetSerialNumber(new DerInteger(serialNumber));
		}

		public void SetNotBefore(DateTime date)
		{
			acInfoGen.SetStartDate(new DerGeneralizedTime(date));
		}

		public void SetNotAfter(DateTime date)
		{
			acInfoGen.SetEndDate(new DerGeneralizedTime(date));
		}

		public void SetSignatureAlgorithm(string signatureAlgorithm)
		{
			this.signatureAlgorithm = signatureAlgorithm;
			try
			{
				sigOID = X509Utilities.GetAlgorithmOid(signatureAlgorithm);
			}
			catch (Exception)
			{
				throw new ArgumentException("Unknown signature type requested");
			}
			sigAlgId = X509Utilities.GetSigAlgID(sigOID, signatureAlgorithm);
			acInfoGen.SetSignature(sigAlgId);
		}

		public void AddAttribute(X509Attribute attribute)
		{
			acInfoGen.AddAttribute(AttributeX509.GetInstance(attribute.ToAsn1Object()));
		}

		public void SetIssuerUniqueId(bool[] iui)
		{
			throw Platform.CreateNotImplementedException("SetIssuerUniqueId()");
		}

		public void AddExtension(string oid, bool critical, Asn1Encodable extensionValue)
		{
			extGenerator.AddExtension(new DerObjectIdentifier(oid), critical, extensionValue);
		}

		public void AddExtension(string oid, bool critical, byte[] extensionValue)
		{
			extGenerator.AddExtension(new DerObjectIdentifier(oid), critical, extensionValue);
		}

		public IX509AttributeCertificate Generate(AsymmetricKeyParameter publicKey, SecureRandom random = null)
		{
			if (!extGenerator.IsEmpty)
			{
				acInfoGen.SetExtensions(extGenerator.Generate());
			}
			AttributeCertificateInfo attributeCertificateInfo = acInfoGen.GenerateAttributeCertificateInfo();
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector();
			asn1EncodableVector.Add(attributeCertificateInfo, sigAlgId);
			try
			{
				asn1EncodableVector.Add(new DerBitString(X509Utilities.GetSignatureForObject(sigOID, signatureAlgorithm, publicKey, random, attributeCertificateInfo)));
				return new X509V2AttributeCertificate(AttributeCertificate.GetInstance(new DerSequence(asn1EncodableVector)));
			}
			catch (Exception e)
			{
				throw new CertificateEncodingException("constructed invalid certificate", e);
			}
		}
	}
}
