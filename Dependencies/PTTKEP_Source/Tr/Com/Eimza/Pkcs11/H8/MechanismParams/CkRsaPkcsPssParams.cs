using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkRsaPkcsPssParams : IMechanismParams
	{
		private CK_RSA_PKCS_PSS_PARAMS _lowLevelStruct;

		public CkRsaPkcsPssParams(ulong hashAlg, ulong mgf, ulong len)
		{
			_lowLevelStruct.HashAlg = hashAlg;
			_lowLevelStruct.Mgf = mgf;
			_lowLevelStruct.Len = len;
		}

		public object ToMarshalableStructure()
		{
			return _lowLevelStruct;
		}
	}
}
