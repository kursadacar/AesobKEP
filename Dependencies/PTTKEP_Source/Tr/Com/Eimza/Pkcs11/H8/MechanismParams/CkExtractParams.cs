using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkExtractParams : IMechanismParams
	{
		private CK_EXTRACT_PARAMS _lowLevelStruct;

		public CkExtractParams(ulong bit)
		{
			_lowLevelStruct.Bit = bit;
		}

		public object ToMarshalableStructure()
		{
			return _lowLevelStruct;
		}
	}
}
