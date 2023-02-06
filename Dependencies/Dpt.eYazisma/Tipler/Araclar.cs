using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dpt.eYazisma.Xsd;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.IO.Packaging;
using System.Text.RegularExpressions;

namespace Dpt.eYazisma.Tipler
{
    /// <summary>
    /// Sabit tanımlar ve yardımcı metodları içerir.
    /// </summary>
    public static class Araclar
    {
        /// <summary>Ustveri bileşeni ilişki türü.</summary>
        public const String RELATION_TYPE_USTVERI = "http://eyazisma.dpt/iliskiler/ustveri";
        /// <summary>BelgeHedef bileşeni ilişki türü.</summary>
        public const String RELATION_TYPE_BELGEHEDEF = "http://eyazisma.dpt/iliskiler/belgehedef";
        /// <summary>BelgeImza bileşeni ilişki türü.</summary>
        public const String RELATION_TYPE_BELGEIMZA = "http://eyazisma.dpt/iliskiler/belgeimza";
        /// <summary>PaketOzeti bileşeni ilişki türü.</summary>
        public const String RELATION_TYPE_PAKETOZETI = "http://eyazisma.dpt/iliskiler/paketozeti";
        /// <summary>NihaiOzet bileşeni ilişki türü.</summary>
        public const String RELATION_TYPE_NIHAIOZET = "http://eyazisma.dpt/iliskiler/nihaiozet";
        /// <summary>UstYazi bileşeni ilişki türü.</summary>
        public const String RELATION_TYPE_USTYAZI = "http://eyazisma.dpt/iliskiler/ustyazi";
        /// <summary>Imzali PaketOzeti bileşeni ilişki türü.</summary>
        public const String RELATION_TYPE_IMZA = "http://eyazisma.dpt/iliskiler/imzacades";
        /// <summary>Mühürlü NihaiOzet bileşeni ilişki türü.</summary>
        public const String RELATION_TYPE_MUHUR = "http://eyazisma.dpt/iliskiler/muhurcades";
        /// <summary>Ek bileşeni ilişki türü.</summary>
        public const String RELATION_TYPE_EK = "http://eyazisma.dpt/iliskiler/ek";
        /// <summary>İmzasız ek bileşeni ilişki türü.</summary>
        public const String RELATION_TYPE_IMZASIZEK = "http://eyazisma.dpt/iliskiler/imzasizEk";
        /// <summary>Core bileşeni ilişki türü.</summary>
        public const String RELATION_TYPE_CORE = "http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties";
        /// <summary>ArşivOzellikleri bileşeni ilişki türü. Bu bileşenin tanımı ileride yapılacaktır.</summary>
        public const String RELATION_TYPE_ARSIVOZELLIKLERI = "http://eyazisma.dpt/iliskiler/arsivozellikleri";
        /// <summary>SifreliIcerik bileşeni ilişki türü.</summary>
        public const String RELATION_TYPE_SIFRELIICERIK = "http://eyazisma.dpt/iliskiler/sifreliicerik";
        /// <summary>SifreliIcerikBilgisi bileşeni ilişki türü.</summary>
        public const String RELATION_TYPE_SIFRELIICERIKBILGISI = "http://eyazisma.dpt/iliskiler/sifreliicerikbilgisi";

        /// <summary>W3C XML Signature Syntax and Processing, SHA1 namespace</summary>
        public const String ALGORITHM_SHA1 = "http://www.w3.org/2000/09/xmldsig#sha1";
        /// <summary>W3C XML Signature Syntax and Processing, SHA256 namespace</summary>
        public const String ALGORITHM_SHA256 = "http://www.w3.org/2001/04/xmlenc#sha256";
        /// <summary>W3C XML Signature Syntax and Processing, SHA512 namespace</summary>
        public const String ALGORITHM_SHA512 = "http://www.w3.org/2001/04/xmlenc#sha512";
        /// <summary>W3C XML Signature Syntax and Processing, RIPEMD160 namespace</summary>
        public const String ALGORITHM_RIPEMD160 = "http://www.w3.org/2001/04/xmlenc#ripemd160";

