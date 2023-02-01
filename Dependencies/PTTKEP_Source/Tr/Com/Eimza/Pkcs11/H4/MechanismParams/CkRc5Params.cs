using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkRc5Params : IMechanismParams
	{
		private CK_RC5_PARAMS _lowLevelStruct;

		public CkRc5Params(uint wordsize, uint rounds)
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
