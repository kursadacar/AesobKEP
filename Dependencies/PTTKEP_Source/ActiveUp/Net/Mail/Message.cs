using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using ActiveUp.Net.Security;
using Tr.Com.Eimza.Cades.Tools;
using Tr.Com.Eimza.EYazisma;
using Tr.Com.Eimza.Log4Net;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ess;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Nist;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Oiw;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Signers;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.X509.Store;
using Tr.Com.Eimza.SmartCard;

namespace ActiveUp.Net.Mail
{
    [Serializable]
	public class Message : Header
	{
		private delegate void DelegateAppend(IMailbox imapMailbox);

		internal Hashtable _CustomCollection = new Hashtable();

		internal AttachmentCollection _attachments = new AttachmentCollection();

		internal EmbeddedObjectCollection _embeddedObjects = new EmbeddedObjectCollection();

		internal MimePartCollection _allMimeParts = new MimePartCollection();

		internal MessageCollection _subMessages = new MessageCollection();

		internal MimePartCollection _otherParts = new MimePartCollection();

		internal bool _builtMimePartTree;

		private MimePart _partTreeRoot = new MimePart();

		private MimeBody _bodyHtml = new MimeBody(BodyFormat.Html);

		private MimeBody _bodyText = new MimeBody(BodyFormat.Text);

		private string _preamble;

		private string _epilogue;

		private Signatures _signatures = new Signatures();

		private bool _isSmimeEncrypted;

		private bool _hasDomainKeySignature;

		private bool _hasSmimeSignature;

		private bool _hasSmimeDetachedSignature;

		private DelegateAppend _delegateAppend;

		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static object Sync = new object();

		public AttachmentCollection Attachments
		{
			get
			{
				return _attachments;
			}
		}

		public Hashtable CustomCollection
		{
			get
			{
				return _CustomCollection;
			}
			set
			{
				_CustomCollection = value;
			}
		}

		public Signatures Signatures
		{
			get
			{
				return _signatures;
			}
			set
			{
				_signatures = value;
			}
		}

		public bool HasDomainKeySignature
		{
			get
			{
				return _hasDomainKeySignature;
			}
			set
			{
				_hasDomainKeySignature = value;
			}
		}

		public bool HasSmimeSignature
		{
			get
			{
				return _hasSmimeSignature;
			}
			set
			{
				_hasSmimeSignature = value;
			}
		}

		public bool HasSmimeDetachedSignature
		{
			get
			{
				return _hasSmimeDetachedSignature;
			}
			set
			{
				_hasSmimeDetachedSignature = value;
			}
		}

		public bool IsHtml { get; set; }

		public bool IsSmimeEncrypted
		{
			get
			{
				return _isSmimeEncrypted;
			}
			set
			{
				_isSmimeEncrypted = value;
			}
		}

		public EmbeddedObjectCollection EmbeddedObjects
		{
			get
			{
				return _embeddedObjects;
			}
		}

		public MessageCollection SubMessages
		{
			get
			{
				return _subMessages;
			}
		}

		public MimePartCollection LeafMimeParts
		{
			get
			{
				return _allMimeParts;
			}
		}

		public MimePartCollection UnknownDispositionMimeParts
		{
			get
			{
				return _otherParts;
			}
			set
			{
				_otherParts = value;
			}
		}

		public MimePart PartTreeRoot
		{
			get
			{
				return _partTreeRoot;
			}
			set
			{
				_partTreeRoot = value;
			}
		}

		public MimeBody BodyText
		{
			get
			{
				return _bodyText;
			}
			set
			{
				_bodyText = value;
			}
		}

		public MimeBody BodyHtml
		{
			get
			{
				return _bodyHtml;
			}
			set
			{
				_bodyHtml = value;
			}
		}

		public string Preamble
		{
			get
			{
				return _preamble;
			}
			set
			{
				_preamble = value;
			}
		}

