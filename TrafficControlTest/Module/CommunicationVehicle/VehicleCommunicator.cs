using AsyncSocket;
using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TrafficControlTest.Library;
using TrafficControlTest.Module.NewCommunication;

namespace TrafficControlTest.Module.CommunicationVehicle
{
    public class VehicleCommunicator : CommunicatorServerUsingiTSSerialData, IVehicleCommunicator
    {
        public event EventHandler<SentDataEventArgs> SentDataSuccessed;
        public event EventHandler<SentDataEventArgs> SentDataFailed;

        public void SendDataAndWaitAck(string IpPort, object Data)
        {
            if (mSerialServer.ClientDictionary.Keys.Contains(IpPort))
            {
                if (Data is Serializable)
                {
                    mSerialServer.SendAndWaitAck(IpPort, Data as Serializable);
                    RaiseEvent_SentData(IpPort, Data);
                }
            }
        }
        public void SendDataOfGoto(string IpPort, string Target)
        {
            SendDataAndWaitAck(IpPort, new GoTo(Target));
        }
        public void SendDataOfGotoPoint(string IpPort, int X, int Y)
        {
            SendDataAndWaitAck(IpPort, new GoToPoint(new List<int> { X, Y }));
        }
        public void SendDataOfGotoTowardPoint(string IpPort, int X, int Y, int Toward)
        {
            SendDataAndWaitAck(IpPort, new GoToTowardPoint(new List<int> { X, Y, Toward }));
        }
        public void SendDataOfStop(string IpPort)
        {
            SendDataAndWaitAck(IpPort, new Stop(null));
        }
		public void SendDataOfCharge(string IpPort)
		{
			SendDataAndWaitAck(IpPort, new Charge(string.Empty));
		}
		public void SendDataOfUncharge(string IpPort)
		{
			SendDataAndWaitAck(IpPort, new Uncharge(null));
		}
        public void SendDataOfInsertMovingBuffer(string IpPort, int X, int Y)
        {
            SendDataAndWaitAck(IpPort, new InsertMovingBuffer(new List<int>() { X, Y }));
        }
        public void SendDataOfRemoveMovingBuffer(string IpPort)
        {
            SendDataAndWaitAck(IpPort, new RemoveMovingBuffer(null));
        }
        public void SendDataOfPauseMoving(string IpPort)
        {
            SendDataAndWaitAck(IpPort, new PauseMoving(null));
        }
        public void SendDataOfResumeMoving(string IpPort)
        {
            SendDataAndWaitAck(IpPort, new ResumeMoving(null));
        }
        public void SendDataOfRequestMapList(string IpPort)
        {
            SendDataAndWaitAck(IpPort, new RequestMapList(null));
        }
        public void SendDataOfGetMap(string IpPort, string MapName)
        {
            // 與 iTS 通訊時， GetMap 帶的參數要移除副檔名
            SendDataAndWaitAck(IpPort, new GetMap(MapName.Replace(".map", string.Empty)));
        }
        public void SendDataOfUploadMapToAGV(string IpPort, string MapPath)
        {
            if (System.IO.File.Exists(MapPath)) SendDataAndWaitAck(IpPort, new UploadMapToAGV(new FileInfo(MapPath)));
        }
        public void SendDataOfChangeMap(string IpPort, string MapName)
        {
            // 與 iTS 通訊時， ChangeMap 帶的參數要移除副檔名
            SendDataAndWaitAck(IpPort, new ChangeMap(MapName.Replace(".map", string.Empty)));
        }

        protected override void SubscribeEvent_SerialServer(SerialServer SerialServer)
        {
            base.SubscribeEvent_SerialServer(SerialServer);
            if (SerialServer != null)
            {
                SerialServer.SentSerializableDataSuccessed += HandleEvent_SerialServerSentSerializableDataSuccessed;
                SerialServer.SentSerializableDataFailed += HandleEvent_SerialServerSentSerializableDataFailed;
            }
        }
        protected override void UnsubscribeEvent_SerialServer(SerialServer SerialServer)
        {
            base.UnsubscribeEvent_SerialServer(SerialServer);
            if (SerialServer != null)
            {
                SerialServer.SentSerializableDataSuccessed -= HandleEvent_SerialServerSentSerializableDataSuccessed;
                SerialServer.SentSerializableDataFailed -= HandleEvent_SerialServerSentSerializableDataFailed;
            }
        }
        protected virtual void RaiseEvent_SentDataSuccessed(string IpPort, object Data, bool Sync = true)
        {
            if (Sync)
            {
                SentDataSuccessed?.Invoke(this, new SentDataEventArgs(DateTime.Now, IpPort, Data));
            }
            else
            {
                System.Threading.Tasks.Task.Run(() => { SentDataSuccessed?.Invoke(this, new SentDataEventArgs(DateTime.Now, IpPort, Data)); });
            }
        }
        protected virtual void RaiseEvent_SentDataFailed(string IpPort, object Data, bool Sync = true)
        {
            if (Sync)
            {
                SentDataFailed?.Invoke(this, new SentDataEventArgs(DateTime.Now, IpPort, Data));
            }
            else
            {
                System.Threading.Tasks.Task.Run(() => { SentDataFailed?.Invoke(this, new SentDataEventArgs(DateTime.Now, IpPort, Data)); });
            }
        }

        private void HandleEvent_SerialServerSentSerializableDataSuccessed(object sender, SentSerializableDataSuccessedEventArgs e)
        {
            RaiseEvent_SentDataSuccessed(e.RemoteInfo.ToString(), e.Data);
        }
        private void HandleEvent_SerialServerSentSerializableDataFailed(object sender, SentSerializableDataFailedEventArgs e)
        {
            RaiseEvent_SentDataFailed(e.RemoteInfo.ToString(), e.Data);
        }
    }
}
