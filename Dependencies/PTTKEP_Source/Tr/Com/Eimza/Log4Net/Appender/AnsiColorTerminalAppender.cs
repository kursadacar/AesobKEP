using System;
using System.Globalization;
using System.Text;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public class AnsiColorTerminalAppender : AppenderSkeleton
	{
		[Flags]
		public enum AnsiAttributes
		{
			Bright = 1,
			Dim = 2,
			Underscore = 4,
			Blink = 8,
			Reverse = 0x10,
			Hidden = 0x20,
			Strikethrough = 0x40
		}

		public enum AnsiColor
		{
			Black,
			Red,
			Green,
			Yellow,
			Blue,
			Magenta,
			Cyan,
			White
		}

		public class LevelColors : LevelMappingEntry
		{
			private AnsiColor m_foreColor;

			private AnsiColor m_backColor;

			private AnsiAttributes m_attributes;

			private string m_combinedColor = "";

			public AnsiColor ForeColor
			{
				get
				{
					return m_foreColor;
				}
				set
				{
					m_foreColor = value;
				}
			}

			public AnsiColor BackColor
			{
				get
				{
					return m_backColor;
				}
				set
				{
					m_backColor = value;
				}
			}

			public AnsiAttributes Attributes
			{
				get
				{
					return m_attributes;
				}
				set
				{
					m_attributes = value;
				}
			}

			internal string CombinedColor
			{
				get
				{
					return m_combinedColor;
				}
			}

			public override void ActivateOptions()
			{
				base.ActivateOptions();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("\u001b[0;");
				stringBuilder.Append((int)(30 + m_foreColor));
				stringBuilder.Append(';');
				stringBuilder.Append((int)(40 + m_backColor));
				if ((m_attributes & AnsiAttributes.Bright) > (AnsiAttributes)0)
				{
					stringBuilder.Append(";1");
				}
				if ((m_attributes & AnsiAttributes.Dim) > (AnsiAttributes)0)
				{
					stringBuilder.Append(";2");
				}
				if ((m_attributes & AnsiAttributes.Underscore) > (AnsiAttributes)0)
				{
					stringBuilder.Append(";4");
				}
				if ((m_attributes & AnsiAttributes.Blink) > (AnsiAttributes)0)
				{
					stringBuilder.Append(";5");
				}
				if ((m_attributes & AnsiAttributes.Reverse) > (AnsiAttributes)0)
				{
					stringBuilder.Append(";7");
				}
				if ((m_attributes & AnsiAttributes.Hidden) > (AnsiAttributes)0)
				{
					stringBuilder.Append(";8");
				}
				if ((m_attributes & AnsiAttributes.Strikethrough) > (AnsiAttributes)0)
				{
					stringBuilder.Append(";9");
				}
				stringBuilder.Append('m');
				m_combinedColor = stringBuilder.ToString();
			}
		}

		public const string ConsoleOut = "Console.Out";

		public const string ConsoleError = "Console.Error";

		private bool m_writeToErrorStream;

		private LevelMapping m_levelMapping = new LevelMapping();

		private const string PostEventCodes = "\u001b[0m";

		public virtual string Target
		{
			get
			{
				if (!m_writeToErrorStream)
				{
					return "Console.Out";
				}
				return "Console.Error";
			}
			set
			{
				string strB = value.Trim();
				if (string.Compare("Console.Error", strB, true, CultureInfo.InvariantCulture) == 0)
				{
					m_writeToErrorStream = true;
				}
				else
				{
					m_writeToErrorStream = false;
				}
			}
		}

		protected override bool RequiresLayout
		{
			get
			{
				return true;
			}
		}

		public void AddMapping(LevelColors mapping)
		{
			m_levelMapping.Add(mapping);
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			string text = RenderLoggingEvent(loggingEvent);
			LevelColors levelColors = m_levelMapping.Lookup(loggingEvent.Level) as LevelColors;
			if (levelColors != null)
			{
				text = levelColors.CombinedColor + text;
			}
			text = ((text.Length > 1) ? ((!text.EndsWith("\r\n") && !text.EndsWith("\n\r")) ? ((!text.EndsWith("\n") && !text.EndsWith("\r")) ? (text + "\u001b[0m") : text.Insert(text.Length - 1, "\u001b[0m")) : text.Insert(text.Length - 2, "\u001b[0m")) : ((text[0] != '\n' && text[0] != '\r') ? (text + "\u001b[0m") : ("\u001b[0m" + text)));
			if (m_writeToErrorStream)
			{
				Console.Error.Write(text);
			}
			else
			{
				Console.Write(text);
			}
		}

		public override void ActivateOptions()
		{
			base.ActivateOptions();
			m_levelMapping.ActivateOptions();
		}
	}
}
