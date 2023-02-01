using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H
{
    internal class MechanismFlags
	{
		private Tr.Com.Eimza.Pkcs11.H4.MechanismFlags _mechanismFlags4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismFlags _mechanismFlags8;

		public ulong Flags
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.Flags;
				}
				return _mechanismFlags4.Flags;
			}
		}

		public bool Hw
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.Hw;
				}
				return _mechanismFlags4.Hw;
			}
		}

		public bool Encrypt
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.Encrypt;
				}
				return _mechanismFlags4.Encrypt;
			}
		}

		public bool Decrypt
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.Decrypt;
				}
				return _mechanismFlags4.Decrypt;
			}
		}

		public bool Digest
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.Digest;
				}
				return _mechanismFlags4.Digest;
			}
		}

		public bool Sign
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.Sign;
				}
				return _mechanismFlags4.Sign;
			}
		}

		public bool SignRecover
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.SignRecover;
				}
				return _mechanismFlags4.SignRecover;
			}
		}

		public bool Verify
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.Verify;
				}
				return _mechanismFlags4.Verify;
			}
		}

		public bool VerifyRecover
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.VerifyRecover;
				}
				return _mechanismFlags4.VerifyRecover;
			}
		}

		public bool Generate
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.Generate;
				}
				return _mechanismFlags4.Generate;
			}
		}

		public bool GenerateKeyPair
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.GenerateKeyPair;
				}
				return _mechanismFlags4.GenerateKeyPair;
			}
		}

		public bool Wrap
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.Wrap;
				}
				return _mechanismFlags4.Wrap;
			}
		}

		public bool Unwrap
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.Unwrap;
				}
				return _mechanismFlags4.Unwrap;
			}
		}

		public bool Derive
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.Derive;
				}
				return _mechanismFlags4.Derive;
			}
		}

		public bool Extension
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.Extension;
				}
				return _mechanismFlags4.Extension;
			}
		}

		public bool EcFp
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.EcFp;
				}
				return _mechanismFlags4.EcFp;
			}
		}

		public bool EcF2m
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.EcF2m;
				}
				return _mechanismFlags4.EcF2m;
			}
		}

		public bool EcEcParameters
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.EcEcParameters;
				}
				return _mechanismFlags4.EcEcParameters;
			}
		}

		public bool EcNamedCurve
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.EcNamedCurve;
				}
				return _mechanismFlags4.EcNamedCurve;
			}
		}

		public bool EcUncompress
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.EcUncompress;
				}
				return _mechanismFlags4.EcUncompress;
			}
		}

		public bool EcCompress
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismFlags8.EcCompress;
				}
				return _mechanismFlags4.EcCompress;
			}
		}

		internal MechanismFlags(Tr.Com.Eimza.Pkcs11.H4.MechanismFlags mechanismFlags)
		{
			if (mechanismFlags == null)
			{
				throw new ArgumentNullException("mechanismFlags");
			}
			_mechanismFlags4 = mechanismFlags;
		}

		internal MechanismFlags(Tr.Com.Eimza.Pkcs11.H8.MechanismFlags mechanismFlags)
		{
			if (mechanismFlags == null)
			{
				throw new ArgumentNullException("mechanismFlags");
			}
			_mechanismFlags8 = mechanismFlags;
		}
	}
}
