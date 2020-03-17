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
			string serialInfo = GetSerial(Data);
			string replyMsg = string.IsNullOrEmpty(serialInfo) ? string.Empty : $"Serial={serialInfo} ";

			if (IsCommandTypeMission(Data))
			{
				IMissionState missionState = ConvertToIMissionState(IpPort, Data, out string analyzeFailedDetail);
				if (missionState != null)
				{
					replyMsg += $"Reply=CommandAccepted MissionID={missionState.GetMissionId()}";
					rHostCommunicator.SendString(IpPort, replyMsg);
					rMissionStateManager.Add(missionState.mName, missionState);
				}
				else
				{
					replyMsg += $"Reply=CommandRejected Reason={analyzeFailedDetail}";
					rHostCommunicator.SendString(IpPort, replyMsg);
				}
			}
			else if (IsCommandTypeQueryCommand(Data))
			{
				
			}
			else
			{
				replyMsg += $"Reply=CommandRejected Reason=UnknownCommand";
				rHostCommunicator.SendString(IpPort, replyMsg);
			}
		}
		private IMissionState ConvertToIMissionState(string IpPort, string Data, out string AnalyzeFailedDetail)
		{
			IMissionState result = null;
			AnalyzeFailedDetail = string.Empty;

			if (rMissionAnalyzers != null && rMissionAnalyzers.Count() > 0)
			{
				for (int i = 0; i < rMissionAnalyzers.Length; ++i)
				{
					if (Data.Contains($"{rMissionAnalyzers[i].mKeyItem}={rMissionAnalyzers[i].mKeyword}"))
					{
						if (rMissionAnalyzers[i].TryParse(Data, out IMission Mission, out AnalyzeFailedDetail) == MissionAnalyzeResult.Successed)
						{
							result = Library.Library.GenerateIMissionState(Mission);
							result.UpdateSourceIpPort(IpPort);
						}
						break;
					}
				}
				if (string.IsNullOrEmpty(AnalyzeFailedDetail)) AnalyzeFailedDetail = "UnknownCommand";
			}
			else
			{
				AnalyzeFailedDetail = "AnalyzerNotFound";
			}

			return result;
		}

		private static bool IsCommandTypeMission(string Data)
		{
			return Data.Contains("Mission=");
		}
		private static bool IsCommandTypeQueryCommand(string Data)
		{
			return Data.Contains("Command=Query");
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
