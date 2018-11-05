using Geometry;
using SerialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	class VehicleSimulator
	{
		public delegate void PositionChangedEventHandler(string name, TowardPair position);
		public event PositionChangedEventHandler PositionChanged;

		public delegate void PathChangedEventHandler(string name, List<Pair> path);
		public event PathChangedEventHandler PathChanged;

		public delegate void StatusChangedEventHandler(string name, string status);
		public event StatusChangedEventHandler StatusChanged;

		public string Name { get; private set; } = "";
		private int X = 0;
		private int Y = 0;
		private double _Toward = 0;
		private double Toward
		{
			get
			{
				return _Toward;
			}
			set
			{
				_Toward = value;
				if (_Toward < 0) Toward += 360;
				if (_Toward >= 360) Toward -= 360;
			}
		}
		public TowardPair Position { get { return new TowardPair(X, Y, Toward); } }
		public double TranslationSpeed { get; private set; } = 0; // mm/s
		public double RotationSpeed { get; private set; } = 0; // degree/s
		private string _Status = "Stopped";
		public string Status
		{
			get
			{
				return _Status;
			}
			private set
			{
				if (_Status != value)
				{
					_Status = value;
					StatusChanged?.Invoke(Name, _Status);
				}
			}
		}
		private List<Pair> Path { get; set; } = null;
		private Thread MainThread;

		public VehicleSimulator(string name, double translationSpeed, double rotationSpeed)
		{
			Name = name;
			SetSpeed(translationSpeed, rotationSpeed);

			MainThread = new Thread(MainTask);
			MainThread.IsBackground = true;
			MainThread.Name = "Main";
			MainThread.Start();
		}

		public void SetSpeed(double translationSpeed, double rotationSpeed)
		{
			TranslationSpeed = translationSpeed;
			RotationSpeed = rotationSpeed;
		}

		public AGVStatus GetAGVStatus()
		{
			AGVStatus result = new AGVStatus();
			result.AlarmMessage = "";
			result.Battery = 75.5f;
			if (Status == "Moving") result.Description = EDescription.Running;
			else if (Status == "Paused") result.Description = EDescription.Pause;
			else if (Status == "Stopped") result.Description = EDescription.Arrived;
			result.GoalName = "";
			result.MapMatch = 99.9f;
			result.Name = Name;
			result.Toward = Toward;
			result.Velocity = TranslationSpeed;
			result.X = X;
			result.Y = Y;
			return result;
		}

		public AGVPath GetAGVPath()
		{
			AGVPath result = null;
			if (Path != null && Path.Count() > 0)
			{
				result.Name = Name;
				result.PathX = new List<double>();
				result.PathY = new List<double>();
				result.PathX.Add(X);
				result.PathY.Add(Y);
				for (int i = 0; i < Path.Count(); ++i)
				{
					result.PathX.Add(Path[i].X);
					result.PathY.Add(Path[i].Y);
				}
			}
			return result;
		}

		public void Move(List<Pair> path)
		{
			if (Status == "Stopped")
			{
				Path = path;
				Status = "Moving";
			}
		}

		private void MoveTo(Pair point)
		{
			if (Status == "Stopped")
			{
			}
		}

		private void MoveTo(TowardPair point)
		{
			if (Status == "Stopped")
			{
			}
		}

		public void PauseMoving()
		{
			if (Status == "Moving")
				Status = "Paused";
		}

		public void ResumeMoving()
		{
			if (Status == "Paused")
				Status = "Moving";
		}

		public void StopMoving()
		{
			if (Status == "Moving" || Status == "Paused")
			{
				Status = "Stopped";
				Path.Clear();
				Path = null;
			}
		}

		private void MainTask()
		{
			int timeInterval = 200;
			while (true)
			{
				if (Status == "Moving")
				{
					if (Path == null || Path.Count() == 0)
						Status = "Stopped";
					else
						MoveToNextPosition((double)timeInterval / 1000);
				}
				else if (Status == "Paused")
				{

				}
				else if (Status == "Stopped")
				{

				}

				Thread.Sleep(timeInterval);
			}
		}

		/// <summary>計算根據 Path 移動時， time 秒後的位置</summary>
		private void MoveToNextPosition(double time)
		{
			if (Path != null && Path.Count() > 0)
			{
				Pair targetPoint = Path[0];
				double targetToward = CalculateVectorAngleInDegree(new Pair(X, Y), Path[0]);
				//Console.WriteLine($"Target Point: ({targetPoint.X}, {targetPoint.Y}). Target Toward: {targetToward}");

				// 進行旋轉
				if (RotationSpeed > 0 && !IsDoubleApproximatelyEqual(Toward, targetToward))
				{
					double newToward = RotateToTarget(Toward, targetToward, RotationSpeed, time);
					ChangePosition(null, null, newToward);
				}
				// 進行平移
				else if (TranslationSpeed > 0 && IsDoubleApproximatelyEqual(Toward, targetToward) && !IsPairEqual(new Pair(X, Y), targetPoint))
				{
					Pair newPosition = TranslateToTarget(new Pair(X, Y), targetPoint, TranslationSpeed, time);
					ChangePosition(newPosition.X, newPosition.Y, null);

					// 判斷是否到達該點
					if (IsPairEqual(new Pair(X, Y), targetPoint))
					{
						Path.RemoveAt(0);
						PathChanged?.Invoke(Name, Path);
					}
				}

			}
		}

		private void ChangePosition(int? x, int? y, double? toward)
		{
			bool changed = false;
			if (x != null && X != x)
			{
				X = (int)x;
				changed = true;
			}
			if (y != null && Y != y)
			{
				Y = (int)y;
				changed = true;
			}
			if (toward != null && Toward != toward)
			{
				Toward = (double)toward;
				changed = true;
			}
			if (changed)
			{
				//Console.WriteLine($"X:{X}, Y:{Y}, Toward:{Toward.ToString("F2")}");
				PositionChanged?.Invoke(Name, new TowardPair(X, Y, Toward));
			}
		}

		private static double CalculateVectorAngleInDegree(Pair src, Pair dst)
		{
			double result = 0;
			if (src != null && dst != null)
			{
				int diffX = dst.X - src.X;
				int diffY = dst.Y - src.Y;

				result = Math.Atan2(diffY, diffX) / Math.PI * 180;
				if (result < 0) result += 360;
			}
			return result;
		}

		private static double CalculateDistance(Pair point1, Pair point2)
		{
			return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
		}

		private static bool IsDoubleApproximatelyEqual(double num1, double num2)
		{
			return ((num1 + 0.1 > num2) && (num1 - 0.1 < num2));
		}

		private static bool IsPairEqual(Pair pair1, Pair pair2)
		{
			return (pair1.X == pair2.X && pair1.Y == pair2.Y);
		}

		/// <summary>計算從當前面向旋轉至目標面向，在指定速度下、指定秒數後的面相角度</summary>
		private static double RotateToTarget(double currentToward, double targetToward, double rotationSpeed, double time)
		{
			double result = 0;
			double diffAngle = targetToward - currentToward;
			double rotationAngle = rotationSpeed * time;
			// 逆時針旋轉
			if ((diffAngle > 0 && diffAngle < 180) || (diffAngle < -180 && diffAngle > -360))
			{
				if (Math.Abs(diffAngle) < rotationAngle)
					result = targetToward;
				else
					result = currentToward + rotationAngle;
			}
			// 順時針旋轉
			else
			{
				if (Math.Abs(diffAngle) < rotationAngle)
					result = targetToward;
				else
					result = currentToward - rotationAngle;
			}
			return result;
		}

		/// <summary>計算從當前點移動至目標點，在指定速度下、指定秒數後的點位置</summary>
		private static Pair TranslateToTarget(Pair currentPoint, Pair targetPoint, double translationSpeed, double time)
		{
			Pair result = null;
			double diffX = targetPoint.X - currentPoint.X;
			double diffY = targetPoint.Y - currentPoint.Y;
			double toward = CalculateVectorAngleInDegree(currentPoint, targetPoint);
			double translateX = Math.Abs(Math.Cos(toward * Math.PI / 180)) * translationSpeed * time;
			double translateY = Math.Abs(Math.Sin(toward * Math.PI / 180)) * translationSpeed * time;

			// 向右向上移動
			if (diffX >= 0 && diffY >= 0)
			{
				if (currentPoint.X + translateX > targetPoint.X || currentPoint.Y + translateY > targetPoint.Y)
					result = new Pair(targetPoint.X, targetPoint.Y);
				else
					result = new Pair((int)(currentPoint.X + translateX), (int)(currentPoint.Y + translateY));
			}
			// 向左向上移動
			else if (diffX < 0 && diffY >= 0)
			{
				if (currentPoint.X - translateX < targetPoint.X || currentPoint.Y + translateY > targetPoint.Y)
					result = new Pair(targetPoint.X, targetPoint.Y);
				else
					result = new Pair((int)(currentPoint.X - translateX), (int)(currentPoint.Y + translateY));
			}
			// 向左向下移動
			else if (diffX < 0 && diffY < 0)
			{
				if (currentPoint.X - translateX < targetPoint.X || currentPoint.Y - translateY < targetPoint.Y)
					result = new Pair(targetPoint.X, targetPoint.Y);
				else
					result = new Pair((int)(currentPoint.X - translateX), (int)(currentPoint.Y - translateY));
			}
			// 向右向下移動
			else if (diffX >= 0 && diffY < 0)
			{
				if (currentPoint.X + translateX > targetPoint.X || currentPoint.Y - translateY < targetPoint.Y)
					result = new Pair(targetPoint.X, targetPoint.Y);
				else
					result = new Pair((int)(currentPoint.X + translateX), (int)(currentPoint.Y - translateY));
			}
			return result;
		}
	}
}
