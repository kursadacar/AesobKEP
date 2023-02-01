using System;
using System.IO;
using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Layout
{
	public class SimpleLayout : LayoutSkeleton
	{
		public SimpleLayout()
		{
			IgnoresException = true;
		}

		public override void ActivateOptions()
		{
		}

		public override void Format(TextWriter writer, LoggingEvent loggingEvent)
		{
			if (loggingEvent == null)
			{
				throw new ArgumentNullException("loggingEvent");
			}
			writer.Write(loggingEvent.Level.DisplayName);
			writer.Write(" - ");
			loggingEvent.WriteRenderedMessage(writer);
			writer.WriteLine();
		}
	}
}
