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
	public partial class UCVehicleManualControl : System.Windows.Forms.UserControl
	{
		public UCVehicleManualControl()
		{
			InitializeComponent();
		}
		public void UpdateVehicleNameList(string[] VehicleNameList)
		{
			if (VehicleNameList == null || VehicleNameList.Count() == 0)
			{
				cbVehicleNameList.Items.Clear();
			}
			else
			{
				string lastSelectedItemText = cbVehicleNameList.SelectedItem != null ? cbVehicleNameList.SelectedItem.ToString() : string.Empty;
				cbVehicleNameList.SelectedIndex = -1;
				cbVehicleNameList.Items.Clear();
				cbVehicleNameList.Items.AddRange(VehicleNameList.OrderBy((o) => o).ToArray());
				if (!string.IsNullOrEmpty(lastSelectedItemText))
				{
					for (int i = 0; i < cbVehicleNameList.Items.Count; ++i)
					{
						if (lastSelectedItemText == cbVehicleNameList.Items[i].ToString())
						{
							cbVehicleNameList.SelectedIndex = i;
							break;
						}
					}
				}
			}
		}
		public void UpdateGoalList(string[] GoalList)
		{
			if (GoalList == null || GoalList.Count() == 0)
			{
				lbGoalList.Items.Clear();
			}
			else
			{
				lbGoalList.Items.Clear();
				lbGoalList.Items.AddRange(GoalList.OrderBy((o) => o).ToArray());
			}
		}
	}
}
