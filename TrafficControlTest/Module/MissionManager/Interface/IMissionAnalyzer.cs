using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.MissionManager.Interface
{
	public enum MissionAnalyzeResult
	{
		Successed,
		Failed
	}

	public interface IMissionAnalyzer
	{
		string mKeyword { get; }
		string mKeyItem { get; }

		MissionAnalyzeResult TryParse(string Message, out IMission Mission, out string AnalyzeFailedDetail);
	}
}
