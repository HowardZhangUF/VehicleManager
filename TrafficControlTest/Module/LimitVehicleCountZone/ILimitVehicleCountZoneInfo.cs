using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.LimitVehicleCountZone
{
	public interface ILimitVehicleCountZoneInfo : IItem
	{
		IRectangle2D mRange { get; }
		int mMaxVehicleCount { get; }
		List<string> mCurrentVehicleNameList { get; }
		List<string> mLastVehicleNameList { get; }
		TimeSpan mCurrentStatusDuration { get; }
		DateTime mLastUpdated { get; }

		void Set(string Name, IRectangle2D Range, int MaxVehicleCount);
		void UpdateCurrentVehicleNameList(List<string> CurrentVehicleNameList);
	}
}
