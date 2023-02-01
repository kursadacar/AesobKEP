using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Tr.Com.Eimza.Org.BouncyCastle.Tools
{
	internal static class NativeMethods
	{
		public static string SystemDirectory
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				SHGetSpecialFolderPath(IntPtr.Zero, stringBuilder, Is64Bit ? 41 : 37, false);
				return stringBuilder.ToString();
			}
		}

		public static bool Is64Bit
		{
			get
			{
				bool isWow;
				IsWow64Process(Process.GetCurrentProcess().Handle, out isWow);
				return isWow;
			}
		}

		[DllImport("shell32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner, [Out] StringBuilder lpszPath, int nFolder, bool fCreate);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWow64Process(IntPtr hProcess, [MarshalAs(UnmanagedType.Bool)] out bool isWow64);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetCurrentProcess();

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandle(string moduleName);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string methodName);
	}
}
