using AsyncSocket;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.NewCommunication
{
    public class CommunicatorClientUsingiTSSerialData : SystemWithLoopTask, ICommunicatorClient
    {
        public event EventHandler<ConnectStateChangedEventArgs> ConnectStateChanged;
        public event EventHandler<SentDataEventArgs> SentData;
        public event EventHandler<ReceivedDataEventArgs> ReceivedData;

        public string mRemoteIpPort { get; private set; } = "127.0.0.1:1025";
        public bool mIsConnected { get { return mSerialClient.ConnectStatus == EConnectStatus.Connect ? true : false; } }

        private SerialClient mSerialClient = null;
        private readonly Queue<ReceivedSerialDataEventArgs> mSerialClientReceivedSerialDataEventArgs = new Queue<ReceivedSerialDataEventArgs>();
        private readonly object mLockOfSerialClientReceivedSerialDataEventArgs = new object();

        public CommunicatorClientUsingiTSSerialData()
        {
            if (mSerialClient == null)
            {
                mSerialClient = new SerialClient();
                SubscribeEvent_SerialClient(mSerialClient);
            }
        }
        ~CommunicatorClientUsingiTSSerialData()
        {
            if (mSerialClient != null)
            {
                UnsubscribeEvent_SerialClient(mSerialClient);
                mSerialClient = null;
            }
        }
        public void Connect()
        {
            if (mSerialClient.ConnectStatus == EConnectStatus.Disconnect)
            {
                int seperatorIndex = mRemoteIpPort.IndexOf(":");
                string ip = mRemoteIpPort.Substring(0, seperatorIndex);
                int port = int.Parse(mRemoteIpPort.Substring(seperatorIndex + 1));
                mSerialClient.Connect(ip, port);
            }
        }
        public void Disconnect()
        {
            if (mSerialClient.ConnectStatus == EConnectStatus.Connect)
            {
                mSerialClient.Disconnect();
            }
        }
        public void SendData(object Data)
        {
            if (mSerialClient.ConnectStatus == EConnectStatus.Connect)
            {
                if (Data is Serializable)
                {
                    mSerialClient.Send(Data as Serializable);
                    RaiseEvent_SentData(mRemoteIpPort, Data);
                }
            }
        }
        public override string GetConfig(string ConfigName)
        {
            switch (ConfigName)
            {
                case "TimePeriod":
                    return mTimePeriod.ToString();
                case "RemoteIpPort":
                    return mRemoteIpPort;
                default:
                    return null;
            }
        }
        public override void SetConfig(string ConfigName, string NewValue)
        {
            switch (ConfigName)
            {
                case "TimePeriod":
                    mTimePeriod = int.Parse(NewValue);
                    RaiseEvent_ConfigUpdated(ConfigName, NewValue);
                    break;
                case "RemoteIpPort":
                    mRemoteIpPort = NewValue;
                    RaiseEvent_ConfigUpdated(ConfigName, NewValue);
                    break;
                default:
                    break;
            }
        }
        public override void Task()
        {
            Subtask_HandleSerialClientReceivedSerialDataEvents();
        }

        protected void SubscribeEvent_SerialClient(SerialClient SerialClient)
        {
            if (SerialClient != null)
            {
                SerialClient.ConnectStatusChangedEvent += HandleEvent_SerialClientConnectStatusChangedEvent;
                SerialClient.ReceivedSerialDataEvent += HandleEvent_SerialClientReceivedSerialDataEvent;
            }
        }
        protected void UnsubscribeEvent_SerialClient(SerialClient SerialClient)
        {
            if (SerialClient != null)
            {
                SerialClient.ConnectStatusChangedEvent -= HandleEvent_SerialClientConnectStatusChangedEvent;
                SerialClient.ReceivedSerialDataEvent -= HandleEvent_SerialClientReceivedSerialDataEvent;
            }
        }
        protected virtual void RaiseEvent_ConnectStateChanged(string IpPort, bool IsConnected, bool Sync = true)
        {
            if (Sync)
            {
                ConnectStateChanged?.Invoke(this, new ConnectStateChangedEventArgs(DateTime.Now, IpPort, IsConnected));
            }
            else
            {
                System.Threading.Tasks.Task.Run(() => { ConnectStateChanged?.Invoke(this, new ConnectStateChangedEventArgs(DateTime.Now, IpPort, IsConnected)); });
            }
        }
        protected virtual void RaiseEvent_SentData(string IpPort, object Data, bool Sync = true)
        {
            if (Sync)
            {
                SentData?.Invoke(this, new SentDataEventArgs(DateTime.Now, IpPort, Data));
            }
            else
            {
                System.Threading.Tasks.Task.Run(() => { SentData?.Invoke(this, new SentDataEventArgs(DateTime.Now, IpPort, Data)); });
            }
        }
        protected virtual void RaiseEvent_ReceivedData(string IpPort, object Data, bool Sync = true)
        {
            if (Sync)
            {
                ReceivedData?.Invoke(this, new ReceivedDataEventArgs(DateTime.Now, IpPort, Data));
            }
            else
            {
                System.Threading.Tasks.Task.Run(() => { ReceivedData?.Invoke(this, new ReceivedDataEventArgs(DateTime.Now, IpPort, Data)); });
            }
        }
        protected void Subtask_HandleSerialClientReceivedSerialDataEvents()
        {
            List<ReceivedSerialDataEventArgs> events = null;
            lock (mLockOfSerialClientReceivedSerialDataEventArgs)
            {
                if (mSerialClientReceivedSerialDataEventArgs.Count > 0)
                {
                    events = mSerialClientReceivedSerialDataEventArgs.ToList();
                    mSerialClientReceivedSerialDataEventArgs.Clear();
                }
            }

            if (events != null && events.Count > 0)
            {
                for (int i = 0; i < events.Count; ++i)
                {
                    RaiseEvent_ReceivedData(events[i].RemoteInfo.ToString(), events[i].Data);
                    System.Threading.Thread.Sleep(5);
                }
            }
        }

        private void HandleEvent_SerialClientConnectStatusChangedEvent(object sender, ConnectStatusChangedEventArgs e)
        {
            RaiseEvent_ConnectStateChanged(e.RemoteInfo.ToString(), e.ConnectStatus == EConnectStatus.Connect ? true : false);
        }
        private void HandleEvent_SerialClientReceivedSerialDataEvent(object sender, ReceivedSerialDataEventArgs e)
        {
            lock (mLockOfSerialClientReceivedSerialDataEventArgs)
            {
                mSerialClientReceivedSerialDataEventArgs.Enqueue(e);
            }
        }
    }
}
