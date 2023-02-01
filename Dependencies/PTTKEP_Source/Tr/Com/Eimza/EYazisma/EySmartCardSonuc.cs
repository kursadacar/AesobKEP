using System.Collections.Generic;
using Tr.Com.Eimza.EYazisma.SmartCard;

namespace Tr.Com.Eimza.EYazisma
{
	public class EySmartCardSonuc
	{
		public List<EYazismaSmartCard> SmartCardList { get; set; }

		internal List<EYazismaCertificate> CertificateList { get; set; }
	}
}
