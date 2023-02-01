using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.Config;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Smime;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;
using Tr.Com.Eimza.SmartCard;

namespace Tr.Com.Eimza.EYazisma
{
	internal class EYazismaImzalaGonder
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private SmartCardReader sReader;

		private static readonly object Sync = new object();

		internal EyImzalaGonderSonuc ImzalaGonder(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string smartCardPasswd, List<string> to, List<string> cc, List<string> bcc, string subject, string content, EYazismaIcerikTur icerikTur, List<string> attachments, EYazismaPaketTur mailType, string mailTypeId, int SelectedSmartCardIndex, int SelectedCertificateIndex, EYazismaApiConfig config)
		{
			EyImzalaGonderSonuc eyImzalaGonderSonuc = new EyImzalaGonderSonuc();
			try
			{
				string mailType2 = "";
				if (to == null || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(content))
				{
					EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "Kime, Konu ve Içerik Alanları Boş Bırakılamaz, Fonksiyona Verdiğiniz Parametreleri Kontrol Ediniz.");
					eyImzalaGonderSonuc.Durum = 161;
					eyImzalaGonderSonuc.HataAciklama = "Kime, Konu ve Içerik Alanları Boş Bırakılamaz, Fonksiyona Verdiğiniz Parametreleri Kontrol Ediniz.";
					return eyImzalaGonderSonuc;
				}
				if (smartCardPasswd != null)
				{
					long num = 0L;
					if (attachments != null && attachments.Count > 0)
					{
						foreach (string attachment in attachments)
						{
							num += File.ReadAllBytes(attachment).LongLength;
						}
						float num2 = num;
						float num3 = 0f;
						switch (config.MaxMailDosyaBoyutu.Cins)
						{
						case Boyut.Suffix.KB:
							num3 = config.MaxMailDosyaBoyutu.Size * 1024f;
							break;
						case Boyut.Suffix.MB:
							num3 = config.MaxMailDosyaBoyutu.Size * 1024f * 1024f;
							break;
						}
						num3 = num3 * 4f / 3f;
						num3 += 285004f / (float)Math.E;
						if (num2 > num3)
						{
							EyLog.Log("Imzala Gonder", EyLogTuru.HATA, "Eklerin Toplam Boyutu " + config.MaxMailDosyaBoyutu.Size + " " + config.MaxMailDosyaBoyutu.Cins.ToString() + " Boyutunu Geçemez.");
							eyImzalaGonderSonuc.HataAciklama = "Eklerin Toplam Boyutu " + config.MaxMailDosyaBoyutu.Size + " " + config.MaxMailDosyaBoyutu.Cins.ToString() + " Boyutunu Geçemez.";
							eyImzalaGonderSonuc.Durum = 161;
							return eyImzalaGonderSonuc;
						}
					}
					lock (Sync)
					{
						sReader = new SmartCardReader(smartCardPasswd, SelectedSmartCardIndex);
						try
						{
							sReader.Initialize();
						}
						catch (Exception ex)
						{
							eyImzalaGonderSonuc = new EyImzalaGonderSonuc();
							eyImzalaGonderSonuc.Durum = 161;
							eyImzalaGonderSonuc.HataAciklama = "Akıllı Kart ile Bağlantı Kurulamadı. Akıllı Kartın Takılı Olduğundan ve Driver'larının Kurulu Olduğundan Emin Olunuz.\nC# Exception : " + ex.Message;
							LOG.Error(ex);
							EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "Akıllı Kart ile Bağlantı Kurulamadı. Akıllı Kartın Takılı Olduğundan ve Driver'larının Kurulu Olduğundan Emin Olunuz.C# Exception : " + ex.Message);
							return eyImzalaGonderSonuc;
						}
						try
						{
							sReader.FindToken(SelectedCertificateIndex);
						}
						catch (Exception ex2)
						{
							eyImzalaGonderSonuc = new EyImzalaGonderSonuc();
							eyImzalaGonderSonuc.Durum = 161;
							eyImzalaGonderSonuc.HataAciklama = "Private Key Karttan Çekilemedi. Lütfen Akıllı Kartınızın PIN'ini Kontrol Ediniz.\nC# Exception : " + ex2.Message;
							LOG.Error(ex2);
							EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "Private Key Karttan Çekilemedi. Lütfen Akıllı Kartınızın PIN'ini Kontrol Ediniz.C# Exception : " + ex2.Message);
							return eyImzalaGonderSonuc;
						}
						switch (mailType)
						{
						case EYazismaPaketTur.ETEbligat:
							mailType2 = "eTebligat";
							break;
						case EYazismaPaketTur.EYazisma:
							mailType2 = "eYazisma";
							break;
						case EYazismaPaketTur.Standart:
							mailType2 = "standart";
							break;
						}
						SmimeCreater smimeCreater = new SmimeCreater();
						byte[] array;
						try
						{
							array = smimeCreater.CreateWithSmartCard(kepHesap.kepHesap, to, cc, bcc, subject, content, attachments, mailType2, mailTypeId, sReader);
						}
						catch (Exception ex3)
						{
							LOG.Error(ex3);
							EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "S/Mime Mail Oluşturulamadı.C# Exception : " + ex3.Message);
							eyImzalaGonderSonuc.Durum = 161;
							eyImzalaGonderSonuc.HataAciklama = "S/Mime Mail Oluşturulamadı.\nC# Exception : " + ex3.Message;
							return eyImzalaGonderSonuc;
						}
						if (array == null)
						{
							EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "S/Mime Mail Oluşturulamadı.");
							eyImzalaGonderSonuc.Durum = 161;
							eyImzalaGonderSonuc.HataAciklama = "S/Mime Mail Oluşturulamadı.";
							return eyImzalaGonderSonuc;
						}
						eyImzalaGonderSonuc.YukleSonuc = Yukle(client, kepHesap, array, mailType);
						eyImzalaGonderSonuc.Durum = 0;
						eyImzalaGonderSonuc.HataAciklama = "S/Mime Mail Oluşturma Başarılı.";
						eyImzalaGonderSonuc.SmimeMail = array;
						return eyImzalaGonderSonuc;
					}
				}
				EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "Akıllı Kartın Şifresini Boş Bırakamazsınız");
				eyImzalaGonderSonuc = new EyImzalaGonderSonuc();
				eyImzalaGonderSonuc.Durum = 161;
				eyImzalaGonderSonuc.HataAciklama = "Akıllı Kartın Şifresini Boş Bırakamazsınız";
				return eyImzalaGonderSonuc;
			}
			catch (Exception exception)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception);
				return null;
			}
		}

		internal EyImzalaGonderSonuc ImzalaGonder(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string smartCardPasswd, List<string> to, List<string> cc, List<string> bcc, string subject, string content, EYazismaIcerikTur icerikTur, List<string> attachments, EYazismaPaketTur mailType, string mailTypeId, OzetAlg ozetAlg, int SelectedSmartCardIndex, int SelectedCertificateIndex, EYazismaApiConfig config)
		{
			EyImzalaGonderSonuc eyImzalaGonderSonuc = new EyImzalaGonderSonuc();
			try
			{
				string mailType2 = "";
				if (to == null || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(content))
				{
					EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "Kime, Konu ve Içerik Alanları Boş Bırakılamaz, Fonksiyona Verdiğiniz Parametreleri Kontrol Ediniz.");
					eyImzalaGonderSonuc.Durum = 161;
					eyImzalaGonderSonuc.HataAciklama = "Kime, Konu ve Içerik Alanları Boş Bırakılamaz, Fonksiyona Verdiğiniz Parametreleri Kontrol Ediniz.";
					return eyImzalaGonderSonuc;
				}
				if (smartCardPasswd != null)
				{
					long num = 0L;
					if (attachments != null && attachments.Count > 0)
					{
						foreach (string attachment in attachments)
						{
							num += File.ReadAllBytes(attachment).LongLength;
						}
						float num2 = num;
						float num3 = 0f;
						switch (config.MaxMailDosyaBoyutu.Cins)
						{
						case Boyut.Suffix.KB:
							num3 = config.MaxMailDosyaBoyutu.Size * 1024f;
							break;
						case Boyut.Suffix.MB:
							num3 = config.MaxMailDosyaBoyutu.Size * 1024f * 1024f;
							break;
						}
						num3 = num3 * 4f / 3f;
						num3 += 285004f / (float)Math.E;
						if (num2 > num3)
						{
							EyLog.Log("Imzala Gonder", EyLogTuru.HATA, "Eklerin Toplam Boyutu " + config.MaxMailDosyaBoyutu.Size + " " + config.MaxMailDosyaBoyutu.Cins.ToString() + " Boyutunu Geçemez.");
							eyImzalaGonderSonuc.HataAciklama = "Eklerin Toplam Boyutu " + config.MaxMailDosyaBoyutu.Size + " " + config.MaxMailDosyaBoyutu.Cins.ToString() + " Boyutunu Geçemez.";
							eyImzalaGonderSonuc.Durum = 161;
							return eyImzalaGonderSonuc;
						}
					}
					lock (Sync)
					{
						sReader = new SmartCardReader(smartCardPasswd, SelectedSmartCardIndex);
						try
						{
							sReader.Initialize();
						}
						catch (Exception ex)
						{
							eyImzalaGonderSonuc = new EyImzalaGonderSonuc();
							eyImzalaGonderSonuc.Durum = 161;
							eyImzalaGonderSonuc.HataAciklama = "Akıllı Kart ile Bağlantı Kurulamadı. Akıllı Kartın Takılı Olduğundan ve Driver'larının Kurulu Olduğundan Emin Olunuz.\nC# Exception : " + ex.Message;
							LOG.Error(ex);
							EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "Akıllı Kart ile Bağlantı Kurulamadı. Akıllı Kartın Takılı Olduğundan ve Driver'larının Kurulu Olduğundan Emin Olunuz.C# Exception : " + ex.Message);
							return eyImzalaGonderSonuc;
						}
						try
						{
							sReader.FindToken(SelectedCertificateIndex);
						}
						catch (Exception ex2)
						{
							eyImzalaGonderSonuc = new EyImzalaGonderSonuc();
							eyImzalaGonderSonuc.Durum = 161;
							eyImzalaGonderSonuc.HataAciklama = "Private Key Karttan Çekilemedi. Lütfen Akıllı Kartınızın PIN'ini Kontrol Ediniz.\nC# Exception : " + ex2.Message;
							LOG.Error(ex2);
							EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "Private Key Karttan Çekilemedi. Lütfen Akıllı Kartınızın PIN'ini Kontrol Ediniz.C# Exception : " + ex2.Message);
							return eyImzalaGonderSonuc;
						}
						switch (mailType)
						{
						case EYazismaPaketTur.ETEbligat:
							mailType2 = "eTebligat";
							break;
						case EYazismaPaketTur.EYazisma:
							mailType2 = "eYazisma";
							break;
						case EYazismaPaketTur.Standart:
							mailType2 = "standart";
							break;
						}
						SmimeCreater smimeCreater = new SmimeCreater();
						byte[] array;
						try
						{
							array = smimeCreater.CreateWithSmartCard(kepHesap.kepHesap, to, cc, bcc, subject, content, attachments, mailType2, mailTypeId, sReader, ozetAlg);
						}
						catch (Exception ex3)
						{
							LOG.Error(ex3);
							EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "S/Mime Mail Oluşturulamadı.C# Exception : " + ex3.Message);
							eyImzalaGonderSonuc.Durum = 161;
							eyImzalaGonderSonuc.HataAciklama = "S/Mime Mail Oluşturulamadı.\nC# Exception : " + ex3.Message;
							return eyImzalaGonderSonuc;
						}
						if (array == null)
						{
							EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "S/Mime Mail Oluşturulamadı.");
							eyImzalaGonderSonuc.Durum = 161;
							eyImzalaGonderSonuc.HataAciklama = "S/Mime Mail Oluşturulamadı.";
							return eyImzalaGonderSonuc;
						}
						eyImzalaGonderSonuc.YukleSonuc = Yukle(client, kepHesap, array, mailType);
						eyImzalaGonderSonuc.Durum = 0;
						eyImzalaGonderSonuc.HataAciklama = "S/Mime Mail Oluşturma Başarılı.";
						eyImzalaGonderSonuc.SmimeMail = array;
						return eyImzalaGonderSonuc;
					}
				}
				EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "Akıllı Kartın Şifresini Boş Bırakamazsınız");
				eyImzalaGonderSonuc = new EyImzalaGonderSonuc();
				eyImzalaGonderSonuc.Durum = 161;
				eyImzalaGonderSonuc.HataAciklama = "Akıllı Kartın Şifresini Boş Bırakamazsınız";
				return eyImzalaGonderSonuc;
			}
			catch (Exception exception)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception);
				return null;
			}
		}

		internal EyImzalaGonderSonuc ImzalaGonder(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string smartCardPasswd, List<string> to, List<string> cc, List<string> bcc, string subject, string content, EYazismaIcerikTur icerikTur, List<Ek> attachments, EYazismaPaketTur mailType, string mailTypeId, int SelectedSmartCardIndex, int SelectedCertificateIndex, EYazismaApiConfig config)
		{
			EyImzalaGonderSonuc eyImzalaGonderSonuc = new EyImzalaGonderSonuc();
			try
			{
				string mailType2 = "";
				if (to == null || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(content))
				{
					EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "Kime, Konu ve Içerik Alanları Boş Bırakılamaz, Fonksiyona Verdiğiniz Parametreleri Kontrol Ediniz.");
					eyImzalaGonderSonuc.Durum = 161;
					eyImzalaGonderSonuc.HataAciklama = "Kime, Konu ve Içerik Alanları Boş Bırakılamaz, Fonksiyona Verdiğiniz Parametreleri Kontrol Ediniz.";
					return eyImzalaGonderSonuc;
				}
				if (smartCardPasswd != null)
				{
					long num = 0L;
					if (attachments != null && attachments.Count > 0)
					{
						foreach (Ek attachment in attachments)
						{
							num += attachment.Degeri.LongLength;
						}
						float num2 = num;
						float num3 = 0f;
						switch (config.MaxMailDosyaBoyutu.Cins)
						{
						case Boyut.Suffix.KB:
							num3 = config.MaxMailDosyaBoyutu.Size * 1024f;
							break;
						case Boyut.Suffix.MB:
							num3 = config.MaxMailDosyaBoyutu.Size * 1024f * 1024f;
							break;
						}
						num3 = num3 * 4f / 3f;
						num3 += 285004f / (float)Math.E;
						if (num2 > num3)
						{
							EyLog.Log("Imzala Gonder", EyLogTuru.HATA, "Eklerin Toplam Boyutu " + config.MaxMailDosyaBoyutu.Size + " " + config.MaxMailDosyaBoyutu.Cins.ToString() + " Boyutunu Geçemez.");
							eyImzalaGonderSonuc.HataAciklama = "Eklerin Toplam Boyutu " + config.MaxMailDosyaBoyutu.Size + " " + config.MaxMailDosyaBoyutu.Cins.ToString() + " Boyutunu Geçemez.";
							eyImzalaGonderSonuc.Durum = 161;
							return eyImzalaGonderSonuc;
						}
					}
					lock (Sync)
					{
						sReader = new SmartCardReader(smartCardPasswd, SelectedSmartCardIndex);
						try
						{
							sReader.Initialize();
						}
						catch (Exception ex)
						{
							eyImzalaGonderSonuc = new EyImzalaGonderSonuc();
							eyImzalaGonderSonuc.Durum = 161;
							eyImzalaGonderSonuc.HataAciklama = "Akıllı Kart ile Bağlantı Kurulamadı. Akıllı Kartın Takılı Olduğundan ve Driver'larının Kurulu Olduğundan Emin Olunuz.\nC# Exception : " + ex.Message;
							LOG.Error(ex);
							EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "Akıllı Kart ile Bağlantı Kurulamadı. Akıllı Kartın Takılı Olduğundan ve Driver'larının Kurulu Olduğundan Emin Olunuz.C# Exception : " + ex.Message);
							return eyImzalaGonderSonuc;
						}
						try
						{
							sReader.FindToken(SelectedCertificateIndex);
						}
						catch (Exception ex2)
						{
							eyImzalaGonderSonuc = new EyImzalaGonderSonuc();
							eyImzalaGonderSonuc.Durum = 161;
							eyImzalaGonderSonuc.HataAciklama = "Private Key Karttan Çekilemedi. Lütfen Akıllı Kartınızın PIN'ini Kontrol Ediniz.\nC# Exception : " + ex2.Message;
							LOG.Error(ex2);
							EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "Private Key Karttan Çekilemedi. Lütfen Akıllı Kartınızın PIN'ini Kontrol Ediniz.C# Exception : " + ex2.Message);
							return eyImzalaGonderSonuc;
						}
						switch (mailType)
						{
						case EYazismaPaketTur.ETEbligat:
							mailType2 = "eTebligat";
							break;
						case EYazismaPaketTur.EYazisma:
							mailType2 = "eYazisma";
							break;
						case EYazismaPaketTur.Standart:
							mailType2 = "standart";
							break;
						}
						SmimeCreater smimeCreater = new SmimeCreater();
						byte[] array;
						try
						{
							array = smimeCreater.CreateWithSmartCard(kepHesap.kepHesap, to, cc, bcc, subject, content, attachments, mailType2, mailTypeId, sReader);
						}
						catch (Exception ex3)
						{
							LOG.Error(ex3);
							EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "S/Mime Mail Oluşturulamadı.C# Exception : " + ex3.Message);
							eyImzalaGonderSonuc.Durum = 161;
							eyImzalaGonderSonuc.HataAciklama = "S/Mime Mail Oluşturulamadı.\nC# Exception : " + ex3.Message;
							return eyImzalaGonderSonuc;
						}
						if (array == null)
						{
							EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "S/Mime Mail Oluşturulamadı.");
							eyImzalaGonderSonuc.Durum = 161;
							eyImzalaGonderSonuc.HataAciklama = "S/Mime Mail Oluşturulamadı.";
							return eyImzalaGonderSonuc;
						}
						eyImzalaGonderSonuc.YukleSonuc = Yukle(client, kepHesap, array, mailType);
						eyImzalaGonderSonuc.Durum = 0;
						eyImzalaGonderSonuc.HataAciklama = "S/Mime Mail Oluşturma Başarılı.";
						eyImzalaGonderSonuc.SmimeMail = array;
						return eyImzalaGonderSonuc;
					}
				}
				EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "Akıllı Kartın Şifresini Boş Bırakamazsınız");
				eyImzalaGonderSonuc = new EyImzalaGonderSonuc();
				eyImzalaGonderSonuc.Durum = 161;
				eyImzalaGonderSonuc.HataAciklama = "Akıllı Kartın Şifresini Boş Bırakamazsınız";
				return eyImzalaGonderSonuc;
			}
			catch (Exception exception)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception);
				return null;
			}
		}

		internal EyImzalaGonderSonuc ImzalaGonder(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string smartCardPasswd, List<string> to, List<string> cc, List<string> bcc, string subject, string content, EYazismaIcerikTur icerikTur, List<Ek> attachments, EYazismaPaketTur mailType, string mailTypeId, OzetAlg ozetAlg, int SelectedSmartCardIndex, int SelectedCertificateIndex, EYazismaApiConfig config)
		{
			EyImzalaGonderSonuc eyImzalaGonderSonuc = new EyImzalaGonderSonuc();
			try
			{
				string mailType2 = "";
				if (to == null || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(content))
				{
					EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "Kime, Konu ve Içerik Alanları Boş Bırakılamaz, Fonksiyona Verdiğiniz Parametreleri Kontrol Ediniz.");
					eyImzalaGonderSonuc.Durum = 161;
					eyImzalaGonderSonuc.HataAciklama = "Kime, Konu ve Içerik Alanları Boş Bırakılamaz, Fonksiyona Verdiğiniz Parametreleri Kontrol Ediniz.";
					return eyImzalaGonderSonuc;
				}
				if (smartCardPasswd != null)
				{
					long num = 0L;
					if (attachments != null && attachments.Count > 0)
					{
						foreach (Ek attachment in attachments)
						{
							num += attachment.Degeri.LongLength;
						}
						float num2 = num;
						float num3 = 0f;
						switch (config.MaxMailDosyaBoyutu.Cins)
						{
						case Boyut.Suffix.KB:
							num3 = config.MaxMailDosyaBoyutu.Size * 1024f;
							break;
						case Boyut.Suffix.MB:
							num3 = config.MaxMailDosyaBoyutu.Size * 1024f * 1024f;
							break;
						}
						num3 = num3 * 4f / 3f;
						num3 += 285004f / (float)Math.E;
						if (num2 > num3)
						{
							EyLog.Log("Imzala Gonder", EyLogTuru.HATA, "Eklerin Toplam Boyutu " + config.MaxMailDosyaBoyutu.Size + " " + config.MaxMailDosyaBoyutu.Cins.ToString() + " Boyutunu Geçemez.");
							eyImzalaGonderSonuc.HataAciklama = "Eklerin Toplam Boyutu " + config.MaxMailDosyaBoyutu.Size + " " + config.MaxMailDosyaBoyutu.Cins.ToString() + " Boyutunu Geçemez.";
							eyImzalaGonderSonuc.Durum = 161;
							return eyImzalaGonderSonuc;
						}
					}
					lock (Sync)
					{
						sReader = new SmartCardReader(smartCardPasswd, SelectedSmartCardIndex);
						try
						{
							sReader.Initialize();
						}
						catch (Exception ex)
						{
							eyImzalaGonderSonuc = new EyImzalaGonderSonuc();
							eyImzalaGonderSonuc.Durum = 161;
							eyImzalaGonderSonuc.HataAciklama = "Akıllı Kart ile Bağlantı Kurulamadı. Akıllı Kartın Takılı Olduğundan ve Driver'larının Kurulu Olduğundan Emin Olunuz.\nC# Exception : " + ex.Message;
							LOG.Error(ex);
							EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "Akıllı Kart ile Bağlantı Kurulamadı. Akıllı Kartın Takılı Olduğundan ve Driver'larının Kurulu Olduğundan Emin Olunuz.C# Exception : " + ex.Message);
							return eyImzalaGonderSonuc;
						}
						try
						{
							sReader.FindToken(SelectedCertificateIndex);
						}
						catch (Exception ex2)
						{
							eyImzalaGonderSonuc = new EyImzalaGonderSonuc();
							eyImzalaGonderSonuc.Durum = 161;
							eyImzalaGonderSonuc.HataAciklama = "Private Key Karttan Çekilemedi. Lütfen Akıllı Kartınızın PIN'ini Kontrol Ediniz.\nC# Exception : " + ex2.Message;
							LOG.Error(ex2);
							EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "Private Key Karttan Çekilemedi. Lütfen Akıllı Kartınızın PIN'ini Kontrol Ediniz.C# Exception : " + ex2.Message);
							return eyImzalaGonderSonuc;
						}
						switch (mailType)
						{
						case EYazismaPaketTur.ETEbligat:
							mailType2 = "eTebligat";
							break;
						case EYazismaPaketTur.EYazisma:
							mailType2 = "eYazisma";
							break;
						case EYazismaPaketTur.Standart:
							mailType2 = "standart";
							break;
						}
						SmimeCreater smimeCreater = new SmimeCreater();
						byte[] array;
						try
						{
							array = smimeCreater.CreateWithSmartCard(kepHesap.kepHesap, to, cc, bcc, subject, content, attachments, mailType2, mailTypeId, sReader, ozetAlg);
						}
						catch (Exception ex3)
						{
							LOG.Error(ex3);
							EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "S/Mime Mail Oluşturulamadı.C# Exception : " + ex3.Message);
							eyImzalaGonderSonuc.Durum = 161;
							eyImzalaGonderSonuc.HataAciklama = "S/Mime Mail Oluşturulamadı.\nC# Exception : " + ex3.Message;
							return eyImzalaGonderSonuc;
						}
						if (array == null)
						{
							EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "S/Mime Mail Oluşturulamadı.");
							eyImzalaGonderSonuc.Durum = 161;
							eyImzalaGonderSonuc.HataAciklama = "S/Mime Mail Oluşturulamadı.";
							return eyImzalaGonderSonuc;
						}
						eyImzalaGonderSonuc.YukleSonuc = Yukle(client, kepHesap, array, mailType);
						eyImzalaGonderSonuc.Durum = 0;
						eyImzalaGonderSonuc.HataAciklama = "S/Mime Mail Oluşturma Başarılı.";
						eyImzalaGonderSonuc.SmimeMail = array;
						return eyImzalaGonderSonuc;
					}
				}
				EyLog.Log("Imzala Gönder", EyLogTuru.HATA, "Akıllı Kartın Şifresini Boş Bırakamazsınız");
				eyImzalaGonderSonuc = new EyImzalaGonderSonuc();
				eyImzalaGonderSonuc.Durum = 161;
				eyImzalaGonderSonuc.HataAciklama = "Akıllı Kartın Şifresini Boş Bırakamazsınız";
				return eyImzalaGonderSonuc;
			}
			catch (Exception exception)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception);
				return null;
			}
		}

		internal EyYukleSonuc Yukle(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, byte[] dosyaBytes, EYazismaPaketTur? paketTuru)
		{
			try
			{
				eyYukle eyYukle = new eyYukle();
				base64Binary base64Binary = new base64Binary();
				base64Binary.Value = dosyaBytes;
				base64Binary.contentType = "application/octet-stream";
				eyYukle.ePaket = base64Binary;
				if (paketTuru.HasValue)
				{
					eyYukle.ePaketTur = (eyPaketTur)paketTuru.Value;
					eyYukle.ePaketTurSpecified = true;
				}
				else
				{
					eyYukle.ePaketTurSpecified = false;
				}
				eyYukle.kepHesap = kepHesap;
				eyYukleSonuc eyYukleSonuc;
				try
				{
					eyYukleSonuc = client.Yukle(eyYukle);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (eyYukleSonuc.durum == 0)
				{
					EyLog.Log("Imzala Gönder", EyLogTuru.BILGI, "Durum: " + eyYukleSonuc.durum, "Sonuç: Başarılı.");
				}
				else
				{
					EyLog.Error("Imzala Gönder", eyYukleSonuc.durum.ToString(), eyYukleSonuc.hataaciklama);
				}
				return EyYukleSonuc.GetEyYukleSonuc(eyYukleSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}

		internal void CloseSmartCardSession()
		{
			try
			{
				if (sReader == null || sReader.SmartCardParams.SmartCard.Session == null || sReader.SmartCardParams.SmartCard.Slot == null)
				{
					return;
				}
				lock (Sync)
				{
					sReader.CloseSession();
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
