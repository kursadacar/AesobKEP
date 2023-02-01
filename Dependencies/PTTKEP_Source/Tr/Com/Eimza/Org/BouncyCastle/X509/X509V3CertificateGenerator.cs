using System;
using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Security.Certificates;
using Tr.Com.Eimza.Org.BouncyCastle.X509.Extension;

namespace Tr.Com.Eimza.Org.BouncyCastle.X509
{
	internal class X509V3CertificateGenerator
	{
		private readonly X509ExtensionsGenerator extGenerator = new X509ExtensionsGenerator();

		private V3TbsCertificateGenerator tbsGen;

		private DerObjectIdentifier sigOid;

		private AlgorithmIdentifier sigAlgId;

		private string signatureAlgorithm;

		public IEnumerable SignatureAlgNames
		{
			get
			{
				return X509Utilities.GetAlgNames();
			}
		}

		public X509V3CertificateGenerator()
		{
			tbsGen = new V3TbsCertificateGenerator();
		}

		public void Reset()
		{
			tbsGen = new V3TbsCertificateGenerator();
			extGenerator.Reset();
		}

		public void SetSerialNumber(BigInteger serialNumber)
		{
			if (serialNumber.SignValue <= 0)
			{
				throw new ArgumentException("serial number must be a positive integer", "serialNumber");
			}
			tbsGen.SetSerialNumber(new DerInteger(serialNumber));
		}

		public void SetIssuerDN(X509Name issuer)
		{
			tbsGen.SetIssuer(issuer);
		}

		public void SetNotBefore(DateTime date)
		{
			tbsGen.SetStartDate(new Time(date));
		}

		public void SetNotAfter(DateTime date)
		{
			tbsGen.SetEndDate(new Time(date));
		}

		public void SetSubjectDN(X509Name subject)
		{
			tbsGen.SetSubject(subject);
		}

		public void SetPublicKey(AsymmetricKeyParameter publicKey)
		{
			tbsGen.SetSubjectPublicKeyInfo(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey));
		}

		public void SetSignatureAlgorithm(string signatureAlgorithm)
		{
			this.signatureAlgorithm = signatureAlgorithm;
			try
			{
				sigOid = X509Utilities.GetAlgorithmOid(signatureAlgorithm);
			}
			catch (Exception)
			{
				throw new ArgumentException("Unknown signature type requested: " + signatureAlgorithm);
			}
			sigAlgId = X509Utilities.GetSigAlgID(sigOid, signatureAlgorithm);
			tbsGen.SetSignature(sigAlgId);
		}

		public void SetSubjectUniqueID(bool[] uniqueID)
		{
			tbsGen.SetSubjectUniqueID(booleanToBitString(uniqueID));
		}

		public void SetIssuerUniqueID(bool[] uniqueID)
		{
			tbsGen.SetIssuerUniqueID(booleanToBitString(uniqueID));
		}

		private DerBitString booleanToBitString(bool[] id)
		{
			byte[] array = new byte[(id.Length + 7) / 8];
			for (int i = 0; i != id.Length; i++)
			{
				if (id[i])
				{
					array[i / 8] |= (byte)(1 << 7 - i % 8);
				}
			}
			int num = id.Length % 8;
			if (num == 0)
			{
				return new DerBitString(array);
			}
			return new DerBitString(array, 8 - num);
		}

		public void AddExtension(string oid, bool critical, Asn1Encodable extensionValue)
		{
			extGenerator.AddExtension(new DerObjectIdentifier(oid), critical, extensionValue);
		}

		public void AddExtension(DerObjectIdentifier oid, bool critical, Asn1Encodable extensionValue)
		{
			extGenerator.AddExtension(oid, critical, extensionValue);
		}

		public void AddExtension(string oid, bool critical, byte[] extensionValue)
		{
			extGenerator.AddExtension(new DerObjectIdentifier(oid), critical, new DerOctetString(extensionValue));
		}

		public void AddExtension(DerObjectIdentifier oid, bool critical, byte[] extensionValue)
		{
			extGenerator.AddExtension(oid, critical, new DerOctetString(extensionValue));
		}

		public void CopyAndAddExtension(string oid, bool critical, X509Certificate cert)
		{
			CopyAndAddExtension(new DerObjectIdentifier(oid), critical, cert);
		}

		public void CopyAndAddExtension(DerObjectIdentifier oid, bool critical, X509Certificate cert)
		{
			Asn1OctetString extensionValue = cert.GetExtensionValue(oid);
			if (extensionValue == null)
			{
				throw new CertificateParsingException("extension " + ((oid != null) ? oid.ToString() : null) + " not present");
			}
			try
			{
				Asn1Encodable extensionValue2 = X509ExtensionUtilities.FromExtensionValue(extensionValue);
				AddExtension(oid, critical, extensionValue2);
			}
			catch (Exception ex)
			{
				throw new CertificateParsingException(ex.Message, ex);
			}
		}

		public X509Certificate Generate(AsymmetricKeyParameter privateKey)
		{
			return Generate(privateKey, null);
		}

		public X509Certificate Generate(AsymmetricKeyParameter privateKey, SecureRandom random)
		{
			TbsCertificateStructure tbsCertificateStructure = GenerateTbsCert();
			byte[] signatureForObject;
			try
			{
				signatureForObject = X509Utilities.GetSignatureForObject(sigOid, signatureAlgorithm, privateKey, random, tbsCertificateStructure);
			}
			catch (Exception e)
			{
				throw new CertificateEncodingException("exception encoding TBS cert", e);
			}
			try
			{
				return GenerateJcaObject(tbsCertificateStructure, signatureForObject);
			}
			catch (CertificateParsingException e2)
			{
				throw new CertificateEncodingException("exception producing certificate object", e2);
			}
		}

		private TbsCertificateStructure GenerateTbsCert()
		{
			if (!extGenerator.IsEmpty)
			{
				tbsGen.SetExtensions(extGenerator.Generate());
			}
			return tbsGen.GenerateTbsCertificate();
		}

		private X509Certificate GenerateJcaObject(TbsCertificateStructure tbsCert, byte[] signature)
		{
			return new X509Certificate(new X509CertificateStructure(tbsCert, sigAlgId, new DerBitString(signature)));
		}
	}
}
