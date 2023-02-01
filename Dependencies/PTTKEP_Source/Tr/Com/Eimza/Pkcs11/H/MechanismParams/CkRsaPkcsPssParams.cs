using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkRsaPkcsPssParams : IMechanismParams
	{
		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkRsaPkcsPssParams _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkRsaPkcsPssParams _params8;

		public CkRsaPkcsPssParams(ulong hashAlg, ulong mgf, ulong len)
		{
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkRsaPkcsPssParams(Convert.ToUInt32(hashAlg), Convert.ToUInt32(mgf), Convert.ToUInt32(len));
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkRsaPkcsPssParams(hashAlg, mgf, len);
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
