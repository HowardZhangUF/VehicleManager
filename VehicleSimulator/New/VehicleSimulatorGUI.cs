using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

		public VehicleSimulatorGUI()
		{
			InitializeComponent();
		}

        private void VehicleSimulatorGUI_Load(object sender, EventArgs e)
        {
            btnMenuOfSimulator_Click(null, null);
        }
        private void btnMinimizeProgram_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
		}
		private void btnCloseProgram_Click(object sender, EventArgs e)
		{
			Close();
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
    }
}
