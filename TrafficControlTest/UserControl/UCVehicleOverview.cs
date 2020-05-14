using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TrafficControlTest.UserControl.UcVehicleInfo;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Vehicle;
using TrafficControlTest.Module.General;

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
		public void Add(string Id, string Battery, string LocationScore, string State, string Target)
		{
			UcVehicleInfo ucVehicleInfo = new UcVehicleInfo() { mId = Id, mBattery = Battery, mLocationScore = LocationScore, mState = State, mTarget = Target, mBorderColor = Color.LightSalmon, Dock = DockStyle.Top, Height = UcVehicleInfo.DefaultHeight };
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
							case Property.LocationScore:
								(Controls[tmpName] as UcVehicleInfo).mLocationScore = Value;
								break;
							case Property.State:
								UcVehicleInfo tmpObj = (Controls[tmpName] as UcVehicleInfo);
								tmpObj.mState = Value;
								switch (tmpObj.mState)
								{
									case "Idle":
										tmpObj.mBorderColor = Color.Green;
										break;
									case "Running":
									case "Pause":
										tmpObj.mBorderColor = Color.Yellow;
										break;
									case "Charge":
										tmpObj.mBorderColor = Color.LawnGreen;
										break;
									default:
										tmpObj.mBorderColor = Color.Red;
										break;
								}
								break;
							case Property.Target:
								(Controls[tmpName] as UcVehicleInfo).mTarget = Value;
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
		private void HandleEvent_VehicleInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			Add(Args.Item.mName, Args.Item.mBatteryValue.ToString("F2"), Args.Item.mLocationScore.ToString("F2"), Args.Item.mCurrentState, Args.Item.mCurrentTarget);
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			Remove(Args.Item.mName);
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			if (Args.StatusName.Contains("BatteryValue")) Update(Args.Item.mName, Property.Battery, Args.Item.mBatteryValue.ToString("F2"));
			if (Args.StatusName.Contains("LocationScore")) Update(Args.Item.mName, Property.LocationScore, Args.Item.mLocationScore.ToString("F2"));
			if (Args.StatusName.Contains("CurrentState")) Update(Args.Item.mName, Property.State, Args.Item.mCurrentState);
			if (Args.StatusName.Contains("CurrentTarget")) Update(Args.Item.mName, Property.Target, Args.Item.mCurrentTarget);
		}
	}
}
