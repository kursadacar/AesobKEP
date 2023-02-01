using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Log4Net.Util
{
	public sealed class NativeError
	{
		private int m_number;

		private string m_message;

		public int Number
		{
			get
			{
				return m_number;
			}
		}

		public string Message
		{
			get
			{
				return m_message;
			}
		}

		private NativeError(int number, string message)
		{
			m_number = number;
			m_message = message;
		}

		public static NativeError GetLastError()
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			return new NativeError(lastWin32Error, GetErrorMessage(lastWin32Error));
		}

		public static NativeError GetError(int number)
		{
			return new NativeError(number, GetErrorMessage(number));
		}

		public static string GetErrorMessage(int messageId)
		{
			int num = 256;
			int num2 = 512;
			int num3 = 4096;
			string lpBuffer = "";
			IntPtr lpSource = default(IntPtr);
			IntPtr arguments = default(IntPtr);
			if (messageId != 0)
			{
				if (FormatMessage(num | num3 | num2, ref lpSource, messageId, 0, ref lpBuffer, 255, arguments) > 0)
				{
					return lpBuffer.TrimEnd('\r', '\n');
				}
				return null;
			}
			return null;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "0x{0:x8}", new object[1] { Number }) + ((Message != null) ? (": " + Message) : "");
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int FormatMessage(int dwFlags, ref IntPtr lpSource, int dwMessageId, int dwLanguageId, ref string lpBuffer, int nSize, IntPtr Arguments);
	}
}
