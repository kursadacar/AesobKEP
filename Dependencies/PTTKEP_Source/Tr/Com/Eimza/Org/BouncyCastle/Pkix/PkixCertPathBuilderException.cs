using System;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Pkix
{
	[Serializable]
	internal class PkixCertPathBuilderException : GeneralSecurityException
	{
		public PkixCertPathBuilderException()
		{
		}

		public PkixCertPathBuilderException(string message)
			: base(message)
		{
		}

		public PkixCertPathBuilderException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}
