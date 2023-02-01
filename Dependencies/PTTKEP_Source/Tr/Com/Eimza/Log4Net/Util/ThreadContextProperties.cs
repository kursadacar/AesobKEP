using System;
using System.Threading;

namespace Tr.Com.Eimza.Log4Net.Util
{
	public sealed class ThreadContextProperties : ContextPropertiesBase
	{
		private static readonly LocalDataStoreSlot s_threadLocalSlot = Thread.AllocateDataSlot();

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

		internal ThreadContextProperties()
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
			PropertiesDictionary propertiesDictionary = (PropertiesDictionary)Thread.GetData(s_threadLocalSlot);
			if (propertiesDictionary == null && create)
			{
				propertiesDictionary = new PropertiesDictionary();
				Thread.SetData(s_threadLocalSlot, propertiesDictionary);
			}
			return propertiesDictionary;
		}
	}
}
