using System;

namespace LibraryForVM
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
