using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkMacGeneralParams : IMechanismParams
	{
		private CK_MAC_GENERAL_PARAMS _lowLevelStruct;

		public CkMacGeneralParams(ulong macLength)
		{
			_lowLevelStruct.MacLength = macLength;
		}

		public object ToMarshalableStructure()
		{
			return _lowLevelStruct;
		}
	}
}
