using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficControlTest.UserControl
{
	public partial class UCVehicle : System.Windows.Forms.UserControl
	{
		public delegate void EventHandlerString(string VehicleName);
		public event EventHandlerString VehicleStateNeedToBeRefreshed;

		public string CurrentVehicleName { get { return cbVehicleNameList.SelectedItem == null ? string.Empty : cbVehicleNameList.SelectedItem.ToString(); } }

		public UCVehicle()
		{
			InitializeComponent();
			InitializeLabelText();
		}
		public void UpdateVehicleNameList(string[] VehicleNameList)
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
							break;
						}
					}
					InitializeLabelText();
				}
			}
		}
		public void UpdateVehicleState(string State)
		{
			if (lblVehicleState.Text != State) lblVehicleState.Text = State;
		}
		public void UpdateVehicleLocation(int X, int Y, double Toward)
		{
			if (lblVehicleLocation.Text != $"({X}, {Y}, {Toward.ToString("F2")})") lblVehicleLocation.Text = $"({X}, {Y}, {Toward.ToString("F2")})";
		}
		public void UpdateVehicleTarget(string Target)
		{
			if (lblVehicleTarget.Text != Target) lblVehicleTarget.Text = Target;
		}
		public void UpdateVehicleVelocity(double Velocity)
		{
			if (lblVehicleVelocity.Text != $"{Velocity.ToString("F2")} (m/s)") lblVehicleVelocity.Text = $"{Velocity.ToString("F2")} (m/s)";
		}
		public void UpdateVehicleLocationScore(double LocationScore)
		{
			if (lblVehicleLocationScore.Text != $"{LocationScore.ToString("F2")} %") lblVehicleLocationScore.Text = $"{LocationScore.ToString("F2")} %";
		}
		public void UpdateVehicleBatteryValue(double BatteryValue)
		{
			if (lblVehicleBatteryValue.Text != $"{BatteryValue.ToString("F2")} %") lblVehicleBatteryValue.Text = $"{BatteryValue.ToString("F2")} %";
		}
		public void UpdateVehicleAlarmMessage(string AlarmMessage)
		{
			if (lblVehicleAlarmMessage.Text != AlarmMessage) lblVehicleAlarmMessage.Text = AlarmMessage;
		}
		public void UpdateVehiclePath(string Path)
		{
			if (lblVehiclePath.Text != Path) lblVehiclePath.Text = Path;
		}
		public void UpdateVehicleIpPort(string IpPort)
		{
			if (lblVehicleIpPort.Text != IpPort) lblVehicleIpPort.Text = IpPort;
		}
		public void UpdateVehicleMissionId(string MissionId)
		{
			if (lblVehicleMissionId.Text != MissionId) lblVehicleMissionId.Text = MissionId;
		}
		public void UpdateVehicleInterveneCommand(string InterveneCommand)
		{
			if (lblVehicleInterveneCommand.Text != InterveneCommand) lblVehicleInterveneCommand.Text = InterveneCommand;
		}
		public void UpdateVehicleMapName(string MapName)
		{
			if (lblVehicleMapName.Text != MapName) lblVehicleMapName.Text = MapName;
		}
		public void UpdateVehicleLastUpdateTime(string Time)
		{
			if (lblVehicleLastUpdateTime.Text != Time) lblVehicleLastUpdateTime.Text = Time;
		}

		protected virtual void RaiseEvent_VehicleStateNeedToBeRefreshed(string VehicleName, bool Sync = true)
		{
			if (Sync)
			{
				VehicleStateNeedToBeRefreshed?.Invoke(VehicleName);
			}
			else
			{
				Task.Run(() => { VehicleStateNeedToBeRefreshed?.Invoke(VehicleName); });
			}
		}
		private void btnRefreshVehicleState_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(CurrentVehicleName))
			{
				RaiseEvent_VehicleStateNeedToBeRefreshed(CurrentVehicleName);
			}
		}
		private void cbVehicleNameList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(CurrentVehicleName))
			{
				RaiseEvent_VehicleStateNeedToBeRefreshed(CurrentVehicleName);
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
