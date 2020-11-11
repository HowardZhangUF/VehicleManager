using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.NewCommunication
{
    public class SentDataEventArgs : EventArgs
    {
        public DateTime OccurTime { get; private set; }
        public string IpPort { get; private set; }
        public object Data { get; private set; }

        public SentDataEventArgs(DateTime OccurTime, string IpPort, object Data) : base()
        {
            this.OccurTime = OccurTime;
            this.IpPort = IpPort;
            this.Data = Data;
        }
    }
}
