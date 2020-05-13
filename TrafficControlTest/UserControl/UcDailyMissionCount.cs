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

namespace TrafficControlTest.UserControl
{
	public partial class UcDailyMissionCount : System.Windows.Forms.UserControl
	{
		public DateTime mDate { get; set; } = default(DateTime);
		public int mSuccessedCount { get; set; } = default(int);
		public int mFailedCount { get; set; } = default(int);
		public int mTotalCount { get { return mSuccessedCount + mFailedCount; } }

		public UcDailyMissionCount()
		{
			InitializeComponent();
		}
		public void Set(DateTime Date, int SuccessedCount, int FailedCount)
		{
			mDate = Date.Date;
			mSuccessedCount = SuccessedCount;
			mFailedCount = FailedCount;
			UpdateGui(Date, SuccessedCount, FailedCount);
		}
		
		private void UpdateGui(DateTime Date, int SuccessedCount, int FailedCount)
		{
			label1.InvokeIfNecessary(() => label1.Text = Date.ToString("yyyy / MM / dd"));
			lblMissionCount.InvokeIfNecessary(() => lblMissionCount.Text = mTotalCount.ToString());
		}
	}
}
