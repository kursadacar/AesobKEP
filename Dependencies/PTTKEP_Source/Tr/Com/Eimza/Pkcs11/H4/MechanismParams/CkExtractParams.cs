using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkExtractParams : IMechanismParams
	{
		private CK_EXTRACT_PARAMS _lowLevelStruct;

		public CkExtractParams(uint bit)
		{
			_lowLevelStruct.Bit = bit;
		}

		public object ToMarshalableStructure()
		{
			return _lowLevelStruct;
		}
	}
}
