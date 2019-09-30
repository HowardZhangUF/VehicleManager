using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.MissionManager.Interface;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Module.MissionManager.Implement
{
	public class GotoPointMissionAnalyzer : MissionAnalyzer
	{
		public static GotoPointMissionAnalyzer mInstance = new GotoPointMissionAnalyzer();
		public override string mKeyword { get; } = "GotoPoint";
		protected override string mKeyItem { get; } = "Mission";
		protected override string[] mNecessaryItem { get; } = new string[] { "X", "Y" };
		protected override string[] mOptionalItem { get; } = new string[] { "Head", "MissionID", "VehicleID", "Priority" };

		protected GotoPointMissionAnalyzer() : base() { }
		protected override MissionAnalyzeResult FillIMissionUsingDictionary(ref IMission Mission, ref string AnalyzeFailedDetail)
		{
			List<string> errorItem = new List<string>();

			// Check Paramter X
			int xValue = 0;
			if (!string.IsNullOrEmpty(mItemCollection["X"]) && int.TryParse(mItemCollection["X"], out int x))
			{
				xValue = x;
			}
			else
			{
				errorItem.Add("X");
			}

			// Check Paramter Y
			int yValue = 0;
			if (!string.IsNullOrEmpty(mItemCollection["Y"]) && int.TryParse(mItemCollection["Y"], out int y))
			{
				yValue = y;
			}
			else
			{
				errorItem.Add("Y");
			}

			// Check Parameter Head
			int headValue = int.MaxValue;
			if (!string.IsNullOrEmpty(mItemCollection["Head"]))
			{
				if (int.TryParse(mItemCollection["Head"], out int head))
				{
					headValue = head;
				}
				else
				{
					errorItem.Add("Head");
				}
			}
			else
			{
				headValue = int.MaxValue;
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
				Mission = GenerateIMission(mKeyword, mItemCollection["MissionID"], priorityValue, mItemCollection["VehicleID"], new string[] { xValue.ToString(), yValue.ToString(), headValue.ToString() });
				return MissionAnalyzeResult.Successed;
			}
			else
			{
				AnalyzeFailedDetail = $"Parameter \"{string.Join(", ", errorItem)}\" Value Error.";
				return MissionAnalyzeResult.Failed;
			}
		}
	}
}
