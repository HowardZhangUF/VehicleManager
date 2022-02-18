using System;

namespace LibraryForVM
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
