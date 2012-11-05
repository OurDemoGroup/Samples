using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using HealthCheckingService;
using System.Threading;

namespace FPSApp
{
    class HealthServiceClient : IFPSCallback
    {
        private HealthServiceProxy healthServiceProxy;
        private System.Timers.Timer timer;
        private BasicTimer fpsTimer;

        public HealthServiceClient()
        {
            healthServiceProxy = new HealthServiceProxy();
            if (healthServiceProxy.Connect(this))
            {
                timer = new System.Timers.Timer();
                // raise per 3 seconds
                timer.Interval = 3;
                timer.Elapsed += new ElapsedEventHandler(UpdateServer);
                timer.Start();
                fpsTimer = new BasicTimer();
            }
        }

        public void ApplicationFPS(FrameRateStatus fpsStatus)
        {

        }

        private void UpdateServer(object sender, System.Timers.ElapsedEventArgs e)
        {
            healthServiceProxy.SendFPS(fpsTimer.FPS);
        }
    }
}
