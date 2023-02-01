using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ActiveUp.Net.Mail;

namespace ActiveUp.Net.Security
{
	[Serializable]
	public class Signature
	{
		private Message _signedMessage;

		private byte[] _b;

		private string _a = "rsa-sha1";

		private string _b64;

		private string _d;

		private string _s;

		private QueryMethod _q = QueryMethod.Dns;

		private CanonicalizationAlgorithm _c;

		private string[] _h;

		public string Algorithm
		{
			get
			{
				return _a;
			}
			set
			{
				_a = value;
			}
		}

		public byte[] Data
		{
			get
			{
				return _b;
			}
			set
			{
				_b = value;
			}
		}

		public string DataBase64
		{
			get
			{
				return _b64;
			}
			set
			{
				_b64 = value;
			}
		}

		public CanonicalizationAlgorithm CanonicalizationAlgorithm
		{
			get
			{
				return _c;
			}
			set
			{
				_c = value;
			}
		}

		public QueryMethod QueryMethod
		{
			get
			{
				return _q;
			}
			set
			{
				_q = value;
			}
		}

		public string[] SignedHeaders
		{
			get
			{
				return _h;
			}
			set
			{
				_h = value;
			}
		}

		public string Domain
		{
			get
			{
				return _d;
			}
			set
			{
				_d = value;
			}
		}

		public string Selector
		{
			get
			{
				return _s;
			}
			set
			{
				_s = value;
			}
		}

		private Signature()
		{
		}

		public Signature(Message signedMessage)
		{
			_signedMessage = signedMessage;
		}

		public static Signature Parse(string input)
		{
			return Parse(input, null);
		}

		public static Signature Parse(string input, Message signedMessage)
		{
			Signature signature = ((signedMessage != null) ? new Signature(signedMessage) : new Signature());
			MatchCollection matchCollection = Regex.Matches(input, "[a-zA-Z]+=[^;]+(?=(;|\\Z))");
			Logger.AddEntry(matchCollection.Count.ToString());
			foreach (Match item in matchCollection)
			{
				string text = item.Value.Substring(0, item.Value.IndexOf('='));
				string text2 = item.Value.Substring(item.Value.IndexOf('=') + 1).Split(',')[0];
				if (text.Equals("a"))
				{
					signature._a = text2;
				}
				else if (text.Equals("b"))
				{
					text2 = text2.Trim('\r', '\n').Replace(" ", "");
					signature._b64 = text2.Replace(" ", "").Replace("\t", "").Replace("\r\n", "");
					signature._b = Convert.FromBase64String(signature._b64);
				}
				else if (text.Equals("c"))
				{
					if (text2.Equals("nofws"))
					{
						signature._c = CanonicalizationAlgorithm.NoFws;
					}
					else if (text2.Equals("simple"))
					{
						signature._c = CanonicalizationAlgorithm.Simple;
					}
					else
					{
						signature._c = CanonicalizationAlgorithm.Other;
					}
				}
				else if (text.Equals("d"))
				{
					signature._d = text2;
				}
				else if (text.Equals("s"))
				{
					signature._s = text2;
				}
				else if (text.Equals("q"))
				{
					signature._q = (text2.Equals("dns") ? QueryMethod.Dns : QueryMethod.Other);
				}
				else if (text.Equals("h"))
				{
					signature._h = text2.Split(':');
				}
			}
			return signature;
		}

		private static string SelectFieldsAndCanonicalize(string header, Signature signature)
		{
			int startIndex = header.ToLower().IndexOf("\ndomainkey-signature");
			header = header.Substring(startIndex);
			StringBuilder stringBuilder = new StringBuilder();
			header = Parser.Unfold(header);
			string[] array = Regex.Split(header, "\r?\n");
			if (signature.SignedHeaders.Length != 0)
			{
				string[] signedHeaders = signature.SignedHeaders;
				foreach (string value in signedHeaders)
				{
					string[] array2 = array;
					foreach (string text in array2)
					{
						if (text.ToLower().StartsWith(value))
						{
							stringBuilder.Append(Canonicalizer.Canonicalize(text, signature.CanonicalizationAlgorithm) + Tokenizer.NewLine);
						}
					}
				}
			}
			else
			{
				bool flag = false;
				string[] signedHeaders = array;
				foreach (string text2 in signedHeaders)
				{
					if (!flag)
					{
						flag = true;
					}
					else
					{
						stringBuilder.Append(text2 + Tokenizer.NewLine);
					}
				}
			}
			return stringBuilder.ToString().TrimEnd('\r', '\n') + "\r\n";
		}

