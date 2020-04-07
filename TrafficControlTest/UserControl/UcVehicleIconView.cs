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
	public partial class UcVehicleIconView : System.Windows.Forms.UserControl
	{
		public enum Property
		{
			Id,
			State,
			Target,
			Battery,
			LocationScore
		}

		public static int DefaultWidth = 50;
		public static string PreFix = "UcVehicleInfo";

		public string mId
		{
			get { return _Id; }
			set { if (_Id != value) { _Id = value; Name = PreFix + value; UpdateToolTip(); } }
		}
		public string mState
		{
			get { return _State; }
			set { if (_State != value) { _State = value; UpdateToolTip(); } }
		}
		public string mTarget
		{
			get { return _Target; }
			set { if (_Target != value) { _Target = value; UpdateToolTip(); } }
		}
		public string mBattery
		{
			get { return _Battery; }
			set { if (_Battery != value) { _Battery = value; UpdateToolTip(); } }
		}
		public string mLocationScore
		{
			get { return _LocationScore; }
			set { if (_LocationScore != value) { _LocationScore = value; UpdateToolTip(); } }
		}
		public Color mBorderColor
		{
			get { return pnlTop.BackColor; }
			set { pnlSecondTop.BackColor = value; pnlSecondBottom.BackColor = value; pnlSecondLeft.BackColor = value; pnlSecondRight.BackColor = value; }
		}

		private string _Id = string.Empty;
		private string _State = string.Empty;
		private string _Target = string.Empty;
		private string _Battery = string.Empty;
		private string _LocationScore = string.Empty;
		private ToolTip mToolTip = new ToolTip();

		public UcVehicleIconView()
		{
			InitializeComponent();
		}

		private void UpdateToolTip()
		{
			string text = string.Empty;
			text += $"ID: {mId}\n";
			text += $"State: {mState}\n";
			text += $"Target: {mTarget}\n";
			text += $"Battery: {mBattery} %\n";
			text += $"LocationScore: {mLocationScore} %";
			mToolTip.SetToolTip(lblVehiclePic, text);
		}
	}
}
