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
		int mMaxVehicleCount { get; } // 當與其他區有聯集關係時，此項目的數值應一樣。例：區域 A 的最大數量為 numberA ，區域 B 的最大數量 numberB ，則聯集區域總和的最大數量 numberTotal = numberA = numberB
		bool mIsUnioned { get; }
		int mUnionId { get; }
		List<Tuple<string, DateTime>> mCurrentVehicleNameList { get; }
		List<Tuple<string, DateTime>> mLastVehicleNameList { get; }
		TimeSpan mCurrentStatusDuration { get; }
		DateTime mLastUpdated { get; }

		Dictionary<string,string> mLetgo { get; set; }

		void Set(string Name, IRectangle2D Range, int MaxVehicleCount, bool IsUnioned, int UnionId);
		void UpdateCurrentVehicleNameList(List<Tuple<string, DateTime>> CurrentVehicleNameList);
		bool ContainsVehicle(string VehicleName);
		DateTime GetVehicleEnterTimestamp(string VehicleName);
	}
}
