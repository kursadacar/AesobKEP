using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Tr.Com.Eimza.EYazisma.Utilities
{
	internal static class MemoryUtilities
	{
		[Flags]
		private enum ProcessAccessFlags : uint
		{
			All = 0x1F0FFFu,
			Terminate = 1u,
			CreateThread = 2u,
			VMOperation = 8u,
			VMRead = 0x10u,
			VMWrite = 0x20u,
			DupHandle = 0x40u,
			SetInformation = 0x200u,
			QueryInformation = 0x400u,
			Synchronize = 0x100000u
		}

		private static Thread th;

		[DllImport("psapi")]
		private static extern bool EnumProcesses([In][Out][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)] IntPtr[] processIds, uint arraySizeBytes, [MarshalAs(UnmanagedType.U4)] out uint bytesCopied);

		[DllImport("kernel32.dll")]
		private static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, IntPtr dwProcessId);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CloseHandle(IntPtr hObject);

		[DllImport("psapi.dll")]
		private static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In][MarshalAs(UnmanagedType.U4)] int nSize);

		[DllImport("psapi.dll", SetLastError = true)]
		public static extern bool EnumProcessModules(IntPtr hProcess, [Out] IntPtr lphModule, uint cb, [MarshalAs(UnmanagedType.U4)] out uint lpcbNeeded);

		[DllImport("psapi.dll")]
		private static extern uint GetModuleBaseName(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In][MarshalAs(UnmanagedType.U4)] int nSize);

		[DllImport("psapi")]
		private static extern bool EmptyWorkingSet(IntPtr hProcess);

		public static void MemoryBooster()
		{
			th = new Thread(Booster)
			{
				Priority = ThreadPriority.Lowest
			};
			th.Start();
		}

		internal static void Booster()
		{
			while (true)
			{
				Process[] processes = Process.GetProcesses();
				for (int i = 0; i < processes.Length; i++)
				{
					TrimWSByProcess(processes[i]);
				}
				Thread.Sleep(1000);
			}
		}

		private static bool TrimWSByProcess(Process process)
		{
			bool result = false;
			try
			{
				if (process != null)
				{
					long workingSet = process.WorkingSet64;
					return EmptyWorkingSet(process.Handle);
				}
				return result;
			}
			catch (Exception)
			{
				return false;
			}
		}

		internal static void Dispose()
		{
			th.Abort();
		}
	}
}
