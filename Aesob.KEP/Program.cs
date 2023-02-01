using Aesob.Web.Core;

namespace AesobKEP
{
    public static class Program
    {
        public static AesobWebProgram Instance { get; private set; }

        public static int Main(string[] args)
        {
            Instance = new AesobWebProgram();

            var mainTask = Instance.Start();

            mainTask.Wait();

            return 0;
        }
    }
}