using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Core
{
	[Serializable]
	public class LocationInfo
	{
		private readonly string m_className;

		private readonly string m_fileName;

		private readonly string m_lineNumber;

		private readonly string m_methodName;

		private readonly string m_fullInfo;

		private const string NA = "?";

		public string ClassName
		{
			get
			{
				return m_className;
			}
		}

		public string FileName
		{
			get
			{
				return m_fileName;
			}
		}

		public string LineNumber
		{
			get
			{
				return m_lineNumber;
			}
		}

		public string MethodName
		{
			get
			{
				return m_methodName;
			}
		}

		public string FullInfo
		{
			get
			{
				return m_fullInfo;
			}
		}

		public LocationInfo(Type callerStackBoundaryDeclaringType)
		{
			m_className = "?";
			m_fileName = "?";
			m_lineNumber = "?";
			m_methodName = "?";
			m_fullInfo = "?";
			if ((object)callerStackBoundaryDeclaringType == null)
			{
				return;
			}
			try
			{
				StackTrace stackTrace = new StackTrace(true);
				int i;
				for (i = 0; i < stackTrace.FrameCount; i++)
				{
					StackFrame frame = stackTrace.GetFrame(i);
					if (frame != null && (object)frame.GetMethod().DeclaringType == callerStackBoundaryDeclaringType)
					{
						break;
					}
				}
				for (; i < stackTrace.FrameCount; i++)
				{
					StackFrame frame2 = stackTrace.GetFrame(i);
					if (frame2 != null && (object)frame2.GetMethod().DeclaringType != callerStackBoundaryDeclaringType)
					{
						break;
					}
				}
				if (i >= stackTrace.FrameCount)
				{
					return;
				}
				StackFrame frame3 = stackTrace.GetFrame(i);
				if (frame3 == null)
				{
					return;
				}
				MethodBase method = frame3.GetMethod();
				if ((object)method != null)
				{
					m_methodName = method.Name;
					if ((object)method.DeclaringType != null)
					{
						m_className = method.DeclaringType.FullName;
					}
				}
				m_fileName = frame3.GetFileName();
				m_lineNumber = frame3.GetFileLineNumber().ToString(NumberFormatInfo.InvariantInfo);
				m_fullInfo = m_className + "." + m_methodName + "(" + m_fileName + ":" + m_lineNumber + ")";
			}
			catch (SecurityException)
			{
				LogLog.Debug("LocationInfo: Security exception while trying to get caller stack frame. Error Ignored. Location Information Not Available.");
			}
		}

		public LocationInfo(string className, string methodName, string fileName, string lineNumber)
		{
			m_className = className;
			m_fileName = fileName;
			m_lineNumber = lineNumber;
			m_methodName = methodName;
			m_fullInfo = m_className + "." + m_methodName + "(" + m_fileName + ":" + m_lineNumber + ")";
		}
	}
}
