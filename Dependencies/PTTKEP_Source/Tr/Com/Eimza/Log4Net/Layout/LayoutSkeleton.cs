using System.IO;
using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Layout
{
	public abstract class LayoutSkeleton : ILayout, IOptionHandler
	{
		private string m_header;

		private string m_footer;

		private bool m_ignoresException = true;

		public virtual string ContentType
		{
			get
			{
				return "text/plain";
			}
		}

		public virtual string Header
		{
			get
			{
				return m_header;
			}
			set
			{
				m_header = value;
			}
		}

		public virtual string Footer
		{
			get
			{
				return m_footer;
			}
			set
			{
				m_footer = value;
			}
		}

		public virtual bool IgnoresException
		{
			get
			{
				return m_ignoresException;
			}
			set
			{
				m_ignoresException = value;
			}
		}

		public abstract void ActivateOptions();

		public abstract void Format(TextWriter writer, LoggingEvent loggingEvent);
	}
}
