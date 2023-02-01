using System;
using System.Collections;
using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal abstract class TlsSrpUtilities
	{
		public static void AddSrpExtension(IDictionary extensions, byte[] identity)
		{
			extensions[12] = CreateSrpExtension(identity);
		}

		public static byte[] GetSrpExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 12);
			if (extensionData != null)
			{
				return ReadSrpExtension(extensionData);
			}
			return null;
		}

		public static byte[] CreateSrpExtension(byte[] identity)
		{
			if (identity == null)
			{
				throw new TlsFatalAlert(80);
			}
			return TlsUtilities.EncodeOpaque8(identity);
		}

		public static byte[] ReadSrpExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			MemoryStream memoryStream = new MemoryStream(extensionData, false);
			byte[] result = TlsUtilities.ReadOpaque8(memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			return result;
		}
	}
}
