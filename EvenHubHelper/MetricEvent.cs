
using System.Runtime.Serialization;
namespace EventHubHelper
{
    [DataContract]
    public class MetricEvent
    {
        [DataMember]
        public int DeviceId { get; set; }
        [DataMember]
        public object MakeTime { get; set; }
        [DataMember]
        public int Purity { get; set; }
        [DataMember]
        public int Shortage { get; set; }
    }
}
