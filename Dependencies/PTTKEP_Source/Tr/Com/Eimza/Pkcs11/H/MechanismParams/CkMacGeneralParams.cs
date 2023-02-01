using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkMacGeneralParams : IMechanismParams
	{
		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkMacGeneralParams _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkMacGeneralParams _params8;

		public CkMacGeneralParams(ulong macLength)
		{
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkMacGeneralParams(Convert.ToUInt32(macLength));
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkMacGeneralParams(macLength);
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
