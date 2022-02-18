using LibraryForVM;
using System;
using System.Collections.Generic;

namespace TrafficControlTest.Module.Vehicle
{
	/// <summary>
	/// - 儲存車子的資訊
	/// - 車子的資訊更新時會拋出事件
	/// </summary>
	public interface IVehicleInfo : IItem
	{
		/*
		 * 車子的屬性可分成：
		 * 1. 靜態屬性：資訊來源為自走車本身或人使用者手動設定，且其值「不會」隨著自走車運行狀況而有所變化。例：名稱、安全框大小、電池識別碼、是否提供路徑資料、是否提供雷射資料、負責區域、負責樓層 ... 等
		 * 2. 動態屬性：資訊來源為自走車本身，且其值「會」隨著自走車運行狀況而有所變化。例：座標面向、電量、定位分數、目標站點 ... 等
		 * 3. 自定義動態屬性：自走車本身並不會提供，但可透過「解析自走車屬性變化」或是透過「事件推斷」所得，且其值「會」隨著自走車運行狀況而有所變化。例：是否移動中、是否旋轉中、當前任務識別碼 ... 等
		 */

		#region 靜態屬性
		/// <summary>工作/負責廠區</summary>
		string mWorkingFactory { get; }
		/// <summary>工作/負責樓層</summary>
		string mWorkingFloor { get; }
		/// <summary>工作/負責區域</summary>
		string mWorkingRegion { get; }
		/// <summary>自走車是否提供「速度(平移)」資訊</summary>
		bool mIsHaveTranslationVelocityData { get; }
		/// <summary>自走車是否提供「速度(旋轉)」資訊</summary>
		bool mIsHaveRotationVelocityData { get; }
		/// <summary>自走車是否提供「錯誤訊息」資訊</summary>
		bool mIsHaveErrorMessageData { get; }
		/// <summary>自走車是否提供「路徑」資訊</summary>
		bool mIsHavePathData { get; }
		/// <summary>自走車是否提供「雷射」資訊</summary>
		bool mIsHaveLaserData { get; }
		/// <summary>自走車是否可「遠端取得地圖資訊」</summary>
		bool mIsCanGetMapDataRemotely { get; }
		/// <summary>安全框寬 (mm)</summary>
		int mSafetyFrameWidth { get; }
		/// <summary>安全框長 (mm)</summary>
		int mSafetyFrameHeight { get; }
		/// <summary>安全框半徑 (mm)</summary>
		int mSafetyFrameRadius { get; }
		/// <summary>緩衝框寬 (mm)</summary>
		int mBufferFrameWidth { get; }
		/// <summary>緩衝框長 (mm)</summary>
		int mBufferFrameHeight { get; }
		/// <summary>緩衝框半徑 (mm)</summary>
		int mBufferFrameRadius { get; }
		/// <summary>總車框寬 (mm) = 安全框寬 + 緩衝框寬</summary>
		int mTotalFrameWidth { get; }
		/// <summary>總車框高 (mm) = 安全框高 + 緩衝框高</summary>
		int mTotalFrameHeight { get; }
		/// <summary>總車框半徑 (mm) = 安全框半徑 + 緩衝框半徑</summary>
		int mTotalFrameRadius { get; }
		/// <summary>速度(平移)最大值 (mm/s)</summary>
		double mTranslationVelocityMaximum { get; }
		/// <summary>速度(旋轉)最大值 (deg/s)</summary>
		double mRotationVelocityMaximum { get; }
		/// <summary>當前使用的地圖檔的名稱</summary>
		string mCurrentMapName { get; }
		/// <summary>當前擁有的地圖檔名稱清單</summary>
		IList<string> mCurrentMapNameList { get; }
		/// <summary>IP:Port</summary>
		string mIpPort { get; }
		#endregion

		#region 動態屬性
		/*
		 * 動態屬性可分成：
		 * 1. 必要的：自走車必須提供的屬性，若沒有提供會導致系統無法運行(監看、控制、會車管理)
		 * 2. 選擇性的：自走車選擇性提供的屬性，若沒有提供不會對系統運行(監看、控制、會車管理)造成影響。但若有提供時，系統可以做進一步的優化
		 * 
		 * 必要的：狀態、座標面向、目標點、定位分數、電池電量
		 * 選擇性的：速度、錯誤訊息、路徑、雷射資料
		 */
		/// <summary>當前狀態</summary>
		string mCurrentState { get; }
		/// <summary>上一個狀態</summary>
		string mPreviousState { get; }
		/// <summary>狀態持續時間</summary>
		TimeSpan mCurrentStateDuration { get; }
		/// <summary>當前自走車定義狀態</summary>
		string mCurrentOriState { get; }
		/// <summary>上一個自走車定義狀態</summary>
		string mPreviousOriState { get; }
		/// <summary>自走車定義狀態持續時間</summary>
		TimeSpan mCurrentOriStateDuration { get; }
		/// <summary>位置 (mm)</summary>
		IPoint2D mLocationCoordinate { get; }
		/// <summary>面向 (degree) 範圍為 0 ~ 360</summary>
		double mLocationToward { get; }
		/// <summary>當前移動目標點</summary>
		string mCurrentTarget { get; }
		/// <summary>上一個移動目標點</summary>
		string mPreviousTarget { get; }
		/// <summary>定位分數 (%)</summary>
		double mLocationScore { get; }
		/// <summary>電池電量 (%)</summary>
		double mBatteryValue { get; }
		/// <summary>速度(平移) (mm/s)</summary>
		double mTranslationVelocity { get; }
		/// <summary>速度(旋轉) (deg/s)</summary>
		double mRotationVelocity { get; }
		/// <summary>平均速度(平移) (mm/s) 。收集最近 n 筆的速度(平移)數據來做平均</summary>
		double mAverageTranslationVelocity { get; }
		/// <summary>平均速度(旋轉) (deg/s) 。收集最近 n 筆的速度(旋轉)數據來做平均</summary>
		double mAverageRotationVelocity { get; }
		/// <summary>錯誤訊息</summary>
		string mErrorMessage { get; }
		/// <summary>路徑</summary>
		IList<IPoint2D> mPath { get; }
		/// <summary>路徑(詳細)。路徑細切</summary>
		IList<IPoint2D> mPathDetail { get; }
		/// <summary>路徑範圍。覆蓋路徑的最小長方形</summary>
		IRectangle2D mPathRegion { get; }
		/// <summary>路徑字串。格式為 (X1,Y1)(X2,Y2)(X3,Y3)</summary>
		string mPathString { get; }
		/// <summary>雷射資料</summary>
		IList<IPoint2D> mLaserData { get; }
		#endregion

