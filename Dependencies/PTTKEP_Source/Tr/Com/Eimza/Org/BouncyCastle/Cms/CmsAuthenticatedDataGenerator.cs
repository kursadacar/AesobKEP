using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class CmsAuthenticatedDataGenerator : CmsAuthenticatedGenerator
	{
		public CmsAuthenticatedDataGenerator()
		{
		}

		public CmsAuthenticatedDataGenerator(SecureRandom rand)
			: base(rand)
		{
		}

		private CmsAuthenticatedData Generate(CmsProcessable content, string macOid, CipherKeyGenerator keyGen)
		{
			KeyParameter keyParameter;
			AlgorithmIdentifier algorithmIdentifier;
			Asn1OctetString content2;
			Asn1OctetString mac2;
			try
			{
				byte[] array = keyGen.GenerateKey();
				keyParameter = ParameterUtilities.CreateKeyParameter(macOid, array);
				Asn1Encodable asn1Params = GenerateAsn1Parameters(macOid, array);
				ICipherParameters cipherParameters;
				algorithmIdentifier = GetAlgorithmIdentifier(macOid, keyParameter, asn1Params, out cipherParameters);
				IMac mac = MacUtilities.GetMac(macOid);
				mac.Init(keyParameter);
				MemoryStream memoryStream = new MemoryStream();
				Stream stream = new TeeOutputStream(memoryStream, new MacOutputStream(mac));
				content.Write(stream);
				stream.Close();
				memoryStream.Close();
				content2 = new BerOctetString(memoryStream.ToArray());
				mac2 = new DerOctetString(MacUtilities.DoFinal(mac));
			}
			catch (SecurityUtilityException e)
			{
				throw new CmsException("couldn't create cipher.", e);
			}
			catch (InvalidKeyException e2)
			{
				throw new CmsException("key invalid in message.", e2);
			}
			catch (IOException e3)
			{
				throw new CmsException("exception decoding algorithm parameters.", e3);
			}
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector();
			foreach (RecipientInfoGenerator recipientInfoGenerator in recipientInfoGenerators)
			{
				try
				{
					asn1EncodableVector.Add(recipientInfoGenerator.Generate(keyParameter, rand));
				}
				catch (InvalidKeyException e4)
				{
					throw new CmsException("key inappropriate for algorithm.", e4);
				}
				catch (GeneralSecurityException e5)
				{
					throw new CmsException("error making encrypted content.", e5);
				}
			}
			ContentInfo encapsulatedContent = new ContentInfo(CmsObjectIdentifiers.Data, content2);
			return new CmsAuthenticatedData(new ContentInfo(CmsObjectIdentifiers.AuthenticatedData, new AuthenticatedData(null, new DerSet(asn1EncodableVector), algorithmIdentifier, null, encapsulatedContent, null, mac2, null)));
		}

		public CmsAuthenticatedData Generate(CmsProcessable content, string encryptionOid)
		{
			try
			{
				CipherKeyGenerator keyGenerator = GeneratorUtilities.GetKeyGenerator(encryptionOid);
				keyGenerator.Init(new KeyGenerationParameters(rand, keyGenerator.DefaultStrength));
				return Generate(content, encryptionOid, keyGenerator);
			}
			catch (SecurityUtilityException e)
			{
				throw new CmsException("can't find key generation algorithm.", e);
			}
		}
	}
}
