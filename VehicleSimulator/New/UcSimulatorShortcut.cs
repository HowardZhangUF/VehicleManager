using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.NewCommunication;

namespace VehicleSimulator.New
{
	public partial class UcSimulatorShortcut : UserControl
	{
		private SimulatorProcess rSimulatorProcess = null;
		private ISimulatorInfo rSimulatorInfo = null;
		private ISimulatorControl rSimulatorControl = null;
		private IHostCommunicator rHostCommunicator = null;

		public UcSimulatorShortcut()
		{
			InitializeComponent();
		}
		public UcSimulatorShortcut(SimulatorProcess SimulatorProcess)
		{
			InitializeComponent();
			Set(SimulatorProcess);
		}
		public void Set(SimulatorProcess SimulatorProcess)
		{
			UnsubscribeEvent_SimulatorProcess(rSimulatorProcess);
			rSimulatorProcess = SimulatorProcess;
			SubscribeEvent_SimulatorProcess(rSimulatorProcess);
			UpdateGui_UpdateSimulatorInfo();
		}
		public void Set(ISimulatorInfo SimulatorInfo)
		{
			UnsubscribeEvent_ISimulatorInfo(rSimulatorInfo);
			rSimulatorInfo = SimulatorInfo;
			SubscribeEvent_ISimulatorInfo(rSimulatorInfo);
		}
		public void Set(ISimulatorControl SimulatorControl)
		{
			UnsubscribeEvent_ISimulatorControl(rSimulatorControl);
			rSimulatorControl = SimulatorControl;
			SubscribeEvent_ISimulatorControl(rSimulatorControl);
		}
		public void Set(IHostCommunicator HostCommunicator)
		{
			UnsubscribeEvent_IHostCommunicator(rHostCommunicator);
			rHostCommunicator = HostCommunicator;
			SubscribeEvent_IHostCommunicator(rHostCommunicator);
		}
		public string GetCurrentSimulatorName()
		{
			return lblSimulatorName.Text;
		}
		public void SetBackColor(Color Color)
		{
			tableLayoutPanel1.InvokeIfNecessary(() => { tableLayoutPanel1.BackColor = Color; });
		}

