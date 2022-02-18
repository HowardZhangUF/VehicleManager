using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace VehicleSimulator.New
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

		void SetName(string Name);
		void SetStatus(ESimulatorStatus Status);
		void SetLocation(int X, int Y, int Toward);
		void SetTarget(string Target);
		void SetPath(List<Point> Points);
		void SetScore(double Score);
		void SetBattery(double Battery);
	}
}
