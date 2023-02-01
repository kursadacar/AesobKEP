using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class CmsEnvelopedData
	{
		internal RecipientInformationStore recipientInfoStore;

		internal ContentInfo contentInfo;

		private AlgorithmIdentifier encAlg;

		private Asn1Set unprotectedAttributes;

		public AlgorithmIdentifier EncryptionAlgorithmID
		{
			get
			{
				return encAlg;
			}
		}

		public string EncryptionAlgOid
		{
			get
			{
				return encAlg.ObjectID.Id;
			}
		}

		public ContentInfo ContentInfo
		{
			get
			{
				return contentInfo;
			}
		}

		public CmsEnvelopedData(byte[] envelopedData)
			: this(CmsUtilities.ReadContentInfo(envelopedData))
		{
		}

		public CmsEnvelopedData(Stream envelopedData)
			: this(CmsUtilities.ReadContentInfo(envelopedData))
		{
		}

		public CmsEnvelopedData(ContentInfo contentInfo)
		{
			this.contentInfo = contentInfo;
			EnvelopedData instance = EnvelopedData.GetInstance(contentInfo.Content);
			Asn1Set recipientInfos = instance.RecipientInfos;
			EncryptedContentInfo encryptedContentInfo = instance.EncryptedContentInfo;
			encAlg = encryptedContentInfo.ContentEncryptionAlgorithm;
			CmsReadable readable = new CmsProcessableByteArray(encryptedContentInfo.EncryptedContent.GetOctets());
			CmsSecureReadable secureReadable = new CmsEnvelopedHelper.CmsEnvelopedSecureReadable(encAlg, readable);
			recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore(recipientInfos, secureReadable);
			unprotectedAttributes = instance.UnprotectedAttrs;
		}

		public RecipientInformationStore GetRecipientInfos()
		{
			return recipientInfoStore;
		}

		public Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.AttributeTable GetUnprotectedAttributes()
		{
			if (unprotectedAttributes == null)
			{
				return null;
			}
			return new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.AttributeTable(unprotectedAttributes);
		}

		public byte[] GetEncoded()
		{
			return contentInfo.GetEncoded();
		}
	}
}
