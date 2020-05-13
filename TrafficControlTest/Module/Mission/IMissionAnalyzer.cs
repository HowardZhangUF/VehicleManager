namespace TrafficControlTest.Module.Mission
{
	public enum MissionAnalyzeResult
	{
		Successed,
		Failed
	}

	/// <summary>
	/// - 提供 TryParse() 方法將 string 轉換成 IMission
	/// - 根據 mKeyword 跟 mKeyItem 屬性去決定要使用哪一個實體化的 MissionAnalyzer
	/// </summary>
	public interface IMissionAnalyzer
	{
		string mKeyword { get; }
		string mKeyItem { get; }

		MissionAnalyzeResult TryParse(string Message, out IMission Mission, out string AnalyzeFailedDetail);
	}
}
