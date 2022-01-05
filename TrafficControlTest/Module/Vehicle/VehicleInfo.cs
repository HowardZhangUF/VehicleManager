using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.Vehicle
{
	class VehicleInfo : IVehicleInfo
	{
		public event EventHandler<StatusUpdatedEventArgs> StatusUpdated;

		public string mName { get; private set; } = string.Empty;
		public string mWorkingFactory { get; private set; } = string.Empty;
		public string mWorkingFloor { get; private set; } = string.Empty;
		public string mWorkingRegion { get; private set; } = string.Empty;
		public bool mIsHaveTranslationVelocityData { get; private set; } = false;
		public bool mIsHaveRotationVelocityData { get; private set; } = false;
		public bool mIsHaveErrorMessageData { get; private set; } = false;
		public bool mIsHavePathData { get; private set; } = false;
		public bool mIsHaveLaserData { get; private set; } = false;
		public bool mIsCanGetMapDataRemotely { get; private set; } = false;
		public int mSafetyFrameWidth { get; private set; } = 0;
		public int mSafetyFrameHeight { get; private set; } = 0;
		public int mSafetyFrameRadius { get { return Math.Max(mSafetyFrameWidth, mSafetyFrameHeight); } }
		public int mBufferFrameWidth { get; private set; } = 0;
		public int mBufferFrameHeight { get; private set; } = 0;
		public int mBufferFrameRadius { get { return Math.Max(mBufferFrameWidth, mBufferFrameHeight); } }
		public int mTotalFrameWidth { get { return mSafetyFrameWidth + mBufferFrameWidth; } }
		public int mTotalFrameHeight { get { return mSafetyFrameHeight + mBufferFrameHeight; } }
		public int mTotalFrameRadius { get { return Math.Max(mTotalFrameWidth, mTotalFrameHeight); } }
		public double mTranslationVelocityMaximum { get; private set; } = 0.0f;
		public double mRotationVelocityMaximum { get; private set; } = 0.0f;
		public string mCurrentMapName { get; private set; } = string.Empty;
		public IList<string> mCurrentMapNameList { get; private set; } = new List<string>();
		public string mIpPort { get; private set; } = string.Empty;

		public string mCurrentState { get; private set; } = string.Empty;
		public string mPreviousState { get; private set; } = string.Empty;
		public TimeSpan mCurrentStateDuration { get { return DateTime.Now.Subtract(mStateStartTimestamp); } }
		public string mCurrentOriState { get; private set; } = string.Empty;
		public string mPreviousOriState { get; private set; } = string.Empty;
		public TimeSpan mCurrentOriStateDuration { get { return DateTime.Now.Subtract(mOriStateStartTimestamp); } }
		public IPoint2D mLocationCoordinate { get; private set; } = Library.Library.GenerateIPoint2D(0, 0);
		public double mLocationToward { get; private set; } = 0.0f;
		public string mCurrentTarget { get; private set; } = string.Empty;
		public string mPreviousTarget { get; private set; } = string.Empty;
		public double mLocationScore { get; private set; } = 0.0f;
		public double mBatteryValue { get; private set; } = 0.0f;
		public double mTranslationVelocity { get; private set; } = 0.0f;
		public double mRotationVelocity { get; private set; } = 0.0f;
		public double mAverageTranslationVelocity { get { return mRecordOfTranslationVelocity.ToList().Sum() / mRecordOfTranslationVelocity.ToList().Count; } }
		public double mAverageRotationVelocity { get { return mRecordOfRotationVelocity.ToList().Sum() / mRecordOfRotationVelocity.ToList().Count; } }
		public string mErrorMessage { get; private set; } = string.Empty;
		public IList<IPoint2D> mPath { get; private set; } = new List<IPoint2D>();
		public IList<IPoint2D> mPathDetail
		{
			get
			{
				if (_PathDetail == null) _PathDetail = CalculatePathDetail(mLocationCoordinate, mPath, (mTotalFrameRadius) / 10);
				return _PathDetail;
			}
		}
		public IRectangle2D mPathRegion
		{
			get
			{
				if (_PathRegion == null) _PathRegion = CalculatePathRegion(mLocationCoordinate, mPath, mTotalFrameRadius);
				return _PathRegion;
			}
		}
		public string mPathString { get { return ConvertToString(mPath); } }
		public IList<IPoint2D> mLaserData { get; private set; } = new List<IPoint2D>();

		public IList<IPoint2D> mEstimatedPath { get; private set; } = new List<IPoint2D>();
		public IList<IPoint2D> mEstimatedPathDetail
		{
			get
			{
				if (_EstimatedPathDetail == null) _EstimatedPathDetail = CalculatePathDetail(mLocationCoordinate, mEstimatedPath, (mTotalFrameRadius) / 10);
				return _EstimatedPathDetail;
			}
		}
		public IRectangle2D mEstimatedPathRegion
		{
			get
			{
				if (_EstimatedPathRegion == null) _EstimatedPathRegion = CalculatePathRegion(mLocationCoordinate, mEstimatedPath, mTotalFrameRadius);
				return _EstimatedPathRegion;
			}
		}
		public string mEstimatedPathString { get { return ConvertToString(mEstimatedPath); } }
		public string mCurrentMissionId { get; private set; } = string.Empty;
		public string mPreviousMissionId { get; private set; } = string.Empty;
		public bool mIsBeingAssignedMission { get { return !string.IsNullOrEmpty(mCurrentMissionId); } }
		public DateTime mTimestampOfBeingAssignedMission { get; private set; } = default(DateTime);
		public string mCurrentInterveneCommand { get; private set; } = string.Empty;
		public string mPreviousInterveneCommand { get; private set; } = string.Empty;
		public bool mIsBeingIntervened { get { return !string.IsNullOrEmpty(mCurrentInterveneCommand); } }
		public DateTime mTimestampOfBeingIntervened { get; private set; } = default(DateTime);
		public bool mIsTranslating { get; private set; } = false;
		public TimeSpan mTranslatingDuration { get { return DateTime.Now.Subtract(mTimestampOfIsTranslatingChanged); } }
		public bool mIsRotating { get; private set; } = false;
		public TimeSpan mRotatingDuration { get { return DateTime.Now.Subtract(mTimestampOfIsRotatingChanged); } }
		public DateTime mLastUpdated { get; private set; } = default(DateTime);

		private DateTime mStateStartTimestamp = DateTime.Now;
		private DateTime mOriStateStartTimestamp = DateTime.Now;
		private List<RecordOfLocationCoordinate> mRecordOfLocationCoordinate = new List<RecordOfLocationCoordinate>();
		private List<RecordOfLocationToward> mRecordOfLocationToward = new List<RecordOfLocationToward>();
		private List<double> mRecordOfLocationScore = new List<double>();
		private List<double> mRecordOfTranslationVelocity = new List<double>();
		private List<double> mRecordOfRotationVelocity = new List<double>();
		private int mLocationCoordinateDataCount = 10;
		private int mLocationTowardDataCount = 10;
		private int mLocationScoreDataCount = 10;
		private int mTranslationVelocityDataCount = 10;
		private int mRotationVelocityDataCount = 10;
		private IList<IPoint2D> _PathDetail = null;
		private IRectangle2D _PathRegion = null;
		private IList<IPoint2D> _EstimatedPathDetail = null;
		private IRectangle2D _EstimatedPathRegion = null;
		private double mThresholdOfTranslating = 0.0f;
		private DateTime mTimestampOfIsTranslatingChanged = default(DateTime);
		private double mThresholdOfRotating = 0.0f;
		private DateTime mTimestampOfIsRotatingChanged = default(DateTime);

		private List<string> mUpdatedItems = new List<string>();
		private bool mIsUpdating = false;

		public VehicleInfo(string Name)
		{
			Set(Name);
			BeginUpdate();
			UpdateTranslationVelocityMaximum(700.0f);
			UpdateRotationVelocityMaximum(20.0f);
			UpdateSafetyFrameWidth(700);
			UpdateSafetyFrameHeight(700);
			UpdateBufferFrameWidth(100);
			UpdateBufferFrameHeight(100);
			EndUpdate();
		}
		public void Set(string Name)
		{
			mName = Name;
			mLastUpdated = DateTime.Now;
			RaiseEvent_StatusUpdated("Name");
		}
		public void BeginUpdate()
		{
			mIsUpdating = true;
		}
		public void EndUpdate()
		{
			mIsUpdating = false;
			if (mUpdatedItems.Count > 0)
			{
				mLastUpdated = DateTime.Now;
				RaiseEvent_StatusUpdated(string.Join(",", mUpdatedItems));
				mUpdatedItems.Clear();
			}
		}

		public void UpdateWorkingFactory(string WorkingFactory)
		{
			if (TryUpdateWorkingFactory(WorkingFactory))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("WorkingFactory");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("WorkingFactory");
				}
			}
		}
		public void UpdateWorkingFloor(string WorkingFloor)
		{
			if (TryUpdateWorkingFloor(WorkingFloor))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("WorkingFloor");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("WorkingFloor");
				}
			}
		}
		public void UpdateWorkingRegion(string WorkingRegion)
		{
			if (TryUpdateWorkingRegion(WorkingRegion))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("WorkingFloor");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("WorkingFloor");
				}
			}
		}
		public void UpdateIsHaveTranslationVelocityData(bool IsHaveTranslationVelocityData)
		{
			if (TryUpdateIsHaveTranslationVelocityData(IsHaveTranslationVelocityData))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("IsHaveTranslationVelocityData");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("IsHaveTranslationVelocityData");
				}
			}
		}
		public void UpdateIsHaveRotationVelocityData(bool IsHaveRotationVelocityData)
		{
			if (TryUpdateIsHaveRotationVelocityData(IsHaveRotationVelocityData))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("IsHaveRotationVelocityData");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("IsHaveRotationVelocityData");
				}
			}
		}
		public void UpdateIsHaveErrorMessageData(bool IsHaveErrorMessageData)
		{
			if (TryUpdateIsHaveErrorMessageData(IsHaveErrorMessageData))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("IsHaveErrorMessageData");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("IsHaveErrorMessageData");
				}
			}
		}
		public void UpdateIsHavePathData(bool IsHavePathData)
		{
			if (TryUpdateIsHavePathData(IsHavePathData))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("IsHavePathData");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("IsHavePathData");
				}
			}
		}
		public void UpdateIsHaveLaserData(bool IsHaveLaserData)
		{
			if (TryUpdateIsHaveLaserData(IsHaveLaserData))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("IsHaveLaserData");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("IsHaveLaserData");
				}
			}
		}
		public void UpdateIsCanGetMapDataRemotely(bool IsCanGetMapDataRemotely)
		{
			if (TryUpdateIsCanGetMapDataRemotely(IsCanGetMapDataRemotely))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("IsCanGetMapDataRemotely");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("IsCanGetMapDataRemotely");
				}
			}
		}
		public void UpdateSafetyFrameWidth(int SafetyFrameWidth)
		{
			if (TryUpdateSafetyFrameWidth(SafetyFrameWidth))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("SafetyFrameWidth");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("SafetyFrameWidth");
				}
			}
		}
		public void UpdateSafetyFrameHeight(int SafetyFrameHeight)
		{
			if (TryUpdateSafetyFrameHeight(SafetyFrameHeight))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("SafetyFrameHeight");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("SafetyFrameHeight");
				}
			}
		}
		public void UpdateBufferFrameWidth(int BufferFrameWidth)
		{
			if (TryUpdateBufferFrameWidth(BufferFrameWidth))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("BufferFrameWidth");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("BufferFrameWidth");
				}
			}
		}
		public void UpdateBufferFrameHeight(int BufferFrameHeight)
		{
			if (TryUpdateBufferFrameHeight(BufferFrameHeight))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("BufferFrameHeight");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("BufferFrameHeight");
				}
			}
		}
		public void UpdateTranslationVelocityMaximum(double TranslationVelocityMaximum)
		{
			if (TryUpdateTranslationVelocityMaximum(TranslationVelocityMaximum))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("TranslationVelocityMaximum");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("TranslationVelocityMaximum");
				}
			}
		}
		public void UpdateRotationVelocityMaximum(double RotationVelocityMaximum)
		{
			if (TryUpdateRotationVelocityMaximum(RotationVelocityMaximum))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("RotationVelocityMaximum");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("RotationVelocityMaximum");
				}
			}
		}
		public void UpdateCurrentMapName(string CurrentMapName)
		{
			if (TryUpdateCurrentMapName(CurrentMapName))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("CurrentMapName");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("CurrentMapName");
				}
			}
		}
		public void UpdateCurrentMapNameList(IEnumerable<string> CurrentMapNameList)
		{
			if (TryUpdateCurrentMapNameList(CurrentMapNameList))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("CurrentMapNameList");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("CurrentMapNameList");
				}
			}
		}
		public void UpdateIpPort(string IpPort)
		{
			if (TryUpdateIpPort(IpPort))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("IpPort");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("IpPort");
				}
			}
		}

		public void UpdateCurrentState(string CurrentState)
		{
			if (TryUpdateCurrentState(CurrentState))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("CurrentState");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("CurrentState");
				}
			}
		}
		public void UpdateCurrentOriState(string CurrentOriState)
		{
			if (TryUpdateCurrentOriState(CurrentOriState))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("CurrentOriState");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("CurrentOriState");
				}
			}
		}
		public void UpdateLocationCoordinate(IPoint2D LocationCoordinate)
		{
			if (TryUpdateLocationCoordinate(LocationCoordinate))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("LocationCoordinate");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("LocationCoordinate");
				}
			}

			// 在解析自走車資料時，齊資料都是從 Queue 中拿出收到的資料並更新此 VehicleInfo ，
			// 但是更新的時間點並非是實際收到資料的時間點，所以在使用時間來計算速度/速率時，基本上都會有誤差
			// 加上，自走車不動時，定位仍會有些許飄動，導致座標飄動，而也有可能因此誤判自走車開始移動/旋轉，所以暫時將 IsTranslating/IsRotating 的功能關閉。
			//if (mRecordOfLocationCoordinate.Count > 1)
			//{
			//	var curr = mRecordOfLocationCoordinate[mRecordOfLocationCoordinate.Count - 1];
			//	var prev = mRecordOfLocationCoordinate[mRecordOfLocationCoordinate.Count - 2];

			//	// 時間差異必須大於 90 ms
			//	if (curr.mTimestamp.Subtract(prev.mTimestamp).TotalMilliseconds > 90.0f)
			//	{
			//		var translationVelocity = CalculateTranslationVelocity(curr, prev);
			//		// 當「當前速度」大於「最高速的十分之一」時，判斷為正在移動。反之為靜止
			//		UpdateIsTranslating(translationVelocity > mThresholdOfTranslating ? true : false);
			//	}
			//}
		}
		public void UpdateLocationToward(double LocationToward)
		{
			if (TryUpdateLocationToward(LocationToward))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("LocationToward");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("LocationToward");
				}
			}

			// 在解析自走車資料時，齊資料都是從 Queue 中拿出收到的資料並更新此 VehicleInfo ，
			// 但是更新的時間點並非是實際收到資料的時間點，所以在使用時間來計算速度/速率時，基本上都會有誤差
			// 加上，自走車不動時，定位仍會有些許飄動，導致座標飄動，而也有可能因此誤判自走車開始移動/旋轉，所以暫時將 IsTranslating/IsRotating 的功能關閉。
			//if (mRecordOfLocationToward.Count > 1)
			//{
			//	var curr = mRecordOfLocationToward[mRecordOfLocationToward.Count - 1];
			//	var prev = mRecordOfLocationToward[mRecordOfLocationToward.Count - 2];

			//	// 時間差異必須大於 90 ms
			//	if (curr.mTimestamp.Subtract(prev.mTimestamp).TotalMilliseconds > 90.0f)
			//	{
			//		var rotationVelocity = CalculateRotationVelocity(curr, prev);
			//		// 當「當前速度」大於「最高速的十分之一」時，判斷為正在移動。反之為靜止
			//		UpdateIsRotating(rotationVelocity > mThresholdOfRotating ? true : false);
			//	}
			//}
		}
		public void UpdateCurrentTarget(string CurrentTarget)
		{
			if (TryUpdateCurrentTarget(CurrentTarget))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("CurrentTarget");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("CurrentTarget");
				}
			}
		}
		public void UpdateLocationScore(double LocationScore)
		{
			if (TryUpdateLocationScore(LocationScore))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("LocationScore");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("LocationScore");
				}
			}
		}
		public void UpdateBatteryValue(double BatteryValue)
		{
			if (TryUpdateBatteryValue(BatteryValue))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("BatteryValue");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("BatteryValue");
				}
			}
		}
		public void UpdateTranslationVelocity(double TranslationVelocity)
		{
			if (TryUpdateTranslationVelocity(TranslationVelocity))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("TranslationVelocity");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("TranslationVelocity");
				}
			}
		}
		public void UpdateRotationVelocity(double RotationVelocity)
		{
			if (TryUpdateTranslationVelocity(RotationVelocity))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("RotationVelocity");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("RotationVelocity");
				}
			}
		}
		public void UpdateErrorMessage(string ErrorMessage)
		{
			if (TryUpdateErrorMessage(ErrorMessage))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("ErrorMessage");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("ErrorMessage");
				}
			}
		}
		public void UpdatePath(IEnumerable<IPoint2D> Path)
		{
			if (TryUpdatePath(Path))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("Path");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("Path");
				}
			}
		}
		public void UpdateLaserData(IEnumerable<IPoint2D> LaserData)
		{
			if (TryUpdateLaserData(LaserData))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("LaserData");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("LaserData");
				}
			}
		}

		public void UpdateEstimatedPath(IEnumerable<IPoint2D> EstimatedPath)
		{
			if (TryUpdateEstimatedPath(EstimatedPath))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("EstimatedPath");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("EstimatedPath");
				}
			}
		}
		public void UpdateCurrentMissionId(string CurrentMissionId)
		{
			if (TryUpdateCurrentMissionId(CurrentMissionId))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("CurrentMissionId");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("CurrentMissionId");
				}
			}
		}
		public void UpdateCurrentInterveneCommand(string CurrentInterveneCommand)
		{
			if (TryUpdateCurrentInterveneCommand(CurrentInterveneCommand))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("CurrentInterveneCommand");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("CurrentInterveneCommand");
				}
			}
		}
		public void UpdateIsTranslating(bool IsTranslating)
		{
			if (TryUpdateIsTranslating(IsTranslating))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("IsTranslating");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("IsTranslating");
				}
			}
		}
		public void UpdateIsRotating(bool IsRotating)
		{
			if (TryUpdateIsRotating(IsRotating))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("IsRotating");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("IsRotating");
				}
			}
		}
		public override string ToString()
		{
			string result = string.Empty;
			result += $"{mName}/";
			result += $"{mCurrentState}/";
			result += $"{mCurrentOriState}/";
			result += $"{(mLocationCoordinate != null ? $"({mLocationCoordinate.mX},{mLocationCoordinate.mY},{mLocationToward.ToString("F2")})" : string.Empty)}/";
			result += $"{mCurrentTarget}/";
			result += $"{mLocationScore.ToString("F2")}(%)/";
			result += $"{mBatteryValue.ToString("F2")}(%)/";
			result += $"{mCurrentMissionId}/";
			result += $"{mCurrentInterveneCommand}";
			return result;
		}

		protected virtual void RaiseEvent_StatusUpdated(string StatusName, bool Sync = true)
		{
			if (Sync)
			{
				StatusUpdated?.Invoke(this, new StatusUpdatedEventArgs(DateTime.Now, mName, StatusName));
			}
			else
			{
				Task.Run(() => { StatusUpdated?.Invoke(this, new StatusUpdatedEventArgs(DateTime.Now, mName, StatusName)); });
			}
		}
		private static IList<IPoint2D> CalculatePathDetail(IPoint2D CurrentPosition, IEnumerable<IPoint2D> Path, int Interval)
		{
			List<IPoint2D> result = null;
			List<IPoint2D> tmpPath = new List<IPoint2D>();
			if (CurrentPosition != null) tmpPath.Add(CurrentPosition);
			if (Path != null && Path.Count() > 0) tmpPath.AddRange(Path);
			if (tmpPath.Count > 0)
			{
				result = new List<IPoint2D>();
				tmpPath.Reverse(); // 計算路徑詳細點時，先將路徑點順序反轉，從終點開始往回算
				result.Add(tmpPath[0]);
				for (int i = 1; i < tmpPath.Count; ++i)
				{
					result.AddRange(Library.Library.ConvertLineToPoints(tmpPath[i - 1], tmpPath[i], Interval));
					result.Add(tmpPath[i]);
				}
				result.Reverse(); // 計算路徑詳細點完成時，再將其順序反轉，以符合起點是車子當前位置以及終點是路徑點末點的順序
			}
			return result;
		}
		private static IRectangle2D CalculatePathRegion(IPoint2D CurrentPosition, IEnumerable<IPoint2D> Path, int Amplify)
		{
			IRectangle2D result = null;
			List<IPoint2D> tmpPath = new List<IPoint2D>();
			if (CurrentPosition != null) tmpPath.Add(CurrentPosition);
			if (Path != null && Path.Count() > 0) tmpPath.AddRange(Path);
			if (tmpPath.Count > 0)
			{
				result = Library.Library.GetCoverRectangle(tmpPath);
				result = Library.Library.GetAmplifyRectangle(result, Amplify, Amplify);
			}
			return result;
		}
		private static string ConvertToString(IEnumerable<IPoint2D> Points)
		{
			if (Points == null || Points.Count() == 0) return string.Empty;
			else return string.Join(string.Empty, Points.Select(o => o.ToString()));
		}
		private bool TryUpdateWorkingFactory(string WorkingFactory)
		{
			bool result = false;
			if (mWorkingFactory != WorkingFactory)
			{
				mWorkingFactory = WorkingFactory ?? string.Empty;
				result = true;
			}
			return result;
		}
		private bool TryUpdateWorkingFloor(string WorkingFloor)
		{
			bool result = false;
			if (mWorkingFloor != WorkingFloor)
			{
				mWorkingFloor = WorkingFloor ?? string.Empty;
				result = true;
			}
			return result;
		}
		private bool TryUpdateWorkingRegion(string WorkingRegion)
		{
			bool result = false;
			if (mWorkingRegion != WorkingRegion)
			{
				mWorkingRegion = WorkingRegion ?? string.Empty;
				result = true;
			}
			return result;
		}
		private bool TryUpdateIsHaveTranslationVelocityData(bool IsHaveTranslationVelocityData)
		{
			bool result = false;
			if (mIsHaveTranslationVelocityData != IsHaveTranslationVelocityData)
			{
				mIsHaveTranslationVelocityData = IsHaveTranslationVelocityData;
				result = true;
			}
			return result;
		}
		private bool TryUpdateIsHaveRotationVelocityData(bool IsHaveRotationVelocityData)
		{
			bool result = false;
			if (mIsHaveRotationVelocityData != IsHaveRotationVelocityData)
			{
				mIsHaveRotationVelocityData = IsHaveRotationVelocityData;
				result = true;
			}
			return result;
		}
		private bool TryUpdateIsHaveErrorMessageData(bool IsHaveErrorMessageData)
		{
			bool result = false;
			if (mIsHaveErrorMessageData != IsHaveErrorMessageData)
			{
				mIsHaveErrorMessageData = IsHaveErrorMessageData;
				result = true;
			}
			return result;
		}
		private bool TryUpdateIsHavePathData(bool IsHavePathData)
		{
			bool result = false;
			if (mIsHavePathData != IsHavePathData)
			{
				mIsHavePathData = IsHavePathData;
				result = true;
			}
			return result;
		}
		private bool TryUpdateIsHaveLaserData(bool IsHaveLaserData)
		{
			bool result = false;
			if (mIsHaveLaserData != IsHaveLaserData)
			{
				mIsHaveLaserData = IsHaveLaserData;
				result = true;
			}
			return result;
		}
		private bool TryUpdateIsCanGetMapDataRemotely(bool IsCanGetMapDataRemotely)
		{
			bool result = false;
			if (mIsCanGetMapDataRemotely != IsCanGetMapDataRemotely)
			{
				mIsCanGetMapDataRemotely = IsCanGetMapDataRemotely;
				result = true;
			}
			return result;
		}
		private bool TryUpdateSafetyFrameWidth(int SafetyFrameWidth)
		{
			bool result = false;
			if (mSafetyFrameWidth != SafetyFrameWidth)
			{
				mSafetyFrameWidth = SafetyFrameWidth;
				result = true;
			}
			return result;
		}
		private bool TryUpdateSafetyFrameHeight(int SafetyFrameHeight)
		{
			bool result = false;
			if (mSafetyFrameHeight != SafetyFrameHeight)
			{
				mSafetyFrameHeight = SafetyFrameHeight;
				result = true;
			}
			return result;
		}
		private bool TryUpdateBufferFrameWidth(int BufferFrameWidth)
		{
			bool result = false;
			if (mBufferFrameWidth != BufferFrameWidth)
			{
				mBufferFrameWidth = BufferFrameWidth;
				result = true;
			}
			return result;
		}
		private bool TryUpdateBufferFrameHeight(int BufferFrameHeight)
		{
			bool result = false;
			if (mBufferFrameHeight != BufferFrameHeight)
			{
				mBufferFrameHeight = BufferFrameHeight;
				result = true;
			}
			return result;
		}
		private bool TryUpdateTranslationVelocityMaximum(double TranslationVelocityMaximum)
		{
			bool result = false;
			if (mTranslationVelocityMaximum != TranslationVelocityMaximum)
			{
				mTranslationVelocityMaximum = TranslationVelocityMaximum;
				mThresholdOfTranslating = mTranslationVelocityMaximum / 10;
				result = true;
			}
			return result;
		}
		private bool TryUpdateRotationVelocityMaximum(double RotationVelocityMaximum)
		{
			bool result = false;
			if (mRotationVelocityMaximum != RotationVelocityMaximum)
			{
				mRotationVelocityMaximum = RotationVelocityMaximum;
				mThresholdOfRotating = mRotationVelocityMaximum / 10;
				result = true;
			}
			return result;
		}
		private bool TryUpdateCurrentMapName(string MapName)
		{
			bool result = false;
			// 不做新舊地圖名的比較，因為有時候會有檔案內容有修改但檔案名稱卻不修改的情況
			mCurrentMapName = MapName ?? string.Empty;
			result = true;
			return result;
		}
		private bool TryUpdateCurrentMapNameList(IEnumerable<string> MapNameList)
		{
			bool result = false;
			if (MapNameList != null)
			{
				mCurrentMapNameList = MapNameList.ToList();
				result = true;
			}
			return result;
		}
		private bool TryUpdateIpPort(string IpPort)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(IpPort) && mIpPort != IpPort)
			{
				mIpPort = IpPort;
				result = true;
			}
			return result;
		}
		private bool TryUpdateCurrentState(string CurrentState)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(CurrentState) && mCurrentState != CurrentState)
			{
				mPreviousState = mCurrentState;
				mCurrentState = CurrentState;
				mStateStartTimestamp = DateTime.Now;
				result = true;
			}
			return result;
		}
		private bool TryUpdateCurrentOriState(string CurrentOriState)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(CurrentOriState) && mCurrentOriState != CurrentOriState)
			{
				mPreviousOriState = mCurrentOriState;
				mCurrentOriState = CurrentOriState;
				mOriStateStartTimestamp = DateTime.Now;
				result = true;
			}
			return result;
		}
		private bool TryUpdateLocationCoordinate(IPoint2D LocationCoordinate)
		{
			bool result = false;

			// Update Record of LocationCoordinate
			mRecordOfLocationCoordinate.Add(new RecordOfLocationCoordinate(DateTime.Now, LocationCoordinate.mX, LocationCoordinate.mY));
			if (mRecordOfLocationCoordinate.Count > mLocationCoordinateDataCount) mRecordOfLocationCoordinate.RemoveAt(0);

			if (LocationCoordinate != null && (mLocationCoordinate == null || (mLocationCoordinate.mX != LocationCoordinate.mX || mLocationCoordinate.mY != LocationCoordinate.mY)))
			{
				mLocationCoordinate = LocationCoordinate;
				_PathDetail = null;
				_PathRegion = null;
				result = true;
			}
			return result;
		}
		private bool TryUpdateLocationToward(double LocationToward)
		{
			bool result = false;

			// Update Record of LocationToward
			mRecordOfLocationToward.Add(new RecordOfLocationToward(DateTime.Now, LocationToward));
			if (mRecordOfLocationToward.Count > mLocationTowardDataCount) mRecordOfLocationToward.RemoveAt(0);

			if (mLocationToward.ToString("F2") != LocationToward.ToString("F2"))
			{
				mLocationToward = LocationToward;
				result = true;
			}
			return result;
		}
		private bool TryUpdateCurrentTarget(string CurrentTarget)
		{
			bool result = false;
			if (mCurrentTarget != CurrentTarget)
			{
				mPreviousTarget = mCurrentTarget;
				mCurrentTarget = CurrentTarget ?? string.Empty;
				result = true;
			}
			return result;
		}
		private bool TryUpdateLocationScore(double LocationScore)
		{
			bool result = false;
			mRecordOfLocationScore.Add(LocationScore);
			if (mRecordOfLocationScore.Count > mLocationScoreDataCount) mRecordOfLocationScore.RemoveAt(0);
			if (mLocationScore.ToString("F2") != LocationScore.ToString("F2"))
			{
				mLocationScore = LocationScore;
				result = true;
			}
			return result;
		}
		private bool TryUpdateBatteryValue(double BatteryValue)
		{
			bool result = false;
			if (mBatteryValue.ToString("F2") != BatteryValue.ToString("F2"))
			{
				mBatteryValue = BatteryValue;
				result = true;
			}
			return result;
		}
		private bool TryUpdateTranslationVelocity(double TranslationVelocity)
		{
			bool result = false;
			mRecordOfTranslationVelocity.Add(TranslationVelocity);
			if (mRecordOfTranslationVelocity.Count > mTranslationVelocityDataCount) mRecordOfTranslationVelocity.RemoveAt(0);
			if (mTranslationVelocity.ToString("F2") != TranslationVelocity.ToString("F2"))
			{
				mTranslationVelocity = TranslationVelocity;
				result = true;
			}
			return result;
		}
		private bool TryUpdateRotationVelocity(double RotationVelocity)
		{
			bool result = false;
			mRecordOfRotationVelocity.Add(RotationVelocity);
			if (mRecordOfRotationVelocity.Count > mRotationVelocityDataCount) mRecordOfRotationVelocity.RemoveAt(0);
			if (mRotationVelocity.ToString("F2") != RotationVelocity.ToString("F2"))
			{
				mRotationVelocity = RotationVelocity;
				result = true;
			}
			return result;
		}
		private bool TryUpdateErrorMessage(string ErrorMessage)
		{
			bool result = false;
			if (mErrorMessage != ErrorMessage)
			{
				mErrorMessage = ErrorMessage ?? string.Empty;
				result = true;
			}
			return result;
		}
		private bool TryUpdatePath(IEnumerable<IPoint2D> Path)
		{
			bool result = false;
			if (mPath != null && mPath.Count() == 0 && Path != null && Path.Count() == 0) return result;
			if ((mPath == null && Path != null) || (mPath != null && Path != null && ConvertToString(mPath) != ConvertToString(Path)))
			{
				mPath = Path.ToList();
				_PathDetail = null;
				_PathRegion = null;
				result = true;
			}
			return result;
		}
		private bool TryUpdateLaserData(IEnumerable<IPoint2D> LaserData)
		{
			bool result = false;
			if (mLaserData != null && mLaserData.Count() == 0 && LaserData != null && LaserData.Count() == 0) return result;
			if ((mLaserData == null && LaserData != null) || (mLaserData != null && LaserData != null && ConvertToString(mLaserData) != ConvertToString(LaserData)))
			{
				mLaserData = LaserData.ToList();
				result = true;
			}
			return result;
		}
		private bool TryUpdateEstimatedPath(IEnumerable<IPoint2D> EstimatedPath)
		{
			bool result = false;
			if (mEstimatedPath != null && mEstimatedPath.Count() == 0 && EstimatedPath != null && EstimatedPath.Count() == 0) return result;
			if ((mEstimatedPath == null && EstimatedPath != null) || (mEstimatedPath != null && EstimatedPath != null && ConvertToString(mEstimatedPath) != ConvertToString(EstimatedPath)))
			{
				mEstimatedPath = EstimatedPath.ToList();
				_EstimatedPathDetail = null;
				_EstimatedPathRegion = null;
				result = true;
			}
			return result;
		}
		private bool TryUpdateCurrentMissionId(string CurrentMissionId)
		{
			bool result = false;
			if (mCurrentMissionId != CurrentMissionId)
			{
				mPreviousMissionId = mCurrentMissionId;
				mCurrentMissionId = CurrentMissionId ?? string.Empty;
				if (!string.IsNullOrEmpty(mCurrentMissionId)) mTimestampOfBeingAssignedMission = DateTime.Now;
				result = true;
			}
			return result;
		}
		private bool TryUpdateCurrentInterveneCommand(string CurrentInterveneCommand)
		{
			bool result = false;
			if (mCurrentInterveneCommand != CurrentInterveneCommand)
			{
				mPreviousInterveneCommand = mCurrentInterveneCommand;
				mCurrentInterveneCommand = CurrentInterveneCommand ?? string.Empty;
				if (!string.IsNullOrEmpty(mCurrentInterveneCommand)) mTimestampOfBeingIntervened = DateTime.Now;
				result = true;
			}
			return result;
		}
		private bool TryUpdateIsTranslating(bool IsTranslating)
		{
			bool result = false;
			if (mIsTranslating != IsTranslating)
			{
				mIsTranslating = IsTranslating;
				mTimestampOfIsTranslatingChanged = DateTime.Now;
				result = true;
			}
			return result;
		}
		private bool TryUpdateIsRotating(bool IsRotating)
		{
			bool result = false;
			if (mIsRotating != IsRotating)
			{
				mIsRotating = IsRotating;
				mTimestampOfIsRotatingChanged = DateTime.Now;
				result = true;
			}
			return result;
		}
		private double CalculateTranslationVelocity(RecordOfLocationCoordinate Current, RecordOfLocationCoordinate Previous)
		{
			var timeDiff = Current.mTimestamp.Subtract(Previous.mTimestamp).TotalSeconds;
			var distanceDiff = CalculateDistance(Previous.mCoordinate, Current.mCoordinate);
			return distanceDiff / timeDiff;
		}
		private double CalculateDistance(IPoint2D Point1, IPoint2D Point2)
		{
			return CalculateDistance(Point1.mX, Point1.mY, Point2.mX, Point2.mY);
		}
		private double CalculateDistance(int X1, int Y1, int X2, int Y2)
		{
			return Math.Sqrt((X2 - X1) * (X2 - X1) + (Y2 - Y1) * (Y2 - Y1));
		}
		private double CalculateRotationVelocity(RecordOfLocationToward Current, RecordOfLocationToward Previous)
		{
			var timeDiff = Current.mTimestamp.Subtract(Previous.mTimestamp).TotalSeconds;
			var distanceDiff = Math.Abs(Current.mToward - Previous.mToward);
			return distanceDiff / timeDiff;
		}
	}

	public class RecordOfLocationCoordinate
	{
		public DateTime mTimestamp { get; private set; } = default(DateTime);
		public IPoint2D mCoordinate { get; private set; } = default(IPoint2D);

		public RecordOfLocationCoordinate(DateTime Timestamp, IPoint2D Coordinate)
		{
			mTimestamp = Timestamp;
			mCoordinate = Coordinate;
		}
		public RecordOfLocationCoordinate(DateTime Timestamp, int X, int Y)
		{
			mTimestamp = Timestamp;
			mCoordinate = Library.Library.GenerateIPoint2D(X, Y);
		}
	}
	public class RecordOfLocationToward
	{
		public DateTime mTimestamp { get; private set; } = default(DateTime);
		public double mToward { get; private set; } = default(double);

		public RecordOfLocationToward(DateTime Timestamp, double Toward)
		{
			mTimestamp = Timestamp;
			mToward = Toward;
		}
	}
}
