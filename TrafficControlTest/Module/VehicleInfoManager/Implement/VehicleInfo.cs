using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.EventHandlerLibrary;

namespace TrafficControlTest.Implement
{
	class VehicleInfo : IVehicleInfo
	{
		public event EventHandlerIItemUpdated Updated;

		public string mName { get; private set; }
		public string mState { get; private set; }
		public string mLastState { get; private set; }
		public TimeSpan mStateDuration { get { return DateTime.Now.Subtract(mStateStartTimestamp); } }
		public IPoint2D mPosition { get; private set; }
		public double mToward { get; private set; }
		public string mTarget { get; private set; }
		public string mLastTarget { get; private set; }
		public double mVelocity { get; private set; }
		public double mAverageVelocity { get { return mLastVelocity.Sum() / mLastVelocity.Count; } }
		public double mMapMatch { get; private set; }
		public double mBattery { get; private set; }
		public bool mIsInterveneAvailable { get; private set; }
		public bool mIsBeingIntervened { get; private set; }
		public string mInterveneCommand { get; private set; }
		public bool mPathBlocked { get; private set; }
		public TimeSpan mPathBlockedDuration { get { return DateTime.Now.Subtract(mPathBlockedStartTimestamp); } }
		public string mAlarmMessage { get; private set; }
		public int mSafetyFrameRadius { get; private set; } = 500;
		public int mBufferFrameRadius { get; private set; } = 500;
		public int mTotalFrameRadius { get { return mSafetyFrameRadius + mBufferFrameRadius; } }
		public IEnumerable<IPoint2D> mPath { get; private set; }
		public IEnumerable<IPoint2D> mPathDetail
		{
			get
			{
				if (_PathDetail == null) _PathDetail = CalculatePathDetail(mPosition, mPath, (mSafetyFrameRadius + mBufferFrameRadius) / 10);
				return _PathDetail;
			}
		}
		public IRectangle2D mPathRegion
		{
			get
			{
				if (_PathRegion == null) _PathRegion = CalculatePathRegion(mPosition, mPath, mSafetyFrameRadius + mBufferFrameRadius);
				return _PathRegion;
			}
		}
		public string mIpPort { get; private set; }
		public DateTime mLastUpdated { get; private set; }

		private IEnumerable<IPoint2D> _PathDetail = null;
		private IRectangle2D _PathRegion = null;

		private DateTime mStateStartTimestamp = DateTime.Now;
		private List<double> mLastVelocity = new List<double>();
		private int mVelocityDataCount = 10;
		private DateTime mPathBlockedStartTimestamp = DateTime.Now;

		public VehicleInfo(string Name)
		{
			Set(Name);
		}
		public void Set(string Name)
		{
			mName = Name;
			mLastUpdated = DateTime.Now;
			RaiseEvent_StateUpdated("Name");
		}
		public void Update(string State, IPoint2D Position, double Toward, double Battery, double Velocity, string Target, string AlarmMessage, bool IsInterveneAvailable, bool IsBeingIntervened, string InterveneCommand)
		{
			List<string> updatedItems = new List<string>();

			if (UpdateState(State)) updatedItems.Add("State");
			if (UpdatePosition(Position)) updatedItems.Add("Position");
			if (UpdateToward(Toward)) updatedItems.Add("Toward");
			if (UpdateBattery(Battery)) updatedItems.Add("Battery");
			if (UpdateVelocity(Velocity)) updatedItems.Add("Velocity");
			if (UpdateTarget(Target)) updatedItems.Add("Target");
			if (UpdateAlarmMessage(AlarmMessage)) updatedItems.Add("AlarmMessage");
			if (UpdateIsInterveneAvailable(IsInterveneAvailable)) updatedItems.Add("IsInterveneAvailable");
			if (UpdateIsBeingIntervened(IsBeingIntervened)) updatedItems.Add("IsBeingIntervened");
			if (UpdateInterveneCommand(InterveneCommand)) updatedItems.Add("InterveneCommand");

			if (updatedItems.Count > 0)
			{
				mLastUpdated = DateTime.Now;
				RaiseEvent_StateUpdated(string.Join(",", updatedItems));
			}
		}
		public void Update(IEnumerable<IPoint2D> Path)
		{
			if (UpdatePath(Path))
			{
				mLastUpdated = DateTime.Now;
				RaiseEvent_StateUpdated("Path");
			}
		}
		public void Update(string IpPort)
		{
			if (UpdateIpPort(IpPort))
			{
				mLastUpdated = DateTime.Now;
				RaiseEvent_StateUpdated("IpPort");
			}
		}
		public override string ToString()
		{
			string result = string.Empty;
			result += $"Name: {mName}, ";
			result += $"IpPort: {mIpPort}, ";
			result += $"State: {mState}, ";
			result += $"Position: {(mPosition != null ? mPosition.ToString() : string.Empty)}, ";
			result += $"Toward: {mToward.ToString("F2")}, ";
			result += $"Target: {mTarget}, ";
			result += $"Velocity: {mVelocity.ToString("F2")}, ";
			result += $"AverageVelocity: {(!double.IsNaN(mAverageVelocity) ? mAverageVelocity.ToString("F2") : string.Empty)}, ";
			result += $"Battery: {mBattery.ToString("F2")}, ";
			result += $"Path: {((mPath != null && mPath.Count() > 0) ? Library.Library.ConvertToString(mPath) : string.Empty)}, ";
			result += $"IsInterveneAvailable: {mIsInterveneAvailable.ToString()}, ";
			result += $"IsBeingIntervened: {mIsBeingIntervened.ToString()}, ";
			result += $"InterveneCommand: {mInterveneCommand}.";
			return result;
		}

