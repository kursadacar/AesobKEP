using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkRc2MacGeneralParams : IMechanismParams
	{
		private CK_RC2_MAC_GENERAL_PARAMS _lowLevelStruct;

		public CkRc2MacGeneralParams(uint effectiveBits, uint macLength)
		{
			_lowLevelStruct.EffectiveBits = effectiveBits;
			_lowLevelStruct.MacLength = macLength;
		}

		public object ToMarshalableStructure()
		{
			return _lowLevelStruct;
		}
	}
}
