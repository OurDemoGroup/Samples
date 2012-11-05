using HealthCheckingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer
{
    class Program
    {
        static void Main(string[] args)
        {
            AppHealthService healthService = new AppHealthService();
            healthService.Start();

            Console.WriteLine("Heath Service is available. " +
              "Press <ENTER> to exit.");
            Console.ReadLine();

            healthService.Stop();
        }
    }
}
