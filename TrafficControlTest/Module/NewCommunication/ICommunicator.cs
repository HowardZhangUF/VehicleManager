using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.NewCommunication
{
    public interface ICommunicator
    {
        event EventHandler<SentDataEventArgs> SentData;
        event EventHandler<ReceivedDataEventArgs> ReceivedData;

        void SendData(object Data);
    }
}
