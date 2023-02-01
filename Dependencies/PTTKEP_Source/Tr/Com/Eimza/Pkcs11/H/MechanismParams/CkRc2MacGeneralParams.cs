using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkRc2MacGeneralParams : IMechanismParams
	{
		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkRc2MacGeneralParams _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkRc2MacGeneralParams _params8;

		public CkRc2MacGeneralParams(ulong effectiveBits, ulong macLength)
		{
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkRc2MacGeneralParams(Convert.ToUInt32(effectiveBits), Convert.ToUInt32(macLength));
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkRc2MacGeneralParams(effectiveBits, macLength);
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
