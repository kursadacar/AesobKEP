using Aesob.Web.Library.Utility;

namespace Aesob.Web.Library
{
    public static class Debug
    {
        public static void Assert(bool condition, string message)
        {
            System.Diagnostics.Debug.Assert(condition, message);
        }

        public static void FailedAssert(string message)
        {
            System.Diagnostics.Debug.Assert(false, message);
        }

        public static void Print(string message)
        {
            Console.WriteLine(DateTimeUtility.GetTextAppendedToDateTime(DateTime.Now, message, true)); ;
        }
    }
}
