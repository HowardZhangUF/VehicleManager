using Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VehicleSimulator
{
	public partial class VehicleSimulatorGUI : Form
	{
		private VehicleSimulatorProcess process = new VehicleSimulatorProcess();

		public VehicleSimulatorGUI()
		{
			InitializeComponent();
		}

		private void VehicleSimulatorGUI_Load(object sender, EventArgs e)
		{
			process.StartCommunication();

			VehicleSimulator vehicle = new VehicleSimulator("AGV01", 20, 20);
			List<Pair> Path = new List<Pair>();
			Path.Add(new Pair(100, 100));
			Path.Add(new Pair(200, 100));
			vehicle.SetPath(Path);
			Thread.Sleep(1000);
			vehicle.StartMoving();
		}

		private void VehicleSimulatorGUI_FormClosing(object sender, FormClosingEventArgs e)
		{
			process.StopCommunication();
		}
	}
}
