namespace Tr.Com.Eimza.Pkcs11.H8
{
	internal class MechanismFlags
	{
		private ulong _flags;

		public ulong Flags
		{
			get
			{
				return _flags;
			}
		}

		public bool Hw
		{
			get
			{
				return (_flags & 1) == 1;
			}
		}

		public bool Encrypt
		{
			get
			{
				return (_flags & 0x100) == 256;
			}
		}

		public bool Decrypt
		{
			get
			{
				return (_flags & 0x200) == 512;
			}
		}

		public bool Digest
		{
			get
			{
				return (_flags & 0x400) == 1024;
			}
		}

		public bool Sign
		{
			get
			{
				return (_flags & 0x800) == 2048;
			}
		}

		public bool SignRecover
		{
			get
			{
				return (_flags & 0x1000) == 4096;
			}
		}

		public bool Verify
		{
			get
			{
				return (_flags & 0x2000) == 8192;
			}
		}

		public bool VerifyRecover
		{
			get
			{
				return (_flags & 0x4000) == 16384;
			}
		}

		public bool Generate
		{
			get
			{
				return (_flags & 0x8000) == 32768;
			}
		}

		public bool GenerateKeyPair
		{
			get
			{
				return (_flags & 0x10000) == 65536;
			}
		}

		public bool Wrap
		{
			get
			{
				return (_flags & 0x20000) == 131072;
			}
		}

		public bool Unwrap
		{
			get
			{
				return (_flags & 0x40000) == 262144;
			}
		}

		public bool Derive
		{
			get
			{
				return (_flags & 0x80000) == 524288;
			}
		}

		public bool Extension
		{
			get
			{
				return (_flags & 0x80000000u) == 2147483648u;
			}
		}

		public bool EcFp
		{
			get
			{
				return (_flags & 0x100000) == 1048576;
			}
		}

		public bool EcF2m
		{
			get
			{
				return (_flags & 0x200000) == 2097152;
			}
		}

		public bool EcEcParameters
		{
			get
			{
				return (_flags & 0x400000) == 4194304;
			}
		}

		public bool EcNamedCurve
		{
			get
			{
				return (_flags & 0x800000) == 8388608;
			}
		}

		public bool EcUncompress
		{
			get
			{
				return (_flags & 0x1000000) == 16777216;
			}
		}

		public bool EcCompress
		{
			get
			{
				return (_flags & 0x2000000) == 33554432;
			}
		}

		internal MechanismFlags(ulong flags)
		{
			_flags = flags;
		}
	}
}
