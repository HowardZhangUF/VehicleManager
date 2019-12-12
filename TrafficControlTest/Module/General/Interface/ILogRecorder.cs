using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using TrafficControlTest.Module.MissionManager.Interface;

namespace TrafficControlTest.Module.General.Interface
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
