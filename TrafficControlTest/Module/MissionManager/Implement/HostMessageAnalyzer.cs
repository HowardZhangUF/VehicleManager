using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General.Interface;
using TrafficControlTest.Module.MissionManager.Interface;

namespace TrafficControlTest.Module.MissionManager.Implement
{
	public class HostMessageAnalyzer : IHostMessageAnalyzer
	{
		private IHostCommunicator rHostCommunicator = null;
		private IMissionStateManager rMissionStateManager = null;
		private IMissionAnalyzer[] rMissionAnalyzers = null;

		public HostMessageAnalyzer(IHostCommunicator HostCommunicator, IMissionStateManager MissionStateManager, IMissionAnalyzer[] MissionAnalyzers)
		{
			Set(HostCommunicator, MissionStateManager, MissionAnalyzers);
		}
		public void Set(IHostCommunicator HostCommunicator)
		{
			UnsubscribeEvent_IHostCommunicator(rHostCommunicator);
			rHostCommunicator = HostCommunicator;
			SubscribeEvent_IHostCommunicator(rHostCommunicator);
		}
		public void Set(IMissionStateManager MissionStateManager)
		{
			rMissionStateManager = MissionStateManager;
		}
		public void Set(IMissionAnalyzer[] MissionAnalyzers)
		{
			rMissionAnalyzers = MissionAnalyzers;
		}
		public void Set(IHostCommunicator HostCommunicator, IMissionStateManager MissionStateManager, IMissionAnalyzer[] MissionAnalyzers)
		{
			Set(HostCommunicator);
			Set(MissionStateManager);
			Set(MissionAnalyzers);
		}

		private void SubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.ReceivedString += HandleEvent_HostCommunicatorReceivedString;
			}
		}
		private void UnsubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.ReceivedString -= HandleEvent_HostCommunicatorReceivedString;
			}
		}
		private void SubscribeEvent_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				// do nothing
			}
		}
		private void HandleEvent_HostCommunicatorReceivedString(DateTime OccurTime, string IpPort, string Data)
		{
			if (rMissionAnalyzers != null && rMissionAnalyzers.Count() > 0)
			{
				string replyMsg = string.Empty;
				for (int i = 0; i < rMissionAnalyzers.Length; ++i)
				{
					if (Data.Contains(rMissionAnalyzers[i].mKeyword))
					{
						if (rMissionAnalyzers[i].TryParse(Data, out IMission Mission, out string AnalyzedFailedDetail) == MissionAnalyzeResult.Successed)
						{
							IMissionState missionState = Library.Library.GenerateIMissionState(Mission);
							missionState.UpdateSourceIpPort(IpPort);
							if (!rMissionStateManager.IsExist(missionState.mMissionId))
							{
								rMissionStateManager.Add(missionState.mMissionId, missionState);
								replyMsg = $"Event=MissionAccepted";
							}
							else
							{
								replyMsg = $"Event=MissionRejected Reason=[Mission ID Duplicated]";
							}
						}
						else
						{
							replyMsg = $"Event=MissionRejected Reason=[{AnalyzedFailedDetail}]";
						}
						break;
					}
				}
				if (string.IsNullOrEmpty(replyMsg)) replyMsg = "Event=MissionRejected Reason=[Unknown Message]";
				rHostCommunicator.SendString(IpPort, replyMsg);
			}
		}
	}
}
