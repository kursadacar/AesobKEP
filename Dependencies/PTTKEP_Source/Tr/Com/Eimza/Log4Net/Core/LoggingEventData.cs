using System;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Core
{
	public struct LoggingEventData
	{
		public string LoggerName;

		public Level Level;

		public string Message;

		public string ThreadName;

		public DateTime TimeStamp;

		public LocationInfo LocationInfo;

		public string UserName;

		public string Identity;

		public string ExceptionString;

		public string Domain;

		public PropertiesDictionary Properties;
	}
}
