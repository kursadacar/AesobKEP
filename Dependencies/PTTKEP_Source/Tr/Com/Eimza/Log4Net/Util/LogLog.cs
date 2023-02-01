#define TRACE
using System;
using System.Diagnostics;

namespace Tr.Com.Eimza.Log4Net.Util
{
	public sealed class LogLog
	{
		private static bool s_debugEnabled;

		private static bool s_quietMode;

		private const string PREFIX = "log4net: ";

		private const string ERR_PREFIX = "log4net:ERROR ";

		private const string WARN_PREFIX = "log4net:WARN ";

		public static bool InternalDebugging
		{
			get
			{
				return s_debugEnabled;
			}
			set
			{
				s_debugEnabled = value;
			}
		}

		public static bool QuietMode
		{
			get
			{
				return s_quietMode;
			}
			set
			{
				s_quietMode = value;
			}
		}

		public static bool IsDebugEnabled
		{
			get
			{
				if (s_debugEnabled)
				{
					return !s_quietMode;
				}
				return false;
			}
		}

		public static bool IsWarnEnabled
		{
			get
			{
				return !s_quietMode;
			}
		}

		public static bool IsErrorEnabled
		{
			get
			{
				return !s_quietMode;
			}
		}

		private LogLog()
		{
		}

		static LogLog()
		{
			try
			{
				InternalDebugging = OptionConverter.ToBoolean(SystemInfo.GetAppSetting("log4net.Internal.Debug"), false);
				QuietMode = OptionConverter.ToBoolean(SystemInfo.GetAppSetting("log4net.Internal.Quiet"), false);
			}
			catch (Exception exception)
			{
				Error("LogLog: Exception while reading ConfigurationSettings. Check your .config file is well formed XML.", exception);
			}
		}

		public static void Debug(string message)
		{
			if (IsDebugEnabled)
			{
				EmitOutLine("log4net: " + message);
			}
		}

		public static void Debug(string message, Exception exception)
		{
			if (IsDebugEnabled)
			{
				EmitOutLine("log4net: " + message);
				if (exception != null)
				{
					EmitOutLine(exception.ToString());
				}
			}
		}

		public static void Warn(string message)
		{
			if (IsWarnEnabled)
			{
				EmitErrorLine("log4net:WARN " + message);
			}
		}

		public static void Warn(string message, Exception exception)
		{
			if (IsWarnEnabled)
			{
				EmitErrorLine("log4net:WARN " + message);
				if (exception != null)
				{
					EmitErrorLine(exception.ToString());
				}
			}
		}

		public static void Error(string message)
		{
			if (IsErrorEnabled)
			{
				EmitErrorLine("log4net:ERROR " + message);
			}
		}

		public static void Error(string message, Exception exception)
		{
			if (IsErrorEnabled)
			{
				EmitErrorLine("log4net:ERROR " + message);
				if (exception != null)
				{
					EmitErrorLine(exception.ToString());
				}
			}
		}

		private static void EmitOutLine(string message)
		{
			try
			{
				Console.Out.WriteLine(message);
				Trace.WriteLine(message);
			}
			catch
			{
			}
		}

		private static void EmitErrorLine(string message)
		{
			try
			{
				Console.Error.WriteLine(message);
				Trace.WriteLine(message);
			}
			catch
			{
			}
		}
	}
}
