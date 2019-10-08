using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.MissionManager.Interface;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Module.MissionManager.Implement
{
	public class GotoMissionAnalyzer : MissionAnalyzer
	{
		public static GotoMissionAnalyzer mInstance = new GotoMissionAnalyzer();
		public override string mKeyword { get; } = "Goto";
		public override string mKeyItem { get; } = "Mission";
		protected override string[] mNecessaryItem { get; } = new string[] { "Target" };
		protected override string[] mOptionalItem { get; } = new string[] { "MissionID", "VehicleID", "Priority" };

		protected GotoMissionAnalyzer() : base() { }
		protected override MissionAnalyzeResult FillIMissionUsingDictionary(ref IMission Mission, ref string AnalyzeFailedDetail)
		{
			List<string> errorItem = new List<string>();

			// Check Parameter Target
			string targetValue = string.Empty;
			if (!string.IsNullOrEmpty(mItemCollection["Target"]))
			{
				targetValue = mItemCollection["Target"];
			}
			else
			{
				errorItem.Add("Target");
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
				Mission = GenerateIMission(mKeyword, mItemCollection["MissionID"], priorityValue, mItemCollection["VehicleID"], new string[] { targetValue });
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
