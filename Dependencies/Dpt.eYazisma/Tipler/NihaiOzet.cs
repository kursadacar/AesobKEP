using System;
using Dpt.eYazisma.Xsd;
using System.Linq;
using System.Collections.Generic;
namespace Dpt.eYazisma.Tipler
{
    /// <summary>
    /// NihaiOzet bileşeni bilgileri.
    /// </summary>
    public abstract class NihaiOzet : IDisposable
    {
        /// <summary>
        /// Kullanılan kaynakları kapatır.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Kullanılan kaynakları kapatır.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
        }
        internal abstract CT_NihaiOzet CT_NihaiOzet { get; set; }
        /// <summary>
        /// NihaiOzet bileşenine özet ekler.
        /// </summary>
        /// <param name="ozetModu">Eklenen özetin hangi algoritma ile alındığı bilgisi.</param>
        /// <param name="ozetDegeri">Eklenecek özet değeri.</param>
        /// <param name="uri">Eklenen özet değerine ilişkin bileşenin paket içindeki URI'si.</param>
        public abstract void Ekle(OzetModu ozetModu, Byte[] ozetDegeri, Uri uri);
        /// <summary>
        /// NihaiOzet bileşenindeki özetleri döner.
        /// </summary>
        /// <returns>Sıralı <see cref="CT_Reference"/> nesnesi.</returns>
        public abstract CT_Reference[] OzetleriAl();
        internal abstract void KontrolEt();
    }

    class NihaiOzetInternal : NihaiOzet
    {
        internal override CT_NihaiOzet CT_NihaiOzet { get; set; }

        public NihaiOzetInternal()
        {
            CT_NihaiOzet = new CT_NihaiOzet();
        }
        internal NihaiOzetInternal(CT_NihaiOzet paketOzeti)
        {
            CT_NihaiOzet = paketOzeti;
        }

        public override void Ekle(OzetModu ozetModu, Byte[] ozetDegeri, Uri uri)
        {
            if (ozetModu == OzetModu.Yok)
                throw new ArgumentException("\"ozetModu\" değeri \"YOK\" olamaz.");
            if (CT_NihaiOzet.Reference == null)
                CT_NihaiOzet.Reference = new CT_Reference[0];
            List<CT_Reference> referanslar = CT_NihaiOzet.Reference.ToList();
            var dahaOncekiOzetler = referanslar.Where(x => string.Compare(x.URI, uri.ToString(), StringComparison.InvariantCultureIgnoreCase) == 0);
            if (dahaOncekiOzetler.Count() > 0)
                referanslar.Remove(dahaOncekiOzetler.First());
            var yeniReferans = new CT_Reference
                        {
                            DigestMethod = new CT_DigestMethod() { Algorithm = Araclar.OzetModuToString(ozetModu) },
                            DigestValue = ozetDegeri,
                            URI = uri.ToString()
                        };
            yeniReferans.Type = Araclar.DAHILI_PAKET_BILESENI_REFERANS_TIPI;
            referanslar.Add(yeniReferans);
            CT_NihaiOzet.Reference = referanslar.ToArray();
        }
        public override CT_Reference[] OzetleriAl()
        {
            return this.CT_NihaiOzet.Reference;
        }
        internal override void KontrolEt()
        {
            this.CT_NihaiOzet.KontrolEt();
        }
    }
}
