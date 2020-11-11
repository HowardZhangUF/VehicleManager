using AsyncSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.CommunicationHost
{
    public class HostCommunicator : NewCommunication.CommunicatorServerUsingString, IHostCommunicator
    {

    }
}
