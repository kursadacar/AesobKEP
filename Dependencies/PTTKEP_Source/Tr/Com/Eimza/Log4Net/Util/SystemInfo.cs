using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security;

namespace Tr.Com.Eimza.Log4Net.Util
{
	public sealed class SystemInfo
	{
		private const string DEFAULT_NULL_TEXT = "(null)";

		private const string DEFAULT_NOT_AVAILABLE_TEXT = "NOT AVAILABLE";

		public static readonly Type[] EmptyTypes;

		private static string s_hostName;

		private static string s_appFriendlyName;

		private static string s_nullText;

		private static string s_notAvailableText;

		private static DateTime s_processStartTime;

		public static string NewLine
		{
			get
			{
				return Environment.NewLine;
			}
		}

		public static string ApplicationBaseDirectory
		{
			get
			{
				return AppDomain.CurrentDomain.BaseDirectory;
			}
		}

		public static string ConfigurationFileLocation
		{
			get
			{
				return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
			}
		}

		public static string EntryAssemblyLocation
		{
			get
			{
				return Assembly.GetEntryAssembly().Location;
			}
		}

		public static int CurrentThreadId
		{
			get
			{
				return AppDomain.GetCurrentThreadId();
			}
		}

		public static string HostName
		{
			get
			{
				if (s_hostName == null)
				{
					try
					{
						s_hostName = Dns.GetHostName();
					}
					catch (SocketException)
					{
					}
					catch (SecurityException)
					{
					}
					if (s_hostName == null || s_hostName.Length == 0)
					{
						try
						{
							s_hostName = Environment.MachineName;
						}
						catch (InvalidOperationException)
						{
						}
						catch (SecurityException)
						{
						}
					}
					if (s_hostName == null || s_hostName.Length == 0)
					{
						s_hostName = s_notAvailableText;
					}
				}
				return s_hostName;
			}
		}

		public static string ApplicationFriendlyName
		{
			get
			{
				if (s_appFriendlyName == null)
				{
					try
					{
						s_appFriendlyName = AppDomain.CurrentDomain.FriendlyName;
					}
					catch (SecurityException)
					{
						LogLog.Debug("SystemInfo: Security exception while trying to get current domain friendly name. Error Ignored.");
					}
					if (s_appFriendlyName == null || s_appFriendlyName.Length == 0)
					{
						try
						{
							s_appFriendlyName = Path.GetFileName(EntryAssemblyLocation);
						}
						catch (SecurityException)
						{
						}
					}
					if (s_appFriendlyName == null || s_appFriendlyName.Length == 0)
					{
						s_appFriendlyName = s_notAvailableText;
					}
				}
				return s_appFriendlyName;
			}
		}

		public static DateTime ProcessStartTime
		{
			get
			{
				return s_processStartTime;
			}
		}

		public static string NullText
		{
			get
			{
				return s_nullText;
			}
			set
			{
				s_nullText = value;
			}
		}

		public static string NotAvailableText
		{
			get
			{
				return s_notAvailableText;
			}
			set
			{
				s_notAvailableText = value;
			}
		}

		private SystemInfo()
		{
		}

		static SystemInfo()
		{
			EmptyTypes = new Type[0];
			s_processStartTime = DateTime.Now;
			string text = "(null)";
			string text2 = "NOT AVAILABLE";
			string appSetting = GetAppSetting("log4net.NullText");
			if (appSetting != null && appSetting.Length > 0)
			{
				LogLog.Debug("SystemInfo: Initializing NullText value to [" + appSetting + "].");
				text = appSetting;
			}
			string appSetting2 = GetAppSetting("log4net.NotAvailableText");
			if (appSetting2 != null && appSetting2.Length > 0)
			{
				LogLog.Debug("SystemInfo: Initializing NotAvailableText value to [" + appSetting2 + "].");
				text2 = appSetting2;
			}
			s_notAvailableText = text2;
			s_nullText = text;
		}

