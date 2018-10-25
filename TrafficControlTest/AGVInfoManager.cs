using SerialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest
{
	/// <summary>AGV 資訊管理器</summary>
	class AGVInfoManager
	{
		public delegate void AGVAddedEventHandler(DateTime occurTime, string agvName, string ipPort, AGVInfo agvInfo);
		public event AGVAddedEventHandler AGVAdded;

		public delegate void AGVRemovedEventHandler(DateTime occurTime, string agvName, string ipPort, AGVInfo agvInfo);
		public event AGVRemovedEventHandler AGVRemoved;

		public delegate void AGVStatusUpdatedEventHandler(DateTime occurTime, string agvName, string ipPort, AGVInfo agvInfo);
		public event AGVStatusUpdatedEventHandler AGVStatusUpdated;

		public delegate void AGVPathUpdateEventHandler(DateTime occurTime, string agvName, string ipPort, AGVInfo agvInfo);
		public event AGVPathUpdateEventHandler AGVPathUpdated;

		/// <summary>AGV 資訊儲存器</summary>
		private Dictionary<string, AGVInfo> AGVInfos = new Dictionary<string, AGVInfo>();

		/// <summary>查詢 AGV 是否存在</summary>
		public bool IsExistByName(string agvName)
		{
			lock (AGVInfos)
			{ 
				return AGVInfos.Keys.Contains(agvName); 
			}
		}

		/// <summary>查詢 AGV 是否存在</summary>
		public bool IsExistByIPPort(string ipPort)
		{
			lock (AGVInfos)
			{
				foreach (AGVInfo agvInfo in AGVInfos.Values)
				{
					if (agvInfo.IPPort == ipPort)
						return true;
				}
				return false;
			}
		}

		/// <summary>取得指定的 AGVInfo</summary>
		public AGVInfo GetAGVInfoByName(string agvName)
		{
			lock (AGVInfos)
			{
				if (AGVInfos.Keys.Contains(agvName))
					return AGVInfos[agvName];
				else
					return null;
			}
		}

		/// <summary>取得指定的 AGVInfo</summary>
		public AGVInfo GetAGVInfoByIPPort(string ipPort)
		{
			lock (AGVInfos)
			{
				foreach (AGVInfo agvInfo in AGVInfos.Values)
				{
					if (agvInfo.IPPort == ipPort)
						return agvInfo;
				}
				return null;
			}
		}

		/// <summary>取得 AGV 的名字清單</summary>
		public string[] GetAGVNames()
		{
			lock (AGVInfos)
			{
				return AGVInfos.Keys.ToArray();
			}
		}

		/// <summary>設定 AGV 狀態</summary>
		public void SetAGVInfo(string ipPort, AGVStatus status)
		{
			lock (AGVInfos)
			{
				if (status != null)
				{
					if (AGVInfos.Keys.Contains(status.Name))
					{
						UpdateAGVStatus(status);
					}
					else
					{
						AddAGVInfo(ipPort, status);
					}
				}
			}
		}

		/// <summary>設定 AGV 路徑</summary>
		public void SetAGVInfo(string ipPort, AGVPath path)
		{
			lock (AGVInfos)
			{
				if (path != null)
				{
					if (AGVInfos.Keys.Contains(path.Name))
					{
						UpdateAGVPath(path);
					}
				}
			}
		}

		/// <summary>移除指定的 AGVInfo</summary>
		public void RemoveAGVInfoByName(string agvName)
		{
			lock (AGVInfos)
			{
				if (AGVInfos.Keys.Contains(agvName))
				{
					AGVInfo tmp = AGVInfos[agvName];
					AGVInfos.Remove(agvName);
					AGVRemoved?.Invoke(DateTime.Now, agvName, tmp.IPPort, tmp);
				}
			}
		}

		/// <summary>移除指定的 AGVInfo</summary>
		public void RemoveAGVInfoByIPPort(string ipPort)
		{
			lock (AGVInfos)
			{
				if (IsExistByIPPort(ipPort))
				{
					AGVInfo tmp = GetAGVInfoByIPPort(ipPort);
					AGVInfos.Remove(tmp.Status.Name);
					AGVRemoved?.Invoke(DateTime.Now, tmp.Status.Name, ipPort, tmp);
				}
			}
		}

		private void AddAGVInfo(string ipPort, AGVStatus status)
		{
			lock (AGVInfos)
			{
				if (status != null)
				{
					if (!AGVInfos.Keys.Contains(status.Name))
					{
						AGVInfo agvInfo = new AGVInfo();
						agvInfo.Status = status;
						agvInfo.IPPort = ipPort;
						AGVInfos.Add(agvInfo.Status.Name, agvInfo);
						AGVAdded?.Invoke(DateTime.Now, status.Name, ipPort, agvInfo);
					}
				}
			}
		}

		private void UpdateAGVStatus(AGVStatus status)
		{
			lock (AGVInfos)
			{
				if (status != null)
				{
					if (AGVInfos.Keys.Contains(status.Name))
					{
						AGVInfos[status.Name].Status = status;
						AGVStatusUpdated?.Invoke(DateTime.Now, status.Name, AGVInfos[status.Name].IPPort, AGVInfos[status.Name]);
					}
				}
			}
		}

		private void UpdateAGVPath(AGVPath path)
		{
			lock (AGVInfos)
			{
				if (path != null)
				{
					if (AGVInfos.Keys.Contains(path.Name))
					{
						AGVInfos[path.Name].Path = path;
						AGVPathUpdated?.Invoke(DateTime.Now, path.Name, AGVInfos[path.Name].IPPort, AGVInfos[path.Name]);
					}
				}
			}
		}
	}
}
