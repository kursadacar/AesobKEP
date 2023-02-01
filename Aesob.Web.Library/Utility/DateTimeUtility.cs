using System.Text;

namespace Aesob.Web.Library.Utility
{
    public static class DateTimeUtility
    {
        private static string GetFormattedDate(DateTime dateTime, bool includeHour)
        {
            if (includeHour)
            {
                return dateTime.ToString("dd:MM:yyyy HH:mm:ss");
            }

            return dateTime.ToString("dd:MM:yyyy");
        }

        public static string GetTextAppendedToDateTime(DateTime dateTime, string text, bool includeHour = true)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append('[');
            stringBuilder.Append(GetFormattedDate(dateTime, includeHour));
            stringBuilder.Append("]: ");
            stringBuilder.Append(text);

            return stringBuilder.ToString();
        }
    }
}
