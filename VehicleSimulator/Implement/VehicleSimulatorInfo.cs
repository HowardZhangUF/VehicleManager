using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using VehicleSimulator.Interface;
using static TrafficControlTest.Library.Library;
using static VehicleSimulator.Library.EventHandlerLibraryOfIVehicleSimulator;

namespace VehicleSimulator.Implement
{
	public class VehicleSimulatorInfo : IVehicleSimulatorInfo
	{
		public event EventHandlerIVehicleSimulator StateUpdated;

		public string mName
		{
			get
			{
				return _Name;
			}
			private set
			{
				if (!string.IsNullOrEmpty(value))
				{
					_Name = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public string mState
		{
			get
			{
				return _State;
			}
			set
			{
				if (!string.IsNullOrEmpty(value) && _State != value)
				{
					_State = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public IPoint2D mPosition
		{
			get
			{
				return _Position;
			}
			set
			{
				if (value != null)
				{
					_Position = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public double mToward
		{
			get
			{
				return _Toward;
			}
			set
			{
				if (_Toward != value)
				{
					_Toward = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public string mTarget
		{
			get
			{
				return _Target;
			}
			set
			{
				if (_Target != value)
				{
					_Target = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public IPoint2D mBufferTarget
		{
			get
			{
				return _BufferTarget;
			}
			set
			{
				if (_BufferTarget != value)
				{
					_BufferTarget = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public double mTranslationVelocity
		{
			get
			{
				return _TranslationVelocity;
			}
			set
			{
				if (_TranslationVelocity != value)
				{
					_TranslationVelocity = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public double mRotationVeloctiy
		{
			get
			{
				return _RotationVeloctiy;
			}
			set
			{
				if (_RotationVeloctiy != value)
				{
					_RotationVeloctiy = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public double mMapMatch
		{
			get
			{
				return _MapMatch;
			}
			set
			{
				if (_MapMatch != value)
				{
					_MapMatch = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public double mBattery
		{
			get
			{
				return _Battery;
			}
			set
			{
				if (_Battery != value)
				{
					_Battery = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public bool mPathBlocked
		{
			get
			{
				return _PathBlocked;
			}
			set
			{
				if (_PathBlocked != value)
				{
					_PathBlocked = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public string mAlarmMessage
		{
			get
			{
				return _AlarmMessage;
			}
			set
			{
				if (_AlarmMessage != value)
				{
					_AlarmMessage = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public int mSafetyFrameRadius
		{
			get
			{
				return _SafetyFrameRadius;
			}
			set
			{
				if (_SafetyFrameRadius != value)
				{
					_SafetyFrameRadius = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public bool mIsInterveneAvailable
		{
			get
			{
				return _IsInterveneAvailable;
			}
			set
			{
				if (_IsInterveneAvailable != value)
				{
					_IsInterveneAvailable = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public bool mIsIntervening
		{
			get
			{
				return _IsIntervening;
			}
			set
			{
				if (_IsIntervening != value)
				{
					_IsIntervening = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public string mInterveneCommand
		{
			get
			{
				return _InterveneCommand;
			}
			set
			{
				if (_InterveneCommand != value)
				{
					_InterveneCommand = value;
					RaiseEvent_StateUpdated();
				}
			}
		}
		public IEnumerable<IPoint2D> mPath
		{
			get
			{
				return _Path;
			}
			private set
			{
				if (value != null)
				{
					_Path = value;
				}
			}
		}

		private string _Name = string.Empty;
		private string _State = string.Empty;
		private IPoint2D _Position = null;
		private double _Toward = 0.0f;
		private string _Target = string.Empty;
		private IPoint2D _BufferTarget = null;
		private double _TranslationVelocity = 700.0f;
		private double _RotationVeloctiy = 30.0f;
		private double _MapMatch = 0.0f;
		private double _Battery = 73.1f;
		private bool _PathBlocked = false;
		private string _AlarmMessage = string.Empty;
		private int _SafetyFrameRadius = 500;
		private bool _IsInterveneAvailable = true;
		private bool _IsIntervening = false;
		private string _InterveneCommand = string.Empty;
		private IEnumerable<IPoint2D> _Path = null;
		private Thread mThdMoveAlongPath = null;
		private bool[] mThdMoveAlongPathExitFlag = null;

		public VehicleSimulatorInfo(string Name)
		{
			Set(Name);
			_Position = GenerateIPoint2D(0, 0);
			_Path = new List<IPoint2D>();
		}
		public void Set(string Name)
		{
			mName = Name;
		}
		public void StartMove(IEnumerable<IPoint2D> Path)
		{
			mPath = Path;
			mState = "Running";
			if (mThdMoveAlongPath == null || !mThdMoveAlongPath.IsAlive) InitializeThread();
		}
		public void StopMove()
		{
			if (mThdMoveAlongPath != null) DestroyThread();
			List<IPoint2D> tmp = _Path.ToList();
			tmp.Clear();
			_Path = tmp;
			mState = "Idle";
		}
		public void PauseMove()
		{
			if (mState == "Running")
			{
				mState = "Pausing";
			}
		}
		public void ResumeMove()
		{
			if (mState == "Pausing")
			{
				mState = "Running";
			}
		}

		private void InitializeThread()
		{
			mThdMoveAlongPathExitFlag = new bool[] { false };
			mThdMoveAlongPath = new Thread(() => Task_MoveAlongPath(mThdMoveAlongPathExitFlag));
			mThdMoveAlongPath.IsBackground = true;
			mThdMoveAlongPath.Start();
		}
		private void DestroyThread()
		{
			if (mThdMoveAlongPath != null)
			{
				if (mThdMoveAlongPath.IsAlive)
				{
					mThdMoveAlongPathExitFlag[0] = true;
				}
				mThdMoveAlongPath = null;
			}
		}
		protected virtual void RaiseEvent_StateUpdated(bool Sync = true)
		{
			if (Sync)
			{
				StateUpdated?.Invoke(DateTime.Now, mName, this);
			}
			else
			{
				Task.Run(() => StateUpdated?.Invoke(DateTime.Now, mName, this));
			}
		}
		private void Task_MoveAlongPath(bool[] ExitFlag)
		{
			int interval = 200;
			while (!ExitFlag[0] && (mState == "Running" || mState == "Pausing"))
			{
				Subtask_Move((double)interval / 1000);

				if (_Path == null || _Path.Count() == 0)
				{
					mState = "Idle";
					break;
				}

				Thread.Sleep(interval);
			}
		}
		private void Subtask_Move(double Time)
		{
			if (_Path != null && _Path.Count() > 0 && mState == "Running")
			{
				IPoint2D targetPoint = mBufferTarget != null ? mBufferTarget : _Path.First();
				double targetToward = CalculateVectorAngleInDegree(_Position.mX, _Position.mY, targetPoint.mX, targetPoint.mY);

				GetNextMove(_Position.mX, _Position.mY, mToward, targetPoint.mX, targetPoint.mY, targetToward, _TranslationVelocity, _RotationVeloctiy, Time, out int NextX, out int NextY, out double NextToward);

				mPosition = GenerateIPoint2D(NextX, NextY);
				mToward = NextToward;

				if (IsApproximatelyEqual(_Position.mX, _Position.mY, _Toward, targetPoint.mX, targetPoint.mY, targetToward))
				{
					if (mBufferTarget != null)
					{
						mBufferTarget = null;
					}
					else
					{
						List<IPoint2D> tmp = _Path.ToList();
						tmp.RemoveAt(0);
						_Path = tmp;
					}
				}
			}
		}
		private static void GetNextMove(int CurrentX, int CurrentY, double CurrentToward, int TargetX, int TargetY, double TargetToward, double TranslationVelocity, double RotationVelocity, double Time, out int NextX, out int NextY, out double NextToward)
		{
			NextX = CurrentX;
			NextY = CurrentY;
			NextToward = CurrentToward;
			if (!IsApproximatelyEqual(CurrentToward, TargetToward))
			{
				RotateToTarget(CurrentToward, TargetToward, RotationVelocity, Time, out NextToward);
			}
			else if (CurrentX != TargetX || CurrentY != TargetY)
			{
				TranslateToTarget(CurrentX, CurrentY, TargetX, TargetY, TranslationVelocity, Time, out NextX, out NextY);
			}
		}
		/// <summary>計算從當前點移動至目標點，在指定速度下、指定秒數後的點位置</summary>
		private static void TranslateToTarget(int CurrentX, int CurrentY, int TargetX, int TargetY, double TranslationVelocity, double Time, out int X, out int Y)
		{
			X = 0;
			Y = 0;
			double diffX = TargetX - CurrentX;
			double diffY = TargetY - CurrentY;
			double toward = CalculateVectorAngleInDegree(CurrentX, CurrentY, TargetX, TargetY);
			double translateX = Math.Abs(Math.Cos(toward * Math.PI / 180)) * TranslationVelocity * Time;
			double translateY = Math.Abs(Math.Sin(toward * Math.PI / 180)) * TranslationVelocity * Time;

			// 向右向上移動
			if (diffX >= 0 && diffY >= 0)
			{
				// 若當前座標與目標座標的距離小於 TranslationVelocity * Time 的話，則將下一個座標設定成目標座標，其餘，下一個座標等於當前座標 + TranslateX / + TranslateY
				if (((TargetX - CurrentX) * (TargetX - CurrentX) + (TargetY - CurrentY) * (TargetY - CurrentY)) < (TranslationVelocity * Time) * (TranslationVelocity * Time))
				{
					X = TargetX;
					Y = TargetY;
				}
				else
				{
					X = (int)(CurrentX + translateX);
					Y = (int)(CurrentY + translateY);
				}
			}
			// 向左向上移動
			else if (diffX < 0 && diffY >= 0)
			{
				if (((TargetX - CurrentX) * (TargetX - CurrentX) + (TargetY - CurrentY) * (TargetY - CurrentY)) < (TranslationVelocity * Time) * (TranslationVelocity * Time))
				{
					X = TargetX;
					Y = TargetY;
				}
				else
				{
					X = (int)(CurrentX - translateX);
					Y = (int)(CurrentY + translateY);
				}
			}
			// 向左向下移動
			else if (diffX < 0 && diffY < 0)
			{
				if (((TargetX - CurrentX) * (TargetX - CurrentX) + (TargetY - CurrentY) * (TargetY - CurrentY)) < (TranslationVelocity * Time) * (TranslationVelocity * Time))
				{
					X = TargetX;
					Y = TargetY;
				}
				else
				{
					X = (int)(CurrentX - translateX);
					Y = (int)(CurrentY - translateY);
				}
			}
			// 向右向下移動
			else if (diffX >= 0 && diffY < 0)
			{
				if (((TargetX - CurrentX) * (TargetX - CurrentX) + (TargetY - CurrentY) * (TargetY - CurrentY)) < (TranslationVelocity * Time) * (TranslationVelocity * Time))
				{
					X = TargetX;
					Y = TargetY;
				}
				else
				{
					X = (int)(CurrentX + translateX);
					Y = (int)(CurrentY - translateY);
				}
			}
		}
		/// <summary>計算從當前面向旋轉至目標面向，在指定速度下、指定秒數後的面相角度</summary>
		private static void RotateToTarget(double CurrentToward, double TargetToward, double RotationVelocity, double Time, out double NextToward)
		{
			double diffAngle = (TargetToward - CurrentToward) % 360;
			double rotationAngle = RotationVelocity * Time;
			// 逆時針旋轉
			if ((diffAngle > 0 && diffAngle < 180) || (diffAngle < -180 && diffAngle > -360))
			{
				if (Math.Abs(diffAngle) < rotationAngle)
					NextToward = TargetToward;
				else
					NextToward = CurrentToward + rotationAngle;
			}
			// 順時針旋轉
			else
			{
				if (Math.Abs(diffAngle) < rotationAngle)
					NextToward = TargetToward;
				else
					NextToward = CurrentToward - rotationAngle;
			}
		}
		private static double CalculateVectorAngleInDegree(int SrcX, int SrcY, int DstX, int DstY)
		{
			double result = 0;
			int diffX = DstX - SrcX;
			int diffY = DstY - SrcY;

			result = Math.Atan2(diffY, diffX) / Math.PI * 180;
			if (result < 0) result += 360;
			return result;
		}
		private static bool IsApproximatelyEqual(double Num1, double Num2)
		{
			return ((Num1 + 0.1 > Num2) && (Num1 - 0.1 < Num2));
		}
		private static bool IsApproximatelyEqual(int X1, int Y1, double Toward1, int X2, int Y2, double Toward2)
		{
			return X1 == X2 && Y1 == Y2 && IsApproximatelyEqual(Toward1, Toward2);
		}
	}
}
