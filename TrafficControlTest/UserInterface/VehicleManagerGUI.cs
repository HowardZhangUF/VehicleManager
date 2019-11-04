using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TrafficControlTest.Base;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;
using TrafficControlTest.Module.MissionManager.Interface;
using TrafficControlTest.UserControl;
using static TrafficControlTest.UserControl.UCVehicleInfo;

namespace TrafficControlTest.UserInterface
{
	public partial class VehicleManagerGUI : Form
	{
		private bool pnlLeftMainDisplay = true;
		private bool pnlBtmDisplay = true;
		private int pnlLeftMainDefaultWidth = 400;
		private int pnlBtmDefaultHeight = 250;

		public VehicleManagerGUI()
		{
			InitializeComponent();

			try
			{

			}
			catch (Exception Ex)
			{
				HandleException(Ex);
			}
		}
		private void Constructor()
		{
			ucMap1.Constructor("Style.ini");
			Constructor_VehicleManagerProcess();
		}
		private void Destructor()
		{
			Destructor_VehicleManagerProcess();
			ucMap1.Destructor();
		}
		private void HandleException(Exception Ex)
		{
			Console.WriteLine(Ex.ToString());
		}
		private void VehicleManagerGUI_Load(object sender, EventArgs e)
		{
			try
			{
				Constructor();
				btnDisplayVehicleOverview_Click(null, null);
				btnDisplayMap_Click(null, null);
				btnDisplayPnlLeftMain_Click(null, null);
				btnDisplayPnlBtm_Click(null, null);
			}
			catch (Exception Ex)
			{
				HandleException(Ex);
			}
		}
		private void VehicleManagerGUI_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				Destructor();
			}
			catch (Exception Ex)
			{
				HandleException(Ex);
			}
		}
		//private void btnInterveneInsertMovingBuffer_Click(object sender, EventArgs e)
		//{
		//	if (cbVehicleList.SelectedItem != null && !string.IsNullOrEmpty(txtInterveneMovingBuffer.Text))
		//	{
		//		SendCommandToVehicle(cbVehicleList.SelectedItem.ToString(), "InsertMovingBuffer", txtInterveneMovingBuffer.Text);
		//	}
		//}
		//private void btnInterveneRemoveMovingBuffer_Click(object sender, EventArgs e)
		//{
		//	if (cbVehicleList.SelectedItem != null)
		//	{
		//		SendCommandToVehicle(cbVehicleList.SelectedItem.ToString(), "RemoveMovingBuffer");
		//	}
		//}
		//private void btnIntervenePauseMoving_Click(object sender, EventArgs e)
		//{
		//	if (cbVehicleList.SelectedItem != null)
		//	{
		//		SendCommandToVehicle(cbVehicleList.SelectedItem.ToString(), "PauseMoving");
		//	}
		//}
		//private void btnInterveneResumeMoving_Click(object sender, EventArgs e)
		//{
		//	if (cbVehicleList.SelectedItem != null)
		//	{
		//		SendCommandToVehicle(cbVehicleList.SelectedItem.ToString(), "ResumeMoving");
		//	}
		//}

		private void btnFormMinimize_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
		}
		private void btnFormClose_Click(object sender, EventArgs e)
		{
			Destructor();
			Close();
		}
		private void btnDisplayPnlLeftMain_Click(object sender, EventArgs e)
		{
			UpdateGui_DisplayPnlLeftMain(!pnlLeftMainDisplay);
		}
		private void btnDisplayVehicleOverview_Click(object sender, EventArgs e)
		{
			UpdateGui_DisplayVehicleOverview();
		}
		private void btnDisplayVehicleManualControl_Click(object sender, EventArgs e)
		{
			UpdateGui_DisplayVehicleManualControl();
		}
		private void btnDisplayAbout_Click(object sender, EventArgs e)
		{
			UpdateGui_DisplayAbout();
		}
		private void btnDisplayMap_Click(object sender, EventArgs e)
		{
			UpdateGui_DisplayMap();
		}
		private void btnDisplayVehicle_Click(object sender, EventArgs e)
		{
			UpdateGui_DisplayVehicle();
		}
		private void btnDisplayMission_Click(object sender, EventArgs e)
		{
			UpdateGui_DisplayMission();
		}
		private void btnDisplaySetting_Click(object sender, EventArgs e)
		{
			UpdateGui_DisplaySetting();
		}
		private void btnDisplayLog_Click(object sender, EventArgs e)
		{
			UpdateGui_DisplayLog();
		}
		private void ucVehicleOverview1_DoubleClickOnVehicleInfo(string VehicleName)
		{
			UpdateGui_MapFocusVehicle(VehicleName);
		}
		private void btnDisplayPnlBtm_Click(object sender, EventArgs e)
		{
			UpdateGui_DisplayPnlBtm(!pnlBtmDisplay);
		}

		#region UpdateGui Functions
		#region General
		private void UpdateGui_ClearComboBoxItems(ComboBox ComboBox)
		{
			ComboBox.InvokeIfNecessary(() => ComboBox.Items.Clear());
		}
		private void UpdateGui_ClearComboBoxSelectedItem(ComboBox ComboBox)
		{
			ComboBox.InvokeIfNecessary(() => ComboBox.SelectedItem = null);
		}
		private void UpdateGui_UpdateComboBoxItems(ComboBox ComboBox, string[] Items)
		{
			UpdateGui_ClearComboBoxItems(ComboBox);
			ComboBox.InvokeIfNecessary(() => ComboBox.Items.AddRange(Items));
		}
		private void UpdateGui_UpdateControlText(Control Control, string Text)
		{
			Control.InvokeIfNecessary(() => { if (Control.Text != Text) Control.Text = Text; });
		}
		private void UpdateGui_UpdateControlBackColor(Control Control, Color Color)
		{
			Control.InvokeIfNecessary(() => { if (Control.BackColor != Color) Control.BackColor = Color; });
		}
		#endregion

		#region PnlTop
		private void UpdateGui_ResetPnlTopMenuButtonBackColor()
		{
			if (btnDisplayMap.BackColor != pnlTop.BackColor) btnDisplayMap.BackColor = pnlTop.BackColor;
			if (btnDisplayVehicle.BackColor != pnlTop.BackColor) btnDisplayVehicle.BackColor = pnlTop.BackColor;
			if (btnDisplayMission.BackColor != pnlTop.BackColor) btnDisplayMission.BackColor = pnlTop.BackColor;
			if (btnDisplaySetting.BackColor != pnlTop.BackColor) btnDisplaySetting.BackColor = pnlTop.BackColor;
			if (btnDisplayLog.BackColor != pnlTop.BackColor) btnDisplayLog.BackColor = pnlTop.BackColor;
		}
		private void UpdateGui_HighlightPnlTopMenuButton(Button button)
		{
			UpdateGui_ResetPnlTopMenuButtonBackColor();
			if (button.BackColor != pnlTopMarker.BackColor) button.BackColor = pnlTopMarker.BackColor;
		}
		#endregion

		#region PnlRightMain
		private void UpdateGui_DisplayMap()
		{
			pnlTopMarker.Width = btnDisplayMap.Width;
			pnlTopMarker.Left = btnDisplayMap.Left;
			ucMap1.BringToFront();
			UpdateGui_HighlightPnlTopMenuButton(btnDisplayMap);
		}
		private void UpdateGui_DisplayVehicle()
		{
			pnlTopMarker.Width = btnDisplayVehicle.Width;
			pnlTopMarker.Left = btnDisplayVehicle.Left;
			ucVehicle1.BringToFront();
			UpdateGui_HighlightPnlTopMenuButton(btnDisplayVehicle);
		}
		private void UpdateGui_DisplayMission()
		{
			pnlTopMarker.Width = btnDisplayMission.Width;
			pnlTopMarker.Left = btnDisplayMission.Left;
			ucMission1.BringToFront();
			UpdateGui_HighlightPnlTopMenuButton(btnDisplayMission);
		}
		private void UpdateGui_DisplaySetting()
		{
			pnlTopMarker.Width = btnDisplaySetting.Width;
			pnlTopMarker.Left = btnDisplaySetting.Left;
			ucSetting1.BringToFront();
			UpdateGui_HighlightPnlTopMenuButton(btnDisplaySetting);
		}
		private void UpdateGui_DisplayLog()
		{
			pnlTopMarker.Width = btnDisplayLog.Width;
			pnlTopMarker.Left = btnDisplayLog.Left;
			ucLog1.BringToFront();
			UpdateGui_HighlightPnlTopMenuButton(btnDisplayLog);
		}
		#region Map
		private void UpdateGui_MapRegisterIconId(IVehicleInfo VehicleInfo)
		{
			ucMap1.RegisterIconId(VehicleInfo);
		}
		private void UpdateGui_MapPrintIcon(IVehicleInfo VehicleInfo)
		{
			ucMap1.PrintIcon(VehicleInfo);
		}
		private void UpdateGui_MapEraseIcon(IVehicleInfo VehicleInfo)
		{
			ucMap1.EraseIcon(VehicleInfo);
		}
		private void UpdateGui_MapRegisterIconId(ICollisionPair CollisionPair)
		{
			ucMap1.RegisterIconId(CollisionPair);
		}
		private void UpdateGui_MapPrintIcon(ICollisionPair CollisionPair)
		{
			ucMap1.PrintIcon(CollisionPair);
		}
		private void UpdateGui_MapEraseIcon(ICollisionPair CollisionPair)
		{
			ucMap1.EraseIcon(CollisionPair);
		}
		private void UpdateGui_MapFocusVehicle(string VehicleName)
		{
			ucMap1.InvokeIfNecessary(() =>
			{
				if (ucMap1.FocusVehicle(VehicleName))
				{
					if (pnlTopMarker.Left != btnDisplayMap.Left)
					{
						UpdateGui_DisplayMap();
					}
				}
			});
		}
		#endregion
		#region Mission
		private void UpdateGui_AddMission(string MissionId, IMissionState MissionState)
		{
			ucMission1.InvokeIfNecessary(() =>
			{
				ucMission1.AddRow(MissionId, MissionState.ToStringArray());
			});
		}
		private void UpdateGui_RemoveMission(string MissionId)
		{
			ucMission1.InvokeIfNecessary(() =>
			{
				ucMission1.RemoveRow(MissionId);
			});
		}
		private void UpdateGui_UpdateMission(string MissionId, string StateName, IMissionState MissionState)
		{
			ucMission1.InvokeIfNecessary(() =>
			{
				string newValue = null;
				if (StateName == "Priority")
				{
					newValue = MissionState.mMission.mPriority.ToString();
				}
				else if (StateName == "SourceIpPort")
				{
					newValue = MissionState.mSourceIpPort;
				}
				else if (StateName == "ExecutorId")
				{
					newValue = MissionState.mExecutorId;
				}
				else if (StateName.StartsWith("SendState"))
				{
					newValue = $"{MissionState.mSendState.ToString()} / {MissionState.mExecuteState.ToString()}";
				}
				else if (StateName.StartsWith("ExecuteState"))
				{
					newValue = $"{MissionState.mSendState.ToString()} / {MissionState.mExecuteState.ToString()}";
				}

				if (newValue != null)
				{
					ucMission1.UpdateRow(MissionId, StateName, newValue);
				}
			});
		}
		#endregion
		#endregion

		#region PnlLeftMain
		private void UpdateGui_DisplayPnlLeftMain(bool Display)
		{
			if (Display)
			{
				if (pnlLeftMainDisplay == false)
				{
					pnlLeftMain.InvokeIfNecessary(() => pnlLeftMain.Width = pnlLeftMainDefaultWidth);
					pnlLeftMainDisplay = true;
				}
			}
			else
			{
				if (pnlLeftMainDisplay == true)
				{
					pnlLeftMain.InvokeIfNecessary(() => pnlLeftMain.Width = 0);
					pnlLeftMainDisplay = false;
				}
			}
		}
		private void UpdateGui_DisplayVehicleOverview()
		{
			if (!pnlLeftMainDisplay) UpdateGui_DisplayPnlLeftMain(true);
			pnlLeftSideMarker.InvokeIfNecessary(() =>
			{
				pnlLeftSideMarker.Height = btnDisplayVehicleOverview.Height;
				pnlLeftSideMarker.Top = btnDisplayVehicleOverview.Top;
			});
			ucVehicleOverview1.InvokeIfNecessary(() =>
			{
				ucVehicleOverview1.BringToFront();
			});
		}
		private void UpdateGui_DisplayVehicleManualControl()
		{
			if (!pnlLeftMainDisplay) UpdateGui_DisplayPnlLeftMain(true);
			pnlLeftSideMarker.InvokeIfNecessary(() =>
			{
				pnlLeftSideMarker.Height = btnDisplayVehicleManualControl.Height;
				pnlLeftSideMarker.Top = btnDisplayVehicleManualControl.Top;
			});
			ucVehicleManualControl1.InvokeIfNecessary(() =>
			{
				ucVehicleManualControl1.BringToFront();
			});
		}
		private void UpdateGui_DisplayAbout()
		{
			if (!pnlLeftMainDisplay) UpdateGui_DisplayPnlLeftMain(true);
			pnlLeftSideMarker.InvokeIfNecessary(() =>
			{
				pnlLeftSideMarker.Height = btnDisplayAbout.Height;
				pnlLeftSideMarker.Top = btnDisplayAbout.Top;
			});
			ucAbout1.InvokeIfNecessary(() =>
			{
				ucAbout1.BringToFront();
			});
		}
		#region VehicleOverview
		private void UpdateGui_AddVehicleOverview(string Id, string Battery, string State)
		{
			ucVehicleOverview1.InvokeIfNecessary(() =>
			{
				ucVehicleOverview1.Add(Id, Battery, State);
			});
		}
		private void UpdateGui_SetVehicleOverview(string Id, Property Property, string Value)
		{
			ucVehicleOverview1.InvokeIfNecessary(() =>
			{
				ucVehicleOverview1.Set(Id, Property, Value);
			});
		}
		private void UpdateGui_RemoveVehicleOverview(string Id)
		{
			ucVehicleOverview1.InvokeIfNecessary(() =>
			{
				ucVehicleOverview1.Remove(Id);
			});
		}
		#endregion
		#region VehicleManualControl
		private void UpdateGui_UpdateVehicleNameList()
		{
			string[] vehicleNameList = mCore.GetVehicleNameList()?.ToArray();
			ucVehicleManualControl1.InvokeIfNecessary(() =>
			{
				ucVehicleManualControl1.UpdateVehicleNameList(vehicleNameList);
			});
		}
		private void UpdateGui_UpdateGoalList()
		{
			string[] goalList = ucMap1.GetGoalList();
			ucVehicleManualControl1.InvokeIfNecessary(() =>
			{
				ucVehicleManualControl1.UpdateGoalList(goalList);
			});
		}
		#endregion
		#endregion

		#region PnlBtm
		private void UpdateGui_DisplayPnlBtm(bool Display)
		{
			if (Display)
			{
				if (pnlBtmDisplay == false)
				{
					pnlBtm.InvokeIfNecessary(() => { pnlBtm.Height = pnlBtmDefaultHeight; btnDisplayPnlBtm.BackColor = pnlTopMarker.BackColor; });
					pnlBtmDisplay = true;
				}
			}
			else
			{
				if (pnlBtmDisplay == true)
				{
					pnlBtm.InvokeIfNecessary(() => { pnlBtm.Height = 0; btnDisplayPnlBtm.BackColor = pnlTop.BackColor; });
					pnlBtmDisplay = false;
				}
			}
		}
		private void UpdateGui_AddSimpleLog(string Date, string Category, string Message)
		{
			ucSimpleLog1.InvokeIfNecessary(() =>
			{
				ucSimpleLog1.AddSimpleLog(Date, Category, Message);
			});
		}
		private void UpdateGui_ClearSimpleLog()
		{
			ucSimpleLog1.InvokeIfNecessary(() =>
			{
				ucSimpleLog1.ClearSimpleLog();
			});
		}
		#endregion
		#endregion

		#region VehicleManagerProcess
		VehicleManagerProcess mCore;

		private void Constructor_VehicleManagerProcess()
		{
			mCore = new VehicleManagerProcess();
			SubscribeEvent_VehicleManagerProcess(mCore);
			mCore.VehicleCommunicatorStartListen(8000);
			mCore.CollisionEventDetectorStart();
			mCore.VehicleControlHandlerStart();
			mCore.HostCommunicatorStartListen(9000);
			mCore.MissionDispatcherStart();
		}
		private void Destructor_VehicleManagerProcess()
		{
			mCore.MissionDispatcherStop();
			mCore.VehicleCommunicatorStopListen();
			mCore.CollisionEventDetectorStop();
			mCore.VehicleControlHandlerStop();
			mCore.HostCommunicatorStopListen();
			UnsubscribeEvent_VehicleManagerProcess(mCore);
			mCore = null;
		}
		private void SubscribeEvent_VehicleManagerProcess(VehicleManagerProcess VehicleManagerProcess)
		{
			if (VehicleManagerProcess != null)
			{
				VehicleManagerProcess.DebugMessage += HandleEvent_VehicleManagerProcessDebugMessage;
				VehicleManagerProcess.VehicleCommunicatorLocalListenStateChagned += HandleEvent_VehicleManagerProcessVehicleCommunicatorLocalListenStateChagned;
				VehicleManagerProcess.VehicleInfoManagerItemAdded += HandleEvent_VehicleManagerProcessVehicleInfoManagerItemAdded;
				VehicleManagerProcess.VehicleInfoManagerItemRemoved += HandleEvent_VehicleManagerProcessVehicleInfoManagerItemRemoved;
				VehicleManagerProcess.VehicleInfoManagerItemUpdated += HandleEvent_VehicleManagerProcessVehicleInfoManagerItemUpdated;
				VehicleManagerProcess.CollisionEventManagerCollisionEventAdded += HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventAdded;
				VehicleManagerProcess.CollisionEventManagerCollisionEventRemoved += HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventRemoved;
				VehicleManagerProcess.CollisionEventManagerCollisionEventStateUpdated += HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventStateUpdated;
				VehicleManagerProcess.MissionStateManagerItemAdded += HandleEvent_VehicleManagerProcessMissionStateManagerItemAdded;
				VehicleManagerProcess.MissionStateManagerItemRemoved += HandleEvent_VehicleManagerProcessMissionStateManagerItemRemoved;
				VehicleManagerProcess.MissionStateManagerItemUpdated += HandleEvent_VehicleManagerProcessMissionStateManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_VehicleManagerProcess(VehicleManagerProcess VehicleManagerProcess)
		{
			if (VehicleManagerProcess != null)
			{
				VehicleManagerProcess.DebugMessage -= HandleEvent_VehicleManagerProcessDebugMessage;
				VehicleManagerProcess.VehicleCommunicatorLocalListenStateChagned -= HandleEvent_VehicleManagerProcessVehicleCommunicatorLocalListenStateChagned;
				VehicleManagerProcess.VehicleInfoManagerItemAdded -= HandleEvent_VehicleManagerProcessVehicleInfoManagerItemAdded;
				VehicleManagerProcess.VehicleInfoManagerItemRemoved -= HandleEvent_VehicleManagerProcessVehicleInfoManagerItemRemoved;
				VehicleManagerProcess.VehicleInfoManagerItemUpdated -= HandleEvent_VehicleManagerProcessVehicleInfoManagerItemUpdated;
				VehicleManagerProcess.CollisionEventManagerCollisionEventAdded -= HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventAdded;
				VehicleManagerProcess.CollisionEventManagerCollisionEventRemoved -= HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventRemoved;
				VehicleManagerProcess.CollisionEventManagerCollisionEventStateUpdated -= HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventStateUpdated;
				VehicleManagerProcess.MissionStateManagerItemAdded -= HandleEvent_VehicleManagerProcessMissionStateManagerItemAdded;
				VehicleManagerProcess.MissionStateManagerItemRemoved -= HandleEvent_VehicleManagerProcessMissionStateManagerItemRemoved;
				VehicleManagerProcess.MissionStateManagerItemUpdated -= HandleEvent_VehicleManagerProcessMissionStateManagerItemUpdated;
			}
		}
		private void HandleEvent_VehicleManagerProcessDebugMessage(string OccurTime, string Category, string Message)
		{
			UpdateGui_AddSimpleLog(OccurTime, Category, Message);
		}
		private void HandleEvent_VehicleManagerProcessVehicleCommunicatorLocalListenStateChagned(DateTime OccurTime, ListenState NewState)
		{
			if (NewState == ListenState.Listening)
			{
				UpdateGui_UpdateControlBackColor(lblConnection, Color.DarkOrange);
			}
			else
			{
				UpdateGui_UpdateControlBackColor(lblConnection, Color.DarkRed);
				UpdateGui_UpdateControlText(lblConnection, "0");
			}
		}
		private void HandleEvent_VehicleManagerProcessVehicleInfoManagerItemAdded(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			UpdateGui_MapRegisterIconId(VehicleInfo);
			UpdateGui_UpdateVehicleNameList();
			UpdateGui_AddVehicleOverview(VehicleInfo.mName, VehicleInfo.mBattery.ToString("F2"), VehicleInfo.mState);
			UpdateGui_UpdateControlBackColor(lblConnection, Color.DarkGreen);
			UpdateGui_UpdateControlText(lblConnection, mCore.GetVehicleNameList().Count.ToString());
		}
		private void HandleEvent_VehicleManagerProcessVehicleInfoManagerItemRemoved(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			UpdateGui_MapEraseIcon(VehicleInfo);
			UpdateGui_UpdateVehicleNameList();
			UpdateGui_RemoveVehicleOverview(VehicleInfo.mName);
			UpdateGui_UpdateControlBackColor(lblConnection, mCore.GetVehicleCount() > 0 ? Color.DarkGreen : Color.DarkOrange);
			UpdateGui_UpdateControlText(lblConnection, mCore.GetVehicleCount().ToString());
		}
		private void HandleEvent_VehicleManagerProcessVehicleInfoManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleInfo VehicleInfo)
		{
			UpdateGui_MapPrintIcon(VehicleInfo);
			UpdateGui_SetVehicleOverview(VehicleInfo.mName, Property.Battery, VehicleInfo.mBattery.ToString("F2"));
			UpdateGui_SetVehicleOverview(VehicleInfo.mName, Property.State, VehicleInfo.mState);
		}
		private void HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventAdded(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			UpdateGui_MapRegisterIconId(CollisionPair);
		}
		private void HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventRemoved(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			UpdateGui_MapEraseIcon(CollisionPair);
		}
		private void HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventStateUpdated(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			UpdateGui_MapPrintIcon(CollisionPair);
		}
		private void HandleEvent_VehicleManagerProcessMissionStateManagerItemAdded(DateTime OccurTime, string MissionId, IMissionState MissionState)
		{
			UpdateGui_AddMission(MissionId, MissionState);
		}
		private void HandleEvent_VehicleManagerProcessMissionStateManagerItemRemoved(DateTime OccurTime, string MissionId, IMissionState MissionState)
		{
			UpdateGui_RemoveMission(MissionId);
		}
		private void HandleEvent_VehicleManagerProcessMissionStateManagerItemUpdated(DateTime OccurTime, string MissionId, string StateName, IMissionState MissionState)
		{
			UpdateGui_UpdateMission(MissionId, StateName, MissionState);
		}

		private void SendCommandToVehicle(string VehicleName, string Command, params string[] Paras)
		{
			mCore.SendCommand(VehicleName, Command, Paras);
		}
		#endregion
	}
}