		private void SubscribeEvent_SimulatorProcess(SimulatorProcess rSimulatorProcess)
		{
			if (rSimulatorProcess != null)
			{
				Set(rSimulatorProcess.GetReferenceOfISimulatorInfo());
				Set(rSimulatorProcess.GetReferenceOfISimulatorControl());
				Set(rSimulatorProcess.GetReferenceOfIHostCommunicator());
			}
		}
		private void UnsubscribeEvent_SimulatorProcess(SimulatorProcess rSimulatorProcess)
		{
			if (rSimulatorProcess != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_ISimulatorInfo(ISimulatorInfo SimulatorInfo)
		{
			if (SimulatorInfo != null)
			{
				SimulatorInfo.StatusUpdated += HandleEvent_SimulatorInfoStatusUpdated;
			}
		}
		private void UnsubscribeEvent_ISimulatorInfo(ISimulatorInfo SimulatorInfo)
		{
			if (SimulatorInfo != null)
			{
				SimulatorInfo.StatusUpdated -= HandleEvent_SimulatorInfoStatusUpdated;
			}
		}
		private void SubscribeEvent_ISimulatorControl(ISimulatorControl SimulatorControl)
		{
			if (SimulatorControl != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_ISimulatorControl(ISimulatorControl SimulatorControl)
		{
			if (SimulatorControl != null)
			{
				// do nothing
			}
		}
		private void SubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.ConnectStateChanged += HandleEvent_HostCommunicatorConnectStateChanged;
			}
		}
		private void UnsubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.ConnectStateChanged -= HandleEvent_HostCommunicatorConnectStateChanged;
			}
		}
		private void HandleEvent_SimulatorInfoStatusUpdated(object sender, StatusUpdatedEventArgs e)
		{
			if (e.StatusName.Contains("Status") || e.StatusName.Contains("Target"))
			{
				UpdateGui_SetSimulatorStatusAndTarget(rSimulatorInfo.mStatus.ToString(), rSimulatorInfo.mTarget);
			}
			else if (e.StatusName.Contains("X") || e.StatusName.Contains("Y") || e.StatusName.Contains("Toward"))
			{
				UpdateGui_SetSimulatorLocation(rSimulatorInfo.mX, rSimulatorInfo.mY, rSimulatorInfo.mToward);
			}
		}
		private void HandleEvent_HostCommunicatorConnectStateChanged(object sender, ConnectStateChangedEventArgs e)
		{
			UpdateGui_SetSimulatorIsConnect(rHostCommunicator.mIsConnected);
		}
		private void UpdateGui_UpdateSimulatorInfo()
		{
			if (rSimulatorProcess != null && rHostCommunicator != null)
			{
				UpdateGui_SetSimulatorName(rSimulatorInfo.mName);
				UpdateGui_SetSimulatorStatusAndTarget(rSimulatorInfo.mStatus.ToString(), rSimulatorInfo.mTarget);
				UpdateGui_SetSimulatorLocation(rSimulatorInfo.mX, rSimulatorInfo.mY, rSimulatorInfo.mToward);
				UpdateGui_SetSimulatorIsConnect(rHostCommunicator.mIsConnected);
			}
			else
			{
				UpdateGui_SetSimulatorName(string.Empty);
				UpdateGui_SetSimulatorStatusAndTarget(string.Empty, string.Empty);
				UpdateGui_SetSimulatorLocation(default(int), default(int), default(int));
				UpdateGui_SetSimulatorIsConnect(default(bool));
			}
		}
		private void UpdateGui_SetSimulatorName(string SimulatorName)
		{
			lblSimulatorName.InvokeIfNecessary(() => { lblSimulatorName.Text = SimulatorName; });
		}
		private void UpdateGui_SetSimulatorStatusAndTarget(string SimulatorStatus, string SimulatorTarget)
		{
			string newText = string.IsNullOrEmpty(SimulatorTarget) ? $"{SimulatorStatus}" : $"{SimulatorStatus} / {SimulatorTarget}";
			lblSimulatorStatus.InvokeIfNecessary(() => { lblSimulatorStatus.Text = newText; });

			if (!string.IsNullOrEmpty(SimulatorStatus) && rSimulatorInfo != null)
			{
				Color color = Color.Red;
				switch (rSimulatorInfo.mStatus)
				{
					case New.ESimulatorStatus.Idle:
					case New.ESimulatorStatus.ChargingButFree:
						color = Color.FromArgb(0, 128, 0);
						break;
					case New.ESimulatorStatus.Working:
					case New.ESimulatorStatus.Paused:
					case New.ESimulatorStatus.Charging:
						color = Color.FromArgb(128, 128, 0);
						break;
					default:
						color = Color.FromArgb(128, 0, 0);
						break;
				}
				lblSimulatorStatus.InvokeIfNecessary(() => { lblSimulatorStatus.BackColor = color; });
			}
		}
		private void UpdateGui_SetSimulatorLocation(int X, int Y, int Toward)
		{
			lblSimulatorLocation.InvokeIfNecessary(() => { lblSimulatorLocation.Text = $"({X},{Y},{Toward})"; });
		}
		private void UpdateGui_SetSimulatorIsConnect(bool Value)
		{
			Color color = Value ? Color.White : Color.DimGray;
			tableLayoutPanel1.InvokeIfNecessary(() => { lblSimulatorName.ForeColor = color; lblSimulatorStatus.ForeColor = color; lblSimulatorLocation.ForeColor = color; });
		}

		private void Control_Click(object sender, EventArgs e)
		{
			OnClick(e);
		}
	}
}
