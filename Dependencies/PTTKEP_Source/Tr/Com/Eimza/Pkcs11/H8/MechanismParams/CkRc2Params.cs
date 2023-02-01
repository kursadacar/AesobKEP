using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkRc2Params : IMechanismParams
	{
		private CK_RC2_PARAMS _lowLevelStruct;

		public CkRc2Params(ulong effectiveBits)
		{
			_lowLevelStruct.EffectiveBits = effectiveBits;
		}

		public object ToMarshalableStructure()
		{
			return _lowLevelStruct;
		}
	}
}
