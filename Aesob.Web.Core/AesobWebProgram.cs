using Aesob.Web.Core.Internal;

namespace Aesob.Web.Core
{
    public class AesobWebProgram
    {
        public bool IsRunning { get; private set; }

        private ServiceManager _serviceManager;

        public AesobWebProgram()
        {
            _serviceManager = new ServiceManager();
        }

        public async Task Start()
        {
            IsRunning = true;

            _serviceManager.Start();

            while (IsRunning)
            {
                await Task.Delay(10);
            }

            _serviceManager.Stop();
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}
