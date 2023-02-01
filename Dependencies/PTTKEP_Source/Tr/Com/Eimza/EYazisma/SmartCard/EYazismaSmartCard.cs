using System.Collections.Generic;

namespace Tr.Com.Eimza.EYazisma.SmartCard
{
	public class EYazismaSmartCard
	{
		public int SmartCardIndex { get; set; }

		public string SmartCardName { get; set; }

		public List<EYazismaCertificate> CertificateList { get; set; }

		public EYazismaSmartCard(int index, string name)
		{
			CertificateList = new List<EYazismaCertificate>();
			SmartCardIndex = index;
			SmartCardName = name;
		}
	}
}
