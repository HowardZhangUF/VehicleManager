using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VehicleSimulator.UserInterface
{
	public partial class VehicleSimulatorGUI : Form
	{
		public VehicleSimulatorGUI()
		{
			InitializeComponent();
			Constructor();
		}
		private void Constructor()
		{
			Constructor_VehicleManagerProcess();
		}
		private void Destructor()
		{
			Destructor_VehicleManagerProcess();
		}
		private void HandleException(Exception Ex)
		{
			Console.WriteLine(Ex.ToString());
		}
		private void VehicleSimulatorGUI_Load(object sender, EventArgs e)
		{
			try
			{

			}
			catch (Exception Ex)
			{
				HandleException(Ex);
			}
		}
		private void VehicleSimulatorGUI_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				Destructor();
			}
			catch (Exception Ex)
			{
				HandleException(Ex);
			}
		}

		private Base.VehicleSimulatorProcess mCore;

		private void Constructor_VehicleManagerProcess()
		{
			mCore = new Base.VehicleSimulatorProcess();
			mCore.CommunicatorClientStartConnect("127.0.0.1", 8000);
			//SubscribeEvent_VehicleSimulatorProcess(mCore);
		}
		private void Destructor_VehicleManagerProcess()
		{
			//UnsubscribeEvent_VehicleSimulatorProcess(mCore);
			mCore = null;
		}
	}
}
