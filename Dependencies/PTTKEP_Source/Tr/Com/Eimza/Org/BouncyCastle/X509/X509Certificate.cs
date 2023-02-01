using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Misc;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509.Qualified;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.OID;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Security.Certificates;
using Tr.Com.Eimza.Org.BouncyCastle.Tools;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Encoders;
using Tr.Com.Eimza.Org.BouncyCastle.X509.Extension;

namespace Tr.Com.Eimza.Org.BouncyCastle.X509
{
	internal class X509Certificate : X509ExtensionBase
	{
		private readonly X509CertificateStructure c;

		private readonly BasicConstraints basicConstraints;

		private readonly bool[] keyUsage;

		private static readonly X509CertificateParser parser = new X509CertificateParser();

		private bool hashValueSet;

		private int hashValue;

		public virtual X509CertificateStructure CertificateStructure
		{
			get
			{
				return c;
			}
		}

		public virtual bool IsValidNow
		{
			get
			{
				return IsValid(DateTime.UtcNow);
			}
		}

		public virtual int Version
		{
			get
			{
				return c.Version;
			}
		}

		public virtual BigInteger SerialNumber
		{
			get
			{
				return c.SerialNumber.Value;
			}
		}

		public SubjectPublicKeyInfo PublicKey
		{
			get
			{
				return c.SubjectPublicKeyInfo;
			}
		}

		public virtual SubjectKeyIdentifier SubjectKeyIdentifier
		{
			get
			{
				try
				{
					X509Extension extension = GetExtension(X509Extensions.SubjectKeyIdentifier);
					if (extension != null && extension.Value != null)
					{
						return SubjectKeyIdentifier.GetInstance(Asn1Object.FromByteArray(extension.Value.GetOctets()));
					}
					return null;
				}
				catch (Exception)
				{
					return null;
				}
			}
		}

		public virtual AuthorityKeyIdentifier AuthorityKeyIdentifier
		{
			get
			{
				try
				{
					X509Extension extension = GetExtension(X509Extensions.AuthorityKeyIdentifier);
					if (extension != null && extension.Value != null)
					{
						return AuthorityKeyIdentifier.GetInstance(Asn1Object.FromByteArray(extension.Value.GetOctets()));
					}
					return null;
				}
				catch (Exception)
				{
					return null;
				}
			}
		}

		public bool IsSelfIssued
		{
			get
			{
				return c.Subject.Equivalent(c.Issuer);
			}
		}

		public bool IsSelfSigned
		{
			get
			{
				try
				{
					AsymmetricKeyParameter publicKey = GetPublicKey();
					Verify(publicKey);
					return true;
				}
				catch (SignatureException)
				{
					return false;
				}
				catch (InvalidKeyException)
				{
					return false;
				}
			}
		}

		public bool IsMaliMuhurCertificate
		{
			get
			{
				Asn1Sequence x509Extension = GetX509Extension(this, X509Extensions.CertificatePolicies);
				if (x509Extension == null)
				{
					return false;
				}
				if (PolicyInformation.GetInstance(x509Extension[0]).PolicyIdentifier.Id != QCStatementsID.KAMUSM_MALI_MUHUR_ILKE.Id)
				{
					return false;
				}
				ArrayList arrayList = (ArrayList)GetExtendedKeyUsage();
				if (arrayList == null)
				{
					return false;
				}
				if (!arrayList.Contains(QCStatementsID.MALI_MUHUR_EXT_KEY_USAGE.Id))
				{
					return false;
				}
				return true;
			}
		}

		public bool IsTimeStampCertificate
		{
			get
			{
				IList extendedKeyUsage = GetExtendedKeyUsage();
				if (extendedKeyUsage == null)
				{
					return false;
				}
				return extendedKeyUsage.Contains(QCStatementsID.TIMESTAMP.Id);
			}
		}

