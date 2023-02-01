using System;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Plugin;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Config
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class PluginAttribute : Attribute, IPluginFactory
	{
		private string m_typeName;

		private Type m_type;

		public Type Type
		{
			get
			{
				return m_type;
			}
			set
			{
				m_type = value;
			}
		}

		public string TypeName
		{
			get
			{
				return m_typeName;
			}
			set
			{
				m_typeName = value;
			}
		}

		public PluginAttribute(string typeName)
		{
			m_typeName = typeName;
		}

		public PluginAttribute(Type type)
		{
			m_type = type;
		}

		public IPlugin CreatePlugin()
		{
			Type type = m_type;
			if ((object)m_type == null)
			{
				type = SystemInfo.GetTypeFromString(m_typeName, true, true);
			}
			if (!typeof(IPlugin).IsAssignableFrom(type))
			{
				throw new LogException("Plugin type [" + type.FullName + "] does not implement the log4net.IPlugin interface");
			}
			return (IPlugin)Activator.CreateInstance(type);
		}

		public override string ToString()
		{
			if ((object)m_type != null)
			{
				return "PluginAttribute[Type=" + m_type.FullName + "]";
			}
			return "PluginAttribute[Type=" + m_typeName + "]";
		}
	}
}
