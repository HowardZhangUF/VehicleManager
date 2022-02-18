using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Module.CommunicationHost;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.NewCommunication;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.Module.Mission
{
    /// <summary>
    /// 
    /// </summary>
    public class HostMessageAnalyzer : SystemWithConfig, IHostMessageAnalyzer
    {
        private IHostCommunicator rHostCommunicator = null;
        private IVehicleInfoManager rVehicleInfoManager = null;
        private IMissionStateManager rMissionStateManager = null;
        private IMissionAnalyzer[] rMissionAnalyzers = null;
        private bool mFilterDuplicateMissionWhenReceivedCommand = true;

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
		public override string[] GetConfigNameList()
		{
			return new string[] { "FilterDuplicateMissionWhenReceivedCommand" };
		}
		public override string GetConfig(string ConfigName)
        {
            switch (ConfigName)
            {
                case "FilterDuplicateMissionWhenReceivedCommand":
                    return mFilterDuplicateMissionWhenReceivedCommand.ToString();
                default:
                    return null;
            }
        }
        public override void SetConfig(string ConfigName, string NewValue)
        {
            switch (ConfigName)
            {
                case "FilterDuplicateMissionWhenReceivedCommand":
                    mFilterDuplicateMissionWhenReceivedCommand = bool.Parse(NewValue);
                    RaiseEvent_ConfigUpdated(ConfigName, NewValue);
                    break;
                default:
                    break;
            }
        }

        private void SubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
        {
            if (HostCommunicator != null)
            {
                HostCommunicator.ReceivedData += HandleEvent_HostCommunicatorReceivedData;
            }
        }
        private void UnsubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
        {
            if (HostCommunicator != null)
            {
                HostCommunicator.ReceivedData -= HandleEvent_HostCommunicatorReceivedData;
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
        private void HandleEvent_HostCommunicatorReceivedData(object Sender, ReceivedDataEventArgs Args)
        {
            string serialInfo = GetSerial(Args.Data.ToString());
            string replyMsg = string.IsNullOrEmpty(serialInfo) ? string.Empty : $"Serial={serialInfo} ";

            if (IsCommandTypeMission(Args.Data.ToString()))
            {
                IMissionState missionState = ConvertToIMissionState(Args.IpPort, Args.Data.ToString(), out string analyzeFailedDetail);
                if (missionState != null)
                {
                    if (mFilterDuplicateMissionWhenReceivedCommand)
                    {
                        if (!rMissionStateManager.GetItems().Any(o => AreTheyHaveSameContent(o, missionState)))
                        {
                            replyMsg += $"Reply=CommandAccepted MissionID={missionState.GetMissionId()}";
                            rHostCommunicator.SendData(Args.IpPort, replyMsg);
                            rMissionStateManager.Add(missionState.mName, missionState);
                        }
                        else
                        {
                            replyMsg += $"Reply=CommandRejected Reason=MissionHasAlreadyExisted";
                            rHostCommunicator.SendData(Args.IpPort, replyMsg);
                        }
                    }
					else
                    {
                        replyMsg += $"Reply=CommandAccepted MissionID={missionState.GetMissionId()}";
                        rHostCommunicator.SendData(Args.IpPort, replyMsg);
                        rMissionStateManager.Add(missionState.mName, missionState);
                    }
                }
                else
                {
                    replyMsg += $"Reply=CommandRejected Reason={analyzeFailedDetail}";
                    rHostCommunicator.SendData(Args.IpPort, replyMsg);
                }
            }
            else if (IsCommandTypeQueryCommand(Args.Data.ToString()))
            {
                if (Args.Data.ToString().Contains("Command=QueryVehicleList"))
                {
                    replyMsg += $"Reply=QueryVehicleList VehicleCount={rVehicleInfoManager.mCount}" + GetVehicleListString();
                }
                else if (Args.Data.ToString().Contains("Command=QueryVehicleInfo"))
                {
                    replyMsg += $"Reply=QueryVehicleInfo VehicleCount={rVehicleInfoManager.mCount}" + GetVehicleInfoString();
                }
                else if (Args.Data.ToString().Contains("Command=QueryMissionList"))
                {
                    replyMsg += $"Reply=QueryMissionList MissionCount={rMissionStateManager.mCount}" + GetMissionListString();
                }
                else if (Args.Data.ToString().Contains("Command=QueryMissionInfo"))
                {
                    replyMsg += $"Reply=QueryMissionInfo MissionCount={rMissionStateManager.mCount}" + GetMissionInfoString();
                }
                else
                {
                    replyMsg += $"Reply=CommandRejected Reason=UnknownCommand";
                }
                rHostCommunicator.SendData(Args.IpPort, replyMsg);
            }
            else
            {
                replyMsg += $"Reply=CommandRejected Reason=UnknownCommand";
                rHostCommunicator.SendData(Args.IpPort, replyMsg);
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
        private bool AreTheyHaveSameContent(IMissionState MissionState1, IMissionState MissionState2)
        {
            return AreTheyHaveSameContent(MissionState1.mMission, MissionState2.mMission);
        }
        private bool AreTheyHaveSameContent(IMission Mission1, IMission Mission2)
        {
            if (Mission1.mMissionType == Mission2.mMissionType
                && Mission1.mMissionId == Mission2.mMissionId
				&& Mission1.mPriority == Mission2.mPriority
				&& Mission1.mVehicleId == Mission2.mVehicleId
				&& Mission1.mParametersString == Mission2.mParametersString)
            {
                return true;
            }
            else
            {
                return false;
            }
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
                result += $" VehicleID{AppendIndex}={vehicleInfo.mName}";
                result += $" State{AppendIndex}={vehicleInfo.mCurrentState}";
				result += $" Target{AppendIndex}={vehicleInfo.mCurrentTarget}";
                result += $" X{AppendIndex}={vehicleInfo.mLocationCoordinate.mX}";
                result += $" Y{AppendIndex}={vehicleInfo.mLocationCoordinate.mY}";
                result += $" Head{AppendIndex}={(int)vehicleInfo.mLocationToward}";
                result += $" Battery{AppendIndex}={(int)vehicleInfo.mBatteryValue}";
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
            List<string> missionIds = rMissionStateManager.GetItems().Select(o => o.GetMissionId()).ToList();
            if (missionIds != null && missionIds.Count > 0)
            {
                for (int i = 0; i < missionIds.Count; ++i)
                {
                    result += $" MissionID{(i + 1).ToString()}={missionIds[i]}";
                }
            }
            return result;
        }
        private string GetMissionInfoString(string MissionStateName, string AppendIndex = "")
        {
            string result = string.Empty;
            IMissionState missionState = rMissionStateManager.GetItem(MissionStateName);
            if (missionState != null)
            {
                result += $" MissionID{AppendIndex}={missionState.GetMissionId()}";
                result += $" Mission{AppendIndex}={missionState.mMission.mMissionType.ToString()}";
                result += $" Parameter{AppendIndex}={missionState.mMission.mParametersString}";
				result += $" ExecutorID{AppendIndex}={missionState.mExecutorId}";
                result += $" ExecuteState{AppendIndex}={missionState.mExecuteState.ToString()}";
            }
            return result;
        }
        private string GetMissionInfoString()
        {
            string result = string.Empty;
            List<string> missionStateNames = rMissionStateManager.GetItemNames().ToList();
            if (missionStateNames != null && missionStateNames.Count > 0)
            {
                for (int i = 0; i < missionStateNames.Count; ++i)
                {
                    result += GetMissionInfoString(missionStateNames[i], (i + 1).ToString());
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