		protected virtual void RaiseEvent_StateUpdated(string StateName, bool Sync = true)
		{
			if (Sync)
			{
				Updated?.Invoke(DateTime.Now, mName, StateName);
			}
			else
			{
				Task.Run(() => Updated?.Invoke(DateTime.Now, mName, StateName));
			}
		}
		private static IEnumerable<IPoint2D> CalculatePathDetail(IPoint2D CurrentPosition, IEnumerable<IPoint2D> Path, int Interval)
		{
			List<IPoint2D> result = null;
			if (Path != null && Path.Count() > 0)
			{
				result = new List<IPoint2D>();
				List<IPoint2D> tmpPath = Path.ToList();
				tmpPath.Insert(0, CurrentPosition);
				result.Add(tmpPath[0]);
				for (int i = 1; i < tmpPath.Count; ++i)
				{
					result.AddRange(Library.Library.ConvertLineToPoints(tmpPath[i - 1], tmpPath[i], Interval));
					result.Add(tmpPath[i]);
				}
			}
			return result;
		}
		private static IRectangle2D CalculatePathRegion(IPoint2D CurrentPosition, IEnumerable<IPoint2D> Path, int Amplify)
		{
			IRectangle2D result = null;
			if (Path != null && Path.Count() > 0)
			{
				List<IPoint2D> tmpPath = Path.ToList();
				tmpPath.Insert(0, CurrentPosition);
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
		private bool UpdateState(string NewState)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(NewState) && mState != NewState)
			{
				mLastState = mState;
				mState = NewState;
				mStateStartTimestamp = DateTime.Now;
				result = true;
			}
			return result;
		}
		private bool UpdatePosition(IPoint2D NewPosition)
		{
			bool result = false;
			if (NewPosition != null && (mPosition == null || (mPosition.mX != NewPosition.mX && mPosition.mY != NewPosition.mY)))
			{
				mPosition = NewPosition;
				_PathDetail = null;
				_PathRegion = null;
				result = true;
			}
			return result;
		}
		private bool UpdateToward(double NewToward)
		{
			bool result = false;
			if (mToward.ToString("F2") != NewToward.ToString("F2"))
			{
				mToward = NewToward;
				result = true;
			}
			return result;
		}
		private bool UpdateTarget(string NewTarget)
		{
			bool result = false;
			if (mTarget != NewTarget)
			{
				mLastTarget = mTarget;
				mTarget = NewTarget;
				result = true;
			}
			return result;
		}
		private bool UpdateBattery(double NewBattery)
		{
			bool result = false;
			if (mBattery.ToString("F2") != NewBattery.ToString("F2"))
			{
				mBattery = NewBattery;
				result = true;
			}
			return result;
		}
		private bool UpdateVelocity(double NewVelocity)
		{
			bool result = false;
			mLastVelocity.Add(NewVelocity);
			if (mLastVelocity.Count > mVelocityDataCount) mLastVelocity.RemoveAt(0);
			if (mVelocity.ToString("F2") != NewVelocity.ToString("F2"))
			{
				mVelocity = NewVelocity;
				result = true;
			}
			return result;
		}
		private bool UpdateAlarmMessage(string NewAlarmMessage)
		{
			bool result = false;
			if (mAlarmMessage != NewAlarmMessage)
			{
				mAlarmMessage = NewAlarmMessage;
				result = true;
			}
			return result;
		}
		private bool UpdateIsInterveneAvailable(bool NewIsInterveneAvailable)
		{
			bool result = false;
			if (mIsInterveneAvailable != NewIsInterveneAvailable)
			{
				mIsInterveneAvailable = NewIsInterveneAvailable;
				result = true;
			}
			return result;
		}
		private bool UpdateIsBeingIntervened(bool NewIsBeingIntervened)
		{
			bool result = false;
			if (mIsBeingIntervened != NewIsBeingIntervened)
			{
				mIsBeingIntervened = NewIsBeingIntervened;
				result = true;
			}
			return result;
		}
		private bool UpdateInterveneCommand(string NewInterveneCommand)
		{
			bool result = false;
			if (NewInterveneCommand != null && mInterveneCommand != NewInterveneCommand)
			{
				mInterveneCommand = NewInterveneCommand;
				result = true;
			}
			return result;
		}
		private bool UpdatePath(IEnumerable<IPoint2D> NewPath)
		{
			bool result = false;
			if (mPath != null && mPath.Count() == 0 && NewPath != null && NewPath.Count() == 0) return result;
			if ((mPath == null && NewPath != null) || (mPath != null && NewPath != null && ConvertToString(mPath) != ConvertToString(NewPath)))
			{
				mPath = NewPath;
				_PathDetail = null;
				_PathRegion = null;
				result = true;
			}
			return result;
		}
		private bool UpdatePathBlocked(bool NewPathBlocked)
		{
			bool result = false;
			if (mPathBlocked != NewPathBlocked)
			{
				mPathBlocked = NewPathBlocked;
				if (mPathBlocked == true) mPathBlockedStartTimestamp = DateTime.Now;
				result = true;
			}
			return result;
		}
		private bool UpdateIpPort(string NewIpPort)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(NewIpPort) && mIpPort != NewIpPort)
			{
				mIpPort = NewIpPort;
				result = true;
			}
			return result;
		}
	}
}