        /// <summary> Paket Özeti ve Nihai Özete eklenen paket dışı bileşenlere ilişkin tip değeri</summary>
        public const String HARICI_PAKET_BILESENI_REFERANS_TIPI = "http://eyazisma.dpt/bilesen#harici";
        /// <summary> Paket Özeti ve Nihai Özete eklenen paket içi bileşenlere ilişkin tip değeri</summary>
        public const String DAHILI_PAKET_BILESENI_REFERANS_TIPI = "http://eyazisma.dpt/bilesen#dahili";

        /// <summary>Şifreleme Yöntemi</summary>
        public const String SIFRELEME_YONTEMI = "Elektronik Belgeleri Açık Anahtar Altyapısı Kullanarak Güvenli İşleme Rehberi";
        /// <summary>Şifreleme Yöntemi Rehberi URI</summary>
        public const String SIFRELEME_YONTEMI_URI_1 = "http://www.kamusm.gov.tr/dokumanlar/belgeler";
        /// <summary>Şifreleme Yöntemi Rehberi URI</summary>
        public const String SIFRELEME_YONTEMI_URI_2 = "http://www.e-yazisma.gov.tr/SitePages/dokumanlar.aspx";
        /// <summary>Şifreleme Yöntemi Versiyonu</summary> 
        public const String SIFRELEME_YONTEMI_VERSIYONU = "1.4";

        /// <summary>e-Yazışma paketi kategorisi</summary>
        public const String RESMIYAZISMA = "RESMIYAZISMA";
        /// <summary>Şifreli e-Yazışma paketi kategorisi</summary>
        public const String RESMIYAZISMASIFRELI = "RESMIYAZISMA/SIFRELI";
        /// <summary>e-Yazışma paketi MIME türü</summary>
        public const String EYAZISMAMIMETURU = "application/eyazisma";

        /// <summary> PaketOzeti bileşenine ait URI.</summary>
        public const String URI_PAKETOZETI = "/PaketOzeti/PaketOzeti.xml";
        /// <summary> NihaiOzet bileşenine ait URI.</summary>
        public const String URI_NIHAIOZET = "/NihaiOzet/NihaiOzet.xml";
        /// <summary> Ustveri bileşenine ait URI.</summary>
        public const String URI_USTVERI = "/Ustveri/Ustveri.xml";
        /// <summary> BelgeHedef bileşenine ait URI.</summary>
        public const String URI_BELGEHEDEF = "/BelgeHedef/BelgeHedef.xml";
        /// <summary> Imzalar bileşenine ait URI.</summary>
        public const String URI_BELGEIMZA = "/Imzalar/BelgeImza.xml";
        /// <summary> UstYazi bileşenine ait URI formatı.</summary>
        public const String URI_FORMAT_USTYAZI = "/UstYazi/{0}";
        /// <summary> Ek bileşenine ait URI.</summary>
        public const String URI_ROOT_EK = "Ekler";
        /// <summary> İmzasızEk bileşenine ait URI.</summary>
        public const String URI_ROOT_IMZASIZEK = "ImzasizEkler";
        /// <summary> Imzali PaketOzeti bileşenine ait URI.</summary>
        public const String URI_IMZA = "/Imzalar/ImzaCades.imz";
        /// <summary> Mühürlü NihaiOzet bileşenine ait URI.</summary>
        public const String URI_MUHUR = "/Muhurler/MuhurCades.imz";
        /// <summary> ArşivOzellikleri bileşenine ait URI. Bu bileşenin tanımı ileride yapılacaktır..</summary>
        public const String URI_ARSIVOZELLIKLERI = "/Arsiv/ArsivOzellikleri.xml";
        /// <summary> SifreliIcerik bileşenine ait URI.</summary>
        public const String URI_FORMAT_SIFRELIICERIK = "/SifreliIcerik/{0}.eyp";
        /// <summary> SifreliIcerikBilgisi bileşenine ait URI.</summary>
        public const String URI_SIFRELIICERIKBILGISI = "/SifreliIcerikBilgisi/SifreliIcerikBilgisi.xml";

