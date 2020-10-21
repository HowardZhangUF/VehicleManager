using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;
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
				MapManager.LoadMapSuccessed += HandleEvent_IMapManagerLoadMapSuccessed;
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.LoadMapSuccessed -= HandleEvent_IMapManagerLoadMapSuccessed;
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
		private void HandleEvent_IMapManagerLoadMapSuccessed(object sender, LoadMapSuccessedEventArgs e)
		{
			rAutomaticDoorInfoManager.RemoveAll();
			string[] automaticDoorNames = rMapManager.GetAutomaticDoorAreaNameList();
			if (automaticDoorNames != null && automaticDoorNames.Length > 0)
			{
				for (int i = 0; i < automaticDoorNames.Length; ++i)
				{
					string[] automaticDoorInfo = rMapManager.GetAutomaticDoorAreaInfo(automaticDoorNames[i]); // xMax yMax xMin yMin ipport
					if (automaticDoorInfo != null && automaticDoorInfo.Length >= 5)
					{
						IPoint2D maxPoint = new Point2D(int.Parse(automaticDoorInfo[0]), int.Parse(automaticDoorInfo[1]));
						IPoint2D minPoint = new Point2D(int.Parse(automaticDoorInfo[2]), int.Parse(automaticDoorInfo[3]));
						IRectangle2D range = new Rectangle2D(maxPoint, minPoint);
						IAutomaticDoorInfo automaticDoor = new AutomaticDoorInfo(automaticDoorNames[i], range, automaticDoorInfo[4]);
						rAutomaticDoorInfoManager.Add(automaticDoor.mName, automaticDoor);
					}
				}
			}
		}
		private void HandleEvent_IAutomaticDoorCommunicatorRemoteConnectStateChanged(object sender, RemoteConnectStateChangedEventArgs e)
		{
			if (rAutomaticDoorInfoManager.IsExisByIpPortt(e.IpPort))
			{
				rAutomaticDoorInfoManager.GetItemByIpPort(e.IpPort).UpdateIsConnected(e.Connected);
			}
		}
		private void HandleEvent_IAutomaticDoorCommunicatorSentData(object sender, SentDataEventArgs e)
		{
			if (rAutomaticDoorInfoManager.IsExisByIpPortt(e.IpPort))
			{
				if (e.Data.Contains("OpenDoor"))
				{
					rAutomaticDoorInfoManager.GetItemByIpPort(e.IpPort).UpdateState(AutomaticDoorState.Opened);
				}
				else if (e.Data.Contains("CloseDoor"))
				{
					rAutomaticDoorInfoManager.GetItemByIpPort(e.IpPort).UpdateState(AutomaticDoorState.Closed);
				}
			}
		}
	}
}
