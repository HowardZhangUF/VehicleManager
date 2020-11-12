using System;
using System.Collections.Generic;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.NewCommunication;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Module.CommunicationVehicle
{
    public interface IVehicleCommunicator : ICommunicatorServer
    {
        event EventHandler<SentDataEventArgs> SentDataSuccessed;
        event EventHandler<SentDataEventArgs> SentDataFailed;

        void SendDataOfGoto(string IpPort, string Target);
        void SendDataOfGotoPoint(string IpPort, int X, int Y);
        void SendDataOfGotoTowardPoint(string IpPort, int X, int Y, int Toward);
        void SendDataOfDock(string IpPort);
        void SendDataOfStop(string IpPort);
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
