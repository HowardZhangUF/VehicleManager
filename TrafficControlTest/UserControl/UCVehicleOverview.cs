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
using TrafficControlTest.Module.Mission;

namespace TrafficControlTest.UserControl
{
	public partial class UcVehicleOverview : System.Windows.Forms.UserControl
	{
		public delegate void EventHandlerString(string VehicleName);
		public event EventHandlerString DoubleClickOnVehicleInfo;

		private IVehicleInfoManager rVehicleInfoManager = null;
		private IMissionStateManager rMissionStateManager = null;
		private object mLockOfUcVehicleInfoCollection = new object();
		private object mLockOfUcMissionInfoCollection = new object();
		private List<UcVehicleInfo> mUcVehicleInfoCollection = new List<UcVehicleInfo>();
		private List<UcMissionInfo> mUcMissionInfoCollection = new List<UcMissionInfo>();

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
		public void Set(IMissionStateManager MissionStateManager)
		{
			UnsubscribeEvent_MissionStateManager(rMissionStateManager);
			rMissionStateManager = MissionStateManager;
			SubscribeEvent_MissionStateManager(rMissionStateManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager)
		{
			Set(VehicleInfoManager);
			Set(MissionStateManager);
		}
		public void AddUcVehicleInfo(string Id, string Battery, string LocationScore, string State, string Target)
		{
			UcVehicleInfo ucVehicleInfo = new UcVehicleInfo() { mId = Id, mBattery = Battery, mLocationScore = LocationScore, mState = State, mTarget = Target, mBorderColor = Color.LightSalmon, Dock = DockStyle.Top, Height = UcVehicleInfo.DefaultHeight };
			string tmpName = ucVehicleInfo.Name;
			lock (mLockOfUcVehicleInfoCollection)
			{
				this.InvokeIfNecessary(() =>
				{
					if (mUcVehicleInfoCollection.FirstOrDefault((o) => o.mId == Id) == null && !pnlVehicleInfo.Controls.ContainsKey(tmpName))
					{
						Visible = false;
						mUcVehicleInfoCollection.Add(ucVehicleInfo);
						mUcVehicleInfoCollection = mUcVehicleInfoCollection.OrderBy((o) => o.mId).ToList();

						pnlVehicleInfo.Controls.Add(ucVehicleInfo);
						// Child Index 越大， Dock 位置越靠邊， Child Index 必須要有 0 ，不然排序會不如預期
						for (int i = 0; i < mUcVehicleInfoCollection.Count; ++i)
						{
							pnlVehicleInfo.Controls.SetChildIndex(mUcVehicleInfoCollection[i], mUcVehicleInfoCollection.Count - 1 - i);
						}
						Visible = true;
						SubscribeEvent_UCVehicleInfo(ucVehicleInfo);
					}
				});
			}
		}
		public void RemoveUcVehicleInfo(string Id)
		{
			string tmpName = UcVehicleInfo.PreFix + Id;
			lock (mLockOfUcVehicleInfoCollection)
			{
				this.InvokeIfNecessary(() =>
				{
					if (mUcVehicleInfoCollection.FirstOrDefault((o) => o.mId == Id) != null && pnlVehicleInfo.Controls.ContainsKey(tmpName))
					{
						UnsubscribeEvent_UCVehicleInfo(pnlVehicleInfo.Controls[tmpName] as UcVehicleInfo);
						mUcVehicleInfoCollection.Remove(mUcVehicleInfoCollection.FirstOrDefault((o) => o.mId == Id));
						pnlVehicleInfo.Controls.RemoveByKey(tmpName);
					}
				});
			}
		}
		public void UpdateUcVehicleInfo(string Id, Property Property, string Value)
		{
			string tmpName = UcVehicleInfo.PreFix + Id;
			lock (mLockOfUcVehicleInfoCollection)
			{
				this.InvokeIfNecessary(() =>
				{
					if (pnlVehicleInfo.Controls.ContainsKey(tmpName))
					{
						switch (Property)
						{
							case Property.Id:
								(pnlVehicleInfo.Controls[tmpName] as UcVehicleInfo).mId = Value;
								break;
							case Property.Battery:
								(pnlVehicleInfo.Controls[tmpName] as UcVehicleInfo).mBattery = Value;
								break;
							case Property.LocationScore:
								(pnlVehicleInfo.Controls[tmpName] as UcVehicleInfo).mLocationScore = Value;
								break;
							case Property.State:
								UcVehicleInfo tmpObj = (pnlVehicleInfo.Controls[tmpName] as UcVehicleInfo);
								tmpObj.mState = Value;
								switch (tmpObj.mState)
								{
									case "Idle":
									case "ChargeIdle":
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
								(pnlVehicleInfo.Controls[tmpName] as UcVehicleInfo).mTarget = Value;
								break;
							default:
								break;
						}
					}
				});
			}
		}
		public void AddUcMissionInfo(string Id, string Type, string Parameter, string State, string ExecutorId, int Priority, DateTime ReceivedTimestamp)
		{
			UcMissionInfo ucMissionInfo = new UcMissionInfo() { mId = Id, mType = Type, mParameter = Parameter, mExecutorId = ExecutorId, mState = State, mPriority = Priority, mReceivedTimestamp = ReceivedTimestamp, Dock = DockStyle.Top, Height = UcMissionInfo.DefaultHeight };
			string tmpName = ucMissionInfo.Name;
			lock (mLockOfUcMissionInfoCollection)
			{
				pnlMissionInfo.InvokeIfNecessary(() =>
				{
					if (mUcMissionInfoCollection.Any(o => o.mId == Id) == false && !pnlMissionInfo.Controls.ContainsKey(tmpName))
					{
						pnlMissionInfo.Visible = false;
						mUcMissionInfoCollection.Add(ucMissionInfo);
						mUcMissionInfoCollection = mUcMissionInfoCollection.OrderBy(o => o.mPriority).ThenBy(o => o.mReceivedTimestamp).ToList();

						pnlMissionInfo.Controls.Add(ucMissionInfo);
						// Child Index 越大， Dock 位置越靠邊， Child Index 必須要有 0 ，不然排序會不如預期
						for (int i = 0; i < mUcMissionInfoCollection.Count; ++i)
						{
							pnlMissionInfo.Controls.SetChildIndex(mUcMissionInfoCollection[i], mUcMissionInfoCollection.Count - 1 - i);
						}
						pnlMissionInfo.Visible = true;
					}
				});
			}
		}
		public void RemoveUcMissionInfo(string Id)
		{
			string tmpName = UcMissionInfo.Prefix + Id;
			lock (mLockOfUcMissionInfoCollection)
			{
				this.InvokeIfNecessary(() =>
				{
					if (mUcMissionInfoCollection.FirstOrDefault(o => o.mId == Id) != null && pnlMissionInfo.Controls.ContainsKey(tmpName))
					{
						mUcMissionInfoCollection.Remove(mUcMissionInfoCollection.FirstOrDefault(o => o.mId == Id));
						pnlMissionInfo.Controls.RemoveByKey(tmpName);
					}
				});
			}
		}
		public void UpdateUcMissionInfo(string Id, UcMissionInfo.MissionProperty Property, string Value)
		{
			string tmpName = UcMissionInfo.Prefix + Id;
			lock (mLockOfUcMissionInfoCollection)
			{
				this.InvokeIfNecessary(() =>
				{
					if (pnlMissionInfo.Controls.ContainsKey(tmpName))
					{
						switch (Property)
						{
							case UcMissionInfo.MissionProperty.State:
								(pnlMissionInfo.Controls[tmpName] as UcMissionInfo).mState = Value;
								break;
							case UcMissionInfo.MissionProperty.ExecutorId:
								(pnlMissionInfo.Controls[tmpName] as UcMissionInfo).mExecutorId = Value;
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
		private void SubscribeEvent_MissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemAdded += HandleEvent_MissionStateManagerItemAdded;
				MissionStateManager.ItemRemoved += HandleEvent_MissionStateManagerItemRemoved;
				MissionStateManager.ItemUpdated += HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_MissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemAdded += HandleEvent_MissionStateManagerItemAdded;
				MissionStateManager.ItemRemoved += HandleEvent_MissionStateManagerItemRemoved;
				MissionStateManager.ItemUpdated += HandleEvent_MissionStateManagerItemUpdated;
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
		private void HandleEvent_VehicleInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			AddUcVehicleInfo(Args.Item.mName, Args.Item.mBatteryValue.ToString("F2"), Args.Item.mLocationScore.ToString("F2"), Args.Item.mCurrentState, Args.Item.mCurrentTarget);
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			RemoveUcVehicleInfo(Args.Item.mName);
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			if (Args.StatusName.Contains("BatteryValue")) UpdateUcVehicleInfo(Args.Item.mName, Property.Battery, Args.Item.mBatteryValue.ToString("F2"));
			if (Args.StatusName.Contains("LocationScore")) UpdateUcVehicleInfo(Args.Item.mName, Property.LocationScore, Args.Item.mLocationScore.ToString("F2"));
			if (Args.StatusName.Contains("CurrentState")) UpdateUcVehicleInfo(Args.Item.mName, Property.State, Args.Item.mCurrentState);
			if (Args.StatusName.Contains("CurrentTarget")) UpdateUcVehicleInfo(Args.Item.mName, Property.Target, Args.Item.mCurrentTarget);
		}
		private void HandleEvent_MissionStateManagerItemAdded(object Sender, ItemCountChangedEventArgs<IMissionState> Args)
		{
			AddUcMissionInfo(Args.Item.mName, Args.Item.mMission.mMissionType.ToString(), Args.Item.mMission.mParametersString, Args.Item.mExecuteState.ToString(), Args.Item.mExecutorId, Args.Item.mMission.mPriority, Args.Item.mReceivedTimestamp);
		}
		private void HandleEvent_MissionStateManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IMissionState> Args)
		{
			RemoveUcMissionInfo(Args.Item.mName);
		}
		private void HandleEvent_MissionStateManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IMissionState> Args)
		{
			if (Args.StatusName.Contains("ExecutorId")) UpdateUcMissionInfo(Args.Item.mName, UcMissionInfo.MissionProperty.ExecutorId, Args.Item.mExecutorId);
			if (Args.StatusName.Contains("ExecuteState")) UpdateUcMissionInfo(Args.Item.mName, UcMissionInfo.MissionProperty.State, Args.Item.mExecuteState.ToString());
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
	}
}
