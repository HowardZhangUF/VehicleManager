using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using TrafficControlTest.Module.MissionManager.Interface;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Module.MissionManager.Implement
{
	public class DockMissionAnalyzer : MissionAnalyzer
	{
		public static DockMissionAnalyzer mInstance = new DockMissionAnalyzer();
		public override MissionType mMissionType { get; } = MissionType.Dock;
		protected override string[] mNecessaryItem { get; } = new string[] { "VehicleID" };
		protected override string[] mOptionalItem { get; } = new string[] { "MissionID", "Priority" };

		protected DockMissionAnalyzer() : base() { }
		protected override MissionAnalyzeResult FillIMissionUsingDictionary(ref IMission Mission, ref string AnalyzeFailedDetail)
		{
			List<string> errorItem = new List<string>();

			// Check Parameter VehicleID
			string vehicleIdValue = string.Empty;
			if (!string.IsNullOrEmpty(mItemCollection["VehicleID"]))
			{
				vehicleIdValue = mItemCollection["VehicleID"];
			}
			else
			{
				errorItem.Add("VehicleID");
			}

			// Check Parameter Priority
			int priorityValue = 0;
			if (!string.IsNullOrEmpty(mItemCollection["Priority"]))
			{
				if (int.TryParse(mItemCollection["Priority"], out int priority))
				{
					if (priority >= mPriorityMin && priority <= mPriorityMax)
					{
						priorityValue = priority;
					}
					else
					{
						errorItem.Add("Priority");
					}
				}
				else
				{
					errorItem.Add("Priority");
				}
			}
			else
			{
				priorityValue = mPriorityDefault;
			}

			// Export Result
			if (errorItem.Count == 0)
			{
				Mission = GenerateIMission(mMissionType, mItemCollection["MissionID"], priorityValue, mItemCollection["VehicleID"], null);
				return MissionAnalyzeResult.Successed;
			}
			else
			{
				AnalyzeFailedDetail = $"Parameter\"{string.Join(",", errorItem)}\"ValueError";
				return MissionAnalyzeResult.Failed;
			}
		}
	}
}
