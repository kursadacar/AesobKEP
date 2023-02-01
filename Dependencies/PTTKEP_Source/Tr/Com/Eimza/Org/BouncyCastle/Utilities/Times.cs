using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Utilities
{
	internal static class Times
	{
		private static long NanosecondsPerTick = 100L;

		public static long NanoTime()
		{
			return DateTime.UtcNow.Ticks * NanosecondsPerTick;
		}
	}
}
