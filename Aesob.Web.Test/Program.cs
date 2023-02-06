using Aesob.Web.Core;

namespace Aesob.Web.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AesobWebProgram program = new AesobWebProgram();

            var mainTask = program.Start();

            mainTask.Wait();
        }
    }
}