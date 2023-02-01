using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H
{
    internal class TokenFlags
	{
		private Tr.Com.Eimza.Pkcs11.H4.TokenFlags _tokenFlags4;

		private Tr.Com.Eimza.Pkcs11.H8.TokenFlags _tokenFlags8;

		public ulong Flags
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.Flags;
				}
				return _tokenFlags4.Flags;
			}
		}

		public bool Rng
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.Rng;
				}
				return _tokenFlags4.Rng;
			}
		}

		public bool WriteProtected
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.WriteProtected;
				}
				return _tokenFlags4.WriteProtected;
			}
		}

		public bool LoginRequired
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.LoginRequired;
				}
				return _tokenFlags4.LoginRequired;
			}
		}

		public bool UserPinInitialized
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.UserPinInitialized;
				}
				return _tokenFlags4.UserPinInitialized;
			}
		}

		public bool RestoreKeyNotNeeded
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.RestoreKeyNotNeeded;
				}
				return _tokenFlags4.RestoreKeyNotNeeded;
			}
		}

		public bool ClockOnToken
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.ClockOnToken;
				}
				return _tokenFlags4.ClockOnToken;
			}
		}

		public bool ProtectedAuthenticationPath
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.ProtectedAuthenticationPath;
				}
				return _tokenFlags4.ProtectedAuthenticationPath;
			}
		}

		public bool DualCryptoOperations
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.DualCryptoOperations;
				}
				return _tokenFlags4.DualCryptoOperations;
			}
		}

		public bool TokenInitialized
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.TokenInitialized;
				}
				return _tokenFlags4.TokenInitialized;
			}
		}

		public bool SecondaryAuthentication
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.SecondaryAuthentication;
				}
				return _tokenFlags4.SecondaryAuthentication;
			}
		}

		public bool UserPinCountLow
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.UserPinCountLow;
				}
				return _tokenFlags4.UserPinCountLow;
			}
		}

		public bool UserPinFinalTry
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.UserPinFinalTry;
				}
				return _tokenFlags4.UserPinFinalTry;
			}
		}

		public bool UserPinLocked
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.UserPinLocked;
				}
				return _tokenFlags4.UserPinLocked;
			}
		}

		public bool UserPinToBeChanged
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.UserPinToBeChanged;
				}
				return _tokenFlags4.UserPinToBeChanged;
			}
		}

		public bool SoPinCountLow
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.SoPinCountLow;
				}
				return _tokenFlags4.SoPinCountLow;
			}
		}

		public bool SoPinFinalTry
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.SoPinFinalTry;
				}
				return _tokenFlags4.SoPinFinalTry;
			}
		}

		public bool SoPinLocked
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.SoPinLocked;
				}
				return _tokenFlags4.SoPinLocked;
			}
		}

		public bool SoPinToBeChanged
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenFlags8.SoPinToBeChanged;
				}
				return _tokenFlags4.SoPinToBeChanged;
			}
		}

		internal TokenFlags(Tr.Com.Eimza.Pkcs11.H4.TokenFlags tokenFlags)
		{
			if (tokenFlags == null)
			{
				throw new ArgumentNullException("tokenFlags");
			}
			_tokenFlags4 = tokenFlags;
		}

		internal TokenFlags(Tr.Com.Eimza.Pkcs11.H8.TokenFlags tokenFlags)
		{
			if (tokenFlags == null)
			{
				throw new ArgumentNullException("tokenFlags");
			}
			_tokenFlags8 = tokenFlags;
		}
	}
}
