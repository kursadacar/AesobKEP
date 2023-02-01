using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkExtractParams : IMechanismParams
	{
		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkExtractParams _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkExtractParams _params8;

		public CkExtractParams(ulong bit)
		{
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkExtractParams(Convert.ToUInt32(bit));
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkExtractParams(bit);
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