		public static bool Verify(byte[] emailData, Signature signature)
		{
			string @string = Encoding.ASCII.GetString(emailData, 0, emailData.Length);
			string input = @string.Substring(0, @string.IndexOf("\r\n\r\n"));
			input = Parser.Unfold(input);
			input = SelectFieldsAndCanonicalize(input, signature);
			StringReader stringReader = new StringReader(@string.Substring(Regex.Match(@string, "(?<=\\r?\\n\\r?\\n).").Index - 1));
			StringBuilder stringBuilder = new StringBuilder();
			while (stringReader.Peek() != -1)
			{
				stringBuilder.Append(Canonicalizer.Canonicalize(stringReader.ReadLine(), signature.CanonicalizationAlgorithm) + Tokenizer.NewLine);
			}
			byte[] bytes = Encoding.ASCII.GetBytes(input + stringBuilder.ToString().TrimEnd('\r', '\n') + "\r\n");
			byte[] rgbHash = new SHA1Managed().ComputeHash(bytes);
			PublicKeyRecord publicKeyRecord = signature.GetPublicKeyRecord();
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			RSAParameters param = default(RSAParameters);
			FillRSAPublicKeyParameters(publicKeyRecord.KeyData, ref param);
			rSACryptoServiceProvider.ImportParameters(param);
			RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureDeformatter.SetHashAlgorithm("SHA1");
			return rSAPKCS1SignatureDeformatter.VerifySignature(rgbHash, signature.Data);
		}

		public PublicKeyRecord GetPublicKeyRecord()
		{
			byte[] txtRecords = Validator.GetTxtRecords(Selector + "._domainkey." + Domain);
			return PublicKeyRecord.Parse(Encoding.ASCII.GetString(txtRecords, 0, txtRecords.Length));
		}

		public SendingDomainPolicy GetSendingDomainPolicy()
		{
			byte[] txtRecords = Validator.GetTxtRecords("._domainkey." + Domain);
			return SendingDomainPolicy.Parse(Encoding.ASCII.GetString(txtRecords, 0, txtRecords.Length));
		}

		public bool Verify()
		{
			if (_signedMessage == null)
			{
				throw new NotSupportedException("The signature must me associated with a message in order to be verified");
			}
			return Verify(_signedMessage.OriginalData, _signedMessage.Signatures.DomainKeys);
		}

		private static void FillRSAPublicKeyParameters(byte[] key, ref RSAParameters param)
		{
			int num = 0;
			int num2 = 0;
			if (key[num] != 48)
			{
				throw new FormatException("Main sequence OID tag not found");
			}
			num++;
			GetContentLength(key, ref num);
			if (key[num] != 48)
			{
				throw new FormatException("RSA OID sequence not found");
			}
			num++;
			num2 = GetContentLength(key, ref num);
			num += num2;
			if (key[num] != 3)
			{
				throw new FormatException("Bit String OID tag not found");
			}
			num++;
			GetContentLength(key, ref num);
			num++;
			if (key[num] != 48)
			{
				throw new FormatException("Public key Sequence OID tag not found");
			}
			num++;
			GetContentLength(key, ref num);
			if (key[num] != 2)
			{
				throw new FormatException("\"n\" Integer OID tag not found");
			}
			num++;
			CopyContent(key, ref num, ref param.Modulus);
			if (key[num] != 2)
			{
				throw new FormatException("\"e\" Integer OID tag not found");
			}
			num++;
			CopyContent(key, ref num, ref param.Exponent);
		}

		private static int GetContentLength(byte[] key, ref int lengthStartIndex)
		{
			int num = 0;
			if (key[lengthStartIndex] == 128)
			{
				throw new FormatException("Indefinite length encoding not supported.");
			}
			if (key[lengthStartIndex] > 128)
			{
				int num2 = key[lengthStartIndex] - 128;
				lengthStartIndex++;
				while (num2 > 0)
				{
					num += key[lengthStartIndex] << 8 * (num2 - 1);
					lengthStartIndex++;
					num2--;
				}
			}
			else
			{
				num = key[lengthStartIndex];
				lengthStartIndex++;
			}
			return num;
		}

		private static void CopyContent(byte[] source, ref int startIndex, ref byte[] destination)
		{
			int num = GetContentLength(source, ref startIndex);
			if (source[startIndex] == 0)
			{
				startIndex++;
				num--;
			}
			destination = new byte[num];
			Array.Copy(source, startIndex, destination, 0, num);
			startIndex += num;
		}
	}
}
