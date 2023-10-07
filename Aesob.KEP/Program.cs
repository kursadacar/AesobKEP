using Aesob.Web.Core;

namespace AesobKEP
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var program = new AesobWebProgram();

            var mainTask = program.Start();

            mainTask.Wait();

            return 0;
        }
    }
}