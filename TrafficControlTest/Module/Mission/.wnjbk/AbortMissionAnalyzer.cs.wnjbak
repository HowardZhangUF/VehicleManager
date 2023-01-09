using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Module.Mission
{
	public class AbortMissionAnalyzer : MissionAnalyzer
	{
		public static AbortMissionAnalyzer mInstance = new AbortMissionAnalyzer();
		public override MissionType mMissionType { get; } = MissionType.Abort;
		protected override string[] mNecessaryItem { get; } = new string[] { "AbortMissionID" };
		protected override string[] mOptionalItem { get; } = new string[] { "MissionID" };

		protected AbortMissionAnalyzer() : base() { }
		protected override MissionAnalyzeResult FillIMissionUsingDictionary(ref IMission Mission, ref string AnalyzeFailedDetail)
		{
			List<string> errorItem = new List<string>();

			// Check Parameter AbortMissionID
			string abortMissionIdValue = string.Empty;
			if (!string.IsNullOrEmpty(mItemCollection["AbortMissionID"]))
			{
				abortMissionIdValue = mItemCollection["AbortMissionID"];
			}
			else
			{
				errorItem.Add("AbortMissionID");
			}

			// Export Result
			if (errorItem.Count == 0)
			{
				Mission = GenerateIMission(mMissionType, mItemCollection["MissionID"], mPriorityMin, string.Empty, new string[] { abortMissionIdValue });
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
