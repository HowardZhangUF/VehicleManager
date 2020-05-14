using System;
using System.Data;
using System.Linq;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.Vehicle;

namespace TrafficControlTest.UserControl
{
	public partial class UcVehicle : System.Windows.Forms.UserControl
	{
		public string CurrentVehicleName
		{
			get
			{
				string result = null;
				cbVehicleNameList.InvokeIfNecessary(() =>
				{
					result = cbVehicleNameList.SelectedItem == null ? string.Empty : cbVehicleNameList.SelectedItem.ToString();
				});
				return result;
			}
		}

		private IVehicleInfoManager rVehicleInfoManager = null;

		public UcVehicle()
		{
			InitializeComponent();
			InitializeLabelText();
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_VehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_VehicleInfoManager(rVehicleInfoManager);
		}
		public void UpdateGui_UpdateVehicleNameList(string[] VehicleNameList)
		{
			cbVehicleNameList.InvokeIfNecessary(() =>
			{
				if (VehicleNameList == null || VehicleNameList.Count() == 0)
				{
					cbVehicleNameList.Items.Clear();
					InitializeLabelText();
				}
				else
				{
					string lastSelectedItemText = cbVehicleNameList.SelectedItem != null ? cbVehicleNameList.SelectedItem.ToString() : string.Empty;
					cbVehicleNameList.SelectedIndex = -1;
					cbVehicleNameList.Items.Clear();
					cbVehicleNameList.Items.AddRange(VehicleNameList.OrderBy((o) => o).ToArray());
					if (!string.IsNullOrEmpty(lastSelectedItemText))
					{
						for (int i = 0; i < cbVehicleNameList.Items.Count; ++i)
						{
							if (lastSelectedItemText == cbVehicleNameList.Items[i].ToString())
							{
								cbVehicleNameList.SelectedIndex = i;
								return;
							}
						}
						InitializeLabelText();
					}
				}
			});
		}
		public void UpdateGui_UpdateVehicleState(string State)
		{
			lblVehicleState.InvokeIfNecessary(() =>
			{
				if (lblVehicleState.Text != State) lblVehicleState.Text = State;
			});
		}
		public void UpdateGui_UpdateVehicleLocation(int X, int Y, double Toward)
		{
			lblVehicleLocation.InvokeIfNecessary(() =>
			{
				if (lblVehicleLocation.Text != $"({X}, {Y}, {Toward.ToString("F2")})") lblVehicleLocation.Text = $"({X}, {Y}, {Toward.ToString("F2")})";
			});
		}
		public void UpdateGui_UpdateVehicleTarget(string Target)
		{
			lblVehicleTarget.InvokeIfNecessary(() =>
			{
				if (lblVehicleTarget.Text != Target) lblVehicleTarget.Text = Target;
			});
		}
		public void UpdateGui_UpdateVehicleVelocity(double Velocity)
		{
			lblVehicleVelocity.InvokeIfNecessary(() =>
			{
				if (lblVehicleVelocity.Text != $"{Velocity.ToString("F2")} (mm/s)") lblVehicleVelocity.Text = $"{Velocity.ToString("F2")} (mm/s)";
			});
		}
		public void UpdateGui_UpdateVehicleLocationScore(double LocationScore)
		{
			lblVehicleLocationScore.InvokeIfNecessary(() =>
			{
				if (lblVehicleLocationScore.Text != $"{LocationScore.ToString("F2")} %") lblVehicleLocationScore.Text = $"{LocationScore.ToString("F2")} %";
			});
		}
		public void UpdateGui_UpdateVehicleBatteryValue(double BatteryValue)
		{
			lblVehicleBatteryValue.InvokeIfNecessary(() =>
			{
				if (lblVehicleBatteryValue.Text != $"{BatteryValue.ToString("F2")} %") lblVehicleBatteryValue.Text = $"{BatteryValue.ToString("F2")} %";
			});
		}
		public void UpdateGui_UpdateVehicleAlarmMessage(string AlarmMessage)
		{
			lblVehicleAlarmMessage.InvokeIfNecessary(() =>
			{
				if (lblVehicleAlarmMessage.Text != AlarmMessage) lblVehicleAlarmMessage.Text = AlarmMessage;
			});
		}
		public void UpdateGui_UpdateVehiclePath(string Path)
		{
			lblVehiclePath.InvokeIfNecessary(() =>
			{
				if (lblVehiclePath.Text != Path) lblVehiclePath.Text = Path;
			});
		}
		public void UpdateGui_UpdateVehicleIpPort(string IpPort)
		{
			lblVehicleIpPort.InvokeIfNecessary(() =>
			{
				if (lblVehicleIpPort.Text != IpPort) lblVehicleIpPort.Text = IpPort;
			});
		}
		public void UpdateGui_UpdateVehicleMissionId(string MissionId)
		{
			lblVehicleMissionId.InvokeIfNecessary(() =>
			{
				if (lblVehicleMissionId.Text != MissionId) lblVehicleMissionId.Text = MissionId;
			});
		}
		public void UpdateGui_UpdateVehicleInterveneCommand(string InterveneCommand)
		{
			lblVehicleInterveneCommand.InvokeIfNecessary(() =>
			{
				if (lblVehicleInterveneCommand.Text != InterveneCommand) lblVehicleInterveneCommand.Text = InterveneCommand;
			});
		}
		public void UpdateGui_UpdateVehicleMapName(string MapName)
		{
			lblVehicleMapName.InvokeIfNecessary(() =>
			{
				if (lblVehicleMapName.Text != MapName) lblVehicleMapName.Text = MapName;
			});
		}
		public void UpdateGui_UpdateVehicleLastUpdateTime(string Time)
		{
			lblVehicleLastUpdateTime.InvokeIfNecessary(() =>
			{
				if (lblVehicleLastUpdateTime.Text != Time) lblVehicleLastUpdateTime.Text = Time;
			});
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
				VehicleInfoManager.ItemAdded -= HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved -= HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated -= HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			UpdateGui_UpdateVehicleNameList(rVehicleInfoManager.GetItemNames().ToArray());
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			UpdateGui_UpdateVehicleNameList(rVehicleInfoManager.GetItemNames().ToArray());
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			if (CurrentVehicleName == Name)
			{
				UpdateGui_UpdateVehicleState(Args.Item.mCurrentState);
				UpdateGui_UpdateVehicleLocation(Args.Item.mLocationCoordinate.mX, Args.Item.mLocationCoordinate.mY, Args.Item.mLocationToward);
				UpdateGui_UpdateVehicleTarget(Args.Item.mCurrentTarget);
				UpdateGui_UpdateVehicleVelocity(Args.Item.mVelocity);
				UpdateGui_UpdateVehicleLocationScore(Args.Item.mLocationScore);
				UpdateGui_UpdateVehicleBatteryValue(Args.Item.mBatteryValue);
				UpdateGui_UpdateVehicleAlarmMessage(Args.Item.mAlarmMessage);
				UpdateGui_UpdateVehiclePath(Args.Item.mPathString);
				UpdateGui_UpdateVehicleIpPort(Args.Item.mIpPort);
				UpdateGui_UpdateVehicleMissionId(Args.Item.mCurrentMissionId);
				UpdateGui_UpdateVehicleInterveneCommand(Args.Item.mCurrentInterveneCommand);
				UpdateGui_UpdateVehicleMapName(Args.Item.mCurrentMapName);
				UpdateGui_UpdateVehicleLastUpdateTime(Args.Item.mLastUpdated.ToString(Library.Library.TIME_FORMAT));
			}
		}
		private void btnRefreshVehicleState_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(CurrentVehicleName))
			{
				IVehicleInfo tmpVehicleInfo = rVehicleInfoManager.GetItem(CurrentVehicleName);
				UpdateGui_UpdateVehicleState(tmpVehicleInfo.mCurrentState);
				UpdateGui_UpdateVehicleLocation(tmpVehicleInfo.mLocationCoordinate.mX, tmpVehicleInfo.mLocationCoordinate.mY, tmpVehicleInfo.mLocationToward);
				UpdateGui_UpdateVehicleTarget(tmpVehicleInfo.mCurrentTarget);
				UpdateGui_UpdateVehicleVelocity(tmpVehicleInfo.mVelocity);
				UpdateGui_UpdateVehicleLocationScore(tmpVehicleInfo.mLocationScore);
				UpdateGui_UpdateVehicleBatteryValue(tmpVehicleInfo.mBatteryValue);
				UpdateGui_UpdateVehicleAlarmMessage(tmpVehicleInfo.mAlarmMessage);
				UpdateGui_UpdateVehiclePath(tmpVehicleInfo.mPathString);
				UpdateGui_UpdateVehicleIpPort(tmpVehicleInfo.mIpPort);
				UpdateGui_UpdateVehicleMissionId(tmpVehicleInfo.mCurrentMissionId);
				UpdateGui_UpdateVehicleInterveneCommand(tmpVehicleInfo.mCurrentInterveneCommand);
				UpdateGui_UpdateVehicleMapName(tmpVehicleInfo.mCurrentMapName);
				UpdateGui_UpdateVehicleLastUpdateTime(tmpVehicleInfo.mLastUpdated.ToString(Library.Library.TIME_FORMAT));
			}
		}
		private void cbVehicleNameList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(CurrentVehicleName))
			{
				IVehicleInfo tmpVehicleInfo = rVehicleInfoManager.GetItem(CurrentVehicleName);
				UpdateGui_UpdateVehicleState(tmpVehicleInfo.mCurrentState);
				UpdateGui_UpdateVehicleLocation(tmpVehicleInfo.mLocationCoordinate.mX, tmpVehicleInfo.mLocationCoordinate.mY, tmpVehicleInfo.mLocationToward);
				UpdateGui_UpdateVehicleTarget(tmpVehicleInfo.mCurrentTarget);
				UpdateGui_UpdateVehicleVelocity(tmpVehicleInfo.mVelocity);
				UpdateGui_UpdateVehicleLocationScore(tmpVehicleInfo.mLocationScore);
				UpdateGui_UpdateVehicleBatteryValue(tmpVehicleInfo.mBatteryValue);
				UpdateGui_UpdateVehicleAlarmMessage(tmpVehicleInfo.mAlarmMessage);
				UpdateGui_UpdateVehiclePath(tmpVehicleInfo.mPathString);
				UpdateGui_UpdateVehicleIpPort(tmpVehicleInfo.mIpPort);
				UpdateGui_UpdateVehicleMissionId(tmpVehicleInfo.mCurrentMissionId);
				UpdateGui_UpdateVehicleInterveneCommand(tmpVehicleInfo.mCurrentInterveneCommand);
				UpdateGui_UpdateVehicleMapName(tmpVehicleInfo.mCurrentMapName);
				UpdateGui_UpdateVehicleLastUpdateTime(tmpVehicleInfo.mLastUpdated.ToString(Library.Library.TIME_FORMAT));
			}
		}
		private void InitializeLabelText()
		{
			lblVehicleState.Text = string.Empty;
			lblVehicleLocation.Text = string.Empty;
			lblVehicleTarget.Text = string.Empty;
			lblVehicleVelocity.Text = string.Empty;
			lblVehicleBatteryValue.Text = string.Empty;
			lblVehiclePath.Text = string.Empty;
			lblVehicleLocationScore.Text = string.Empty;
			lblVehicleAlarmMessage.Text = string.Empty;
			lblVehicleIpPort.Text = string.Empty;
			lblVehicleMissionId.Text = string.Empty;
			lblVehicleMapName.Text = string.Empty;
			lblVehicleInterveneCommand.Text = string.Empty;
			lblVehicleLastUpdateTime.Text = string.Empty;
		}
	}
}
