using Geometry;
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
				if (_Toward > 360) Toward -= 360;
			}
		}
		public TowardPair Position { get { return new TowardPair(X, Y, Toward); } }
		public double TranslationSpeed { get; private set; } = 0; // mm/s
		public double RotationSpeed { get; private set; } = 0; // degree/s
		private string _Status = "";
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

		public void SetPath(List<Pair> path)
		{
			Path = path;
		}
		
		public void StartMoving()
		{
			Status = "Moving";
		}

		public void PauseMoving()
		{
			Status = "Paused";
		}

		public void StopMoving()
		{
			Status = "Stopped";
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
				double targetToward = CalculateHorizontalAngle(new Pair(X, Y), Path[0]);
				Console.WriteLine($"Target Point: ({targetPoint.X}, {targetPoint.Y}). Target Toward: {targetToward}");

				// 進行旋轉
				if (RotationSpeed > 0 && !IsApproximatelyEqual(Toward, targetToward))
				{
					double diffAngle = targetToward - Toward;
					double rotationAngle = RotationSpeed * time;
					// 逆時針旋轉
					if ((diffAngle > 0 && diffAngle < 180) || (diffAngle < -180 && diffAngle > -360))
					{
						if (Math.Abs(diffAngle) < rotationAngle)
							ChangePosition(X, Y, targetToward);
						else
							ChangePosition(X, Y, Toward + rotationAngle);
					}
					// 順時針旋轉
					else
					{
						if (Math.Abs(diffAngle) < rotationAngle)
							ChangePosition(X, Y, targetToward);
						else
							ChangePosition(X, Y, Toward - rotationAngle);
					}
				}
				// 進行平移
				else if (TranslationSpeed > 0 && IsApproximatelyEqual(Toward, targetToward) && !IsEqual(new Pair(X, Y), targetPoint))
				{
					double diffX = targetPoint.X - X;
					double diffY = targetPoint.Y - Y;
					double translateX = Math.Abs(Math.Cos(Toward * Math.PI / 180)) * TranslationSpeed * time;
					double translateY = Math.Abs(Math.Sin(Toward * Math.PI / 180)) * TranslationSpeed * time;

					// 向右向上移動
					if (diffX >= 0 && diffY >= 0)
					{
						if (X + translateX > targetPoint.X || Y + translateY > targetPoint.Y)
							ChangePosition(targetPoint.X, targetPoint.Y, Toward);
						else
							ChangePosition((int)(X + translateX), (int)(Y + translateY), Toward);
					}
					// 向左向上移動
					else if (diffX < 0 && diffY >= 0)
					{
						if (X - translateX < targetPoint.X || Y + translateY > targetPoint.Y)
							ChangePosition(targetPoint.X, targetPoint.Y, Toward);
						else
							ChangePosition((int)(X - translateX), (int)(Y + translateY), Toward);
					}
					// 向左向下移動
					else if (diffX < 0 && diffY < 0)
					{
						if (X - translateX < targetPoint.X || Y - translateY < targetPoint.Y)
							ChangePosition(targetPoint.X, targetPoint.Y, Toward);
						else
							ChangePosition((int)(X - translateX), (int)(Y - translateY), Toward);
					}
					// 向右向下移動
					else if (diffX >= 0 && diffY < 0) 
					{
						if (X + translateX > targetPoint.X || Y - translateY < targetPoint.Y)
							ChangePosition(targetPoint.X, targetPoint.Y, Toward);
						else
							ChangePosition((int)(X + translateX), (int)(Y - translateY), Toward);
					}

					// 判斷是否到達該點
					if (IsEqual(new Pair(X, Y), targetPoint))
					{
						Path.RemoveAt(0);
						PathChanged?.Invoke(Name, Path);
					}
				}

			}
		}

		private double CalculateHorizontalAngle(Pair src, Pair dst)
		{
			double result = 0;
			if (src != null && dst != null)
			{
				int diffX = dst.X - src.X;
				int diffY = dst.Y - src.Y;

				if (diffX == 0)
				{
					if (diffY > 0) result = 90;
					else result = 270;
				}
				else
				{
					result = Math.Atan2(diffY, diffX) / Math.PI * 180;
					if (result < 0) result += 360;
				}
			}
			return result;
		}

		private double CalculateDistance(Pair point1, Pair point2)
		{
			return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
		}

		private bool IsApproximatelyEqual(double num1, double num2)
		{
			return ((num1 + 0.1 > num2) && (num1 - 0.1 < num2));
		}

		private bool IsEqual(Pair point1, Pair point2)
		{
			return (point1.X == point2.X && point1.Y == point2.Y);
		}

		private void ChangePosition(int x, int y, double toward)
		{
			bool changed = false;
			if (X != x)
			{
				X = x;
				changed = true;
			}
			if (Y != y)
			{
				Y = y;
				changed = true;
			}
			if (Toward != toward)
			{
				Toward = toward;
				changed = true;
			}
			if (changed)
			{
				Console.WriteLine($"X:{X}, Y:{Y}, Toward:{Toward.ToString("F2")}");
				PositionChanged?.Invoke(Name, new TowardPair(X, Y, Toward));
			}
		}
	}
}
