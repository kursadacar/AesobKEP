using System.Collections.Generic;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509
{
	internal sealed class KeyPurposeID : DerObjectIdentifier
	{
		private const string IdKP = "1.3.6.1.5.5.7.3";

		public static readonly KeyPurposeID AnyExtendedKeyUsage;

		public static readonly KeyPurposeID IdKPServerAuth;

		public static readonly KeyPurposeID IdKPClientAuth;

		public static readonly KeyPurposeID IdKPCodeSigning;

		public static readonly KeyPurposeID IdKPEmailProtection;

		public static readonly KeyPurposeID IdKPIpsecEndSystem;

		public static readonly KeyPurposeID IdKPIpsecTunnel;

		public static readonly KeyPurposeID IdKPIpsecUser;

		public static readonly KeyPurposeID IdKPTimeStamping;

		public static readonly KeyPurposeID IdKPOcspSigning;

		public static readonly KeyPurposeID KamuSM_E_Muhur;

		public static readonly Dictionary<DerObjectIdentifier, string> KeyPurposeIds;

		public static readonly KeyPurposeID IdKPSmartCardLogon;

		private KeyPurposeID(string id)
			: base(id)
		{
		}

		static KeyPurposeID()
		{
			AnyExtendedKeyUsage = new KeyPurposeID(X509Extensions.ExtendedKeyUsage.Id + ".0");
			IdKPServerAuth = new KeyPurposeID("1.3.6.1.5.5.7.3.1");
			IdKPClientAuth = new KeyPurposeID("1.3.6.1.5.5.7.3.2");
			IdKPCodeSigning = new KeyPurposeID("1.3.6.1.5.5.7.3.3");
			IdKPEmailProtection = new KeyPurposeID("1.3.6.1.5.5.7.3.4");
			IdKPIpsecEndSystem = new KeyPurposeID("1.3.6.1.5.5.7.3.5");
			IdKPIpsecTunnel = new KeyPurposeID("1.3.6.1.5.5.7.3.6");
			IdKPIpsecUser = new KeyPurposeID("1.3.6.1.5.5.7.3.7");
			IdKPTimeStamping = new KeyPurposeID("1.3.6.1.5.5.7.3.8");
			IdKPOcspSigning = new KeyPurposeID("1.3.6.1.5.5.7.3.9");
			KamuSM_E_Muhur = new KeyPurposeID("2.16.792.1.2.1.1.5.7.50.1");
			KeyPurposeIds = new Dictionary<DerObjectIdentifier, string>();
			IdKPSmartCardLogon = new KeyPurposeID("1.3.6.1.4.1.311.20.2.2");
			KeyPurposeIds.Add(AnyExtendedKeyUsage, "AnyExtended KeyUsage");
			KeyPurposeIds.Add(IdKPServerAuth, "Sunucu Kimlik Doğrulama");
			KeyPurposeIds.Add(IdKPClientAuth, "İstemci Kimlik Doğrulama");
			KeyPurposeIds.Add(IdKPCodeSigning, "Kod İmzalama");
			KeyPurposeIds.Add(IdKPEmailProtection, "E-Posta Koruma");
			KeyPurposeIds.Add(IdKPIpsecEndSystem, "Son Sistem IP Güvenliği");
			KeyPurposeIds.Add(IdKPIpsecTunnel, "IPSec Tüneli");
			KeyPurposeIds.Add(IdKPIpsecUser, "IPSec Kullanıcısı");
			KeyPurposeIds.Add(IdKPTimeStamping, "Zaman Damgalama");
			KeyPurposeIds.Add(IdKPOcspSigning, "OCSP İmzalama");
			KeyPurposeIds.Add(KamuSM_E_Muhur, "KamuSM E-Mühür");
			KeyPurposeIds.Add(IdKPSmartCardLogon, "Microsoft Akıllı Kart ile Giriş");
		}
	}
}
