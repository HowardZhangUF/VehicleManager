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

		Dictionary<string, int> VehicleIconIDs = new Dictionary<string, int>();

		public VehicleSimulatorGUI()
		{
			InitializeComponent();

			StyleManager.LoadStyle("Style.ini");
		}

		private void VehicleSimulatorGUI_Load(object sender, EventArgs e)
		{
			SubscribeVehicleSimulatorProcessEvent();

			List<Pair> path1 = new List<Pair>();
			path1.Add(new Pair(-2000, -1000));
			path1.Add(new Pair(3000, -2000));
			path1.Add(new Pair(-2000, 2000));
			path1.Add(new Pair(2000, 1000));
			path1.Add(new Pair(-2000, -1000));
			path1.Add(new Pair(1000, 1000));
			path1.Add(new Pair(2000, 1000));
			path1.Add(new Pair(2000, 2000));
			path1.Add(new Pair(1000, 2000));
			path1.Add(new Pair(1000, 1000));

			List<Pair> path2 = new List<Pair>();
			path2.Add(new Pair(-2000, -1000));
			path2.Add(new Pair(3000, -2000));
			path2.Add(new Pair(-2000, 2000));
			path2.Add(new Pair(2000, 1000));
			path2.Add(new Pair(-2000, -1000));
			path2.Add(new Pair(1000, 1000));
			path2.Add(new Pair(2000, 1000));
			path2.Add(new Pair(2000, 2000));
			path2.Add(new Pair(1000, 2000));
			path2.Add(new Pair(1000, 1000));

			process.AddVehicleSimualtor("AGV01", 1000, 40);
			process.AddVehicleSimualtor("AGV02", 500, 20);
			process.VehicleSimulatorMove("AGV01", path1);
			process.VehicleSimulatorMove("AGV02", path2);
			//process.StartCommunication("127.0.0.1", 8000);
		}

		private void VehicleSimulatorGUI_FormClosing(object sender, FormClosingEventArgs e)
		{
			UnsubscribeVehicleSimulatorProcessEvent();
			//process.StopCommunication();
		}

		private void AddAGVIcon(string name, int x, int y, double toward)
		{
			if (!VehicleIconIDs.Keys.Contains(name))
			{
				VehicleIconIDs.Add(name, GLCMD.CMD.SerialNumber.Next());
			}

			GLCMD.CMD.AddAGV(VehicleIconIDs[name], name, x, y, toward);
		}

		private void RemoveAGVIcon(string name)
		{
			if (!VehicleIconIDs.Keys.Contains(name))
			{
				GLCMD.CMD.DeleteAGV(VehicleIconIDs[name]);
				VehicleIconIDs.Remove(name);
			}
		}

		private void SubscribeVehicleSimulatorProcessEvent()
		{
			process.VehicleSimulatorPositionChanged += Process_VehicleSimulatorPositionChanged;
			process.DebugMessage += Process_DebugMessage;
		}

		private void UnsubscribeVehicleSimulatorProcessEvent()
		{
			process.VehicleSimulatorPositionChanged -= Process_VehicleSimulatorPositionChanged;
			process.DebugMessage -= Process_DebugMessage;
		}

		private void Process_VehicleSimulatorPositionChanged(string name, TowardPair position)
		{
			AddAGVIcon(name, position.Position.X, position.Position.Y, position.Toward.Theta);
		}

		private void Process_DebugMessage(DateTime timeStamp, string category, string message)
		{
		}
	}
}
