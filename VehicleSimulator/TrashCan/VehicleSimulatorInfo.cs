using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;
using VehicleSimulator.Interface;
using static TrafficControlTest.Library.Library;
using static VehicleSimulator.Library.EventHandlerLibraryOfIVehicleSimulator;

namespace VehicleSimulator.Implement
{
	public class VehicleSimulatorInfo : IVehicleSimulatorInfo
	{
		public event EventHandlerIVehicleSimulator StateUpdated;

		public const double MAX_TRANSLATION_VELOCITY = 700.0f;
		public const double MAX_ROTATION_VELOCITY = 30.0f;

		public string mName { get; private set; } = string.Empty;
		public string mState { get; private set; } = string.Empty; // Running, Pause, Idling
		public IPoint2D mPosition { get; private set; } = null;
		public double mToward { get; private set; } = 0.0f;
		public string mTarget { get; private set; } = string.Empty;
		public IPoint2D mBufferTarget { get; private set; } = null;
		public double mTranslationVelocity { get; private set; } = 0.0f;
		public double mRotationVelocity { get; private set; } = 0.0f;
		public double mMapMatch { get; private set; } = 0.0f;
		public double mBattery { get; private set; } = 0.0f;
		public bool mPathBlocked { get; private set; } = false;
		public string mAlarmMessage { get; private set; } = string.Empty;
		public int mSafetyFrameRadius { get; private set; } = 0;
		public bool mIsInterveneAvailable { get; private set; } = false;
		public bool mIsBeingIntervened { get; private set; } = false;
		public string mInterveneCommand { get; private set; } = string.Empty;
		public IEnumerable<IPoint2D> mPath { get; private set; } = null;

		private Thread mThdMoveAlongPath = null;
		private bool[] mThdMoveAlongPathExitFlag = null;

