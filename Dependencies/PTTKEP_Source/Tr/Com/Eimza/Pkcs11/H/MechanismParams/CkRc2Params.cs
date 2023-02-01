using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkRc2Params : IMechanismParams
	{
		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkRc2Params _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkRc2Params _params8;

		public CkRc2Params(ulong effectiveBits)
		{
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkRc2Params(Convert.ToUInt32(effectiveBits));
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkRc2Params(effectiveBits);
			}
		}

		public object ToMarshalableStructure()
		{
			if (UnmanagedLong.Size == 4)
			{
				return _params4.ToMarshalableStructure();
			}
			return _params8.ToMarshalableStructure();
		}
	}
}
