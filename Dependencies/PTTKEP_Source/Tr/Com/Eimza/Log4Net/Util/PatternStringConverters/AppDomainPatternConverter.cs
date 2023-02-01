using System.IO;

namespace Tr.Com.Eimza.Log4Net.Util.PatternStringConverters
{
	internal sealed class AppDomainPatternConverter : PatternConverter
	{
		protected override void Convert(TextWriter writer, object state)
		{
			writer.Write(SystemInfo.ApplicationFriendlyName);
		}
	}
}
