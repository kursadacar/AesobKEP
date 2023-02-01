using System;
using System.Collections.Generic;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.OID;
using Tr.Com.Eimza.Org.BouncyCastle.Shared;
using Tr.Com.Eimza.Org.BouncyCastle.Tools;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Encoders;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ext
{
	internal class EImzaProfilleriDocInfo : ProfileDocInfo
	{
		public static readonly List<Pair<DerObjectIdentifier, byte[]>> hashes;

		public const string URI = "http://www.tk.gov.tr/bilgi_teknolojileri/elektronik_imza/dosyalar/Elektronik_Imza_Kullanim_Profilleri_Rehberi.pdf";

		static EImzaProfilleriDocInfo()
		{
			hashes = new List<Pair<DerObjectIdentifier, byte[]>>();
			hashes.Add(new Pair<DerObjectIdentifier, byte[]>(DigestAlgorithms.SHA1_OID, Hex.Decode("7E3B50D9020C676BBBF07A998E75E63734909CD8")));
			hashes.Add(new Pair<DerObjectIdentifier, byte[]>(DigestAlgorithms.SHA224_OID, Hex.Decode("B6600FE76A7E1973367382D81F224620242A1957595C20C882A5EC8F")));
			hashes.Add(new Pair<DerObjectIdentifier, byte[]>(DigestAlgorithms.SHA256_OID, Hex.Decode("FF39BD29463383F69B2052AC47439E06CE7C3B8646E888B6E5AE3E46BA08117A")));
			hashes.Add(new Pair<DerObjectIdentifier, byte[]>(DigestAlgorithms.SHA384_OID, Hex.Decode("E7503CF83D21EB179C3A89FDE8BF2216E5F4F24C9DA9752D8FE86C8A8B71D4BD58A1EF426B8B0071B8C1D0754C71A810")));
			hashes.Add(new Pair<DerObjectIdentifier, byte[]>(DigestAlgorithms.SHA512_OID, Hex.Decode("AF925EEE76562989CD5DA4000DA2C35F3D9E95BC6604BB13FE3924A6E223914756BF54FCE4CCFD2617DE906EA135B9474CCB1DA83F8468C2DB18341EC7552EDE")));
		}

		public byte[] GetDigestOfProfile(DerObjectIdentifier oid)
		{
			foreach (Pair<DerObjectIdentifier, byte[]> hash in hashes)
			{
				if (hash.First.Id.Equals(oid.Id))
				{
					return hash.Second;
				}
			}
			return null;
		}

		public Stream GetProfile()
		{
			MyWebClient myWebClient = new MyWebClient
			{
				UseDefaultCredentials = true
			};
			Stream stream = null;
			try
			{
				return myWebClient.OpenRead("http://www.tk.gov.tr/bilgi_teknolojileri/elektronik_imza/dosyalar/Elektronik_Imza_Kullanim_Profilleri_Rehberi.pdf");
			}
			catch (Exception e)
			{
				throw new CmsException("Problem at reading profile document.", e);
			}
		}

		public string GetURI()
		{
			return "http://www.tk.gov.tr/bilgi_teknolojileri/elektronik_imza/dosyalar/Elektronik_Imza_Kullanim_Profilleri_Rehberi.pdf";
		}

		public byte[] GetDigestOfProfile(string oid)
		{
			return GetDigestOfProfile(new DerObjectIdentifier(oid));
		}
	}
}
