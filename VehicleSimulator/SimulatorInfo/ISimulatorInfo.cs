using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	public interface ISimulatorInfo : IItem
	{
		ESimulatorStatus mStatus { get; }
		int mX { get; }
		int mY { get; }
		int mToward { get; }
		string mTarget { get; }
		List<Point> mPath { get; }
		double mScore { get; }
		double mBattery { get; }
		int mTranslateVelocity { get; }
		int mRotateVelocity { get; }
		string mMapFilePath { get; }

		void SetName(string Name);
		void SetStatus(ESimulatorStatus Status);
		void SetLocation(int X, int Y, int Toward);
		void SetTarget(string Target);
		void SetPath(List<Point> Points);
		void SetScore(double Score);
		void SetBattery(double Battery);
		void SetTranslateVelocity(int TranslateVelocity);
		void SetRotateVelocity(int RotateVelocity);
		void SetMapFilePath(string MapFilePath);
	}
}
