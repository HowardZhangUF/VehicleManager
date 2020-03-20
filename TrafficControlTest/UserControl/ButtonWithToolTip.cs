using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficControlTest.UserControl
{
	public class ButtonWithToolTip : Button
	{
		private ToolTip mToolTip = new ToolTip();

		public ButtonWithToolTip() : base()
		{
			mToolTip.SetToolTip(this, string.Empty);
		}

		public void SetToolTipText(string ToolTipText)
		{
			mToolTip.SetToolTip(this, ToolTipText);
		}
	}
}
