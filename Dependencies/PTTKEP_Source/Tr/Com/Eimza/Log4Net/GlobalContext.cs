using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net
{
	public sealed class GlobalContext
	{
		private static readonly GlobalContextProperties s_properties;

		public static GlobalContextProperties Properties
		{
			get
			{
				return s_properties;
			}
		}

		private GlobalContext()
		{
		}

		static GlobalContext()
		{
			s_properties = new GlobalContextProperties();
			Properties["log4net:HostName"] = SystemInfo.HostName;
		}
	}
}
