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
    public class CommunicatorServerUsingiTSSerialData : SystemWithLoopTask, ICommunicatorServer
    {
        public event EventHandler<ListenStateChangedEventArgs> LocalListenStateChanged;
        public event EventHandler<ConnectStateChangedEventArgs> RemoteConnectStateChanged;
        public event EventHandler<SentDataEventArgs> SentData;
        public event EventHandler<ReceivedDataEventArgs> ReceivedData;

        public int mLocalPort { get; private set; } = 1025;
        public bool mIsListened { get { return mSerialServer.ListenStatus == EListenStatus.Listening ? true : false; } }
        public string[] mClientIpPorts { get { return mSerialServer.ClientDictionary.Keys.ToArray(); } }

        protected SerialServer mSerialServer = null;
        protected readonly Queue<ReceivedSerialDataEventArgs> mSerialServerReceivedSerialDataEventArgs = new Queue<ReceivedSerialDataEventArgs>();
        protected readonly object mLockOfSerialServerReceivedSerialDataEventArgs = new object();

        public CommunicatorServerUsingiTSSerialData()
        {
            if (mSerialServer == null)
            {
                mSerialServer = new SerialServer();
                SubscribeEvent_SerialServer(mSerialServer);
            }
        }
        ~CommunicatorServerUsingiTSSerialData()
        {
            if (mSerialServer != null)
            {
                UnsubscribeEvent_SerialServer(mSerialServer);
                mSerialServer = null;
            }
        }
        public void StartListen()
        {
            if (mSerialServer.ListenStatus == EListenStatus.Idle)
            {
                mSerialServer.StartListening(mLocalPort);
            }
        }
        public void StopListen()
        {
            if (mSerialServer.ListenStatus == EListenStatus.Listening)
            {
                mSerialServer.StopListen();
            }
        }
        public virtual void SendData(string IpPort, object Data)
        {
            if (mSerialServer.ClientDictionary.Keys.Contains(IpPort))
            {
                if (Data is Serializable)
                {
                    mSerialServer.Send(IpPort, Data as Serializable);
                    RaiseEvent_SentData(IpPort, Data);
                }
            }
        }
        public virtual void SendData(object Data)
        {
            foreach (string ipPort in mSerialServer.ClientDictionary.Keys.ToArray())
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
        public override void Task()
        {
            Subtask_HandleSerialServerReceivedSerialDataEventArgs();
        }

        protected virtual void SubscribeEvent_SerialServer(SerialServer SerialServer)
        {
            if (SerialServer != null)
            {
                SerialServer.ListenStatusChangedEvent += HandleEvent_SerialServerListenStatusChangedEvent;
                SerialServer.ConnectStatusChangedEvent += HandleEvent_SerialServerConnectStatusChangedEvent;
                SerialServer.ReceivedSerialDataEvent += HandleEvent_SerialServerReceivedSerialDataEvent;
            }
        }
        protected virtual void UnsubscribeEvent_SerialServer(SerialServer SerialServer)
        {
            if (SerialServer != null)
            {
                SerialServer.ListenStatusChangedEvent -= HandleEvent_SerialServerListenStatusChangedEvent;
                SerialServer.ConnectStatusChangedEvent -= HandleEvent_SerialServerConnectStatusChangedEvent;
                SerialServer.ReceivedSerialDataEvent -= HandleEvent_SerialServerReceivedSerialDataEvent;
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
        protected void Subtask_HandleSerialServerReceivedSerialDataEventArgs()
        {
            List<ReceivedSerialDataEventArgs> events = null;
            lock (mLockOfSerialServerReceivedSerialDataEventArgs)
            {
                if (mSerialServerReceivedSerialDataEventArgs.Count > 0)
                {
                    events = mSerialServerReceivedSerialDataEventArgs.ToList();
                    mSerialServerReceivedSerialDataEventArgs.Clear();
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

        private void HandleEvent_SerialServerListenStatusChangedEvent(object sender, ListenStatusChangedEventArgs e)
        {
            RaiseEvent_LocalListenStateChanged(mLocalPort, e.ListenStatus == EListenStatus.Listening ? true : false);
        }
        private void HandleEvent_SerialServerConnectStatusChangedEvent(object sender, ConnectStatusChangedEventArgs e)
        {
            RaiseEvent_RemoteConnectStateChanged(e.RemoteInfo.ToString(), e.ConnectStatus == EConnectStatus.Connect ? true : false);
        }
        private void HandleEvent_SerialServerReceivedSerialDataEvent(object sender, ReceivedSerialDataEventArgs e)
        {
            lock (mLockOfSerialServerReceivedSerialDataEventArgs)
            {
                mSerialServerReceivedSerialDataEventArgs.Enqueue(e);
            }
        }
    }
}
