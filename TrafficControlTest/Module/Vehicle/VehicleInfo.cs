﻿using System;
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
		public string mCurrentState { get; private set; } = string.Empty;
		public string mPreviousState { get; private set; } = string.Empty;
		public TimeSpan mCurrentStateDuration { get { return DateTime.Now.Subtract(mStateStartTimestamp); } }
		public IPoint2D mLocationCoordinate { get; private set; } = Library.Library.GenerateIPoint2D(0, 0);
		public double mLocationToward { get; private set; } = 0.0f;
		public string mCurrentTarget { get; private set; } = string.Empty;
		public string mPreviousTarget { get; private set; } = string.Empty;
		public double mVelocity { get; private set; } = 0.0f;
		public double mAverageVelocity { get { return mRecordOfVelocity.Sum() / mRecordOfVelocity.Count; } }
		public double mLocationScore { get; private set; } = 0.0f;
		public double mBatteryValue { get; private set; } = 0.0f;
		public string mAlarmMessage { get; private set; } = string.Empty;
		public IList<IPoint2D> mPath { get; private set; } = new List<IPoint2D>();
		public IList<IPoint2D> mPathDetail
		{
			get
			{
				if (_PathDetail == null) _PathDetail = CalculatePathDetail(mLocationCoordinate, mPath, (mSafetyFrameRadius + mBufferFrameRadius) / 10);
				return _PathDetail;
			}
		}
		public IRectangle2D mPathRegion
		{
			get
			{
				if (_PathRegion == null) _PathRegion = CalculatePathRegion(mLocationCoordinate, mPath, mSafetyFrameRadius + mBufferFrameRadius);
				return _PathRegion;
			}
		}
		public string mPathString { get { return ConvertToString(mPath); } }
		public string mIpPort { get; private set; } = string.Empty;
		public DateTime mLastUpdated { get; private set; } = DateTime.Now;

		public string mCurrentMissionId { get; private set; } = string.Empty;
		public string mPreviousMissionId { get; private set; } = string.Empty;
		public string mCurrentInterveneCommand { get; private set; } = string.Empty;
		public string mPreviousInterveneCommand { get; private set; } = string.Empty;
		public string mCurrentMapName { get; private set; } = string.Empty;
		public IList<string> mCurrentMapNameList { get; private set; } = new List<string>();

		public double mVelocityMaximum { get; private set; } = Library.Library.DefaultVehicleVelocityMaximum;
		public int mSafetyFrameRadius { get; private set; } = Library.Library.DefaultVehicleSafetyFrameRadius;
		public int mBufferFrameRadius { get; private set; } = Library.Library.DefaultVehicleBufferFrameRadius;
		public int mTotalFrameRadius { get { return mSafetyFrameRadius + mBufferFrameRadius; } }

		private IList<IPoint2D> _PathDetail = null;
		private IRectangle2D _PathRegion = null;
		private DateTime mStateStartTimestamp = DateTime.Now;
		private List<double> mRecordOfVelocity = new List<double>();
		private int mVelocityDataCount = Library.Library.DefaultVehicleAverageVelocityDataCount;
		private List<string> mUpdatedItems = new List<string>();
		private bool mIsUpdating = false;

		public VehicleInfo(string Name)
		{
			Set(Name);
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
		public void UpdateCurrentState(string NewState)
		{
			if (TryUpdateCurrentState(NewState))
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
		public void UpdateLocationCoordinate(IPoint2D NewLocationCoordinate)
		{
			if (TryUpdateLocationCoordinate(NewLocationCoordinate))
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
		}
		public void UpdateLocationToward(double NewLocationToward)
		{
			if (TryUpdateLocationToward(NewLocationToward))
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
		}
		public void UpdateCurrentTarget(string NewTarget)
		{
			if (TryUpdateCurrentTarget(NewTarget))
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
		public void UpdateVelocity(double NewVelocity)
		{
			if (TryUpdateVelocity(NewVelocity))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("Velocity");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("Velocity");
				}
			}
		}
		public void UpdateLocationScore(double NewLocationScore)
		{
			if (TryUpdateLocationScore(NewLocationScore))
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
		public void UpdateBatteryValue(double NewBattery)
		{
			if (TryUpdateBatteryValue(NewBattery))
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
		public void UpdateAlarmMessage(string NewAlarmMessage)
		{
			if (TryUpdateAlarmMessage(NewAlarmMessage))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("AlarmMessage");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("AlarmMessage");
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
		public void UpdateCurrentMissionId(string NewMissionId)
		{
			if (TryUpdateCurrentMissionId(NewMissionId))
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
		public void UpdateCurrentInterveneCommand(string NewInterveneCommand)
		{
			if (TryUpdateCurrentInterveneCommand(NewInterveneCommand))
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
		public void UpdateCurrentMapName(string NewMapName)
		{
			if (TryUpdateCurrentMapName(NewMapName))
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
		public void UpdateCurrentMapNameList(IEnumerable<string> NewMapNameList)
		{
			if (TryUpdateCurrentMapNameList(NewMapNameList))
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
		public void UpdateVelocityMaximum(double NewVelocityMaximum)
		{
			if (TryUpdateVelocityMaximum(NewVelocityMaximum))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("VelocityMaximum");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("VelocityMaximum");
				}
			}
		}
		public void UpdateSafetyFrameRadius(int NewSafetyFrameRadius)
		{
			if (TryUpdateSafetyFrameRadius(NewSafetyFrameRadius))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("SafetyFrameRadius");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("SafetyFrameRadius");
				}
			}
		}
		public void UpdateBufferFrameRadius(int NewBufferFrameRadius)
		{
			if (TryUpdateBufferFrameRadius(NewBufferFrameRadius))
			{
				if (mIsUpdating)
				{
					mUpdatedItems.Add("BufferFrameRadius");
				}
				else
				{
					mLastUpdated = DateTime.Now;
					RaiseEvent_StatusUpdated("BufferFrameRadius");
				}
			}
		}
		public override string ToString()
		{
			string result = string.Empty;
			result += $"{mName}/";
			result += $"{mCurrentState}/";
			result += $"{(mLocationCoordinate != null ? $"({mLocationCoordinate.mX},{mLocationCoordinate.mY},{mLocationToward.ToString("F2")})" : string.Empty)}/";
			result += $"{mLocationScore.ToString("F2")}(%)/";
			result += $"{mCurrentTarget}/";
			result += $"{mPathString}/";
			result += $"{mVelocity.ToString("F2")}(mm/s)/";
			result += $"{mBatteryValue.ToString("F2")}(%)/";
			result += $"{mCurrentMissionId}/";
			result += $"{mCurrentInterveneCommand}/";
			result += $"{mCurrentMapName}/";
			result += $"{mIpPort}";
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
		private bool TryUpdateCurrentState(string NewState)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(NewState) && mCurrentState != NewState)
			{
				mPreviousState = mCurrentState;
				mCurrentState = NewState;
				mStateStartTimestamp = DateTime.Now;
				result = true;
			}
			return result;
		}
		private bool TryUpdateLocationCoordinate(IPoint2D NewLocationCoordinate)
		{
			bool result = false;
			if (NewLocationCoordinate != null && (mLocationCoordinate == null || (mLocationCoordinate.mX != NewLocationCoordinate.mX || mLocationCoordinate.mY != NewLocationCoordinate.mY)))
			{
				mLocationCoordinate = NewLocationCoordinate;
				_PathDetail = null;
				_PathRegion = null;
				result = true;
			}
			return result;
		}
		private bool TryUpdateLocationToward(double NewLocationToward)
		{
			bool result = false;
			if (mLocationToward.ToString("F2") != NewLocationToward.ToString("F2"))
			{
				mLocationToward = NewLocationToward;
				result = true;
			}
			return result;
		}
		private bool TryUpdateCurrentTarget(string NewTarget)
		{
			bool result = false;
			if (mCurrentTarget != NewTarget)
			{
				mPreviousTarget = mCurrentTarget;
				mCurrentTarget = NewTarget ?? string.Empty;
				result = true;
			}
			return result;
		}
		private bool TryUpdateVelocity(double NewVelocity)
		{
			bool result = false;
			mRecordOfVelocity.Add(NewVelocity);
			if (mRecordOfVelocity.Count > mVelocityDataCount) mRecordOfVelocity.RemoveAt(0);
			if (mVelocity.ToString("F2") != NewVelocity.ToString("F2"))
			{
				mVelocity = NewVelocity;
				result = true;
			}
			return result;
		}
		private bool TryUpdateLocationScore(double NewLocationScore)
		{
			bool result = false;
			if (mLocationScore.ToString("F2") != NewLocationScore.ToString("F2"))
			{
				mLocationScore = NewLocationScore;
				result = true;
			}
			return result;
		}
		private bool TryUpdateBatteryValue(double NewBatteryValue)
		{
			bool result = false;
			if (mBatteryValue.ToString("F2") != NewBatteryValue.ToString("F2"))
			{
				mBatteryValue = NewBatteryValue;
				result = true;
			}
			return result;
		}
		private bool TryUpdateAlarmMessage(string NewAlarmMessage)
		{
			bool result = false;
			if (mAlarmMessage != NewAlarmMessage)
			{
				mAlarmMessage = NewAlarmMessage ?? string.Empty;
				result = true;
			}
			return result;
		}
		private bool TryUpdatePath(IEnumerable<IPoint2D> NewPath)
		{
			bool result = false;
			if (mPath != null && mPath.Count() == 0 && NewPath != null && NewPath.Count() == 0) return result;
			if ((mPath == null && NewPath != null) || (mPath != null && NewPath != null && ConvertToString(mPath) != ConvertToString(NewPath)))
			{
				mPath = NewPath.ToList();
				_PathDetail = null;
				_PathRegion = null;
				result = true;
			}
			return result;
		}
		private bool TryUpdateIpPort(string NewIpPort)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(NewIpPort) && mIpPort != NewIpPort)
			{
				mIpPort = NewIpPort;
				result = true;
			}
			return result;
		}
		private bool TryUpdateCurrentMissionId(string NewMissionId)
		{
			bool result = false;
			if (mCurrentMissionId != NewMissionId)
			{
				mPreviousMissionId = mCurrentMissionId;
				mCurrentMissionId = NewMissionId ?? string.Empty;
				result = true;
			}
			return result;
		}
		private bool TryUpdateCurrentInterveneCommand(string NewInterveneCommand)
		{
			bool result = false;
			if (mCurrentInterveneCommand != NewInterveneCommand)
			{
				mPreviousInterveneCommand = mCurrentInterveneCommand;
				mCurrentInterveneCommand = NewInterveneCommand ?? string.Empty;
				result = true;
			}
			return result;
		}
		private bool TryUpdateCurrentMapName(string NewMapName)
		{
			bool result = false;
			// 不做新舊地圖名的比較，因為有時候會有檔案內容有修改但檔案名稱卻不修改的情況
			mCurrentMapName = NewMapName ?? string.Empty;
			result = true;
			return result;
		}
		private bool TryUpdateCurrentMapNameList(IEnumerable<string> NewMapNameList)
		{
			bool result = false;
			if (NewMapNameList != null)
			{
				mCurrentMapNameList = NewMapNameList.ToList();
				result = true;
			}
			return result;
		}
		private bool TryUpdateVelocityMaximum(double NewVelocityMaximum)
		{
			bool result = false;
			if (mVelocityMaximum != NewVelocityMaximum)
			{
				mVelocityMaximum = NewVelocityMaximum;
				result = true;
			}
			return result;
		}
		private bool TryUpdateSafetyFrameRadius(int NewSafetyFrameRadius)
		{
			bool result = false;
			if (mSafetyFrameRadius != NewSafetyFrameRadius)
			{
				mSafetyFrameRadius = NewSafetyFrameRadius;
				result = true;
			}
			return result;
		}
		private bool TryUpdateBufferFrameRadius(int NewBufferFrameRadius)
		{
			bool result = false;
			if (mBufferFrameRadius != NewBufferFrameRadius)
			{
				mBufferFrameRadius = NewBufferFrameRadius;
				result = true;
			}
			return result;
		}
	}
}
