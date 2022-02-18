using LibraryForVM;
using System;

namespace TrafficControlTest.Module.CommunicationVehicle
{
	public interface IVehicleCommunicator : ICommunicatorServer
    {
        event EventHandler<SentDataEventArgs> SentDataSuccessed;
        event EventHandler<SentDataEventArgs> SentDataFailed;

        void SendDataAndWaitAck(string IpPort, object Data);
        void SendDataOfGoto(string IpPort, string Target);
        void SendDataOfGotoPoint(string IpPort, int X, int Y);
        void SendDataOfGotoTowardPoint(string IpPort, int X, int Y, int Toward);
        void SendDataOfStop(string IpPort);
		void SendDataOfCharge(string IpPort);
		void SendDataOfUncharge(string IpPort);
        void SendDataOfInsertMovingBuffer(string IpPort, int X, int Y);
        void SendDataOfRemoveMovingBuffer(string IpPort);
        void SendDataOfPauseMoving(string IpPort);
        void SendDataOfResumeMoving(string IpPort);
        void SendDataOfRequestMapList(string IpPort);
        void SendDataOfGetMap(string IpPort, string MapName);
        void SendDataOfUploadMapToAGV(string IpPort, string MapPath);
        void SendDataOfChangeMap(string IpPort, string MapName);
    }
}