        /// <summary> Ek bileşeni ilişkisi Id formatı.</summary>
        public const String ID_ROOT_EK = "IdEk_";
        /// <summary> İmzasız Ek bileşeni ilişkisi Id formatı.</summary>
        public const String ID_ROOT_IMZASIZEK = "IdImzasizEk_";
        /// <summary> İmzalı PaketOzeti bileşeni ilişkisi Id'si.</summary>
        public const String ID_IMZA = "IdImzaCades";
        /// <summary> İmzalı NihaiOzet bileşeni ilişkisi Id'si.</summary>
        public const String ID_MUHUR = "IdMuhurCades";
        /// <summary> BelgeImza bileşeni ilişkisi Id'si.</summary>
        public const String ID_BELGEIMZA = "IdBelgeImza";
        /// <summary> UstYazi bileşeni ilişkisi Id'si.</summary>
        public const String ID_USTYAZI = "IdUstYazi";
        /// <summary> Ustveri bileşeni ilişkisi Id'si.</summary>
        public const String ID_USTVERI = "IdUstveri";
        /// <summary> BelgeHedef bileşeni ilişkisi Id'si.</summary>
        public const String ID_BELGEHEDEF = "IdBelgeHedef";
        /// <summary> PaketOzeti bileşeni ilişkisi Id'si.</summary>
        public const String ID_PAKETOZETI = "IdPaketOzeti";
        /// <summary> NihaiOzet bileşeni ilişkisi Id'si.</summary>
        public const String ID_NIHAIOZETI = "IdNihaiOzet";
        /// <summary> SifreliIcerik bileşeni ilişkisi Id'si.</summary>
        public const String ID_SIFRELIICERIK = "IdSifreliIcerik";
        /// <summary> Ek bileşeni ilişkisi Id'si.</summary>
        public const String ID_SIFRELIICERIKBILGISI = "IdSifreliIcerikBilgisi";

        /// <summary> XML Mime türü.</summary>
        public const String MIME_XML = "application/xml";
        /// <summary> XML Octet-stream türü.</summary>
        public const String MIME_OCTETSTREAM = "application/octet-stream";

        #region Sha1
        /// <summary>
        /// Verilen lokasyondaki dosyanın SHA1 özetini hesaplar.
        /// </summary>
        /// <param name="dosyaYolu">Özeti hesaplanacak dosyanın yolu.</param>
        /// <returns>Özet değeri.</returns>
        public static Byte[] Sha1OzetHesapla(String dosyaYolu)
        {
            using (var fileStream = new FileStream(dosyaYolu, FileMode.Open))
            {
                return Sha1OzetHesapla(fileStream);
            }
        }
        /// <summary>
        /// Verilen STREAM'in SHA1 özetini hesaplar.
        /// </summary>
        /// <param name="dosya">Özeti hesaplanacak STREAM.</param>
        /// <returns>Özet değeri.</returns>
        public static Byte[] Sha1OzetHesapla(Stream dosya)
        {
            using (var sha1Managed = new SHA1Managed())
            {
                return sha1Managed.ComputeHash(dosya);
            }
        }
        /// <summary>
        /// Verilen lokasyondaki dosyanın SHA1 özetini hesaplar.
        /// </summary>
        /// <param name="dosyaYolu">Özeti hesaplanacak dosyanın yolu.</param>
        /// <returns>Özet değerinin BASE64 notasyonu.</returns>
        public static String Sha1OzetHesaplaBase64(String dosyaYolu)
        {
            using (var fileStream = new FileStream(dosyaYolu, FileMode.Open))
            {
                using (var sha1Managed = new SHA1Managed())
                {
                    return Convert.ToBase64String(sha1Managed.ComputeHash(fileStream));
                }
            }
        }
        #endregion

