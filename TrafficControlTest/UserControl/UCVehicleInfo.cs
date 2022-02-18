using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryForVM;

namespace TrafficControlTest.UserControl
{
	public partial class UcVehicleInfo : System.Windows.Forms.UserControl
	{
		public event EventHandler DoubleClickOnControl;

		public enum Property
		{
			Id,
			Battery,
			LocationScore,
			State,
			Target
		}

		public static int DefaultHeight = 80;
		public static string PreFix = "UcVehicleInfo";

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
		public string mLocationScore
		{
			get { return lblLocationScore.Text.Replace(" %", string.Empty); }
			set { if (lblLocationScore.Text.Replace(" %", string.Empty) != value) { lblLocationScore.Text = value + " %"; } }
		}
		public string mState
		{
			get { return lblState.Text; }
			set { if (lblState.Text != value) { lblState.Text = value; } }
		}
		public string mTarget
		{
			get { return lblTarget.Text; }
			set { if (lblTarget.Text != value) { lblTarget.Text = value; } }
		}
		public Color mBorderColor
		{
			get { return pnlBorder.BackColor; }
			set { pnlBorder.BackColor = value; }
		}

		public UcVehicleInfo()
		{
			InitializeComponent();
		}
		public override string ToString()
		{
			return string.IsNullOrEmpty(mTarget) ? $"{mId}/{mBattery}/{mLocationScore}/{mState}" : $"{mId}/{mBattery}/{mLocationScore}/{mState}/{mTarget}";
		}

		protected virtual void Control_DoubleClick(object Sender, EventArgs E)
		{
			try
			{
				DoubleClickOnControl?.Invoke(this, E);
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
	}
}
