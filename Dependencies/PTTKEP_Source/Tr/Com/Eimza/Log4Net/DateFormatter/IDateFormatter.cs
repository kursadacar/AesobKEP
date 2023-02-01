using System;
using System.IO;

namespace Tr.Com.Eimza.Log4Net.DateFormatter
{
	public interface IDateFormatter
	{
		void FormatDate(DateTime dateToFormat, TextWriter writer);
	}
}
