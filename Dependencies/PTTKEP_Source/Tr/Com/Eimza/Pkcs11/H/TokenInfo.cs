using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H
{
    internal class TokenInfo
	{
		private Tr.Com.Eimza.Pkcs11.H4.TokenInfo _tokenInfo4;

		private Tr.Com.Eimza.Pkcs11.H8.TokenInfo _tokenInfo8;

		private TokenFlags _tokenFlags;

		public ulong SlotId
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.SlotId;
				}
				return _tokenInfo4.SlotId;
			}
		}

		public string Label
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.Label;
				}
				return _tokenInfo4.Label;
			}
		}

		public string ManufacturerId
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.ManufacturerId;
				}
				return _tokenInfo4.ManufacturerId;
			}
		}

		public string Model
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.Model;
				}
				return _tokenInfo4.Model;
			}
		}

		public string SerialNumber
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.SerialNumber;
				}
				return _tokenInfo4.SerialNumber;
			}
		}

		public TokenFlags TokenFlags
		{
			get
			{
				if (_tokenFlags == null)
				{
					_tokenFlags = ((UnmanagedLong.Size == 4) ? new TokenFlags(_tokenInfo4.TokenFlags) : new TokenFlags(_tokenInfo8.TokenFlags));
				}
				return _tokenFlags;
			}
		}

		public ulong MaxSessionCount
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.MaxSessionCount;
				}
				return _tokenInfo4.MaxSessionCount;
			}
		}

		public ulong SessionCount
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.SessionCount;
				}
				return _tokenInfo4.SessionCount;
			}
		}

		public ulong MaxRwSessionCount
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.MaxRwSessionCount;
				}
				return _tokenInfo4.MaxRwSessionCount;
			}
		}

		public ulong RwSessionCount
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.RwSessionCount;
				}
				return _tokenInfo4.RwSessionCount;
			}
		}

		public ulong MaxPinLen
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.MaxPinLen;
				}
				return _tokenInfo4.MaxPinLen;
			}
		}

		public ulong MinPinLen
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.MinPinLen;
				}
				return _tokenInfo4.MinPinLen;
			}
		}

		public ulong TotalPublicMemory
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.TotalPublicMemory;
				}
				return _tokenInfo4.TotalPublicMemory;
			}
		}

		public ulong FreePublicMemory
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.FreePublicMemory;
				}
				return _tokenInfo4.FreePublicMemory;
			}
		}

		public ulong TotalPrivateMemory
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.TotalPrivateMemory;
				}
				return _tokenInfo4.TotalPrivateMemory;
			}
		}

		public ulong FreePrivateMemory
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.FreePrivateMemory;
				}
				return _tokenInfo4.FreePrivateMemory;
			}
		}

		public string HardwareVersion
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.HardwareVersion;
				}
				return _tokenInfo4.HardwareVersion;
			}
		}

		public string FirmwareVersion
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.FirmwareVersion;
				}
				return _tokenInfo4.FirmwareVersion;
			}
		}

		public string UtcTimeString
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.UtcTimeString;
				}
				return _tokenInfo4.UtcTimeString;
			}
		}

		public DateTime? UtcTime
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _tokenInfo8.UtcTime;
				}
				return _tokenInfo4.UtcTime;
			}
		}

		internal TokenInfo(Tr.Com.Eimza.Pkcs11.H4.TokenInfo tokenInfo)
		{
			if (tokenInfo == null)
			{
				throw new ArgumentNullException("tokenInfo");
			}
			_tokenInfo4 = tokenInfo;
		}

		internal TokenInfo(Tr.Com.Eimza.Pkcs11.H8.TokenInfo tokenInfo)
		{
			if (tokenInfo == null)
			{
				throw new ArgumentNullException("tokenInfo");
			}
			_tokenInfo8 = tokenInfo;
		}
	}
}
