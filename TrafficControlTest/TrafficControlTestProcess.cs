using AsyncSocket;
using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrafficControlTest
{
	class TrafficControlTestProcess
	{
		public const string TIME_FORMAT_DETAIL = "yyyy/MM/dd HH:mm:ss.fff";

		public TrafficControlTestProcess()
		{
			SubscribeAGVInfoManagerEvent();
			SubscribeAGVMonitorEvent();
		}

		~TrafficControlTestProcess()
		{
			UnsubscribeAGVInfoManagerEvent();
			UnsubscribeAGVMonitorEvent();
		}

		public event AGVInfoManager.AGVAddedEventHandler AGVAdded;

		public event AGVInfoManager.AGVRemovedEventHandler AGVRemoved;

		public event AGVInfoManager.AGVStatusUpdatedEventHandler AGVStatusUpdated;

		public event AGVInfoManager.AGVPathUpdateEventHandler AGVPathUpdated;

		#region AGV 資訊管理

		private AGVInfoManager AGVInfoManager = new AGVInfoManager();

		public bool DisplayAGVInfoManagerDebugMessage = true;

		/// <summary>訂閱 AGVInfoManager 事件</summary>
		public void SubscribeAGVInfoManagerEvent()
		{
			AGVInfoManager.AGVAdded += AGVInfoManager_AGVAdded;
			AGVInfoManager.AGVRemoved += AGVInfoManager_AGVRemoved;
			AGVInfoManager.AGVStatusUpdated += AGVInfoManager_AGVStatusUpdated;
			AGVInfoManager.AGVPathUpdated += AGVInfoManager_AGVPathUpdated;
		}

		/// <summary>取消訂閱 AGVInfoManager 事件</summary>
		public void UnsubscribeAGVInfoManagerEvent()
		{
			AGVInfoManager.AGVAdded -= AGVInfoManager_AGVAdded;
			AGVInfoManager.AGVRemoved -= AGVInfoManager_AGVRemoved;
			AGVInfoManager.AGVStatusUpdated -= AGVInfoManager_AGVStatusUpdated;
			AGVInfoManager.AGVPathUpdated -= AGVInfoManager_AGVPathUpdated;
		}

		/// <summary>查詢 AGV 是否存在</summary>
		public bool IsAGVExistByName(string agvName)
		{
			return AGVInfoManager.IsExistByName(agvName);
		}

		/// <summary>查詢 AGV 是否存在</summary>
		public bool IsAGVExistByIPPort(string ipPort)
		{
			return AGVInfoManager.IsExistByIPPort(ipPort);
		}

		/// <summary>取得指定 AGVInfo</summary>
		public AGVInfo GetAGVInfoByName(string agvName)
		{
			return AGVInfoManager.GetAGVInfoByName(agvName);
		}

		/// <summary>取得指定 AGVInfo</summary>
		public AGVInfo GetAGVInfoByIPPort(string ipPort)
		{
			return AGVInfoManager.GetAGVInfoByIPPort(ipPort);
		}

		/// <summary>取得 AGV 的名字清單</summary>
		public string[] GetAGVNames()
		{
			return AGVInfoManager.GetAGVNames();
		}

		/// <summary>設定 AGV 狀態</summary>
		private void SetAGVInfo(EndPointInfo remoteInfo, AGVStatus data)
		{
			AGVInfoManager.SetAGVInfo(remoteInfo.ToString(), data);
		}

		/// <summary>設定 AGV 路徑</summary>
		private void SetAGVInfo(EndPointInfo remoteInfo, AGVPath data)
		{
			AGVInfoManager.SetAGVInfo(remoteInfo.ToString(), data);
		}

		/// <summary>移除指定 AGVInfo</summary>
		private void RemoveAGVInfoByName(string agvName)
		{
			AGVInfoManager.RemoveAGVInfoByName(agvName);
		}

		/// <summary>移除指定 AGVInfo</summary>
		private void RemoveAGVInfoByIPPort(string ipPort)
		{
			AGVInfoManager.RemoveAGVInfoByIPPort(ipPort);
		}

		private void AGVInfoManager_AGVAdded(DateTime occurTime, string agvName, string ipPort, AGVInfo agvInfo)
		{
			if (DisplayAGVInfoManagerDebugMessage) Console.WriteLine(occurTime.ToString(TIME_FORMAT_DETAIL) + $" - AGV Added - Name: {agvName} IPPort: {ipPort}");
			AGVAdded?.Invoke(occurTime, agvName, ipPort, agvInfo);
		}

		private void AGVInfoManager_AGVRemoved(DateTime occurTime, string agvName, string ipPort, AGVInfo agvInfo)
		{
			if (DisplayAGVInfoManagerDebugMessage) Console.WriteLine(occurTime.ToString(TIME_FORMAT_DETAIL) + $" - AGV Removed - Name: {agvName} IPPort: {ipPort}");
			AGVRemoved?.Invoke(occurTime, agvName, ipPort, agvInfo);
		}

		private void AGVInfoManager_AGVStatusUpdated(DateTime occurTime, string agvName, string ipPort, AGVInfo agvInfo)
		{
			if (DisplayAGVInfoManagerDebugMessage) Console.WriteLine(occurTime.ToString(TIME_FORMAT_DETAIL) + $" - AGV Status Updated - Name: {agvName} IPPort: {ipPort}");
			AGVStatusUpdated?.Invoke(occurTime, agvName, ipPort, agvInfo);
		}

		private void AGVInfoManager_AGVPathUpdated(DateTime occurTime, string agvName, string ipPort, AGVInfo agvInfo)
		{
			if (DisplayAGVInfoManagerDebugMessage) Console.WriteLine(occurTime.ToString(TIME_FORMAT_DETAIL) + $" - AGV Path Updated - Name: {agvName} IPPort: {ipPort}");
			AGVPathUpdated?.Invoke(occurTime, agvName, ipPort, agvInfo);
		}

		#endregion

		#region AGV 監視器

		private AGVMonitor AGVMonitor = new AGVMonitor();

		public bool DisplayAGVMonitorDebugMessage = true;

		/// <summary>開始監控</summary>
		public void StartAGVMonitor(int port)
		{
			AGVMonitor.Start(port);
		}

		/// <summary>停止監控</summary>
		public void StopAGVMonitor()
		{
			AGVMonitor.Stop();
		}

		/// <summary>訂閱 AGVMonitor 事件</summary>
		public void SubscribeAGVMonitorEvent()
		{
			AGVMonitor.AGVMonitorStarted += AGVMonitor_AGVMonitorStarted;
			AGVMonitor.AGVMonitorStopped += AGVMonitor_AGVMonitorStopped;
			AGVMonitor.AGVConnectStatusChanged += AGVMonitor_AGVConnectStatusChanged;
			AGVMonitor.AGVMonitorListenStatusChanged += AGVMonitor_AGVMonitorListenStatusChanged;
			AGVMonitor.ReceivedAGVStatusData += AGVMonitor_ReceivedAGVStatusData;
			AGVMonitor.ReceivedAGVPathData += AGVMonitor_ReceivedAGVPathData;
			AGVMonitor.ReceivedRequestMapListData += AGVMonitor_ReceivedRequestMapListData;
			AGVMonitor.ReceivedGetMapData += AGVMonitor_ReceivedGetMapData;
			AGVMonitor.ReceivedUploadMapToAGVData += AGVMonitor_ReceivedUploadMapToAGVData;
			AGVMonitor.ReceivedChangeMapData += AGVMonitor_ReceivedChangeMapData;
		}

		/// <summary>取消訂閱 AGVMonitor 事件</summary>
		public void UnsubscribeAGVMonitorEvent()
		{
			AGVMonitor.AGVMonitorStarted -= AGVMonitor_AGVMonitorStarted;
			AGVMonitor.AGVMonitorStopped -= AGVMonitor_AGVMonitorStopped;
			AGVMonitor.AGVConnectStatusChanged -= AGVMonitor_AGVConnectStatusChanged;
			AGVMonitor.AGVMonitorListenStatusChanged -= AGVMonitor_AGVMonitorListenStatusChanged;
			AGVMonitor.ReceivedAGVStatusData -= AGVMonitor_ReceivedAGVStatusData;
			AGVMonitor.ReceivedAGVPathData -= AGVMonitor_ReceivedAGVPathData;
			AGVMonitor.ReceivedRequestMapListData -= AGVMonitor_ReceivedRequestMapListData;
			AGVMonitor.ReceivedGetMapData -= AGVMonitor_ReceivedGetMapData;
			AGVMonitor.ReceivedUploadMapToAGVData -= AGVMonitor_ReceivedUploadMapToAGVData;
			AGVMonitor.ReceivedChangeMapData -= AGVMonitor_ReceivedChangeMapData;
		}

		/// <summary>向指定 AGV 傳送要求地圖清單的命令</summary>
		public void SendRequestMapListCommand(string agvName)
		{
			if (IsAGVExistByName(agvName))
			{
				AGVMonitor.SendRequestMapListCommand(GetAGVInfoByName(agvName).IPPort);
			}
		}

		/// <summary>向指定 AGV 傳送要求指定地圖的命令</summary>
		public void SendGetMapCommand(string agvName, string fileName)
		{
			if (IsAGVExistByName(agvName))
			{
				AGVMonitor.SendGetMapCommand(GetAGVInfoByName(agvName).IPPort, fileName);
			}
		}

		/// <summary>向指定 AGV 傳送上傳指定地圖的命令</summary>
		public void SendUploadMapCommand(string agvName, string fileName)
		{
			if (IsAGVExistByName(agvName))
			{
				AGVMonitor.SendUploadMapCommand(GetAGVInfoByName(agvName).IPPort, fileName);
			}
		}

		/// <summary>向指定 AGV 傳送使用指定地圖的命令</summary>
		public void SendChangeMapCommand(string agvName, string fileName)
		{
			if (IsAGVExistByName(agvName))
			{
				AGVMonitor.SendChangeMapCommand(GetAGVInfoByName(agvName).IPPort, fileName);
			}
		}

		private void AGVMonitor_AGVMonitorStarted(DateTime occurTime)
		{
			if (DisplayAGVMonitorDebugMessage) Console.WriteLine(occurTime.ToString(TIME_FORMAT_DETAIL) + " - AGV Monitor Started.");
		}

		private void AGVMonitor_AGVMonitorStopped(DateTime occurTime)
		{
			if (DisplayAGVMonitorDebugMessage) Console.WriteLine(occurTime.ToString(TIME_FORMAT_DETAIL) + " - AGV Monitor Stopped.");
		}

		private void AGVMonitor_AGVConnectStatusChanged(DateTime occurTime, EndPointInfo remoteInfo, EConnectStatus newStatus)
		{
			if (DisplayAGVMonitorDebugMessage) Console.WriteLine(occurTime.ToString(TIME_FORMAT_DETAIL) + $" - {remoteInfo.ToString()} - Connect Status Change to: {newStatus.ToString()}.");
			if (newStatus == EConnectStatus.Disconnect)
			{
				if (IsAGVExistByIPPort(remoteInfo.ToString()))
				{
					RemoveAGVInfoByIPPort(remoteInfo.ToString());
				}
			}
		}

		private void AGVMonitor_AGVMonitorListenStatusChanged(DateTime occurTime, EListenStatus newStatus)
		{
			if (DisplayAGVMonitorDebugMessage) Console.WriteLine(occurTime.ToString(TIME_FORMAT_DETAIL) + $" - AGV Monitor - Listen Status Change to: {newStatus}.");
		}

		private void AGVMonitor_ReceivedAGVStatusData(DateTime receivedTime, EndPointInfo remoteInfo, AGVStatus data)
		{
			if (DisplayAGVMonitorDebugMessage) Console.WriteLine(receivedTime.ToString(TIME_FORMAT_DETAIL) + $" - Received - {remoteInfo.ToString()} - AGVStatus.");
			SetAGVInfo(remoteInfo, data);
		}

		private void AGVMonitor_ReceivedAGVPathData(DateTime receivedTime, EndPointInfo remoteInfo, AGVPath data)
		{
			if (DisplayAGVMonitorDebugMessage) Console.WriteLine(receivedTime.ToString(TIME_FORMAT_DETAIL) + $" - Received - {remoteInfo.ToString()} - AGVPath.");
			SetAGVInfo(remoteInfo, data);
		}

		private void AGVMonitor_ReceivedRequestMapListData(DateTime receivedTime, EndPointInfo remoteInfo, RequestMapList data)
		{
			if (DisplayAGVMonitorDebugMessage) Console.WriteLine(receivedTime.ToString(TIME_FORMAT_DETAIL) + $" - Received - {remoteInfo.ToString()} - RequestMapList.");
		}

		private void AGVMonitor_ReceivedGetMapData(DateTime receivedTime, EndPointInfo remoteInfo, GetMap data)
		{
			if (DisplayAGVMonitorDebugMessage) Console.WriteLine(receivedTime.ToString(TIME_FORMAT_DETAIL) + $" - Received - {remoteInfo.ToString()} - GetMap.");
		}

		private void AGVMonitor_ReceivedUploadMapToAGVData(DateTime receivedTime, EndPointInfo remoteInfo, UploadMapToAGV data)
		{
			if (DisplayAGVMonitorDebugMessage) Console.WriteLine(receivedTime.ToString(TIME_FORMAT_DETAIL) + $" - Received - {remoteInfo.ToString()} - UploadMapToAGV.");
		}

		private void AGVMonitor_ReceivedChangeMapData(DateTime receivedTime, EndPointInfo remoteInfo, ChangeMap data)
		{
			if (DisplayAGVMonitorDebugMessage) Console.WriteLine(receivedTime.ToString(TIME_FORMAT_DETAIL) + $" - Received - {remoteInfo.ToString()} - ChangeMap.");
		}

		#endregion
	}
}
