using Geometry;
using GLCore;
using GLStyle;
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

		VehicleSimulator Vehicle = null;
		int id = 100;

		public VehicleSimulatorGUI()
		{
			InitializeComponent();

			StyleManager.LoadStyle("Style.ini");
			VehicleSimulatorTest();
		}

		private void VehicleSimulatorGUI_Load(object sender, EventArgs e)
		{
			process.StartCommunication();
		}

		private void VehicleSimulatorGUI_FormClosing(object sender, FormClosingEventArgs e)
		{
			process.StopCommunication();
		}

		private void VehicleSimulatorTest()
		{
			Vehicle = new VehicleSimulator("AGV01", 1000, 40);
			Vehicle.PositionChanged += Vehicle_PositionChanged;

			List<Pair> path = new List<Pair>();
			path.Add(new Pair(-2000, -1000));
			path.Add(new Pair(3000, -2000));
			path.Add(new Pair(-2000, 2000));
			path.Add(new Pair(2000, 1000));
			path.Add(new Pair(-2000, -1000));
			path.Add(new Pair(1000, 1000));
			path.Add(new Pair(2000, 1000));
			path.Add(new Pair(2000, 2000));
			path.Add(new Pair(1000, 2000));
			path.Add(new Pair(1000, 1000));
			Thread.Sleep(1000);
			Vehicle.Move(path);
		}

		private void Vehicle_PositionChanged(string name, TowardPair position)
		{
			GLCMD.CMD.AddAGV(id, name, position.Position.X, position.Position.Y, position.Toward.Theta);
		}
	}
}
