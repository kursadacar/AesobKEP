using System;
using System.IO.Packaging;
using System.IO;
using Dpt.eYazisma.Xsd;
using System.Xml.Serialization;
using System.Linq;
using System.Xml;
using System.Text;
using System.Collections.Generic;

namespace Dpt.eYazisma.Tipler
{
    /// <summary>
    ///     e-Yazışma paketini tanımlar. Paket oluşturma ve açma işlemleri bu sınıf ile yapılır.
    /// </summary>
    public class SifreliPaket : IDisposable
    {
        internal Package _package;
        internal PaketModu _paketModu;
        internal bool _sifreliIcerikEklenmisMi;
        private Stream _streamInternal;
        /// <summary>Paketin iletileceği hedefleri barındıran objeye ulaşılır. </summary>
        /// <value> BelgeHedef nesnesi.</value>
        public BelgeHedef BelgeHedef { get; private set; }
        /// <summary>Paketin içerisindeki şifreli içeriğe ilişkin bilgileri içeren nesneye ulaşılır.</summary>
        /// <value> SifreliIcerikBilgisi nesnesi.</value>
        public SifreliIcerikBilgisi SifreliIcerikBilgisi { get; private set; }
        /// <summary>Paket içerisinde imzalanan bileşenlere ait özet bilgilerinin bulunduğu objeye ulaşılır.</summary>
        /// <value> PaketOzeti nesnesi. </value>
        public PaketOzeti PaketOzeti { get; private set; }
        /// <summary>
        /// Contructor.
        /// </summary>
        protected SifreliPaket()
        {

        }
        #region Aç Kapat
        /// <summary>Yeni bir paket oluşturmak, var olan bir paketi açmak veya güncellemek için kullanılır.</summary>
        /// <exception cref="Exception">    Güncelleme veya açma modunda e-Yazışma standartlarına uygun bir paket olmaması durumunda EXCEPTION oluşur.</exception>
        /// <param name="stream">       Pakete ilişkin STREAM objesidir. </param>
        /// <param name="paketModu">    Paketin açılma, oluşturma veya güncelleme amacıyla açıldığını belirtir. </param>
        /// <returns>  İşlem yapılan paket objesi. </returns>
        /// <example>  Şifreli bir paket oluşturma örneği.
        /// <code>
        /// static void SifreliPaketOlustur(string olusturulacakPaketDosyasiYolu, string sifrelenecekIcPaketDosyaYolu)
        /// {
        ///     using (SifreliPaket sifreliPaket = SifreliPaket.Ac(olusturulacakPaketDosyasiYolu, PaketModu.Olustur))
        ///     {
        ///         using (Paket paket = Paket.Ac(sifrelenecekIcPaketDosyaYolu, PaketModu.Ac))
        ///         {
        ///             sifreliPaket.PaketOzetiEkle(paket.PaketOzetiAl());
        ///             sifreliPaket.BelgeHedefEkle(paket.BelgeHedefiAl());
        ///             paket.Kapat();
        ///         }
        ///         String tempfile = System.IO.Path.GetTempFileName();
        ///         System.IO.File.WriteAllBytes(tempfile, PaketSifrele(sifrelenecekIcPaketDosyaYolu));
        ///         using (Stream s = File.OpenRead(tempfile))
        ///         {
        ///             sifreliPaket.SifreliIcerikEkle(s, Guid.NewGuid());
        ///         }
        ///         sifreliPaket.SifreliIcerikBilgisiOlustur();
        ///         sifreliPaket.Kapat(Guid.NewGuid(), null, null);
        ///     }
        /// }
        /// </code>
        /// </example>
        public static SifreliPaket Ac(Stream stream, PaketModu paketModu)
        {
            switch (paketModu)
            {
                case PaketModu.Guncelle:
                    var paket = new SifreliPaket
                    {
                        _package = Package.Open(stream, FileMode.Open, FileAccess.ReadWrite)
                    };
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEHEDEF).Count() == 1)
                    {
                        try
                        {
                            CT_BelgeHedef readedBelgeHedef = (CT_BelgeHedef)(new XmlSerializer(typeof(CT_Ustveri))).Deserialize(paket._package.GetPart(PackUriHelper.CreatePartUri(paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEHEDEF).First().TargetUri)).GetStream(FileMode.Open));
                            paket.BelgeHedef = new BelgeHedefInternal(readedBelgeHedef);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Geçersiz e-Yazışma paketi. \"BelgeHedef\" bileşeni hatalı.", ex);
                        }

                    }
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_SIFRELIICERIKBILGISI).Count() == 1)
                    {
                        try
                        {
                            CT_SifreliIcerikBilgisi readedSifreliIcerikBilgisi = (CT_SifreliIcerikBilgisi)(new XmlSerializer(typeof(CT_SifreliIcerikBilgisi))).Deserialize(paket._package.GetPart(PackUriHelper.CreatePartUri(paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_SIFRELIICERIKBILGISI).First().TargetUri)).GetStream(FileMode.Open));
                            paket.SifreliIcerikBilgisi = new SifreliIcerikBilgisiInternal(readedSifreliIcerikBilgisi);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Geçersiz e-Yazışma paketi. \"SifreliIcerikBilgisi\" bileşeni hatalı.", ex);
                        }

                    }
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PAKETOZETI).Count() == 1)
                    {
                        try
                        {
                            CT_PaketOzeti readedPaketOzeti = (CT_PaketOzeti)(new XmlSerializer(typeof(CT_PaketOzeti))).Deserialize(paket._package.GetPart(PackUriHelper.CreatePartUri(paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PAKETOZETI).First().TargetUri)).GetStream(FileMode.Open));
                            paket.PaketOzeti = new PaketOzetiInternal(readedPaketOzeti);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Geçersiz e-Yazışma paketi. \"PaketÖzeti\" bileşeni hatalı.", ex);
                        }
                    }
                    paket._paketModu = paketModu;
                    paket._streamInternal = stream;
                    return paket;
                case PaketModu.Ac:
                    var paketAcilan = new SifreliPaket
                    {
                        _package = Package.Open(stream, FileMode.Open, FileAccess.Read)
                    };

                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEHEDEF).Count() != 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"BelgeHedef\" bileşeni yok veya birden fazla.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_CORE).Count() != 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"Core\" bileşeni yok veya birden fazla.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PAKETOZETI).Count() != 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"PaketÖzeti\" bileşeni yok veya birden fazla.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_SIFRELIICERIKBILGISI).Count() != 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"SifreliIcerikBilgisi\" bileşeni yok veya birden fazla.");
                    }
                    Uri readedBelgeHedefUriAcilan = PackUriHelper.CreatePartUri(paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEHEDEF).First().TargetUri);
                    CT_BelgeHedef readedBelgeHedefAcilan = (CT_BelgeHedef)(new XmlSerializer(typeof(CT_BelgeHedef))).Deserialize(paketAcilan._package.GetPart(readedBelgeHedefUriAcilan).GetStream(FileMode.Open));

                    Uri readedPaketOzetiUriAcilan = PackUriHelper.CreatePartUri(paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PAKETOZETI).First().TargetUri);
                    CT_PaketOzeti readedPaketOzetiAcilan = (CT_PaketOzeti)(new XmlSerializer(typeof(CT_PaketOzeti))).Deserialize(paketAcilan._package.GetPart(readedPaketOzetiUriAcilan).GetStream(FileMode.Open));

                    Uri readedSifreliIcerikBilgisiUriAcilan = PackUriHelper.CreatePartUri(paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_SIFRELIICERIKBILGISI).First().TargetUri);
                    CT_SifreliIcerikBilgisi readedSifreliIcerikBilgisiAcilan = (CT_SifreliIcerikBilgisi)(new XmlSerializer(typeof(CT_SifreliIcerikBilgisi))).Deserialize(paketAcilan._package.GetPart(readedSifreliIcerikBilgisiUriAcilan).GetStream(FileMode.Open));

                    paketAcilan.PaketOzeti = new PaketOzetiInternal(readedPaketOzetiAcilan);
                    paketAcilan.BelgeHedef = new BelgeHedefInternal(readedBelgeHedefAcilan);
                    paketAcilan.SifreliIcerikBilgisi = new SifreliIcerikBilgisiInternal(readedSifreliIcerikBilgisiAcilan);
                    paketAcilan._paketModu = paketModu;
                    paketAcilan._streamInternal = stream;

                    return paketAcilan;

                case PaketModu.Olustur:
                    var paketOlusturulan = new SifreliPaket
                    {
                        PaketOzeti = new PaketOzetiInternal(),
                        BelgeHedef = new BelgeHedefInternal(),
                        SifreliIcerikBilgisi = new SifreliIcerikBilgisiInternal(),
                        _package = Package.Open(stream, FileMode.OpenOrCreate, FileAccess.ReadWrite)
                    };
                    paketOlusturulan._paketModu = paketModu;
                    paketOlusturulan._streamInternal = stream;
                    return paketOlusturulan;
                default:
                    return null;
            }
        }
        /// <summary>Yeni bir paket oluşturmak, var olan bir paketi açmak veya güncellemek için kullanılır.</summary>
        /// <param name="dosyaYolu">Pakete ilişkin dosya yoludur. </param>
        /// <param name="paketModu">Paketin açılma, oluşturma veya güncelleme amacıyla açıldığını belirtir. </param>
        /// <returns>   İşlem yapılan paket objesi. </returns>
        public static SifreliPaket Ac(string dosyaYolu, PaketModu paketModu)
        {
            switch (paketModu)
            {
                case PaketModu.Guncelle:
                    return SifreliPaket.Ac(File.Open(dosyaYolu, FileMode.Open, FileAccess.ReadWrite), paketModu);
                case PaketModu.Ac:
                    return SifreliPaket.Ac(File.Open(dosyaYolu, FileMode.Open), paketModu);
                case PaketModu.Olustur:
                    return SifreliPaket.Ac(File.Open(dosyaYolu, FileMode.OpenOrCreate, FileAccess.ReadWrite), paketModu);
                default:
                    return null;
            }
        }
        /// <summary>İşlem yapılan paket için sonladırma işlemi yapılarak açılan kaynaklar kapatılır.</summary>
        public void Kapat(Guid belgeId, CT_Olusturan olusturan, string konu)
        {
            if (_paketModu == PaketModu.Olustur)
            {
                if (belgeId == Guid.Empty)
                {
                    throw new ArgumentNullException("belgeId");
                }
                PaketBelirteciBelirle(belgeId.ToString().ToUpperInvariant());
                if (olusturan != null)
                {
                    PaketOlusturanBelirle(Araclar.OlusturanAdiOlustur(olusturan));
                }
                if (konu != null)
                {
                    PaketKonusuBelirle(konu);
                }

                PaketKategorisiBelirle(Araclar.RESMIYAZISMASIFRELI);
                PaketIcerikTuruBelirle(Araclar.EYAZISMAMIMETURU);
                PaketVersiyonuBelirle("1.3");
                PaketRevizyonuBelirle("DotNet/" + System.Reflection.Assembly.GetAssembly(typeof(Paket)).GetName().Version.ToString());

                _package.Close();

            }
            else if (_paketModu == PaketModu.Guncelle)
            {
                if (belgeId == Guid.Empty)
                {
                    throw new ArgumentNullException("belgeId");
                }
                PaketBelirteciBelirle(belgeId.ToString().ToUpperInvariant());
                if (konu != null)
                {
                    PaketKonusuBelirle(konu);
                }
                if (olusturan != null)
                {
                    PaketOlusturanBelirle(Araclar.OlusturanAdiOlustur(olusturan));
                }
                PaketKonusuBelirle(konu);
                PaketKategorisiBelirle(Araclar.RESMIYAZISMASIFRELI);
                PaketIcerikTuruBelirle(Araclar.EYAZISMAMIMETURU);
                PaketVersiyonuBelirle("1.3");
                PaketRevizyonuBelirle("DotNet/" + System.Reflection.Assembly.GetAssembly(typeof(Paket)).GetName().Version.ToString());

                _package.Close();

            }
            else if (_paketModu == PaketModu.Ac)
            {
                _package.Close();
            }
        }
        #endregion
        #region Ozellikler
        #region Olusturan Belirler
        /// <summary>Paket açıklama değeri verilir.</summary>
        /// <param name="aciklama">Verilen açıklama değeri.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: An explanation of the content of the resource. [Example: 
        /// Values might include an abstract, table of contents, 
        /// reference to a graphical representation of content, and a 
        /// free-text account of the content. end example] </remarks>
        public void PaketAciklamasiBelirle(String aciklama) //DC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketAciklamasiBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.Description = aciklama;
        }
        /// <summary>Paket son yazdılırılma tarihi verilir.</summary>
        /// <param name="sonYazdirilmaTarihi">Son yazdırılma tarihi.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: The date and time of the last printing. </remarks>
        public void PaketSonYazdirilmaTarihiBelirle(DateTime? sonYazdirilmaTarihi) // OPC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketSonYazdirilmaTarihiBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.LastPrinted = sonYazdirilmaTarihi;
        }
        /// <summary>Paket güncelleme tarihi verilir.</summary>
        /// <param name="guncellemeTarihi">Güncelleme tarihi.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: Date on which the resource was changed. </remarks>
        public void PaketGuncellemeTarihiBelirle(DateTime? guncellemeTarihi) // DC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketGuncellemeTarihiBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.Modified = guncellemeTarihi;
        }
        /// <summary>Paket son güncelleyen bilgisi verilir.</summary>
        /// <param name="sonGuncelleyen">Son güncelleyen bilgisi.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: The user who performed the last modification. The 
        ///identification is environment-specific. [Example: A name, 
        ///email address, or employee ID. end example] It is 
        ///recommended that this value be as concise as possible. </remarks>
        public void PaketSonGuncelleyenBelirle(String sonGuncelleyen) // OPC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketSonGuncelleyenBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.LastModifiedBy = sonGuncelleyen;
        }
        /// <summary>Paket anahtar kelimeleri verilir.</summary>
        /// <param name="anahtarKelimeler">Anahtar kelimeler.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: A delimited set of keywords to support searching and 
        ///indexing. This is typically a list of terms that are not 
        ///available elsewhere in the properties.  
        /// 
        ///The definition of this element uniquely allows for: 
        ///  Use of the xml:lang attribute to identify languages 
        ///  A mixed content model, such that keywords can be 
        ///flagged individually 
        ///
        ///[Example: The following instance of the keywords element 
        ///has keywords in English (Canada), English (U.S.), and French 
        ///(France): 
        /// 
        /// <keywords xml:lang="en-US"> 
        /// color  
        /// <value xml:lang="en-CA">colour</value> 
        /// <value xml:lang="fr-FR">couleur</value> 
        /// </keywords> 
        ///
        ///end example]  </remarks>
        public void PaketAnahtarKelimeleriBelirle(String anahtarKelimeler) // OPC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketAnahtarKelimeleriBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.Keywords = anahtarKelimeler;
        }
        /// <summary>Paket durumu verilir.</summary>
        /// <param name="durum">Durum.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: The status of the content. [Example: Values might include 
        ///“Draft”, “Reviewed”, and “Final”.  end example]  </remarks>
        public void PaketDurumuBelirle(String durum) // OPC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketDurumuBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.ContentStatus = durum;
        }
        /// <summary>Paket oluşturulma tarihi verilir.</summary>
        /// <param name="olusturulmaTarihi">Oluşturulma tarihi.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: Date of creation of the resource. </remarks>
        public void PaketOlusturulmaTarihiBelirle(DateTime? olusturulmaTarihi) // DC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketOlusturulmaTarihiBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.Created = olusturulmaTarihi;
        }
        /// <summary>Paket başlığı verilir.</summary>
        /// <param name="baslik">Başlık.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: The name given to the resource. </remarks>
        public void PaketBasligiBelirle(String baslik) // DC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketBasligiBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.Title = baslik;
        }
        /// <summary>Paket dili verilir.</summary>
        /// <param name="dil">Dil.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: The language of the intellectual content of the resource. 
        /// [Note: IETF RFC 3066 provides guidance on encoding to 
        /// represent languages.  end note]  </remarks>
        public void PaketDiliBelirle(String dil) //DC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketDiliBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.Language = dil;
        }
        #endregion
        #region Ustveri Ile Ayni
        private void PaketBelirteciBelirle(String belirtec) //DC, Ustveri alanı ile aynı
        {
            _package.PackageProperties.Identifier = belirtec;
        }
        private void PaketOlusturanBelirle(String olusturan) // DC, Ustveri alanı ile aynı
        {
            _package.PackageProperties.Creator = olusturan;
        }
        private void PaketKonusuBelirle(String konu) // DC, Ustveri alanı ile aynı
        {
            _package.PackageProperties.Subject = konu;
        }
        #endregion
        #region Static
        private void PaketKategorisiBelirle(String kategori) // OPC, static
        {
            _package.PackageProperties.Category = kategori;
        }
        private void PaketIcerikTuruBelirle(String icerikTuru) // OPC, static
        {
            _package.PackageProperties.ContentType = icerikTuru;
        }
        private void PaketVersiyonuBelirle(String versiyon) // DC, static
        {
            _package.PackageProperties.Version = versiyon;
        }
        private void PaketRevizyonuBelirle(String revisyon) // DC, static
        {
            _package.PackageProperties.Revision = revisyon;
        }
        #endregion
        #endregion
        #region Paket Ozeti
        /// <summary>   Paket içerisindeki PaketOzeti bileşenini STREAM olarak döner. </summary>
        /// <returns>   Paket içerisindeki PaketOzeti bileşeninin STREAM nesnesi. PaketOzeti bileşeni olmaması durumunda null döner. </returns>
        public Stream PaketOzetiAl()
        {
            if (_package.PartExists(new Uri(Araclar.URI_PAKETOZETI, UriKind.Relative)))
            {
                return _package.GetPart(new Uri(Araclar.URI_PAKETOZETI, UriKind.Relative)).GetStream();
            }
            return null;
        }
        /// <summary>
        /// Mevcut bir PaketOzeti bileşenini pakete ekler.
        /// </summary>
        /// <param name="paketOzeti">PaketOzeti bileşenini barındıran STREAM nesnesi.</param>
        /// 
        public void PaketOzetiEkle(Stream paketOzeti)
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketOzetiEkle fonksiyonu kullanılamaz.");
            }
            if (paketOzeti == null)
            {
                throw new ArgumentNullException("paketOzeti");
            }
            var partUriPaketOzeti = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_PAKETOZETI, UriKind.Relative));
            if (_paketModu == PaketModu.Guncelle)
            {
                if (_package.PartExists(partUriPaketOzeti))
                {
                    _package.DeletePart(partUriPaketOzeti);
                    _package.DeleteRelationship(Araclar.ID_PAKETOZETI);
                }
            }
            var partPaketOzeti = _package.CreatePart(partUriPaketOzeti, Araclar.MIME_XML, CompressionOption.Maximum);
            _package.CreateRelationship(partPaketOzeti.Uri, TargetMode.Internal, Araclar.RELATION_TYPE_PAKETOZETI, Araclar.ID_PAKETOZETI);
            Araclar.CopyStream(paketOzeti, partPaketOzeti.GetStream());

            paketOzeti.Position = 0;
            CT_PaketOzeti readedPaketOzeti = (CT_PaketOzeti)(new XmlSerializer(typeof(CT_PaketOzeti))).Deserialize(paketOzeti);
            PaketOzeti = new PaketOzetiInternal(readedPaketOzeti);
            PaketOzeti.KontrolEt();
        }
        #endregion
        #region Hedefler
        /// <summary>   Paket içerisindeki BelgeHedef bileşenini STREAM olarak döner. </summary>
        /// <returns>   Paket içerisindeki BelgeHedef bileşeninin STREAM nesnesi. BelgeHedef bileşeni olmaması durumunda null döner. </returns>
        public Stream BelgeHedefiAl()
        {
            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEHEDEF).Count() == 1)
            {
                return _package.GetPart(PackUriHelper.CreatePartUri(_package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEHEDEF).First().TargetUri)).GetStream();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Mevcut bir BelgeHedef bileşenini pakete ekler.
        /// </summary>
        /// <param name="belgeHedef">BelgeHedef bileşenini barındıran STREAM nesnesi.</param>
        public void BelgeHedefEkle(Stream belgeHedef)
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için BelgeHedefEkle fonksiyonu kullanılamaz.");
            }
            if (belgeHedef == null)
            {
                throw new ArgumentNullException("belgeHedef");
            }
            var partUriBelgeHedef = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_BELGEHEDEF, UriKind.Relative));
            if (_paketModu == PaketModu.Guncelle)
            {
                if (_package.PartExists(partUriBelgeHedef))
                {
                    _package.DeletePart(partUriBelgeHedef);
                    _package.DeleteRelationship(Araclar.ID_BELGEHEDEF);
                }
            }
            var partBelgeHedef = _package.CreatePart(partUriBelgeHedef, Araclar.MIME_XML, CompressionOption.Maximum);
            _package.CreateRelationship(partBelgeHedef.Uri, TargetMode.Internal, Araclar.RELATION_TYPE_BELGEHEDEF, Araclar.ID_BELGEHEDEF);
            Araclar.CopyStream(belgeHedef, partBelgeHedef.GetStream());

            belgeHedef.Position = 0;
            CT_BelgeHedef readedBelgeHedef = (CT_BelgeHedef)(new XmlSerializer(typeof(CT_BelgeHedef))).Deserialize(belgeHedef);
            BelgeHedef = new BelgeHedefInternal(readedBelgeHedef);
            BelgeHedef.KontrolEt();
        }
        #endregion
        #region SifreliIcerikBilgisi
        /// <summary>SifreliIcerikBilgisi bileşeninin 'serialize' edilerek paket içerisine eklenmesini sağlar.</summary>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        public void SifreliIcerikBilgisiOlustur()
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için SifreliIcerikBilgisiOlustur fonksiyonu kullanılamaz.");
            }
            SifreliIcerikBilgisi.CT_SifreliIcerikBilgisi.Id = "";
            SifreliIcerikBilgisi.CT_SifreliIcerikBilgisi.URI = (new List<string>() { Araclar.SIFRELEME_YONTEMI_URI_1, Araclar.SIFRELEME_YONTEMI_URI_2 }).ToArray();
            SifreliIcerikBilgisi.CT_SifreliIcerikBilgisi.Yontem = Araclar.SIFRELEME_YONTEMI;
            SifreliIcerikBilgisi.CT_SifreliIcerikBilgisi.Version = Araclar.SIFRELEME_YONTEMI_VERSIYONU;
            SifreliIcerikBilgisi.KontrolEt();
            var partUriUstveri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_SIFRELIICERIKBILGISI, UriKind.Relative));
            if (_paketModu == PaketModu.Guncelle)
            {
                if (_package.PartExists(partUriUstveri))
                {
                    _package.DeletePart(partUriUstveri);
                    _package.DeleteRelationship(Araclar.ID_SIFRELIICERIKBILGISI);
                }
            }
            var partUstveri = _package.CreatePart(partUriUstveri, Araclar.MIME_XML, CompressionOption.Normal);
            _package.CreateRelationship(partUstveri.Uri, TargetMode.Internal, Araclar.RELATION_TYPE_SIFRELIICERIKBILGISI, Araclar.ID_SIFRELIICERIKBILGISI);
            using (var memoryStream = new MemoryStream())
            {
                var x = new XmlSerializer(typeof(CT_SifreliIcerikBilgisi));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xmlTextWriter.Formatting = Formatting.Indented;
                x.Serialize(xmlTextWriter, SifreliIcerikBilgisi.CT_SifreliIcerikBilgisi);
                memoryStream.Flush();
                memoryStream.Position = 0;
                Araclar.CopyStream(memoryStream, partUstveri.GetStream());
            }
        }
        #endregion
        #region SifreliIcerik
        /// <summary>
        /// Mevcut bir paketi SifreliIcerik olarak pakete ekler.
        /// </summary>
        /// <param name="sifreliIcerik">Paketi barındıran STREAM nesnesi.</param>
        /// <param name="paketId">Eklenen paketin ID'si.</param>
        public void SifreliIcerikEkle(Stream sifreliIcerik, Guid paketId)
        {
            if (_sifreliIcerikEklenmisMi)
                throw new SystemException("Daha önce şifreli içerik eklenmiş.");
            if (sifreliIcerik == null)
                throw new ArgumentNullException("dosya");
            if (paketId == null || paketId == Guid.Empty)
                throw new ArgumentNullException("paketId");
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için SifreliIcerikEkle fonksiyonu kullanılamaz.");
            }
            var assemblyPartUri = PackUriHelper.CreatePartUri(new Uri(string.Format(Araclar.URI_FORMAT_SIFRELIICERIK, Uri.EscapeDataString(paketId.ToString().ToUpperInvariant())), UriKind.Relative));
            var part = _package.CreatePart(assemblyPartUri, "application/pkcs7-mime", CompressionOption.Maximum);
            Araclar.CopyStream(sifreliIcerik, part.GetStream());

            _package.CreateRelationship(part.Uri, TargetMode.Internal, Araclar.RELATION_TYPE_SIFRELIICERIK, Araclar.ID_SIFRELIICERIK);
            sifreliIcerik.Position = 0;
            _sifreliIcerikEklenmisMi = true;
        }
        /// <summary>   Paket içerisindeki PaketOzeti SifreliIcerik STREAM olarak döner. </summary>
        /// <returns>   Paket içerisindeki PaketOzeti SifreliIcerik STREAM nesnesi. SifreliIcerik bileşeni olmaması durumunda null döner. </returns>
        public Stream SifreliIcerikAl()
        {
            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_SIFRELIICERIK).Count() == 1)
            {
                return _package.GetPart(PackUriHelper.CreatePartUri(_package.GetRelationshipsByType(Araclar.RELATION_TYPE_SIFRELIICERIK).First().TargetUri)).GetStream();
            }
            throw new Exception("Şifreli içerik ilişkisi bulunamadı.");
        }
        /// <summary>Paketteki şifreli dosyanın adını döner.</summary>
        /// <returns>Paketteki şifreli dosyanın adı.</returns>
        public String SifreliIcerikDosyasiAdiAl()
        {
            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_SIFRELIICERIK).Count() == 1)
            {
                return _package.GetRelationshipsByType(Araclar.RELATION_TYPE_SIFRELIICERIK).First().TargetUri.OriginalString.Split('/').Last();
            }
            else
            {
                throw new Exception("Şifreli içerik bulunamadı.");
            }

        }
        #endregion
        /// <summary>
        /// Kullanılan kaynakları kapatır.
        /// </summary>
        public void Dispose()
        {
            if (_streamInternal != null)
                _streamInternal.Close();
            if (_streamInternal != null)
                _streamInternal.Dispose();
        }
    }

}
