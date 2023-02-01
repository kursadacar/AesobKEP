using System;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Math;

namespace Tr.Com.Eimza.Org.BouncyCastle.X509
{
	internal interface IX509AttributeCertificate : IX509Extension
	{
		int Version { get; }

		BigInteger SerialNumber { get; }

		DateTime NotBefore { get; }

		DateTime NotAfter { get; }

		AttributeCertificateHolder Holder { get; }

		AttributeCertificateIssuer Issuer { get; }

		bool IsValidNow { get; }

		X509Attribute[] GetAttributes();

		X509Attribute[] GetAttributes(string oid);

		bool[] GetIssuerUniqueID();

		bool IsValid(DateTime date);

		void CheckValidity();

		void CheckValidity(DateTime date);

		byte[] GetSignature();

		void Verify(AsymmetricKeyParameter publicKey);

		byte[] GetEncoded();
	}
}
