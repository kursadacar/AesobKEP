using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.C
{
	internal static class UnmanagedMemory
	{
		public static IntPtr Allocate(int size)
		{
			if (size < 0)
			{
				throw new ArgumentException("Value has to be positive number", "size");
			}
			IntPtr intPtr = Marshal.AllocHGlobal(size);
			Write(intPtr, new byte[size]);
			return intPtr;
		}

		public static void Free(ref IntPtr memory)
		{
			if (memory != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(memory);
				memory = IntPtr.Zero;
			}
		}

		public static int SizeOf(Type structureType)
		{
			if ((object)structureType == null)
			{
				throw new ArgumentNullException("structureType");
			}
			return Marshal.SizeOf(structureType);
		}

		public static void Write(IntPtr memory, byte[] content)
		{
			if (memory == IntPtr.Zero)
			{
				throw new ArgumentNullException("memory");
			}
			if (content == null)
			{
				throw new ArgumentNullException("content");
			}
			Marshal.Copy(content, 0, memory, content.Length);
		}

		public static void Write(IntPtr memory, object structure)
		{
			if (memory == IntPtr.Zero)
			{
				throw new ArgumentNullException("memory");
			}
			if (structure == null)
			{
				throw new ArgumentNullException("structure");
			}
			Marshal.StructureToPtr(structure, memory, false);
		}

		public static byte[] Read(IntPtr memory, int size)
		{
			if (memory == IntPtr.Zero)
			{
				throw new ArgumentNullException("memory");
			}
			if (size < 0)
			{
				throw new ArgumentException("Value has to be positive number", "size");
			}
			byte[] array = new byte[size];
			Marshal.Copy(memory, array, 0, size);
			return array;
		}

		public static object Read(IntPtr memory, Type structureType)
		{
			if (memory == IntPtr.Zero)
			{
				throw new ArgumentNullException("memory");
			}
			if ((object)structureType == null)
			{
				throw new ArgumentNullException("structureType");
			}
			return Marshal.PtrToStructure(memory, structureType);
		}

		public static void Read(IntPtr memory, object structure)
		{
			if (memory == IntPtr.Zero)
			{
				throw new ArgumentNullException("memory");
			}
			if (structure == null)
			{
				throw new ArgumentNullException("structure");
			}
			Marshal.PtrToStructure(memory, structure);
		}
	}
}
