using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.ChargeStation
{
    public interface IChargeStationInfoManager : IItemManager<IChargeStationInfo>
    {
        void UpdateEnable(string Name, bool Enable);
        void UpdateIsBeUsing(string Name, bool IsBeUsing);
    }
}
