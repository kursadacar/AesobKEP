using System;
using System.Reflection;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Repository;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Config
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class SecurityContextProviderAttribute : ConfiguratorAttribute
	{
		private Type m_providerType;

		public Type ProviderType
		{
			get
			{
				return m_providerType;
			}
			set
			{
				m_providerType = value;
			}
		}

		public SecurityContextProviderAttribute(Type providerType)
			: base(100)
		{
			m_providerType = providerType;
		}

		public override void Configure(Assembly sourceAssembly, ILoggerRepository targetRepository)
		{
			if ((object)m_providerType == null)
			{
				LogLog.Error("SecurityContextProviderAttribute: Attribute specified on assembly [" + sourceAssembly.FullName + "] with null ProviderType.");
				return;
			}
			LogLog.Debug("SecurityContextProviderAttribute: Creating provider of type [" + m_providerType.FullName + "]");
			SecurityContextProvider securityContextProvider = Activator.CreateInstance(m_providerType) as SecurityContextProvider;
			if (securityContextProvider == null)
			{
				LogLog.Error("SecurityContextProviderAttribute: Failed to create SecurityContextProvider instance of type [" + m_providerType.Name + "].");
			}
			else
			{
				SecurityContextProvider.DefaultProvider = securityContextProvider;
			}
		}
	}
}
