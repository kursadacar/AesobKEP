using Aesob.Web.Library.Path;
using Aesob.Web.Library.Utility;
using System.Text;

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

        public static void Log(string message)
        {
            var logPath = DataPaths.Root.Copy();
            logPath.Append("Log");

            var logPathString = logPath.ToString();

            try
            {
                if (!Directory.Exists(logPathString))
                {
                    Directory.CreateDirectory(logPathString);
                }

                var filePath = logPath.Append("log.txt").ToString();
                if (!File.Exists(filePath))
                {
                    using (StreamWriter streamWriter = new StreamWriter(filePath))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(DateTime.Now.ToString("[ss:mm:HH - dd/MM/yyyy]"));
                        sb.Append(" ERROR: ");
                        sb.AppendLine(message);

                        streamWriter.Write(sb.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                FailedAssert("Failed to log message: " + message + "\nError: " + e.Message);
            }
        }
    }
}
