using TrafficControlTest.Library;

namespace TrafficControlTest.Module.Log
{
	public enum DatabaseDataOperation
	{
		Add,
		Remove,
		Update
	}

	/// <summary>提供記錄 General Log 至資料庫的功能</summary>
	public interface ILogRecorder
	{
		bool mIsExecuting { get; }

		void Set(DatabaseAdapter DatabaseAdapter);
		void Start();
		void Stop();
		void RecordGeneralLog(string Timestamp, string Category, string SubCategory, string Message);
	}
}
