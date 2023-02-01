using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net
{
	public sealed class ThreadContext
	{
		private static readonly ThreadContextProperties s_properties = new ThreadContextProperties();

		private static readonly ThreadContextStacks s_stacks = new ThreadContextStacks(s_properties);

		public static ThreadContextProperties Properties
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

		private ThreadContext()
		{
		}
	}
}
