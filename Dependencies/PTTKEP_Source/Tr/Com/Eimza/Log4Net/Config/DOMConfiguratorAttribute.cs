using System;

namespace Tr.Com.Eimza.Log4Net.Config
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Assembly)]
	[Obsolete("Use XmlConfiguratorAttribute instead of DOMConfiguratorAttribute")]
	public sealed class DOMConfiguratorAttribute : XmlConfiguratorAttribute
	{
	}
}
