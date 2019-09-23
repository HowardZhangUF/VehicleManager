using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficControlTest.UserControl
{
	public partial class UCVehicleInfo : System.Windows.Forms.UserControl
	{
		public enum Property
		{
			Id,
			Battery,
			State
		}

		public static int DefaultHeight = 100;
		public static string PreFix = "UCVehicleInfo";

		public string mId
		{
			get { return lblId.Text; }
			set { if (lblId.Text != value) { lblId.Text = value; Name = PreFix + value; } }
		}
		public string mBattery
		{
			get { return lblBattery.Text.Replace(" %", string.Empty); }
			set { if (lblBattery.Text.Replace(" %", string.Empty) != value) { lblBattery.Text = value + " %"; } }
		}
		public string mState
		{
			get { return lblState.Text; }
			set { if (lblState.Text != value) { lblState.Text = value; } }
		}
		public Color mBorderColor
		{
			get { return pnlTop.BackColor; }
			set { pnlSecondTop.BackColor = value; pnlSecondBottom.BackColor = value; pnlSecondLeft.BackColor = value; pnlSecondRight.BackColor = value; }
		}

		public UCVehicleInfo()
		{
			InitializeComponent();
		}
		public override string ToString()
		{
			return $"{mId}/{mBattery}/{mState}";
		}
	}
}