        #region Sha256
        /// <summary>
        /// Verilen lokasyondaki dosyanın SHA256 özetini hesaplar.
        /// </summary>
        /// <param name="dosyaYolu">Özeti hesaplanacak dosyanın yolu.</param>
        /// <returns>Özet değeri.</returns>
        public static Byte[] Sha256OzetHesapla(String dosyaYolu)
        {
            using (var fileStream = new FileStream(dosyaYolu, FileMode.Open))
            {
                return Sha256OzetHesapla(fileStream);
            }
        }
        /// <summary>
        /// Verilen STREAM'in SHA256 özetini hesaplar.
        /// </summary>
        /// <param name="dosya">Özeti hesaplanacak STREAM.</param>
        /// <returns>Özet değeri.</returns>
        public static Byte[] Sha256OzetHesapla(Stream dosya)
        {
            using (var sha256Managed = new SHA256Managed())
            {
                return sha256Managed.ComputeHash(dosya);
            }
        }
        #endregion

        #region Sha512
        /// <summary>
        /// Verilen lokasyondaki dosyanın SHA512 özetini hesaplar.
        /// </summary>
        /// <param name="dosyaYolu">Özeti hesaplanacak dosyanın yolu.</param>
        /// <returns>Özet değeri.</returns>
        public static Byte[] Sha512OzetHesapla(String dosyaYolu)
        {
            using (var fileStream = new FileStream(dosyaYolu, FileMode.Open))
            {
                return Sha512OzetHesapla(fileStream);
            }
        }
        /// <summary>
        /// Verilen STREAM'in SHA512 özetini hesaplar.
        /// </summary>
        /// <param name="dosya">Özeti hesaplanacak STREAM.</param>
        /// <returns>Özet değeri.</returns>
        public static Byte[] Sha512OzetHesapla(Stream dosya)
        {
            using (var sha512Managed = new SHA512Managed())
            {
                return sha512Managed.ComputeHash(dosya);
            }
        }
        #endregion

        #region RIPEMD160
        /// <summary>
        /// Verilen lokasyondaki dosyanın RIPEMD160 özetini hesaplar.
        /// </summary>
        /// <param name="dosyaYolu">Özeti hesaplanacak dosyanın yolu.</param>
        /// <returns>Özet değeri.</returns>
        public static Byte[] RIPEMD160OzetHesapla(String dosyaYolu)
        {
            using (var fileStream = new FileStream(dosyaYolu, FileMode.Open))
            {
                return RIPEMD160OzetHesapla(fileStream);
            }
        }
        /// <summary>
        /// Verilen STREAM'in RIPEMD160 özetini hesaplar
        /// </summary>
        /// <param name="dosya">Özeti hesaplanacak STREAM</param>
        /// <returns>Özet değeri.</returns>
        public static Byte[] RIPEMD160OzetHesapla(Stream dosya)
        {
            using (var RIPEMD160Managed = new RIPEMD160Managed())
            {
                return RIPEMD160Managed.ComputeHash(dosya);
            }
        }
        #endregion

        #region Ozet
        /// <summary>
        /// Verilen STREAM'in belirtilen algoritma ile özetini hesaplar.
        /// </summary>
        /// <param name="dosya">Özeti hesaplanacak STREAM.</param>
        /// <param name="mod">Özetleme algoritması.</param>
        /// <returns></returns>
        public static byte[] OzetHesapla(Stream dosya, OzetModu mod)
        {
            switch (mod)
            {
                case OzetModu.Yok:
                    return null;
                case OzetModu.SHA1:
                    return Sha1OzetHesapla(dosya);
                case OzetModu.SHA256:
                    return Sha256OzetHesapla(dosya);
                case OzetModu.RIPEMD160:
                    return RIPEMD160OzetHesapla(dosya);
                case OzetModu.SHA512:
                    return Sha512OzetHesapla(dosya);
                default:
                    return null;
            }
        }
        #endregion

