using LibraryForVM;
using System.Linq;

namespace TrafficControlTest.Module.InterveneCommand
{
	class VehicleControlManager : ItemManager<IVehicleControl>, IVehicleControlManager
	{
		public VehicleControlManager()
		{

		}
		public bool IsExistByCauseId(string CauseId)
		{
			return mItems.Values.Any(o => o.mCauseId == CauseId);
		}
		public IVehicleControl GetItemByCauseId(string CauseId)
		{
			return (IsExistByCauseId(CauseId) ? mItems.Values.FirstOrDefault((o) => o.mCauseId == CauseId) : null);
		}
		public void UpdateSendState(string Name, SendState SendState)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdateSendState(SendState);
			}
		}
		public void UpdateExecuteState(string Name, ExecuteState ExecuteState)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdateExecuteState(ExecuteState);
			}
		}
		public void UpdateExecuteFailedReason(string Name, FailedReason ExecuteFailedReason)
		{
			if (IsExist(Name))
			{
				mItems[Name].UpdateExecuteFailedReason(ExecuteFailedReason);
			}
		}
	}
}
