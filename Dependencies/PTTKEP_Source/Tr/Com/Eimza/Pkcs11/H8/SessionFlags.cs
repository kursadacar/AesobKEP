namespace Tr.Com.Eimza.Pkcs11.H8
{
	internal class SessionFlags
	{
		private ulong _flags;

		public ulong Flags
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

		internal SessionFlags(ulong flags)
		{
			_flags = flags;
		}
	}
}
