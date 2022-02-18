using LibraryForVM;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Module.Map;

namespace TrafficControlTest.Module.AutomaticDoor
{
	public class AutomaticDoorInfoManagerUpdater : IAutomaticDoorInfoManagerUpdater
	{
		private IAutomaticDoorInfoManager rAutomaticDoorInfoManager = null;
		private IMapManager rMapManager = null;
		private IAutomaticDoorCommunicator rAutomaticDoorCommunicator = null;

		public AutomaticDoorInfoManagerUpdater(IAutomaticDoorInfoManager AutomaticDoorInfoManager, IMapManager MapManager, IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			Set(AutomaticDoorInfoManager, MapManager, AutomaticDoorCommunicator);
		}
		public void Set(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			UnsubscribeEvent_IAutomaticDoorInfoManager(rAutomaticDoorInfoManager);
			rAutomaticDoorInfoManager = AutomaticDoorInfoManager;
			SubscribeEvent_IAutomaticDoorInfoManager(rAutomaticDoorInfoManager);
		}
		public void Set(IMapManager MapManager)
		{
			UnsubscribeEvent_IMapManager(rMapManager);
			rMapManager = MapManager;
			SubscribeEvent_IMapManager(rMapManager);
		}
		public void Set(IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			UnsubscribeEvent_IAutomaticDoorCommunicator(rAutomaticDoorCommunicator);
			rAutomaticDoorCommunicator = AutomaticDoorCommunicator;
			SubscribeEvent_IAutomaticDoorCommunicator(rAutomaticDoorCommunicator);
		}
		public void Set(IAutomaticDoorInfoManager AutomaticDoorInfoManager, IMapManager MapManager, IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			Set(AutomaticDoorInfoManager);
			Set(MapManager);
			Set(AutomaticDoorCommunicator);
		}
		private void SubscribeEvent_IAutomaticDoorInfoManager(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			if (AutomaticDoorInfoManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorInfoManager(IAutomaticDoorInfoManager AutomaticDoorInfoManager)
		{
			if (AutomaticDoorInfoManager != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapChanged += HandleEvent_IMapManagerMapChanged;
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapChanged -= HandleEvent_IMapManagerMapChanged;
			}
		}
		private void SubscribeEvent_IAutomaticDoorCommunicator(IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			if (AutomaticDoorCommunicator != null)
			{
				AutomaticDoorCommunicator.RemoteConnectStateChanged += HandleEvent_IAutomaticDoorCommunicatorRemoteConnectStateChanged;
				AutomaticDoorCommunicator.SentData += HandleEvent_IAutomaticDoorCommunicatorSentData;
			}
		}
		private void UnsubscribeEvent_IAutomaticDoorCommunicator(IAutomaticDoorCommunicator AutomaticDoorCommunicator)
		{
			if (AutomaticDoorCommunicator != null)
			{
				AutomaticDoorCommunicator.RemoteConnectStateChanged -= HandleEvent_IAutomaticDoorCommunicatorRemoteConnectStateChanged;
				AutomaticDoorCommunicator.SentData -= HandleEvent_IAutomaticDoorCommunicatorSentData;
			}
		}
		private void HandleEvent_IMapManagerMapChanged(object sender, MapChangedEventArgs e)
		{
			rAutomaticDoorInfoManager.RemoveAll();
			List<IMapObjectOfRectangle> automaticDoorInfos = rMapManager.GetRectangleMapObjects(TypeOfMapObjectOfRectangle.AutomaticDoor);
			if (automaticDoorInfos != null && automaticDoorInfos.Count > 0)
			{
				for (int i = 0; i < automaticDoorInfos.Count; ++i)
				{
					string ipPort = string.Empty;
					if (automaticDoorInfos[i].mParameters.Any(o => o.StartsWith("IPPort=")))
					{
						ipPort = automaticDoorInfos[i].mParameters.First(o => o.StartsWith("IPPort=")).Replace("IPPort=", string.Empty);
					}
					IAutomaticDoorInfo automaticDoorInfo = new AutomaticDoorInfo(automaticDoorInfos[i].mName, automaticDoorInfos[i].mRange, ipPort);
					rAutomaticDoorInfoManager.Add(automaticDoorInfo.mName, automaticDoorInfo);
				}
			}
		}
		private void HandleEvent_IAutomaticDoorCommunicatorRemoteConnectStateChanged(object sender, ConnectStateChangedEventArgs e)
		{
			if (rAutomaticDoorInfoManager.IsExisByIpPortt(e.IpPort))
			{
				rAutomaticDoorInfoManager.GetItemByIpPort(e.IpPort).UpdateIsConnected(e.IsConnected);
			}
		}
		private void HandleEvent_IAutomaticDoorCommunicatorSentData(object sender, SentDataEventArgs e)
		{
			if (rAutomaticDoorInfoManager.IsExisByIpPortt(e.IpPort))
			{
				if ((e.Data as string).Contains("OpenDoor"))
				{
					rAutomaticDoorInfoManager.GetItemByIpPort(e.IpPort).UpdateState(AutomaticDoorState.Opened);
				}
				else if ((e.Data as string).Contains("CloseDoor"))
				{
					rAutomaticDoorInfoManager.GetItemByIpPort(e.IpPort).UpdateState(AutomaticDoorState.Closed);
				}
			}
		}
	}
}
