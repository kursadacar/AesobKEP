using System;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8.MechanismParams;

namespace Tr.Com.Eimza.Pkcs11.H8.MechanismParams
{
	internal class CkRc2CbcParams : IMechanismParams
	{
		private CK_RC2_CBC_PARAMS _lowLevelStruct;

		public CkRc2CbcParams(ulong effectiveBits, byte[] iv)
		{
			_lowLevelStruct.EffectiveBits = 0uL;
			_lowLevelStruct.Iv = new byte[8];
			_lowLevelStruct.EffectiveBits = effectiveBits;
			if (iv == null)
			{
				throw new ArgumentNullException("iv");
			}
			if (iv.Length != 8)
			{
				throw new ArgumentOutOfRangeException("iv", "Array has to be 8 bytes long");
			}
			Array.Copy(iv, _lowLevelStruct.Iv, iv.Length);
		}

		public object ToMarshalableStructure()
		{
			return _lowLevelStruct;
		}
	}
}
