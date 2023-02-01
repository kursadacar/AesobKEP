using System;

namespace Tr.Com.Eimza.Pkcs11.C
{
	internal static class UnmanagedLong
	{
		private static int _size;

		public static int Size
		{
			get
			{
				if (_size != 0)
				{
					return _size;
				}
				PlatformID platform = Environment.OSVersion.Platform;
				if (platform == PlatformID.Unix || platform == PlatformID.MacOSX)
				{
					_size = IntPtr.Size;
				}
				else
				{
					_size = 4;
				}
				return _size;
			}
			set
			{
				if (value != 4 && value != 8)
				{
					throw new ArgumentException();
				}
				_size = value;
			}
		}
	}
}
