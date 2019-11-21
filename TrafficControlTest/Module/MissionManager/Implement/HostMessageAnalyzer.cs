using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General.Interface;
using TrafficControlTest.Module.MissionManager.Interface;

namespace TrafficControlTest.Module.MissionManager.Implement
{
	/// <summary>
	/// 
	/// </summary>
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
				IMissionState missionState = null;
				for (int i = 0; i < rMissionAnalyzers.Length; ++i)
				{
					if (Data.Contains($"{rMissionAnalyzers[i].mKeyItem}={rMissionAnalyzers[i].mKeyword}"))
					{
						if (rMissionAnalyzers[i].TryParse(Data, out IMission Mission, out string AnalyzedFailedDetail) == MissionAnalyzeResult.Successed)
						{
							if (!rMissionStateManager.IsExist(Mission.mMissionId))
							{
								missionState = Library.Library.GenerateIMissionState(Mission);
								missionState.UpdateSourceIpPort(IpPort);
								replyMsg = $"Event=CommandAccepted MissionID={missionState.mName}";
							}
							else
							{
								replyMsg = $"Event=CommandRejected Reason=MissionIDDuplicated";
							}
						}
						else
						{
							replyMsg = $"Event=CommandRejected Reason={AnalyzedFailedDetail}";
						}
						break;
					}
				}
				if (string.IsNullOrEmpty(replyMsg)) replyMsg = "Event=CommandRejected Reason=UnknownCommand";
				string serial = GetSerial(Data);
				if (!string.IsNullOrEmpty(serial)) replyMsg = $"Serial={serial} {replyMsg}";
				rHostCommunicator.SendString(IpPort, replyMsg);
				if (missionState != null) rMissionStateManager.Add(missionState.mName, missionState);
			}
		}
		private static string GetSerial(string Data)
		{
			string result = null;
			if (Data.Contains("Serial="))
			{
				int beginIndex = Data.IndexOf("Serial=") + "Serial=".Length;
				int endIndex = Data.IndexOf(" ", beginIndex);
				if (endIndex == -1)
				{
					result = Data.Substring(beginIndex);
				}
				else
				{
					result = Data.Substring(beginIndex, endIndex - beginIndex);
				}
			}
			return result;
		}
	}
}
