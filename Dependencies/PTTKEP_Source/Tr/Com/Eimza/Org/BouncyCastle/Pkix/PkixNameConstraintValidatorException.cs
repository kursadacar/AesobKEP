using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Pkix
{
	[Serializable]
	internal class PkixNameConstraintValidatorException : Exception
	{
		public PkixNameConstraintValidatorException(string msg)
			: base(msg)
		{
		}
	}
}
