using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.ChargeStation
{
    public interface IChargeStationInfo : IItem
    {
        ITowardPoint2D mLocation { get; }
		IRectangle2D mLocationRange { get; }
        bool mEnable { get; }
        bool mIsBeingUsed { get; }
		TimeSpan mIsBeingUsedDuration { get; }
        DateTime mLastUpdated { get; }

        void Set(string Name, ITowardPoint2D Location, IRectangle2D LocationRange);
        void UpdateEnable(bool Enable);
        void UpdateIsBeingUsed(bool IsBeingUsed);
    }
}