		public bool IsOcspCertificate
		{
			get
			{
				IList extendedKeyUsage = GetExtendedKeyUsage();
				if (extendedKeyUsage == null)
				{
					return false;
				}
				return extendedKeyUsage.Contains(QCStatementsID.OCSP_SIGNING.Id);
			}
		}

		public bool IsCACertificate
		{
			get
			{
				BasicConstraints basicConstraints = this.basicConstraints;
				if (basicConstraints == null)
				{
					return false;
				}
				return basicConstraints.IsCA();
			}
		}

		public bool IsQualifiedCertificate
		{
			get
			{
				Asn1Sequence x509Extension = GetX509Extension(this, X509Extensions.QCStatements);
				if (x509Extension != null)
				{
					bool flag = false;
					for (int i = 0; i < x509Extension.Count; i++)
					{
						try
						{
							QCStatement instance = QCStatement.GetInstance(x509Extension[i]);
							if (instance.StatementId.Id == QCStatementsID.BTK_NITELIKLI.Id || instance.StatementId.Id == QCStatementsID.ETSI_NITELIKLI.Id)
							{
								flag = true;
							}
						}
						catch (Exception)
						{
						}
					}
					if (flag)
					{
						return true;
					}
					return false;
				}
				return false;
			}
		}

		public string EshsName
		{
			get
			{
				Asn1Sequence x509Extension = CertificateUtil.GetX509Extension(this, X509Extensions.CertificatePolicies);
				if (x509Extension == null || x509Extension.Count == 0)
				{
					return GetEshsName();
				}
				string text = null;
				for (int i = 0; i < x509Extension.Count; i++)
				{
					PolicyInformation instance = PolicyInformation.GetInstance(x509Extension[i]);
					if (instance.PolicyIdentifier.Id.Equals(QCStatementsID.NES_ILKE_KAMUSM.Id))
					{
						text = "Tübitak";
						break;
					}
					if (instance.PolicyIdentifier.Id.Equals(QCStatementsID.NES_ILKE_KAMUSM_TEST.Id))
					{
						text = "Tübitak Test";
						break;
					}
					if (instance.PolicyIdentifier.Id.Equals(QCStatementsID.KAMUSM_MALI_MUHUR_ILKE.Id))
					{
						text = "Tübitak Mali Mühür";
						break;
					}
					if (instance.PolicyIdentifier.Id.Equals(QCStatementsID.NES_ILKE_KAMUSM_MOBIL.Id))
					{
						text = "Tübitak Mobil";
						break;
					}
					if (instance.PolicyIdentifier.Id.Equals(QCStatementsID.NES_ILKE_EGUVEN.Id))
					{
						text = "e-Güven";
						break;
					}
					if (instance.PolicyIdentifier.Id.Equals(QCStatementsID.NES_ILKE_EGUVEN_MOBIL.Id))
					{
						text = "e-Güven Mobil";
						break;
					}
					if (instance.PolicyIdentifier.Id.Equals(QCStatementsID.NES_ILKE_EIMZATR_1.Id) || instance.PolicyIdentifier.Id.Equals(QCStatementsID.NES_ILKE_EIMZATR_2.Id))
					{
						text = "e-imzaTR";
						break;
					}
					if (instance.PolicyIdentifier.Id.Equals(QCStatementsID.NES_ILKE_ETUGRA.Id))
					{
						text = "e-Tuğra";
						break;
					}
					if (instance.PolicyIdentifier.Id.Equals(QCStatementsID.NES_ILKE_TURKTRUST.Id))
					{
						text = "Türktrust";
						break;
					}
				}
				return text ?? GetEshsName();
			}
		}

		public virtual string SerialNumberHex
		{
			get
			{
				return BitConverter.ToString(c.SerialNumber.Value.ToByteArray()).Replace("-", string.Empty);
			}
		}

		public virtual X509Name IssuerDN
		{
			get
			{
				return c.Issuer;
			}
		}

		public virtual X509Name SubjectDN
		{
			get
			{
				return c.Subject;
			}
		}

