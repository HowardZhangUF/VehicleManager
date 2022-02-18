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
using Library;

namespace TrafficControlTest.UserControl
{
	public partial class UcDailyMissionAverageCost : System.Windows.Forms.UserControl
	{
		public DateTime mDate { get; set; } = default(DateTime);
		public int mSuccessedMissionCount { get; set; } = default(int);
		public double mAverageCostInSec { get; set; } = default(double);

		public UcDailyMissionAverageCost()
		{
			InitializeComponent();
		}
		public void Set(DateTime Date, int SuccessedMissionCount, double AverageCostInSec)
		{
			mDate = Date.Date;
			mSuccessedMissionCount = SuccessedMissionCount;
			mAverageCostInSec = AverageCostInSec;
			UpdateGui(Date, SuccessedMissionCount, AverageCostInSec);
		}

		private void UpdateGui(DateTime Date, int SuccessedMissionCount, double AverageCostInSec)
		{
			label1.InvokeIfNecessary(() => label1.Text = Date.ToString("yyyy / MM / dd"));
			lblMissionAverageCost.InvokeIfNecessary(() => lblMissionAverageCost.Text = AverageCostInSec.ToString("F1"));
		}
	}
}
