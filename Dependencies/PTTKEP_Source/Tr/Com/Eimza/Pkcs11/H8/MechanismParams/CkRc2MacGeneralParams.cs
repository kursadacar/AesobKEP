using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkRc2MacGeneralParams : IMechanismParams
	{
		private CK_RC2_MAC_GENERAL_PARAMS _lowLevelStruct;

		public CkRc2MacGeneralParams(ulong effectiveBits, ulong macLength)
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
