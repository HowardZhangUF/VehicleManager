using Library;
using System.Collections.Generic;
using TrafficControlTest.Module.General;
using static VehicleSimulator.Library.EventHandlerLibraryOfIVehicleSimulator;

namespace VehicleSimulator.Interface
{
	public interface IVehicleSimulatorInfo
	{
		event EventHandlerIVehicleSimulator StateUpdated;

		string mName { get; }
		/// <summary>當前狀態</summary>
		string mState { get; }
		/// <summary>位置 (mm)</summary>
		IPoint2D mPosition { get; }
		/// <summary>面向 (degree)</summary>
		double mToward { get; }
		/// <summary>當前移動目標點</summary>
		string mTarget { get; }
		/// <summary>當前移動目標 Buffer 點</summary>
		IPoint2D mBufferTarget { get; }
		/// <summary>平移速度(mm/s)</summary>
		double mTranslationVelocity { get; }
		/// <summary>旋轉速度(mm/s)</summary>
		double mRotationVelocity { get; }
		/// <summary>匹配度 (%)</summary>
		double mMapMatch { get; }
		/// <summary>電池電量 (%)</summary>
		double mBattery { get; }
		/// <summary>前方是否有物體擋住導致無法移動</summary>
		bool mPathBlocked { get; }
		/// <summary>錯誤訊息</summary>
		string mAlarmMessage { get; }
		/// <summary>安全框半徑</summary>
		int mSafetyFrameRadius { get; }
		/// <summary>是否可被干預</summary>
		bool mIsInterveneAvailable { get; }
		/// <summary>是否被干預中</summary>
		bool mIsBeingIntervened { get; }
		/// <summary>目前被干預中的指令。沒有被干預時，此值會為空字串</summary>
		string mInterveneCommand { get; }
		/// <summary>路徑</summary>
		IEnumerable<IPoint2D> mPath { get; }

		/// <summary>設定路徑並開始移動</summary>
		void StartMove(IEnumerable<IPoint2D> Path);
		/// <summary>設定路徑與目標並開始移動</summary>
		void StartMove(IEnumerable<IPoint2D> Path, string Target);
		/// <summary>停止移動</summary>
		void StopMove();
		/// <summary>暫停移動</summary>
		void PauseMove();
		/// <summary>繼續移動</summary>
		void ResumeMove();
		void Dock();
		void Undock();
		void SetInterveneCommand(string Command, params string[] Paras);
		string ToString();
		string[] ToStringArray();
	}
}
