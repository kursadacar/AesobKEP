using System;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Pkix
{
	[Serializable]
	internal class PkixCertPathValidatorException : GeneralSecurityException
	{
		private Exception cause;

		private PkixCertPath certPath;

		private int index = -1;

		public override string Message
		{
			get
			{
				string message = base.Message;
				if (message != null)
				{
					return message;
				}
				if (cause != null)
				{
					return cause.Message;
				}
				return null;
			}
		}

		public PkixCertPath CertPath
		{
			get
			{
				return certPath;
			}
		}

		public int Index
		{
			get
			{
				return index;
			}
		}

		public PkixCertPathValidatorException()
		{
		}

		public PkixCertPathValidatorException(string message)
			: base(message)
		{
		}

		public PkixCertPathValidatorException(string message, Exception cause)
			: base(message)
		{
			this.cause = cause;
		}

		public PkixCertPathValidatorException(string message, Exception cause, PkixCertPath certPath, int index)
			: base(message)
		{
			if (certPath == null && index != -1)
			{
				throw new ArgumentNullException("certPath = null and index != -1");
			}
			if (index < -1 || (certPath != null && index >= certPath.Certificates.Count))
			{
				throw new IndexOutOfRangeException(" index < -1 or out of bound of certPath.getCertificates()");
			}
			this.cause = cause;
			this.certPath = certPath;
			this.index = index;
		}
	}
}
