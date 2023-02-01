using System.Diagnostics;
using System.IO;
using System.Security;

namespace Tr.Com.Eimza.Log4Net.Util.PatternStringConverters
{
	internal sealed class ProcessIdPatternConverter : PatternConverter
	{
		protected override void Convert(TextWriter writer, object state)
		{
			try
			{
				writer.Write(Process.GetCurrentProcess().Id);
			}
			catch (SecurityException)
			{
				LogLog.Debug("ProcessIdPatternConverter: Security exception while trying to get current process id. Error Ignored.");
				writer.Write(SystemInfo.NotAvailableText);
			}
		}
	}
}
