using System;
using System.IO;
using System.Text;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Esf;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Oiw;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Cades.Tools
{
	internal static class Utilities
	{
		public static int SaveFile(byte[] file, string filename)
		{
			try
			{
				FileStream fileStream = new FileStream(filename, FileMode.Create);
				fileStream.Write(file, 0, file.Length);
				fileStream.Close();
				return 1;
			}
			catch (Exception)
			{
				return -1;
			}
		}

		public static int SaveCert(X509Certificate cert, string fileName)
		{
			byte[] encoded = cert.GetEncoded();
			try
			{
				FileStream fileStream = new FileStream(fileName, FileMode.Create);
				fileStream.Write(encoded, 0, encoded.Length);
				fileStream.Close();
				return 1;
			}
			catch (Exception)
			{
				return -1;
			}
		}

		public static OtherHash CreateOtherHash(byte[] aData, string aDigestAlg)
		{
			if (!aDigestAlg.Equals(OiwObjectIdentifiers.IdSha1.Id))
			{
				return new OtherHash(CreateOtherHashAlgAndValue(aData, aDigestAlg));
			}
			return new OtherHash(CreateOtherHashValue(aData, OiwObjectIdentifiers.IdSha1.Id));
		}

		public static OtherHashAlgAndValue CreateOtherHashAlgAndValue(byte[] aData, string aDigestAlg)
		{
			byte[] str = CreateOtherHashValue(aData, aDigestAlg);
			return new OtherHashAlgAndValue(new AlgorithmIdentifier(aDigestAlg), new DerOctetString(str));
		}

		public static byte[] CreateOtherHashValue(byte[] aData, string aDigestAlg)
		{
			IDigest digest = DigestUtilities.GetDigest(new DerObjectIdentifier(aDigestAlg));
			try
			{
				return DigestUtilities.DoFinal(digest, aData);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static byte[] FileToByte(Stream stream)
		{
			stream.Seek(0L, SeekOrigin.Begin);
			byte[] array = new byte[32768];
			using (MemoryStream memoryStream = new MemoryStream())
			{
				while (true)
				{
					int num = stream.Read(array, 0, array.Length);
					if (num <= 0)
					{
						break;
					}
					memoryStream.Write(array, 0, num);
				}
				return memoryStream.ToArray();
			}
		}

		public static byte[] FileToByte(string filePath)
		{
			FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			fileStream.Seek(0L, SeekOrigin.Begin);
			byte[] array = new byte[32768];
			using (MemoryStream memoryStream = new MemoryStream())
			{
				while (true)
				{
					int num = fileStream.Read(array, 0, array.Length);
					if (num <= 0)
					{
						break;
					}
					memoryStream.Write(array, 0, num);
				}
				return memoryStream.ToArray();
			}
		}

		public static int SaveDer(CmsSignedData cms, string filename)
		{
			byte[] encoded = cms.GetEncoded();
			try
			{
				FileStream fileStream = new FileStream(filename, FileMode.Create);
				fileStream.Write(encoded, 0, encoded.Length);
				fileStream.Close();
				return 1;
			}
			catch (Exception)
			{
				return -1;
			}
		}

		public static string GetByteToStringHash(byte[] value, IDigest digestAlg)
		{
			string text = null;
			try
			{
				return Convert.ToBase64String(DigestUtilities.DoFinal(digestAlg, value));
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static byte[] GetStringToByteHash(string value, IDigest digestAlg)
		{
			byte[] array = null;
			try
			{
				return DigestUtilities.DoFinal(digestAlg, Encoding.ASCII.GetBytes(value));
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static byte[] GetByteToByteHash(byte[] value, IDigest digestAlg)
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

		public static string GetStrToStrHash(string value, IDigest digestAlg)
		{
			string text = null;
			try
			{
				return Convert.ToBase64String(DigestUtilities.DoFinal(digestAlg, Encoding.ASCII.GetBytes(value)));
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
