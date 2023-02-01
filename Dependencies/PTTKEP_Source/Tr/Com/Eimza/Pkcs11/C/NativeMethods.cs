using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.C
{
	internal static class NativeMethods
	{
		internal const int RTLD_LAZY = 1;

		internal const int RTLD_NOW = 2;

		[DllImport("kernel32", BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true)]
		internal static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

		[DllImport("kernel32", SetLastError = true)]
		internal static extern bool FreeLibrary(IntPtr hModule);

		[DllImport("kernel32", BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true)]
		internal static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

		[DllImport("libdl", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr dlerror();

		[DllImport("libdl", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true)]
		internal static extern IntPtr dlopen(string filename, int flag);

		[DllImport("libdl", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int dlclose(IntPtr handle);

		[DllImport("libdl", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true)]
		internal static extern IntPtr dlsym(IntPtr handle, string symbol);
	}
}
