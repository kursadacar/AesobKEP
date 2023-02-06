using System;
using System.IO.Packaging;
using System.IO;
using Dpt.eYazisma.Xsd;
using System.Xml.Serialization;
using System.Linq;
using System.Xml;
using System.Text;
using System.Collections.Generic;

// TODO Butun enum degerlerinin DEFAULKT valueya sahip oldugu ve deger verilmemsi durumunda o degeri aldigi dokumantasyona eklensin, JAVA da ayni konu mu kontrol et

// namespace: Dpt.eYazisma.Tipler
//
// summary:	.
namespace Dpt.eYazisma.Tipler
{
 
    /// <summary>
    /// <example>
    /// Yeni bir paket oluşturma örneği
    /// <code>
    /// static void PaketOlustur(string dosyaYolu)
    /// {
    ///     var paketId = Guid.NewGuid();
    ///     var islemTarihi = DateTime.Now;
    ///     var paketKonu = new TextType() { Value = "e-Yazışma Test Paketi" };
    ///     var paketNo = "B.02.1.DPT.0.14.00.00-107.01/2474";
    ///     var ek1 = new CT_Ek()
    ///     {
    ///         Id = new CT_Id() { Value = Guid.NewGuid().ToString() },    // Zorunlu alan
    ///         BelgeNo = "B.02.1.DPT.0.14.00.00-107.01/2475",
    ///         Tur = ST_KodEkTuru.DED,        // Zorunlu alan
    ///         DosyaAdi = System.IO.Path.GetFileName(ekDosyasi1),
    ///         MimeTuru = "application/pdf",
    ///         Ad = new TextType() { Value = "Birinci ek" },
    ///         SiraNo = 1,     // Zorunlu alan
    ///         Aciklama = new TextType() { Value = "Birinci ek açıklaması" },
    ///         OzId = new IdentifierType()
    ///         {
    ///             Value = "A14A4DCC-AE6A-4FD5-AAB3-8A33DC6125DD",
    ///             schemeID = "GUID"     // Zorunlu alan
    ///         },
    ///         ImzaliMi = true,
    ///         ImzaliMiSpecified = true
    ///     };
    /// 
    ///     var ek2 = new CT_Ek()
    ///     {
    ///         Id = new CT_Id() { Value = Guid.NewGuid().ToString() },     // Zorunlu alan
    ///         BelgeNo = "B.02.1.DPT.0.14.00.00-107.01/2476",
    ///         Tur = ST_KodEkTuru.HRF,       // Zorunlu alan
    ///         Ad = new TextType() { Value = "Harici referans" },
    ///         SiraNo = 2,        // Zorunlu alan
    ///         Aciklama = new TextType() { Value = "Hairici referans açıklaması" },
    ///         Referans = "http://www.bilgitoplumu.gov.tr/Documents/1/Raporlar/Calisma_Raporu_2.pdf",
    ///         OzId = new IdentifierType()
    ///         {
    ///             Value = "0C9C4D34-D370-46C2-A996-0FADC0FA591F",
    ///             schemeID = "GUID"       // Zorunlu alan
    ///         },
    ///         ImzaliMi = false,
    ///         ImzaliMiSpecified = true,
    ///         Ozet = new CT_Ozet()
    ///         {
    ///             OzetAlgoritmasi = new CT_OzetAlgoritmasi() { Algorithm = Araclar.ALGORITHM_SHA256 },    // Zorunlu alan
    ///             OzetDegeri = Convert.FromBase64String("rv9s4QZT2kVl5ozTKAHmAz6ICboSjvGYH1WcqhoIAYs=")   // Zorunlu alan
    ///         }
    ///     };
    /// 
    ///     var ek3 = new CT_Ek()
    ///     {
    ///         Id = new CT_Id() { Value = Guid.NewGuid().ToString() },     // Zorunlu alan
    ///         BelgeNo = "B.02.1.DPT.0.14.00.00-107.01/2477",
    ///         Tur = ST_KodEkTuru.FZK,        // Zorunlu alan
    ///         Ad = new TextType() { Value = "Fiziksel ek" },
    ///         SiraNo = 3,     // Zorunlu alan
    ///         Aciklama = new TextType() { Value = "İki adet CD" },
    ///         OzId = new IdentifierType() { Value = "45A778BA-B943-4B68-8504-71854DF0DDFF", schemeID = "GUID" },
    ///         ImzaliMi = false,
    ///         ImzaliMiSpecified = true
    ///     };
    /// 
    ///     var ek4 = new CT_Ek()
    ///     {
    ///         Id = new CT_Id() { Value = Guid.NewGuid().ToString() },     // Zorunlu alan
    ///         BelgeNo = "B.02.1.DPT.0.14.00.00-107.01/2478",
    ///         Tur = ST_KodEkTuru.DED,     // Zorunlu alan
    ///         DosyaAdi = System.IO.Path.GetFileName(ekDosyasi2),
    ///         MimeTuru = "application/MSWord",
    ///         Ad = new TextType() { Value = "İmzasız ek" },
    ///         SiraNo = 4,     // Zorunlu alan
    ///         Aciklama = new TextType() { Value = "İmzasız ek açıklaması" },
    ///         OzId = new IdentifierType()
    ///         {
    ///             Value = "76E16DDA-AB44-46DB-B615-42FBE7E9BEE3",
    ///             schemeID = "GUID"
    ///         },
    ///         ImzaliMi = false,
    ///         ImzaliMiSpecified = true   // False ve true değerleri için test edilmeli
    ///     };
    /// 
    ///     var ek5 = new CT_Ek()
    ///     {
    ///         Id = new CT_Id() { Value = Guid.NewGuid().ToString(), EYazismaIdMi = true },     // Zorunlu alan
    ///         BelgeNo = "B.02.1.DPT.0.14.00.00-107.01/2479",
    ///         Tur = ST_KodEkTuru.DED,            // Zorunlu alan
    ///         DosyaAdi = System.IO.Path.GetFileName(ekDosyasi3),
    ///         MimeTuru = "application/pdf",
    ///         Ad = new TextType() { Value = "İkinci ek" },
    ///         SiraNo = 5,         // Zorunlu alan
    ///         Aciklama = new TextType() { Value = "İkinci ek açıklaması" },
    ///         OzId = new IdentifierType() { Value = "D7D16B9C-7419-451B-82B0-34D334572464", schemeID = "GUID" },
    ///         ImzaliMi = true,
    ///         ImzaliMiSpecified = true
    ///     };
    /// 
    ///     var ilgiA = new CT_Ilgi()
    ///     {
    ///         Id = new CT_Id() { Value = Guid.NewGuid().ToString(), EYazismaIdMi = false },       // Zorunlu alan
    ///         BelgeNo = "B.02.1.DPT.0.14.00.00-107.01/2481",
    ///         Tarih = new DateTime(2009, 11, 5),
    ///         TarihSpecified = true,
    ///         Etiket = "a",       // Zorunlu alan
    ///         EkId = ek1.Id.Value,
    ///         Ad = new TextType() { Value = "İlgi (a) yazı" },
    ///         Aciklama = new TextType() { Value = "İlgi (a) yazının açıklaması" },
    ///         OzId = new IdentifierType()
    ///         {
    ///             Value = "1C26C956-AE45-4DC1-BE86-1B8278A34904",
    ///             schemeID = "GUID"       // Zorunlu alan
    ///         }
    ///     };
    /// 
    ///     var ilgiB = new CT_Ilgi()
    ///     {
    ///         Id = new CT_Id() { Value = Guid.NewGuid().ToString(), EYazismaIdMi = false },    // Zorunlu alan   
    ///         BelgeNo = "B.02.1.DPT.0.14.00.00-107.01/2482",
    ///         Tarih = new DateTime(2009, 11, 5),
    ///         TarihSpecified = true,
    ///         Etiket = "b",      // Zorunlu alan                
    ///         Ad = new TextType() { Value = "İlgi (b) yazı" },
    ///         Aciklama = new TextType() { Value = "İlgi (b) yazının açıklaması" },
    ///         OzId = new IdentifierType()
    ///         {
    ///             Value = "1C26C956-AE45-4DC1-BE86-1B8278A34904",
    ///             schemeID = "GUID"
    ///         }
    ///     };
    /// 
    ///     var imza = new CT_Imza()
    ///     {
    ///         Imzalayan = new CT_GercekSahis()        // Zorunlu alan
    ///         {
    ///             Kisi = new CT_Kisi()        // Zorunlu alan
    ///             {
    ///                 IlkAdi = new NameType() { Value = "Osman" },        // Zorunlu alan
    ///                 Soyadi = new NameType() { Value = "Yeşil" },        // Zorunlu alan
    ///                 Unvan = new NameType() { Value = "Büyükelçi" },
    ///                 IkinciAdi = new NameType() { Value = "Murat" },
    ///                 OnEk = new TextType() { Value = "Dr." }
    ///             },
    ///             TCKN = "12345678902",
    ///             Gorev = new TextType() { Value = "Daire Başkanı" },
    ///             IletisimBilgisi = new CT_IletisimBilgisi()
    ///             {
    ///                 Telefon = "0-312-2666666",
    ///                 TelefonDiger = "0-532-4828282",
    ///                 EPosta = "om_yesil@xyz.com.tr",
    ///                 Faks = "0-312-2888888",
    ///                 Adres = new TextType() { Value = "Kalkınma Bakanlığı Necatibey Cad. No:108" },
    ///                 Ilce = new NameType() { Value = "Çankaya" },
    ///                 Il = new NameType() { Value = "Ankara" },
    ///                 Ulke = new NameType() { Value = "Türkiye" },
    ///                 WebAdresi = "www.kalkinma.gov.tr"
    ///             }
    ///         },
    ///         YetkiDevreden = new CT_GercekSahis()
    ///         {
    ///             Kisi = new CT_Kisi()   // Zorunlu alan
    ///             {
    ///                 IlkAdi = new NameType() { Value = "Hasan" },        // Zorunlu alan
    ///                 Soyadi = new NameType() { Value = "Aydın" },        // Zorunlu alan
    ///                 Unvan = new NameType() { Value = "Genel Müdür" },
    ///                 IkinciAdi = new NameType() { Value = "Şaban" },
    ///                 OnEk = new TextType() { Value = "Uzman" }
    ///             },
    ///             TCKN = "12345678901",
    ///             Gorev = new TextType() { Value = "Genel Müdür" },
    ///             IletisimBilgisi = new CT_IletisimBilgisi()
    ///             {
    ///                 Telefon = "0-312-2999999",
    ///                 TelefonDiger = "0-532-4827777",
    ///                 EPosta = "iletisim@abc.com",
    ///                 Faks = "0-312-2946345",
    ///                 Adres = new TextType() { Value = "TUİK Necatibey Cad. No:112" },
    ///                 Ilce = new NameType() { Value = "Çankaya" },
    ///                 Il = new NameType() { Value = "Ankara" },
    ///                 Ulke = new NameType() { Value = "Türkiye" },
    ///                 WebAdresi = "www.tuik.gov.tr"
    ///             }
    ///         },
    ///         VekaletVeren = new CT_GercekSahis()
    ///         {
    ///             Kisi = new CT_Kisi()  // Zorunlu alan
    ///             {
    ///                 IlkAdi = new NameType() { Value = "Semih" },  // Zorunlu alan
    ///                 Soyadi = new NameType() { Value = "Yılmaz" },  // Zorunlu alan
    ///                 Unvan = new NameType() { Value = "Dr." }
    ///             },
    ///             Gorev = new TextType() { Value = "Müsteşar" }
    ///         },
    ///         Makam = new NameType() { Value = "Müsteşar" },
    ///         Amac = new TextType() { Value = "Onay" },
    ///         Aciklama = new TextType() { Value = "İmzanın açıklaması" },
    ///         Tarih = islemTarihi,
    ///         TarihSpecified = true
    ///     };
    /// 
    ///     var ilgili = new CT_GercekSahis()   // Seçimli zorunlu (Kurum, gerçek, tüzel)
    ///     {
    ///         Kisi = new CT_Kisi()        // Zorunlu alan
    ///         {
    ///             OnEk = new TextType() { Value = "Uzman" },
    ///             IlkAdi = new NameType() { Value = "Hüseyin" },      // Zorunlu alan
    ///             Soyadi = new NameType() { Value = "Yetmez" }        // Zorunlu alan
    ///         },
    ///         Gorev = new TextType() { Value = "Birim Sorumlusu" },
    ///         TCKN = "12345678903",
    ///         IletisimBilgisi = new CT_IletisimBilgisi()
    ///         {
    ///             Telefon = "0-505-4821111",
    ///             TelefonDiger = "0-312-2444444"
    ///         }
    ///     };
    /// 
    ///     var olusturan = new CT_KurumKurulus()
    ///     {
    ///         KKK = "23ED5E7A15B9A2",     // Zorunlu alan
    ///         BYK = "B.02.0.000.0.00.00.00",
    ///         Adi = new NameType() { Value = "Başbakanlık" },
    ///         IletisimBilgisi = new CT_IletisimBilgisi()
    ///         {
    ///             Telefon = "0-312-2947586",
    ///             EPosta = "info@basbakanlik.gov.tr",
    ///             Faks = "0-312-2663333",
    ///             Adres = new TextType() { Value = "Kızılay" },
    ///             Ilce = new NameType() { Value = "Çankaya" },
    ///             Il = new NameType() { Value = "Ankara" },
    ///             Ulke = new NameType() { Value = "Türkiye" },
    ///             WebAdresi = "www.basbakanlik.gov.tr"
    ///         }
    ///     };
    /// 
    ///     var dagitim1 = new CT_Dagitim()
    ///     {
    ///         DagitimTuru = ST_KodDagitimTuru.GRG,        // Zorunlu alan
    ///         Ivedilik = ST_KodIvedilik.GNL,      // Zorunlu alan
    ///         //Miat = duration tipinde olacakmış, nasıl olacaksa
    ///         Item = new CT_KurumKurulus()        // Seçimli zorunlu
    ///         {
    ///             KKK = "23ED5E7C583269",      // Zorunlu alan
    ///             BYK = "B.03.0.000.0.00.00.00",
    ///             Adi = new NameType() { Value = "Adalet Bakanlığı" },
    ///             IletisimBilgisi = new CT_IletisimBilgisi()
    ///             {
    ///                 Telefon = "0-312-2665555",
    ///                 EPosta = "info@adalet.gov.tr",
    ///                 Faks = "0-312-2665556",
    ///                 Adres = new TextType() { Value = "Kızılay" },
    ///                 Ilce = new NameType() { Value = "Çankaya" },
    ///                 Il = new NameType() { Value = "Ankara" },
    ///                 Ulke = new NameType() { Value = "Türkiye" },
    ///                 WebAdresi = "www.adalet.gov.tr"
    ///             }
    ///         },
    ///         KonulmamisEkListesi = new CT_KonulmamisEk[]
    ///         {
    ///             new CT_KonulmamisEk()       // Zorunlu alan
    ///             {
    ///                 EkId = ek5.Id.Value     // Zorunlu alan
    ///             }       
    ///         }
    ///     };
    /// 
    ///     var dagitim2 = new CT_Dagitim()
    ///     {
    ///         DagitimTuru = ST_KodDagitimTuru.BLG, // Zorunlu alan
    ///         Ivedilik = ST_KodIvedilik.CIV,      // Zorunlu alan
    ///         Item = new CT_KurumKurulus()
    ///         {
    ///             KKK = "23ED5E84BA4603",     // Zorunlu alan
    ///             BYK = "B.10.0.000.0.00.00.00 ",
    ///             Adi = new NameType() { Value = "Sağlık Bakanlığı" }
    ///         }
    ///     };
    /// 
    ///     using (var paket = Paket.Ac(dosyaYolu, PaketModu.Olustur))
    ///     {
    ///         paket.Ustveri.BelgeIdBelirle(paketId);      // Zorunlu alan
    ///         paket.Ustveri.KonuBelirle(paketKonu);       // Zorunlu alan
    ///         paket.Ustveri.TarihBelirle(islemTarihi);        // Zorunlu alan
    ///         paket.Ustveri.BelgeNoBelirle(paketNo);      // Zorunlu alan
    ///         paket.Ustveri.GuvenlikKoduBelirle(ST_KodGuvenlikKodu.HZO);      // Zorunlu alan
    ///         paket.Ustveri.GuvenlikGecerlilikTarihiBelirle(islemTarihi.AddYears(10));
    ///         paket.Ustveri.OzIdBelirle("8CEA7FF7-75F2-4CCF-B3C1-1B9B2054E8E9", "GUID");
    ///         paket.Ustveri.DagitimEkle(dagitim1);        // Zorunlu alan
    ///         paket.Ustveri.DagitimEkle(dagitim2);        // Zorunlu alan
    ///         paket.Ustveri.EkEkle(ek1);
    ///         paket.Ustveri.EkEkle(ek2);
    ///         paket.Ustveri.EkEkle(ek3);
    ///         paket.Ustveri.EkEkle(ek4);
    ///         paket.Ustveri.EkEkle(ek5);
    ///         paket.Ustveri.IlgiEkle(ilgiA);
    ///         paket.Ustveri.IlgiEkle(ilgiB);
    ///         paket.Ustveri.DilBelirle("tur");
    ///         paket.Ustveri.OlusturanBelirle(olusturan);      // Zorunlu alan
    ///         paket.Ustveri.IlgiliEkle(ilgili);
    ///         paket.Ustveri.DosyaAdiBelirle(System.IO.Path.GetFileName(ustYaziDosyasi));      // Zorunlu alan
    /// 
    ///         paket.BelgeHedef.HedefEkle(Araclar.Dagitim2Hedef(dagitim1));        // Zorunlu alan
    ///         paket.BelgeHedef.HedefEkle(Araclar.Dagitim2Hedef(dagitim2));        // Zorunlu alan
    /// 
    ///         paket.BelgeImza.ImzaEkle(imza);
    /// 
    ///         paket.EkEkle(ek1, ekDosyasi1);
    ///         paket.EkEkle(ek4, ekDosyasi2, OzetModu.Yok);
    ///         paket.EkEkle(ek5, ekDosyasi3, OzetModu.SHA256);
    /// 
    ///         paket.UstYaziEkle(ustYaziDosyasi, "application/pdf", OzetModu.SHA256);        // Zorunlu alan
    /// 
    ///         paket.EkleriKontrolEt();
    ///         paket.IlgileriKontrolEt();
    /// 
    ///         paket.PaketSonGuncelleyenBelirle("Başbakanlık");
    ///         paket.PaketAnahtarKelimeleriBelirle("e-Yazişma, Test");
    ///         paket.PaketBasligiBelirle("Başlık");
    ///         paket.PaketDurumuBelirle("Son versiyon");
    ///         paket.PaketGuncellemeTarihiBelirle(islemTarihi);
    ///         paket.CoreOlustur();
    /// 
    ///         paket.UstveriOlustur();     // Zorunlu alan
    ///         paket.BelgeHedefOlustur();     // Zorunlu alan
    ///         paket.BelgeImzaOlustur();
    /// 
    ///         paket.PaketOzeti.Ekle(OzetModu.SHA512, new byte[] { 10, 22, 58 }, new Uri("http://HariciBilesen/eYazışma.udf"), true);
    /// 
    ///         paket.PaketOzetiOlustur();      // Zorunlu alan
    ///         Stream paketOzeti = paket.PaketOzetiAl();
    ///         Byte[] imzaliVeri = Imzala(StreamToByteArray(paketOzeti));
    ///         paket.ImzaEkle(imzaliVeri);         // Zorunlu alan
    /// 
    ///         paket.NihaiOzetOlustur();
    ///         Stream nihaiOzet = paket.NihaiOzetAl();
    ///         Byte[] muhurluVeri = Muhurle(StreamToByteArray(nihaiOzet));
    ///         paket.MuhurEkle(muhurluVeri);
    /// 
    ///         paket.Kapat();
    ///     }
    ///     Console.WriteLine("Paket oluşturuldu.");
    /// }
    /// </code>
    /// Yardımcı metodlar.
    /// <code>
    /// static byte[] Imzala(byte[] imzalanacakVeri)
    /// {
    ///     return new byte[] { 1, 2, 3 }; // burada imzalanacakVeri herhangi bir imzala API'si ile imzalanarak imzali veri donulur
    /// }
    /// 
    /// static byte[] Muhurle(byte[] muhurlenecekVeri)
    /// {
    ///     return new byte[] { 1, 2, 3 }; // burada muhurlenecekVeri herhangi bir muhur API'si ile muhurlenerek donulur
    /// }
    /// 
    /// static byte[] StreamToByteArray(Stream input)
    /// {
    ///     byte[] buffer = new byte[16 * 1024];
    ///     using (MemoryStream ms = new MemoryStream())
    ///     {
    ///         int read;
    ///         while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
    ///         {
    ///             ms.Write(buffer, 0, read);
    ///         }
    ///         return ms.ToArray();
    ///     }
    /// }
    /// 
    /// static byte[] PaketSifrele(String dosyaYolu)
    /// {
    ///     return System.IO.File.ReadAllBytes(dosyaYolu);  // Burada, şifrelenen paketin geri döndürülmesi gerekiyor.
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public class Paket : IDisposable
    {
        internal Package _package;
        internal PaketModu _paketModu;

        private Stream _streamInternal;

        /// <summary>   Belgeye ait üstveri alanlarını barıdıran objeye ulaşılır. </summary>
        /// <value> Ustveri nesnesi. </value>
        public Ustveri Ustveri { get; private set; }

        /// <summary>   Paketin iletileceği hedefleri barındıran objeye ulaşılır. </summary>
        /// <value> BelgeHedef nesnesi. </value>
        public BelgeHedef BelgeHedef { get; private set; }

        /// <summary>Paket içerisinde imzalanan bileşenlere ait özet bilgilerinin bulunduğu objeye ulaşılır.</summary>
        /// <value> PaketOzeti nesnesi. </value>
        public PaketOzeti PaketOzeti { get; private set; }

        /// <summary>Paket içerisinde mühürlenen bileşenlere ait özet bilgilerinin bulunduğu objeye ulaşılır.</summary>
        /// <value> NihaiOzet nesnesi. </value>
        public NihaiOzet NihaiOzet { get; private set; }

        /// <summary>Belgeye atılmış olan imzalara ilişkin üstveri bilgilerini içeren objeye ulaşılır.</summary>
        /// <value> BelgeImza nesnesi. </value>
        public BelgeImza BelgeImza { get; private set; }

        /// <summary>
        /// PaketOzeti'ine eklenecek özetlerin oluşturulmasında kullanılacak algoritmayı belirtir.
        /// </summary>
        public OzetModu VarsayilanOzetModu { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected Paket()
        {
            VarsayilanOzetModu = OzetModu.SHA256;
        }

        #region Aç Kapat

        /// <summary>Yeni bir paket oluşturmak, var olan bir paketi açmak veya güncellemek için kullanılır.</summary>
        /// <param name="stream">       Pakete ilişkin STREAM objesidir. </param>
        /// <param name="paketModu">    Paketin açılma, oluşturma veya güncelleme amacıyla açıldığını belirtir. </param>
        /// <returns>  İşlem yapılan paket objesi. </returns>
        /// <example>
        /// Mevcut bir paketi açma örneği.
        /// <code>
        /// static void PaketAc(String dosyaYolu)
        /// {
        ///     using (var paket = Paket.Ac(dosyaYolu, PaketModu.Ac))
        ///     {
        ///         var paketOzetiDogrulamaSonucu = new List&lt;OzetDogrulamaHatasi&gt;();
        ///         var nihaiOzetDogrulamaSonucu = new List&lt;OzetDogrulamaHatasi&gt;();
        ///         var hedefler = paket.BelgeHedef.HedefleriAl();
        ///         var imzalar = paket.BelgeImza.ImzalariAl();
        /// 
        ///         if ((paket.Ustveri.EkleriAl() != null) &amp;&amp; (paket.Ustveri.EkleriAl().Count() &gt; 0))
        ///         {
        ///             var dedEkler = paket.Ustveri.EkleriAl().Where(x =&gt; x.Tur == ST_KodEkTuru.DED);
        ///             if ((dedEkler != null) &amp;&amp; (dedEkler.Count() &gt; 0))
        ///             {
        ///                 if (!System.IO.Directory.Exists(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu)))
        ///                     System.IO.Directory.CreateDirectory(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu));
        ///                 foreach (var dedEk in dedEkler)
        ///                 {
        ///                     var ek = paket.EkAl(new Guid(dedEk.Id.Value));
        ///                     if (ek != null)
        ///                         using (Stream s = File.OpenWrite(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu) + "\\" + dedEk.DosyaAdi))
        ///                         {
        ///                             Dpt.eYazisma.Tipler.Araclar.CopyStream(ek, s);
        ///                         }
        ///                 }
        ///             }
        ///         }
        /// 
        ///         using (Stream s = File.OpenWrite(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu) + "\\ImzaCades.imz"))
        ///         {
        ///             Dpt.eYazisma.Tipler.Araclar.CopyStream(paket.ImzaAl(), s);
        ///         }
        /// 
        ///         var belgeImza = paket.BelgeImzaAl();
        ///         var muhur = paket.MuhurAl();
        ///         var nihaiOzetler = paket.NihaiOzet.OzetleriAl();
        ///         var nihaiOzet = paket.NihaiOzetAl();
        ///         var paketOzetleri = paket.PaketOzeti.OzetleriAl();
        ///         var paketOzetiDogrulamasi = paket.PaketOzetiDogrula(Araclar.PaketOzetiAl(paket.PaketOzetiAl()), ref paketOzetiDogrulamaSonucu);
        ///         var nihaiOzetDogrulamasi = paket.NihaiOzetDogrula(Araclar.NihaiOzetAl(paket.NihaiOzetAl()), ref nihaiOzetDogrulamaSonucu);
        ///         var balgeId = paket.Ustveri.BelgeIdAl();
        ///         var belgeNo = paket.Ustveri.BelgeNoAl();
        ///         var dagitimlar = paket.Ustveri.DagitimlariAl();
        ///         var dil = paket.Ustveri.DilAl();
        ///         var ekler = paket.Ustveri.EkleriAl();
        ///         var guvenlikGecerlilikTarihi = paket.Ustveri.GuvenlikGecerlilikTarihiAl();
        ///         var guvenlikKodu = paket.Ustveri.GuvenlikKoduAl();
        ///         var ilgiler = paket.Ustveri.IlgileriAl();
        ///         var ilgililer = paket.Ustveri.IlgilileriAl();
        ///         var konu = paket.Ustveri.KonuAl();
        ///         var mimeTuru = paket.Ustveri.MimeTuruAl();
        ///         var olusturan = paket.Ustveri.OlusturanAl();
        ///         var ozId = paket.Ustveri.OzIdAl();
        ///         var tarih = paket.Ustveri.TarihAl();
        ///         var ustVeri = paket.UstveriAl();
        ///         var ustYazi = paket.UstYaziAl();
        ///         var belgeHedef = paket.BelgeHedefiAl();
        ///         using (Stream s = File.OpenWrite(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu) + "\\" + "Ustveri.xml"))
        ///         {
        ///             Dpt.eYazisma.Tipler.Araclar.CopyStream(ustVeri, s);
        ///         }
        ///         using (Stream s = File.OpenWrite(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu) + "\\" + "ustyazi.pdf"))
        ///         {
        ///             Dpt.eYazisma.Tipler.Araclar.CopyStream(ustYazi, s);
        ///         }
        ///         using (Stream s = File.OpenWrite(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu) + "\\" + "BelgeHedef.xml"))
        ///         {
        ///             Dpt.eYazisma.Tipler.Araclar.CopyStream(belgeHedef, s);
        ///         }
        ///         using (Stream s = File.OpenWrite(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu) + "\\" + "BelgeImza.xml"))
        ///         {
        ///             Dpt.eYazisma.Tipler.Araclar.CopyStream(belgeImza, s);
        ///         }
        ///     }
        /// } 
        /// </code>
        /// </example>
        public static Paket Ac(Stream stream, PaketModu paketModu)
        {
            switch (paketModu)
            {
                case PaketModu.Guncelle:
                    var paket = new Paket
                    {
                        _package = Package.Open(stream, FileMode.Open, FileAccess.ReadWrite)
                    };
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).Count() == 1)
                    {
                        try
                        {
                            CT_Ustveri readedUstveri = (CT_Ustveri)(new XmlSerializer(typeof(CT_Ustveri))).Deserialize(paket._package.GetPart(PackUriHelper.CreatePartUri(paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).First().TargetUri)).GetStream(FileMode.Open));
                            paket.Ustveri = new UstveriInternal(readedUstveri);
                        }
                        catch (Exception ex)
                        {
                            var t = new Exception();

                            throw new Exception("Geçersiz e-Yazışma paketi. \"Üstveri\" bileşeni hatalı.", ex);
                        }
                    }
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEHEDEF).Count() == 1)
                    {
                        try
                        {
                            CT_BelgeHedef readedBelgeHedef = (CT_BelgeHedef)(new XmlSerializer(typeof(CT_BelgeHedef))).Deserialize(paket._package.GetPart(PackUriHelper.CreatePartUri(paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEHEDEF).First().TargetUri)).GetStream(FileMode.Open));
                            paket.BelgeHedef = new BelgeHedefInternal(readedBelgeHedef);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Geçersiz e-Yazışma paketi. \"BelgeHedef\" bileşeni hatalı.", ex);
                        }

                    }
                    else
                    {
                        paket.BelgeHedef = new BelgeHedefInternal();
                    }
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEIMZA).Count() == 1)
                    {
                        try
                        {
                            CT_BelgeImza readedBelgeImza = (CT_BelgeImza)(new XmlSerializer(typeof(CT_BelgeImza))).Deserialize(paket._package.GetPart(PackUriHelper.CreatePartUri(paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEIMZA).First().TargetUri)).GetStream(FileMode.Open));
                            paket.BelgeImza = new BelgeImzaInternal(readedBelgeImza);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Geçersiz e-Yazışma paketi. \"BelgeImza\" bileşeni hatalı.", ex);
                        }

                    }
                    else
                    {
                        paket.BelgeImza = new BelgeImzaInternal();
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
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIOZET).Count() == 1)
                    {
                        try
                        {
                            CT_NihaiOzet readedNihaiOzet = (CT_NihaiOzet)(new XmlSerializer(typeof(CT_NihaiOzet))).Deserialize(paket._package.GetPart(PackUriHelper.CreatePartUri(paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIOZET).First().TargetUri)).GetStream(FileMode.Open));
                            paket.NihaiOzet = new NihaiOzetInternal(readedNihaiOzet);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Geçersiz e-Yazışma paketi. \"NihaiÖzet\" bileşeni hatalı.", ex);
                        }
                    }
                    paket._paketModu = paketModu;
                    paket._streamInternal = stream;
                    return paket;
                case PaketModu.Ac:
                    var paketAcilan = new Paket
                    {
                        _package = Package.Open(stream, FileMode.Open, FileAccess.Read)
                    };
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).Count() != 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"Üstveri\" bileşeni yok veya birden fazla.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEHEDEF).Count() != 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"BelgeHedef\" bileşeni yok veya birden fazla.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).Count() != 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"ÜstYazı\" bileşeni yok veya birden fazla.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_CORE).Count() != 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"Core\" bileşeni yok veya birden fazla.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEIMZA).Count() > 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"BelgeImza\" bileşeni yok veya birden fazla olamaz.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PAKETOZETI).Count() != 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"PaketÖzeti\" bileşeni yok veya birden fazla.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIOZET).Count() > 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"NihaiÖzet\" bileşeni birden fazla olamaz.");
                    }
                    try
                    {
                        Uri readedUstveriUriAcilan = PackUriHelper.CreatePartUri(paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).First().TargetUri);
                        CT_Ustveri readedUstveriAcilan = (CT_Ustveri)(new XmlSerializer(typeof(CT_Ustveri))).Deserialize(paketAcilan._package.GetPart(readedUstveriUriAcilan).GetStream(FileMode.Open));
                        paketAcilan.Ustveri = new UstveriInternal(readedUstveriAcilan);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"Ustveri\" bileşeni hatalı.", ex);
                    }
                    try
                    {
                        Uri readedBelgeHedefUriAcilan = PackUriHelper.CreatePartUri(paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEHEDEF).First().TargetUri);
                        CT_BelgeHedef readedBelgeHedefAcilan = (CT_BelgeHedef)(new XmlSerializer(typeof(CT_BelgeHedef))).Deserialize(paketAcilan._package.GetPart(readedBelgeHedefUriAcilan).GetStream(FileMode.Open));
                        paketAcilan.BelgeHedef = new BelgeHedefInternal(readedBelgeHedefAcilan);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"BelgeHedef\" bileşeni hatalı.", ex);
                    }
                    try
                    {
                        Uri readedPaketOzetiUriAcilan = PackUriHelper.CreatePartUri(paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PAKETOZETI).First().TargetUri);
                        CT_PaketOzeti readedPaketOzetiAcilan = (CT_PaketOzeti)(new XmlSerializer(typeof(CT_PaketOzeti))).Deserialize(paketAcilan._package.GetPart(readedPaketOzetiUriAcilan).GetStream(FileMode.Open));
                        paketAcilan.PaketOzeti = new PaketOzetiInternal(readedPaketOzetiAcilan);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"PaketOzeti\" bileşeni hatalı.", ex);
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIOZET).Count() != 0)
                    {
                        try
                        {
                            Uri readedNihaiOzetUriAcilan = PackUriHelper.CreatePartUri(paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIOZET).First().TargetUri);
                            CT_NihaiOzet readedNihaiOzetAcilan = (CT_NihaiOzet)(new XmlSerializer(typeof(CT_NihaiOzet))).Deserialize(paketAcilan._package.GetPart(readedNihaiOzetUriAcilan).GetStream(FileMode.Open));
                            paketAcilan.NihaiOzet = new NihaiOzetInternal(readedNihaiOzetAcilan);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Geçersiz e-Yazışma paketi. \"NihaiOzet\" bileşeni hatalı.", ex);
                        }

                    }
                    else
                    {
                        paketAcilan.NihaiOzet = new NihaiOzetInternal();
                    }

                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEIMZA).Count() != 0)
                    {
                        try
                        {
                            Uri readedBelgeImzaUriAcilan = PackUriHelper.CreatePartUri(paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEIMZA).First().TargetUri);
                            CT_BelgeImza readedBelgeImzaAcilan = (CT_BelgeImza)(new XmlSerializer(typeof(CT_BelgeImza))).Deserialize(paketAcilan._package.GetPart(readedBelgeImzaUriAcilan).GetStream(FileMode.Open));
                            paketAcilan.BelgeImza = new BelgeImzaInternal(readedBelgeImzaAcilan);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Geçersiz e-Yazışma paketi. \"BelgeImza\" bileşeni hatalı.", ex);
                        }
                    }
                    else
                    {
                        paketAcilan.BelgeImza = new BelgeImzaInternal();
                    }

                    paketAcilan._paketModu = paketModu;
                    paketAcilan._streamInternal = stream;

                    return paketAcilan;

                case PaketModu.Olustur:
                    var paketOlusturulan = new Paket
                    {
                        Ustveri = new UstveriInternal(),
                        PaketOzeti = new PaketOzetiInternal(),
                        NihaiOzet = new NihaiOzetInternal(),
                        BelgeImza = new BelgeImzaInternal(),
                        BelgeHedef = new BelgeHedefInternal(),
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
        public static Paket Ac(string dosyaYolu, PaketModu paketModu)
        {
            switch (paketModu)
            {
                case PaketModu.Guncelle:
                    return Paket.Ac(File.Open(dosyaYolu, FileMode.Open, FileAccess.ReadWrite), paketModu);
                case PaketModu.Ac:
                    return Paket.Ac(File.Open(dosyaYolu, FileMode.Open), paketModu);
                case PaketModu.Olustur:
                    return Paket.Ac(File.Open(dosyaYolu, FileMode.OpenOrCreate, FileAccess.ReadWrite), paketModu);
                default:
                    return null;
            }

        }
        /// <summary>Üstveri'de ek olarak belirtilmiş ilgiye ilişkin ekin paket içerisinde bulunup bulunmadığını kontrol eder.</summary>
        /// <remarks>Paket oluşturma sırasında, <see cref="Paket.Kapat"/> metodundan önce kullanılmalıdır.</remarks>
        /// <exception cref="System.ApplicationException">Ek olarak belirtilmiş ilgiye ait ek paket içinde yoksa oluşur.</exception>
        public void IlgileriKontrolEt()
        {
            if (Ustveri.CT_Ustveri.Ilgiler != null)
            {
                var ekOlarakBelirtilmisIlgiler = Ustveri.CT_Ustveri.Ilgiler.Where(ilgi => !(ilgi.EkId.IsNullOrWhiteSpace()));
                foreach (var ekOlarakBelirtilmisIlgi in ekOlarakBelirtilmisIlgiler)
                {
                    if (Ustveri.CT_Ustveri.Ekler.Where(ek => string.Compare(ek.Id.Value.ToString(), ekOlarakBelirtilmisIlgi.EkId, true) == 0).Count() == 0)
                    {
                        throw new ApplicationException("Üstveri bileşeninde verilen ilgilerden, ek olarak belirtilmiş ilgi için, paket içerisine eklenmiş Ek bileşeni bulunamadı.");
                    }
                }
                if (Ustveri.CT_Ustveri.Ilgiler != null && Ustveri.CT_Ustveri.Ilgiler.Select(x => x.Etiket).Distinct().Count() < Ustveri.CT_Ustveri.Ilgiler.Count())
                {
                    throw new ApplicationException("Paket içerisine aynı Etiket değerine sahip birden fazla ilgi eklenemez.");
                }
            }
        }
        // TODO No.7 MimeTuru verilmeyen harici bir ek eklenebilmeli, DED ise mimeturu zorunlu olmali. Ama HRF olup ve mimeturu olmayan bir ek, denedik eklendi. ama koda bakiyotum sanki hata vermesi lazim, Satir 1107
        // TODO No.13 Geregi ve bilgi yer degistirdi, documan ve API dokumanstasyonu degisesek
        /// <summary>Üstveri'de Dahili Elektronik Dosya türünden ek olarak belirtilmiş ekin paket içerisinde bulunup bulunmadığını kontrol eder. Pakete eklenmiş ek dosyalarının üstveride belirtilip belirtilmediğini kontrol eder.</summary>
        /// <remarks>Paket oluşturma sırasında, <see cref="Paket.Kapat"/> metodundan önce kullanılmalıdır.</remarks>
        /// <exception cref="System.ApplicationException">Üstveri ile paket içerisindeki eklerin uyumsuz olması durumunda oluşur.</exception>
        public void EkleriKontrolEt()
        {
            if (this.Ustveri.CT_Ustveri.Ekler != null)
            {
                foreach (var ustveriEki in this.Ustveri.CT_Ustveri.Ekler)
                {
                    if (ustveriEki.Tur == ST_KodEkTuru.DED)
                    {
                        if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_EK).Where(xRelationship => String.Compare(xRelationship.Id.ToString(), Araclar.ID_ROOT_EK + ustveriEki.Id.Value, true) == 0).Count() == 0)
                        {
                            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_IMZASIZEK).Where(xRelationship => String.Compare(xRelationship.Id.ToString(), Araclar.ID_ROOT_IMZASIZEK + ustveriEki.Id.Value, true) == 0).Count() == 0)
                            {
                                throw new ApplicationException(String.Format("Üstveri bileşeni için eklenen ek, paket içerisine eklenmemiş. EkId:{0}", ustveriEki.Id.Value));
                            }
                        }
                    }
                }
            }
            foreach (var relationship in _package.GetRelationshipsByType(Araclar.RELATION_TYPE_EK))
            {
                if (Ustveri.CT_Ustveri.Ekler == null)
                {
                    throw new ApplicationException("Paket içerisine eklenmiş eklerin hiç biri, üstveri bileşeninde belirtilmemiş.");
                }
                var UstveriEkleri = Ustveri.CT_Ustveri.Ekler.Where(ustveriEki => String.Compare(relationship.Id.ToString(), Araclar.ID_ROOT_EK + ustveriEki.Id.Value, true) == 0);
                if (UstveriEkleri.Count() == 0)
                {
                    throw new ApplicationException("Paket içerisine eklenmiş ek, üstveri bileşeninde belirtilmemiş.");
                }
                else
                {
                    if (UstveriEkleri.First().Tur != ST_KodEkTuru.DED)
                    {
                        throw new ApplicationException("Paket içerisine eklenmiş ek, üstveri bileşeninde DED (Dahili Elektronik Dosya) olarak belirtilmelidir.");
                    }
                }
            }
            foreach (var relationship in _package.GetRelationshipsByType(Araclar.RELATION_TYPE_IMZASIZEK))
            {
                if (Ustveri.CT_Ustveri.Ekler == null)
                {
                    throw new ApplicationException("Paket içerisine eklenmiş eklerin hiç biri, üstveri bileşeninde belirtilmemiş.");
                }
                var UstveriEkleri = Ustveri.CT_Ustveri.Ekler.Where(ustveriEki => String.Compare(relationship.Id.ToString(), Araclar.ID_ROOT_IMZASIZEK + ustveriEki.Id.Value, true) == 0);
                if (UstveriEkleri.Count() == 0)
                {
                    throw new ApplicationException("Paket içerisine eklenmiş ek, üstveri bileşeninde belirtilmemiş.");
                }
                else
                {
                    if (UstveriEkleri.First().Tur != ST_KodEkTuru.DED)
                    {
                        throw new ApplicationException("Paket içerisine eklenmiş ek, üstveri bileşeninde DED (Dahili Elektronik Dosya) olarak belirtilmelidir.");
                    }
                }
            }
            if (Ustveri.CT_Ustveri.Ekler != null && Ustveri.CT_Ustveri.Ekler.Where(x => x.SiraNo <= 0).Count() > 0)
            {
                throw new ApplicationException("Paket içerisine SiraNo değeri '1'den küçük olan ek eklenemez.");
            }
            if (Ustveri.CT_Ustveri.Ekler != null && Ustveri.CT_Ustveri.Ekler.Select(x => x.SiraNo).Distinct().Count() < Ustveri.CT_Ustveri.Ekler.Count())
            {
                throw new ApplicationException("Paket içerisine aynı SiraNo değerine sahip birden fazla ek eklenemez.");
            }
            foreach (var item in Ustveri.CT_Ustveri.DagitimListesi.Where(x => x.KonulmamisEkListesi != null && x.KonulmamisEkListesi.Length > 0).Select(x => x.KonulmamisEkListesi).SelectMany(x => x))
            {
                if (Ustveri.CT_Ustveri.Ekler.Where(x => x.Id.Value.ToString().ToUpperInvariant() == item.EkId.ToUpperInvariant()).Count() == 0)
                {
                    throw new ApplicationException("UstVeri'de belirtilmemiş bir ek, DagitimListesinde KonulmamisEk olarak belirtilemez.");
                }
            }
        }
        /// <summary>Core bileşeninin 'serialize' edilerek paket içerisine eklenmesini sağlar.</summary>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        /// <remarks> Java API için açıklama. .Net API kullanıyorsanız bu açıklamyı dikkate almayınız. 
        /// Kullanılan Apache POI API'sı, 'core' bileşeninin oluşturulması aşamasına müdahaleye engel olduğundan dolayı, CoreuNihaiOzeteEkle yardımcı metod kullanılarak Core bileşeni özeti NihaiOzet'e eklenir. </remarks>
        public void CoreOlustur()
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için Core fonksiyonu kullanılamaz.");
            }

            PaketBelirteciBelirle(Ustveri.CT_Ustveri.BelgeId);
            PaketOlusturanBelirle(Araclar.OlusturanAdiOlustur(Ustveri.CT_Ustveri.Olusturan));
            PaketKonusuBelirle(Ustveri.CT_Ustveri.Konu.Value);
            PaketKategorisiBelirle(Araclar.RESMIYAZISMA);
            PaketIcerikTuruBelirle(Araclar.EYAZISMAMIMETURU);
            PaketVersiyonuBelirle("1.3");
            PaketRevizyonuBelirle("DotNet/" + System.Reflection.Assembly.GetAssembly(typeof(Paket)).GetName().Version.ToString());

            _package.Flush();

            var coreRelations = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_CORE);
            if (coreRelations == null || coreRelations.Count() == 0)
            {
                throw new ApplicationException("Core bileşeni ilişkisi alınamadı.");
            }
            var corePart = _package.GetPart(coreRelations.First().TargetUri);
            var stream = corePart.GetStream();
            stream.Position = 0;
            using (var memoryStream = new MemoryStream())
            {
                Araclar.CopyStream(stream, memoryStream);
                memoryStream.Position = 0;
                byte[] ozet = Araclar.OzetHesapla(memoryStream, VarsayilanOzetModu);
                NihaiOzet.Ekle(VarsayilanOzetModu, ozet, coreRelations.First().TargetUri);
            }
        }
        /// <summary>PaketOzeti bileşeninin 'serialize' edilerek paket içerisine eklenmesini sağlar.</summary>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        public void PaketOzetiOlustur()
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new ApplicationException("PaketModu 'Ac' olarak işlem yapılan paketler için PaketOzetiOlustur fonksiyonu kullanılamaz.");
            }
            PaketOzeti.CT_PaketOzeti.Id = Ustveri.BelgeIdAl().ToUpperInvariant();
            PaketOzeti.KontrolEt();
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
            using (var memoryStream = new MemoryStream())
            {
                var x = new XmlSerializer(typeof(CT_PaketOzeti));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xmlTextWriter.Formatting = Formatting.Indented;
                x.Serialize(xmlTextWriter, PaketOzeti.CT_PaketOzeti);

                memoryStream.Flush();
                memoryStream.Position = 0;
                Araclar.CopyStream(memoryStream, partPaketOzeti.GetStream());
                memoryStream.Position = 0;
                byte[] ozet = Araclar.OzetHesapla(memoryStream, VarsayilanOzetModu);
                NihaiOzet.Ekle(VarsayilanOzetModu, ozet, partPaketOzeti.Uri);
            }
        }
        /// <summary>NihaiOzet bileşeninin 'serialize' edilerek paket içerisine eklenmesini sağlar.</summary>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        public void NihaiOzetOlustur()
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için NihaiOzetOlustur fonksiyonu kullanılamaz.");
            }
            NihaiOzet.CT_NihaiOzet.Id = Ustveri.BelgeIdAl().ToUpperInvariant();
            NihaiOzet.KontrolEt();
            var partUriNihaiOzet = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative));
            if (_paketModu == PaketModu.Guncelle)
            {
                if (_package.PartExists(partUriNihaiOzet))
                {
                    _package.DeletePart(partUriNihaiOzet);
                    _package.DeleteRelationship(Araclar.ID_NIHAIOZETI);
                }
            }
            var partNihaiOzet = _package.CreatePart(partUriNihaiOzet, Araclar.MIME_XML, CompressionOption.Maximum);
            _package.CreateRelationship(partNihaiOzet.Uri, TargetMode.Internal, Araclar.RELATION_TYPE_NIHAIOZET, Araclar.ID_NIHAIOZETI);
            using (var memoryStream = new MemoryStream())
            {
                var x = new XmlSerializer(typeof(CT_NihaiOzet));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xmlTextWriter.Formatting = Formatting.Indented;
                x.Serialize(xmlTextWriter, NihaiOzet.CT_NihaiOzet);
                memoryStream.Flush();
                memoryStream.Position = 0;
                Araclar.CopyStream(memoryStream, partNihaiOzet.GetStream());

            }
        }
        /// <summary>BelgeImza bileşeninin 'serialize' edilerek paket içerisine eklenmesini sağlar.</summary>
        /// <remarks>BelgeImza bileşeninin varsayılan OzetModu ile özet değeri hesaplanarak NihaiOzet objesine eklenir. Oluşturma sırasında BelgeImza bileşeni olası hatalara karşı kontrol edilir.</remarks>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        public void BelgeImzaOlustur()
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için BelgeImzaOlustur fonksiyonu kullanılamaz.");
            }
            BelgeImza.KontrolEt();
            var partUriBelgeImza = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_BELGEIMZA, UriKind.Relative));
            if (_paketModu == PaketModu.Guncelle)
            {
                if (_package.PartExists(partUriBelgeImza))
                {
                    _package.DeletePart(partUriBelgeImza);
                    _package.DeleteRelationship(Araclar.ID_BELGEIMZA);
                }
            }
            var partBelgeImza = _package.CreatePart(partUriBelgeImza, Araclar.MIME_XML, CompressionOption.Maximum);
            _package.CreateRelationship(partBelgeImza.Uri, TargetMode.Internal, Araclar.RELATION_TYPE_BELGEIMZA, Araclar.ID_BELGEIMZA);
            using (var memoryStream = new MemoryStream())
            {
                XmlSerializerNamespaces ss = new XmlSerializerNamespaces();
                ss.Add("tipler", "urn:dpt:eyazisma:schema:xsd:Tipler-1");
                var x = new XmlSerializer(typeof(CT_BelgeImza));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xmlTextWriter.Formatting = Formatting.Indented;
                x.Serialize(xmlTextWriter, BelgeImza.CT_BelgeImza, ss);
                memoryStream.Flush();
                memoryStream.Position = 0;
                Araclar.CopyStream(memoryStream, partBelgeImza.GetStream());
                memoryStream.Position = 0;
                byte[] ozet = Araclar.OzetHesapla(memoryStream, VarsayilanOzetModu);
                NihaiOzet.Ekle(VarsayilanOzetModu, ozet, partBelgeImza.Uri);
            }
        }
        /// <summary>BelgeHedef bileşeninin 'serialize' edilerek paket içerisine eklenmesini sağlar.</summary>
        /// <remarks>BelgeHedef bileşeninin varsayılan OzetModu ile özet değeri hesaplanarak PaketOzeti ve NihaiOzet objesine eklenir. Oluşturma sırasında BelgeHedef bileşeni olası hatalara karşı kontrol edilir.</remarks>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        public void BelgeHedefOlustur()
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için BelgeHedefOlustur fonksiyonu kullanılamaz.");
            }
            BelgeHedef.KontrolEt();
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
            using (var memoryStream = new MemoryStream())
            {
                XmlSerializerNamespaces ss = new XmlSerializerNamespaces();
                ss.Add("tipler", "urn:dpt:eyazisma:schema:xsd:Tipler-1");
                var x = new XmlSerializer(typeof(CT_BelgeHedef));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xmlTextWriter.Formatting = Formatting.Indented;
                x.Serialize(xmlTextWriter, BelgeHedef.CT_BelgeHedef, ss);
                memoryStream.Flush();
                memoryStream.Position = 0;
                Araclar.CopyStream(memoryStream, partBelgeHedef.GetStream());
                memoryStream.Position = 0;
                byte[] ozet = Araclar.OzetHesapla(memoryStream, VarsayilanOzetModu);
                PaketOzeti.Ekle(VarsayilanOzetModu, ozet, partBelgeHedef.Uri);
                NihaiOzet.Ekle(VarsayilanOzetModu, ozet, partBelgeHedef.Uri);
            }
        }
        /// <summary>İşlem yapılan paket için sonladırma işlemi yapılarak açılan kaynaklar kapatılır.</summary>
        public void Kapat()
        {
            if (_paketModu == PaketModu.Olustur)
            {
                var ustYaziRelation = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI);
                if (ustYaziRelation == null || ustYaziRelation.Count() == 0)
                {
                    throw new Exception("Üst yazı eklenmemiş.");
                }
                _package.Close();
            }
            else if (_paketModu == PaketModu.Guncelle)
            {
                _package.Close();
            }
            else if (_paketModu == PaketModu.Ac)
            {
                _package.Close();
            }

        }
        #endregion
        #region Üstveri
        /// <summary>   Paket içerisindeki Üstveri elemanını STREAM olarak verir. </summary>
        /// <returns>   Üstveri bileşenine ait STREAM nesnesini döner. Bileşenin bulunmaması durumunda null döner. </returns>
        public Stream UstveriAl()
        {
            if (_package.PartExists(new Uri(Araclar.URI_USTVERI, UriKind.Relative)))
            {
                return _package.GetPart(new Uri(Araclar.URI_USTVERI, UriKind.Relative)).GetStream();
            }
            return null;
        }
        /// <summary>Ustveri bileşeninin 'serialize' edilerek paket içerisine eklenmesini sağlar.</summary>
        /// <remarks>Ustveri bileşeninin varsayılan OzetModu ile özet değeri hesaplanarak PaketOzeti ve NihaiOzet objesine eklenir. Oluşturma sırasında Ustveri bileşeni olası hatalara karşı kontrol edilir.</remarks>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        public void UstveriOlustur()
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new ApplicationException("PaketModu 'Ac' olarak işlem yapılan paketler için UstveriOlustur fonksiyonu kullanılamaz.");
            }
            Ustveri.KontrolEt();
            var partUriUstveri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_USTVERI, UriKind.Relative));
            if (_paketModu == PaketModu.Guncelle)
            {
                if (_package.PartExists(partUriUstveri))
                {
                    _package.DeletePart(partUriUstveri);
                    _package.DeleteRelationship(Araclar.ID_USTVERI);
                }
            }
            var partUstveri = _package.CreatePart(partUriUstveri, Araclar.MIME_XML, CompressionOption.Normal);
            _package.CreateRelationship(partUstveri.Uri, TargetMode.Internal, Araclar.RELATION_TYPE_USTVERI, Araclar.ID_USTVERI);
            using (var memoryStream = new MemoryStream())
            {
                XmlSerializerNamespaces ss = new XmlSerializerNamespaces();
                ss.Add("tipler", "urn:dpt:eyazisma:schema:xsd:Tipler-1");
                var x = new XmlSerializer(typeof(CT_Ustveri));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xmlTextWriter.Formatting = Formatting.Indented;
                x.Serialize(xmlTextWriter, Ustveri.CT_Ustveri, ss);
                memoryStream.Flush();
                memoryStream.Position = 0;
                Araclar.CopyStream(memoryStream, partUstveri.GetStream());
                memoryStream.Position = 0;
                byte[] ozet = Araclar.OzetHesapla(memoryStream, VarsayilanOzetModu);
                PaketOzeti.Ekle(VarsayilanOzetModu, ozet, partUstveri.Uri);
                NihaiOzet.Ekle(VarsayilanOzetModu, ozet, partUstveri.Uri);
            }
        }
        #endregion
        #region Ust Yazi
        /// <summary>   Paket içerisine üst yazı bileşeni ekler. </summary>
        /// <param name="dosyaYolu">    Üst yazı bileşenine ilişkin dosya yoludur. </param>
        /// <param name="mimeTuru">     Üst yazı bileşeni dosyasının mime türüdür. </param>
        /// <param name="ozetModu">     Eklenen üst yazıya ait özet değerinin paket özeti bileşenine hangi algoritma ile ekleneceğini belirtir. </param>
        /// ### <exception cref="SystemException">          Daha önceden üst yazı eklenmiş bir pakete tekrar üst yazı eklenemez. </exception>
        /// ### <exception cref="ArgumentNullException">    Dosya yolu boş veya Mime türü null olamaz. </exception>
        /// ### <exception cref="Exception">                Açma modunda kullanıldığında EXCEPTIONoluşur. </exception>
        public void UstYaziEkle(string dosyaYolu, String mimeTuru, OzetModu ozetModu = OzetModu.SHA256)
        {
            using (var fileStream = new FileStream(dosyaYolu, FileMode.Open, FileAccess.Read))
            {
                UstYaziEkle(fileStream, System.IO.Path.GetFileName(dosyaYolu), mimeTuru, ozetModu);
            }
        }
        /// <summary>   Paket içerisine üst yazı bileşeni ekler. </summary>
        /// <exception cref="SystemException">          Daha önceden üst yazı eklenmiş bir pakete tekrar üst yazı eklenemez. </exception>
        /// <exception cref="ArgumentNullException">    Dosya yolu ve mime türü  boş veya null olamaz. </exception>
        /// <exception cref="Exception">                Açma modunda kullanıldığında EXCEPTION oluşur. </exception>
        /// <param name="dosya">    Üst yazı bileşenine ilişkin STREAM'dir. </param>
        /// <param name="dosyaAdi"> Eklenen üst yazı dosyasının adıdır. Dosya adında bulunan boşluklar kaldırılır ve Türkçe karakterler İngilizce olanlarla değiştirilir.</param>
        /// <param name="mimeTuru"> Üst yazı bileşeni dosyasının mime türüdür. </param>
        /// <param name="ozetModu"> Eklenen üst yazıya ait özet değerinin paket özeti bileşenine hangi algoritma ile ekleneceğini belirtir. </param>
        public void UstYaziEkle(Stream dosya, String dosyaAdi, String mimeTuru, OzetModu ozetModu = OzetModu.SHA256)
        {
            var ustYaziRelation = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI);
            if (ustYaziRelation != null && ustYaziRelation.Count() > 0)
            {
                throw new SystemException("Daha önce üstyazı eklenmiş.");
            }
            if (dosya == null)
                throw new ArgumentNullException("dosya");
            if (dosyaAdi == null)
                throw new ArgumentNullException("dosyaAdi");
            if (mimeTuru == null)
                throw new ArgumentNullException("mimeTuru");
            if (ozetModu == OzetModu.Yok)
                throw new ArgumentException("OzetModu YOK olamaz.");
            if (_paketModu == PaketModu.Ac)
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için UstYaziEkle fonksiyonu kullanılamaz.");
            var assemblyPartUri = PackUriHelper.CreatePartUri(new Uri(string.Format(Araclar.URI_FORMAT_USTYAZI, Araclar.EncodePath(dosyaAdi)), UriKind.Relative));
            var part = _package.CreatePart(assemblyPartUri, mimeTuru, CompressionOption.Maximum);
            Araclar.CopyStream(dosya, part.GetStream());
            _package.CreateRelationship(part.Uri, TargetMode.Internal, Araclar.RELATION_TYPE_USTYAZI, Araclar.ID_USTYAZI);
            Ustveri.MimeTuruBelirle(mimeTuru);
            dosya.Position = 0;
            byte[] ozetDegeri = Araclar.OzetHesapla(dosya, ozetModu);
            PaketOzeti.Ekle(ozetModu, ozetDegeri, part.Uri);
            NihaiOzet.Ekle(ozetModu, ozetDegeri, part.Uri);
        }
        /// <summary>   Paket içerisindeki üst yazı bileşenini STREAM olarak döner. </summary>
        /// <returns>   Paket içerisindeki üst yazı bileşeni STREAM'i. UstYazi bileşeni yoksa null döner. </returns>
        public Stream UstYaziAl()
        {
            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).Count() == 1)
            {
                return _package.GetPart(PackUriHelper.CreatePartUri(_package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).First().TargetUri)).GetStream();
            }
            return null;
        }
        #endregion
        #region Ek
        /// <summary>   Paket içerisine ek bileşeni ekler. </summary>
        /// <exception cref="ArgumentNullException">    Dosya yolu boş veya null olamaz. </exception>
        /// <exception cref="Exception">                Verilen ek dosyası bulunamazsa EXCEPTION oluşur. </exception>
        /// <param name="ek">           Eklenen ek bileşenine ait üstveriyi barındıran objedir. </param>
        /// <param name="dosyaYolu">    Ek bileşenine ilişkin dosya yoludur. </param>
        /// <param name="ozetModu">     Eklenen ek bileşenine ait özet değerinin paket özeti bileşenine
        ///                             hangi algoritma ile ekleneceğini belirtir. </param>
        /// ### <exception cref="ArgumentNullException">    Ek objesi null ise EXCEPTION oluşur. </exception>
        /// ### <exception cref="ArgumentNullException">    Ek objesi mime türü null ise EXCEPTION
        ///                                                 oluşur. </exception>
        /// ### <exception cref="ArgumentNullException">    Ek objesi ID null ise EXCEPTION oluşur. </exception>
        /// ### <exception cref="Exception">                PaketModu 'Ac' olarak işlem yapılan paketler
        ///                                                 için EkEkle fonksiyonu kullanılamaz. </exception>
        public void EkEkle(CT_Ek ek, string dosyaYolu, OzetModu ozetModu = OzetModu.SHA256)
        {
            if (dosyaYolu == null)
                throw new ArgumentNullException("dosyaYolu");
            if (!(File.Exists(dosyaYolu)))
                throw new Exception("Dosya bulunamadı.");
            if (ek == null)
                throw new ArgumentNullException("ek");
            if (ek.MimeTuru.IsNullOrWhiteSpace())
                throw new ArgumentNullException("ek.MimeTuru");
            if (ek.Id == null || ek.Id.Value.IsNullOrWhiteSpace())
                throw new ArgumentNullException("ek.Item");
            if (_paketModu == PaketModu.Ac)
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için EkEkle fonksiyonu kullanılamaz.");
            using (var t = System.IO.File.OpenRead(dosyaYolu))
            {
                EkEkle(ek, t, System.IO.Path.GetFileName(dosyaYolu), ozetModu);
            }
        }
        /// <summary>   Paket içerisine ek bileşeni ekler. </summary>
        /// <exception cref="ArgumentNullException">    Dosya null olamaz. </exception>
        /// <param name="ek">       Eklenen ek bileşenine ait üstveriyi barındıran objedir. </param>
        /// <param name="dosya">    Ek bileşenine ilişkin STREAM'dir. </param>
        /// <param name="dosyaAdi"> Eklenen ek bileşeni dosyasının adıdır. Dosya adında bulunan boşluklar kaldırılır ve Türkçe karakterler İngilizce olanlarla değiştirilir.</param>
        /// <param name="ozetModu"> Eklenen ek bileşenine ait özet değerinin paket özeti bileşenine hangi
        ///                         algoritma ile ekleneceğini belirtir. </param>
        /// ### <exception cref="ArgumentNullException">    Dosya adı null ise EXCEPTION oluşur. </exception>
        /// ### <exception cref="ArgumentNullException">    Ek objesi null ise EXCEPTION oluşur. </exception>
        /// ### <exception cref="ArgumentNullException">    Ek objesi mime türü null ise EXCEPTION
        ///                                                 oluşur. </exception>
        /// ### <exception cref="ArgumentNullException">    Ek objesi ID null ise EXCEPTION oluşur. </exception>
        /// ### <exception cref="Exception">                PaketModu 'Ac' olarak işlem yapılan paketler
        ///                                                 için EkEkle fonksiyonu kullanılamaz. </exception>
        public void EkEkle(CT_Ek ek, Stream dosya, String dosyaAdi, OzetModu ozetModu = OzetModu.SHA256)
        {
            if (dosya == null)
                throw new ArgumentNullException("dosya");
            if (dosyaAdi.IsNullOrWhiteSpace())
                throw new ArgumentNullException("dosyaAdi");
            try
            {
                string fileName = Path.GetFileName(dosyaAdi);
                if (fileName == null)
                {
                    throw new ArgumentException("dosyaAdi");
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("dosyaAdi");
            }
            if (ek == null)
                throw new ArgumentNullException("ek");
            if (ek.MimeTuru.IsNullOrWhiteSpace())
                throw new ArgumentNullException("ek.MimeTuru");
            if (ek.Id == null || ek.Id.Value.IsNullOrWhiteSpace())
                throw new ArgumentNullException("ek.Item");
            if (ek.ImzaliMiSpecified == true && ek.ImzaliMi == true && ozetModu == OzetModu.Yok)
            {
                throw new ApplicationException("İmzalı bir ek, ÖzetModu değeri verilmeksizin pakete eklenemez.");
            }
            if (ek.ImzaliMiSpecified == false && ozetModu == OzetModu.Yok)
            {
                throw new ApplicationException("İmzalı bir ek, ÖzetModu değeri verilmeksizin pakete eklenemez.");
            }
            if (ek.ImzaliMiSpecified == true && ek.ImzaliMi == false && ozetModu != OzetModu.Yok)
            {
                throw new ApplicationException("İmzasız bir ek, ÖzetModu değeri verilerek pakete eklenemez.");
            }

            String klasorAdi;
            String iliskiAdi;
            String id;
            if (ozetModu == OzetModu.Yok)
            {
                klasorAdi = Araclar.URI_ROOT_IMZASIZEK;
                iliskiAdi = Araclar.RELATION_TYPE_IMZASIZEK;
                id = Araclar.ID_ROOT_IMZASIZEK;
            }
            else
            {
                klasorAdi = Araclar.URI_ROOT_EK;
                iliskiAdi = Araclar.RELATION_TYPE_EK;
                id = Araclar.ID_ROOT_EK;
            }
            var assemblyPartUri = PackUriHelper.CreatePartUri(new Uri(string.Format("/{0}/{1}", klasorAdi, Araclar.EncodePath(dosyaAdi)), UriKind.Relative));
            var part = _package.CreatePart(assemblyPartUri, ek.MimeTuru, CompressionOption.Maximum);
            Araclar.CopyStream(dosya, part.GetStream());

            _package.CreateRelationship(part.Uri, TargetMode.Internal, iliskiAdi, id + ek.Id.Value.ToUpperInvariant());
            if (ozetModu != OzetModu.Yok)
            {
                dosya.Position = 0;
                byte[] ozetDegeri = Araclar.OzetHesapla(dosya, ozetModu);
                PaketOzeti.Ekle(ozetModu, ozetDegeri, part.Uri);
                NihaiOzet.Ekle(ozetModu, ozetDegeri, part.Uri);
            }

        }
        /// <summary>Id'si verilen eke ait STREAM'i döner.</summary>
        /// <param name="id">Alınmak istenen eke ait Id değeri.</param>
        /// <returns>Alınan eke ait STREAM nesnesi. Ek bulunamaması durumunda null döner.</returns>
        public Stream EkAl(Guid id)
        {
            if (_package.RelationshipExists(Araclar.ID_ROOT_EK + id.ToString().ToUpperInvariant()))
            {
                return _package.GetPart(PackUriHelper.CreatePartUri(_package.GetRelationship(Araclar.ID_ROOT_EK + id.ToString().ToUpperInvariant()).TargetUri)).GetStream();
            }
            if (_package.RelationshipExists(Araclar.ID_ROOT_IMZASIZEK + id.ToString().ToUpperInvariant()))
            {
                return _package.GetPart(PackUriHelper.CreatePartUri(_package.GetRelationship(Araclar.ID_ROOT_IMZASIZEK + id.ToString().ToUpperInvariant()).TargetUri)).GetStream();
            }
            return null;
        }
        /// <summary>Id'si verilen ek paketten çıkartılır.</summary>
        /// <param name="id">Çıkartılmak istenen eke ait Id.</param>
        /// <returns>Çıkarma işlemi başarılı ise true, aksi takdirde false döner.</returns>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        /// <example> Mevcut bir paketten ek çıkartma örneği.
        /// <code>
        /// static void PakettenEkCikar(String dosyaYolu)
        /// {
        ///     using (var paket = Paket.Ac(dosyaYolu, PaketModu.Guncelle))
        ///     {
        ///         paket.EkCikar(new Guid(paket.Ustveri.EkleriAl().First().Id.Value));
        ///         paket.Kapat();
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool EkCikar(Guid id)
        {
            if (_paketModu == PaketModu.Ac)
                throw new ApplicationException("PaketModu 'Ac' olarak işlem yapılan paketler için EkCikar fonksiyonu kullanılamaz.");
            if (_package.RelationshipExists(Araclar.ID_ROOT_EK + id.ToString().ToUpperInvariant()))
            {
                _package.DeletePart(_package.GetRelationship(Araclar.ID_ROOT_EK + id.ToString().ToUpperInvariant()).TargetUri);
                _package.DeleteRelationship(Araclar.ID_ROOT_EK + id.ToString().ToUpperInvariant());
                return true;
            }
            if (_package.RelationshipExists(Araclar.ID_ROOT_IMZASIZEK + id.ToString().ToUpperInvariant()))
            {
                _package.DeletePart(_package.GetRelationship(Araclar.ID_ROOT_IMZASIZEK + id.ToString().ToUpperInvariant()).TargetUri);
                _package.DeleteRelationship(Araclar.ID_ROOT_IMZASIZEK + id.ToString().ToUpperInvariant());
                return true;
            }
            return false;
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

        #region Imza
        /// <summary>Paket içerisine PaketOzeti bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri eklenir.</summary>
        /// <param name="imza">PaketOzeti bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri.</param>
        /// /// <param name="ozetModu">     Eklenen imza bileşenine ait özet değerinin nihai özeti bileşenine hangi algoritma ile ekleneceğini belirtir. </param>
        /// <exception cref="System.ArgumentNullException">İmza değeri null olduğunda oluşur.</exception>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        /// <example> İmzasız bir paketi güncelleme modunda açarak imza ekleme örneği.
        /// <code>
        /// private static void ImzasizPaketeImzaEkle(string dosyaYolu, byte[] imza)
        ///  {
        ///      using (var paket = Paket.Ac(dosyaYolu, PaketModu.Guncelle))
        ///      {
        ///          paket.ImzaEkle(imza);
        ///          paket.Kapat();
        ///      }
        ///  }
        /// </code>
        /// </example>
        public void ImzaEkle(Byte[] imza, OzetModu ozetModu = OzetModu.SHA256)
        {
            if (imza == null)
            {
                throw new ArgumentNullException("imza");
            }
            if (_paketModu == PaketModu.Ac)
                throw new ApplicationException("PaketModu 'Ac' olarak işlem yapılan paketler için ImzaEkle fonksiyonu kullanılamaz.");
            Uri assemblyPartUri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_IMZA, UriKind.Relative));

            if (_package.PartExists(assemblyPartUri))
            {
                _package.DeletePart(assemblyPartUri);
                _package.GetPart(new Uri(Araclar.URI_PAKETOZETI, UriKind.Relative)).DeleteRelationship(Araclar.ID_IMZA);
            }
            PackagePart part02 = _package.CreatePart(assemblyPartUri, Araclar.MIME_OCTETSTREAM, CompressionOption.Maximum);
            using (MemoryStream fileStream = new MemoryStream(imza, 0, imza.Length, false))
            {
                Araclar.CopyStream(fileStream, part02.GetStream());
                fileStream.Position = 0;
                byte[] ozetDegeri = Araclar.OzetHesapla(fileStream, ozetModu);
                NihaiOzet.Ekle(ozetModu, ozetDegeri, assemblyPartUri);
            }
            _package.GetPart(new Uri(Araclar.URI_PAKETOZETI, UriKind.Relative)).CreateRelationship(PackUriHelper.GetRelativeUri(new Uri(Araclar.URI_PAKETOZETI, UriKind.Relative), assemblyPartUri), TargetMode.Internal, Araclar.RELATION_TYPE_IMZA, Araclar.ID_IMZA);
        }
        /// <summary>   Paket içerisindeki PaketOzeti bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri STREAM olarak döner. </summary>
        /// <returns>   Paket içerisindeki PaketOzeti bileşeninin imzalı (Ayrık olmayan CAdES-X-L) STREAM nesnesi. Imzali bileşen olmaması durumunda null döner. </returns>
        public Stream ImzaAl()
        {
            if (_package.PartExists(new Uri(Araclar.URI_IMZA, UriKind.Relative)))
            {
                return _package.GetPart(new Uri(Araclar.URI_IMZA, UriKind.Relative)).GetStream();
            }
            else return null;
        }
        /// <summary>   Paket içerisindeki BelgeImza bileşenini STREAM olarak döner. </summary>
        /// <returns>   Paket içerisindeki BelgeImza bileşeninin STREAM nesnesi. BelgeImza bileşeni olmaması durumunda null döner. </returns>
        public Stream BelgeImzaAl()
        {
            if (_package.PartExists(new Uri(Araclar.URI_BELGEIMZA, UriKind.Relative)))
            {
                return _package.GetPart(new Uri(Araclar.URI_BELGEIMZA, UriKind.Relative)).GetStream();
            }
            return null;
        }
        #endregion
        #region Muhur
        /// <summary>Paket içerisine NihaiOzet bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri eklenir.</summary>
        /// <param name="muhur">NihaiOzet bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri.</param>
        /// <exception cref="System.ArgumentNullException">Muhur değeri null olduğunda oluşur.</exception>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        public void MuhurEkle(Byte[] muhur)
        {
            if (muhur == null)
            {
                throw new ArgumentNullException("muhur");
            }
            if (_paketModu == PaketModu.Ac)
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için MuhurEkle fonksiyonu kullanılamaz.");
            Uri assemblyPartUri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_MUHUR, UriKind.Relative));

            if (_package.PartExists(assemblyPartUri))
            {
                _package.DeletePart(assemblyPartUri);
                _package.GetPart(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative)).DeleteRelationship(Araclar.ID_MUHUR);
            }
            PackagePart part02 = _package.CreatePart(assemblyPartUri, Araclar.MIME_OCTETSTREAM, CompressionOption.Maximum);
            using (MemoryStream fileStream = new MemoryStream(muhur, 0, muhur.Length, false))
            {
                Araclar.CopyStream(fileStream, part02.GetStream());
            }
            _package.GetPart(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative)).CreateRelationship(PackUriHelper.GetRelativeUri(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative), assemblyPartUri), TargetMode.Internal, Araclar.RELATION_TYPE_MUHUR, Araclar.ID_MUHUR);
        }
        /// <summary>   Paket içerisindeki NihaiOzet bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri STREAM olarak döner. </summary>
        /// <returns>   Paket içerisindeki NihaiOzet bileşeninin imzalı (Ayrık olmayan CAdES-X-L) STREAM nesnesi. Mühürlü bileşen olmaması durumunda null döner. </returns>
        public Stream MuhurAl()
        {
            Uri muhurUri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_MUHUR, UriKind.Relative));
            if (_package.PartExists(muhurUri))
            {
                return _package.GetPart(new Uri(Araclar.URI_MUHUR, UriKind.Relative)).GetStream();
            }
            else
            {
                return null;
            }
        }
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
        /// Verilen PaketOzeti nesnesindeki özet değerleri ile paket içerisindeki bileşenlerin özet değerlerini doğrular.
        /// </summary>
        /// <param name="ozet">Doğrulanacak PaketÖzeti nesnesi.</param>
        /// <param name="sonuclar">Doğrulanamayan bileşenler için açıklamaları barındırır.</param>
        /// <returns>Bileşen doğrulandığında true, aksi halde false döner.
        /// Hata Kodu Açıklamaları:
        /// 1   Ozet değeri verilmemiş.
        /// 2   Ustveri bileşeni yok.
        /// 3   PaketOzet'inde Ustveri ozet değeri yok.
        /// 4   BelgeHedef bileşeni yok.
        /// 5   PaketOzet'inde BelgeHedef ozet değeri yok.
        /// 6   UstYazi bileşeni yok.
        /// 7   PaketOzet'inde UstYazi ozet değeri yok.
        /// 8   Reference değeri verilmemiş.
        /// 9   URI değeri boş
        /// 10  DigestMethod değeri boş.
        /// 11  Algorithm değeri boş.
        /// 12  Desteklenmeyen OzetModu.
        /// 13  Paket bileşeni alınamadı.
        /// 14  Paket bileşenine ait hash hesaplanamadı.
        /// 15  Hashler eşit değil.</returns>
        public bool PaketOzetiDogrula(CT_PaketOzeti ozet, ref List<OzetDogrulamaHatasi> sonuclar)
        {
            if (sonuclar == null)
            {
                return false;
            }
            if (ozet == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "Ozet değeri verilmemiş.", HataKodu = OzetDogrulamaHataKodu.OZET_DEGERI_VERILMEMIS });
                return false;
            }

            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "Ustveri bileşeni yok.", HataKodu = OzetDogrulamaHataKodu.USTVERI_BILESENI_YOK });
                return false;
            }
            else
            {
                Uri readedUstveriUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).First().TargetUri;
                if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedUstveriUri).ToString())).Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "PaketOzet'inde Ustveri ozet değeri yok.", HataKodu = OzetDogrulamaHataKodu.PAKETOZET_INDE_USTVERI_OZET_DEGERI_YOK });
                    return false;
                }
            }
            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEHEDEF) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEHEDEF).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "BelgeHedef bileşeni yok.", HataKodu = OzetDogrulamaHataKodu.BELGEHEDEF_BILESENI_YOK });
                return false;
            }
            else
            {
                Uri readedBelgeHedefUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEHEDEF).First().TargetUri;
                if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedBelgeHedefUri).ToString())).Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "PaketOzet'inde BelgeHedef ozet değeri yok.", HataKodu = OzetDogrulamaHataKodu.PAKETOZET_INDE_BELGEHEDEF_OZET_DEGERI_YOK });
                    return false;
                }
            }
            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "UstYazi bileşeni yok.", HataKodu = OzetDogrulamaHataKodu.USTYAZI_BILESENI_YOK });
                return false;
            }
            else
            {
                Uri readedUstYaziUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).First().TargetUri;
                if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedUstYaziUri).ToString())).Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "PaketOzet'inde UstYazi ozet değeri yok.", HataKodu = OzetDogrulamaHataKodu.PAKETOZET_INDE_USTYAZI_OZET_DEGERI_YOK });
                    return false;
                }
            }
            foreach (CT_Reference item in ozet.Reference)
            {
                PaketOzetiDogrula(item, ref sonuclar);
            }
            if (sonuclar.Count == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Tek bir özet değeri ile paket içerisindeki ilgili bileşenin özet değerlerini doğrular.
        /// </summary>
        /// <param name="item">Doğrulanacak ÖzetDeğerini barındıran CT_Reference  nesnesi.</param>
        /// <param name="sonuclar">Doğrulanamayan bileşenler için açıklamaları barındırır.</param>
        /// <returns>Bileşen doğrulandığında true, aksi halde false döner.
        /// Hata Kodu Açıklamaları:
        /// 1   Ozet değeri verilmemiş.
        /// 2   Ustveri bileşeni yok.
        /// 3   PaketOzet'inde Ustveri ozet değeri yok.
        /// 4   BelgeHedef bileşeni yok.
        /// 5   PaketOzet'inde BelgeHedef ozet değeri yok.
        /// 6   UstYazi bileşeni yok.
        /// 7   PaketOzet'inde UstYazi ozet değeri yok.
        /// 8   Reference değeri verilmemiş.
        /// 9   URI değeri boş
        /// 10  DigestMethod değeri boş.
        /// 11  Algorithm değeri boş.
        /// 12  Desteklenmeyen OzetModu.
        /// 13  Paket bileşeni alınamadı.
        /// 14  Paket bileşenine ait hash hesaplanamadı.
        /// 15  Hashler eşit değil.</returns>
        public bool PaketOzetiDogrula(CT_Reference item, ref List<OzetDogrulamaHatasi> sonuclar)
        {
            if (sonuclar == null) return false;
            if (item == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "Reference değeri verilmemiş.", HataKodu = OzetDogrulamaHataKodu.REFERENCE_DEGERI_VERILMEMIS });
                return false;
            }
            if (item.URI == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "\"URI\" değeri boş.", HataKodu = OzetDogrulamaHataKodu.URI_DEGERI_BOS });
                return false;
            }
            if (item.DigestMethod == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = string.Format("\"DigestMethod\" değeri boş. URI:{0}.", item.URI), HataKodu = OzetDogrulamaHataKodu.DIGESTMETHOD_DEGERI_BOS, Uri = item.URI });
                return false;
            }
            if (item.DigestMethod.Algorithm == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = string.Format("\"Algorithm\" değeri boş. URI:{0}.", item.URI), HataKodu = OzetDogrulamaHataKodu.ALGORITHM_DEGERI_BOS, Uri = item.URI });
                return false;
            }
            if (item.Type != null && item.Type == Araclar.HARICI_PAKET_BILESENI_REFERANS_TIPI)
            {
                return true;
            }
            OzetModu mod;
            if (string.Compare(item.DigestMethod.Algorithm, Araclar.OzetModuToString(OzetModu.SHA256), true) == 0) mod = OzetModu.SHA256;
            else if (string.Compare(item.DigestMethod.Algorithm, Araclar.OzetModuToString(OzetModu.SHA512), true) == 0) mod = OzetModu.SHA512;
            else if (string.Compare(item.DigestMethod.Algorithm, Araclar.OzetModuToString(OzetModu.SHA1), true) == 0) mod = OzetModu.SHA1;
            else if (string.Compare(item.DigestMethod.Algorithm, Araclar.OzetModuToString(OzetModu.RIPEMD160), true) == 0) mod = OzetModu.RIPEMD160;
            else
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = string.Format("Desteklenmeyen OzetModu. URI:{0}.", item.URI), HataKodu = OzetDogrulamaHataKodu.DESTEKLENMEYEN_OZETMODU, Uri = item.URI });
                return false;
            }
            PackagePart p;
            Stream s;
            try
            {
                p = _package.GetPart(PackUriHelper.CreatePartUri(new Uri(Uri.UnescapeDataString(item.URI), UriKind.Relative)));
                s = p.GetStream();
            }
            catch (Exception e)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = String.Format("Paket bileşeni alınamadı. URI:{0}.", item.URI), HataKodu = OzetDogrulamaHataKodu.PAKET_BILESENI_ALINAMADI, Uri = item.URI, InnerException = e });
                return false;
            }
            byte[] computedDigestValue = null;
            try
            {
                switch (mod)
                {
                    case OzetModu.Yok:
                        break;
                    case OzetModu.SHA1:
                        computedDigestValue = (Araclar.Sha1OzetHesapla(s));
                        break;
                    case OzetModu.SHA256:
                        computedDigestValue = (Araclar.Sha256OzetHesapla(s));
                        break;
                    case OzetModu.RIPEMD160:
                        computedDigestValue = (Araclar.RIPEMD160OzetHesapla(s));
                        break;
                    case OzetModu.SHA512:
                        computedDigestValue = (Araclar.Sha512OzetHesapla(s));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = String.Format("Paket bileşenine ait hash hesaplanamadı. URI:{0}.", item.URI), HataKodu = OzetDogrulamaHataKodu.PAKET_BILESENINE_AIT_HASH_HESAPLANAMADI, Uri = item.URI, InnerException = e });
                return false;
            }
            if (computedDigestValue.SequenceEqual(item.DigestValue))
            {

            }
            else
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = String.Format("Hashler eşit değil. URI:{0}.", item.URI), HataKodu = OzetDogrulamaHataKodu.HASHLER_ESIT_DEGIL, Uri = item.URI });
                return false;
            }
            if (sonuclar.Count == 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Nihai Ozeti
        /// <summary>   Paket içerisindeki NihaiOzet bileşenini STREAM olarak döner. </summary>
        /// <returns>   Paket içerisindeki NihaiOzet bileşeninin STREAM nesnesi. NihaiOzet bileşeni olmaması durumunda null döner. </returns>
        public Stream NihaiOzetAl()
        {
            Uri nihaiOzetUri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative));
            if (_package.PartExists(nihaiOzetUri))
            {
                return _package.GetPart(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative)).GetStream();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Verilen NihaiOzet nesnesindeki özet değerleri ile paket içerisindeki bileşenlerin özet değerlerini doğrular.
        /// </summary>
        /// <param name="ozet">Doğrulanacak NihaiOzet nesnesi.</param>
        /// <param name="sonuclar">Doğrulanamayan bileşenler için açıklamaları barındırır.</param>
        /// <returns>Bileşen doğrulandığında true, aksi halde false döner.
        /// Hata Kodu Açıklamaları:
        /// 1   Ozet değeri verilmemiş.
        /// 2   Ustveri bileşeni yok.
        /// 4   BelgeHedef bileşeni yok.
        /// 6   UstYazi bileşeni yok.
        /// 8   Reference değeri verilmemiş.
        /// 9   URI değeri boş
        /// 10  DigestMethod değeri boş.
        /// 11  Algorithm değeri boş.
        /// 12  Desteklenmeyen OzetModu.
        /// 13  Paket bileşeni alınamadı.
        /// 14  Paket bileşenine ait hash hesaplanamadı.
        /// 15  Hashler eşit değil.
        /// 16  BelgeImza bileşeni yok.
        /// 17  Imza bileşeni yok.
        /// 18  Core bileşeni yok.
        /// 19  NihaiOzet'te Ustveri ozet değeri yok.
        /// 20  NihaiOzet'te BelgeHedef ozet değeri yok.
        /// 21  NihaiOzet'te UstYazi ozet değeri yok.
        /// 22  NihaiOzet'te UstYazi ozet değeri yok.
        /// 23  NihaiOzet'te UstYazi ozet değeri yok.
        /// 24  NihaiOzet'te UstYazi ozet değeri yok.
        /// </returns>
        /// <remarks> Java API için açıklama. .Net API kullanıyorsanız bu açıklamyı dikkate almayınız. 
        /// Kullanılan Apache POI API'sı, 'core' bileşenine ulaşılmasına engel olduğundan dolayı, NihaiOzetDogrula metodu Core bileşeni özetini doğrulamaz. Bu nedenle NihaiÖzetDogrula metodundan sonra NihaiOzetCoreDogrula yardımcı metodu kullanılarak doğrulama işlemi tamamlanır. </remarks>
        public bool NihaiOzetDogrula(CT_NihaiOzet ozet, ref List<OzetDogrulamaHatasi> sonuclar)
        {
            if (sonuclar == null)
            {
                return false;
            }
            if (ozet == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "Ozet değeri verilmemiş.", HataKodu = OzetDogrulamaHataKodu.OZET_DEGERI_VERILMEMIS });
                return false;
            }

            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "Ustveri bileşeni yok.", HataKodu = OzetDogrulamaHataKodu.USTVERI_BILESENI_YOK });
                return false;
            }
            else
            {
                Uri readedUstveriUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).First().TargetUri;
                if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedUstveriUri).ToString())).Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "NihaiOzet'te Ustveri ozet değeri yok.", HataKodu = OzetDogrulamaHataKodu.NIHAIOZET_TE_USTVERI_OZET_DEGERI_YOK });
                    return false;
                }
            }
            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEHEDEF) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEHEDEF).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "BelgeHedef bileşeni yok.", HataKodu = OzetDogrulamaHataKodu.BELGEHEDEF_BILESENI_YOK });
                return false;
            }
            else
            {
                Uri readedBelgeHedefUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEHEDEF).First().TargetUri;
                if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedBelgeHedefUri).ToString())).Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "NihaiOzet'te BelgeHedef ozet değeri yok.", HataKodu = OzetDogrulamaHataKodu.NIHAIOZET_TE_BELGEHEDEF_OZET_DEGERI_YOK });
                    return false;
                }
            }
            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "UstYazi bileşeni yok.", HataKodu = OzetDogrulamaHataKodu.USTYAZI_BILESENI_YOK });
                return false;
            }
            else
            {
                Uri readedUstYaziUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).First().TargetUri;
                if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedUstYaziUri).ToString())).Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "NihaiOzet'te UstYazi ozet değeri yok.", HataKodu = OzetDogrulamaHataKodu.NIHAIOZET_TE_USTYAZI_OZET_DEGERI_YOK });
                    return false;
                }
            }
            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEIMZA) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEIMZA).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "BelgeImza bileşeni yok.", HataKodu = OzetDogrulamaHataKodu.BELGEIMZA_BILESENI_YOK });
                return false;
            }
            else
            {
                Uri readedBelgeImzaUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_BELGEIMZA).First().TargetUri;
                if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedBelgeImzaUri).ToString())).Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "NihaiOzet'te BelgeImza ozet değeri yok.", HataKodu = OzetDogrulamaHataKodu.NIHAIOZET_TE_BELGEIMZA_OZET_DEGERI_YOK });
                    return false;
                }
            }
            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_PAKETOZETI) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_PAKETOZETI).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "PaketOzeti bileşeni yok.", HataKodu = OzetDogrulamaHataKodu.PAKETOZETI_BILESENI_YOK });
                return false;
            }
            else
            {
                var relImza = _package.GetPart(new Uri(Araclar.URI_PAKETOZETI, UriKind.Relative)).GetRelationshipsByType(Araclar.RELATION_TYPE_IMZA);
                if (relImza == null || relImza.Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "Imza bileşeni yok.", HataKodu = OzetDogrulamaHataKodu.IMZA_BILESENI_YOK });
                    return false;
                }
                Uri readedImzaUri = relImza.First().TargetUri;
                if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedImzaUri).ToString())).Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "NihaiOzet'te Imza ozet değeri yok.", HataKodu = OzetDogrulamaHataKodu.NIHAIOZET_TE_IMZA_OZET_DEGERI_YOK });
                    return false;
                }
            }
            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_CORE) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_CORE).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "Core bileşeni yok.", HataKodu = OzetDogrulamaHataKodu.CORE_BILESENI_YOK });
                return false;
            }
            else
            {
                Uri readedCoreUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_CORE).First().TargetUri;
                if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedCoreUri).ToString())).Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "NihaiOzet'te Core ozet değeri yok.", HataKodu = OzetDogrulamaHataKodu.NIHAIOZET_TE_CORE_OZET_DEGERI_YOK });
                    return false;
                }
            }
            foreach (CT_Reference item in ozet.Reference)
            {
                PaketOzetiDogrula(item, ref sonuclar);
            }
            if (sonuclar.Count == 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Hedefler
        /// <summary>   Paket içerisindeki BelgeHedef bileşenini STREAM olarak döner. </summary>
        /// <returns>   Paket içerisindeki BelgeHedef bileşeninin STREAM nesnesi. BelgeHedef bileşeni olmaması durumunda null döner. </returns>
        public Stream BelgeHedefiAl()
        {
            Uri belgeHedefUri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_BELGEHEDEF, UriKind.Relative));
            if (_package.PartExists(belgeHedefUri))
            {
                return _package.GetPart(new Uri(Araclar.URI_BELGEHEDEF, UriKind.Relative)).GetStream();
            }
            else
            {
                return null;
            }
        }
        #endregion

        /// <summary> Kullanılan kaynakları kapatır.</summary>
        /// <remarks>Paket objesinin using bloğu ile kullanılması önerilir.</remarks>
        /// <example><code>
        ///  using (var paket = Paket.Ac("63771493-DDC0-476D-98A4-E025C5D3B42B.eyp"), PaketModu.Guncelle))
        ///  {
        ///     paket.EkCikar(new Guid(paket.Ustveri.EkleriAl().First().Id.Value));
        ///     paket.Kapat();
        ///  }
        ///  </code>
        /// </example>
        public void Dispose()
        {
            if (_streamInternal != null)
                _streamInternal.Close();
            if (_streamInternal != null)
                _streamInternal.Dispose();
        }


    }

}
