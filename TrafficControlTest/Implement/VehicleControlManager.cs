using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleControl;

namespace TrafficControlTest.Implement
{
	class VehicleControlManager : IVehicleControlManager
	{
		public event EventHandlerIVehicleControl ControlAdded;
		public event EventHandlerIVehicleControl ControlRemoved;
		public event EventHandlerIVehicleControlStateUpdated ControlStateUpdated;

		public IVehicleControl this[string Name] { get { return Get(Name); } }
		public int Count { get { return mVehicleControls.Count; } }

		private Dictionary<string, IVehicleControl> mVehicleControls = new Dictionary<string, IVehicleControl>();

		public VehicleControlManager()
		{

		}
		public bool IsExist(string Name)
		{
			return mVehicleControls.Keys.Contains(Name);
		}
		public bool IsCauseExist(string CauseId)
		{
			return mVehicleControls.Values.Any((o) => o.mCauseId == CauseId);
		}
		public IVehicleControl Get(string Name)
		{
			return (IsExist(Name) ? mVehicleControls[Name] : null);
		}
		public IVehicleControl GetViaCause(string CauseId)
		{
			return (IsCauseExist(CauseId) ? mVehicleControls.Values.First((o) => o.mCauseId == CauseId) : null);
		}
		public List<string> GetNames()
		{
			return (mVehicleControls.Count > 0 ? mVehicleControls.Keys.ToList() : null);
		}
		public List<IVehicleControl> GetList()
		{
			return (mVehicleControls.Count > 0 ? mVehicleControls.Values.ToList() : null);
		}
		public void Add(string Name, IVehicleControl Data)
		{
			if (!IsExist(Name))
			{
				mVehicleControls.Add(Name, Data);
				SubscribeEvent_IVehicleControl(mVehicleControls[Name]);
				RaiseEvent_ControlAdded(mVehicleControls[Name].mName, mVehicleControls[Name]);
			}
		}
		public void Remove(string Name)
		{
			if (IsExist(Name))
			{
				IVehicleControl tmpData = mVehicleControls[Name];
				UnsubscribeEvent_IVehicleControl(mVehicleControls[Name]);
				mVehicleControls.Remove(Name);
				RaiseEvent_ControlRemoved(Name, tmpData);
			}
		}
		public void UpdateSendState(string Name, SendState SendState)
		{
			if (IsExist(Name))
			{
				mVehicleControls[Name].UpdateSendState(SendState);
			}
		}
		public void UpdateParameters(string Name, string[] Parameters)
		{
			if (IsExist(Name))
			{
				mVehicleControls[Name].UpdateParameters(Parameters);
			}
		}

		private void SubscribeEvent_IVehicleControl(IVehicleControl VehicleControl)
		{
			if (VehicleControl != null)
			{
				VehicleControl.StateUpdated += HandleEvent_VehicleControlStateUpdated;
			}
		}
		private void UnsubscribeEvent_IVehicleControl(IVehicleControl VehicleControl)
		{
			if (VehicleControl != null)
			{
				VehicleControl.StateUpdated -= HandleEvent_VehicleControlStateUpdated;
			}
		}
		protected virtual void RaiseEvent_ControlAdded(string Name, IVehicleControl VehicleControl, bool Sync = true)
		{
			if (Sync)
			{
				ControlAdded?.Invoke(DateTime.Now, Name, VehicleControl);
			}
			else
			{
				Task.Run(() => { ControlAdded?.Invoke(DateTime.Now, Name, VehicleControl); });
			}
		}
		protected virtual void RaiseEvent_ControlRemoved(string Name, IVehicleControl VehicleControl, bool Sync = true)
		{
			if (Sync)
			{
				ControlRemoved?.Invoke(DateTime.Now, Name, VehicleControl);
			}
			else
			{
				Task.Run(() => { ControlRemoved?.Invoke(DateTime.Now, Name, VehicleControl); });
			}
		}
		protected virtual void RaiseEvent_ControlStateUpdated(string Name, string StateName, IVehicleControl VehicleControl, bool Sync = true)
		{
			if (Sync)
			{
				ControlStateUpdated?.Invoke(DateTime.Now, Name, StateName, VehicleControl);
			}
			else
			{
				Task.Run(() => { ControlStateUpdated?.Invoke(DateTime.Now, Name, StateName, VehicleControl); });
			}
		}
		private void HandleEvent_VehicleControlStateUpdated(DateTime OccurTime, string Name, string StateName, IVehicleControl VehicleControl)
		{
			RaiseEvent_ControlStateUpdated(Name, StateName, VehicleControl);
		}
	}
}
