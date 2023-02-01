namespace Tr.Com.Eimza.Pkcs11.H4
{
	internal class SessionFlags
	{
		private uint _flags;

		public uint Flags
		{
			get
			{
				return _flags;
			}
		}

		public bool RwSession
		{
			get
			{
				return (_flags & 2) == 2;
			}
		}

		public bool SerialSession
		{
			get
			{
				return (_flags & 4) == 4;
			}
		}

		internal SessionFlags(uint flags)
		{
			_flags = flags;
		}
	}
}
