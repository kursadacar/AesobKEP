using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H4.MechanismParams
{
	internal class CkAesCtrParams : IMechanismParams
	{
		private CK_AES_CTR_PARAMS _lowLevelStruct;

		public CkAesCtrParams(uint counterBits, byte[] cb)
		{
			_lowLevelStruct.CounterBits = 0u;
			_lowLevelStruct.Cb = new byte[16];
			_lowLevelStruct.CounterBits = counterBits;
			if (cb == null)
			{
				throw new ArgumentNullException("cb");
			}
			if (cb.Length != 16)
			{
				throw new ArgumentOutOfRangeException("cb", "Array has to be 16 bytes long");
			}
			Array.Copy(cb, _lowLevelStruct.Cb, cb.Length);
		}

		public object ToMarshalableStructure()
		{
			return _lowLevelStruct;
		}
	}
}
