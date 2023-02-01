using System.IO;
using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Layout
{
	public interface ILayout
	{
		string ContentType { get; }

		string Header { get; }

		string Footer { get; }

		bool IgnoresException { get; }

		void Format(TextWriter writer, LoggingEvent loggingEvent);
	}
}
