using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;

namespace HealthCheckingService
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
        UseSynchronizationContext = false)]
    public class AppHealthService : IFPSService
    {
        #region Fields

        public const string BaseAddress = "net.pipe://localhost/";
        public const string ServiceAddress = "AppHealthService";
        public ServiceHost host = null;
        // TODO: refactor
        private static List<IFPSCallback> subscribers = new List<IFPSCallback>();
        private Process process;
        private string subscriberAppName;
        //

        private Stopwatch appWatch = new Stopwatch();
        private const double appCriticalTimespan = 6000.0f;

        #endregion

        #region AppHealthService Creation

        public void Start()
        {
            host = new ServiceHost(
                typeof(AppHealthService),
                new Uri(BaseAddress));
            
                host.AddServiceEndpoint(typeof(IFPSService),
                  new NetNamedPipeBinding(),
                  ServiceAddress);

                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();

                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);

                host.AddServiceEndpoint(
                  ServiceMetadataBehavior.MexContractName,
                  MetadataExchangeBindings.CreateMexNamedPipeBinding(),
                  ServiceAddress + "mex"
                );

                try
                {
                    host.Open();                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            
        }

        public void Stop()
        {
            try
            {
                host.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void RelaunchSubscriber(IFPSCallback subscriber)
        {
            if (!process.HasExited)
            {
                process.Kill();
            }
            Process.Start(subscriberAppName);
            subscribers.Remove(subscriber);
        }

        #endregion

        #region IFPSService Implementation

        private void ChannelFactory_Faulted(object sender, EventArgs e)
        {
            
            Console.WriteLine("Client Channel Failed!");
            RelaunchSubscriber((IFPSCallback)sender);
        }

        public bool Subscribe(string subscriberApplicationName, string subscriberProcessName)
        {
            try
            {
                //Get the hashCode of the connecting app and store it as a connection
                IFPSCallback callback = OperationContext.Current.GetCallbackChannel<IFPSCallback>();
                OperationContext.Current.Channel.Faulted += new EventHandler(ChannelFactory_Faulted);
                if (!subscribers.Contains(callback))
                {
                    subscribers.Add(callback);
                    subscriberAppName = subscriberApplicationName;
                    process = Process.GetProcessesByName(subscriberProcessName)[0];
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool Unsubscribe()
        {
            try
            {
                //remove any connection that is leaving
                IFPSCallback callback = OperationContext.Current.GetCallbackChannel<IFPSCallback>();
                if (subscribers.Contains(callback))
                    subscribers.Remove(callback);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void GetFPS(double value)
        {
            //Go through the list of connections and call their callback function
            subscribers.ForEach(delegate(IFPSCallback callback)
            {
                if (((ICommunicationObject)callback).State == CommunicationState.Opened)
                {
                    Console.WriteLine("FPS: " + value.ToString());
                    FrameRateStatus status = new FrameRateStatus(value);
                    callback.ApplicationFPS(status);

                    // Just for test, fast code
                    // Relaunch Application
                    if (status.FPSStatus == Status.Critical)
                    {
                        if (!appWatch.IsRunning)
                        {
                            appWatch.Reset();
                            appWatch.Start();
                        }
                        if (appWatch.Elapsed.TotalMilliseconds >= appCriticalTimespan)
                        {
                            appWatch.Reset();
                            // kill
                            if (!process.HasExited)
                            {
                                process.Kill();
                            }
                        }
                    }
                    else
                    {
                        if (appWatch.IsRunning)
                        {
                            appWatch.Reset();
                        }
                    }
                }
                else
                {
                    subscribers.Remove(callback);
                }
            });
        }

        #endregion
    }
}
