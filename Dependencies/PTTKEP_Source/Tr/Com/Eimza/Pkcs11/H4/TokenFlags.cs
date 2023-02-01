namespace Tr.Com.Eimza.Pkcs11.H4
{
	internal class TokenFlags
	{
		private uint _flags;

		public uint Flags
		{
			get
			{
				return _flags;
			}
		}

		public bool Rng
		{
			get
			{
				return (_flags & 1) == 1;
			}
		}

		public bool WriteProtected
		{
			get
			{
				return (_flags & 2) == 2;
			}
		}

		public bool LoginRequired
		{
			get
			{
				return (_flags & 4) == 4;
			}
		}

		public bool UserPinInitialized
		{
			get
			{
				return (_flags & 8) == 8;
			}
		}

		public bool RestoreKeyNotNeeded
		{
			get
			{
				return (_flags & 0x20) == 32;
			}
		}

		public bool ClockOnToken
		{
			get
			{
				return (_flags & 0x40) == 64;
			}
		}

		public bool ProtectedAuthenticationPath
		{
			get
			{
				return (_flags & 0x100) == 256;
			}
		}

		public bool DualCryptoOperations
		{
			get
			{
				return (_flags & 0x200) == 512;
			}
		}

		public bool TokenInitialized
		{
			get
			{
				return (_flags & 0x400) == 1024;
			}
		}

		public bool SecondaryAuthentication
		{
			get
			{
				return (_flags & 0x800) == 2048;
			}
		}

		public bool UserPinCountLow
		{
			get
			{
				return (_flags & 0x10000) == 65536;
			}
		}

		public bool UserPinFinalTry
		{
			get
			{
				return (_flags & 0x20000) == 131072;
			}
		}

		public bool UserPinLocked
		{
			get
			{
				return (_flags & 0x40000) == 262144;
			}
		}

		public bool UserPinToBeChanged
		{
			get
			{
				return (_flags & 0x80000) == 524288;
			}
		}

		public bool SoPinCountLow
		{
			get
			{
				return (_flags & 0x100000) == 1048576;
			}
		}

		public bool SoPinFinalTry
		{
			get
			{
				return (_flags & 0x200000) == 2097152;
			}
		}

		public bool SoPinLocked
		{
			get
			{
				return (_flags & 0x400000) == 4194304;
			}
		}

		public bool SoPinToBeChanged
		{
			get
			{
				return (_flags & 0x800000) == 8388608;
			}
		}

		internal TokenFlags(uint flags)
		{
			_flags = flags;
		}
	}
}
