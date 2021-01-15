using AsyncSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.NewCommunication
{
    public class CommunicatorServerUsingString : SystemWithLoopTask, ICommunicatorServer
    {
        public event EventHandler<ListenStateChangedEventArgs> LocalListenStateChanged;
        public event EventHandler<ConnectStateChangedEventArgs> RemoteConnectStateChanged;
        public event EventHandler<SentDataEventArgs> SentData;
        public event EventHandler<ReceivedDataEventArgs> ReceivedData;

        public int mLocalPort { get; private set; } = 1025;
        public bool mIsListened { get { return mServer.ListenStatus == EListenStatus.Listening ? true : false; } }
        public string[] mClientIpPorts { get { return mServer.ClientDictionary.Keys.ToArray(); } }

        protected Server mServer = null;
        protected readonly Queue<AsyncSocket.ReceivedDataEventArgs> mServerReceivedDataEventArgs = new Queue<AsyncSocket.ReceivedDataEventArgs>();
        protected readonly object mLockOfServerReceivedDataEventArgs = new object();

        public CommunicatorServerUsingString()
        {
            if (mServer == null)
            {
                mServer = new Server();
                SubscribeEvent_Server(mServer);
            }
        }
        ~CommunicatorServerUsingString()
        {
            if (mServer != null)
            {
                UnsubscribeEvent_Server(mServer);
                mServer = null;
            }
        }
        public void StartListen()
        {
            if (mServer.ListenStatus == EListenStatus.Idle)
            {
                mServer.StartListening(mLocalPort);
            }
        }
        public void StopListen()
        {
            if (mServer.ListenStatus == EListenStatus.Listening)
            {
                mServer.StopListen();
            }
        }
        public virtual void SendData(string IpPort, object Data)
        {
            if (mServer.ClientDictionary.Keys.Contains(IpPort))
            {
                if (Data is string)
                {
                    mServer.Send(IpPort, Data as string);
                    RaiseEvent_SentData(IpPort, Data);
                }
            }
        }
        public virtual void SendData(object Data)
        {
            foreach (string ipPort in mServer.ClientDictionary.Keys.ToArray())
            {
                SendData(ipPort, Data);
            }
		}
		public override string[] GetConfigNameList()
		{
			return new string[] { "TimePeriod", "LocalPort" };
		}
		public override string GetConfig(string ConfigName)
        {
            switch (ConfigName)
            {
                case "TimePeriod":
                    return mTimePeriod.ToString();
                case "LocalPort":
                    return mLocalPort.ToString();
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
                case "LocalPort":
                    mLocalPort = int.Parse(NewValue);
                    RaiseEvent_ConfigUpdated(ConfigName, NewValue);
                    break;
                default:
                    break;
            }
		}
		public override string GetSystemInfo()
		{
			return $"LocalPort: {mLocalPort}, IsListened: {mIsListened.ToString()}, CountOfReceivedData: {mServerReceivedDataEventArgs.Count}";
		}
		public override void Task()
        {
            Subtask_HandleServerReceivedDataEventArgs();
        }

        protected virtual void SubscribeEvent_Server(Server Server)
        {
            if (Server != null)
            {
                Server.ListenStatusChangedEvent += HandleEvent_ServerListenStatusChangedEvent;
                Server.ConnectStatusChangedEvent += HandleEvent_ServerConnectStatusChangedEvent;
                Server.ReceivedDataEvent += HandleEvent_ServerReceivedDataEvent;
            }
        }
        protected virtual void UnsubscribeEvent_Server(Server Server)
        {
            if (Server != null)
            {
                Server.ListenStatusChangedEvent -= HandleEvent_ServerListenStatusChangedEvent;
                Server.ConnectStatusChangedEvent -= HandleEvent_ServerConnectStatusChangedEvent;
                Server.ReceivedDataEvent -= HandleEvent_ServerReceivedDataEvent;
            }
        }
        protected virtual void RaiseEvent_LocalListenStateChanged(int Port, bool IsListened, bool Sync = true)
        {
            if (Sync)
            {
                LocalListenStateChanged?.Invoke(this, new ListenStateChangedEventArgs(DateTime.Now, Port, IsListened));
            }
            else
            {
                System.Threading.Tasks.Task.Run(() => { LocalListenStateChanged?.Invoke(this, new ListenStateChangedEventArgs(DateTime.Now, Port, IsListened)); });
            }
        }
        protected virtual void RaiseEvent_RemoteConnectStateChanged(string IpPort, bool IsConnected, bool Sync = true)
        {
            if (Sync)
            {
                RemoteConnectStateChanged?.Invoke(this, new ConnectStateChangedEventArgs(DateTime.Now, IpPort, IsConnected));
            }
            else
            {
                System.Threading.Tasks.Task.Run(() => { RemoteConnectStateChanged?.Invoke(this, new ConnectStateChangedEventArgs(DateTime.Now, IpPort, IsConnected)); });
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
        protected void Subtask_HandleServerReceivedDataEventArgs()
        {
            List<AsyncSocket.ReceivedDataEventArgs> events = null;
            lock (mLockOfServerReceivedDataEventArgs)
            {
                if (mServerReceivedDataEventArgs.Count > 0)
                {
                    events = mServerReceivedDataEventArgs.ToList();
                    mServerReceivedDataEventArgs.Clear();
                }
            }

            if (events != null && events.Count > 0)
            {
                for (int i = 0; i < events.Count; ++i)
                {
                    RaiseEvent_ReceivedData(events[i].RemoteInfo.ToString(), Encoding.Default.GetString(events[i].Data));
                }
            }
        }

        private void HandleEvent_ServerListenStatusChangedEvent(object sender, ListenStatusChangedEventArgs e)
        {
            RaiseEvent_LocalListenStateChanged(mLocalPort, e.ListenStatus == EListenStatus.Listening ? true : false);
        }
        private void HandleEvent_ServerConnectStatusChangedEvent(object sender, ConnectStatusChangedEventArgs e)
        {
            RaiseEvent_RemoteConnectStateChanged(e.RemoteInfo.ToString(), e.ConnectStatus == EConnectStatus.Connect ? true : false);
        }
        private void HandleEvent_ServerReceivedDataEvent(object sender, AsyncSocket.ReceivedDataEventArgs e)
        {
            lock (mLockOfServerReceivedDataEventArgs)
            {
                mServerReceivedDataEventArgs.Enqueue(e);
            }
        }
    }
}
