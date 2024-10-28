using Aesob.Web.Core;

namespace Aesob.KEP.Backup
{
	internal class Program
	{
		static int Main(string[] args)
		{
			var program = new AesobWebProgram();

			var mainTask = program.Start();

			mainTask.Wait();

			return 0;
		}
	}
}
