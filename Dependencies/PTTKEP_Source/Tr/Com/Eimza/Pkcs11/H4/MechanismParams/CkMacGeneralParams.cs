using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkMacGeneralParams : IMechanismParams
	{
		private CK_MAC_GENERAL_PARAMS _lowLevelStruct;

		public CkMacGeneralParams(uint macLength)
		{
			_lowLevelStruct.MacLength = macLength;
		}

		public object ToMarshalableStructure()
		{
			return _lowLevelStruct;
		}
	}
}
