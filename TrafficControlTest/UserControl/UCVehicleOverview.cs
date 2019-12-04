using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TrafficControlTest.UserControl.UcVehicleInfo;
using TrafficControlTest.Interface;

namespace TrafficControlTest.UserControl
{
	public partial class UcVehicleOverview : System.Windows.Forms.UserControl
	{
		public delegate void EventHandlerString(string VehicleName);
		public event EventHandlerString DoubleClickOnVehicleInfo;

		private IVehicleInfoManager rVehicleInfoManager = null;
		private object mLock = new object();
		private List<UcVehicleInfo> mControls = new List<UcVehicleInfo>();

		public UcVehicleOverview()
		{
			InitializeComponent();
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_VehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_VehicleInfoManager(rVehicleInfoManager);
		}
		public void Add(string Id, string Battery, string State)
		{
			UcVehicleInfo ucVehicleInfo = new UcVehicleInfo() { mId = Id, mBattery = Battery, mState = State, mBorderColor = Color.LightSalmon, Dock = DockStyle.Top, Height = UcVehicleInfo.DefaultHeight };
			string tmpName = ucVehicleInfo.Name;
			lock (mLock)
			{
				this.InvokeIfNecessary(() =>
				{
					if (mControls.FirstOrDefault((o) => o.mId == Id) == null && !Controls.ContainsKey(tmpName))
					{
						Visible = false;
						mControls.Add(ucVehicleInfo);
						mControls = mControls.OrderBy((o) => o.mId).ToList();

						Controls.Add(ucVehicleInfo);
						// Child Index 越大， Dock 位置越靠邊
						Controls.SetChildIndex(lblTitle, mControls.Count);
						for (int i = 0; i < mControls.Count; ++i)
						{
							Controls.SetChildIndex(mControls[i], mControls.Count - 1 - i);
						}
						Visible = true;
						SubscribeEvent_UCVehicleInfo(ucVehicleInfo);
					}
				});
			}
		}
		public void Remove(string Id)
		{
			string tmpName = UcVehicleInfo.PreFix + Id;
			lock (mLock)
			{
				this.InvokeIfNecessary(() =>
				{
					if (mControls.FirstOrDefault((o) => o.mId == Id) != null && Controls.ContainsKey(tmpName))
					{
						UnsubscribeEvent_UCVehicleInfo(Controls[tmpName] as UcVehicleInfo);
						mControls.Remove(mControls.FirstOrDefault((o) => o.mId == Id));
						Controls.RemoveByKey(tmpName);
					}
				});
			}
		}
		public void Update(string Id, Property Property, string Value)
		{
			string tmpName = UcVehicleInfo.PreFix + Id;
			lock (mLock)
			{
				this.InvokeIfNecessary(() =>
				{
					if (Controls.ContainsKey(tmpName))
					{
						switch (Property)
						{
							case Property.Id:
								(Controls[tmpName] as UcVehicleInfo).mId = Value;
								break;
							case Property.Battery:
								(Controls[tmpName] as UcVehicleInfo).mBattery = Value;
								break;
							case Property.State:
								(Controls[tmpName] as UcVehicleInfo).mState = Value;
								break;
							default:
								break;
						}
					}
				});
			}
		}

		private void SubscribeEvent_VehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded += HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved += HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated += HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_VehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded += HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved += HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated += HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void SubscribeEvent_UCVehicleInfo(UcVehicleInfo UCVehicleInfo)
		{
			if (UCVehicleInfo != null)
			{
				UCVehicleInfo.DoubleClickOnControl += HandleEvent_UCVehicleInfoDoubleClickOnControl;
			}
		}
		private void UnsubscribeEvent_UCVehicleInfo(UcVehicleInfo UCVehicleInfo)
		{
			if (UCVehicleInfo != null)
			{
				UCVehicleInfo.DoubleClickOnControl -= HandleEvent_UCVehicleInfoDoubleClickOnControl;
			}
		}
		protected virtual void RaiseEvent_DoubleClickOnVehicleInfo(string VehicleName, bool Sync = true)
		{
			if (Sync)
			{
				DoubleClickOnVehicleInfo?.Invoke(VehicleName);
			}
			else
			{
				Task.Run(() => { DoubleClickOnVehicleInfo?.Invoke(VehicleName); });
			}
		}
		private void HandleEvent_UCVehicleInfoDoubleClickOnControl(object sender, EventArgs e)
		{
			if (sender is UcVehicleInfo)
			{
				string controlName = (sender as UcVehicleInfo).Name;
				string vehicleName = controlName.Replace(UcVehicleInfo.PreFix, string.Empty);
				RaiseEvent_DoubleClickOnVehicleInfo(vehicleName);
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(DateTime OccurTime, string Name, IVehicleInfo Item)
		{
			Add(Item.mName, Item.mBatteryValue.ToString("F2"), Item.mCurrentState);
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(DateTime OccurTime, string Name, IVehicleInfo Item)
		{
			Remove(Item.mName);
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleInfo Item)
		{
			Update(Item.mName, Property.Battery, Item.mBatteryValue.ToString("F2"));
			Update(Item.mName, Property.State, Item.mCurrentState);
		}
	}
}
