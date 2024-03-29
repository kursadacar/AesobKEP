using System;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Pkcs
{
	internal class Pkcs12Utilities
	{
		public static byte[] ConvertToDefiniteLength(byte[] berPkcs12File)
		{
			return new Pfx(Asn1Sequence.GetInstance(Asn1Object.FromByteArray(berPkcs12File))).GetEncoded("DER");
		}

		public static byte[] ConvertToDefiniteLength(byte[] berPkcs12File, char[] passwd)
		{
			Pfx pfx = new Pfx(Asn1Sequence.GetInstance(Asn1Object.FromByteArray(berPkcs12File)));
			ContentInfo authSafe = pfx.AuthSafe;
			authSafe = new ContentInfo(content: new DerOctetString(Asn1Object.FromByteArray(Asn1OctetString.GetInstance(authSafe.Content).GetOctets()).GetEncoded("DER")), contentType: authSafe.ContentType);
			MacData macData = pfx.MacData;
			try
			{
				int intValue = macData.IterationCount.IntValue;
				byte[] octets = Asn1OctetString.GetInstance(authSafe.Content).GetOctets();
				byte[] digest = Pkcs12Store.CalculatePbeMac(macData.Mac.AlgorithmID.ObjectID, macData.GetSalt(), intValue, passwd, false, octets);
				macData = new MacData(new DigestInfo(new AlgorithmIdentifier(macData.Mac.AlgorithmID.ObjectID, DerNull.Instance), digest), macData.GetSalt(), intValue);
			}
			catch (Exception ex)
			{
				throw new IOException("error constructing MAC: " + ex.ToString());
			}
			return new Pfx(authSafe, macData).GetEncoded("DER");
		}
	}
}
