using Aesob.Web.Library.Path;
using Aesob.Web.Library.Utility;
using System.IO;
using System.Text;

namespace Aesob.Web.Library
{
    public static class Debug
    {
        private static DateTime _applicationStartDate;
        private static string _logFileName;

        public static void Assert(bool condition, string message)
        {
            System.Diagnostics.Debug.Assert(condition, message);

            if (!condition)
            {
                Log(message);
            }
        }

        public static void FailedAssert(string message)
        {
            System.Diagnostics.Debug.Assert(false, message);

            Log(message);
        }

        public static void Print(string message)
        {
            var textWithDateTime = DateTimeUtility.GetTextAppendedToDateTime(DateTime.Now, message, true);
            Console.WriteLine(textWithDateTime);
            Log(textWithDateTime);
        }

        public static void Initialize(DateTime applicationStartDate)
        {
            _applicationStartDate = applicationStartDate;
            _logFileName = "log_" + _applicationStartDate.ToString("dd_MM_yyyy__HH_mm_ss") + ".txt";
        }

        public static void Log(string message)
        {
            var logPath = ApplicationPath.LogFolder;
            var logPathString = logPath.ToString();

            try
            {
                if (!Directory.Exists(logPathString))
                {
                    Directory.CreateDirectory(logPathString);
                }

                var filePath = logPath.Append(_logFileName).ToString();

                StreamWriter streamWriter;
                if (!File.Exists(filePath))
                {
                    streamWriter = new StreamWriter(File.Create(filePath));
                }
                else
                {
                    streamWriter = File.AppendText(filePath);
                }

                streamWriter.WriteLine(message);
                streamWriter.Close();
                streamWriter.Dispose();
            }
            catch (Exception e)
            {
                FailedAssert("Failed to log message: " + message + "\nError: " + e.Message);
            }
        }
    }
}
