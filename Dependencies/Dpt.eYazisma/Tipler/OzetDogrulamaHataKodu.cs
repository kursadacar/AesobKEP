using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dpt.eYazisma.Tipler
{
    /// <summary>
    /// Paket Özeti bileşeni doğrulanması sırasında oluşan hatalara ilişkin kod değerleri.
    /// </summary>
    public enum OzetDogrulamaHataKodu
    {
        /// <summary>Ozet değeri verilmemiş.</summary>
        OZET_DEGERI_VERILMEMIS = 1,
        /// <summary>Ustveri bileşeni yok.</summary>
        USTVERI_BILESENI_YOK = 2,
        /// <summary>PaketOzet'inde Ustveri ozet değeri yok.</summary>
        PAKETOZET_INDE_USTVERI_OZET_DEGERI_YOK = 3,
        /// <summary>BelgeHedef bileşeni yok.</summary>
        BELGEHEDEF_BILESENI_YOK = 4,
        /// <summary>PaketOzet'inde BelgeHedef ozet değeri yok.</summary>
        PAKETOZET_INDE_BELGEHEDEF_OZET_DEGERI_YOK = 5,
        /// <summary>UstYazi bileşeni yok.</summary>
        USTYAZI_BILESENI_YOK = 6,
        /// <summary>PaketOzet'inde UstYazi ozet değeri yok.</summary>
        PAKETOZET_INDE_USTYAZI_OZET_DEGERI_YOK = 7,
        /// <summary>Reference değeri verilmemiş.</summary>
        REFERENCE_DEGERI_VERILMEMIS = 8,
        /// <summary>URI değeri boş.</summary>
        URI_DEGERI_BOS = 9,
        /// <summary>DigestMethod değeri boş.</summary>
        DIGESTMETHOD_DEGERI_BOS = 10,
        /// <summary>Algorithm değeri boş.</summary>
        ALGORITHM_DEGERI_BOS = 11,
        /// <summary>Desteklenmeyen OzetModu.</summary>
        DESTEKLENMEYEN_OZETMODU = 12,
        /// <summary>Paket bileşeni alınamadı.</summary>
        PAKET_BILESENI_ALINAMADI = 13,
        /// <summary>Paket bileşenine ait hash hesaplanamadı.</summary>
        PAKET_BILESENINE_AIT_HASH_HESAPLANAMADI = 14,
        /// <summary>Hashler eşit değil.</summary>
        HASHLER_ESIT_DEGIL = 15,
        /// <summary>BelgeImza bileşeni yok.</summary>
        BELGEIMZA_BILESENI_YOK = 16,
        /// <summary>Imza bileşeni yok.</summary>
        IMZA_BILESENI_YOK = 17,
        /// <summary>Core bileşeni yok.</summary>
        CORE_BILESENI_YOK = 18,
        /// <summary>NihaiOzet'te Ustveri ozet değeri yok.</summary>
        NIHAIOZET_TE_USTVERI_OZET_DEGERI_YOK = 19,
        /// <summary>NihaiOzet'te BelgeHedef ozet değeri yok.</summary>
        NIHAIOZET_TE_BELGEHEDEF_OZET_DEGERI_YOK = 20,
        /// <summary>NihaiOzet'te UstYazi ozet değeri yok.</summary>
        NIHAIOZET_TE_USTYAZI_OZET_DEGERI_YOK = 21,
        /// <summary>NihaiOzet'te UstYazi ozet değeri yok.</summary>
        NIHAIOZET_TE_BELGEIMZA_OZET_DEGERI_YOK = 22,
        /// <summary>NihaiOzet'te UstYazi ozet değeri yok.</summary>
        NIHAIOZET_TE_IMZA_OZET_DEGERI_YOK = 23,
        /// <summary>NihaiOzet'te UstYazi ozet değeri yok.</summary>
        NIHAIOZET_TE_CORE_OZET_DEGERI_YOK = 24,
        /// <summary>PaketOzeti bileşeni yok.</summary>
        PAKETOZETI_BILESENI_YOK = 25,
    }
}
