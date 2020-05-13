using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Communication;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.UserControl
{
	public partial class UcSystemOverview : System.Windows.Forms.UserControl
	{
		private IVehicleCommunicator rVehicleCommunicator = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private object mLock = new object();
		private List<UcVehicleIconView> mControls = new List<UcVehicleIconView>();

		public UcSystemOverview()
		{
			InitializeComponent();
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			UnsubscribeEvent_VehicleCommunicator(VehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			SubscribeEvent_VehicleCommunicator(VehicleCommunicator);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_VehicleInfoManager(VehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_VehicleInfoManager(VehicleInfoManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager)
		{
			Set(VehicleCommunicator);
			Set(VehicleInfoManager);
		}
		public void AddVehicleIconView(string Id, string State, string Target, string Battery, string LocationScore)
		{
			UcVehicleIconView ucVehicleIconView = new UcVehicleIconView() { mId = Id, mState = State, mTarget = Target, mBattery = Battery, mLocationScore = LocationScore, mBorderColor = Color.LightSalmon, Dock = DockStyle.Left, Width = UcVehicleIconView.DefaultWidth };
			string tmpName = ucVehicleIconView.Name;
			lock (mLock)
			{
				this.InvokeIfNecessary(() =>
				{
					if (mControls.FirstOrDefault(o => o.mId == Id) == null && !Controls.ContainsKey(tmpName))
					{
						Visible = false;
						mControls.Add(ucVehicleIconView);
						mControls = mControls.OrderBy(o => o.mId).ToList();

						Controls.Add(ucVehicleIconView);
						for (int i = 0; i < mControls.Count; ++i)
						{
							Controls.SetChildIndex(mControls[i], mControls.Count - i);
						}
						Visible = true;
					}
				});
			}
		}
		public void RemoveVehicleIconView(string Id)
		{
			string tmpName = UcVehicleIconView.PreFix + Id;
			lock (mLock)
			{
				this.InvokeIfNecessary(() =>
				{
					if (mControls.FirstOrDefault(o => o.mId == Id) != null && Controls.ContainsKey(tmpName))
					{
						mControls.Remove(mControls.FirstOrDefault(o => o.mId == Id));
						Controls.RemoveByKey(tmpName);
					}
				});
			}
		}
		public void UpdateVehicleIconView(string Id, UcVehicleIconView.Property Property, string Value)
		{
			string tmpName = UcVehicleIconView.PreFix + Id;
			lock (mLock)
			{
				this.InvokeIfNecessary(() =>
				{
					if (Controls.ContainsKey(tmpName))
					{
						switch (Property)
						{
							case UcVehicleIconView.Property.Id:
								(Controls[tmpName] as UcVehicleInfo).mId = Value;
								break;
							case UcVehicleIconView.Property.State:
								UcVehicleIconView tmpObj = (Controls[tmpName] as UcVehicleIconView);
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
							case UcVehicleIconView.Property.Target:
								(Controls[tmpName] as UcVehicleIconView).mTarget = Value;
								break;
							case UcVehicleIconView.Property.Battery:
								(Controls[tmpName] as UcVehicleIconView).mBattery = Value;
								break;
							case UcVehicleIconView.Property.LocationScore:
								(Controls[tmpName] as UcVehicleIconView).mLocationScore = Value;
								break;
							default:
								break;
						}
					}
				});
			}
		}

		private void SubscribeEvent_VehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.LocalListenStateChanged += HandleEvent_VehicleCommunicatorLocalListenStateChanged;
			}
		}
		private void UnsubscribeEvent_VehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.LocalListenStateChanged -= HandleEvent_VehicleCommunicatorLocalListenStateChanged;
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
		private void HandleEvent_VehicleCommunicatorLocalListenStateChanged(DateTime OccurTime, ListenState NewState, int Port)
		{
			if (NewState == ListenState.Listening)
			{
				UpdateGui_UpdateControlBackColor(lblConnection, Color.DarkOrange);
			}
			else
			{
				UpdateGui_UpdateControlBackColor(lblConnection, Color.DarkRed);
				UpdateGui_UpdateControlText(lblConnection, "0");
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(DateTime OccurTime, string Name, IVehicleInfo Item)
		{
			UpdateGui_UpdateControlBackColor(lblConnection, Color.DarkGreen);
			UpdateGui_UpdateControlText(lblConnection, rVehicleInfoManager.mCount.ToString());
			AddVehicleIconView(Item.mName, Item.mCurrentState, Item.mCurrentTarget, Item.mBatteryValue.ToString("F2"), Item.mLocationScore.ToString("F2"));
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(DateTime OccurTime, string Name, IVehicleInfo Item)
		{
			UpdateGui_UpdateControlBackColor(lblConnection, rVehicleInfoManager.mCount > 0 ? Color.DarkGreen : Color.DarkOrange);
			UpdateGui_UpdateControlText(lblConnection, rVehicleInfoManager.mCount.ToString());
			RemoveVehicleIconView(Item.mName);
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleInfo Item)
		{
			if (StateName.Contains("CurrentState")) UpdateVehicleIconView(Item.mName, UcVehicleIconView.Property.State, Item.mCurrentState);
			if (StateName.Contains("CurrentTarget")) UpdateVehicleIconView(Item.mName, UcVehicleIconView.Property.Target, Item.mCurrentTarget);
			if (StateName.Contains("BatteryValue")) UpdateVehicleIconView(Item.mName, UcVehicleIconView.Property.Battery, Item.mBatteryValue.ToString("F2"));
			if (StateName.Contains("LocationScore")) UpdateVehicleIconView(Item.mName, UcVehicleIconView.Property.LocationScore, Item.mLocationScore.ToString("F2"));
		}
		private void UpdateGui_UpdateControlText(Control Control, string Text)
		{
			Control.InvokeIfNecessary(() => { if (Control.Text != Text) Control.Text = Text; });
		}
		private void UpdateGui_UpdateControlBackColor(Control Control, Color Color)
		{
			Control.InvokeIfNecessary(() => { if (Control.BackColor != Color) Control.BackColor = Color; });
		}
	}
}
