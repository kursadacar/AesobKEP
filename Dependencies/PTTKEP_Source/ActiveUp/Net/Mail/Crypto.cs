using System;
using System.Security.Cryptography;
using System.Text;

namespace ActiveUp.Net.Mail
{
	public abstract class Crypto
	{
		public static string MD5Digest(string data)
		{
			return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(data))).ToLower().Replace("-", "");
		}

		public static string HMACMD5Digest(string key, string data)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			if (key.Length > 64)
			{
				key = MD5Digest(key);
			}
			key = key.PadRight(64, '\0');
			byte[] bytes = Encoding.ASCII.GetBytes(key);
			for (int i = 0; i < 64; i++)
			{
				bytes[i] ^= 54;
			}
			byte[] bytes2 = Encoding.ASCII.GetBytes(data);
			byte[] array = new byte[bytes2.Length + 64];
			for (int j = 0; j < 64; j++)
			{
				array[j] = bytes[j];
			}
			for (int k = 64; k < array.Length; k++)
			{
				array[k] = bytes2[k - 64];
			}
			byte[] array2 = mD5CryptoServiceProvider.ComputeHash(array);
			bytes = Encoding.ASCII.GetBytes(key);
			for (int l = 0; l < 64; l++)
			{
				bytes[l] ^= 92;
			}
			byte[] array3 = new byte[array2.Length + 64];
			for (int m = 0; m < 64; m++)
			{
				array3[m] = bytes[m];
			}
			for (int n = 64; n < array3.Length; n++)
			{
				array3[n] = array2[n - 64];
			}
			return BitConverter.ToString(mD5CryptoServiceProvider.ComputeHash(array3)).ToLower().Replace("-", "");
		}
	}
}
