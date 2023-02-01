using System;
using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.OpenSsl;
using Tr.Com.Eimza.Org.BouncyCastle.Security.Certificates;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Pkix
{
	internal class PkixCertPath
	{
		internal static readonly IList certPathEncodings;

		private readonly IList certificates;

		public virtual IEnumerable Encodings
		{
			get
			{
				return new EnumerableProxy(certPathEncodings);
			}
		}

		public virtual IList Certificates
		{
			get
			{
				return CollectionUtilities.ReadOnly(certificates);
			}
		}

		static PkixCertPath()
		{
			IList list = Platform.CreateArrayList();
			list.Add("PkiPath");
			list.Add("PEM");
			list.Add("PKCS7");
			certPathEncodings = CollectionUtilities.ReadOnly(list);
		}

		private static IList SortCerts(IList certs)
		{
			if (certs.Count < 2)
			{
				return certs;
			}
			X509Name issuerDN = ((X509Certificate)certs[0]).IssuerDN;
			bool flag = true;
			for (int i = 1; i != certs.Count; i++)
			{
				X509Certificate x509Certificate = (X509Certificate)certs[i];
				if (issuerDN.Equivalent(x509Certificate.SubjectDN, true))
				{
					issuerDN = ((X509Certificate)certs[i]).IssuerDN;
					continue;
				}
				flag = false;
				break;
			}
			if (flag)
			{
				return certs;
			}
			IList list = Platform.CreateArrayList(certs.Count);
			IList result = Platform.CreateArrayList(certs);
			for (int j = 0; j < certs.Count; j++)
			{
				X509Certificate x509Certificate2 = (X509Certificate)certs[j];
				bool flag2 = false;
				X509Name subjectDN = x509Certificate2.SubjectDN;
				foreach (X509Certificate cert in certs)
				{
					if (cert.IssuerDN.Equivalent(subjectDN, true))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					list.Add(x509Certificate2);
					certs.RemoveAt(j);
				}
			}
			if (list.Count > 1)
			{
				return result;
			}
			for (int k = 0; k != list.Count; k++)
			{
				issuerDN = ((X509Certificate)list[k]).IssuerDN;
				for (int l = 0; l < certs.Count; l++)
				{
					X509Certificate x509Certificate3 = (X509Certificate)certs[l];
					if (issuerDN.Equivalent(x509Certificate3.SubjectDN, true))
					{
						list.Add(x509Certificate3);
						certs.RemoveAt(l);
						break;
					}
				}
			}
			if (certs.Count > 0)
			{
				return result;
			}
			return list;
		}

		public PkixCertPath(ICollection certificates)
		{
			this.certificates = SortCerts(Platform.CreateArrayList(certificates));
		}

		public PkixCertPath(Stream inStream)
			: this(inStream, "PkiPath")
		{
		}

		public PkixCertPath(Stream inStream, string encoding)
		{
			string text = encoding.ToUpper();
			IList list;
			try
			{
				if (text.Equals("PkiPath".ToUpper()))
				{
					Asn1Object asn1Object = new Asn1InputStream(inStream).ReadObject();
					if (!(asn1Object is Asn1Sequence))
					{
						throw new CertificateException("input stream does not contain a ASN1 SEQUENCE while reading PkiPath encoded data to load CertPath");
					}
					list = Platform.CreateArrayList();
					foreach (Asn1Encodable item in (Asn1Sequence)asn1Object)
					{
						Stream cStream = new MemoryStream(item.GetEncoded("DER"), false);
						list.Insert(0, new X509Certificate(cStream));
					}
				}
				else
				{
					if (!text.Equals("PKCS7") && !text.Equals("PEM"))
					{
						throw new CertificateException("unsupported encoding: " + encoding);
					}
					list = Platform.CreateArrayList(new X509CertificateParser().ReadCertificates(inStream));
				}
			}
			catch (IOException ex)
			{
				throw new CertificateException("IOException throw while decoding CertPath:\n" + ex.ToString());
			}
			certificates = SortCerts(list);
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			PkixCertPath pkixCertPath = obj as PkixCertPath;
			if (pkixCertPath == null)
			{
				return false;
			}
			IList list = Certificates;
			IList list2 = pkixCertPath.Certificates;
			if (list.Count != list2.Count)
			{
				return false;
			}
			IEnumerator enumerator = list.GetEnumerator();
			IEnumerator enumerator2 = list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator2.MoveNext();
				if (!object.Equals(enumerator.Current, enumerator2.Current))
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			return Certificates.GetHashCode();
		}

		public virtual byte[] GetEncoded()
		{
			foreach (object encoding in Encodings)
			{
				if (encoding is string)
				{
					return GetEncoded((string)encoding);
				}
			}
			return null;
		}

		public virtual byte[] GetEncoded(string encoding)
		{
			if (Platform.CompareIgnoreCase(encoding, "PkiPath") == 0)
			{
				Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector();
				for (int num = certificates.Count - 1; num >= 0; num--)
				{
					asn1EncodableVector.Add(ToAsn1Object((X509Certificate)certificates[num]));
				}
				return ToDerEncoded(new DerSequence(asn1EncodableVector));
			}
			if (Platform.CompareIgnoreCase(encoding, "PKCS7") == 0)
			{
				ContentInfo contentInfo = new ContentInfo(PkcsObjectIdentifiers.Data, null);
				Asn1EncodableVector asn1EncodableVector2 = new Asn1EncodableVector();
				for (int i = 0; i != certificates.Count; i++)
				{
					asn1EncodableVector2.Add(ToAsn1Object((X509Certificate)certificates[i]));
				}
				SignedData content = new SignedData(new DerInteger(1), new DerSet(), contentInfo, new DerSet(asn1EncodableVector2), null, new DerSet());
				return ToDerEncoded(new ContentInfo(PkcsObjectIdentifiers.SignedData, content));
			}
			if (Platform.CompareIgnoreCase(encoding, "PEM") == 0)
			{
				MemoryStream memoryStream = new MemoryStream();
				PemWriter pemWriter = new PemWriter(new StreamWriter(memoryStream));
				try
				{
					for (int j = 0; j != certificates.Count; j++)
					{
						pemWriter.WriteObject(certificates[j]);
					}
					pemWriter.Writer.Close();
				}
				catch (Exception)
				{
					throw new CertificateEncodingException("can't encode certificate for PEM encoded path");
				}
				return memoryStream.ToArray();
			}
			throw new CertificateEncodingException("unsupported encoding: " + encoding);
		}

		private Asn1Object ToAsn1Object(X509Certificate cert)
		{
			try
			{
				return Asn1Object.FromByteArray(cert.GetEncoded());
			}
			catch (Exception e)
			{
				throw new CertificateEncodingException("Exception while encoding certificate", e);
			}
		}

		private byte[] ToDerEncoded(Asn1Encodable obj)
		{
			try
			{
				return obj.GetEncoded("DER");
			}
			catch (IOException e)
			{
				throw new CertificateEncodingException("Exception thrown", e);
			}
		}
	}
}
