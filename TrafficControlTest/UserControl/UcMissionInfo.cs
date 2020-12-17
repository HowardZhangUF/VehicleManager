using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrafficControlTest.Module.Mission;

namespace TrafficControlTest.UserControl
{
	public partial class UcMissionInfo : System.Windows.Forms.UserControl
	{
        public enum MissionProperty
        {
            State,
            ExecutorId
        }

        public static int DefaultHeight = 60;
        public static string Prefix = "UcMissionInfo";

        public string mId
        {
            get { return Name.Replace(Prefix, string.Empty); }
            set { if (Name != Prefix + value) Name = Prefix + value; }
        }
        public string mType
        {
            get { return lblMissionType.Text; }
            set { if (lblMissionType.Text != value) lblMissionType.Text = value; }
        }
        public string mParameter
        {
            get { return lblMissionParameter.Text; }
            set { if (lblMissionParameter.Text != value) lblMissionParameter.Text = value; }
        }
        public string mState
        {
            get { return lblMissionState.Text; }
            set { lblMissionState.Text = (value == "Unexecute") ? "Unexecute" : $"{value} by {mExecutorId}"; SetBorderColor((value == "Unexecute") ? Color.FromArgb(147, 147, 147) : Color.Yellow); SetTextColor((value == "Unexecute") ? Color.FromArgb(147, 147, 147) : Color.Yellow); }
        }
        public string mExecutorId { get; set; }
        public int mPriority { get; set; }
        public DateTime mReceivedTimestamp { get; set; }

		public UcMissionInfo()
		{
			InitializeComponent();
		}
		public override string ToString()
		{
			return $"{mId}/{mType}/{mParameter}/{mState}/{mExecutorId}/{mPriority.ToString()}/{mReceivedTimestamp.ToString("yyyy/MM/dd HH:mm:ss.fff")}";
		}

		private void SetBorderColor(Color BorderColor)
        {
            if (pnlBorder.BackColor != BorderColor)
            {
                pnlBorder.BackColor = BorderColor;
                pnlSplit1.BackColor = BorderColor;
				pnlSplit2.BackColor = BorderColor;
			}
        }
		private void SetTextColor(Color TextColor)
		{
			if (lblMissionType.ForeColor != TextColor)
			{
				lblMissionType.ForeColor = TextColor;
				lblMissionParameter.ForeColor = TextColor;
				lblMissionState.ForeColor = TextColor;
			}
		}
    }
}
