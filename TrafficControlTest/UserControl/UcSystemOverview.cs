using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Vehicle;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.NewCommunication;
using LibraryForVM;

namespace TrafficControlTest.UserControl
{
	public partial class UcSystemOverview : System.Windows.Forms.UserControl
	{
		private IVehicleCommunicator rVehicleCommunicator = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private ITimeElapseDetector rTimeElapseDetector = null;
		private object mLock = new object();
		private List<UcVehicleIconView> mControls = new List<UcVehicleIconView>();
		private ToolTip mToolTipOfDate = new ToolTip();

		public UcSystemOverview()
		{
			InitializeComponent();
			HandleEvent_TimeElapseDetectorDayChanged(null, null);
			HandleEvent_TimeElapseDetectorMinuteChanged(null, null);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			UnsubscribeEvent_VehicleCommunicator(rVehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			SubscribeEvent_VehicleCommunicator(rVehicleCommunicator);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_VehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_VehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(ITimeElapseDetector TimeElapseDetector)
		{
			UnsubscribeEvent_TimeElapseDetector(rTimeElapseDetector);
			rTimeElapseDetector = TimeElapseDetector;
			SubscribeEvent_TimeElapseDetector(rTimeElapseDetector);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager, ITimeElapseDetector TimeElapseDetector)
		{
			Set(VehicleCommunicator);
			Set(VehicleInfoManager);
			Set(TimeElapseDetector);
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
									case "ChargeIdle":
										tmpObj.mBorderColor = Color.Green;
										break;
									case "Running":
									case "Operating":
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
		private void SubscribeEvent_TimeElapseDetector(ITimeElapseDetector TimeElapseDetector)
		{
			if (TimeElapseDetector != null)
			{
				TimeElapseDetector.YearChanged += HandleEvent_TimeElapseDetectorYearChanged;
				TimeElapseDetector.MonthChanged += HandleEvent_TimeElapseDetectorMonthChanged;
				TimeElapseDetector.DayChanged += HandleEvent_TimeElapseDetectorDayChanged;
				TimeElapseDetector.HourChanged += HandleEvent_TimeElapseDetectorHourChanged;
				TimeElapseDetector.MinuteChanged += HandleEvent_TimeElapseDetectorMinuteChanged;
			}
		}
		private void UnsubscribeEvent_TimeElapseDetector(ITimeElapseDetector TimeElapseDetector)
		{
			if (TimeElapseDetector != null)
			{
				TimeElapseDetector.YearChanged -= HandleEvent_TimeElapseDetectorYearChanged;
				TimeElapseDetector.MonthChanged -= HandleEvent_TimeElapseDetectorMonthChanged;
				TimeElapseDetector.DayChanged -= HandleEvent_TimeElapseDetectorDayChanged;
				TimeElapseDetector.HourChanged -= HandleEvent_TimeElapseDetectorHourChanged;
				TimeElapseDetector.MinuteChanged -= HandleEvent_TimeElapseDetectorMinuteChanged;
			}
		}
		private void HandleEvent_VehicleCommunicatorLocalListenStateChanged(object Sender, ListenStateChangedEventArgs Args)
		{
			if (Args.IsListened)
			{
				UpdateGui_UpdateControlBackColor(lblConnection, Color.DarkOrange);
			}
			else
			{
				UpdateGui_UpdateControlBackColor(lblConnection, Color.DarkRed);
				UpdateGui_UpdateControlText(lblConnection, "0");
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			UpdateGui_UpdateControlBackColor(lblConnection, Color.DarkGreen);
			UpdateGui_UpdateControlText(lblConnection, rVehicleInfoManager.mCount.ToString());
			AddVehicleIconView(Args.Item.mName, Args.Item.mCurrentState, Args.Item.mCurrentTarget, Args.Item.mBatteryValue.ToString("F2"), Args.Item.mLocationScore.ToString("F2"));
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			UpdateGui_UpdateControlBackColor(lblConnection, rVehicleInfoManager.mCount > 0 ? Color.DarkGreen : Color.DarkOrange);
			UpdateGui_UpdateControlText(lblConnection, rVehicleInfoManager.mCount.ToString());
			RemoveVehicleIconView(Args.Item.mName);
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			if (Args.StatusName.Contains("CurrentState")) UpdateVehicleIconView(Args.Item.mName, UcVehicleIconView.Property.State, Args.Item.mCurrentState);
			if (Args.StatusName.Contains("CurrentTarget")) UpdateVehicleIconView(Args.Item.mName, UcVehicleIconView.Property.Target, Args.Item.mCurrentTarget);
			if (Args.StatusName.Contains("BatteryValue")) UpdateVehicleIconView(Args.Item.mName, UcVehicleIconView.Property.Battery, Args.Item.mBatteryValue.ToString("F2"));
			if (Args.StatusName.Contains("LocationScore")) UpdateVehicleIconView(Args.Item.mName, UcVehicleIconView.Property.LocationScore, Args.Item.mLocationScore.ToString("F2"));
		}
		private void HandleEvent_TimeElapseDetectorYearChanged(object Sender, DateTimeChangedEventArgs Args)
		{
			lblClock.InvokeIfNecessary(() => { mToolTipOfDate.SetToolTip(lblClock, DateTime.Now.ToString("yyyy/MM/dd")); });
		}
		private void HandleEvent_TimeElapseDetectorMonthChanged(object Sender, DateTimeChangedEventArgs Args)
		{
			lblClock.InvokeIfNecessary(() => { mToolTipOfDate.SetToolTip(lblClock, DateTime.Now.ToString("yyyy/MM/dd")); });
		}
		private void HandleEvent_TimeElapseDetectorDayChanged(object Sender, DateTimeChangedEventArgs Args)
		{
			lblClock.InvokeIfNecessary(() => { mToolTipOfDate.SetToolTip(lblClock, DateTime.Now.ToString("yyyy/MM/dd")); });
		}
		private void HandleEvent_TimeElapseDetectorHourChanged(object Sender, DateTimeChangedEventArgs Args)
		{
			UpdateGui_UpdateControlText(lblClock, DateTime.Now.ToString("HH:mm"));
		}
		private void HandleEvent_TimeElapseDetectorMinuteChanged(object Sender, DateTimeChangedEventArgs Args)
		{
			UpdateGui_UpdateControlText(lblClock, DateTime.Now.ToString("HH:mm"));
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
