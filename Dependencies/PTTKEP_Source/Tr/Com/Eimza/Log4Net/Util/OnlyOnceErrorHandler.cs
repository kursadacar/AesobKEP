using System;
using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Util
{
	public class OnlyOnceErrorHandler : IErrorHandler
	{
		private bool m_firstTime = true;

		private readonly string m_prefix;

		private bool IsEnabled
		{
			get
			{
				if (m_firstTime)
				{
					m_firstTime = false;
					return true;
				}
				if (LogLog.InternalDebugging && !LogLog.QuietMode)
				{
					return true;
				}
				return false;
			}
		}

		public OnlyOnceErrorHandler()
		{
			m_prefix = "";
		}

		public OnlyOnceErrorHandler(string prefix)
		{
			m_prefix = prefix;
		}

		public void Error(string message, Exception e, ErrorCode errorCode)
		{
			if (IsEnabled)
			{
				LogLog.Error("[" + m_prefix + "] " + message, e);
			}
		}

		public void Error(string message, Exception e)
		{
			if (IsEnabled)
			{
				LogLog.Error("[" + m_prefix + "] " + message, e);
			}
		}

		public void Error(string message)
		{
			if (IsEnabled)
			{
				LogLog.Error("[" + m_prefix + "] " + message);
			}
		}
	}
}
