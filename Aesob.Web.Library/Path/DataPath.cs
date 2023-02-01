namespace Aesob.Web.Library.Path
{
    public static class DataPath
    {
        public static string Root
        {
            get
            {
#if DEBUG
                return AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\..";
#else
                return AppDomain.CurrentDomain.BaseDirectory;
#endif

            }
        }

        public static string DataFolder => Root + "\\Data";
        public static string ConfigFolder => DataFolder + "\\Config";
    }
}
