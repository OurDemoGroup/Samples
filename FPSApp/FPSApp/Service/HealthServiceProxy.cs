using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCheckingService;
using System.ServiceModel;
using System.Diagnostics;

namespace FPSApp
{
    class HealthServiceProxy : IDisposable
    {
        #region Fields

        private IFPSService pipeProxy = null;

        #endregion

        #region HealthServiceProxy Live Handling

        public bool Connect(IFPSCallback callbackInstance)
        {
            DuplexChannelFactory<IFPSService> pipeFactory =
                  new DuplexChannelFactory<IFPSService>(
                      new InstanceContext(callbackInstance),
                      new NetNamedPipeBinding(),
                      new EndpointAddress(AppHealthService.BaseAddress +
                          AppHealthService.ServiceAddress));
            try
            {
                pipeProxy = pipeFactory.CreateChannel();
                pipeProxy.Subscribe(System.Reflection.Assembly.GetExecutingAssembly().Location,
                    Process.GetCurrentProcess().ProcessName);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public void Close()
        {
            pipeProxy.Unsubscribe();
        }

        public void Dispose()
        {
            pipeProxy.Unsubscribe();
        }

        #endregion

        #region HealthServiceProxy Server Methods Calls

        public void SendFPS(double fps)
        {
            // TODO: Add try catch
            pipeProxy.GetFPS(fps);
        }

        #endregion
    }
}
