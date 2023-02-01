using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkCamelliaCtrParams : IMechanismParams
	{
		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkCamelliaCtrParams _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkCamelliaCtrParams _params8;

		public CkCamelliaCtrParams(ulong counterBits, byte[] cb)
		{
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkCamelliaCtrParams(Convert.ToUInt32(counterBits), cb);
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkCamelliaCtrParams(counterBits, cb);
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
