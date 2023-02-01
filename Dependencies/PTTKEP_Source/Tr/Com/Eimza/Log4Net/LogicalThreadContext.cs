using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net
{
	public sealed class LogicalThreadContext
	{
		private static readonly LogicalThreadContextProperties s_properties = new LogicalThreadContextProperties();

		private static readonly ThreadContextStacks s_stacks = new ThreadContextStacks(s_properties);

		public static LogicalThreadContextProperties Properties
		{
			get
			{
				return s_properties;
			}
		}

		public static ThreadContextStacks Stacks
		{
			get
			{
				return s_stacks;
			}
		}

		private LogicalThreadContext()
		{
		}
	}
}
