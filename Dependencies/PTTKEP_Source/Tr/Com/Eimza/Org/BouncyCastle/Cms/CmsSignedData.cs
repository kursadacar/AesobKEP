using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.X509;
using Tr.Com.Eimza.Org.BouncyCastle.X509.Store;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class CmsSignedData
	{
		private static readonly CmsSignedHelper Helper = CmsSignedHelper.Instance;

		private readonly CmsProcessable signedContent;

		private SignedData signedData;

		private ContentInfo contentInfo;

		private SignerInformationStore signerInfoStore;

		private IX509Store attrCertStore;

		private IX509Store certificateStore;

		private IX509Store crlStore;

		private readonly IDictionary hashes;

		public SignedData SignedData
		{
			get
			{
				return signedData;
			}
			set
			{
				signedData = value;
			}
		}

		public int Version
		{
			get
			{
				return signedData.Version.Value.IntValue;
			}
		}

		[Obsolete("Use 'SignedContentType' property instead.")]
		public string SignedContentTypeOid
		{
			get
			{
				return signedData.EncapContentInfo.ContentType.Id;
			}
		}

		public DerObjectIdentifier SignedContentType
		{
			get
			{
				return signedData.EncapContentInfo.ContentType;
			}
		}

		public CmsProcessable SignedContent
		{
			get
			{
				return signedContent;
			}
		}

		public ContentInfo ContentInfo
		{
			get
			{
				return contentInfo;
			}
		}

		private CmsSignedData(CmsSignedData c)
		{
			signedData = c.signedData;
			contentInfo = c.contentInfo;
			signedContent = c.signedContent;
			signerInfoStore = c.signerInfoStore;
		}

		public CmsSignedData(byte[] sigBlock)
			: this(CmsUtilities.ReadContentInfo(new MemoryStream(sigBlock, false)))
		{
		}

		public CmsSignedData(CmsProcessable signedContent, byte[] sigBlock)
			: this(signedContent, CmsUtilities.ReadContentInfo(new MemoryStream(sigBlock, false)))
		{
		}

		public CmsSignedData(IDictionary hashes, byte[] sigBlock)
			: this(hashes, CmsUtilities.ReadContentInfo(sigBlock))
		{
		}

		public CmsSignedData(CmsProcessable signedContent, Stream sigData)
			: this(signedContent, CmsUtilities.ReadContentInfo(sigData))
		{
		}

		public CmsSignedData(Stream sigData)
			: this(CmsUtilities.ReadContentInfo(sigData))
		{
		}

		public CmsSignedData(CmsProcessable signedContent, ContentInfo sigData)
		{
			this.signedContent = signedContent;
			contentInfo = sigData;
			signedData = SignedData.GetInstance(contentInfo.Content);
		}

		public CmsSignedData(IDictionary hashes, ContentInfo sigData)
		{
			this.hashes = hashes;
			contentInfo = sigData;
			signedData = SignedData.GetInstance(contentInfo.Content);
		}

		public CmsSignedData(ContentInfo sigData)
		{
			contentInfo = sigData;
			signedData = SignedData.GetInstance(contentInfo.Content);
			if (signedData.EncapContentInfo.Content != null)
			{
				signedContent = new CmsProcessableByteArray(((Asn1OctetString)signedData.EncapContentInfo.Content).GetOctets());
			}
		}

		public SignerInformationStore GetSignerInfos()
		{
			if (signerInfoStore == null)
			{
				IList list = Platform.CreateArrayList();
				foreach (object signerInfo in signedData.SignerInfos)
				{
					SignerInfo instance = SignerInfo.GetInstance(signerInfo);
					DerObjectIdentifier contentType = signedData.EncapContentInfo.ContentType;
					if (hashes == null)
					{
						list.Add(new SignerInformation(instance, contentType, signedContent, null));
						continue;
					}
					byte[] digest = (byte[])hashes[instance.DigestAlgorithm.ObjectID.Id];
					list.Add(new SignerInformation(instance, contentType, null, new BaseDigestCalculator(digest)));
				}
				signerInfoStore = new SignerInformationStore(list);
			}
			return signerInfoStore;
		}

		public void AddSigner(SignerInformation newSigner)
		{
			if (signedData.SignerInfos != null)
			{
				signedData.SignerInfos.AddObject(newSigner.ToSignerInfo());
			}
		}

		public IX509Store GetAttributeCertificates(string type)
		{
			return attrCertStore ?? (attrCertStore = Helper.CreateAttributeStore(type, signedData.Certificates));
		}

		public IX509Store GetCertificates(string type)
		{
			if (certificateStore == null)
			{
				certificateStore = Helper.CreateCertificateStore(type, signedData.Certificates);
			}
			return certificateStore;
		}

		public List<X509Certificate> GetCertificates()
		{
			List<X509Certificate> list = new List<X509Certificate>();
			foreach (Asn1Encodable certificate in signedData.Certificates)
			{
				try
				{
					Asn1Object asn1Object = certificate.ToAsn1Object();
					if (asn1Object is Asn1Sequence)
					{
						list.Add(new X509Certificate(asn1Object.GetEncoded()));
					}
				}
				catch (Exception e)
				{
					throw new CmsException("can't re-encode certificate!", e);
				}
			}
			return list;
		}

		public IX509Store GetCrls(string type)
		{
			return crlStore ?? (crlStore = Helper.CreateCrlStore(type, signedData.CRLs));
		}

		public List<X509Crl> GetCrls()
		{
			List<X509Crl> list = new List<X509Crl>();
			if (signedData.CRLs != null)
			{
				foreach (Asn1Encodable cRL in signedData.CRLs)
				{
					try
					{
						Asn1Object asn1Object = cRL.ToAsn1Object();
						if (asn1Object is Asn1Sequence)
						{
							list.Add(new X509Crl(asn1Object.GetEncoded()));
						}
					}
					catch (Exception e)
					{
						throw new CmsException("can't re-encode crl!", e);
					}
				}
				return list;
			}
			return list;
		}

		public byte[] GetEncoded()
		{
			return contentInfo.GetEncoded();
		}

		public static CmsSignedData ReplaceSigners(CmsSignedData signedData, SignerInformationStore signerInformationStore)
		{
			CmsSignedData cmsSignedData = new CmsSignedData(signedData);
			cmsSignedData.signerInfoStore = signerInformationStore;
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector();
			Asn1EncodableVector asn1EncodableVector2 = new Asn1EncodableVector();
			foreach (SignerInformation signer in signerInformationStore.GetSigners())
			{
				asn1EncodableVector.Add(Helper.FixAlgID(signer.DigestAlgorithmID));
				asn1EncodableVector2.Add(signer.ToSignerInfo());
			}
			Asn1Set asn1Set = new DerSet(asn1EncodableVector);
			Asn1Set asn1Set2 = new DerSet(asn1EncodableVector2, false);
			Asn1Sequence asn1Sequence = (Asn1Sequence)signedData.signedData.ToAsn1Object();
			asn1EncodableVector2 = new Asn1EncodableVector(asn1Sequence[0], asn1Set);
			for (int i = 2; i != asn1Sequence.Count - 1; i++)
			{
				asn1EncodableVector2.Add(asn1Sequence[i]);
			}
			asn1EncodableVector2.Add(asn1Set2);
			cmsSignedData.signedData = SignedData.GetInstance(new BerSequence(asn1EncodableVector2));
			cmsSignedData.contentInfo = new ContentInfo(cmsSignedData.contentInfo.ContentType, cmsSignedData.signedData);
			return cmsSignedData;
		}

		public static CmsSignedData ReplaceSigners(CmsSignedData signedData, List<SignerInformation> signerInformationList)
		{
			SignerInformationStore signerInformationStore = new SignerInformationStore(signerInformationList);
			CmsSignedData cmsSignedData = new CmsSignedData(signedData);
			cmsSignedData.signerInfoStore = signerInformationStore;
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector();
			Asn1EncodableVector asn1EncodableVector2 = new Asn1EncodableVector();
			foreach (SignerInformation signer in signerInformationStore.GetSigners())
			{
				asn1EncodableVector.Add(Helper.FixAlgID(signer.DigestAlgorithmID));
				asn1EncodableVector2.Add(signer.ToSignerInfo());
			}
			Asn1Set asn1Set = new DerSet(asn1EncodableVector, false);
			Asn1Set asn1Set2 = new DerSet(asn1EncodableVector2, false);
			Asn1Sequence asn1Sequence = (Asn1Sequence)signedData.signedData.ToAsn1Object();
			asn1EncodableVector2 = new Asn1EncodableVector(asn1Sequence[0], asn1Set);
			for (int i = 2; i != asn1Sequence.Count - 1; i++)
			{
				asn1EncodableVector2.Add(asn1Sequence[i]);
			}
			asn1EncodableVector2.Add(asn1Set2);
			cmsSignedData.signedData = SignedData.GetInstance(new BerSequence(asn1EncodableVector2));
			cmsSignedData.contentInfo = new ContentInfo(cmsSignedData.contentInfo.ContentType, cmsSignedData.signedData);
			return cmsSignedData;
		}

		public static CmsSignedData ReplaceCertificatesAndCrls(CmsSignedData signedData, IX509Store x509Certs, IX509Store x509Crls, IX509Store x509AttrCerts)
		{
			if (x509AttrCerts != null)
			{
				throw Platform.CreateNotImplementedException("Currently can't replace attribute certificates");
			}
			CmsSignedData cmsSignedData = new CmsSignedData(signedData);
			Asn1Set certificates = null;
			try
			{
				Asn1Set asn1Set = CmsUtilities.CreateBerSetFromList(CmsUtilities.GetCertificatesFromStore(x509Certs));
				if (asn1Set.Count != 0)
				{
					certificates = asn1Set;
				}
			}
			catch (X509StoreException e)
			{
				throw new CmsException("error getting certificates from store", e);
			}
			Asn1Set crls = null;
			try
			{
				Asn1Set asn1Set2 = CmsUtilities.CreateBerSetFromList(CmsUtilities.GetCrlsFromStore(x509Crls));
				if (asn1Set2.Count != 0)
				{
					crls = asn1Set2;
				}
			}
			catch (X509StoreException e2)
			{
				throw new CmsException("error getting CRLs from store", e2);
			}
			SignedData signedData2 = signedData.signedData;
			cmsSignedData.signedData = new SignedData(signedData2.DigestAlgorithms, signedData2.EncapContentInfo, certificates, crls, signedData2.SignerInfos);
			cmsSignedData.contentInfo = new ContentInfo(cmsSignedData.contentInfo.ContentType, cmsSignedData.signedData);
			return cmsSignedData;
		}
	}
}
