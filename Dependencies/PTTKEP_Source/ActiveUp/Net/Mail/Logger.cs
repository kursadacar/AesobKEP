using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Web;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class Logger
	{
		private static string _logFile = string.Empty;

		private static ArrayList _logEntries = new ArrayList();

		private static bool _logInMemory;

		private static bool _disabled;

		private static bool _useTraceContext;

		private static bool _useTraceConsole;

		private static int _logLevel;

		private static bool _isChecked = false;

		public static ArrayList LogEntries
		{
			get
			{
				return _logEntries ?? (_logEntries = new ArrayList());
			}
			set
			{
				_logEntries = value;
			}
		}

		public static int LogLevel
		{
			get
			{
				return _logLevel;
			}
			set
			{
				_logLevel = value;
			}
		}

		public static bool LogInMemory
		{
			get
			{
				return _logInMemory;
			}
			set
			{
				_logInMemory = value;
			}
		}

		public static string LogFile
		{
			get
			{
				return _logFile;
			}
			set
			{
				_logFile = value;
			}
		}

		public static int Count
		{
			get
			{
				if (_logEntries != null)
				{
					return _logEntries.Count;
				}
				return 0;
			}
		}

		public static bool UseTraceContext
		{
			get
			{
				return _useTraceContext;
			}
			set
			{
				_useTraceContext = value;
			}
		}

		public static bool UseTraceConsole
		{
			get
			{
				return _useTraceConsole;
			}
			set
			{
				_useTraceConsole = value;
			}
		}

		public static bool Disabled
		{
			get
			{
				return _disabled;
			}
			set
			{
				_disabled = value;
			}
		}

		public static event EventHandler EntryAdded;

		public static void AddEntry(string line, int level)
		{
			if (!_disabled && level >= _logLevel)
			{
				AddEntry(line);
			}
		}

		public static void AddEntry(string line)
		{
			if (!_disabled)
			{
				DateTime now = DateTime.Now;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(now.Year.ToString());
				stringBuilder.Append(".");
				stringBuilder.Append(now.Month.ToString().PadLeft(2, '0'));
				stringBuilder.Append(".");
				stringBuilder.Append(now.Day.ToString().PadLeft(2, '0'));
				stringBuilder.Append("-");
				stringBuilder.Append(now.Hour.ToString().PadLeft(2, '0'));
				stringBuilder.Append(":");
				stringBuilder.Append(now.Minute.ToString().PadLeft(2, '0'));
				stringBuilder.Append(":");
				stringBuilder.Append(now.Second.ToString().PadLeft(2, '0'));
				stringBuilder.Append(" ");
				stringBuilder.Append(line);
				if (!string.IsNullOrEmpty(_logFile))
				{
					AddEntryToFile(stringBuilder.ToString());
				}
				if (_useTraceContext)
				{
					AddEntryToTrace(stringBuilder.ToString());
				}
				if (_useTraceConsole)
				{
					AddEntryToConsole(stringBuilder.ToString());
				}
				OnEntryAdded(EventArgs.Empty);
			}
		}

		protected static void AddEntryToFile(string line)
		{
			if (!_disabled)
			{
				StreamWriter streamWriter = new StreamWriter(_logFile, true, Encoding.Default);
				streamWriter.WriteLine(line);
				streamWriter.Close();
			}
		}

		protected static void AddEntryToConsole(string line)
		{
			if (!_disabled)
			{
				Console.WriteLine("ActiveMail:{0}", line);
			}
		}

		protected static void AddEntryToTrace(string line)
		{
			if (!_disabled && HttpContext.Current != null)
			{
				HttpContext.Current.Trace.Write("ActiveMail", line);
			}
		}

		public static ArrayList LastEntries(int lines)
		{
			ArrayList arrayList = new ArrayList();
			int count = Count;
			if (count > 0)
			{
				for (int i = count - lines; i <= count; i++)
				{
					if (i >= 0 && _logEntries != null)
					{
						arrayList.Add(_logEntries[i]);
					}
				}
			}
			return arrayList;
		}

		public static string LastEntries()
		{
			ArrayList arrayList = LastEntries(30);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string item in arrayList)
			{
				stringBuilder.Append(item + "\n");
			}
			return stringBuilder.ToString();
		}

		public static string LastEntry()
		{
			if (_logEntries != null)
			{
				return _logEntries[_logEntries.Count - 1].ToString();
			}
			return string.Empty;
		}

		protected static void OnEntryAdded(EventArgs e)
		{
			if (Logger.EntryAdded != null)
			{
				Logger.EntryAdded(null, e);
			}
		}
	}
}
