using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator.New
{
	public class SimulatorInfo : ISimulatorInfo
	{
		public event EventHandler<StatusUpdatedEventArgs> StatusUpdated;

		public string mName { get; private set; } = string.Empty;
		public ESimulatorStatus mStatus { get; private set; } = ESimulatorStatus.Idle;
		public int mX { get; private set; } = default(int);
		public int mY { get; private set; } = default(int);
		public int mToward { get; private set; } = default(int);
		public string mTarget { get; private set; } = string.Empty;
		public List<Point> mPath { get; private set; } = new List<Point>();
		public double mScore { get; private set; } = default(double);
		public double mBattery { get; private set; } = default(double);

		public SimulatorInfo(string Name)
		{
			SetName(Name);
		}
		public void SetName(string Name)
		{
			if (mName != Name)
			{
				mName = Name;
				RaiseEvent_StatusUpdated("Name");
			}
		}
		public void SetStatus(ESimulatorStatus Status)
		{
			if (mStatus != Status)
			{
				mStatus = Status;
				RaiseEvent_StatusUpdated("Status");
			}
		}
		public void SetLocation(int X, int Y, int Toward)
		{
			if (mX != X)
			{
				mX = X;
				RaiseEvent_StatusUpdated("X");
			}
			if (mY != Y)
			{
				mY = Y;
				RaiseEvent_StatusUpdated("Y");
			}
			if (mToward != Toward)
			{
				mToward = Toward;
				RaiseEvent_StatusUpdated("Toward");
			}
		}
		public void SetTarget(string Target)
		{
			if (mTarget != Target)
			{
				mTarget = Target;
				RaiseEvent_StatusUpdated("Target");
			}
		}
		public void SetPath(List<Point> Points)
		{
			if (ConvertToString(mPath) != ConvertToString(Points))
			{
				mPath.Clear();
				mPath.AddRange(Points);
				RaiseEvent_StatusUpdated("Path");
			}
		}
		public void SetScore(double Score)
		{
			if (!IsApproximatelyEqual(mScore, Score, 0.1))
			{
				mScore = Score;
				RaiseEvent_StatusUpdated("Score");
			}
		}
		public void SetBattery(double Battery)
		{
			if (!IsApproximatelyEqual(mBattery, Battery, 0.1))
			{
				mBattery = Battery;
				RaiseEvent_StatusUpdated("Battery");
			}
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

		private static bool IsApproximatelyEqual(double Value1, double Value2, double tolerence)
		{
			if (Math.Abs(Value1 - Value2) < tolerence)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		private static string ConvertToString(List<Point> Points)
		{
			string result = string.Empty;
			if (Points != null && Points.Count > 0)
			{
				for (int i = 0; i < Points.Count; ++i)
				{
					result += $"({Points[i].mX},{Points[i].mY})";
				}
			}
			return result;
		}
	}
}
