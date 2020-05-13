using System.Collections.Generic;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Module.Mission
{
	public class GotoMissionAnalyzer : MissionAnalyzer
	{
		public static GotoMissionAnalyzer mInstance = new GotoMissionAnalyzer();
		public override MissionType mMissionType { get; } = MissionType.Goto;
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
				Mission = GenerateIMission(mMissionType, mItemCollection["MissionID"], priorityValue, mItemCollection["VehicleID"], new string[] { targetValue });
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
