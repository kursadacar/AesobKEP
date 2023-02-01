using System.Security.Cryptography.X509Certificates;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.EYazisma.SmartCard
{
    public class EYazismaCertificate
	{
		public EYazismaSmartCard SmartCard { get; set; }

		public string SertifikaSahibi { get; set; }

		public string SubjectName { get; set; }

		public string SerialNumber { get; set; }

		public string SertifikaYayinlayici { get; set; }

		public string IssuerName { get; set; }

		public X509Certificate2 SignerCertificate { get; set; }

		public int CertificateIndex { get; set; }

		public int SmartCardIndex { get; set; }

		public string EshsName { get; set; }

		internal EYazismaCertificate(int Index, Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate x509Certificate, EYazismaSmartCard smartCard)
		{
			SignerCertificate = new X509Certificate2(DotNetUtilities.ToX509Certificate(x509Certificate));
			EshsName = x509Certificate.EshsName;
			SertifikaSahibi = SignerCertificate.GetNameInfo(X509NameType.SimpleName, false);
			SubjectName = SignerCertificate.SubjectName.Name;
			SertifikaYayinlayici = SignerCertificate.GetNameInfo(X509NameType.SimpleName, true);
			IssuerName = SignerCertificate.IssuerName.Name;
			SerialNumber = SignerCertificate.SerialNumber;
			CertificateIndex = Index;
			SmartCard = smartCard;
			SmartCardIndex = smartCard.SmartCardIndex;
		}

		internal EYazismaCertificate()
		{
			CertificateIndex = 0;
			SmartCardIndex = 0;
			SmartCard = new EYazismaSmartCard(0, "Default SmartCard");
		}
	}
}
