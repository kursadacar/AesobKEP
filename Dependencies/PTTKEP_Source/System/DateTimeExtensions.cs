namespace System
{
	internal static class DateTimeExtensions
	{
		public static bool IsEquals(this DateTime firstTime, DateTime secondTime)
		{
			if (firstTime.Kind != DateTimeKind.Utc)
			{
				firstTime = firstTime.ToUniversalTime();
			}
			if (secondTime.Kind != DateTimeKind.Utc)
			{
				secondTime = secondTime.ToUniversalTime();
			}
			if (firstTime.Equals(secondTime))
			{
				return true;
			}
			return false;
		}
	}
}
