using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace VehicleSimulator.New
{
	public class SimulatorControl : SystemWithLoopTask, ISimulatorControl
	{
		private ISimulatorInfo rSimulatorInfo = null;
		private string mTarget = string.Empty;
		private List<MoveRequest> mMoveRequests = new List<MoveRequest>();
		private bool mPauseMoveFlag = false;
		private int mTranslationVelocity = 800; // mm/sec
		private int mRotationVelocity = 30; // degree/sec

		public SimulatorControl(ISimulatorInfo ISimulatorInfo)
		{
			Set(ISimulatorInfo);
		}
		public void Set(ISimulatorInfo ISimulatorInfo)
		{
			rSimulatorInfo = ISimulatorInfo;
		}
		public void StartMove(int X, int Y)
		{
			string target = $"({X},{Y})";
			StartMove(target, X, Y);
		}
		public void StartMove(int X, int Y, int Toward)
		{
			string target = $"({X},{Y},{Toward})";
			StartMove(target, X, Y, Toward);
		}
		public void StartMove(string Target, int X, int Y)
		{
			List<MoveRequest> moveRequests = new List<MoveRequest>() { new MoveRequest(X, Y) };
			StartMove(Target, moveRequests);
		}
		public void StartMove(string Target, int X, int Y, int Toward)
		{
			List<MoveRequest> moveRequests = new List<MoveRequest>() { new MoveRequest(X, Y, Toward) };
			StartMove(Target, moveRequests);
		}
		public void StartMove(string Target, List<MoveRequest> MoveRequests)
		{
			if (mIsExecuting) return;

			mTarget = Target;
			mMoveRequests.Clear();
			mMoveRequests.AddRange(MoveRequests);
			mPauseMoveFlag = false;

			rSimulatorInfo.SetStatus(ESimulatorStatus.Working);
			rSimulatorInfo.SetTarget(mTarget);
			rSimulatorInfo.SetPath(mMoveRequests.Select(o => new Point(o.mX, o.mY)).ToList());

			Start();
		}
		public void StopMove()
		{
			Stop();

			mTarget = string.Empty;
			mMoveRequests.Clear();
			mPauseMoveFlag = false;

			rSimulatorInfo.SetStatus(ESimulatorStatus.Idle);
			rSimulatorInfo.SetTarget(string.Empty);
			rSimulatorInfo.SetPath(new List<Point>());
		}
		public void PauseMove()
		{
			rSimulatorInfo.SetStatus(ESimulatorStatus.Paused);
			mPauseMoveFlag = true;
		}
		public void ResumeMove()
		{
			rSimulatorInfo.SetStatus(ESimulatorStatus.Working);
			mPauseMoveFlag = false;
		}
		public override void Task()
		{
			Subtask_Move();
		}

		protected void Subtask_Move()
		{
			if (!string.IsNullOrEmpty(mTarget) && mMoveRequests.Count > 0)
			{
				if (!mPauseMoveFlag)
				{
					MoveRequest currentMoveRequest = mMoveRequests.First();
					if (IsCompleteRequest(rSimulatorInfo.mX, rSimulatorInfo.mY, rSimulatorInfo.mToward, currentMoveRequest))
					{
						mMoveRequests.RemoveAt(0);
						rSimulatorInfo.SetPath(mMoveRequests.Select(o => new Point(o.mX, o.mY)).ToList());
					}
					else
					{
						GetNextMove(rSimulatorInfo.mX, rSimulatorInfo.mY, rSimulatorInfo.mToward, currentMoveRequest.mX, currentMoveRequest.mY, AdjustTowardValue(currentMoveRequest.mToward), mTranslationVelocity, mRotationVelocity, (double)mTimePeriod / 1000.0f, currentMoveRequest.mIsMoveBackward, currentMoveRequest.mIsRequestToward, out int NextX, out int NextY, out int NextToward);
						rSimulatorInfo.SetLocation(NextX, NextY, NextToward);
					}
				}
			}
			else
			{
				Stop();
				rSimulatorInfo.SetStatus(ESimulatorStatus.Idle);
			}
		}

		private static bool IsCompleteRequest(int CurrentX, int CurrentY, int CurrentToward, MoveRequest MoveRequest)
		{
			if (!MoveRequest.mIsRequestToward)
			{
				return IsCoordinateEqual(CurrentX, CurrentY, MoveRequest.mX, MoveRequest.mY);
			}
			else
			{
				return IsCoordinateEqual(CurrentX, CurrentY, MoveRequest.mX, MoveRequest.mY) && IsTowardEqual(CurrentToward, MoveRequest.mToward);
			}
		}
		private static bool IsCoordinateEqual(int X1, int Y1, int X2, int Y2)
		{
			return X1 == X2 && Y1 == Y2;
		}
		private static bool IsTowardEqual(int Toward1, int Toward2)
		{
			int tmpToward1 = AdjustTowardValue(Toward1);
			int tmpToward2 = AdjustTowardValue(Toward2);
			return tmpToward1 == tmpToward2;
		}
		private static void GetNextMove(int CurrentX, int CurrentY, int CurrentToward, int TargetX, int TargetY, int TargetToward, int TranslationVelocity, int RotationVelocity, double TimeInSec, bool IsMoveBackward, bool IsRequestTargetToward, out int NextX, out int NextY, out int NextToward)
		{
			NextX = CurrentX;
			NextY = CurrentY;
			NextToward = CurrentToward;

			int towardWhenTranslating = 0;
			if (!IsMoveBackward)
			{
				towardWhenTranslating = CalculateVectorAngleInDegree(CurrentX, CurrentY, TargetX, TargetY);
			}
			else
			{
				towardWhenTranslating = (CalculateVectorAngleInDegree(CurrentX, CurrentY, TargetX, TargetY) + 180) % 360;
			}

			if (!IsCoordinateEqual(CurrentX, CurrentY, TargetX, TargetY))
			{
				if (CurrentToward != towardWhenTranslating)
				{
					// 如果當前 (Current) 座標 (X, Y) 不同，且面相 (Toward) 不是面相目標 (Target) ，此時應該旋轉
					RotateToTarget(CurrentToward, towardWhenTranslating, RotationVelocity, TimeInSec, out NextToward);
				}
				else
				{
					// 如果當前 (Current) 座標 (X, Y) 不同，且面相 (Toward) 是面相目標 (Target) ，此時應該平移
					TranslateToTarget(CurrentX, CurrentY, TargetX, TargetY, TranslationVelocity, TimeInSec, out NextX, out NextY);
				}
			}
			else
			{
				if (IsRequestTargetToward)
				{
					if (CurrentToward != TargetToward)
					{
						// 如果當前座標與目標座標相同，且目標座標有要求面向，且當前面向與目標座標面向不同，此時應該旋轉
						RotateToTarget(CurrentToward, TargetToward, RotationVelocity, TimeInSec, out NextToward);
					}
				}
			}
		}
		private static void TranslateToTarget(int SrcX, int SrcY, int DstX, int DstY, int TranslationVelocity, double TimeInSec, out int NextX, out int NextY)
		{
			int remainingDistance = CalculateDistance(SrcX, SrcY, DstX, DstY); // 剩餘距離
			int translationDistance = (int)(TranslationVelocity * TimeInSec); // 當前速度於指定時間內能走的距離
			int towardWhenTranslating = CalculateVectorAngleInDegree(SrcX, SrcY, DstX, DstY); // 移動方向

			if (remainingDistance < translationDistance)
			{
				NextX = DstX;
				NextY = DstY;
			}
			else
			{
				int diffX = (int)(translationDistance * Math.Cos(towardWhenTranslating * Math.PI / 180));
				int diffY = (int)(translationDistance * Math.Sin(towardWhenTranslating * Math.PI / 180));
				NextX = SrcX + diffX;
				NextY = SrcY + diffY;
			}
		}
		private static void RotateToTarget(int SrcToward, int DstToward, int RotationVelocity, double TimeInSec, out int NextToward)
		{
			// Case 1:
			//		Src = 170, Dst = 250
			//		Dst - Src = 80 = 逆時針轉 80 度
			//		最佳解 = 逆時針轉 80 度
			// Case 2:
			//		Src = 250, Dst = 170
			//		Dst - Src = -80 = 順時針轉 80 度
			//		最佳解 = 順時針轉 80 度
			// Case 3:
			//		Src = 320, Dst = 10
			//		Dst - Src = -310 = 順時針轉 310 度
			//		最佳解 = 逆時針轉 50 度
			// Case 4:
			//		Src = 10, Dst = 320
			//		Dst - Src = 310 = 逆時針轉 310 度
			//		最佳解 = 順時針轉 50 度
			// 結論:
			//		如果旋轉角度 (a) 的絕對值小於等於 180 度，則旋轉角度 = a
			//		如果旋轉角度 (a) 的絕對值大於 180 度，則旋轉角度 = (360 - Math.Abs(a)) * (a / Math.Abs(a)) * (-1)
			int remainingAngle;
			if (Math.Abs(DstToward - SrcToward) <= 180)
			{
				remainingAngle = DstToward - SrcToward;
			}
			else
			{
				int a = DstToward - SrcToward;
				remainingAngle = (360 - Math.Abs(a)) * (a / Math.Abs(a)) * (-1);
			}
			int rotationAngle = (int)(RotationVelocity * TimeInSec);

			if (Math.Abs(remainingAngle) < rotationAngle)
			{
				NextToward = DstToward;
			}
			else
			{
				if (remainingAngle > 0)
				{
					NextToward = AdjustTowardValue(SrcToward + rotationAngle);
				}
				else
				{
					NextToward = AdjustTowardValue(SrcToward - rotationAngle);
				}
			}
		}
		private static int CalculateDistance(int X1, int Y1, int X2, int Y2)
		{
			return (int)Math.Sqrt((X2 - X1) * (X2 - X1) + (Y2 - Y1) * (Y2 - Y1));
		}
		private static int CalculateVectorAngleInDegree(int SrcX, int SrcY, int DstX, int DstY)
		{
			int result = 0;

			int diffX = DstX - SrcX;
			int diffY = DstY - SrcY;
			result = (int)(Math.Atan2(diffY, diffX) / Math.PI * 180);
			result = AdjustTowardValue(result);

			return result;
		}
		private static int AdjustTowardValue(int Toward)
		{
			int tmpToward = Toward % 360;
			if (tmpToward < 0) tmpToward += 360;
			return tmpToward;
		}
	}
}
