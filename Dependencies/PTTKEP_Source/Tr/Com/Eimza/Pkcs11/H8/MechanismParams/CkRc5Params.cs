using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkRc5Params : IMechanismParams
	{
		private CK_RC5_PARAMS _lowLevelStruct;

		public CkRc5Params(ulong wordsize, ulong rounds)
		{
			_lowLevelStruct.Wordsize = wordsize;
			_lowLevelStruct.Rounds = rounds;
		}

		public object ToMarshalableStructure()
		{
			return _lowLevelStruct;
		}
	}
}
