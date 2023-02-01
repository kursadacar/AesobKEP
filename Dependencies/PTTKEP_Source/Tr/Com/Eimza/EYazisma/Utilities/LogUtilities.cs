using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Tr.Com.Eimza.Log4Net;
using Tr.Com.Eimza.Log4Net.Appender;
using Tr.Com.Eimza.Log4Net.Config;
using Tr.Com.Eimza.Log4Net.Layout;

namespace Tr.Com.Eimza.EYazisma.Utilities
{
	public static class LogUtilities
	{
		private static readonly ILog LOG = LogManager.GetLogger(typeof(LogUtilities));

		public static void Write(string EYAZISMA_API_LOG, string EY_LOGLARI, string LOG_CONFIG_FILE)
		{
			if (string.IsNullOrEmpty(EyLog.EYazismaApiLog.ToString()))
			{
				return;
			}
			try
			{
				if (File.Exists(EY_LOGLARI))
				{
					if ((ulong)new FileInfo(EY_LOGLARI).Length / 1024uL / 1024uL > 10)
					{
						File.Move(EY_LOGLARI, EY_LOGLARI + "_100MB");
						CreateLogFile(EYAZISMA_API_LOG, EY_LOGLARI, LOG_CONFIG_FILE);
						StreamWriter streamWriter = File.CreateText(EY_LOGLARI);
						streamWriter.WriteLine(EyLog.EYazismaApiLog.ToString());
						streamWriter.Close();
					}
					else
					{
						StreamWriter streamWriter2 = File.AppendText(EY_LOGLARI);
						streamWriter2.WriteLine(EyLog.EYazismaApiLog.ToString());
						streamWriter2.Close();
					}
				}
				else
				{
					StreamWriter streamWriter3 = File.CreateText(EY_LOGLARI);
					streamWriter3.WriteLine(EyLog.EYazismaApiLog.ToString());
					streamWriter3.Close();
				}
			}
			catch (Exception)
			{
				LOG.Error(EyLog.EYazismaApiLog);
			}
			EyLog.EYazismaApiLog = new StringBuilder();
		}

		public static void CreateLogFile(string EYAZISMA_API_LOG, string EY_LOGLARI, string LOG_CONFIG_FILE)
		{
			try
			{
				string text = (IntPtr.Size.Equals(4) ? "32 Bit" : "64 Bit");
				string text2 = "------------------------------------------------------------------------" + Environment.NewLine + ".NETFrameWork Versiyonu: " + RuntimeEnvironment.GetSystemVersion() + Environment.NewLine + "İşletim Sistemi: " + Environment.OSVersion.ToString() + " " + Environment.NewLine + "İşletim Sistemi Versiyonu: " + Environment.OSVersion.VersionString + " " + text + Environment.NewLine + "------------------------------------------------------------------------";
				if (File.Exists(EYAZISMA_API_LOG) && !File.ReadAllText(EYAZISMA_API_LOG, Encoding.GetEncoding(Util.TURKISH_ENCODING)).Contains(".NETFrameWork Versiyonu:"))
				{
					File.Delete(EYAZISMA_API_LOG);
					BasicConfigurator.Configure(GetDefaultLogAppender(EYAZISMA_API_LOG));
					LOG.Info(Environment.NewLine + text2 + Environment.NewLine);
				}
				else if (!File.Exists(EYAZISMA_API_LOG))
				{
					BasicConfigurator.Configure(GetDefaultLogAppender(EYAZISMA_API_LOG));
					LOG.Info(Environment.NewLine + text2 + Environment.NewLine);
				}
				else
				{
					BasicConfigurator.Configure(GetDefaultLogAppender(EYAZISMA_API_LOG));
					if (!File.GetLastAccessTime(EYAZISMA_API_LOG).ToShortDateString().Equals(DateTime.Now.ToShortDateString()))
					{
						LOG.Info(Environment.NewLine + text2 + Environment.NewLine);
					}
				}
				if (File.Exists(EY_LOGLARI) && !File.GetLastAccessTime(EY_LOGLARI).ToShortDateString().Equals(DateTime.Now.ToShortDateString()))
				{
					File.Move(EY_LOGLARI, EY_LOGLARI + "." + File.GetLastAccessTime(EY_LOGLARI).ToShortDateString());
				}
				if (File.Exists(EY_LOGLARI) && File.ReadAllText(EY_LOGLARI, Encoding.GetEncoding(Util.TURKISH_ENCODING)).Length <= text2.Length)
				{
					File.Delete(EY_LOGLARI);
					EyLog.EYazismaApiLog = new StringBuilder();
					EyLog.Log(text2);
					Write(EYAZISMA_API_LOG, EY_LOGLARI, LOG_CONFIG_FILE);
				}
				else if (!File.Exists(EY_LOGLARI))
				{
					EyLog.EYazismaApiLog = new StringBuilder();
					EyLog.Log(text2);
					Write(EYAZISMA_API_LOG, EY_LOGLARI, LOG_CONFIG_FILE);
				}
			}
			catch (Exception exception)
			{
				LOG.Error("Log Dosyalarını Oluşturmada Hata Meydana Geldi.", exception);
			}
		}

		public static IAppender GetDefaultLogAppender(string EYAZISMA_API_LOG)
		{
			string conversionPattern = "Tarih : [%date{dd-MM-yyyy - HH:mm:ss}] %-5level %logger - %message%newline";
			RollingFileAppender obj = new RollingFileAppender
			{
				File = EYAZISMA_API_LOG,
				AppendToFile = true,
				LockingModel = new FileAppender.MinimalLock(),
				MaximumFileSize = "100MB"
			};
			PatternLayout patternLayout = new PatternLayout
			{
				ConversionPattern = conversionPattern
			};
			obj.Layout = patternLayout;
			patternLayout.ActivateOptions();
			obj.ActivateOptions();
			return obj;
		}
	}
}
