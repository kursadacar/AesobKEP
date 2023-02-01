using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkAesCtrParams : IMechanismParams
	{
		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkAesCtrParams _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkAesCtrParams _params8;

		public CkAesCtrParams(ulong counterBits, byte[] cb)
		{
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkAesCtrParams(Convert.ToUInt32(counterBits), cb);
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkAesCtrParams(counterBits, cb);
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
