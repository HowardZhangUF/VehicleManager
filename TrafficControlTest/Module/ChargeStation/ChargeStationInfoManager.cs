using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.ChargeStation
{
    public class ChargeStationInfoManager : ItemManager<IChargeStationInfo>, IChargeStationInfoManager
    {
        public void UpdateEnable(string Name, bool Enable)
        {
            lock (mLock)
            {
                if (mItems.Keys.Contains(Name))
                {
                    mItems[Name].UpdateEnable(Enable);
                }
            }
        }
        public void UpdateIsBeUsing(string Name, bool IsBeUsing)
        {
            lock (mLock)
            {
                if (mItems.Keys.Contains(Name))
                {
                    mItems[Name].UpdateIsBeUsing(IsBeUsing);
                }
            }
        }
    }
}
