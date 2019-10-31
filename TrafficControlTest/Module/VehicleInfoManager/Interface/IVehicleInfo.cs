using System;
using System.Collections.Generic;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Interface
{
	public interface IVehicleInfo
	{
		event EventHandlerIVehicleInfoStateUpdated StateUpdated;

		/// <summary>名稱</summary>
		string mName { get; }
		/// <summary>當前狀態</summary>
		string mState { get; }
		/// <summary>上一個狀態</summary>
		string mLastState { get; }
		/// <summary>狀態持續時間</summary>
		TimeSpan mStateDuration { get; }
		/// <summary>位置 (mm)</summary>
		IPoint2D mPosition { get; }
		/// <summary>面向 (degree)</summary>
		double mToward { get; }
		/// <summary>當前移動目標點</summary>
		string mTarget { get; }
		/// <summary>上一個移動目標點</summary>
		string mLastTarget { get; }
		/// <summary>速度(mm/s)</summary>
		double mVelocity { get; }
		/// <summary>平均速度 (mm/s) 。收集最近 n 秒內的速度數據來做平均</summary>
		double mAverageVelocity { get; }
		/// <summary>匹配度 (%)</summary>
		double mMapMatch { get; }
		/// <summary>電池電量 (%)</summary>
		double mBattery { get; }
		/// <summary>前方是否有物體擋住導致無法移動</summary>
		bool mPathBlocked { get; }
		/// <summary>有物體擋住導致無法移動持續時間</summary>
		TimeSpan mPathBlockedDuration { get; }
		/// <summary>錯誤訊息</summary>
		string mAlarmMessage { get; }
		/// <summary>安全框半徑</summary>
		int mSafetyFrameRadius { get; }
		/// <summary>Buffer 框半徑</summary>
		int mBufferFrameRadius { get; }
		/// <summary>車框半徑。車框 = 車身安全框 + Buffer 框</summary>
		int mTotalFrameRadius { get; }
		/// <summary>是否可被干預</summary>
		bool mIsInterveneAvailable { get; }
		/// <summary>是否被干預中</summary>
		bool mIsBeingIntervened { get; }
		/// <summary>目前被干預中的指令。沒有被干預時，此值會為空字串</summary>
		string mInterveneCommand { get; }
		/// <summary>路徑</summary>
		IEnumerable<IPoint2D> mPath { get; }
		/// <summary>路徑(詳細)</summary>
		IEnumerable<IPoint2D> mPathDetail { get; }
		/// <summary>路徑範圍</summary>
		IRectangle2D mPathRegion { get; }
		/// <summary>IP:Port</summary>
		string mIpPort { get; }
		/// <summary>上次更新時間</summary>
		DateTime mLastUpdated { get; }

		void Set(string Name);
		void Update(string State, IPoint2D Position, double Toward, double Battery, double Velocity, string Target, string AlarmMessage, bool IsInterveneAvailable, bool IsBeingIntervened, string InterveneCommand);
		void Update(IEnumerable<IPoint2D> Path);
		void Update(string IpPort);
		string ToString();
	}
}
