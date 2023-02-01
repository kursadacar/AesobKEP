using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Layout;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public class ColoredConsoleAppender : AppenderSkeleton
	{
		[Flags]
		public enum Colors
		{
			Blue = 1,
			Green = 2,
			Red = 4,
			White = 7,
			Yellow = 6,
			Purple = 5,
			Cyan = 3,
			HighIntensity = 8
		}

		private struct COORD
		{
			public ushort x;

			public ushort y;
		}

		private struct SMALL_RECT
		{
			public ushort Left;

			public ushort Top;

			public ushort Right;

			public ushort Bottom;
		}

		private struct CONSOLE_SCREEN_BUFFER_INFO
		{
			public COORD dwSize;

			public COORD dwCursorPosition;

			public ushort wAttributes;

			public SMALL_RECT srWindow;

			public COORD dwMaximumWindowSize;
		}

		public class LevelColors : LevelMappingEntry
		{
			private Colors m_foreColor;

			private Colors m_backColor;

			private ushort m_combinedColor;

			public Colors ForeColor
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

			public Colors BackColor
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

			internal ushort CombinedColor
			{
				get
				{
					return m_combinedColor;
				}
			}

			public override void ActivateOptions()
			{
				base.ActivateOptions();
				m_combinedColor = (ushort)(m_foreColor + ((int)m_backColor << 4));
			}
		}

		private static readonly char[] s_windowsNewline = new char[2] { '\r', '\n' };

		public const string ConsoleOut = "Console.Out";

		public const string ConsoleError = "Console.Error";

		private bool m_writeToErrorStream;

		private LevelMapping m_levelMapping = new LevelMapping();

		private StreamWriter m_consoleOutputWriter;

		private const uint STD_OUTPUT_HANDLE = 4294967285u;

		private const uint STD_ERROR_HANDLE = 4294967284u;

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

		public ColoredConsoleAppender()
		{
		}

		[Obsolete("Instead use the default constructor and set the Layout property")]
		public ColoredConsoleAppender(ILayout layout)
			: this(layout, false)
		{
		}

		[Obsolete("Instead use the default constructor and set the Layout & Target properties")]
		public ColoredConsoleAppender(ILayout layout, bool writeToErrorStream)
		{
			Layout = layout;
			m_writeToErrorStream = writeToErrorStream;
		}

		public void AddMapping(LevelColors mapping)
		{
			m_levelMapping.Add(mapping);
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			if (m_consoleOutputWriter != null)
			{
				IntPtr zero = IntPtr.Zero;
				zero = ((!m_writeToErrorStream) ? GetStdHandle(4294967285u) : GetStdHandle(4294967284u));
				ushort attributes = 7;
				LevelColors levelColors = m_levelMapping.Lookup(loggingEvent.Level) as LevelColors;
				if (levelColors != null)
				{
					attributes = levelColors.CombinedColor;
				}
				string text = RenderLoggingEvent(loggingEvent);
				CONSOLE_SCREEN_BUFFER_INFO bufferInfo;
				GetConsoleScreenBufferInfo(zero, out bufferInfo);
				SetConsoleTextAttribute(zero, attributes);
				char[] array = text.ToCharArray();
				int num = array.Length;
				bool flag = false;
				if (num > 1 && array[num - 2] == '\r' && array[num - 1] == '\n')
				{
					num -= 2;
					flag = true;
				}
				m_consoleOutputWriter.Write(array, 0, num);
				SetConsoleTextAttribute(zero, bufferInfo.wAttributes);
				if (flag)
				{
					m_consoleOutputWriter.Write(s_windowsNewline, 0, 2);
				}
			}
		}

		public override void ActivateOptions()
		{
			base.ActivateOptions();
			m_levelMapping.ActivateOptions();
			Stream stream = null;
			stream = ((!m_writeToErrorStream) ? Console.OpenStandardOutput() : Console.OpenStandardError());
			Encoding encoding = Encoding.GetEncoding(GetConsoleOutputCP());
			m_consoleOutputWriter = new StreamWriter(stream, encoding, 256);
			m_consoleOutputWriter.AutoFlush = true;
			GC.SuppressFinalize(m_consoleOutputWriter);
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetConsoleOutputCP();

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool SetConsoleTextAttribute(IntPtr consoleHandle, ushort attributes);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool GetConsoleScreenBufferInfo(IntPtr consoleHandle, out CONSOLE_SCREEN_BUFFER_INFO bufferInfo);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetStdHandle(uint type);
	}
}
