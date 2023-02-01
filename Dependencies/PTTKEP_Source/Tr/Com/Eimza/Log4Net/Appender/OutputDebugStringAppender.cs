using System.Runtime.InteropServices;
using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public class OutputDebugStringAppender : AppenderSkeleton
	{
		protected override bool RequiresLayout
		{
			get
			{
				return true;
			}
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			OutputDebugString(RenderLoggingEvent(loggingEvent));
		}

		[DllImport("kernel32.dll")]
		protected static extern void OutputDebugString(string message);
	}
}
