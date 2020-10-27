using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrafficControlTest.Library;

namespace TrafficControlTest.UserControl
{
	public partial class formProgress : Form
	{
		public formProgress()
		{
			InitializeComponent();
		}
		public new void Show()
		{
			base.Show();
			WindowState = FormWindowState.Normal;
			BringToFront();
			TopMost = true;
		}
		public new void Close()
		{
			WindowState = FormWindowState.Minimized;
			base.Close();
		}
		public void SetTitleText(string TitleText)
		{
			lblTitle.InvokeIfNecessary(() =>
			{
				lblTitle.Text = TitleText;
			});
		}
		public void SetProgressValue(int ProgressValue)
		{
			lblProgressValue.InvokeIfNecessary(() =>
			{
				if (progressBar1.Value != ProgressValue)
				{
					progressBar1.Value = ProgressValue;
					lblProgressValue.Text = $"{ProgressValue.ToString()} %";
				}
			});
		}
	}
}
