using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Util
{
	public class WindowsSecurityContext : SecurityContext, IOptionHandler
	{
		public enum ImpersonationMode
		{
			User,
			Process
		}

		private sealed class DisposableImpersonationContext : IDisposable
		{
			private readonly WindowsImpersonationContext m_impersonationContext;

			public DisposableImpersonationContext(WindowsImpersonationContext impersonationContext)
			{
				m_impersonationContext = impersonationContext;
			}

			public void Dispose()
			{
				m_impersonationContext.Undo();
			}
		}

		private ImpersonationMode m_impersonationMode;

		private string m_userName;

		private string m_domainName = Environment.MachineName;

		private string m_password;

		private WindowsIdentity m_identity;

		public ImpersonationMode Credentials
		{
			get
			{
				return m_impersonationMode;
			}
			set
			{
				m_impersonationMode = value;
			}
		}

		public string UserName
		{
			get
			{
				return m_userName;
			}
			set
			{
				m_userName = value;
			}
		}

		public string DomainName
		{
			get
			{
				return m_domainName;
			}
			set
			{
				m_domainName = value;
			}
		}

		public string Password
		{
			set
			{
				m_password = value;
			}
		}

		public void ActivateOptions()
		{
			if (m_impersonationMode == ImpersonationMode.User)
			{
				if (m_userName == null)
				{
					throw new ArgumentNullException("m_userName");
				}
				if (m_domainName == null)
				{
					throw new ArgumentNullException("m_domainName");
				}
				if (m_password == null)
				{
					throw new ArgumentNullException("m_password");
				}
				m_identity = LogonUser(m_userName, m_domainName, m_password);
			}
		}

		public override IDisposable Impersonate(object state)
		{
			if (m_impersonationMode == ImpersonationMode.User)
			{
				if (m_identity != null)
				{
					return new DisposableImpersonationContext(m_identity.Impersonate());
				}
			}
			else if (m_impersonationMode == ImpersonationMode.Process)
			{
				return new DisposableImpersonationContext(WindowsIdentity.Impersonate(IntPtr.Zero));
			}
			return null;
		}

		private static WindowsIdentity LogonUser(string userName, string domainName, string password)
		{
			IntPtr phToken = IntPtr.Zero;
			if (!LogonUser(userName, domainName, password, 2, 0, ref phToken))
			{
				NativeError lastError = NativeError.GetLastError();
				throw new Exception("Failed to LogonUser [" + userName + "] in Domain [" + domainName + "]. Error: " + lastError.ToString());
			}
			IntPtr DuplicateTokenHandle = IntPtr.Zero;
			if (!DuplicateToken(phToken, 2, ref DuplicateTokenHandle))
			{
				NativeError lastError2 = NativeError.GetLastError();
				if (phToken != IntPtr.Zero)
				{
					CloseHandle(phToken);
				}
				throw new Exception("Failed to DuplicateToken after LogonUser. Error: " + lastError2.ToString());
			}
			WindowsIdentity result = new WindowsIdentity(DuplicateTokenHandle);
			if (DuplicateTokenHandle != IntPtr.Zero)
			{
				CloseHandle(DuplicateTokenHandle);
			}
			if (phToken != IntPtr.Zero)
			{
				CloseHandle(phToken);
			}
			return result;
		}

		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern bool CloseHandle(IntPtr handle);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool DuplicateToken(IntPtr ExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);
	}
}
