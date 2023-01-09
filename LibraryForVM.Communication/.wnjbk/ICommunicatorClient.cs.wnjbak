using System;

namespace LibraryForVM
{
	public interface ICommunicatorClient : ICommunicator, ISystemWithLoopTask
    {
        event EventHandler<ConnectStateChangedEventArgs> ConnectStateChanged;

        string mRemoteIpPort { get; }
        bool mIsConnected { get; }

        void Connect();
        void Disconnect();
    }
}
