using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.C
{
	internal static class UnmanagedLibrary
	{
		internal static IntPtr Load(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			IntPtr zero = IntPtr.Zero;
			PlatformID platform = Environment.OSVersion.Platform;
			if (platform == PlatformID.Unix || platform == PlatformID.MacOSX)
			{
				zero = NativeMethods.dlopen(fileName, 2);
				if (zero == IntPtr.Zero)
				{
					IntPtr intPtr = NativeMethods.dlerror();
					if (intPtr != IntPtr.Zero)
					{
						throw new Exception(string.Format("Unable to load library: {0}", Marshal.PtrToStringAnsi(intPtr)));
					}
					throw new Exception("Unable to load library");
				}
			}
			else
			{
				zero = NativeMethods.LoadLibrary(fileName);
				if (zero == IntPtr.Zero)
				{
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}
			}
			return zero;
		}

		internal static void Unload(IntPtr libraryHandle)
		{
			if (libraryHandle == IntPtr.Zero)
			{
				throw new ArgumentNullException("libraryHandle");
			}
			PlatformID platform = Environment.OSVersion.Platform;
			if (platform == PlatformID.Unix || platform == PlatformID.MacOSX)
			{
				if (NativeMethods.dlclose(libraryHandle) != 0)
				{
					IntPtr intPtr = NativeMethods.dlerror();
					if (intPtr != IntPtr.Zero)
					{
						throw new Exception(string.Format("Unable to unload library: {0}", Marshal.PtrToStringAnsi(intPtr)));
					}
					throw new Exception("Unable to unload library");
				}
			}
			else if (!NativeMethods.FreeLibrary(libraryHandle))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}

		internal static IntPtr GetFunctionPointer(IntPtr libraryHandle, string function)
		{
			if (libraryHandle == IntPtr.Zero)
			{
				throw new ArgumentNullException("libraryHandle");
			}
			if (function == null)
			{
				throw new ArgumentNullException("function");
			}
			IntPtr zero = IntPtr.Zero;
			PlatformID platform = Environment.OSVersion.Platform;
			if (platform == PlatformID.Unix || platform == PlatformID.MacOSX)
			{
				NativeMethods.dlerror();
				zero = NativeMethods.dlsym(libraryHandle, function);
				IntPtr intPtr = NativeMethods.dlerror();
				if (intPtr != IntPtr.Zero)
				{
					throw new Exception(string.Format("Unable to get function pointer: {0}", Marshal.PtrToStringAnsi(intPtr)));
				}
			}
			else
			{
				zero = NativeMethods.GetProcAddress(libraryHandle, function);
				if (zero == IntPtr.Zero)
				{
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}
			}
			return zero;
		}
	}
}
