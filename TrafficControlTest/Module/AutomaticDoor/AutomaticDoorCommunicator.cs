using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.NewCommunication;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public class AutomaticDoorCommunicator : SystemWithLoopTask, IAutomaticDoorCommunicator
	{
		public event EventHandler<ClientAddedEventArgs> ClientAdded;
		public event EventHandler<ClientRemovedEventArgs> ClientRemoved;
		public event EventHandler<ConnectStateChangedEventArgs> RemoteConnectStateChanged;
		public event EventHandler<SentDataEventArgs> SentData;
		public event EventHandler<ReceivedDataEventArgs> ReceivedData;

		private Dictionary<string, ICommunicatorClient> mClients = new Dictionary<string, ICommunicatorClient>();
		private bool mAutoConnect = false;
		
		public List<string> GetClientList()
		{
			if (mClients.Any())
			{
				return mClients.Keys.ToList();
			}
			else
			{
				return null;
			}
		}
		public bool IsConnected(string IpPort)
		{
			if (mClients.Keys.Contains(IpPort))
			{
				return mClients[IpPort].mIsConnected;
			}
			else
			{
				return false;
			}
		}
		public void Add(string IpPort)
		{
            if (!mClients.Keys.Contains(IpPort))
            {
                mClients.Add(IpPort, new CommunicatorClientUsingString());
                mClients[IpPort].ConnectStateChanged += HandleEvent_ICommunicatorClientUsingStringConnectStateChanged;
                mClients[IpPort].SentData += HandleEvent_ICommunicatorClientSentData;
                mClients[IpPort].ReceivedData += HandleEvent_ICommunicatorClientReceivedData;
                mClients[IpPort].SetConfig("RemoteIpPort", IpPort);
				mClients[IpPort].Start();
				RaiseEvent_ClientAdded(IpPort);
			}
		}
		public void Remove(string IpPort)
		{
			if (mClients.Keys.Contains(IpPort))
			{
				// 底層的 Disconnect 動作非預期。若未取消訂閱事件就 Disconnect 的話，會發生系統在處理事件的執行緒裡面卡住。
				// 為避免此不明原因的錯誤，決定先取消訂閱事件在執行 Disconnect 動作。
				mClients[IpPort].ConnectStateChanged -= HandleEvent_ICommunicatorClientUsingStringConnectStateChanged;
				mClients[IpPort].SentData -= HandleEvent_ICommunicatorClientSentData;
				mClients[IpPort].ReceivedData -= HandleEvent_ICommunicatorClientReceivedData;
				if (mClients[IpPort].mIsConnected) mClients[IpPort].Disconnect();
				mClients[IpPort].Stop();
				mClients.Remove(IpPort);
				RaiseEvent_ClientRemoved(IpPort);
			}
		}
		public void RemoveAll()
		{
			while (mClients.Any())
			{
				Remove(mClients.Keys.First());
			}
		}
		public void Connect(string IpPort)
		{
			if (mClients.Keys.Contains(IpPort) && !mClients[IpPort].mIsConnected)
			{
				mClients[IpPort].Connect();
			}
		}
		public void Disconnect(string IpPort)
		{
			if (mClients.Keys.Contains(IpPort) && mClients[IpPort].mIsConnected)
			{
				mClients[IpPort].Disconnect();
			}
		}
		public void SendData(string IpPort, string Data)
		{
			if (mClients.Keys.Contains(IpPort) && mClients[IpPort].mIsConnected)
			{
				mClients[IpPort].SendData(Data);
			}
		}
		public override string GetConfig(string ConfigName)
		{
			switch (ConfigName)
			{
				case "TimePeriod":
					return mTimePeriod.ToString();
				case "AutoConnect":
					return mAutoConnect.ToString();
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
				case "AutoConnect":
					mAutoConnect = bool.Parse(NewValue);
					RaiseEvent_ConfigUpdated(ConfigName, NewValue);
					break;
				default:
					break;
			}
		}
		public override void Task()
		{
			Subtask_AutoConnect();
		}
		
		protected virtual void RaiseEvent_ClientAdded(string IpPort, bool Sync = true)
		{
			if (Sync)
			{
				ClientAdded?.Invoke(this, new ClientAddedEventArgs(DateTime.Now, IpPort));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { ClientAdded?.Invoke(this, new ClientAddedEventArgs(DateTime.Now, IpPort)); });
			}
		}
		protected virtual void RaiseEvent_ClientRemoved(string IpPort, bool Sync = true)
		{
			if (Sync)
			{
				ClientRemoved?.Invoke(this, new ClientRemovedEventArgs(DateTime.Now, IpPort));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { ClientRemoved?.Invoke(this, new ClientRemovedEventArgs(DateTime.Now, IpPort)); });
			}
		}
		protected virtual void RaiseEvent_RemoteConnectStateChanged(string IpPort, bool Connected, bool Sync = true)
		{
			if (Sync)
			{
				RemoteConnectStateChanged?.Invoke(this, new ConnectStateChangedEventArgs(DateTime.Now, IpPort, Connected));
			}
			else
			{
				System.Threading.Tasks.Task.Run(() => { RemoteConnectStateChanged?.Invoke(this, new ConnectStateChangedEventArgs(DateTime.Now, IpPort, Connected)); });
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

		private void HandleEvent_ICommunicatorClientUsingStringConnectStateChanged(object sender, ConnectStateChangedEventArgs e)
		{
			RaiseEvent_RemoteConnectStateChanged(e.IpPort, e.IsConnected);
		}
		private void HandleEvent_ICommunicatorClientSentData(object sender, SentDataEventArgs e)
		{
			RaiseEvent_SentData(e.IpPort, e.Data);
		}
		private void HandleEvent_ICommunicatorClientReceivedData(object sender, ReceivedDataEventArgs e)
		{
			RaiseEvent_ReceivedData(e.IpPort, e.Data);
		}
		private void Subtask_AutoConnect()
		{
			if (mAutoConnect)
			{
				if (mClients.Any())
				{
					foreach (string ipPort in mClients.Keys)
					{
						if (!mClients[ipPort].mIsConnected)
						{
							mClients[ipPort].Connect();
						}
					}
				}
			}
		}
	}
}
