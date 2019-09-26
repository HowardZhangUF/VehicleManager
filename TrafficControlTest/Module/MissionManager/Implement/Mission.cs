using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.MissionManager.Interface;

namespace TrafficControlTest.Module.MissionManager.Implement
{
	public class Mission : IMission
	{
		public string mMissionType { get; private set; }
		public string mMissionId { get; private set; }
		public int mPriority { get; private set; }
		public string mVehicleId { get; private set; }
		public string[] mParameters { get; private set; }
		public string mSourceIpPort { get; private set; }
		public DateTime mReceivedTimestamp { get; private set; }

		public Mission(string MissionType, string MissionId, int Priority, string VehicleId, string[] Parameters, string SourceIpPort)
		{
			Set(MissionType, MissionId, Priority, VehicleId, Parameters, SourceIpPort);
		}
		public void Set(string MissionType, string MissionId, int Priority, string VehicleId, string[] Parameters, string SourceIpPort)
		{
			mMissionType = MissionType;
			mMissionId = MissionId;
			mPriority = Priority;
			mVehicleId = VehicleId;
			mParameters = Parameters;
			mSourceIpPort = SourceIpPort;
			mReceivedTimestamp = DateTime.Now;
		}
		public void UpdatePriority(int Priority)
		{
			mPriority = Priority;
		}
		public override string ToString()
		{
			string result = string.Empty;
			result += mMissionType;
			if (!string.IsNullOrEmpty(mVehicleId)) result += $"/{mVehicleId}";
			if (mParameters != null && mParameters.Length > 0) result += $"/{string.Join(",", mParameters)}";
			return result;
		}
	}

	public interface IMissionAnalyzer
	{
		string mKeyword { get; }
		string[] mNecessaryItem { get; }
		string[] mOptionalItem { get; }

		bool TryParse(string Message, out IMission Mission);
	}

	public class GotoMissionAnalyzer : IMissionAnalyzer
	{
		public string mKeyword { get; } = "Mission=Goto";
		public string[] mNecessaryItem { get; } = new string[] { "Target" };
		public string[] mOptionalItem { get; } = new string[] { "MissionID", "VehicleID" };
		private Dictionary<string, string> Items = new Dictionary<string, string>()
		{
			{ "Mission", "Go" },
			{ "Target", string.Empty },
			{ "MissionID", string.Empty },
			{ "VehicleID", string.Empty },
		};

		public bool TryParse(string Message, out IMission Mission)
		{
			bool result = false;
			Mission = null;
			if (Message.Contains(mKeyword))
			{
				// 如果關鍵字正確
				// 如果必要項目皆有存在
				// 回傳成功與 IMission
			}
			return result;
		}
		private void InitializeItems()
		{
			Items["Mission"] = "Go";
			Items["Target"] = string.Empty;
			Items["MissionID"] = string.Empty;
			Items["VehicleID"] = string.Empty;
		}
	}

	public class GotoPointMissionAnalyzer : IMissionAnalyzer
	{
		public string mKeyword { get; } = "Mission=GotoPoint";
		public string[] mNecessaryItem { get; } = new string[] { "X", "Y" };
		public string[] mOptionalItem { get; } = new string[] { "Head", "MissionID", "VehicleID" };

		public bool TryParse(string Message, out IMission Mission)
		{
			bool result = false;
			Mission = null;

			return result;
		}
	}

	public class DockMissionAnalyzer : IMissionAnalyzer
	{
		public string mKeyword { get; } = "Mission=Dock";
		public string[] mNecessaryItem { get; } = new string[] { "VehicleID" };
		public string[] mOptionalItem { get; } = new string[] { "MissionID" };

		public bool TryParse(string Message, out IMission Mission)
		{
			bool result = false;
			Mission = null;

			return result;
		}
	}
}
