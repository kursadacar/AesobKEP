using System.Collections.Generic;
using Tr.Com.Eimza.EYazisma;
//using Tr.Com.Eimza.EYazisma;

namespace Aesob.KEP.Model
{
    public class PackageData
    {
        public int? Durum { get; set; }

        public string KepId { get; set; }

        public int? KepSiraNo { get; set; }

        public string From { get; set; }

        public string FromKep { get; set; }

        public string Konu { get; set; }

        public string Tur { get; set; }

        public string HataAciklama { get; set; }

        public string OrjinalMesajId { get; set; }

        public static List<PackageData> CreateFrom(EyPaketSonuc paketsonuc)
        {
            var list = new List<PackageData>();

            if (paketsonuc == null || paketsonuc.KepId == null || paketsonuc.OrjinalMesajId == null)
            {
                return list;
            }

            for (int i = 0; i < paketsonuc.Durum.Length; i++)
            {
                var newPaketData = new PackageData();

                newPaketData.Durum = paketsonuc.Durum[i];
                newPaketData.KepId = paketsonuc.KepId[i];
                newPaketData.KepSiraNo = paketsonuc.KepSiraNo[i];
                newPaketData.From = paketsonuc.From[i];
                newPaketData.FromKep = paketsonuc.FromKep[i];
                newPaketData.Konu = paketsonuc.Konu[i];
                newPaketData.Tur = paketsonuc.Tur[i];
                newPaketData.HataAciklama = paketsonuc.HataAciklama[i];
                newPaketData.OrjinalMesajId = paketsonuc.OrjinalMesajId[i];

                list.Add(newPaketData);
            }

            return list;
        }
    }
}
