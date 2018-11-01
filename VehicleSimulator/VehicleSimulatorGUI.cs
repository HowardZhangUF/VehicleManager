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

		VehicleSimulator vehicle = null;
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
			vehicle = new VehicleSimulator("AGV01", 500, 40);
			vehicle.PositionChanged += Vehicle_PositionChanged;

			List<Pair> Path = new List<Pair>();
			Path.Add(new Pair(-2000, -1000));
			Path.Add(new Pair(3000, -1000));
			Path.Add(new Pair(-2000, 1000));
			Path.Add(new Pair(2000, 1000));
			Path.Add(new Pair(-2000, -1000));
			Path.Add(new Pair(1000, 1000));
			Path.Add(new Pair(2000, 1000));
			Path.Add(new Pair(2000, 2000));
			Path.Add(new Pair(1000, 2000));
			Path.Add(new Pair(1000, 1000));
			vehicle.SetPath(Path);
			Thread.Sleep(1000);
			vehicle.StartMoving();
		}

		private void Vehicle_PositionChanged(string name, TowardPair position)
		{
			GLCMD.CMD.AddAGV(id, name, position.Position.X, position.Position.Y, position.Toward.Theta);
		}
	}
}
