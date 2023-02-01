using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Pkix
{
	internal abstract class PkixAttrCertChecker
	{
		public abstract ISet GetSupportedExtensions();

		public abstract void Check(IX509AttributeCertificate attrCert, PkixCertPath certPath, PkixCertPath holderCertPath, ICollection unresolvedCritExts);

		public abstract PkixAttrCertChecker Clone();
	}
}