		public string Epilogue
		{
			get
			{
				return _epilogue;
			}
			set
			{
				_epilogue = value;
			}
		}

		public int Size
		{
			get
			{
				return base.OriginalData.Length;
			}
		}

		public new string Summary
		{
			get
			{
				string text = ((base.From.Email != "") ? base.From.Link : base.Sender.Link);
				text += "<br />";
				text = text + "To : " + base.To.Links + "<br />";
				if (base.Cc != null)
				{
					text = text + "Cc : " + base.Cc.Links + "<br />";
				}
				text = text + "Subject : " + base.Subject + "<br />";
				text = text + "Received : " + base.DateString + "<br />";
				return text + "Body : <br />" + BodyText.Text;
			}
		}

		private string GetEncodedMimePart(MimePart part)
		{
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				stringBuilder.Append(part.ToMimeString());
				if (part.ContentTransferEncoding == ContentTransferEncoding.Base64)
				{
					stringBuilder.Append("\r\n\r\n" + Regex.Replace(Convert.ToBase64String(part.BinaryContent, 0, part.BinaryContent.Length), "(?<found>[^\n]{100})", "${found}\n"));
				}
				else if (part.ContentTransferEncoding == ContentTransferEncoding.QuotedPrintable)
				{
					stringBuilder.Append("\r\n\r\n" + Codec.ToQuotedPrintable(Encoding.ASCII.GetString(part.BinaryContent, 0, part.BinaryContent.Length), part.Charset));
				}
				else if (part.ContentTransferEncoding == ContentTransferEncoding.SevenBits)
				{
					stringBuilder.Append("\r\n\r\n" + Encoding.UTF7.GetString(part.BinaryContent, 0, part.BinaryContent.Length));
				}
				else if (part.ContentTransferEncoding == ContentTransferEncoding.EightBits)
				{
					stringBuilder.Append("\r\n\r\n" + Encoding.UTF8.GetString(part.BinaryContent, 0, part.BinaryContent.Length));
				}
				else
				{
					stringBuilder.Append("\r\n\r\n" + Encoding.ASCII.GetString(part.BinaryContent, 0, part.BinaryContent.Length));
				}
			}
			catch (Exception)
			{
			}
			return stringBuilder.ToString();
		}

