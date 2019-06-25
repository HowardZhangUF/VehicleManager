using System;
using System.Collections.Generic;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleInfo;

namespace TrafficControlTest.Interface
{
	public interface IVehicleInfo
	{
		event EventHandlerIVehicleInfo VehicleStateUpdated;

		/// <summary>名稱</summary>
		string mName { get; }
		/// <summary>當前狀態</summary>
		string mState { get; set; }
		/// <summary>上一個狀態</summary>
		string mLastState { get; }
		/// <summary>狀態持續時間</summary>
		TimeSpan mStateDuration { get; }
		/// <summary>位置 (mm)</summary>
		IPoint2D mPosition { get; set; }
		/// <summary>面向 (degree)</summary>
		double mToward { get; set; }
		/// <summary>當前移動目標點</summary>
		string mTarget { get; set; }
		/// <summary>上一個移動目標點</summary>
		string mLastTarget { get; }
		/// <summary>速度(mm/s)</summary>
		double mVelocity { get; set; }
		/// <summary>平均速度 (mm/s) 。收集最近 n 秒內的速度數據來做平均</summary>
		double mAverageVelocity { get; }
		/// <summary>匹配度 (%)</summary>
		double mMapMatch { get; set; }
		/// <summary>電池電量 (%)</summary>
		double mBattery { get; set; }
		/// <summary>前方是否有物體擋住導致無法移動</summary>
		bool mPathBlocked { get; set; }
		/// <summary>有物體擋住導致無法移動持續時間</summary>
		TimeSpan mPathBlockedDuration { get; }
		/// <summary>錯誤訊息</summary>
		string mAlarmMessage { get; set; }
		/// <summary>安全框半徑</summary>
		int mSafetyFrameRadius { get; set; }
		/// <summary>Buffer 框半徑</summary>
		int mBufferFrameRadius { get; }
		/// <summary>車框半徑。車框 = 車身安全框 + Buffer 框</summary>
		int mTotalFrameRadius { get; }
		/// <summary>路徑</summary>
		IEnumerable<IPoint2D> mPath { get; set; }
		/// <summary>路徑(詳細)</summary>
		IEnumerable<IPoint2D> mPathDetail { get; }
		/// <summary>路徑範圍</summary>
		IRectangle2D mPathRegion { get; }
		/// <summary>IP:Port</summary>
		string mIpPort { get; set; }
		/// <summary>上次更新時間</summary>
		DateTime mLastUpdated { get; }
		/// <summary>車圖像識別碼</summary>
		int mVehicleIconId { get; set; }
		/// <summary>路徑線圖像識別碼</summary>
		int mPathIconId { get; set; }

		void Set(string Name);
		string ToString();
	}
}
