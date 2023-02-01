using System.IO;
using System.Security;
using System.Security.Principal;

namespace Tr.Com.Eimza.Log4Net.Util.PatternStringConverters
{
	internal sealed class UserNamePatternConverter : PatternConverter
	{
		protected override void Convert(TextWriter writer, object state)
		{
			try
			{
				WindowsIdentity windowsIdentity = null;
				windowsIdentity = WindowsIdentity.GetCurrent();
				if (windowsIdentity != null && windowsIdentity.Name != null)
				{
					writer.Write(windowsIdentity.Name);
				}
			}
			catch (SecurityException)
			{
				LogLog.Debug("UserNamePatternConverter: Security exception while trying to get current windows identity. Error Ignored.");
				writer.Write(SystemInfo.NotAvailableText);
			}
		}
	}
}
