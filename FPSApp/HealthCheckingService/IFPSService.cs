using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace HealthCheckingService
{
    [ServiceContract]
    public interface IFPSCallback
    {
        // Method, which client implements and can
        // vary him behavior, depending on FPS
        [OperationContract(IsOneWay = true)]
        void ApplicationFPS(FrameRateStatus fpsStatus);
    }

    [ServiceContract(CallbackContract = typeof(IFPSCallback))]
    public interface IFPSService
    {
        [OperationContract]
        void GetFPS(double value);
        [OperationContract]
        bool Subscribe(string subscriberApplicationName, string subscriberProcessName);
        [OperationContract]
        bool Unsubscribe();
    }

    [DataContract(Name = "Status")]
    public enum Status
    {
        [EnumMember]
        Critical = 0, // FPS [0..5]
        [EnumMember]
        LowFrameRate = 6, // FPS [6..18]
        [EnumMember]
        NormalFrameRate = 19 // FPS [19.....]
    }

    [DataContract(Name = "FrameRateStatus")]
    public class FrameRateStatus
    {
        public FrameRateStatus(double frameRate)
        {
            FPS = frameRate;
        }
        
        [DataMember]
        public double FPS
        {
            get { return fps; }
            set
            {
                fps = value;
                foreach (Status status in Enum.GetValues(typeof(Status)))
                {
                    if ((int)status > fps)
                    {
                        return;
                    }
                    FPSStatus = status;
                }
            }
        }
        [DataMember]
        public Status FPSStatus;
        private double fps = 0;
    }
}
