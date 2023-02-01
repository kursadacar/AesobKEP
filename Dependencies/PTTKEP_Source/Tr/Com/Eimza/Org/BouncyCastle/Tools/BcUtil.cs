using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Pkcs;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Tools
{
	internal static class BcUtil
	{
		public static byte[] Digest(byte[] value, IDigest digestAlg)
		{
			byte[] array = null;
			try
			{
				return DigestUtilities.DoFinal(digestAlg, value);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static byte[] Digest(byte[] value, string digestAlgOid)
		{
			IDigest digest = DigestUtilities.GetDigest(digestAlgOid);
			byte[] array = null;
			try
			{
				return DigestUtilities.DoFinal(digest, value);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static byte[] Digest(byte[] value, DerObjectIdentifier digestAlgOid)
		{
			IDigest digest = DigestUtilities.GetDigest(digestAlgOid);
			byte[] array = null;
			try
			{
				return DigestUtilities.DoFinal(digest, value);
			}
			catch (Exception)
			{
				return null;
			}
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
				string text = string.Empty;
				IEnumerator enumerator = keyStore.Aliases.GetEnumerator();
				if (enumerator.MoveNext())
				{
					text = enumerator.Current as string;
				}
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

		public static X509Certificate GetCertificateFromPfx(string strCertificatePath, string strCertificatePassword)
		{
			try
			{
				Pkcs12Store keyStore = GetKeyStore(strCertificatePath, strCertificatePassword);
				string text = string.Empty;
				IEnumerator enumerator = keyStore.Aliases.GetEnumerator();
				if (enumerator.MoveNext())
				{
					text = enumerator.Current as string;
				}
				if (text == null)
				{
					throw new NotImplementedException("Alias");
				}
				return keyStore.GetCertificate(text).Certificate;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static string FindFileNameFromUrl(string downloadUrl)
		{
			return Path.GetFileName(new Uri(downloadUrl).LocalPath);
		}

		public static byte[] GetValue(Stream responseStream)
		{
			byte[] array = new byte[4096];
			if (responseStream.CanSeek)
			{
				responseStream.Seek(0L, SeekOrigin.Begin);
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int num = 0;
				do
				{
					num = responseStream.Read(array, 0, array.Length);
					memoryStream.Write(array, 0, num);
				}
				while (num != 0);
				return memoryStream.ToArray();
			}
		}

		public static List<string> Parse(string data, string delimiter)
		{
			try
			{
				if (data == null)
				{
					return null;
				}
				if (!delimiter.EndsWith("="))
				{
					delimiter += "=";
				}
				if (!data.Contains(delimiter))
				{
					return null;
				}
				List<string> list = new List<string>();
				int num = data.IndexOf(delimiter) + delimiter.Length;
				int num2 = data.IndexOf(',', num) - num;
				if (num2 == 0)
				{
					return null;
				}
				if (num2 > 0)
				{
					list.Add(data.Substring(num, num2));
					List<string> list2 = Parse(data.Substring(num + num2), delimiter);
					if (list2 != null)
					{
						list.AddRange(list2);
					}
				}
				else
				{
					list.Add(data.Substring(num));
				}
				return list;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
