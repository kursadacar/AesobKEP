using System;
using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Security.Certificates;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.X509;
using Tr.Com.Eimza.Org.BouncyCastle.X509.Store;

namespace Tr.Com.Eimza.Org.BouncyCastle.Pkix
{
	internal class PkixCertPathBuilder
	{
		private Exception certPathException;

		public virtual PkixCertPathBuilderResult Build(PkixBuilderParameters pkixParams)
		{
			IX509Selector targetCertConstraints = pkixParams.GetTargetCertConstraints();
			if (!(targetCertConstraints is X509CertStoreSelector))
			{
				string[] obj = new string[5]
				{
					"TargetConstraints must be an instance of ",
					typeof(X509CertStoreSelector).FullName,
					" for ",
					null,
					null
				};
				Type type = GetType();
				obj[3] = (((object)type != null) ? type.ToString() : null);
				obj[4] = " class.";
				throw new PkixCertPathBuilderException(string.Concat(obj));
			}
			ISet set = new HashSet();
			try
			{
				set.AddAll(PkixCertPathValidatorUtilities.FindCertificates((X509CertStoreSelector)targetCertConstraints, pkixParams.GetStores()));
			}
			catch (Exception exception)
			{
				throw new PkixCertPathBuilderException("Error finding target certificate.", exception);
			}
			if (set.IsEmpty)
			{
				throw new PkixCertPathBuilderException("No certificate found matching targetContraints.");
			}
			PkixCertPathBuilderResult pkixCertPathBuilderResult = null;
			IList tbvPath = Platform.CreateArrayList();
			foreach (X509Certificate item in set)
			{
				pkixCertPathBuilderResult = Build(item, pkixParams, tbvPath);
				if (pkixCertPathBuilderResult != null)
				{
					break;
				}
			}
			if (pkixCertPathBuilderResult == null && certPathException != null)
			{
				throw new PkixCertPathBuilderException(certPathException.Message, certPathException.InnerException);
			}
			if (pkixCertPathBuilderResult == null && certPathException == null)
			{
				throw new PkixCertPathBuilderException("Unable to find certificate chain.");
			}
			return pkixCertPathBuilderResult;
		}

		protected virtual PkixCertPathBuilderResult Build(X509Certificate tbvCert, PkixBuilderParameters pkixParams, IList tbvPath)
		{
			if (tbvPath.Contains(tbvCert))
			{
				return null;
			}
			if (pkixParams.GetExcludedCerts().Contains(tbvCert))
			{
				return null;
			}
			if (pkixParams.MaxPathLength != -1 && tbvPath.Count - 1 > pkixParams.MaxPathLength)
			{
				return null;
			}
			tbvPath.Add(tbvCert);
			PkixCertPathBuilderResult pkixCertPathBuilderResult = null;
			PkixCertPathValidator pkixCertPathValidator = new PkixCertPathValidator();
			try
			{
				if (PkixCertPathValidatorUtilities.FindTrustAnchor(tbvCert, pkixParams.GetTrustAnchors()) != null)
				{
					PkixCertPath pkixCertPath = null;
					try
					{
						pkixCertPath = new PkixCertPath(tbvPath);
					}
					catch (Exception innerException)
					{
						throw new Exception("Certification path could not be constructed from certificate list.", innerException);
					}
					PkixCertPathValidatorResult pkixCertPathValidatorResult = null;
					try
					{
						pkixCertPathValidatorResult = pkixCertPathValidator.Validate(pkixCertPath, pkixParams);
					}
					catch (Exception innerException2)
					{
						throw new Exception("Certification path could not be validated.", innerException2);
					}
					return new PkixCertPathBuilderResult(pkixCertPath, pkixCertPathValidatorResult.TrustAnchor, pkixCertPathValidatorResult.PolicyTree, pkixCertPathValidatorResult.SubjectPublicKey);
				}
				try
				{
					PkixCertPathValidatorUtilities.AddAdditionalStoresFromAltNames(tbvCert, pkixParams);
				}
				catch (CertificateParsingException innerException3)
				{
					throw new Exception("No additiontal X.509 stores can be added from certificate locations.", innerException3);
				}
				HashSet hashSet = new HashSet();
				try
				{
					hashSet.AddAll(PkixCertPathValidatorUtilities.FindIssuerCerts(tbvCert, pkixParams));
				}
				catch (Exception innerException4)
				{
					throw new Exception("Cannot find issuer certificate for certificate in certification path.", innerException4);
				}
				if (hashSet.IsEmpty)
				{
					throw new Exception("No issuer certificate for certificate in certification path found.");
				}
				foreach (X509Certificate item in hashSet)
				{
					pkixCertPathBuilderResult = Build(item, pkixParams, tbvPath);
					if (pkixCertPathBuilderResult != null)
					{
						break;
					}
				}
			}
			catch (Exception ex)
			{
				certPathException = ex;
			}
			if (pkixCertPathBuilderResult == null)
			{
				tbvPath.Remove(tbvCert);
			}
			return pkixCertPathBuilderResult;
		}
	}
}
