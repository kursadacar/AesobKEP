using System.Collections.Generic;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace Tr.Com.Eimza.EYazisma
{
	public class EyDonusBarkod
	{
		public string Alici { get; set; }

		public string Barkod { get; set; }

		internal static List<EyDonusBarkod> GetEyDonusBarkod(eyDonusBarkod[] eySonuc)
		{
			if (eySonuc != null)
			{
				List<EyDonusBarkod> list = new List<EyDonusBarkod>();
				for (int i = 0; i < eySonuc.Length; i++)
				{
					EyDonusBarkod eyDonusBarkod = new EyDonusBarkod();
					eyDonusBarkod.Alici = eyDonusBarkod.Alici;
					eyDonusBarkod.Barkod = eyDonusBarkod.Barkod;
					list.Add(eyDonusBarkod);
				}
				return list;
			}
			return null;
		}

		public static List<eyDonusBarkod> ConvertToWebServiceSonuc(List<EyDonusBarkod> eySonuc)
		{
			if (eySonuc != null)
			{
				List<eyDonusBarkod> list = new List<eyDonusBarkod>();
				for (int i = 0; i < eySonuc.Count; i++)
				{
					eyDonusBarkod eyDonusBarkod = new eyDonusBarkod();
					eyDonusBarkod.Alici = eyDonusBarkod.Alici;
					eyDonusBarkod.Barkod = eyDonusBarkod.Barkod;
					list.Add(eyDonusBarkod);
				}
				return list;
			}
			return null;
		}
	}
}
