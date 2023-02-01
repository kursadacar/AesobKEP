namespace Tr.Com.Eimza.Pkcs11.H4
{
	internal class SlotFlags
	{
		private uint _flags;

		public uint Flags
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

		internal SlotFlags(uint flags)
		{
			_flags = flags;
		}
	}
}
