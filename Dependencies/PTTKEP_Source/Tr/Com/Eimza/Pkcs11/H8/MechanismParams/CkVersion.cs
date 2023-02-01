using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkVersion : IMechanismParams
	{
		private CK_VERSION _lowLevelStruct;

		public byte Major
		{
			get
			{
				return _lowLevelStruct.Major[0];
			}
		}

		public byte Minor
		{
			get
			{
				return _lowLevelStruct.Minor[0];
			}
		}

		public CkVersion(byte major, byte minor)
		{
			_lowLevelStruct.Major = new byte[1] { major };
			_lowLevelStruct.Minor = new byte[1] { minor };
		}

		public object ToMarshalableStructure()
		{
			return _lowLevelStruct;
		}

		public override string ToString()
		{
			return ConvertUtils.CkVersionToString(_lowLevelStruct);
		}
	}
}
