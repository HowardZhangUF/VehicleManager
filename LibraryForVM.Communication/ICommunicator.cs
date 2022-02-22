using System;

namespace LibraryForVM
{
	public interface ICommunicator
    {
        event EventHandler<SentDataEventArgs> SentData;
        event EventHandler<ReceivedDataEventArgs> ReceivedData;

        void SendData(object Data);
    }
}