		public VehicleSimulatorInfo(string Name)
		{
			mName = Name;
			mState = "Idling";
			mPosition = new Point2D(0, 0);
			mBattery = 73.7f;
			mSafetyFrameRadius = 500;
			mIsInterveneAvailable = true;
			mPath = new List<IPoint2D>();
			RaiseEvent_StateUpdated();

			InitializeThread();
		}
		~VehicleSimulatorInfo()
		{
			DestroyThread();
		}
		public void StartMove(IEnumerable<IPoint2D> Path)
		{
			StartMove(Path, Path.Last().ToString());
		}
		public void StartMove(IEnumerable<IPoint2D> Path, string Target)
		{
			mPath = Path;
			mTarget = Target;
			mTranslationVelocity = MAX_TRANSLATION_VELOCITY;
			mRotationVelocity = MAX_ROTATION_VELOCITY;
			if (mInterveneCommand != "PauseMoving") mState = "Running";
			RaiseEvent_StateUpdated();
		}
		public void StopMove()
		{
			if (mIsBeingIntervened) ClearInterveneCommand();
			List<IPoint2D> tmp = mPath.ToList();
			tmp.Clear();
			mPath = tmp;
			mTranslationVelocity = 0.0f;
			mRotationVelocity = 0.0f;
			mState = "Idling";
			RaiseEvent_StateUpdated();
		}
		public void PauseMove()
		{
			if (mState == "Running")
			{
				mTranslationVelocity = 0.0f;
				mRotationVelocity = 0.0f;
				mState = "Pause";
				RaiseEvent_StateUpdated();
			}
		}
		public void ResumeMove()
		{
			if (mState == "Pause")
			{
				mTranslationVelocity = MAX_TRANSLATION_VELOCITY;
				mRotationVelocity = MAX_ROTATION_VELOCITY;
				mState = "Running";
				RaiseEvent_StateUpdated();
			}
		}
		public void Dock()
		{
			if (mState == "Idling")
			{
				mState = "Charge";
				RaiseEvent_StateUpdated();
			}
		}
		public void Undock()
		{
			if (mState == "Charge")
			{
				mState = "Idling";
				RaiseEvent_StateUpdated();
			}
		}
		public void SetInterveneCommand(string Command, params string[] Paras)
		{
			if (mIsInterveneAvailable)
			{
				if (Command == "InsertMovingBuffer" && Paras != null && Paras.Length == 2)
				{
					SetInterveneCommand_Insert(int.Parse(Paras[0]), int.Parse(Paras[1]));
				}
				else if (Command == "RemoveMovingBuffer" && (Paras == null || Paras.Length == 0))
				{
					SetInterveneCommand_CancelInsert();
				}
				else if (Command == "PauseMoving" && (Paras == null || Paras.Length == 0))
				{
					SetInterveneCommand_Pause();
				}
				else if (Command == "ResumeMoving" && (Paras == null || Paras.Length == 0))
				{
					SetInterveneCommand_Resume();
				}
			}
		}
		public override string ToString()
		{
			string result = string.Empty;
			result = $"{mName} ({mPosition.mX}, {mPosition.mY}, {mToward.ToString("F2")}) {mState}";
			return result;
		}
		public string[] ToStringArray()
		{
			string[] result = null;
			result = new string[]
			{
				mName,
				mState,
				mPosition.ToString(),
				mToward.ToString("F2"),
				mTarget,
				mBufferTarget?.ToString(),
				mTranslationVelocity.ToString("F2"),
				mRotationVelocity.ToString("F2"),
				mMapMatch.ToString("F2"),
				mBattery.ToString("F2"),
				mPathBlocked.ToString(),
				mAlarmMessage,
				mSafetyFrameRadius.ToString(),
				mIsInterveneAvailable.ToString(),
				mIsBeingIntervened.ToString(),
				mInterveneCommand,
				ConvertToString(mPath)
			};
			return result;
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
		private void SetInterveneCommand_Insert(int x, int y)
		{
			if (mIsBeingIntervened) ClearInterveneCommand();
			mTranslationVelocity = MAX_TRANSLATION_VELOCITY;
			mRotationVelocity = MAX_ROTATION_VELOCITY;
			mBufferTarget = new Point2D(x, y);
			mIsBeingIntervened = true;
			mInterveneCommand = $"InsertMovingBuffer:({mBufferTarget.mX},{mBufferTarget.mY})";
			mState = "Running";
			RaiseEvent_StateUpdated();
		}
		private void SetInterveneCommand_CancelInsert()
		{
			if (mIsBeingIntervened) ClearInterveneCommand();
		}
		private void SetInterveneCommand_Pause()
		{
			if (mIsBeingIntervened) ClearInterveneCommand();
			PauseMove();
			mIsBeingIntervened = true;
			mInterveneCommand = "PauseMoving";
			RaiseEvent_StateUpdated();
		}
		private void SetInterveneCommand_Resume()
		{
			if (mIsBeingIntervened) ClearInterveneCommand();
		}
		private void ClearInterveneCommand()
		{
			if (!string.IsNullOrEmpty(mInterveneCommand))
			{
				if (mInterveneCommand.StartsWith("InsertMovingBuffer"))
				{
					mBufferTarget = null;
				}
				else if (mInterveneCommand.StartsWith("PauseMoving"))
				{
					ResumeMove();
				}
			}
			mIsBeingIntervened = false;
			mInterveneCommand = string.Empty;
			RaiseEvent_StateUpdated();
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
			while (!ExitFlag[0])
			{
				if (mState == "Running")
				{
					if (mBufferTarget == null && (mPath == null || mPath.Count() == 0))
					{
						mState = "Idling";
						mTarget = string.Empty;
						RaiseEvent_StateUpdated();
					}
					else
					{
						Subtask_Move((double)interval / 1000);
					}
				}

				Thread.Sleep(interval);
			}
		}
		private void Subtask_Move(double Time)
		{
			if (mBufferTarget != null || (mPath != null && mPath.Count() > 0))
			{
				IPoint2D targetPoint = mBufferTarget != null ? mBufferTarget : mPath.First();
				double targetToward = CalculateVectorAngleInDegree(mPosition.mX, mPosition.mY, targetPoint.mX, targetPoint.mY);

				GetNextMove(mPosition.mX, mPosition.mY, mToward, targetPoint.mX, targetPoint.mY, targetToward, mTranslationVelocity, mRotationVelocity, Time, out int NextX, out int NextY, out double NextToward);

				mPosition = new Point2D(NextX, NextY);
				mToward = NextToward;

				if (IsApproximatelyEqual(mPosition.mX, mPosition.mY, mToward, targetPoint.mX, targetPoint.mY, targetToward))
				{
					if (mBufferTarget != null)
					{
						mBufferTarget = null;
						if (mIsBeingIntervened) SetInterveneCommand_CancelInsert();
					}
					else
					{
						List<IPoint2D> tmp = mPath.ToList();
						tmp.RemoveAt(0);
						mPath = tmp;
					}
				}
				RaiseEvent_StateUpdated();
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
		private static string ConvertToString(IEnumerable<IPoint2D> Points)
		{
			string result = string.Empty;
			if (Points != null && Points.Count() > 0)
			{
				for (int i = 0; i < Points.Count(); ++i)
				{
					result += Points.ElementAt(i).ToString();
				}
			}
			return result;
		}
	}
}
