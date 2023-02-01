using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class CmsAuthenticatedData
	{
		internal readonly RecipientInformationStore recipientInfoStore;

		internal readonly ContentInfo contentInfo;

		private readonly AlgorithmIdentifier macAlg;

		private readonly Asn1Set authAttrs;

		private readonly Asn1Set unauthAttrs;

		private readonly byte[] mac;

		public AlgorithmIdentifier MacAlgorithmID
		{
			get
			{
				return macAlg;
			}
		}

		public string MacAlgOid
		{
			get
			{
				return macAlg.ObjectID.Id;
			}
		}

		public ContentInfo ContentInfo
		{
			get
			{
				return contentInfo;
			}
		}

		public CmsAuthenticatedData(byte[] authData)
			: this(CmsUtilities.ReadContentInfo(authData))
		{
		}

		public CmsAuthenticatedData(Stream authData)
			: this(CmsUtilities.ReadContentInfo(authData))
		{
		}

		public CmsAuthenticatedData(ContentInfo contentInfo)
		{
			this.contentInfo = contentInfo;
			AuthenticatedData instance = AuthenticatedData.GetInstance(contentInfo.Content);
			Asn1Set recipientInfos = instance.RecipientInfos;
			macAlg = instance.MacAlgorithm;
			CmsReadable readable = new CmsProcessableByteArray(Asn1OctetString.GetInstance(instance.EncapsulatedContentInfo.Content).GetOctets());
			CmsSecureReadable secureReadable = new CmsEnvelopedHelper.CmsAuthenticatedSecureReadable(macAlg, readable);
			recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore(recipientInfos, secureReadable);
			authAttrs = instance.AuthAttrs;
			mac = instance.Mac.GetOctets();
			unauthAttrs = instance.UnauthAttrs;
		}

		public byte[] GetMac()
		{
			return Arrays.Clone(mac);
		}

		public RecipientInformationStore GetRecipientInfos()
		{
			return recipientInfoStore;
		}

		public Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.AttributeTable GetAuthAttrs()
		{
			if (authAttrs == null)
			{
				return null;
			}
			return new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.AttributeTable(authAttrs);
		}

		public Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.AttributeTable GetUnauthAttrs()
		{
			if (unauthAttrs == null)
			{
				return null;
			}
			return new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.AttributeTable(unauthAttrs);
		}

		public byte[] GetEncoded()
		{
			return contentInfo.GetEncoded();
		}
	}
}
