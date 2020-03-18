using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
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
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IMissionStateManager rMissionStateManager = null;
		private IMissionAnalyzer[] rMissionAnalyzers = null;

		public HostMessageAnalyzer(IHostCommunicator HostCommunicator, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IMissionAnalyzer[] MissionAnalyzers)
		{
			Set(HostCommunicator, VehicleInfoManager, MissionStateManager, MissionAnalyzers);
		}
		public void Set(IHostCommunicator HostCommunicator)
		{
			UnsubscribeEvent_IHostCommunicator(rHostCommunicator);
			rHostCommunicator = HostCommunicator;
			SubscribeEvent_IHostCommunicator(rHostCommunicator);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			rVehicleInfoManager = VehicleInfoManager;
		}
		public void Set(IMissionStateManager MissionStateManager)
		{
			rMissionStateManager = MissionStateManager;
		}
		public void Set(IMissionAnalyzer[] MissionAnalyzers)
		{
			rMissionAnalyzers = MissionAnalyzers;
		}
		public void Set(IHostCommunicator HostCommunicator, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager, IMissionAnalyzer[] MissionAnalyzers)
		{
			Set(HostCommunicator);
			Set(VehicleInfoManager);
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
				if (Data.Contains("Command=QueryVehicleList"))
				{
					replyMsg += $"Reply=QueryVehicleList VehicleCount={rVehicleInfoManager.mCount}" + GetVehicleListString();
				}
				else if (Data.Contains("Command=QueryVehicleInfo"))
				{
					replyMsg += $"Reply=QueryVehicleInfo VehicleCount={rVehicleInfoManager.mCount}" + GetVehicleInfoString();
				}
				else if (Data.Contains("Command=QueryMissionList"))
				{
					replyMsg += $"Reply=QueryMissionList MissionCount={rMissionStateManager.mCount}" + GetMissionListString();
				}
				else if (Data.Contains("Command=QueryMissionInfo"))
				{
					replyMsg += $"Reply=QueryMissionInfo MissionCount={rMissionStateManager.mCount}" + GetMissionInfoString();
				}
				else
				{
					replyMsg += $"Reply=CommandRejected Reason=UnknownCommand";
				}
				rHostCommunicator.SendString(IpPort, replyMsg);
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
		private string GetVehicleListString()
		{
			string result = string.Empty;
			List<string> vehicleIds = rVehicleInfoManager.GetListOfVehicleId();
			if (vehicleIds != null && vehicleIds.Count > 0)
			{
				for (int i = 0; i < vehicleIds.Count; ++i)
				{
					result += $" VehicleID{(i + 1).ToString()}={vehicleIds[i]}";
				}
			}
			return result;
		}
		private string GetVehicleInfoString(string VehicleId, string AppendIndex = "")
		{
			string result = string.Empty;
			IVehicleInfo vehicleInfo = rVehicleInfoManager.GetItem(VehicleId);
			if (vehicleInfo != null)
			{
				result += $" VehicleID{AppendIndex}={vehicleInfo.mName} State{AppendIndex}={vehicleInfo.mCurrentState} X{AppendIndex}={vehicleInfo.mLocationCoordinate.mX} Y{AppendIndex}={vehicleInfo.mLocationCoordinate.mY} Head{AppendIndex}={(int)vehicleInfo.mLocationToward} Battery{AppendIndex}={(int)vehicleInfo.mBatteryValue}";
			}
			return result;
		}
		private string GetVehicleInfoString()
		{
			string result = string.Empty;
			List<string> vehicleIds = rVehicleInfoManager.GetListOfVehicleId();
			if (vehicleIds != null && vehicleIds.Count > 0)
			{
				for (int i = 0; i < vehicleIds.Count; ++i)
				{
					result += GetVehicleInfoString(vehicleIds[i], (i + 1).ToString());
				}
			}
			return result;
		}
		private string GetMissionListString()
		{
			string result = string.Empty;
			List<string> missionIds = rMissionStateManager.GetListOfMissionId();
			if (missionIds != null && missionIds.Count > 0)
			{
				for (int i = 0; i < missionIds.Count; ++i)
				{
					result += $" MissionID{(i + 1).ToString()}={missionIds[i]}";
				}
			}
			return result;
		}
		private string GetMissionInfoString(string MissionId, string AppendIndex = "")
		{
			string result = string.Empty;
			IMissionState missionState = rMissionStateManager.GetItem(MissionId);
			if (missionState != null)
			{
				result += $" MissionID{AppendIndex}={missionState.GetMissionId()} Mission{AppendIndex}={missionState.mMission.mMissionType.ToString()}";
				result += $" Parameter{AppendIndex}={missionState.mMission.mParametersString}";
				result += $" ExecuteState{AppendIndex}={missionState.mExecuteState.ToString()}";
			}
			return result;
		}
		private string GetMissionInfoString()
		{
			string result = string.Empty;
			List<string> missionIds = rMissionStateManager.GetListOfMissionId();
			if (missionIds != null && missionIds.Count > 0)
			{
				for (int i = 0; i < missionIds.Count; ++i)
				{
					result += GetMissionInfoString(missionIds[i], (i + 1).ToString());
				}
			}
			return result;
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
		private static bool IsCommandTypeMission(string Data)
		{
			return Data.Contains("Mission=");
		}
		private static bool IsCommandTypeQueryCommand(string Data)
		{
			return Data.Contains("Command=Query");
		}
	}
}