		public virtual DateTime NotBefore
		{
			get
			{
				return c.StartDate.ToDateTime();
			}
		}

		public virtual DateTime NotAfter
		{
			get
			{
				return c.EndDate.ToDateTime();
			}
		}

		public virtual string SigAlgName
		{
			get
			{
				return SignerUtilities.GetEncodingName(c.SignatureAlgorithm.ObjectID);
			}
		}

		public virtual string SigAlgOid
		{
			get
			{
				return c.SignatureAlgorithm.ObjectID.Id;
			}
		}

		public virtual DerBitString IssuerUniqueID
		{
			get
			{
				return c.TbsCertificate.IssuerUniqueID;
			}
		}

		public virtual DerBitString SubjectUniqueID
		{
			get
			{
				return c.TbsCertificate.SubjectUniqueID;
			}
		}

		public virtual KeyUsage KeyUsage
		{
			get
			{
				try
				{
					X509Extension extension = GetExtension(X509Extensions.KeyUsage);
					if (extension != null && extension.Value != null)
					{
						return KeyUsage.GetInstance(Asn1Object.FromByteArray(extension.Value.GetOctets()));
					}
				}
				catch (Exception)
				{
					return null;
				}
				return null;
			}
		}

		public BasicConstraints BasicConstraints
		{
			get
			{
				return basicConstraints;
			}
		}

		public virtual X509Name CrlIssuerName
		{
			get
			{
				try
				{
					X509Extension extension = GetExtension(X509Extensions.CrlDistributionPoints);
					if (extension == null || extension.Value == null)
					{
						return IssuerDN;
					}
					CrlDistPoint instance = CrlDistPoint.GetInstance(Asn1Object.FromByteArray(extension.Value.GetOctets()));
					if (instance != null)
					{
						DistributionPoint[] distributionPoints = instance.GetDistributionPoints();
						foreach (DistributionPoint distributionPoint in distributionPoints)
						{
							if (distributionPoint.CrlIssuer != null)
							{
								GeneralName generalName = distributionPoint.CrlIssuer.GetNames()[0];
								if (generalName != null && generalName.Name is X509Name)
								{
									return X509Name.GetInstance(generalName.Name);
								}
							}
						}
					}
				}
				catch (Exception)
				{
					return IssuerDN;
				}
				return IssuerDN;
			}
		}

		public CrlDistPoint CrlDistributionPoints
		{
			get
			{
				X509Extension extension = GetExtension(X509Extensions.CrlDistributionPoints);
				if (extension != null && extension.Value != null)
				{
					return CrlDistPoint.GetInstance(Asn1Object.FromByteArray(extension.Value.GetOctets()));
				}
				return null;
			}
		}

		public bool HasIndirectCrl
		{
			get
			{
				if (CrlIssuerName != null)
				{
					return !CrlIssuerName.Equivalent(IssuerDN);
				}
				return false;
			}
		}

		public X509Certificate(byte[] cBytes)
			: this(parser.ReadCertificate(cBytes).CertificateStructure)
		{
		}

		public X509Certificate(Stream cStream)
			: this(parser.ReadCertificate(cStream).CertificateStructure)
		{
		}

