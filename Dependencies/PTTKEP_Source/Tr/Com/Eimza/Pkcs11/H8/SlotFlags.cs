namespace Tr.Com.Eimza.Pkcs11.H8
{
	internal class SlotFlags
	{
		private ulong _flags;

		public ulong Flags
		{
			get
			{
				return _flags;
			}
		}

		public bool TokenPresent
		{
			get
			{
				return (_flags & 1) == 1;
			}
		}

		public bool RemovableDevice
		{
			get
			{
				return (_flags & 2) == 2;
			}
		}

		public bool HardwareSlot
		{
			get
			{
				return (_flags & 4) == 4;
			}
		}

		internal SlotFlags(ulong flags)
		{
			_flags = flags;
		}
	}
}
