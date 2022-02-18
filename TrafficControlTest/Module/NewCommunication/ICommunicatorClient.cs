﻿using AsyncSocket;
using LibraryForVM;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.NewCommunication
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
