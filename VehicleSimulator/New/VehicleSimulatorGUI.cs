using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrafficControlTest.Process;

namespace VehicleSimulator.New
{
    public enum Language
    {
        EnUs,
        ZhTw
    }

	public partial class VehicleSimulatorGUI : Form
	{
        private Color mColorOfMenuSelected = Color.FromArgb(41, 41, 41);
        private Color mColorOfMenuUnselected = Color.FromArgb(28, 28, 28);

		private SimulatorProcessContainer mCore = new SimulatorProcessContainer();

		public VehicleSimulatorGUI()
		{
			InitializeComponent();
		}

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr a, int msg, int wParam, int lParam);
		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		protected void Constructor()
		{
			UnsubscribeEvent_SimulatorProcessContainer(mCore);
			mCore = new SimulatorProcessContainer();
			SubscribeEvent_SimulatorProcessContainer(mCore);

			ucContentOfSimulator1.Set(mCore);
			ucContentOfSetting1.Set(mCore);
			btnMenuOfSimulator_Click(null, null);
		}
		protected void Destructor()
		{
			mCore.Clear();
			UnsubscribeEvent_SimulatorProcessContainer(mCore);
			mCore = null;
		}

        private void VehicleSimulatorGUI_Load(object sender, EventArgs e)
        {
			Constructor();
		}
		private void VehicleSimulatorGUI_FormClosing(object sender, FormClosingEventArgs e)
		{

		}
		private void ctrlTitle_MouseDown(object sender, MouseEventArgs e)
		{
			bool flag = e.Button == MouseButtons.Left;
			if (flag)
			{
				ReleaseCapture();
				SendMessage(Handle, 0x112, 0xf012, 0);
			}
		}
		private void btnMinimizeProgram_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
		}
		private void btnCloseProgram_Click(object sender, EventArgs e)
		{
			Destructor();
			Task.Run(() => 
			{
				TrafficControlTest.UserControl.formProgress frm = new TrafficControlTest.UserControl.formProgress();
				frm.StartPosition = FormStartPosition.CenterParent;
				frm.SetTitleText("Program Closing ...");
				frm.Show();
				Application.DoEvents();
				int i = 0;
				while (i <= 20)
				{
					frm.SetProgressValue(i * 5);
					Application.DoEvents();
					System.Threading.Thread.Sleep(100);
					i++;
				}
				frm.Close();
				pnlTitle.InvokeIfNecessary(() => { Close(); });
			});
		}
        private void btnMenuOfSimulator_Click(object sender, EventArgs e)
        {
            btnMenuOfSimulator.BackColor = mColorOfMenuSelected;
            btnMenuOfConsole.BackColor = mColorOfMenuUnselected;
            btnMenuOfSetting.BackColor = mColorOfMenuUnselected;
            btnMenuOfAbout.BackColor = mColorOfMenuUnselected;
            ucContentOfSimulator1.BringToFront();
        }
        private void btnMenuOfConsole_Click(object sender, EventArgs e)
        {
            btnMenuOfSimulator.BackColor = mColorOfMenuUnselected;
            btnMenuOfConsole.BackColor = mColorOfMenuSelected;
            btnMenuOfSetting.BackColor = mColorOfMenuUnselected;
            btnMenuOfAbout.BackColor = mColorOfMenuUnselected;
            ucContentOfConsole1.BringToFront();
        }
        private void btnMenuOfSetting_Click(object sender, EventArgs e)
        {
            btnMenuOfSimulator.BackColor = mColorOfMenuUnselected;
            btnMenuOfConsole.BackColor = mColorOfMenuUnselected;
            btnMenuOfSetting.BackColor = mColorOfMenuSelected;
            btnMenuOfAbout.BackColor = mColorOfMenuUnselected;
            ucContentOfSetting1.BringToFront();
        }
        private void btnMenuOfAbout_Click(object sender, EventArgs e)
        {
            btnMenuOfSimulator.BackColor = mColorOfMenuUnselected;
            btnMenuOfConsole.BackColor = mColorOfMenuUnselected;
            btnMenuOfSetting.BackColor = mColorOfMenuUnselected;
            btnMenuOfAbout.BackColor = mColorOfMenuSelected;
            ucContentOfAbout1.BringToFront();
		}
		private void SubscribeEvent_SimulatorProcessContainer(SimulatorProcessContainer SimulatorProcessContainer)
		{
			if (SimulatorProcessContainer != null)
			{
				SimulatorProcessContainer.SimulatorAdded += HandleEvent_SimulatorProcessContainerSimulatorAdded;
				SimulatorProcessContainer.SimulatorRemoved += HandleEvent_SimulatorProcessContainerSimulatorRemoved;
			}
		}
		private void UnsubscribeEvent_SimulatorProcessContainer(SimulatorProcessContainer SimulatorProcessContainer)
		{
			if (SimulatorProcessContainer != null)
			{
				SimulatorProcessContainer.SimulatorAdded -= HandleEvent_SimulatorProcessContainerSimulatorAdded;
				SimulatorProcessContainer.SimulatorRemoved -= HandleEvent_SimulatorProcessContainerSimulatorRemoved;
			}
		}
		private void SubscribeEvent_SimulatorProcess(SimulatorProcess SimulatorProcess)
		{
			if (SimulatorProcess != null)
			{
				SimulatorProcess.DebugMessage += HandleEvent_SimulatorProcessDebugMessage;
			}
		}
		private void UnsubscribeEvent_SimulatorProcess(SimulatorProcess SimulatorProcess)
		{
			if (SimulatorProcess != null)
			{
				SimulatorProcess.DebugMessage -= HandleEvent_SimulatorProcessDebugMessage;
			}
		}
		private void HandleEvent_SimulatorProcessContainerSimulatorAdded(object sender, SimulatorAddedEventArgs e)
		{
			SubscribeEvent_SimulatorProcess(e.SimulatorProcess);
		}
		private void HandleEvent_SimulatorProcessContainerSimulatorRemoved(object sender, SimulatorRemovedEventArgs e)
		{
			UnsubscribeEvent_SimulatorProcess(e.SimulatorProcess);
		}
		private void HandleEvent_SimulatorProcessDebugMessage(object sender, DebugMessageEventArgs e)
		{
			//
		}
	}
}
