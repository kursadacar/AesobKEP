using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Pkcs;
using Tr.Com.Eimza.Org.BouncyCastle.X509;
using Tr.Com.Eimza.Org.BouncyCastle.X509.Store;

namespace Tr.Com.Eimza.Cades.Tools
{
	internal static class CertificateUtils
	{
		public static X509Certificate GetCertFromFile(string fileName)
		{
			FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			byte[] input = Utilities.FileToByte(fileStream);
			fileStream.Close();
			return new X509CertificateParser().ReadCertificate(input);
		}

		public static Pkcs12Store GetKeyStore(string strCertificatePath, string strCertificatePassword)
		{
			try
			{
				FileStream fileStream = new FileStream(strCertificatePath, FileMode.Open, FileAccess.Read);
				Pkcs12Store pkcs12Store = new Pkcs12Store();
				pkcs12Store.Load(fileStream, strCertificatePassword.ToCharArray());
				fileStream.Close();
				return pkcs12Store;
			}
			catch (Exception)
			{
				throw new NotImplementedException("Key Store");
			}
		}

		public static AsymmetricKeyParameter GetPrivateKeyFromPfx(string strCertificatePath, string strCertificatePassword)
		{
			try
			{
				Pkcs12Store keyStore = GetKeyStore(strCertificatePath, strCertificatePassword);
				string text = keyStore.Aliases.Cast<string>().FirstOrDefault((string n) => keyStore.IsKeyEntry(n));
				if (text == null)
				{
					throw new NotImplementedException("Alias");
				}
				return keyStore.GetKey(text).Key;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static IX509Store CreateCertificateStore(X509Certificate cert)
		{
			List<X509Certificate> list = new List<X509Certificate>();
			if (cert != null)
			{
				list.Add(cert);
			}
			return X509StoreFactory.Create("CERTIFICATE/COLLECTION", new X509CollectionStoreParameters(list));
		}
	}
}
