using System.Collections.Generic;
using System.Linq;

namespace Tr.Com.Eimza.EYazisma
{
	public class EyDizinSonuc
	{
		public List<string> Dizinler { get; set; }

		public int? Durum { get; set; }

		public string HataAciklama { get; set; }

		internal static EyDizinSonuc GetEyDizinSonuc(string[] eySonuc)
		{
			if (eySonuc != null)
			{
				return new EyDizinSonuc
				{
					Durum = 0,
					HataAciklama = "",
					Dizinler = eySonuc.ToList()
				};
			}
			return new EyDizinSonuc
			{
				Durum = -1,
				HataAciklama = "Dizin Bulunamadı.",
				Dizinler = null
			};
		}
	}
}
