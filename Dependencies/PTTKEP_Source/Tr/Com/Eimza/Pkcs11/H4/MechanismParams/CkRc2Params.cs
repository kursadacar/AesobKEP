using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkRc2Params : IMechanismParams
	{
		private CK_RC2_PARAMS _lowLevelStruct;

		public CkRc2Params(uint effectiveBits)
		{
			_lowLevelStruct.EffectiveBits = effectiveBits;
		}

		public object ToMarshalableStructure()
		{
			return _lowLevelStruct;
		}
	}
}
