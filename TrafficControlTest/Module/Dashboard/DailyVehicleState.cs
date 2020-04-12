using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.Dashboard
{
	public class DailyVehicleState
	{
		// Offline / Idle / Charge / Running / Pause / Alarm / ObstacleExists / Lock / RouteNotFind / BumperTrigger
		public string mVehicleId { get; private set; } = string.Empty;
		public DateTime mDate { get; private set; } = default(DateTime);
		public double mHourOfOffline { get; private set; } = default(double);
		public double mHourOfIdle { get; private set; } = default(double);
		public double mHourOfCharge { get; private set; } = default(double);
		public double mHourOfRunning { get; private set; } = default(double);
		public double mHourOfPause { get; private set; } = default(double);
		public double mHourOfAlarm { get; private set; } = default(double);
		public double mHourOfObstacleExists { get; private set; } = default(double);
		public double mHourOfLock { get; private set; } = default(double);
		public double mHourOfRouteNotFind { get; private set; } = default(double);
		public double mHourOfBumperTrigger { get; private set; } = default(double);

		public DailyVehicleState(string VehicleId, DateTime Date, double HourOfOffline, double HourOfIdle, double HourOfCharge, double HourOfRunning, double HourOfPause, double HourOfAlarm, double HourOfObstacleExists, double HourOfLock, double HourOfRouteNotFind, double HourOfBumperTrigger)
		{
			mVehicleId = VehicleId;
			mDate = Date.Date;
			mHourOfOffline = Math.Round(HourOfOffline, 1, MidpointRounding.AwayFromZero);
			mHourOfIdle = Math.Round(HourOfIdle, 1, MidpointRounding.AwayFromZero);
			mHourOfCharge = Math.Round(HourOfCharge, 1, MidpointRounding.AwayFromZero);
			mHourOfRunning = Math.Round(HourOfRunning, 1, MidpointRounding.AwayFromZero);
			mHourOfPause = Math.Round(HourOfPause, 1, MidpointRounding.AwayFromZero);
			mHourOfAlarm = Math.Round(HourOfAlarm, 1, MidpointRounding.AwayFromZero);
			mHourOfObstacleExists = Math.Round(HourOfObstacleExists, 1, MidpointRounding.AwayFromZero);
			mHourOfLock = Math.Round(HourOfLock, 1, MidpointRounding.AwayFromZero);
			mHourOfRouteNotFind = Math.Round(HourOfRouteNotFind, 1, MidpointRounding.AwayFromZero);
			mHourOfBumperTrigger = Math.Round(HourOfBumperTrigger, 1, MidpointRounding.AwayFromZero);
		}
		public double[] GetStateDurationCollection()
		{
			return new double[] { mHourOfOffline, mHourOfIdle, mHourOfCharge, mHourOfRunning, mHourOfPause + mHourOfAlarm + mHourOfObstacleExists + mHourOfLock + mHourOfRouteNotFind + mHourOfBumperTrigger };
		}
		public static string[] GetStateCollection()
		{
			return new string[] { "Offline", "Idle", "Charge", "Running", "Others" };
		}
	}
}