        /// <summary>
        /// Verilen CT_Dagitim nesnesini <see cref="CT_Hedef"/> nesnesine dönüştürür.
        /// </summary>
        /// <param name="dagitim">Dönüştürülecek <see cref="CT_Dagitim"/> nesnesi.</param>
        /// <returns><see cref="CT_Hedef"/> nesnesi.</returns>
        public static CT_Hedef Dagitim2Hedef(CT_Dagitim dagitim)
        {
            CT_Hedef hedef = new CT_Hedef();
            hedef.Item = dagitim.Item;
            return hedef;
        }
        /// <summary>
        /// .Net 4.0'da bulunan <see cref="String.IsNullOrWhiteSpace"/> fonksiyonu.
        /// </summary>
        /// <param name="s">Kontrol edilecek <see cref="String"/></param>
        /// <returns>true if the value parameter is null or <see cref="String.Empty"/>, or if value consists exclusively of white-space characters.</returns>
        public static bool IsNullOrWhiteSpace(this String s)
        {
            if (s == null || "".Equals(s.Trim()))
                return (true);
            return (false);
        }
        /// <summary>
        /// Verilen <see cref="CT_GercekSahis"/> nesnesindeki bilgileri kullanarak gerçek şahıs adı oluşturur.
        /// </summary>
        /// <param name="olusturan">İsmi oluşturulacak <see cref="CT_GercekSahis"/> nesnesi.</param>
        /// <returns>Gerçek şahsın adı.</returns>
        private static String GercekSahisAdiOlustur(CT_GercekSahis olusturan)
        {
            String sonuc = string.Empty;
            String tcKimlikNo = olusturan.TCKN;
            String gorev = string.Empty;
            String ad = String.Empty;
            if (olusturan.Gorev != null && !(olusturan.Gorev.Value.IsNullOrWhiteSpace()))
                gorev = olusturan.Gorev.Value;
            if (olusturan.Kisi != null)
            {
                if (olusturan.Kisi.OnEk != null && !(olusturan.Kisi.OnEk.Value.IsNullOrWhiteSpace()))
                    ad += olusturan.Kisi.OnEk.Value;
                if (olusturan.Kisi.Unvan != null && !(olusturan.Kisi.Unvan.Value.IsNullOrWhiteSpace()))
                {
                    if (ad != String.Empty)
                        ad += " ";
                    ad += olusturan.Kisi.Unvan.Value;
                }
                if (olusturan.Kisi.IlkAdi != null && !(olusturan.Kisi.IlkAdi.Value.IsNullOrWhiteSpace()))
                {
                    if (ad != String.Empty)
                        ad += " ";
                    ad += olusturan.Kisi.IlkAdi.Value;
                }
                if (olusturan.Kisi.IkinciAdi != null && !(olusturan.Kisi.IkinciAdi.Value.IsNullOrWhiteSpace()))
                {
                    if (ad != String.Empty)
                        ad += " ";
                    ad += olusturan.Kisi.IkinciAdi.Value;
                }
                if (olusturan.Kisi.Soyadi != null && !(olusturan.Kisi.Soyadi.Value.IsNullOrWhiteSpace()))
                {
                    if (ad != String.Empty)
                        ad += " ";
                    ad += olusturan.Kisi.Soyadi.Value;
                }
            }
            if (!(tcKimlikNo.IsNullOrWhiteSpace()))
                sonuc += tcKimlikNo + ",";
            if (!(gorev.IsNullOrWhiteSpace()))
                sonuc += gorev + ",";
            if (!(ad.IsNullOrWhiteSpace()))
                sonuc += ad + ",";
            if (!(sonuc.IsNullOrWhiteSpace()))
                sonuc = sonuc.Substring(0, sonuc.Length - 1);
            return sonuc;
        }
        /// <summary>
        /// Verilen <see cref="CT_TuzelSahis"/> nesnesindeki bilgileri kullanarak tüzel şahıs adı oluşturur.
        /// </summary>
        /// <param name="olusturan">İsmi oluşturulacak <see cref="CT_TuzelSahis"/> nesnesi.</param>
        /// <returns>Tüzel şahsın adı.</returns>
        private static String TuzelSahisAdiOlustur(CT_TuzelSahis olusturan)
        {
            if (olusturan.Adi != null && !(olusturan.Adi.Value.IsNullOrWhiteSpace()))
                return (olusturan.Adi.Value);
            else if (olusturan.Id != null && !(olusturan.Id.Value.IsNullOrWhiteSpace()))
            {
                String id = String.Empty;
                if (olusturan.Id != null && !(olusturan.Id.schemeID.IsNullOrWhiteSpace()))
                    id += olusturan.Id.schemeID;
                if (olusturan.Id != null && !(olusturan.Id.Value.IsNullOrWhiteSpace()))
                {
                    if (!(id.IsNullOrWhiteSpace()))
                        id += ":";
                    id += olusturan.Id.Value;
                }
                return id;
            }
            return String.Empty;
        }
        /// <summary>
        /// Verilen <see cref="CT_KurumKurulus"/> nesnesindeki bilgileri kullanarak kurum/kuruluş adı oluşturur.
        /// </summary>
        /// <param name="olusturan">İsmi oluşturulacak <see cref="CT_KurumKurulus"/> nesnesi.</param>
        /// <returns>Kurum kuruluşun adı.</returns>
        private static String KurumKurulusAdiOlustur(CT_KurumKurulus olusturan)
        {
            if (olusturan.Adi != null && !(olusturan.Adi.Value.IsNullOrWhiteSpace()))
                return (olusturan.Adi.Value);
            else
                return (olusturan.KKK);
        }
        /// <summary>
        /// Verilen <see cref="CT_Olusturan"/> nesnesindeki bilgileri kullanarak oluşturan adı oluşturur.
        /// </summary>
        /// <param name="olusturan">İsmi oluşturulacak <see cref="CT_Olusturan"/> nesnesi.</param>
        /// <returns>Oluşturan adı.</returns>
        public static String OlusturanAdiOlustur(CT_Olusturan olusturan)
        {
            if (olusturan.Item.GetType() == typeof(CT_KurumKurulus))
                return Araclar.KurumKurulusAdiOlustur((CT_KurumKurulus)olusturan.Item);
            if (olusturan.Item.GetType() == typeof(CT_TuzelSahis))
                return Araclar.TuzelSahisAdiOlustur((CT_TuzelSahis)olusturan.Item);
            if (olusturan.Item.GetType() == typeof(CT_GercekSahis))
                return Araclar.GercekSahisAdiOlustur((CT_GercekSahis)olusturan.Item);
            return String.Empty;
        }
        /// <summary>
        /// Verilen sıralı byte nesnesini deserialize ederek <see cref="CT_PaketOzeti"/> nesnesi oluşturur.
        /// </summary>
        /// <param name="paketOzetiVerisi">Deserialize edilecek sıralı byte.</param>
        /// <returns>Oluşturulan <see cref="CT_PaketOzeti"/> nesnesi.</returns>
        /// <exception cref="System.ApplicationException">Sıralı byte'ın STREAM'e dönüştürülmesinde hata olduğunda oluşur.</exception>
        /// <exception cref="System.ApplicationException">Deserialize hatası.</exception>
        public static CT_PaketOzeti PaketOzetiAl(byte[] paketOzetiVerisi)
        {
            MemoryStream mStream;
            try
            {
                mStream = new MemoryStream(paketOzetiVerisi);
            }
            catch (Exception e)
            {
                throw new ApplicationException("MemoryStream oluşturulamadı.", e);
            }
            try
            {
                CT_PaketOzeti readedPaketOzeti = (CT_PaketOzeti)(new XmlSerializer(typeof(CT_PaketOzeti))).Deserialize(mStream);
                return readedPaketOzeti;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Deserialize işlemi sırasında hata oluştu.", e);
            }
            finally
            {
                mStream.Dispose();
                mStream = null;
            }
        }
        /// <summary>
        /// Verilen sıralı STREAM'i deserialize ederek <see cref="CT_PaketOzeti"/> nesnesi oluşturur.
        /// </summary>
        /// <param name="paketOzetiStream">Deserialize edilecek STREAM.</param>
        /// <returns>Oluşturulan <see cref="CT_PaketOzeti"/> nesnesi.</returns>
        /// <exception cref="System.ApplicationException">Deserialize hatası.</exception>
        public static CT_PaketOzeti PaketOzetiAl(Stream paketOzetiStream)
        {
            try
            {
                CT_PaketOzeti readedPaketOzeti = (CT_PaketOzeti)(new XmlSerializer(typeof(CT_PaketOzeti))).Deserialize(paketOzetiStream);
                return readedPaketOzeti;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Deserialize işlemi sırasında hata oluştu.", e);
            }
        }
        /// <summary>
        /// Verilen sıralı STREAM'i deserialize ederek <see cref="CT_NihaiOzet"/> nesnesi oluşturur.
        /// </summary>
        /// <param name="nihaiOzetStream">Deserialize edilecek STREAM.</param>
        /// <returns>Oluşturulan <see cref="CT_NihaiOzet"/> nesnesi.</returns>
        /// <exception cref="System.ApplicationException">Deserialize hatası.</exception>
        public static CT_NihaiOzet NihaiOzetAl(Stream nihaiOzetStream)
        {
            try
            {
                CT_NihaiOzet readedNihaiOzet = (CT_NihaiOzet)(new XmlSerializer(typeof(CT_NihaiOzet))).Deserialize(nihaiOzetStream);
                return readedNihaiOzet;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Deserialize işlemi sırasında hata oluştu.", e);
            }
        }
        /// <summary>
        /// Bir STREAM'deki veriyi diğerine kopyalar.
        /// </summary>
        /// <param name="source">Kaynak STREAM.</param>
        /// <param name="target">Hedef STREAM.</param>
        /// <exception cref="System.ArgumentNullException">Kaynak veya hedef STREAM'in null olması durumunda oluşur.</exception>
        public static void CopyStream(Stream source, Stream target)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (target == null) throw new ArgumentNullException("source");
            const int bufSize = 0x1000;
            var buf = new byte[bufSize];
            int bytesRead;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
                target.Write(buf, 0, bytesRead);
        }
        /// <summary>
        /// Verilen STREAM'de bulunan paketin şifreli bir paket olup olmadığı bilgisini döner.
        /// </summary>
        /// <param name="stream">Paketin bulunduğu STREAM.</param>
        /// <returns>Paket şifreli ise true, aksi halde false döner.</returns>
        /// <exception cref="System.Exception">Paketin geçerli olmaması durumunda oluşur.</exception>
        public static bool PaketSifreliMi(Stream stream)
        {
            using (Package _package = Package.Open(stream, FileMode.Open, FileAccess.ReadWrite))
            {
                if (_package.PackageProperties.Category == null)
                    throw new Exception("Geçersiz e-yazışma paketi.");
                if (_package.PackageProperties.Category == Araclar.RESMIYAZISMASIFRELI)
                    return true;
                else if (_package.PackageProperties.Category == Araclar.RESMIYAZISMA)
                    return false;
                else
                    throw new Exception("Geçersiz e-yazışma paketi.");
            }
        }
        internal static String EncodePath(String s)
        {
            s = s.Replace('ş', 's');
            s = s.Replace('Ş', 'S');
            s = s.Replace('ö', 'o');
            s = s.Replace('Ö', 'O');
            s = s.Replace('ç', 'c');
            s = s.Replace('Ç', 'C');
            s = s.Replace('i', 'i');
            s = s.Replace('İ', 'I');
            s = s.Replace('ı', 'i');
            s = s.Replace('I', 'I');
            s = s.Replace('ğ', 'g');
            s = s.Replace('Ğ', 'G');
            s = s.Replace('ü', 'u');
            s = s.Replace('Ü', 'U');
            s = (new Regex("[^\\x20-\\x7e]")).Replace(s, "");
            s = (new Regex("\\s+")).Replace(s, "");
            return s;
        }
        internal static String OzetModuToString(OzetModu ozetModu)
        {
            switch (ozetModu)
            {
                case OzetModu.Yok:
                    return "";
                case OzetModu.SHA1:
                    return Araclar.ALGORITHM_SHA1;
                case OzetModu.SHA256:
                    return Araclar.ALGORITHM_SHA256;
                case OzetModu.SHA512:
                    return Araclar.ALGORITHM_SHA512;
                case OzetModu.RIPEMD160:
                    return Araclar.ALGORITHM_RIPEMD160;
                default:
                    return "";
            }
        }
    }
}
