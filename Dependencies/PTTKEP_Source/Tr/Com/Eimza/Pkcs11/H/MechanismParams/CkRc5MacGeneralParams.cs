using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkRc5MacGeneralParams : IMechanismParams
	{
		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkRc5MacGeneralParams _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkRc5MacGeneralParams _params8;

		public CkRc5MacGeneralParams(ulong wordsize, ulong rounds, ulong macLength)
		{
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkRc5MacGeneralParams(Convert.ToUInt32(wordsize), Convert.ToUInt32(rounds), Convert.ToUInt32(macLength));
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkRc5MacGeneralParams(wordsize, rounds, macLength);
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
