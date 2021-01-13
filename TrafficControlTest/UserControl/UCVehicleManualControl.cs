using System;
using System.Data;
using System.Linq;
using TrafficControlTest.Library;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.General;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.Vehicle;

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

		private IVehicleControlManager rVehicleControlManager = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IMapManager rMapManager = null;

		public UcVehicleManualControl()
		{
			InitializeComponent();
		}
		public void Set(IVehicleControlManager VehicleControlManager)
		{
			UnsubscribeEvent_IVehicleControlManager(rVehicleControlManager);
			rVehicleControlManager = VehicleControlManager;
			SubscriebEvent_IVehicleControlManager(rVehicleControlManager);
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
		public void Set(IVehicleControlManager VehicleControlManager, IVehicleInfoManager VehicleInfoManager, IMapManager MapManager)
		{
			Set(VehicleControlManager);
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

		private void SubscriebEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
			{

			}
		}
		private void UnsubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
		{
			if (VehicleControlManager != null)
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
				MapManager.LoadMapSuccessed += HandleEvent_MapManagerLoadMapSuccessed;
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.LoadMapSuccessed -= HandleEvent_MapManagerLoadMapSuccessed;
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			UpdateGui_UpdateVehicleNameList(rVehicleInfoManager.GetItemNames().ToArray());
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			UpdateGui_UpdateVehicleNameList(rVehicleInfoManager.GetItemNames().ToArray());
		}
		private void HandleEvent_MapManagerLoadMapSuccessed(object Sender, LoadMapSuccessedEventArgs Args)
		{
			UpdateGui_UpdateGoalList(rMapManager.mTowardPointMapObjects.Select(o => o.mName).ToArray());
		}
		private void btnGoto_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem != null && lbGoalNameList.SelectedItem != null)
				{
					IVehicleControl control = Library.Library.GenerateIVehicleControl(CurrentVehicleName, Command.Goto, new string[] { lbGoalNameList.SelectedItem.ToString() }, "Manual", string.Empty);
					rVehicleControlManager.Add(control.mName, control);
				}
			}
			catch (Exception Ex)
			{
				Library.ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnStop_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem != null)
				{
					IVehicleControl control = Library.Library.GenerateIVehicleControl(CurrentVehicleName, Command.Stop, null, "Manual", string.Empty);
					rVehicleControlManager.Add(control.mName, control);
				}
			}
			catch (Exception Ex)
			{
				Library.ExceptionHandling.HandleException(Ex);
			}
		}
	}
}