		public static string AssemblyLocationInfo(Assembly myAssembly)
		{
			if (myAssembly.GlobalAssemblyCache)
			{
				return "Global Assembly Cache";
			}
			try
			{
				return myAssembly.Location;
			}
			catch (SecurityException)
			{
				return "Location Permission Denied";
			}
		}

		public static string AssemblyQualifiedName(Type type)
		{
			return type.FullName + ", " + type.Assembly.FullName;
		}

		public static string AssemblyShortName(Assembly myAssembly)
		{
			string text = myAssembly.FullName;
			int num = text.IndexOf(',');
			if (num > 0)
			{
				text = text.Substring(0, num);
			}
			return text.Trim();
		}

		public static string AssemblyFileName(Assembly myAssembly)
		{
			return Path.GetFileName(myAssembly.Location);
		}

		public static Type GetTypeFromString(Type relativeType, string typeName, bool throwOnError, bool ignoreCase)
		{
			return GetTypeFromString(relativeType.Assembly, typeName, throwOnError, ignoreCase);
		}

		public static Type GetTypeFromString(string typeName, bool throwOnError, bool ignoreCase)
		{
			return GetTypeFromString(Assembly.GetCallingAssembly(), typeName, throwOnError, ignoreCase);
		}

		public static Type GetTypeFromString(Assembly relativeAssembly, string typeName, bool throwOnError, bool ignoreCase)
		{
			if (typeName.IndexOf(',') == -1)
			{
				Type type = relativeAssembly.GetType(typeName, false, ignoreCase);
				if ((object)type != null)
				{
					return type;
				}
				Assembly[] array = null;
				try
				{
					array = AppDomain.CurrentDomain.GetAssemblies();
				}
				catch (SecurityException)
				{
				}
				if (array != null)
				{
					Assembly[] array2 = array;
					foreach (Assembly assembly in array2)
					{
						type = assembly.GetType(typeName, false, ignoreCase);
						if ((object)type != null)
						{
							LogLog.Debug("SystemInfo: Loaded type [" + typeName + "] from assembly [" + assembly.FullName + "] by searching loaded assemblies.");
							return type;
						}
					}
				}
				if (throwOnError)
				{
					throw new TypeLoadException("Could not load type [" + typeName + "]. Tried assembly [" + relativeAssembly.FullName + "] and all loaded assemblies");
				}
				return null;
			}
			return Type.GetType(typeName, throwOnError, ignoreCase);
		}

		public static Guid NewGuid()
		{
			return Guid.NewGuid();
		}

		public static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(string parameterName, object actualValue, string message)
		{
			return new ArgumentOutOfRangeException(parameterName, actualValue, message);
		}

		public static bool TryParse(string s, out int val)
		{
			val = 0;
			try
			{
				double result;
				if (double.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
				{
					val = Convert.ToInt32(result);
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		public static bool TryParse(string s, out long val)
		{
			val = 0L;
			try
			{
				double result;
				if (double.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
				{
					val = Convert.ToInt64(result);
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		public static string GetAppSetting(string key)
		{
			try
			{
				return ConfigurationSettings.AppSettings[key];
			}
			catch (Exception exception)
			{
				LogLog.Error("DefaultRepositorySelector: Exception while reading ConfigurationSettings. Check your .config file is well formed XML.", exception);
			}
			return null;
		}

		public static string ConvertToFullPath(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			string text = "";
			try
			{
				string applicationBaseDirectory = ApplicationBaseDirectory;
				if (applicationBaseDirectory != null)
				{
					Uri uri = new Uri(applicationBaseDirectory);
					if (uri.IsFile)
					{
						text = uri.LocalPath;
					}
				}
			}
			catch
			{
			}
			if (text != null && text.Length > 0)
			{
				return Path.GetFullPath(Path.Combine(text, path));
			}
			return Path.GetFullPath(path);
		}

		public static Hashtable CreateCaseInsensitiveHashtable()
		{
			return CollectionsUtil.CreateCaseInsensitiveHashtable();
		}
	}
}
