using System;
using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class ServerNameList
	{
		protected readonly IList mServerNameList;

		public virtual IList ServerNames
		{
			get
			{
				return mServerNameList;
			}
		}

		public ServerNameList(IList serverNameList)
		{
			if (serverNameList == null || serverNameList.Count < 1)
			{
				throw new ArgumentException("must not be null or empty", "serverNameList");
			}
			mServerNameList = serverNameList;
		}

		public virtual void Encode(Stream output)
		{
			MemoryStream memoryStream = new MemoryStream();
			foreach (ServerName serverName in ServerNames)
			{
				serverName.Encode(memoryStream);
			}
			TlsUtilities.CheckUint16(memoryStream.Length);
			TlsUtilities.WriteUint16((int)memoryStream.Length, output);
			memoryStream.WriteTo(output);
		}

		public static ServerNameList Parse(Stream input)
		{
			int num = TlsUtilities.ReadUint16(input);
			if (num < 1)
			{
				throw new TlsFatalAlert(50);
			}
			MemoryStream memoryStream = new MemoryStream(TlsUtilities.ReadFully(num, input), false);
			IList list = Platform.CreateArrayList();
			while (memoryStream.Position < memoryStream.Length)
			{
				ServerName value = ServerName.Parse(memoryStream);
				list.Add(value);
			}
			return new ServerNameList(list);
		}
	}
}
