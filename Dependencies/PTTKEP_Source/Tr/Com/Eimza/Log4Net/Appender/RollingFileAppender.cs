using System;
using System.Collections;
using System.Globalization;
using System.IO;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public class RollingFileAppender : FileAppender
	{
		public enum RollingMode
		{
			Once,
			Size,
			Date,
			Composite
		}

		protected enum RollPoint
		{
			InvalidRollPoint = -1,
			TopOfMinute,
			TopOfHour,
			HalfDay,
			TopOfDay,
			TopOfWeek,
			TopOfMonth
		}

		public interface IDateTime
		{
			DateTime Now { get; }
		}

		private class DefaultDateTime : IDateTime
		{
			public DateTime Now
			{
				get
				{
					return DateTime.Now;
				}
			}
		}

		private IDateTime m_dateTime;

		private string m_datePattern = ".yyyy-MM-dd";

		private string m_scheduledFilename;

		private DateTime m_nextCheck = DateTime.MaxValue;

		private DateTime m_now;

		private RollPoint m_rollPoint;

		private long m_maxFileSize = 10485760L;

		private int m_maxSizeRollBackups;

		private int m_curSizeRollBackups;

		private int m_countDirection = -1;

		private RollingMode m_rollingStyle = RollingMode.Composite;

		private bool m_rollDate = true;

		private bool m_rollSize = true;

		private bool m_staticLogFileName = true;

		private string m_baseFileName;

		private static readonly DateTime s_date1970 = new DateTime(1970, 1, 1);

		public string DatePattern
		{
			get
			{
				return m_datePattern;
			}
			set
			{
				m_datePattern = value;
			}
		}

		public int MaxSizeRollBackups
		{
			get
			{
				return m_maxSizeRollBackups;
			}
			set
			{
				m_maxSizeRollBackups = value;
			}
		}

		public long MaxFileSize
		{
			get
			{
				return m_maxFileSize;
			}
			set
			{
				m_maxFileSize = value;
			}
		}

		public string MaximumFileSize
		{
			get
			{
				return m_maxFileSize.ToString(NumberFormatInfo.InvariantInfo);
			}
			set
			{
				m_maxFileSize = OptionConverter.ToFileSize(value, m_maxFileSize + 1);
			}
		}

		public int CountDirection
		{
			get
			{
				return m_countDirection;
			}
			set
			{
				m_countDirection = value;
			}
		}

		public RollingMode RollingStyle
		{
			get
			{
				return m_rollingStyle;
			}
			set
			{
				m_rollingStyle = value;
				switch (m_rollingStyle)
				{
				case RollingMode.Once:
					m_rollDate = false;
					m_rollSize = false;
					base.AppendToFile = false;
					break;
				case RollingMode.Size:
					m_rollDate = false;
					m_rollSize = true;
					break;
				case RollingMode.Date:
					m_rollDate = true;
					m_rollSize = false;
					break;
				case RollingMode.Composite:
					m_rollDate = true;
					m_rollSize = true;
					break;
				}
			}
		}

		public bool StaticLogFileName
		{
			get
			{
				return m_staticLogFileName;
			}
			set
			{
				m_staticLogFileName = value;
			}
		}

		public RollingFileAppender()
		{
			m_dateTime = new DefaultDateTime();
		}

		protected override void SetQWForFiles(TextWriter writer)
		{
			base.QuietWriter = new CountingQuietTextWriter(writer, ErrorHandler);
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			AdjustFileBeforeAppend();
			base.Append(loggingEvent);
		}

		protected override void Append(LoggingEvent[] loggingEvents)
		{
			AdjustFileBeforeAppend();
			base.Append(loggingEvents);
		}

		protected virtual void AdjustFileBeforeAppend()
		{
			if (m_rollDate)
			{
				DateTime now = m_dateTime.Now;
				if (now >= m_nextCheck)
				{
					m_now = now;
					m_nextCheck = NextCheckDate(m_now, m_rollPoint);
					RollOverTime(true);
				}
			}
			if (m_rollSize && File != null && ((CountingQuietTextWriter)base.QuietWriter).Count >= m_maxFileSize)
			{
				RollOverSize();
			}
		}

		protected override void OpenFile(string fileName, bool append)
		{
			lock (this)
			{
				fileName = GetNextOutputFileName(fileName);
				long count = 0L;
				if (append)
				{
					using (base.SecurityContext.Impersonate(this))
					{
						if (System.IO.File.Exists(fileName))
						{
							count = new FileInfo(fileName).Length;
						}
					}
				}
				else if (LogLog.IsErrorEnabled && m_maxSizeRollBackups != 0 && FileExists(fileName))
				{
					LogLog.Error("RollingFileAppender: INTERNAL ERROR. Append is False but OutputFile [" + fileName + "] already exists.");
				}
				if (!m_staticLogFileName)
				{
					m_scheduledFilename = fileName;
				}
				base.OpenFile(fileName, append);
				((CountingQuietTextWriter)base.QuietWriter).Count = count;
			}
		}

		protected string GetNextOutputFileName(string fileName)
		{
			if (!m_staticLogFileName)
			{
				fileName = fileName.Trim();
				if (m_rollDate)
				{
					fileName += m_now.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo);
				}
				if (m_countDirection >= 0)
				{
					fileName = fileName + "." + m_curSizeRollBackups;
				}
			}
			return fileName;
		}

		private void DetermineCurSizeRollBackups()
		{
			m_curSizeRollBackups = 0;
			string text = null;
			string baseFile = null;
			using (base.SecurityContext.Impersonate(this))
			{
				text = Path.GetFullPath(m_baseFileName);
				baseFile = Path.GetFileName(text);
			}
			ArrayList existingFiles = GetExistingFiles(text);
			InitializeRollBackups(baseFile, existingFiles);
			LogLog.Debug("RollingFileAppender: curSizeRollBackups starts at [" + m_curSizeRollBackups + "]");
		}

		private static string GetWildcardPatternForFile(string baseFileName)
		{
			return baseFileName + "*";
		}

		private ArrayList GetExistingFiles(string baseFilePath)
		{
			ArrayList arrayList = new ArrayList();
			string text = null;
			using (base.SecurityContext.Impersonate(this))
			{
				string fullPath = Path.GetFullPath(baseFilePath);
				text = Path.GetDirectoryName(fullPath);
				if (Directory.Exists(text))
				{
					string fileName = Path.GetFileName(fullPath);
					string[] files = Directory.GetFiles(text, GetWildcardPatternForFile(fileName));
					if (files != null)
					{
						for (int i = 0; i < files.Length; i++)
						{
							string fileName2 = Path.GetFileName(files[i]);
							if (fileName2.StartsWith(fileName))
							{
								arrayList.Add(fileName2);
							}
						}
					}
				}
			}
			LogLog.Debug("RollingFileAppender: Searched for existing files in [" + text + "]");
			return arrayList;
		}

		private void RollOverIfDateBoundaryCrossing()
		{
			if (m_staticLogFileName && m_rollDate && FileExists(m_baseFileName))
			{
				DateTime lastWriteTime;
				using (base.SecurityContext.Impersonate(this))
				{
					lastWriteTime = System.IO.File.GetLastWriteTime(m_baseFileName);
				}
				LogLog.Debug("RollingFileAppender: [" + lastWriteTime.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo) + "] vs. [" + m_now.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo) + "]");
				if (!lastWriteTime.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo).Equals(m_now.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo)))
				{
					m_scheduledFilename = m_baseFileName + lastWriteTime.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo);
					LogLog.Debug("RollingFileAppender: Initial roll over to [" + m_scheduledFilename + "]");
					RollOverTime(false);
					LogLog.Debug("RollingFileAppender: curSizeRollBackups after rollOver at [" + m_curSizeRollBackups + "]");
				}
			}
		}

		protected void ExistingInit()
		{
			DetermineCurSizeRollBackups();
			RollOverIfDateBoundaryCrossing();
			if (base.AppendToFile)
			{
				return;
			}
			bool flag = false;
			string nextOutputFileName = GetNextOutputFileName(m_baseFileName);
			using (base.SecurityContext.Impersonate(this))
			{
				flag = System.IO.File.Exists(nextOutputFileName);
			}
			if (flag)
			{
				if (m_maxSizeRollBackups == 0)
				{
					LogLog.Debug("RollingFileAppender: Output file [" + nextOutputFileName + "] already exists. MaxSizeRollBackups is 0; cannot roll. Overwriting existing file.");
					return;
				}
				LogLog.Debug("RollingFileAppender: Output file [" + nextOutputFileName + "] already exists. Not appending to file. Rolling existing file out of the way.");
				RollOverRenameFiles(nextOutputFileName);
			}
		}

		private void InitializeFromOneFile(string baseFile, string curFileName)
		{
			if (!curFileName.StartsWith(baseFile) || curFileName.Equals(baseFile))
			{
				return;
			}
			int num = curFileName.LastIndexOf(".");
			if (-1 == num)
			{
				return;
			}
			if (m_staticLogFileName)
			{
				int num2 = curFileName.Length - num;
				if (baseFile.Length + num2 != curFileName.Length)
				{
					return;
				}
			}
			if (m_rollDate && !m_staticLogFileName && !curFileName.StartsWith(baseFile + m_dateTime.Now.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo)))
			{
				LogLog.Debug("RollingFileAppender: Ignoring file [" + curFileName + "] because it is from a different date period");
				return;
			}
			try
			{
				int val;
				if (!SystemInfo.TryParse(curFileName.Substring(num + 1), out val) || val <= m_curSizeRollBackups)
				{
					return;
				}
				if (m_maxSizeRollBackups != 0)
				{
					if (-1 == m_maxSizeRollBackups)
					{
						m_curSizeRollBackups = val;
					}
					else if (m_countDirection >= 0)
					{
						m_curSizeRollBackups = val;
					}
					else if (val <= m_maxSizeRollBackups)
					{
						m_curSizeRollBackups = val;
					}
				}
				LogLog.Debug("RollingFileAppender: File name [" + curFileName + "] moves current count to [" + m_curSizeRollBackups + "]");
			}
			catch (FormatException)
			{
				LogLog.Debug("RollingFileAppender: Encountered a backup file not ending in .x [" + curFileName + "]");
			}
		}

		private void InitializeRollBackups(string baseFile, ArrayList arrayFiles)
		{
			if (arrayFiles == null)
			{
				return;
			}
			string baseFile2 = baseFile.ToLower(CultureInfo.InvariantCulture);
			foreach (string arrayFile in arrayFiles)
			{
				InitializeFromOneFile(baseFile2, arrayFile.ToLower(CultureInfo.InvariantCulture));
			}
		}

		private RollPoint ComputeCheckPeriod(string datePattern)
		{
			string text = s_date1970.ToString(datePattern, DateTimeFormatInfo.InvariantInfo);
			for (int i = 0; i <= 5; i++)
			{
				string text2 = NextCheckDate(s_date1970, (RollPoint)i).ToString(datePattern, DateTimeFormatInfo.InvariantInfo);
				LogLog.Debug("RollingFileAppender: Type = [" + i + "], r0 = [" + text + "], r1 = [" + text2 + "]");
				if (text != null && text2 != null && !text.Equals(text2))
				{
					return (RollPoint)i;
				}
			}
			return RollPoint.InvalidRollPoint;
		}

		public override void ActivateOptions()
		{
			if (m_rollDate && m_datePattern != null)
			{
				m_now = m_dateTime.Now;
				m_rollPoint = ComputeCheckPeriod(m_datePattern);
				if (m_rollPoint == RollPoint.InvalidRollPoint)
				{
					throw new ArgumentException("Invalid RollPoint, unable to parse [" + m_datePattern + "]");
				}
				m_nextCheck = NextCheckDate(m_now, m_rollPoint);
			}
			else if (m_rollDate)
			{
				ErrorHandler.Error("Either DatePattern or rollingStyle options are not set for [" + base.Name + "].");
			}
			if (base.SecurityContext == null)
			{
				base.SecurityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
			}
			using (base.SecurityContext.Impersonate(this))
			{
				base.File = FileAppender.ConvertToFullPath(base.File.Trim());
				m_baseFileName = base.File;
			}
			if (m_rollDate && File != null && m_scheduledFilename == null)
			{
				m_scheduledFilename = File + m_now.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo);
			}
			ExistingInit();
			base.ActivateOptions();
		}

		protected void RollOverTime(bool fileIsOpen)
		{
			if (m_staticLogFileName)
			{
				if (m_datePattern == null)
				{
					ErrorHandler.Error("Missing DatePattern option in rollOver().");
					return;
				}
				string text = m_now.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo);
				if (m_scheduledFilename.Equals(File + text))
				{
					ErrorHandler.Error("Compare " + m_scheduledFilename + " : " + File + text);
					return;
				}
				if (fileIsOpen)
				{
					CloseFile();
				}
				for (int i = 1; i <= m_curSizeRollBackups; i++)
				{
					string fromFile = File + "." + i;
					string toFile = m_scheduledFilename + "." + i;
					RollFile(fromFile, toFile);
				}
				RollFile(File, m_scheduledFilename);
			}
			m_curSizeRollBackups = 0;
			m_scheduledFilename = File + m_now.ToString(m_datePattern, DateTimeFormatInfo.InvariantInfo);
			if (fileIsOpen)
			{
				SafeOpenFile(m_baseFileName, false);
			}
		}

		protected void RollFile(string fromFile, string toFile)
		{
			if (FileExists(fromFile))
			{
				DeleteFile(toFile);
				try
				{
					LogLog.Debug("RollingFileAppender: Moving [" + fromFile + "] -> [" + toFile + "]");
					using (base.SecurityContext.Impersonate(this))
					{
						System.IO.File.Move(fromFile, toFile);
						return;
					}
				}
				catch (Exception e)
				{
					ErrorHandler.Error("Exception while rolling file [" + fromFile + "] -> [" + toFile + "]", e, ErrorCode.GenericFailure);
					return;
				}
			}
			LogLog.Warn("RollingFileAppender: Cannot RollFile [" + fromFile + "] -> [" + toFile + "]. Source does not exist");
		}

		protected bool FileExists(string path)
		{
			using (base.SecurityContext.Impersonate(this))
			{
				return System.IO.File.Exists(path);
			}
		}

		protected void DeleteFile(string fileName)
		{
			if (!FileExists(fileName))
			{
				return;
			}
			string text = fileName;
			string text2 = fileName + "." + Environment.TickCount + ".DeletePending";
			try
			{
				using (base.SecurityContext.Impersonate(this))
				{
					System.IO.File.Move(fileName, text2);
				}
				text = text2;
			}
			catch (Exception exception)
			{
				LogLog.Debug("RollingFileAppender: Exception while moving file to be deleted [" + fileName + "] -> [" + text2 + "]", exception);
			}
			try
			{
				using (base.SecurityContext.Impersonate(this))
				{
					System.IO.File.Delete(text);
				}
				LogLog.Debug("RollingFileAppender: Deleted file [" + fileName + "]");
			}
			catch (Exception ex)
			{
				if (text == fileName)
				{
					ErrorHandler.Error("Exception while deleting file [" + text + "]", ex, ErrorCode.GenericFailure);
				}
				else
				{
					LogLog.Debug("RollingFileAppender: Exception while deleting temp file [" + text + "]", ex);
				}
			}
		}

		protected void RollOverSize()
		{
			CloseFile();
			LogLog.Debug("RollingFileAppender: rolling over count [" + ((CountingQuietTextWriter)base.QuietWriter).Count + "]");
			LogLog.Debug("RollingFileAppender: maxSizeRollBackups [" + m_maxSizeRollBackups + "]");
			LogLog.Debug("RollingFileAppender: curSizeRollBackups [" + m_curSizeRollBackups + "]");
			LogLog.Debug("RollingFileAppender: countDirection [" + m_countDirection + "]");
			RollOverRenameFiles(File);
			if (!m_staticLogFileName && m_countDirection >= 0)
			{
				m_curSizeRollBackups++;
			}
			SafeOpenFile(m_baseFileName, false);
		}

		protected void RollOverRenameFiles(string baseFileName)
		{
			if (m_maxSizeRollBackups == 0)
			{
				return;
			}
			if (m_countDirection < 0)
			{
				if (m_curSizeRollBackups == m_maxSizeRollBackups)
				{
					DeleteFile(baseFileName + "." + m_maxSizeRollBackups);
					m_curSizeRollBackups--;
				}
				for (int num = m_curSizeRollBackups; num >= 1; num--)
				{
					RollFile(baseFileName + "." + num, baseFileName + "." + (num + 1));
				}
				m_curSizeRollBackups++;
				RollFile(baseFileName, baseFileName + ".1");
				return;
			}
			if (m_curSizeRollBackups >= m_maxSizeRollBackups && m_maxSizeRollBackups > 0)
			{
				int num2 = m_curSizeRollBackups - m_maxSizeRollBackups;
				if (m_staticLogFileName)
				{
					num2++;
				}
				string text = baseFileName;
				if (!m_staticLogFileName)
				{
					int num3 = text.LastIndexOf(".");
					if (num3 >= 0)
					{
						text = text.Substring(0, num3);
					}
				}
				DeleteFile(text + "." + num2);
			}
			if (m_staticLogFileName)
			{
				m_curSizeRollBackups++;
				RollFile(baseFileName, baseFileName + "." + m_curSizeRollBackups);
			}
		}

		protected DateTime NextCheckDate(DateTime currentDateTime, RollPoint rollPoint)
		{
			DateTime result = currentDateTime;
			switch (rollPoint)
			{
			case RollPoint.TopOfMinute:
				result = result.AddMilliseconds(-result.Millisecond);
				result = result.AddSeconds(-result.Second).AddMinutes(1.0);
				break;
			case RollPoint.TopOfHour:
				result = result.AddMilliseconds(-result.Millisecond);
				result = result.AddSeconds(-result.Second);
				result = result.AddMinutes(-result.Minute).AddHours(1.0);
				break;
			case RollPoint.HalfDay:
				result = result.AddMilliseconds(-result.Millisecond);
				result = result.AddSeconds(-result.Second);
				result = result.AddMinutes(-result.Minute);
				result = ((result.Hour >= 12) ? result.AddHours(-result.Hour).AddDays(1.0) : result.AddHours(12 - result.Hour));
				break;
			case RollPoint.TopOfDay:
				result = result.AddMilliseconds(-result.Millisecond);
				result = result.AddSeconds(-result.Second);
				result = result.AddMinutes(-result.Minute);
				result = result.AddHours(-result.Hour).AddDays(1.0);
				break;
			case RollPoint.TopOfWeek:
				result = result.AddMilliseconds(-result.Millisecond);
				result = result.AddSeconds(-result.Second);
				result = result.AddMinutes(-result.Minute);
				result = result.AddHours(-result.Hour);
				result = result.AddDays((double)(7 - result.DayOfWeek));
				break;
			case RollPoint.TopOfMonth:
				result = result.AddMilliseconds(-result.Millisecond);
				result = result.AddSeconds(-result.Second);
				result = result.AddMinutes(-result.Minute);
				result = result.AddHours(-result.Hour);
				result = result.AddDays(1 - result.Day).AddMonths(1);
				break;
			}
			return result;
		}
	}
}
