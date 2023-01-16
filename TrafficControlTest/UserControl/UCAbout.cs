using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrafficControlTest.Library;
using LibraryForVM;

namespace TrafficControlTest.UserControl
{
	public partial class UcAbout : System.Windows.Forms.UserControl
	{
		public UcAbout()
		{
			InitializeComponent();
		}
		public void Set(string Name, string Version, ProjectType ProjectType, string CopyRight)
		{
			this.InvokeIfNecessary(() =>
			{
				lblProgramName.Text = Name;
				lblProgramVersion.Text = Version;
				lblProgramProjectType.Text = ProjectType.ToString();
				lblProgramCopyRight.Text = CopyRight;
			});
		}

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
