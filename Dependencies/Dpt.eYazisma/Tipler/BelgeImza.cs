using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dpt.eYazisma.Xsd;

namespace Dpt.eYazisma.Tipler
{
    /// <summary>
    /// BelgeImza bileşeni bilgileri.
    /// </summary>
    public abstract class BelgeImza : IDisposable
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
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
        }
        internal abstract CT_BelgeImza CT_BelgeImza { get; set; }
        /// <summary>
        /// BelgeImza bileşenindeki imzaları döner.
        /// </summary>
        /// <returns>Sıralı <see cref="CT_Imza"/> nesnesi.</returns>
        public abstract CT_Imza[] ImzalariAl();
        /// <summary>
        /// BelgeImza bileşenine hedef ekler.
        /// </summary>
        /// <param name="imza">Eklenecek <see cref="CT_Imza"/> nesnesi.</param>
        public abstract void ImzaEkle(CT_Imza imza);
        internal abstract void KontrolEt();
    }
    internal class BelgeImzaInternal : BelgeImza
    {
        internal override CT_BelgeImza CT_BelgeImza { get; set; }

        public BelgeImzaInternal()
        {
            CT_BelgeImza = new CT_BelgeImza();
        }

        internal BelgeImzaInternal(CT_BelgeImza belgeImza)
        {
            CT_BelgeImza = belgeImza;
        }

        public override CT_Imza[] ImzalariAl()
        {
            return this.CT_BelgeImza.ImzaListesi;
        }
        public override void ImzaEkle(CT_Imza imza)
        {
            if (CT_BelgeImza.ImzaListesi == null)
                CT_BelgeImza.ImzaListesi = new CT_Imza[0];
            List<CT_Imza> L = CT_BelgeImza.ImzaListesi.ToList();
            L.Add(imza);
            CT_BelgeImza.ImzaListesi = L.ToArray();
        }
        internal override void KontrolEt()
        {
            this.CT_BelgeImza.KontrolEt();
        }

    }
}
