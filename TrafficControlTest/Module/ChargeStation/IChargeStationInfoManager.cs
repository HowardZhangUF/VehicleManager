using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest.Module.ChargeStation
{
    public interface IChargeStationInfoManager : IItemManager<IChargeStationInfo>
    {
        void UpdateEnable(string Name, bool Enable);
        void UpdateIsBeingUsed(string Name, bool IsBeingUsed);
    }
}
