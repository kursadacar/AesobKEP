using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4;

namespace Tr.Com.Eimza.Pkcs11.H4
{
	internal class TokenInfo
	{
		private uint _slotId;

		private string _label;

		private string _manufacturerId;

		private string _model;

		private string _serialNumber;

		private TokenFlags _tokenFlags;

		private uint _maxSessionCount;

		private uint _sessionCount;

		private uint _maxRwSessionCount;

		private uint _rwSessionCount;

		private uint _maxPinLen;

		private uint _minPinLen;

		private uint _totalPublicMemory;

		private uint _freePublicMemory;

		private uint _totalPrivateMemory;

		private uint _freePrivateMemory;

		private string _hardwareVersion;

		private string _firmwareVersion;

		private string _utcTimeString;

		private DateTime? _utcTime;

		public uint SlotId
		{
			get
			{
				return _slotId;
			}
		}

		public string Label
		{
			get
			{
				return _label;
			}
		}

		public string ManufacturerId
		{
			get
			{
				return _manufacturerId;
			}
		}

		public string Model
		{
			get
			{
				return _model;
			}
		}

		public string SerialNumber
		{
			get
			{
				return _serialNumber;
			}
		}

		public TokenFlags TokenFlags
		{
			get
			{
				return _tokenFlags;
			}
		}

		public uint MaxSessionCount
		{
			get
			{
				return _maxSessionCount;
			}
		}

		public uint SessionCount
		{
			get
			{
				return _sessionCount;
			}
		}

		public uint MaxRwSessionCount
		{
			get
			{
				return _maxRwSessionCount;
			}
		}

		public uint RwSessionCount
		{
			get
			{
				return _rwSessionCount;
			}
		}

		public uint MaxPinLen
		{
			get
			{
				return _maxPinLen;
			}
		}

		public uint MinPinLen
		{
			get
			{
				return _minPinLen;
			}
		}

		public uint TotalPublicMemory
		{
			get
			{
				return _totalPublicMemory;
			}
		}

		public uint FreePublicMemory
		{
			get
			{
				return _freePublicMemory;
			}
		}

		public uint TotalPrivateMemory
		{
			get
			{
				return _totalPrivateMemory;
			}
		}

		public uint FreePrivateMemory
		{
			get
			{
				return _freePrivateMemory;
			}
		}

		public string HardwareVersion
		{
			get
			{
				return _hardwareVersion;
			}
		}

		public string FirmwareVersion
		{
			get
			{
				return _firmwareVersion;
			}
		}

		public string UtcTimeString
		{
			get
			{
				return _utcTimeString;
			}
		}

		public DateTime? UtcTime
		{
			get
			{
				return _utcTime;
			}
		}

		internal TokenInfo(uint slotId, CK_TOKEN_INFO ck_token_info)
		{
			_slotId = slotId;
			_label = ConvertUtils.BytesToUtf8String(ck_token_info.Label, true);
			_manufacturerId = ConvertUtils.BytesToUtf8String(ck_token_info.ManufacturerId, true);
			_model = ConvertUtils.BytesToUtf8String(ck_token_info.Model, true);
			_serialNumber = ConvertUtils.BytesToUtf8String(ck_token_info.SerialNumber, true);
			_tokenFlags = new TokenFlags(ck_token_info.Flags);
			_maxSessionCount = ck_token_info.MaxSessionCount;
			_sessionCount = ck_token_info.SessionCount;
			_maxRwSessionCount = ck_token_info.MaxRwSessionCount;
			_rwSessionCount = ck_token_info.RwSessionCount;
			_maxPinLen = ck_token_info.MaxPinLen;
			_minPinLen = ck_token_info.MinPinLen;
			_totalPublicMemory = ck_token_info.TotalPublicMemory;
			_freePublicMemory = ck_token_info.FreePublicMemory;
			_totalPrivateMemory = ck_token_info.TotalPrivateMemory;
			_freePrivateMemory = ck_token_info.FreePrivateMemory;
			_hardwareVersion = ConvertUtils.CkVersionToString(ck_token_info.HardwareVersion);
			_firmwareVersion = ConvertUtils.CkVersionToString(ck_token_info.FirmwareVersion);
			_utcTimeString = ConvertUtils.BytesToUtf8String(ck_token_info.UtcTime, true);
			try
			{
				_utcTime = ConvertUtils.UtcTimeStringToDateTime(_utcTimeString);
			}
			catch
			{
				_utcTime = null;
			}
		}
	}
}
