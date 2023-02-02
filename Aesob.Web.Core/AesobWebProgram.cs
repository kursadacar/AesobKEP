using Aesob.Web.Core.Internal;
using Aesob.Web.Library.Service;

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

            await _serviceManager.Update();

            _serviceManager.Stop();
        }

        public void Stop()
        {
            IsRunning = false;
        }

        internal IAesobService GetService<T>() where T : IAesobService
        {
            return _serviceManager.GetService<T>();
        }
    }
}
