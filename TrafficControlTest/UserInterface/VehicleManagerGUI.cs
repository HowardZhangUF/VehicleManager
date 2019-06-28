using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrafficControlTest.Base;

namespace TrafficControlTest.UserInterface
{
	public partial class VehicleManagerGUI : Form
	{
		VehicleManagerProcess mCore = new VehicleManagerProcess();

		public VehicleManagerGUI()
		{
			InitializeComponent();

			try
			{
				mCore.VehicleCommunicatorStartListen(8000);
				mCore.CollisionEventDetectorStart();
			}
			catch (Exception Ex)
			{
				HandleException(Ex);
			}
		}
		private void HandleException(Exception Ex)
		{
			Console.WriteLine(Ex.ToString());
		}
		private void VehicleManagerGUI_Load(object sender, EventArgs e)
		{
			try
			{

			}
			catch (Exception Ex)
			{
				HandleException(Ex);
			}
		}
	}
}