		#region 自定義動態屬性
		/// <summary>預估路徑</summary>
		IList<IPoint2D> mEstimatedPath { get; }
		/// <summary>預估路徑(詳細)。預估路徑細切</summary>
		IList<IPoint2D> mEstimatedPathDetail { get; }
		/// <summary>預估路徑範圍。覆蓋預估路經的最小長方形</summary>
		IRectangle2D mEstimatedPathRegion { get; }
		/// <summary>預估路徑字串。格式為 (X1,Y1)(X2,Y2)(X3,Y3)</summary>
		string mEstimatedPathString { get; }
		/// <summary>當前執行的任務識別碼。沒有執行任務時為空值</summary>
		string mCurrentMissionId { get; }
		/// <summary>上一個執行的任務識別碼</summary>
		string mPreviousMissionId { get; }
		/// <summary>是否正在執行任務</summary>
		bool mIsBeingAssignedMission { get; }
		/// <summary>上次被指派任務時間</summary>
		DateTime mTimestampOfBeingAssignedMission { get; }
		/// <summary>當前執行的干預指令。沒有執行干預時為空值</summary>
		string mCurrentInterveneCommand { get; }
		/// <summary>上一個執行的干預指令</summary>
		string mPreviousInterveneCommand { get; }
		/// <summary>是否正在被干預中</summary>
		bool mIsBeingIntervened { get; }
		/// <summary>上次被干預時間</summary>
		DateTime mTimestampOfBeingIntervened { get; }
		/// <summary>是否移動(平移)中</summary>
		bool mIsTranslating { get; }
		/// <summary>移動(平移)持續時間。 mIsTranslating 為 true 時此為移動(平移)持續時間，為 false 時此為停止移動(平移)持續時間</summary>
		TimeSpan mTranslatingDuration { get; }
		/// <summary>是否移動(旋轉)中</summary>
		bool mIsRotating { get; }
		/// <summary>移動(旋轉)持續時間。 mIsRotating 為 true 時此為移動(旋轉)持續時間，為 false 時此為停止移動(旋轉)持續時間</summary>
		TimeSpan mRotatingDuration { get; }
		/// <summary>上次更新時間</summary>
		DateTime mLastUpdated { get; }
		#endregion

		void Set(string Name);
		/// <summary>開始收集更新事件資訊</summary>
		void BeginUpdate();
		/// <summary>結束收集更新事件資訊並發出更新事件</summary>
		void EndUpdate();

		void UpdateWorkingFactory(string WorkingFactory);
		void UpdateWorkingFloor(string WorkingFloor);
		void UpdateWorkingRegion(string WorkingRegion);
		void UpdateIsHaveTranslationVelocityData(bool IsHaveTranslationVelocityData);
		void UpdateIsHaveRotationVelocityData(bool IsHaveRotationVelocityData);
		void UpdateIsHaveErrorMessageData(bool IsHaveErrorMessageData);
		void UpdateIsHavePathData(bool IsHavePathData);
		void UpdateIsHaveLaserData(bool IsHaveLaserData);
		void UpdateIsCanGetMapDataRemotely(bool IsCanGetMapDataRemotely);
		void UpdateSafetyFrameWidth(int SafetyFrameWidth);
		void UpdateSafetyFrameHeight(int SafetyFrameHeight);
		void UpdateBufferFrameWidth(int BufferFrameWidth);
		void UpdateBufferFrameHeight(int BufferFrameHeight);
		void UpdateTranslationVelocityMaximum(double TranslationVelocityMaximum);
		void UpdateRotationVelocityMaximum(double RotationVelocityMaximum);
		void UpdateCurrentMapName(string MapName);
		void UpdateCurrentMapNameList(IEnumerable<string> MapNameList);
		void UpdateIpPort(string IpPort);

		void UpdateCurrentState(string CurrentState);
		void UpdateCurrentOriState(string CurrentOriState);
		void UpdateLocationCoordinate(IPoint2D LocationCoordinate);
		void UpdateLocationToward(double LocationToward);
		void UpdateCurrentTarget(string Target);
		void UpdateLocationScore(double LocationScore);
		void UpdateBatteryValue(double BatteryValue);
		void UpdateTranslationVelocity(double TranslationVelocity);
		void UpdateRotationVelocity(double RotationVelocity);
		void UpdateErrorMessage(string ErrorMessage);
		void UpdatePath(IEnumerable<IPoint2D> Path);
		void UpdateLaserData(IEnumerable<IPoint2D> LaserData);

		void UpdateEstimatedPath(IEnumerable<IPoint2D> EstimatedPath);
		void UpdateCurrentMissionId(string MissionId);
		void UpdateCurrentInterveneCommand(string InterveneCommand);

		string ToString();
	}
}
