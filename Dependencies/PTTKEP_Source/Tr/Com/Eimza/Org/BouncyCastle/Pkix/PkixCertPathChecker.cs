using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Pkix
{
	internal abstract class PkixCertPathChecker
	{
		public abstract void Init(bool forward);

		public abstract bool IsForwardCheckingSupported();

		public abstract ISet GetSupportedExtensions();

		public abstract void Check(X509Certificate cert, ICollection unresolvedCritExts);

		public virtual object Clone()
		{
			return MemberwiseClone();
		}
	}
}
