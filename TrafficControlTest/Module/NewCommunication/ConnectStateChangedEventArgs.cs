using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.NewCommunication
{
    public class ConnectStateChangedEventArgs : EventArgs
    {
        public DateTime OccurTime { get; private set; }
        public string IpPort { get; private set; }
        public bool IsConnected { get; private set; }

        public ConnectStateChangedEventArgs(DateTime OccurTime, string IpPort, bool IsConnected) : base()
        {
            this.OccurTime = OccurTime;
            this.IpPort = IpPort;
            this.IsConnected = IsConnected;
        }
    }
}
