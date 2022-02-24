using LibraryForVM;
using SerialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	public class SimulatorInfoReporter : SystemWithLoopTask, ISimulatorInfoReporter
	{
		private ISimulatorInfo rSimulatorInfo = null;
		private IHostCommunicator rHostCommunicator = null;

		public SimulatorInfoReporter(ISimulatorInfo ISimulatorInfo, IHostCommunicator IHostCommunicator)
		{
			Set(ISimulatorInfo, IHostCommunicator);
		}
		public void Set(ISimulatorInfo ISimulatorInfo)
		{
			UnsubscribeEvent_ISimulatorInfo(ISimulatorInfo);
			rSimulatorInfo = ISimulatorInfo;
			SubscribeEvent_ISimualtorInfo(ISimulatorInfo);
		}
		public void Set(IHostCommunicator IHostCommunicator)
		{
			rHostCommunicator = IHostCommunicator;
		}
		public void Set(ISimulatorInfo ISimulatorInfo, IHostCommunicator IHostCommunicator)
		{
			Set(ISimulatorInfo);
			Set(IHostCommunicator);
		}
		public override string GetSystemInfo()
		{
			return $"";
		}
		public override void Task()
		{
			Subtask_ReportSimualtorInfo();
		}

		protected static AGVStatus ConvertToAGVStatus(ISimulatorInfo ISimulatorInfo)
		{
			AGVStatus result = null;
			if (ISimulatorInfo == null)
			{
				result = null;
			}
			else
			{
				result = new AGVStatus();
				result.Name = ISimulatorInfo.mName;
				result.Description = ConvertToEDescription(ISimulatorInfo.mStatus);
				result.X = ISimulatorInfo.mX;
				result.Y = ISimulatorInfo.mY;
				result.Toward = ISimulatorInfo.mToward;
				result.GoalName = ISimulatorInfo.mTarget;
				result.MapMatch = ISimulatorInfo.mScore;
				result.Battery = ISimulatorInfo.mBattery;
				result.AlarmMessage = string.Empty;
				result.IsInterveneAvailable = default(bool);
				result.IsBeingIntervened = default(bool);
				result.InterveneCommand = string.Empty;
			}
			return result;
		}
		protected static AGVPath ConvertToAGVPath(ISimulatorInfo ISimulatorInfo)
		{
			AGVPath result = null;
			if (ISimulatorInfo == null)
			{
				result = null;
			}
			else
			{
				result = new AGVPath();
				result.Name = ISimulatorInfo.mName;
				if (ISimulatorInfo.mPath == null || ISimulatorInfo.mPath.Count == 0)
				{
					result.PathX = new List<double>();
					result.PathY = new List<double>();
				}
				else
				{
					result.PathX = ISimulatorInfo.mPath.Select(o => (double)o.mX).ToList();
					result.PathY = ISimulatorInfo.mPath.Select(o => (double)o.mY).ToList();
				}
			}
			return result;
		}
		protected static EDescription ConvertToEDescription(ESimulatorStatus SimulatorStatus)
		{
			switch (SimulatorStatus)
			{
				case ESimulatorStatus.Idle:
					return EDescription.Idle;
				case ESimulatorStatus.Working:
					return EDescription.Running;
				case ESimulatorStatus.ChargingButFree:
					return EDescription.ChargeIdle;
				case ESimulatorStatus.Charging:
					return EDescription.Charge;
				case ESimulatorStatus.Maintaining:
					return EDescription.Running;
				case ESimulatorStatus.Failing:
					return EDescription.ObstacleExists;
				case ESimulatorStatus.Paused:
					return EDescription.Pause;
				case ESimulatorStatus.None:
				default:
					return EDescription.Running;
			}
		}

		private void SubscribeEvent_ISimualtorInfo(ISimulatorInfo ISimulatorInfo)
		{
			if (ISimulatorInfo != null)
			{
				ISimulatorInfo.StatusUpdated += HandleEvent_ISimulatorInfoStatusUpdated;
			}
		}
		private void UnsubscribeEvent_ISimulatorInfo(ISimulatorInfo ISimulatorInfo)
		{
			if (ISimulatorInfo != null)
			{
				ISimulatorInfo.StatusUpdated -= HandleEvent_ISimulatorInfoStatusUpdated;
			}
		}
		private void HandleEvent_ISimulatorInfoStatusUpdated(object sender, StatusUpdatedEventArgs e)
		{
			if (e.StatusName.Contains("MapFilePath"))
			{
				if (!string.IsNullOrEmpty(rSimulatorInfo.mMapFilePath))
				{
					LoadMap tmpLoadMap = new LoadMap(new List<string> { "Simulator", "127.0.0.1:65535", System.IO.Path.GetFileName(rSimulatorInfo.mMapFilePath) });
					rHostCommunicator.SendData(tmpLoadMap);
				}
			}
		}
		private void Subtask_ReportSimualtorInfo()
		{
			if (rSimulatorInfo != null && rHostCommunicator != null && rHostCommunicator.mIsConnected)
			{
				AGVStatus tmpAgvStatus = ConvertToAGVStatus(rSimulatorInfo);
				if (tmpAgvStatus != null)
				{
					rHostCommunicator.SendData(tmpAgvStatus);
				}
				AGVPath tmpAgvPath = ConvertToAGVPath(rSimulatorInfo);
				if (tmpAgvPath != null)
				{
					rHostCommunicator.SendData(tmpAgvPath);
				}
			}
		}
	}
}
