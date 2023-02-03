using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tr.Com.Eimza.EYazisma;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace KepStandalone
{
    public class PackageDownloadResult
    {
        public PackageData OriginalPackage { get; private set; }
        public int Durum { get; set; }

        public base64Binary[] EyazismaPaketi { get; set; }

        public string HataAciklama { get; set; }

        public static PackageDownloadResult CreateFrom(EyPaketIndirSonuc paketIndirSonuc, PackageData originalPackage)
        {
            return new PackageDownloadResult()
            {
                OriginalPackage = originalPackage,
                Durum = paketIndirSonuc.Durum,
                EyazismaPaketi = paketIndirSonuc.EyazismaPaketi,
                HataAciklama = paketIndirSonuc.HataAciklama
            };
        }
    }
}
