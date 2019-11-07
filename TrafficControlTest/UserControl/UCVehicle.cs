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
		public void UpdateVehicleVelocity(double Velocity)
		{
			if (lblVehicleVelocity.Text != $"{Velocity.ToString("F2")} (m/s)") lblVehicleVelocity.Text = $"{Velocity.ToString("F2")} (m/s)";
		}
		public void UpdateVehiclePosition(int X, int Y)
		{
			if (lblVehiclePosition.Text != $"({X}, {Y})") lblVehiclePosition.Text = $"({X}, {Y})";
		}
		public void UpdateVehicleToward(double Toward)
		{
			if (lblVehicleToward.Text != $"{Toward.ToString("F2")} deg") lblVehicleToward.Text = $"{Toward.ToString("F2")} deg";
		}
		public void UpdateVehicleTarget(string Target)
		{
			if (lblVehicleTarget.Text != Target) lblVehicleTarget.Text = Target;
		}
		public void UpdateVehiclePath(string Path)
		{
			if (lblVehiclePath.Text != Path) lblVehiclePath.Text = Path;
		}
		public void UpdateVehicleMatch(double Match)
		{
			if (lblVehicleMatch.Text != $"{Match.ToString("F2")} %") lblVehicleMatch.Text = $"{Match.ToString("F2")} %";
		}
		public void UpdateVehicleBattery(double Battery)
		{
			if (lblVehicleBattery.Text != $"{Battery.ToString("F2")} %") lblVehicleBattery.Text = $"{Battery.ToString("F2")} %";
		}
		public void UpdateVehicleIntervenable(bool Intervenable)
		{
			if (lblVehicleIntervenable.Text != Intervenable.ToString()) lblVehicleIntervenable.Text = Intervenable.ToString();
		}
		public void UpdateVehicleInterveneCommand(string InterveneCommand)
		{
			if (lblVehicleInterveneCommand.Text != InterveneCommand) lblVehicleInterveneCommand.Text = InterveneCommand;
		}
		public void UpdateVehicleIntervening(bool Intervening)
		{
			if (lblVehicleIntervening.Text != Intervening.ToString()) lblVehicleIntervening.Text = Intervening.ToString();
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
			lblVehicleVelocity.Text = string.Empty;
			lblVehiclePosition.Text = string.Empty;
			lblVehicleToward.Text = string.Empty;
			lblVehicleTarget.Text = string.Empty;
			lblVehiclePath.Text = string.Empty;
			lblVehicleMatch.Text = string.Empty;
			lblVehicleBattery.Text = string.Empty;
			lblVehicleIntervenable.Text = string.Empty;
			lblVehicleInterveneCommand.Text = string.Empty;
			lblVehicleIntervening.Text = string.Empty;
			lblVehicleLastUpdateTime.Text = string.Empty;
		}
	}
}
