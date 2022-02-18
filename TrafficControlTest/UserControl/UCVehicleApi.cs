using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.Vehicle;
using TrafficControlTest.Module.InterveneCommand;
using LibraryForVM;

namespace TrafficControlTest.UserControl
{
	public partial class UcVehicleApi : System.Windows.Forms.UserControl
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

		private IVehicleInfoManager rVehicleInfoManager = null;
		private IVehicleCommunicator rVehicleCommunicator = null;
		private IVehicleControlManager rVehicleControlManager = null;
		private IMapFileManager rMapFileManager = null;
		private IMapManager rMapManager = null;

		public UcVehicleApi()
		{
			InitializeComponent();

			foreach (Control ctrl in tableLayoutPanel1.Controls)
			{
				if (ctrl is Button && ctrl.Name.StartsWith("btnVehicle"))
				{
					(ctrl as Button).Click += btn_Click;
				}
			}

			txtCoordinate1.SetHintText("X,Y or X,Y,Head");
			txtCoordinate1.KeyPress += ((sender, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ',') && (e.KeyChar != '-')) e.Handled = true; });

			btnVehicleGoto.Click += btnVehicleGoto_Click;
			btnVehicleGotoPoint.Click += btnVehicleGotoPoint_Click;
			btnVehicleStop.Click += btnVehicleStop_Click;
			btnVehicleCharge.Click += btnVehicleCharge_Click;
			btnVehicleUncharge.Click += btnVehicleUncharge_Click;
			btnVehiclePause.Click += btnVehiclePause_Click;
			btnVehicleResume.Click += btnVehicleResume_Click;
			btnVehicleRequestMapList.Click += btnVehicleRequestMapList_Click;
			btnVehicleGetMap.Click += btnVehicleGetMap_Click;
			btnVehicleUploadMap.Click += btnVehicleUploadMap_Click;
			btnVehicleChangeMap.Click += btnVehicleChangeMap_Click;
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehcileInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			UnsubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			SubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
		}
		public void Set(IVehicleControlManager VehicleControlManager)
		{
			UnsubscribeEvent_IVehicleControlManager(rVehicleControlManager);
			rVehicleControlManager = VehicleControlManager;
			SubscribeEvent_IVehicleControlManager(rVehicleControlManager);
		}
		public void Set(IMapFileManager MapFileManager)
		{
			UnsubscribeEvent_IMapFileManager(rMapFileManager);
			rMapFileManager = MapFileManager;
			SubscribeEvent_IMapFileManager(rMapFileManager);
		}
		public void Set(IMapManager MapManager)
		{
			UnsubscribeEvent_IMapManager(rMapManager);
			rMapManager = MapManager;
			SubscribeEvent_IMapManager(rMapManager);
		}
		public void Set(IVehicleInfoManager VehicleInfoManager, IVehicleCommunicator VehicleCommunicator, IVehicleControlManager VehicleControlManager, IMapFileManager MapFileManager, IMapManager MapManager)
		{
			Set(VehicleInfoManager);
			Set(VehicleCommunicator);
			Set(VehicleControlManager);
			Set(MapFileManager);
			Set(MapManager);
		}
		public void UpdateVehicleNameList(string[] VehicleNameList)
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
		public void UpdateGoalNameList(string[] GoalNameList)
		{
			this.InvokeIfNecessary(() =>
			{
				if (GoalNameList == null || GoalNameList.Count() == 0)
				{
					cbGoalNameList.Items.Clear();
				}
				else
				{
					cbGoalNameList.Items.Clear();
					cbGoalNameList.Items.AddRange(GoalNameList.OrderBy((o) => o).ToArray());
				}
			});
		}
		public void UpdateRemoteMapNameList(string[] MapNameList)
		{
			this.InvokeIfNecessary(() =>
			{
				if (MapNameList == null || MapNameList.Count() == 0)
				{
					cbRemoteMapNameList1.Items.Clear();
					cbRemoteMapNameList1.AdjustDropDownWidth();
					cbRemoteMapNameList2.Items.Clear();
					cbRemoteMapNameList2.AdjustDropDownWidth();
				}
				else
				{
					cbRemoteMapNameList1.Items.Clear();
					cbRemoteMapNameList1.Items.AddRange(MapNameList.OrderBy((o) => o).ToArray());
					cbRemoteMapNameList1.AdjustDropDownWidth();
					cbRemoteMapNameList2.Items.Clear();
					cbRemoteMapNameList2.Items.AddRange(MapNameList.OrderBy((o) => o).ToArray());
					cbRemoteMapNameList2.AdjustDropDownWidth();
				}
			});
		}
		public void UpdateLocalMapNameList(string[] MapNameList)
		{
			this.InvokeIfNecessary(() =>
			{
				if (MapNameList == null || MapNameList.Count() == 0)
				{
					cbLocalMapNameList.Items.Clear();
					cbLocalMapNameList.AdjustDropDownWidth();
				}
				else
				{
					cbLocalMapNameList.Items.Clear();
					cbLocalMapNameList.Items.AddRange(MapNameList.OrderBy((o) => o).ToArray());
					cbLocalMapNameList.AdjustDropDownWidth();
				}
			});
		}

		private void SubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded += HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved += HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated += HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IVehcileInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded -= HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved -= HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated -= HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{

			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{

			}
		}
		private void SubscribeEvent_IVehicleControlManager(IVehicleControlManager VehicleControlManager)
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
		private void SubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{
				MapFileManager.MapFileAdded += HandleEvent_MapFileManagerMapFileAdded;
			}
		}
		private void UnsubscribeEvent_IMapFileManager(IMapFileManager MapFileManager)
		{
			if (MapFileManager != null)
			{
				MapFileManager.MapFileAdded -= HandleEvent_MapFileManagerMapFileAdded;
			}
		}
		private void SubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapChanged += HandleEvent_MapManagerMapChanged;
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.MapChanged -= HandleEvent_MapManagerMapChanged;
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			UpdateVehicleNameList(rVehicleInfoManager.GetItemNames().ToArray());
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(object Sender, ItemCountChangedEventArgs<IVehicleInfo> Args)
		{
			UpdateVehicleNameList(rVehicleInfoManager.GetItemNames().ToArray());
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(object Sender, ItemUpdatedEventArgs<IVehicleInfo> Args)
		{
			if (!string.IsNullOrEmpty(CurrentVehicleName) && Args.StatusName.Contains("CurrentMapNameList"))
			{
				UpdateRemoteMapNameList(rVehicleInfoManager.GetItem(CurrentVehicleName).mCurrentMapNameList.ToArray());
				UpdateLocalMapNameList(rMapFileManager.GetLocalMapFileFullPathList());
			}
		}
		private void HandleEvent_MapFileManagerMapFileAdded(object Sender, MapFileCountChangedEventArgs Args)
		{
			UpdateLocalMapNameList(rMapFileManager.GetLocalMapFileFullPathList());
		}
		private void HandleEvent_MapManagerMapChanged(object Sender, MapChangedEventArgs Args)
		{
			UpdateGoalNameList(rMapManager.mTowardPointMapObjects.Select(o => o.mName).ToArray());
		}
		private void btn_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem == null)
				{
					CustomMessageBox.OutputBox("You Have to Choose \"Vehicle\" First!");
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnVehicleGoto_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem != null && cbGoalNameList.SelectedItem != null)
				{
					IVehicleControl control = Library.Library.GenerateIVehicleControl(CurrentVehicleName, Command.Goto, new string[] { cbGoalNameList.SelectedItem.ToString() }, "Manual", string.Empty);
					rVehicleControlManager.Add(control.mName, control);
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnVehicleGotoPoint_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem != null && !string.IsNullOrEmpty(txtCoordinate1.Text))
				{
					string[] datas = txtCoordinate1.Text.Split(',');
					if (datas.Length == 2)
					{
						IVehicleControl control = Library.Library.GenerateIVehicleControl(CurrentVehicleName, Command.GotoPoint, datas, "Manual", string.Empty);
						rVehicleControlManager.Add(control.mName, control);
					}
					else if (datas.Length == 3)
					{
						IVehicleControl control = Library.Library.GenerateIVehicleControl(CurrentVehicleName, Command.GotoTowardPoint, datas, "Manual", string.Empty);
						rVehicleControlManager.Add(control.mName, control);
					}
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnVehicleStop_Click(object sender, EventArgs e)
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
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnVehicleCharge_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem != null)
				{
					IVehicleControl control = Library.Library.GenerateIVehicleControl(CurrentVehicleName, Command.Charge, null, "Manual", string.Empty);
					rVehicleControlManager.Add(control.mName, control);
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnVehicleUncharge_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem != null)
				{
					IVehicleControl control = Library.Library.GenerateIVehicleControl(CurrentVehicleName, Command.Uncharge, null, "Manual", string.Empty);
					rVehicleControlManager.Add(control.mName, control);
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnVehiclePause_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem != null)
				{
					IVehicleControl control = Library.Library.GenerateIVehicleControl(CurrentVehicleName, Command.PauseMoving, null, "Manual", string.Empty);
					rVehicleControlManager.Add(control.mName, control);
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnVehicleResume_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem != null)
				{
					IVehicleControl control = Library.Library.GenerateIVehicleControl(CurrentVehicleName, Command.ResumeMoving, null, "Manual", string.Empty);
					rVehicleControlManager.Add(control.mName, control);
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnVehicleStay_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem != null)
				{
					IVehicleControl control = Library.Library.GenerateIVehicleControl(CurrentVehicleName, Command.Stay, null, "Manual", string.Empty);
					rVehicleControlManager.Add(control.mName, control);
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnVehicleUnstay_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem != null)
				{
					IVehicleControl control = Library.Library.GenerateIVehicleControl(CurrentVehicleName, Command.Unstay, null, "Manual", string.Empty);
					rVehicleControlManager.Add(control.mName, control);
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnVehiclePauseControl_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem != null)
				{
					IVehicleControl control = Library.Library.GenerateIVehicleControl(CurrentVehicleName, Command.PauseControl, null, "Manual", string.Empty);
					rVehicleControlManager.Add(control.mName, control);
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnVehicleResumeControl_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem != null)
				{
					IVehicleControl control = Library.Library.GenerateIVehicleControl(CurrentVehicleName, Command.ResumeControl, null, "Manual", string.Empty);
					rVehicleControlManager.Add(control.mName, control);
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnVehicleRequestMapList_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem != null)
				{
					rVehicleCommunicator.SendDataOfRequestMapList(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort);
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnVehicleGetMap_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem != null && cbRemoteMapNameList1.SelectedItem != null)
				{
					rVehicleCommunicator.SendDataOfGetMap(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort, cbRemoteMapNameList1.SelectedItem.ToString().Replace("*", string.Empty));
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnVehicleUploadMap_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem != null && cbLocalMapNameList.SelectedItem != null)
				{
					rVehicleCommunicator.SendDataOfUploadMapToAGV(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort, cbLocalMapNameList.SelectedItem.ToString());
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void btnVehicleChangeMap_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbVehicleNameList.SelectedItem != null && cbRemoteMapNameList2.SelectedItem != null)
				{
					rVehicleCommunicator.SendDataOfChangeMap(rVehicleInfoManager.GetItem(CurrentVehicleName).mIpPort, cbRemoteMapNameList2.SelectedItem.ToString().Replace("*", string.Empty));
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
		private void cbVehicleNameList_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(CurrentVehicleName))
				{
					UpdateRemoteMapNameList(rVehicleInfoManager.GetItem(CurrentVehicleName).mCurrentMapNameList.ToArray());
					UpdateLocalMapNameList(rMapFileManager.GetLocalMapFileFullPathList());
				}
			}
			catch (Exception Ex)
			{
				ExceptionHandling.HandleException(Ex);
			}
		}
	}
}
