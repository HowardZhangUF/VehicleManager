using AsyncSocket;
using LibraryForVM;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.NewCommunication
{
    public interface ICommunicatorServer : ICommunicator, ISystemWithLoopTask
    {
        event EventHandler<ListenStateChangedEventArgs> LocalListenStateChanged;
        event EventHandler<ConnectStateChangedEventArgs> RemoteConnectStateChanged;

        int mLocalPort { get; }
        bool mIsListened { get; }
        string[] mClientIpPorts { get; }

        void StartListen();
        void StopListen();
        void SendData(string IpPort, object Data);
    }
}
