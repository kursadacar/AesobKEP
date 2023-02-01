using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkRc5MacGeneralParams : IMechanismParams
	{
		private CK_RC5_MAC_GENERAL_PARAMS _lowLevelStruct;

		public CkRc5MacGeneralParams(ulong wordsize, ulong rounds, ulong macLength)
		{
			_lowLevelStruct.Wordsize = wordsize;
			_lowLevelStruct.Rounds = rounds;
			_lowLevelStruct.MacLength = macLength;
		}

		public object ToMarshalableStructure()
		{
			return _lowLevelStruct;
		}
	}
}