		public X509Certificate(X509CertificateStructure c)
		{
			this.c = c;
			try
			{
				Asn1OctetString extensionValue = GetExtensionValue(new DerObjectIdentifier("2.5.29.19"));
				if (extensionValue != null)
				{
					basicConstraints = BasicConstraints.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue));
				}
			}
			catch (Exception ex)
			{
				throw new CertificateParsingException("cannot construct BasicConstraints: " + ((ex != null) ? ex.ToString() : null));
			}
			try
			{
				Asn1OctetString extensionValue2 = GetExtensionValue(new DerObjectIdentifier("2.5.29.15"));
				if (extensionValue2 != null)
				{
					DerBitString instance = DerBitString.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue2));
					byte[] bytes = instance.GetBytes();
					int num = bytes.Length * 8 - instance.PadBits;
					keyUsage = new bool[(num < 9) ? 9 : num];
					for (int i = 0; i != num; i++)
					{
						keyUsage[i] = (bytes[i / 8] & (128 >> i % 8)) != 0;
					}
				}
				else
				{
					keyUsage = null;
				}
			}
			catch (Exception ex2)
			{
				throw new CertificateParsingException("cannot construct KeyUsage: " + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		public virtual bool IsValid(DateTime time)
		{
			if (time.CompareTo(NotBefore) >= 0)
			{
				return time.CompareTo(NotAfter) <= 0;
			}
			return false;
		}

		public virtual void CheckValidity()
		{
			CheckValidity(DateTime.UtcNow);
		}

		public virtual void CheckValidity(DateTime time)
		{
			if (time.CompareTo(NotAfter) > 0)
			{
				throw new CertificateExpiredException("certificate expired on " + c.EndDate.GetTime());
			}
			if (time.CompareTo(NotBefore) < 0)
			{
				throw new CertificateNotYetValidException("certificate not valid until " + c.StartDate.GetTime());
			}
		}

		public string GetEshsName()
		{
			string stringValue = IssuerDN.StringValue;
			string text = null;
			if (stringValue.Contains("TÜRKTRUST") || stringValue.Contains("TURKTRUST"))
			{
				return "Türktrust";
			}
			if (stringValue.Contains("e-Guven") || stringValue.Contains("E-GUVEN") || stringValue.Contains("TNB"))
			{
				return "e-Güven";
			}
			if (stringValue.Contains("EBG") || stringValue.Contains("E-Tugra") || stringValue.Contains("E-Tuğra"))
			{
				return "e-Tuğra";
			}
			if (stringValue.Contains("TÜBİTAK") || stringValue.Contains("UEKAE"))
			{
				return "Tübitak";
			}
			if (stringValue.Contains("e-imzaTR"))
			{
				return "e-imzaTR";
			}
			return "ESHS";
		}

		public Asn1Sequence GetX509Extension(byte[] Obj)
		{
			return (Asn1Sequence)new Asn1InputStream(Obj).ReadObject();
		}

		private Asn1Sequence GetX509Extension(X509Certificate SignerCert, DerObjectIdentifier ID)
		{
			try
			{
				return (Asn1Sequence)new Asn1InputStream(SignerCert.GetExtensionValue(ID).GetOctets()).ReadObject();
			}
			catch (Exception)
			{
				return null;
			}
		}

		public virtual byte[] GetTbsCertificate()
		{
			return c.TbsCertificate.GetDerEncoded();
		}

		public virtual byte[] GetSignature()
		{
			return c.Signature.GetBytes();
		}

		public virtual byte[] GetSigAlgParams()
		{
			if (c.SignatureAlgorithm.Parameters != null)
			{
				return c.SignatureAlgorithm.Parameters.GetDerEncoded();
			}
			return null;
		}

		public virtual bool[] GetKeyUsage()
		{
			if (keyUsage != null)
			{
				return (bool[])keyUsage.Clone();
			}
			return null;
		}

		public virtual IList GetExtendedKeyUsage()
		{
			Asn1OctetString extensionValue = GetExtensionValue(new DerObjectIdentifier("2.5.29.37"));
			if (extensionValue == null)
			{
				return null;
			}
			try
			{
				Asn1Sequence instance = Asn1Sequence.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue));
				IList list = Platform.CreateArrayList();
				foreach (DerObjectIdentifier item in instance)
				{
					list.Add(item.Id);
				}
				return list;
			}
			catch (Exception exception)
			{
				throw new CertificateParsingException("error processing extended key usage extension", exception);
			}
		}

		public virtual int GetBasicConstraints()
		{
			if (basicConstraints != null && basicConstraints.IsCA())
			{
				if (basicConstraints.PathLenConstraint == null)
				{
					return int.MaxValue;
				}
				return basicConstraints.PathLenConstraint.IntValue;
			}
			return -1;
		}

		public virtual ICollection GetSubjectAlternativeNames()
		{
			return GetAlternativeNames("2.5.29.17");
		}

		public virtual ICollection GetIssuerAlternativeNames()
		{
			return GetAlternativeNames("2.5.29.18");
		}

		protected virtual ICollection GetAlternativeNames(string oid)
		{
			Asn1OctetString extensionValue = GetExtensionValue(new DerObjectIdentifier(oid));
			if (extensionValue == null)
			{
				return null;
			}
			GeneralNames instance = GeneralNames.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue));
			IList list = Platform.CreateArrayList();
			GeneralName[] names = instance.GetNames();
			foreach (GeneralName generalName in names)
			{
				IList list2 = Platform.CreateArrayList();
				list2.Add(generalName.TagNo);
				list2.Add(generalName.Name.ToString());
				list.Add(list2);
			}
			return list;
		}

		public virtual List<GeneralName> GetAlternativeNamesList(string oid)
		{
			Asn1OctetString extensionValue = GetExtensionValue(new DerObjectIdentifier(oid));
			if (extensionValue == null)
			{
				return null;
			}
			GeneralNames instance = GeneralNames.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue));
			List<GeneralName> list = new List<GeneralName>();
			GeneralName[] names = instance.GetNames();
			foreach (GeneralName item in names)
			{
				list.Add(item);
			}
			return list;
		}

		protected override X509Extensions GetX509Extensions()
		{
			if (c.Version != 3)
			{
				return null;
			}
			return c.TbsCertificate.Extensions;
		}

		public virtual AsymmetricKeyParameter GetPublicKey()
		{
			return PublicKeyFactory.CreateKey(c.SubjectPublicKeyInfo);
		}

		public virtual byte[] GetEncoded()
		{
			return c.GetDerEncoded();
		}

		public virtual byte[] GetEncoded(Asn1Encoding asn1Encoding)
		{
			if (asn1Encoding == Asn1Encoding.DER)
			{
				return c.GetEncoded(Asn1Encoding.DER);
			}
			return c.GetEncoded(Asn1Encoding.BER);
		}

		public byte[] GetDigest(string digestAlgorithm)
		{
			return DigestUtilities.CalculateDigest(digestAlgorithm, GetEncoded());
		}

		public byte[] GetDigest(DerObjectIdentifier digestAlgorithm)
		{
			return DigestUtilities.CalculateDigest(digestAlgorithm, GetEncoded());
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			X509Certificate x509Certificate = obj as X509Certificate;
			if (x509Certificate == null)
			{
				return false;
			}
			return c.Equals(x509Certificate.c);
		}

		public override int GetHashCode()
		{
			lock (this)
			{
				if (!hashValueSet)
				{
					hashValue = c.GetHashCode();
					hashValueSet = true;
				}
			}
			return hashValue;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string newLine = Platform.NewLine;
			stringBuilder.Append("  [0]         Version: ").Append(Version).Append(newLine);
			stringBuilder.Append("         SerialNumber: ").Append(SerialNumber).Append(newLine);
			stringBuilder.Append("            SubjectDN: ").Append(SubjectDN).Append(newLine);
			stringBuilder.Append("             IssuerDN: ").Append(IssuerDN).Append(newLine);
			stringBuilder.Append("           Start Date: ").Append(NotBefore).Append(newLine);
			stringBuilder.Append("           Final Date: ").Append(NotAfter).Append(newLine);
			stringBuilder.Append("           Public Key: ").Append(GetPublicKey()).Append(newLine);
			stringBuilder.Append("  Signature Algorithm: ").Append(SigAlgName).Append(newLine);
			byte[] signature = GetSignature();
			stringBuilder.Append("            Signature: ").Append(Hex.ToHexString(signature, 0, 20)).Append(newLine);
			for (int i = 20; i < signature.Length; i += 20)
			{
				int length = System.Math.Min(20, signature.Length - i);
				stringBuilder.Append("                       ").Append(Hex.ToHexString(signature, i, length)).Append(newLine);
			}
			X509Extensions extensions = c.TbsCertificate.Extensions;
			if (extensions != null)
			{
				IEnumerator enumerator = extensions.ExtensionOids.GetEnumerator();
				if (enumerator.MoveNext())
				{
					stringBuilder.Append("       Extensions: \n");
				}
				do
				{
					DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)enumerator.Current;
					X509Extension extension = extensions.GetExtension(derObjectIdentifier);
					if (extension.Value != null)
					{
						Asn1Object asn1Object = Asn1Object.FromByteArray(extension.Value.GetOctets());
						stringBuilder.Append("                       critical(").Append(extension.IsCritical).Append(") ");
						try
						{
							if (derObjectIdentifier.Equals(X509Extensions.BasicConstraints))
							{
								stringBuilder.Append(BasicConstraints.GetInstance(asn1Object));
							}
							else if (derObjectIdentifier.Equals(X509Extensions.KeyUsage))
							{
								stringBuilder.Append(KeyUsage.GetInstance(asn1Object));
							}
							else if (derObjectIdentifier.Equals(MiscObjectIdentifiers.NetscapeCertType))
							{
								stringBuilder.Append(new NetscapeCertType((DerBitString)asn1Object));
							}
							else if (derObjectIdentifier.Equals(MiscObjectIdentifiers.NetscapeRevocationUrl))
							{
								stringBuilder.Append(new NetscapeRevocationUrl((DerIA5String)asn1Object));
							}
							else if (derObjectIdentifier.Equals(MiscObjectIdentifiers.VerisignCzagExtension))
							{
								stringBuilder.Append(new VerisignCzagExtension((DerIA5String)asn1Object));
							}
							else
							{
								stringBuilder.Append(derObjectIdentifier.Id);
								stringBuilder.Append(" value = ").Append(Asn1Dump.DumpAsString(asn1Object));
							}
						}
						catch (Exception)
						{
							stringBuilder.Append(derObjectIdentifier.Id);
							stringBuilder.Append(" value = ").Append("*****");
						}
					}
					stringBuilder.Append(newLine);
				}
				while (enumerator.MoveNext());
			}
			return stringBuilder.ToString();
		}

		public virtual void Verify(AsymmetricKeyParameter key)
		{
			ISigner signer = SignerUtilities.GetSigner(X509SignatureUtilities.GetSignatureName(c.SignatureAlgorithm));
			CheckSignature(key, signer);
		}

		protected virtual void CheckSignature(AsymmetricKeyParameter publicKey, ISigner signature)
		{
			if (!IsAlgIDEqual(c.SignatureAlgorithm, c.TbsCertificate.Signature))
			{
				throw new CertificateException("signature algorithm in TBS cert not same as outer cert");
			}
			Asn1Encodable parameters = c.SignatureAlgorithm.Parameters;
			X509SignatureUtilities.SetSignatureParameters(signature, parameters);
			signature.Init(false, publicKey);
			byte[] tbsCertificate = GetTbsCertificate();
			signature.BlockUpdate(tbsCertificate, 0, tbsCertificate.Length);
			byte[] signature2 = GetSignature();
			if (!signature.VerifySignature(signature2))
			{
				throw new InvalidKeyException("Public key presented not for certificate signature");
			}
		}

		private static bool IsAlgIDEqual(AlgorithmIdentifier id1, AlgorithmIdentifier id2)
		{
			if (!id1.ObjectID.Equals(id2.ObjectID))
			{
				return false;
			}
			Asn1Encodable parameters = id1.Parameters;
			Asn1Encodable parameters2 = id2.Parameters;
			if (parameters == null == (parameters2 == null))
			{
				return object.Equals(parameters, parameters2);
			}
			if (parameters != null)
			{
				return parameters.ToAsn1Object() is Asn1Null;
			}
			return parameters2.ToAsn1Object() is Asn1Null;
		}
	}
}
