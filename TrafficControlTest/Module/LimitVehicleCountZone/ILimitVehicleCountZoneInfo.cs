using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.LimitVehicleCountZone
{
	public interface ILimitVehicleCountZoneInfo : IItem
	{
		IRectangle2D mRange { get; }
		int mMaxVehicleCount { get; }
		bool mIsUnioned { get; }
		int mUnionId { get; }
		List<Tuple<string, DateTime>> mCurrentVehicleNameList { get; }
		List<Tuple<string, DateTime>> mLastVehicleNameList { get; }
		TimeSpan mCurrentStatusDuration { get; }
		DateTime mLastUpdated { get; }

		void Set(string Name, IRectangle2D Range, int MaxVehicleCount, bool IsUnioned, int UnionId);
		void UpdateCurrentVehicleNameList(List<string> CurrentVehicleNameList);
	}
}
