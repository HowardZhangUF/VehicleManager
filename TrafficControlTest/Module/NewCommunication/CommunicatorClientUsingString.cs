using AsyncSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.NewCommunication
{
    public class CommunicatorClientUsingString : SystemWithLoopTask, ICommunicatorClient
    {
        public event EventHandler<ConnectStateChangedEventArgs> ConnectStateChanged;
        public event EventHandler<SentDataEventArgs> SentData;
        public event EventHandler<ReceivedDataEventArgs> ReceivedData;

        public string mRemoteIpPort { get; private set; } = "127.0.0.1:1025";
        public bool mIsConnected { get { return mClient.ConnectStatus == EConnectStatus.Connect ? true : false; } }

        private Client mClient = null;
        private readonly Queue<AsyncSocket.ReceivedDataEventArgs> mClientReceivedDataEventArgs = new Queue<AsyncSocket.ReceivedDataEventArgs>();
        private readonly object mLockOfClientReceivedDataEventArgs = new object();

        public CommunicatorClientUsingString()
        {
            if (mClient == null)
            {
                mClient = new Client();
                SubscribeEvent_Client(mClient);
            }
        }
        ~CommunicatorClientUsingString()
        {
            if (mClient != null)
            {
                UnsubscribeEvent_Client(mClient);
                mClient = null;
            }
        }
        public void Connect()
        {
            if (mClient.ConnectStatus == EConnectStatus.Disconnect)
            {
                int seperatorIndex = mRemoteIpPort.IndexOf(":");
                string ip = mRemoteIpPort.Substring(0, seperatorIndex);
                int port = int.Parse(mRemoteIpPort.Substring(seperatorIndex + 1));
                mClient.Connect(ip, port);
            }
        }
        public void Disconnect()
        {
            if (mClient.ConnectStatus == EConnectStatus.Connect)
            {
                mClient.Disconnect();
            }
        }
        public void SendData(object Data)
        {
            if (mClient.ConnectStatus == EConnectStatus.Connect)
            {
                if (Data is string)
                {
                    mClient.Send(Data as string);
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
            Subtask_HandleClientReceivedDataEvents();
        }

        protected void SubscribeEvent_Client(Client Client)
        {
            if (Client != null)
            {
                Client.ConnectStatusChangedEvent += HandleEvent_ClientConnectStatusChangedEvent;
                Client.ReceivedDataEvent += HandleEvent_ClientReceivedDataEvent;
            }
        }
        protected void UnsubscribeEvent_Client(Client Client)
        {
            if (Client != null)
            {
                Client.ConnectStatusChangedEvent -= HandleEvent_ClientConnectStatusChangedEvent;
                Client.ReceivedDataEvent -= HandleEvent_ClientReceivedDataEvent;
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
        protected void Subtask_HandleClientReceivedDataEvents()
        {
            List<AsyncSocket.ReceivedDataEventArgs> events = null;
            lock (mLockOfClientReceivedDataEventArgs)
            {
                if (mClientReceivedDataEventArgs.Count > 0)
                {
                    events = mClientReceivedDataEventArgs.ToList();
                    mClientReceivedDataEventArgs.Clear();
                }
            }

            if (events != null && events.Count > 0)
            {
                for (int i = 0; i < events.Count; ++i)
                {
                    RaiseEvent_ReceivedData(events[i].RemoteInfo.ToString(), Encoding.Default.GetString(events[i].Data));
                    System.Threading.Thread.Sleep(5);
                }
            }
        }

        private void HandleEvent_ClientConnectStatusChangedEvent(object sender, ConnectStatusChangedEventArgs e)
        {
            RaiseEvent_ConnectStateChanged(e.RemoteInfo.ToString(), e.ConnectStatus == EConnectStatus.Connect ? true : false);
        }
        private void HandleEvent_ClientReceivedDataEvent(object sender, AsyncSocket.ReceivedDataEventArgs e)
        {
            lock (mLockOfClientReceivedDataEventArgs)
            {
                mClientReceivedDataEventArgs.Enqueue(e);
            }
        }
    }
}