		private string GetEmbeddedObjects(string boundary)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (MimePart embeddedObject in EmbeddedObjects)
			{
				stringBuilder.Append(boundary + embeddedObject.ToMimeString());
			}
			return stringBuilder.ToString();
		}

		private string GetAttachments(string boundary)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (MimePart attachment in Attachments)
			{
				stringBuilder.Append(boundary + attachment.ToMimeString());
			}
			return stringBuilder.ToString();
		}

		private MimePart GetBodiesPart()
		{
			MimePart mimePart = new MimePart();
			bool flag = BodyHtml.Text.Length > 0;
			bool flag2 = BodyText.Text.Length > 0;
			if (flag && flag2)
			{
				mimePart.ContentType.MimeType = "multipart/alternative";
				string uniqueString = Codec.GetUniqueString();
				string value = "---=_Part_" + uniqueString;
				mimePart.ContentType.Parameters.Add("boundary", value);
				mimePart.SubParts.Add(BodyText.ToMimePart());
				mimePart.SubParts.Add(BodyHtml.ToMimePart());
			}
			else if (flag)
			{
				mimePart = BodyHtml.ToMimePart();
			}
			else if (flag2)
			{
				mimePart = BodyText.ToMimePart();
			}
			return mimePart;
		}

		private MimePart GetMultipartRelatedContainer()
		{
			MimePart mimePart = new MimePart();
			if (EmbeddedObjects.Count > 0)
			{
				mimePart.ContentType.MimeType = "multipart/related";
				string uniqueString = Codec.GetUniqueString();
				string value = "---=_Part_" + uniqueString;
				mimePart.ContentType.Parameters.Add("boundary", value);
				mimePart.ContentType.Parameters.Add("type", "\"multipart/alternative\"");
				mimePart.SubParts.Add(GetBodiesPart());
				{
					foreach (MimePart embeddedObject in EmbeddedObjects)
					{
						mimePart.SubParts.Add(embeddedObject);
					}
					return mimePart;
				}
			}
			return GetBodiesPart();
		}

		private MimePart GetMultipartMixedContainer()
		{
			MimePart mimePart = new MimePart();
			if (Attachments.Count > 0 || UnknownDispositionMimeParts.Count > 0 || SubMessages.Count > 0)
			{
				mimePart.ContentType.MimeType = "multipart/mixed";
				string uniqueString = Codec.GetUniqueString();
				string value = "---=_Part_" + uniqueString;
				mimePart.ContentType.Parameters.Add("boundary", value);
				mimePart.SubParts.Add(GetMultipartRelatedContainer());
				foreach (MimePart attachment in Attachments)
				{
					mimePart.SubParts.Add(attachment);
				}
				foreach (MimePart unknownDispositionMimePart in UnknownDispositionMimeParts)
				{
					mimePart.SubParts.Add(unknownDispositionMimePart);
				}
				{
					foreach (Message subMessage in SubMessages)
					{
						mimePart.SubParts.Add(subMessage.ToMimePart());
					}
					return mimePart;
				}
			}
			return GetMultipartRelatedContainer();
		}

		public MimePart ToMimePart()
		{
			MimePart mimePart = new MimePart();
			try
			{
				mimePart.Charset = base.Charset;
				mimePart.ContentTransferEncoding = ContentTransferEncoding.SevenBits;
				mimePart.ContentDisposition.Disposition = "attachment";
				mimePart.ContentDisposition.FileName = base.Subject.Trim(' ').Replace(" ", "_") + ".eml";
				mimePart.ContentType.MimeType = "message/rfc822";
				mimePart.TextContent = ToMimeString();
				return mimePart;
			}
			catch (Exception)
			{
				return mimePart;
			}
		}

		public string ToMimeString()
		{
			return ToMimeString(false);
		}

		public string ToMimeString(bool removeBlindCopies, bool forceBase64Encoding = false)
		{
			CheckBuiltMimePartTree();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(ToHeaderString(removeBlindCopies, forceBase64Encoding).TrimEnd('\r', '\n') + Tokenizer.NewLine);
			stringBuilder.Append(Tokenizer.NewLine);
			string text = PartTreeRoot.ToMimeString(forceBase64Encoding);
			int index = Regex.Match(text, "(?<=\\r?\\n\\r?\\n).").Index;
			stringBuilder.Append(text.Substring(index).TrimStart('\r', '\n'));
			string result = stringBuilder.ToString();
			if (base.ContentType.Type.Equals("multipart"))
			{
				result = stringBuilder.ToString().TrimEnd('\r', '\n');
			}
			return result;
		}

		public void CheckBuiltMimePartTree()
		{
			if (!_builtMimePartTree)
			{
				BuildMimePartTree();
			}
		}

		public void BuildMimePartTree()
		{
			PartTreeRoot = GetMultipartMixedContainer();
			base.ContentType = PartTreeRoot.ContentType;
			_builtMimePartTree = true;
		}

		public string GetMidReference()
		{
			return "mid:" + HttpUtility.UrlEncode(base.MessageId.Trim('<', '>'));
		}

		public void Append(IMailbox imapMailbox)
		{
			imapMailbox.Append(ToString());
		}

		public IAsyncResult BeginAppend(IMailbox imapMailbox, AsyncCallback callback)
		{
			_delegateAppend = Append;
			return _delegateAppend.BeginInvoke(imapMailbox, callback, _delegateAppend);
		}

		public void EndAppend(IAsyncResult result)
		{
			_delegateAppend.EndInvoke(result);
		}

		public string StoreToFile(string fileName, bool useTemp)
		{
			string text = "";
			text = (useTemp ? System.IO.Path.GetTempFileName() : fileName);
			string contents = ToMimeString();
			File.WriteAllText(text, contents, Encoding.UTF8);
			if (useTemp)
			{
				if (!string.IsNullOrEmpty(fileName))
				{
					if (!Directory.Exists(System.IO.Path.GetDirectoryName(fileName)))
					{
						Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fileName));
					}
					if (string.IsNullOrEmpty(System.IO.Path.GetFileName(fileName)))
					{
						string text2 = System.IO.Path.Combine(fileName, System.IO.Path.GetFileNameWithoutExtension(text) + ".eml");
						File.Move(text, text2);
						File.Delete(text);
						return text2;
					}
					File.Move(text, fileName);
					File.Delete(text);
					return fileName;
				}
				return text;
			}
			return text;
		}

		public override string StoreToFile(string path)
		{
			return StoreToFile(path, false);
		}

		public new BounceResult GetBounceStatus()
		{
			return GetBounceStatus(null);
		}

		public new BounceResult GetBounceStatus(string signaturesFilePath)
		{
			string empty = string.Empty;
			empty = (string.IsNullOrEmpty(signaturesFilePath) ? Header.GetResource("ActiveUp.Net.Common.BouncedSignatures.xml") : File.OpenText(signaturesFilePath).ReadToEnd());
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(empty);
			BounceResult bounceResult = new BounceResult();
			foreach (XmlElement item in xmlDocument.GetElementsByTagName("signature"))
			{
				int level = bounceResult.Level;
				bounceResult.Level = 0;
				if (item.GetAttribute("from").Trim() != "" && base.From.Merged.IndexOf(item.GetAttribute("from")) != -1)
				{
					bounceResult.Level++;
				}
				if (base.Subject != null && item.GetAttribute("subject").Trim() != "" && base.Subject.IndexOf(item.GetAttribute("subject")) != -1)
				{
					bounceResult.Level++;
				}
				if (item.GetAttribute("body").Trim() != "" && BodyText.Text.IndexOf(item.GetAttribute("body")) != -1)
				{
					bounceResult.Level++;
				}
				if (bounceResult.Level < level)
				{
					bounceResult.Level = level;
				}
				if (bounceResult.Level > 0)
				{
					int num = 0;
					string text = BodyText.Text;
					if (item.GetAttribute("body") != string.Empty)
					{
						num = text.IndexOf(item.GetAttribute("body"));
					}
					if (num < 0)
					{
						num = 0;
					}
					Regex regex = new Regex("\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
					if (regex.IsMatch(text, num))
					{
						Match match = regex.Match(text, num);
						bounceResult.Email = match.Value;
						return bounceResult;
					}
					return bounceResult;
				}
			}
			return bounceResult;
		}

		public Message Clone()
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream memoryStream = new MemoryStream();
			binaryFormatter.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			Message obj = (Message)binaryFormatter.Deserialize(memoryStream);
			obj.Signatures = Signatures;
			return obj;
		}

		public Message SmimeDevelopeAndDecrypt()
		{
			return SmimeDevelopeAndDecrypt(new X509Certificate2Collection());
		}

		public Message SmimeDevelopeAndDecrypt(X509Certificate2Collection extraStore)
		{
			if (!IsSmimeEncrypted)
			{
				throw new InvalidOperationException("This message doesn't seem to be encrypted, or the encryption method is unknown.");
			}
			EnvelopedCms envelopedCms = new EnvelopedCms();
			envelopedCms.Decode(PartTreeRoot.BinaryContent);
			envelopedCms.Decrypt(extraStore);
			return Parser.ParseMessage(envelopedCms.ContentInfo.Content);
		}

		public Message SmimeDevelopeAndExposeSignature()
		{
			if (!HasSmimeSignature)
			{
				throw new InvalidOperationException("This message doesn't seem to be signed, or the signing method is unknown.");
			}
			SignedCms signedCms = new SignedCms();
			signedCms.Decode(PartTreeRoot.BinaryContent);
			Message message = Parser.ParseMessage(signedCms.ContentInfo.Content);
			message.Signatures.Smime = signedCms;
			return message;
		}

		public void SmimeEnvelopeAndEncryptFor(CmsRecipient recipient)
		{
			SmimeEnvelopeAndEncryptFor(new CmsRecipientCollection(recipient));
		}

		public void SmimeEnvelopeAndEncryptFor(CmsRecipientCollection recipients)
		{
			string s = ToMimeString();
			EnvelopedCms envelopedCms = new EnvelopedCms(new System.Security.Cryptography.Pkcs.ContentInfo(Encoding.ASCII.GetBytes(s)));
			envelopedCms.Encrypt(recipients);
			MimePart mimePart = new MimePart();
			mimePart.ContentType.MimeType = "application/pkcs7-mime";
			mimePart.ContentType.Parameters.Add("smime-type", "encrypted-data");
			mimePart.ContentType.Parameters.Add("name", "smime.p7m");
			mimePart.ContentDisposition.Disposition = "attachment";
			mimePart.ContentDisposition.FileName = "smime.p7m";
			mimePart.ContentTransferEncoding = ContentTransferEncoding.Base64;
			mimePart.BinaryContent = envelopedCms.Encode();
			PartTreeRoot = mimePart;
			base.ContentType = PartTreeRoot.ContentType;
			base.ContentDisposition = PartTreeRoot.ContentDisposition;
			base.ContentTransferEncoding = PartTreeRoot.ContentTransferEncoding;
		}

		public void SmimeEnvelopeAndSignBy(CmsSigner signer)
		{
			string s = ToMimeString();
			SignedCms signedCms = new SignedCms(new System.Security.Cryptography.Pkcs.ContentInfo(Encoding.ASCII.GetBytes(s)));
			signedCms.ComputeSignature(signer);
			MimePart mimePart = new MimePart();
			mimePart.ContentType.MimeType = "application/pkcs7-mime";
			mimePart.ContentType.Parameters.Add("smime-type", "signed-data");
			mimePart.ContentType.Parameters.Add("name", "smime.p7m");
			mimePart.ContentDisposition.Disposition = "attachment";
			mimePart.ContentDisposition.FileName = "smime.p7m";
			mimePart.ContentTransferEncoding = ContentTransferEncoding.Base64;
			mimePart.BinaryContent = signedCms.Encode();
			PartTreeRoot = mimePart;
			base.ContentType = PartTreeRoot.ContentType;
			base.ContentDisposition = PartTreeRoot.ContentDisposition;
			base.ContentTransferEncoding = PartTreeRoot.ContentTransferEncoding;
		}

		public void SmimeAttachSignatureBy(CmsSigner signer)
		{
			string text = PartTreeRoot.ToMimeString();
			SignedCms signedCms = new SignedCms(new System.Security.Cryptography.Pkcs.ContentInfo(Encoding.ASCII.GetBytes(text.TrimEnd('\r', '\n') + "\r\n")), true);
			signedCms.ComputeSignature(signer);
			MimePart mimePart = new MimePart();
			Signatures.Smime = signedCms;
			mimePart.ContentType.MimeType = "multipart/signed";
			mimePart.ContentType.Parameters.Add("protocol", "\"application/x-pkcs7-signature\"");
			mimePart.ContentType.Parameters.Add("micalg", signedCms.SignerInfos[0].DigestAlgorithm.FriendlyName);
			string uniqueString = Codec.GetUniqueString();
			string value = "---=_Part_" + uniqueString;
			mimePart.ContentType.Parameters.Add("boundary", value);
			mimePart.SubParts.Add(PartTreeRoot);
			mimePart.SubParts.Add(MimePart.GetSignaturePart(signedCms));
			PartTreeRoot = mimePart;
			base.ContentType = PartTreeRoot.ContentType;
			base.ContentDisposition = PartTreeRoot.ContentDisposition;
			base.ContentTransferEncoding = PartTreeRoot.ContentTransferEncoding;
		}

		internal void SmimeAttachSignatureBy(SmartCardReader reader)
		{
			string text = PartTreeRoot.ToMimeString();
			byte[] bytes = Encoding.ASCII.GetBytes(text.TrimEnd('\r', '\n') + "\r\n");
			byte[] signatureWithSmartCard = GetSignatureWithSmartCard(bytes, reader, true);
			SignedCms signedCms = new SignedCms();
			signedCms.Decode(signatureWithSmartCard);
			MimePart mimePart = new MimePart();
			Signatures.Smime = signedCms;
			mimePart.ContentType.MimeType = "multipart/signed";
			mimePart.ContentType.Parameters.Add("protocol", "\"application/x-pkcs7-signature\"");
			mimePart.ContentType.Parameters.Add("micalg", signedCms.SignerInfos[0].DigestAlgorithm.FriendlyName);
			string uniqueString = Codec.GetUniqueString();
			string value = "---=_Part_" + uniqueString;
			mimePart.ContentType.Parameters.Add("boundary", value);
			mimePart.SubParts.Add(PartTreeRoot);
			mimePart.SubParts.Add(MimePart.GetSignaturePart(signedCms));
			PartTreeRoot = mimePart;
			base.ContentType = PartTreeRoot.ContentType;
			base.ContentDisposition = PartTreeRoot.ContentDisposition;
			base.ContentTransferEncoding = PartTreeRoot.ContentTransferEncoding;
		}

		internal void SmimeAttachSignatureBy(SmartCardReader reader, OzetAlg ozetAlg)
		{
			string text = PartTreeRoot.ToMimeString();
			byte[] bytes = Encoding.ASCII.GetBytes(text.TrimEnd('\r', '\n') + "\r\n");
			byte[] signatureWithSmartCard = GetSignatureWithSmartCard(bytes, reader, ozetAlg, true);
			SignedCms signedCms = new SignedCms();
			signedCms.Decode(signatureWithSmartCard);
			MimePart mimePart = new MimePart();
			Signatures.Smime = signedCms;
			mimePart.ContentType.MimeType = "multipart/signed";
			mimePart.ContentType.Parameters.Add("protocol", "\"application/x-pkcs7-signature\"");
			mimePart.ContentType.Parameters.Add("micalg", signedCms.SignerInfos[0].DigestAlgorithm.FriendlyName);
			string uniqueString = Codec.GetUniqueString();
			string value = "---=_Part_" + uniqueString;
			mimePart.ContentType.Parameters.Add("boundary", value);
			mimePart.SubParts.Add(PartTreeRoot);
			mimePart.SubParts.Add(MimePart.GetSignaturePart(signedCms));
			PartTreeRoot = mimePart;
			base.ContentType = PartTreeRoot.ContentType;
			base.ContentDisposition = PartTreeRoot.ContentDisposition;
			base.ContentTransferEncoding = PartTreeRoot.ContentTransferEncoding;
		}

		internal byte[] GetSignatureWithSmartCard(byte[] messageBytes, SmartCardReader reader, bool DETACHED)
		{
			try
			{
				string encryptionOID = "";
				string digestOID = "";
				reader.SmartCardParams.SignatureAlgorithm = reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex].SigAlgOid;
				DefaultSignedAttributeTableGenerator defaultSignedAttributeTable = GetDefaultSignedAttributeTable(reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex]);
				switch (reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex].SigAlgOid)
				{
				case "1.2.840.113549.1.1.5":
					encryptionOID = "1.2.840.113549.1.1.11";
					digestOID = "2.16.840.1.101.3.4.2.1";
					break;
				case "1.2.840.113549.1.1.11":
					encryptionOID = "1.2.840.113549.1.1.11";
					digestOID = "2.16.840.1.101.3.4.2.1";
					break;
				case "1.2.840.113549.1.1.12":
					encryptionOID = "1.2.840.113549.1.1.12";
					digestOID = "2.16.840.1.101.3.4.2.2";
					break;
				case "1.2.840.113549.1.1.13":
					encryptionOID = "1.2.840.113549.1.1.13";
					digestOID = "2.16.840.1.101.3.4.2.3";
					break;
				}
				CmsSignedDataGenerator cmsSignedDataGenerator = new CmsSignedDataGenerator();
				cmsSignedDataGenerator.AddSigner(new NullPrivateKey(), reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex], encryptionOID, digestOID, defaultSignedAttributeTable, null);
				IX509Store certStore = CertificateUtils.CreateCertificateStore(reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex]);
				cmsSignedDataGenerator.AddCertificates(certStore);
				SmartCardSigner signerProvider = new SmartCardSigner(reader);
				cmsSignedDataGenerator.SignerProvider = signerProvider;
				CmsProcessableByteArray cmsProcessableByteArray = new CmsProcessableByteArray(messageBytes);
				CmsSignedData cmsSignedData = cmsSignedDataGenerator.Generate(cmsProcessableByteArray, !DETACHED);
				cmsSignedData = new CmsSignedData(cmsProcessableByteArray, cmsSignedData.GetEncoded());
				return cmsSignedData.GetEncoded();
			}
			catch (Exception)
			{
				return null;
			}
		}

		internal byte[] GetSignatureWithSmartCard(byte[] messageBytes, SmartCardReader reader, OzetAlg ozetAlg, bool DETACHED)
		{
			try
			{
				string encryptionOID = "";
				string digestOID = "";
				reader.SmartCardParams.SignatureAlgorithm = reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex].SigAlgOid;
				DefaultSignedAttributeTableGenerator defaultSignedAttributeTable = GetDefaultSignedAttributeTable(reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex]);
				switch (ozetAlg)
				{
				case OzetAlg.SHA256:
					encryptionOID = "1.2.840.113549.1.1.11";
					digestOID = "2.16.840.1.101.3.4.2.1";
					break;
				case OzetAlg.SHA512:
					encryptionOID = "1.2.840.113549.1.1.13";
					digestOID = "2.16.840.1.101.3.4.2.3";
					break;
				}
				CmsSignedDataGenerator cmsSignedDataGenerator = new CmsSignedDataGenerator();
				cmsSignedDataGenerator.AddSigner(new NullPrivateKey(), reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex], encryptionOID, digestOID, defaultSignedAttributeTable, null);
				IX509Store certStore = CertificateUtils.CreateCertificateStore(reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex]);
				cmsSignedDataGenerator.AddCertificates(certStore);
				SmartCardSigner signerProvider = new SmartCardSigner(reader);
				cmsSignedDataGenerator.SignerProvider = signerProvider;
				CmsProcessableByteArray cmsProcessableByteArray = new CmsProcessableByteArray(messageBytes);
				CmsSignedData cmsSignedData = cmsSignedDataGenerator.Generate(cmsProcessableByteArray, !DETACHED);
				cmsSignedData = new CmsSignedData(cmsProcessableByteArray, cmsSignedData.GetEncoded());
				return cmsSignedData.GetEncoded();
			}
			catch (Exception)
			{
				return null;
			}
		}

		internal DefaultSignedAttributeTableGenerator GetDefaultSignedAttributeTable(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate signerCert)
		{
			return new DefaultSignedAttributeTableGenerator(GetSignedAttributeTable(signerCert));
		}

		internal Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.AttributeTable GetSignedAttributeTable(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate signerCert)
		{
			return new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.AttributeTable(GetSignedAttribute(signerCert));
		}

		internal Dictionary<DerObjectIdentifier, Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute> GetSignedAttribute(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate signerCert)
		{
			Dictionary<DerObjectIdentifier, Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute> dictionary = new Dictionary<DerObjectIdentifier, Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute>();
			Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute attribute = MakeSigningCertificateAttribute(signerCert);
			dictionary.Add(attribute.AttrType, attribute);
			return dictionary;
		}

		internal Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute MakeSigningCertificateAttribute(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate signerCert)
		{
			Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute attribute = null;
			byte[] array = null;
			if (signerCert.SigAlgName.Equals("SHA-1withRSA"))
			{
				array = Utilities.GetByteToByteHash(signerCert.GetEncoded(), DigestUtilities.GetDigest(OiwObjectIdentifiers.IdSha1));
				new IssuerSerial(new GeneralNames(new GeneralName(signerCert.IssuerDN)), new DerInteger(signerCert.SerialNumber));
				DerSet attrValues = new DerSet(new SigningCertificate(new EssCertID(array)));
				return new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute(PkcsObjectIdentifiers.IdAASigningCertificate, attrValues);
			}
			array = Utilities.GetByteToByteHash(signerCert.GetEncoded(), DigestUtilities.GetDigest(NistObjectIdentifiers.IdSha256));
			IssuerSerial issuerSerial = new IssuerSerial(new GeneralNames(new GeneralName(signerCert.IssuerDN)), new DerInteger(signerCert.SerialNumber));
			EssCertIDv2 essCertIDv = new EssCertIDv2(new Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509.AlgorithmIdentifier(NistObjectIdentifiers.IdSha256.Id), array, issuerSerial);
			DerSet attrValues2 = new DerSet(new SigningCertificateV2(new EssCertIDv2[1] { essCertIDv }));
			return new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute(PkcsObjectIdentifiers.IdAASigningCertificateV2, attrValues2);
		}

		public Message GenerateConfirmReadMessage()
		{
			Message message = new Message();
			message.To.Add(base.ConfirmRead);
			message.From = base.To[0];
			message.Subject = "Read: " + base.Subject;
			message.AddHeaderField("In-Reply-To", base.MessageId);
			DateTime date = base.Date;
			DateTime now = DateTime.Now;
			message.BodyText.Text = string.Format("Your message\r\n\r\n    To:  {0}\r\n    Subject:  {1}\r\n    Sent:  {2} {3}\r\n\r\nwas read on {4} {5}.", base.To[0].Email, base.Subject, date.ToShortDateString(), date.ToShortTimeString(), now.ToShortDateString(), now.ToShortTimeString());
			message.BodyHtml.Text = string.Format("<P><FONT SIZE=3D2>Your message<BR>\r\n<BR>\r\n&nbsp;&nbsp;&nbsp; To:&nbsp; {0}<BR>\r\n&nbsp;&nbsp;&nbsp; Subject:&nbsp; {1}<BR>\r\n&nbsp;&nbsp;&nbsp; Sent:&nbsp; {2} {3}<BR>\r\n<BR>\r\nwas read on {4} {5}.</FONT>\r\n</P>", base.To[0].Email, base.Subject, date.ToShortDateString(), date.ToShortTimeString(), now.ToShortDateString(), now.ToShortTimeString());
			MimePart mimePart = new MimePart();
			mimePart.ContentType.MimeType = "message/disposition-notification";
			mimePart.ContentTransferEncoding = ContentTransferEncoding.QuotedPrintable;
			mimePart.TextContent = string.Format("Reporting-UA: {0}; ActiveUp.MailSystem\r\nFinal-Recipient: rfc822;{1}\r\nOriginal-Message-ID: <{2}>\r\nDisposition: manual-action/MDN-sent-manually; displayed", "domain", base.To[0].Email, base.MessageId);
			message.UnknownDispositionMimeParts.Add(mimePart);
			return message;
		}
	}
}
