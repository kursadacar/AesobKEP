using System;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal abstract class RecipientInformation
	{
		internal RecipientID rid = new RecipientID();

		internal readonly AlgorithmIdentifier keyEncAlg;

		internal readonly CmsSecureReadable secureReadable;

		private byte[] resultMac;

		public RecipientID RecipientID
		{
			get
			{
				return rid;
			}
		}

		public AlgorithmIdentifier KeyEncryptionAlgorithmID
		{
			get
			{
				return keyEncAlg;
			}
		}

		public string KeyEncryptionAlgOid
		{
			get
			{
				return keyEncAlg.ObjectID.Id;
			}
		}

		public Asn1Object KeyEncryptionAlgParams
		{
			get
			{
				Asn1Encodable parameters = keyEncAlg.Parameters;
				if (parameters != null)
				{
					return parameters.ToAsn1Object();
				}
				return null;
			}
		}

		internal RecipientInformation(AlgorithmIdentifier keyEncAlg, CmsSecureReadable secureReadable)
		{
			this.keyEncAlg = keyEncAlg;
			this.secureReadable = secureReadable;
		}

		internal string GetContentAlgorithmName()
		{
			return secureReadable.Algorithm.ObjectID.Id;
		}

		internal CmsTypedStream GetContentFromSessionKey(KeyParameter sKey)
		{
			CmsReadable readable = secureReadable.GetReadable(sKey);
			try
			{
				return new CmsTypedStream(readable.GetInputStream());
			}
			catch (IOException e)
			{
				throw new CmsException("error getting .", e);
			}
		}

		public byte[] GetContent(ICipherParameters key)
		{
			try
			{
				return CmsUtilities.StreamToByteArray(GetContentStream(key).ContentStream);
			}
			catch (IOException ex)
			{
				throw new Exception("unable to parse internal stream: " + ((ex != null) ? ex.ToString() : null));
			}
		}

		public byte[] GetMac()
		{
			if (resultMac == null)
			{
				object cryptoObject = secureReadable.CryptoObject;
				if (cryptoObject is IMac)
				{
					resultMac = MacUtilities.DoFinal((IMac)cryptoObject);
				}
			}
			return Arrays.Clone(resultMac);
		}

		protected abstract CmsTypedStream GetContentStream(ICipherParameters key);
	}
}
