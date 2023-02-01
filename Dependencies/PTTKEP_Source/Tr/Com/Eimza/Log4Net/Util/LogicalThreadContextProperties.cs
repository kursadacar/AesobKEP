using System.Runtime.Remoting.Messaging;

namespace Tr.Com.Eimza.Log4Net.Util
{
	public sealed class LogicalThreadContextProperties : ContextPropertiesBase
	{
		public override object this[string key]
		{
			get
			{
				PropertiesDictionary properties = GetProperties(false);
				if (properties != null)
				{
					return properties[key];
				}
				return null;
			}
			set
			{
				GetProperties(true)[key] = value;
			}
		}

		internal LogicalThreadContextProperties()
		{
		}

		public void Remove(string key)
		{
			PropertiesDictionary properties = GetProperties(false);
			if (properties != null)
			{
				properties.Remove(key);
			}
		}

		public void Clear()
		{
			PropertiesDictionary properties = GetProperties(false);
			if (properties != null)
			{
				properties.Clear();
			}
		}

		internal PropertiesDictionary GetProperties(bool create)
		{
			PropertiesDictionary propertiesDictionary = (PropertiesDictionary)CallContext.GetData("log4net.Util.LogicalThreadContextProperties");
			if (propertiesDictionary == null && create)
			{
				propertiesDictionary = new PropertiesDictionary();
				CallContext.SetData("log4net.Util.LogicalThreadContextProperties", propertiesDictionary);
			}
			return propertiesDictionary;
		}
	}
}
