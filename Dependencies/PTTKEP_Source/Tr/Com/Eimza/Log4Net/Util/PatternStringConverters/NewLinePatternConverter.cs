using System.Globalization;
using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Util.PatternStringConverters
{
	internal sealed class NewLinePatternConverter : LiteralPatternConverter, IOptionHandler
	{
		public void ActivateOptions()
		{
			if (string.Compare(Option, "DOS", true, CultureInfo.InvariantCulture) == 0)
			{
				Option = "\r\n";
			}
			else if (string.Compare(Option, "UNIX", true, CultureInfo.InvariantCulture) == 0)
			{
				Option = "\n";
			}
			else
			{
				Option = SystemInfo.NewLine;
			}
		}
	}
}
