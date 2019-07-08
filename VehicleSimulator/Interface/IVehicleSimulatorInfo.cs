using System.Collections.Generic;
using TrafficControlTest.Interface;
using static VehicleSimulator.Library.EventHandlerLibraryOfIVehicleSimulator;

namespace VehicleSimulator.Interface
{
	public interface IVehicleSimulatorInfo
	{
		event EventHandlerIVehicleSimulator StateUpdated;

		string mName { get; }
		/// <summary>當前狀態</summary>
		string mState { get; set; }
		/// <summary>位置 (mm)</summary>
		IPoint2D mPosition { get; set; }
		/// <summary>面向 (degree)</summary>
		double mToward { get; set; }
		/// <summary>當前移動目標點</summary>
		string mTarget { get; set; }
		/// <summary>當前移動目標 Buffer 點</summary>
		IPoint2D mBufferTarget { get; set; }
		/// <summary>平移速度(mm/s)</summary>
		double mTranslationVelocity { get; set; }
		/// <summary>旋轉速度(mm/s)</summary>
		double mRotationVeloctiy { get; set; }
		/// <summary>匹配度 (%)</summary>
		double mMapMatch { get; set; }
		/// <summary>電池電量 (%)</summary>
		double mBattery { get; set; }
		/// <summary>前方是否有物體擋住導致無法移動</summary>
		bool mPathBlocked { get; set; }
		/// <summary>錯誤訊息</summary>
		string mAlarmMessage { get; set; }
		/// <summary>安全框半徑</summary>
		int mSafetyFrameRadius { get; set; }
		/// <summary>是否可被干預</summary>
		bool mIsInterveneAvailable { get; set; }
		/// <summary>是否被干預中</summary>
		bool mIsIntervening { get; set; }
		/// <summary>目前被干預中的指令。沒有被干預時，此值會為空字串</summary>
		string mInterveneCommand { get; set; }
		/// <summary>路徑</summary>
		IEnumerable<IPoint2D> mPath { get; }

		void Set(string mName);
		/// <summary>設定路徑並開始移動</summary>
		void StartMove(IEnumerable<IPoint2D> Path);
		/// <summary>停止移動</summary>
		void StopMove();
		/// <summary>暫停移動</summary>
		void PauseMove();
		/// <summary>繼續移動</summary>
		void ResumeMove();
		string ToString();
	}
}
