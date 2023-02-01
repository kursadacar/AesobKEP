using System;
using System.Reflection;
using System.Text;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma.Utilities
{
	public static class EyLog
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static StringBuilder eYazismaApiLog = new StringBuilder();

		public static StringBuilder EYazismaApiLog
		{
			get
			{
				return eYazismaApiLog;
			}
			set
			{
				eYazismaApiLog = value;
			}
		}

		public static void Log(string log)
		{
			eYazismaApiLog.AppendLine("[TARIH : " + DateTime.Now.ToString() + "]").Append(log);
			eYazismaApiLog.AppendLine();
		}

		public static void Log(string log, int olay)
		{
			eYazismaApiLog.AppendLine();
			switch (olay)
			{
			case 0:
				eYazismaApiLog.Append("[TARIH : " + DateTime.Now.ToString() + "] - [BILGI] - ").Append(log);
				break;
			case 1:
				eYazismaApiLog.Append("[TARIH : " + DateTime.Now.ToString() + "] - [UYARI] - ").Append(log);
				break;
			case 2:
				eYazismaApiLog.Append("[TARIH : " + DateTime.Now.ToString() + "] - [HATA] - ").Append(log);
				break;
			}
		}

		public static void Error(string className, int durum, string hataAciklama)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("EYazismaApi " + className + " Fonksiyonu");
			stringBuilder.Append(" - ");
			if (!string.IsNullOrEmpty(durum.ToString()))
			{
				stringBuilder.Append(string.Format("[Durum : {0}]", durum.ToString()));
			}
			if (!string.IsNullOrEmpty(hataAciklama))
			{
				stringBuilder.Append(" - ");
				stringBuilder.Append(string.Format("[Hata : {0}]", hataAciklama));
			}
			Log(stringBuilder.ToString(), 2);
		}

		public static void Error(string className, string durum, string hataAciklama)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("EYazismaApi " + className + " Fonksiyonu");
			stringBuilder.Append(" - ");
			if (!string.IsNullOrEmpty(durum))
			{
				stringBuilder.Append(string.Format("[Durum : {0}]", durum));
			}
			if (!string.IsNullOrEmpty(hataAciklama))
			{
				stringBuilder.Append(" - ");
				stringBuilder.Append(string.Format("[Hata : {0}]", hataAciklama));
			}
			Log(stringBuilder.ToString(), 2);
		}

		public static void Log(string className, EyLogTuru logTuru, params string[] log)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("EYazismaApi " + className + " Fonksiyonu");
			stringBuilder.Append(" - ");
			for (int i = 0; i < log.Length; i++)
			{
				stringBuilder.Append("[");
				if (!string.IsNullOrEmpty(log[i]))
				{
					stringBuilder.Append(string.Format("{0}", log[i]));
				}
				stringBuilder.Append("]");
				if (i != log.Length - 1)
				{
					stringBuilder.Append(" - ");
				}
			}
			switch (logTuru)
			{
			case EyLogTuru.BILGI:
				Log(stringBuilder.ToString(), 0);
				break;
			case EyLogTuru.UYARI:
				Log(stringBuilder.ToString(), 1);
				break;
			case EyLogTuru.HATA:
				Log(stringBuilder.ToString(), 2);
				break;
			}
		}

		public static void Log(Type className, EyLogTuru logTuru, Exception log)
		{
			Log(className.Name, logTuru, log.StackTrace);
		}

		public static void Log(string className, string olay, string kepHesabi)
		{
			Log(className + " " + olay + ". [Kep Hesabı : " + kepHesabi + "]", 0);
		}

		public static StringBuilder GetYazdir()
		{
			Console.WriteLine(eYazismaApiLog);
			return eYazismaApiLog;
		}
	}
}
