using System;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaDizinSorgula
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyDizinSonuc DizinSorgula(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap)
		{
			try
			{
				string[] array = null;
				try
				{
					array = client.DizinSorgula(kepHesap);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (array == null || array.Length == 0)
				{
					EyLog.Log("Dizin Sorgula", EyLogTuru.HATA, "Web Servisten Dizin Bilgisi Boş Döndü.");
				}
				return EyDizinSonuc.GetEyDizinSonuc(array);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}
	}
}
