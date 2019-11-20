using System;
using System.Collections.Generic;
using TrafficControlTest.Module.General.Interface;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Interface
{
	/// <summary>
	/// - 儲存車子的資訊
	/// - 儲存物件的識別碼
	/// - 物件的資訊更新時會拋出事件
	/// </summary>
	public interface IVehicleInfo : IItem
	{
		/// <summary>當前狀態</summary>
		string mCurrentState { get; }
		/// <summary>上一個狀態</summary>
		string mPreviousState { get; }
		/// <summary>狀態持續時間</summary>
		TimeSpan mCurrentStateDuration { get; }
		/// <summary>位置 (mm)</summary>
		IPoint2D mLocationCoordinate { get; }
		/// <summary>面向 (degree)</summary>
		double mLocationToward { get; }
		/// <summary>當前移動目標點</summary>
		string mCurrentTarget { get; }
		/// <summary>上一個移動目標點</summary>
		string mPreviousTarget { get; }
		/// <summary>速度 (mm/s)</summary>
		double mVelocity { get; }
		/// <summary>平均速度 (mm/s) 。收集最近 n 筆的速度數據來做平均</summary>
		double mAverageVelocity { get; }
		/// <summary>定位分數 (%)</summary>
		double mLocationScore { get; }
		/// <summary>電池電量 (%)</summary>
		double mBatteryValue { get; }
		/// <summary>錯誤訊息</summary>
		string mAlarmMessage { get; }
		/// <summary>路徑</summary>
		IList<IPoint2D> mPath { get; }
		/// <summary>路徑(詳細)。透過 mPath 計算而得</summary>
		IList<IPoint2D> mPathDetail { get; }
		/// <summary>路徑範圍。透過 mPath 計算而得</summary>
		IRectangle2D mPathRegion { get; }
		/// <summary>IP:Port</summary>
		string mIpPort { get; }
		/// <summary>上次更新時間</summary>
		DateTime mLastUpdated { get; }

		/// <summary>當前執行的任務識別碼。沒有執行任務時為空值</summary>
		string mCurrentMissionId { get; }
		/// <summary>上一個執行的任務識別碼</summary>
		string mPreviousMissionId { get; }
		/// <summary>當前執行的干預指令。沒有執行干預時為空值</summary>
		string mCurrentInterveneCommand { get; }
		/// <summary>上一個執行的干預指令</summary>
		string mPreviousInterveneCommand { get; }
		/// <summary>當前使用的地圖檔的名稱(不含副檔名)</summary>
		string mCurrentMapName { get; }
		/// <summary>當前擁有的地圖檔名稱清單(不含副檔名)</summary>
		IList<string> mCurrentMapNameList { get; }

		/// <summary>速度最大值</summary>
		double mVelocityMaximum { get; }
		/// <summary>安全框半徑</summary>
		int mSafetyFrameRadius { get; }
		/// <summary>Buffer 框半徑</summary>
		int mBufferFrameRadius { get; }
		/// <summary>總車框半徑。總車框 = 車身安全框 + Buffer 框。在偵測碰撞事件時會使用到此資料</summary>
		int mTotalFrameRadius { get; }

		void Set(string Name);
		/// <summary>開始收集更新事件資訊</summary>
		void BeginUpdate();
		/// <summary>結束收集更新事件資訊並發出更新事件</summary>
		void EndUpdate();
		void UpdateCurrentState(string NewState);
		void UpdateLocationCoordinate(IPoint2D NewLocationCoordinate);
		void UpdateLocationToward(double NewLocationToward);
		void UpdateCurrentTarget(string NewTarget);
		void UpdateVelocity(double NewVelocity);
		void UpdateLocationScore(double NewLocationScore);
		void UpdateBatteryValue(double NewBattery);
		void UpdateAlarmMessage(string NewAlarmMessage);
		void UpdatePath(IEnumerable<IPoint2D> Path);
		void UpdateIpPort(string IpPort);
		void UpdateCurrentMissionId(string NewMissionId);
		void UpdateCurrentInterveneCommand(string NewInterveneCommand);
		void UpdateCurrentMapName(string NewMapName);
		void UpdateCurrentMapNameList(IEnumerable<string> NewMapNameList);
		void UpdateVelocityMaximum(double NewVelocityMaximum);
		void UpdateSafetyFrameRadius(int NewSafetyFrameRadius);
		void UpdateBufferFrameRadius(int NewBufferFrameRadius);
		string ToString();
	}
}
