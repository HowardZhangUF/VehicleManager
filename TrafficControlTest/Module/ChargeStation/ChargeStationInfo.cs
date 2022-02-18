using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.ChargeStation
{
    public class ChargeStationInfo : IChargeStationInfo
    {
        public event EventHandler<StatusUpdatedEventArgs> StatusUpdated;

        public string mName { get; private set; } = string.Empty;
        public ITowardPoint2D mLocation { get; private set; } = null;
        public bool mEnable { get; private set; } = true;
        public bool mIsBeUsing { get; private set; } = false;
        public DateTime mLastUpdated { get; private set; } = DateTime.Now;

        public ChargeStationInfo(string Name, ITowardPoint2D Location)
        {
            Set(Name, Location);
        }
        public void Set(string Name, ITowardPoint2D Location)
        {
            mName = Name;
            mLocation = Location;
            mLastUpdated = DateTime.Now;
        }
        public void UpdateEnable(bool Enable)
        {
            if (mEnable != Enable)
            {
                mEnable = Enable;
                mLastUpdated = DateTime.Now;
                RaiseEvent_StatusUpdated("Enable");
            }
        }
        public void UpdateIsBeUsing(bool IsBeUsing)
        {
            if (mIsBeUsing != IsBeUsing)
            {
                mIsBeUsing = IsBeUsing;
                mLastUpdated = DateTime.Now;
                RaiseEvent_StatusUpdated("IsBeUsing");
            }
        }
        public override string ToString()
        {
            return $"{mName}/Enable:{mEnable.ToString()}/IsBeUsing:{mIsBeUsing.ToString()}";
        }

        protected virtual void RaiseEvent_StatusUpdated(string StatusName, bool Sync = true)
        {
            if (Sync)
            {
                StatusUpdated?.Invoke(this, new StatusUpdatedEventArgs(DateTime.Now, mName, StatusName));
            }
            else
            {
                Task.Run(() => { StatusUpdated?.Invoke(this, new StatusUpdatedEventArgs(DateTime.Now, mName, StatusName)); });
            }
        }
    }
}
