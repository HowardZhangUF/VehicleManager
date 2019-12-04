using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrafficControlTest.Interface;
using TrafficControlTest.Module.General.Interface;

namespace TrafficControlTest.UserControl
{
	public partial class UcVehicleManualControl : System.Windows.Forms.UserControl
	{
		public string CurrentVehicleName
		{
			get
			{
				string result = null;
				cbVehicleNameList.InvokeIfNecessary(() =>
				{
					result = cbVehicleNameList.SelectedItem == null ? string.Empty : cbVehicleNameList.SelectedItem.ToString();
				});
				return result;
			}
		}

		private IVehicleCommunicator rVehicleCommunicator = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IMapManager rMapManager = null;

		public UcVehicleManualControl()
		{
			InitializeComponent();
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			UnsubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			SubscriebEvent_IVehicleCommunicator(rVehicleCommunicator);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(IMapManager MapManager)
		{
			UnsubscribeEvent_IMapManager(rMapManager);
			rMapManager = MapManager;
			SubscribeEvent_IMapManager(rMapManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator, IVehicleInfoManager VehicleInfoManager, IMapManager MapManager)
		{
			Set(VehicleCommunicator);
			Set(VehicleInfoManager);
			Set(MapManager);
		}
		public void UpdateGui_UpdateVehicleNameList(string[] VehicleNameList)
		{
			this.InvokeIfNecessary(() =>
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
			});
		}
		public void UpdateGui_UpdateGoalList(string[] GoalList)
		{
			this.InvokeIfNecessary(() =>
			{
				if (GoalList == null || GoalList.Count() == 0)
				{
					lbGoalNameList.Items.Clear();
				}
				else
				{
					lbGoalNameList.Items.Clear();
					lbGoalNameList.Items.AddRange(GoalList.OrderBy((o) => o).ToArray());
				}
			});
		}

		private void SubscriebEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (rVehicleCommunicator != null)
			{

			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (rVehicleCommunicator != null)
			{

			}
		}
		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded += HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved += HandleEvent_VehicleInfoManagerItemRemoved;
			}
		}
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded -= HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved -= HandleEvent_VehicleInfoManagerItemRemoved;
			}
		}
		private void SubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapLoaded += HandleEvent_MapManagerMapLoaded;
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapLoaded -= HandleEvent_MapManagerMapLoaded;
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(DateTime OccurTime, string Name, IVehicleInfo Item)
		{
			UpdateGui_UpdateVehicleNameList(rVehicleInfoManager.GetItemNames().ToArray());
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(DateTime OccurTime, string Name, IVehicleInfo Item)
		{
			UpdateGui_UpdateVehicleNameList(rVehicleInfoManager.GetItemNames().ToArray());
		}
		private void HandleEvent_MapManagerMapLoaded(DateTime OccurTime, string MapFileName)
		{
			UpdateGui_UpdateGoalList(rMapManager.GetGoalNameList());
		}
		private void btnGoto_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null && lbGoalNameList.SelectedItem != null)
			{
				rVehicleCommunicator.SendSerializableData_Goto(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort, lbGoalNameList.SelectedItem.ToString());
			}
		}
		private void btnStop_Click(object sender, EventArgs e)
		{
			if (cbVehicleNameList.SelectedItem != null)
			{
				rVehicleCommunicator.SendSerializableData_Stop(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort);
			}
		}
	}
}
