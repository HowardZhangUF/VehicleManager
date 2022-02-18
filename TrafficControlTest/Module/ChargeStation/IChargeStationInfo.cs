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
        bool mEnable { get; }
        bool mIsBeUsing { get; }
        DateTime mLastUpdated { get; }

        void Set(string Name, ITowardPoint2D Location);
        void UpdateEnable(bool Enable);
        void UpdateIsBeUsing(bool IsBeUsing);
    }
}
