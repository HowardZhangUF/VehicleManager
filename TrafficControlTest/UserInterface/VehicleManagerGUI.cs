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
			Constructor_Configuration();
			ucMap1.Constructor("Style.ini");
			Constructor_VehicleManagerProcess();
		}
		private void Destructor()
		{
			Destructor_VehicleManagerProcess();
			ucMap1.Destructor();
			Destructor_Configuration();
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
			UpdateGui_PnlLeftMain_DisplayPnlLeftMain(!pnlLeftMainDisplay);
		}
		private void btnDisplayVehicleOverview_Click(object sender, EventArgs e)
		{
			UpdateGui_PnlLeftMain_DisplayVehicleOverview();
		}
		private void btnDisplayVehicleManualControl_Click(object sender, EventArgs e)
		{
			UpdateGui_PnlLeftMain_DisplayVehicleManualControl();
		}
		private void btnDisplayVehicleApi_Click(object sender, EventArgs e)
		{
			UpdateGui_PnlLeftMain_DisplayVehicleApi();
		}
		private void btnDisplayAbout_Click(object sender, EventArgs e)
		{
			UpdateGui_PnlLeftMain_DisplayAbout();
		}
		private void btnDisplayMap_Click(object sender, EventArgs e)
		{
			UpdateGui_PnlRightMain_DisplayMap();
		}
		private void btnDisplayVehicle_Click(object sender, EventArgs e)
		{
			UpdateGui_PnlRightMain_DisplayVehicle();
		}
		private void btnDisplayMission_Click(object sender, EventArgs e)
		{
			UpdateGui_PnlRightMain_DisplayMission();
		}
		private void btnDisplaySetting_Click(object sender, EventArgs e)
		{
			UpdateGui_PnlRightMain_DisplaySetting();
		}
		private void btnDisplayLog_Click(object sender, EventArgs e)
		{
			UpdateGui_PnlRightMain_DisplayLog();
		}
		private void btnDisplayPnlBtm_Click(object sender, EventArgs e)
		{
			UpdateGui_PnlBtm_DisplayPnlBtm(!pnlBtmDisplay);
		}
		private void ucVehicle1_VehicleStateNeedToBeRefreshed(string VehicleName)
		{
			UpdateGui_UcVehicle_UpdateVehicleInfo(VehicleName, string.Empty, GetVehicleInfo(VehicleName));
		}
		private void ucVehicleOverview1_DoubleClickOnVehicleInfo(string VehicleName)
		{
			UpdateGui_UcMap_MapFocusVehicle(VehicleName);
		}
		private void ucVehicleApi1_VehicleAction(string VehicleName, string Command, string[] Parameters)
		{
			SendCommandToVehicle(VehicleName, Command, Parameters);
		}
		private void ucVehicleApi1_VehicleStateNeedToBeRefreshed(string VehicleName)
		{
			UpdateGui_UcVehicleApi_UpdateRemoteMapNameList(VehicleName);
			UpdateGui_UcVehicleApi_UpdateLocalMapNameList();
		}

		#region UpdateGui Functions
		#region General
		private void UpdateGui_UpdateControlText(Control Control, string Text)
		{
			Control.InvokeIfNecessary(() => { if (Control.Text != Text) Control.Text = Text; });
		}
		private void UpdateGui_UpdateControlBackColor(Control Control, Color Color)
		{
			Control.InvokeIfNecessary(() => { if (Control.BackColor != Color) Control.BackColor = Color; });
		}

		private void UpdateGui_All_UpdateVehicleInfo(string VehicleName, string StateName, IVehicleInfo VehicleInfo)
		{
			if (VehicleInfo != null)
			{
				// Update Page of Vehicle
				UpdateGui_UcVehicle_UpdateVehicleInfo(VehicleName, StateName, VehicleInfo);

				// Update Page of Vehicle Overview
				UpdateGui_UcVehicleOverview_SetVehicleOverview(VehicleInfo.mName, Property.Battery, VehicleInfo.mBatteryValue.ToString("F2"));
				UpdateGui_UcVehicleOverview_SetVehicleOverview(VehicleInfo.mName, Property.State, VehicleInfo.mCurrentState);

				// Update Page of Vehicle Api
				if (StateName.Contains("CurrentMapNameList"))
				{
					UpdateGui_UcVehicleApi_UpdateRemoteMapNameList(VehicleName, VehicleInfo.mCurrentMapNameList);
					UpdateGui_UcVehicleApi_UpdateLocalMapNameList();
				}
			}
		}
		private void UpdateGui_All_UpdateVehicleNameList()
		{
			string[] vehicleNameList = GetVehicleNameList();
			ucVehicle1.InvokeIfNecessary(() =>
			{
				ucVehicle1.UpdateVehicleNameList(vehicleNameList);
			});
			ucVehicleManualControl1.InvokeIfNecessary(() =>
			{
				ucVehicleManualControl1.UpdateVehicleNameList(vehicleNameList);
			});
			ucVehicleApi1.InvokeIfNecessary(() =>
			{
				ucVehicleApi1.UpdateVehicleNameList(vehicleNameList);
			});
		}
		private void UpdateGui_All_UpdateGoalNameList()
		{
			string[] goalList = ucMap1.GetGoalList();
			ucVehicleManualControl1.InvokeIfNecessary(() =>
			{
				ucVehicleManualControl1.UpdateGoalList(goalList);
			});
		}
		#endregion

		#region PnlTop
		private void UpdateGui_PnlTop_ResetPnlTopMenuButtonBackColor()
		{
			if (btnDisplayMap.BackColor != pnlTop.BackColor) btnDisplayMap.BackColor = pnlTop.BackColor;
			if (btnDisplayVehicle.BackColor != pnlTop.BackColor) btnDisplayVehicle.BackColor = pnlTop.BackColor;
			if (btnDisplayMission.BackColor != pnlTop.BackColor) btnDisplayMission.BackColor = pnlTop.BackColor;
			if (btnDisplaySetting.BackColor != pnlTop.BackColor) btnDisplaySetting.BackColor = pnlTop.BackColor;
			if (btnDisplayLog.BackColor != pnlTop.BackColor) btnDisplayLog.BackColor = pnlTop.BackColor;
		}
		private void UpdateGui_PnlTop_HighlightPnlTopMenuButton(Button button)
		{
			UpdateGui_PnlTop_ResetPnlTopMenuButtonBackColor();
			if (button.BackColor != pnlTopMarker.BackColor) button.BackColor = pnlTopMarker.BackColor;
		}
		#endregion

		#region PnlRightMain
		private void UpdateGui_PnlRightMain_DisplayMap()
		{
			pnlTopMarker.Width = btnDisplayMap.Width;
			pnlTopMarker.Left = btnDisplayMap.Left;
			ucMap1.BringToFront();
			UpdateGui_PnlTop_HighlightPnlTopMenuButton(btnDisplayMap);
		}
		private void UpdateGui_PnlRightMain_DisplayVehicle()
		{
			pnlTopMarker.Width = btnDisplayVehicle.Width;
			pnlTopMarker.Left = btnDisplayVehicle.Left;
			ucVehicle1.BringToFront();
			UpdateGui_PnlTop_HighlightPnlTopMenuButton(btnDisplayVehicle);
		}
		private void UpdateGui_PnlRightMain_DisplayMission()
		{
			pnlTopMarker.Width = btnDisplayMission.Width;
			pnlTopMarker.Left = btnDisplayMission.Left;
			ucMission1.BringToFront();
			UpdateGui_PnlTop_HighlightPnlTopMenuButton(btnDisplayMission);
		}
		private void UpdateGui_PnlRightMain_DisplaySetting()
		{
			pnlTopMarker.Width = btnDisplaySetting.Width;
			pnlTopMarker.Left = btnDisplaySetting.Left;
			ucSetting1.BringToFront();
			UpdateGui_PnlTop_HighlightPnlTopMenuButton(btnDisplaySetting);
		}
		private void UpdateGui_PnlRightMain_DisplayLog()
		{
			pnlTopMarker.Width = btnDisplayLog.Width;
			pnlTopMarker.Left = btnDisplayLog.Left;
			ucLog1.BringToFront();
			UpdateGui_PnlTop_HighlightPnlTopMenuButton(btnDisplayLog);
		}
		#region Map
		private void UpdateGui_UcMap_MapRegisterIconId(IVehicleInfo VehicleInfo)
		{
			ucMap1.RegisterIconId(VehicleInfo);
		}
		private void UpdateGui_UcMap_MapPrintIcon(IVehicleInfo VehicleInfo)
		{
			ucMap1.PrintIcon(VehicleInfo);
		}
		private void UpdateGui_UcMap_MapEraseIcon(IVehicleInfo VehicleInfo)
		{
			ucMap1.EraseIcon(VehicleInfo);
		}
		private void UpdateGui_UcMap_MapRegisterIconId(ICollisionPair CollisionPair)
		{
			ucMap1.RegisterIconId(CollisionPair);
		}
		private void UpdateGui_UcMap_MapPrintIcon(ICollisionPair CollisionPair)
		{
			ucMap1.PrintIcon(CollisionPair);
		}
		private void UpdateGui_UcMap_MapEraseIcon(ICollisionPair CollisionPair)
		{
			ucMap1.EraseIcon(CollisionPair);
		}
		private void UpdateGui_UcMap_MapFocusVehicle(string VehicleName)
		{
			ucMap1.InvokeIfNecessary(() =>
			{
				if (ucMap1.FocusVehicle(VehicleName))
				{
					if (pnlTopMarker.Left != btnDisplayMap.Left)
					{
						UpdateGui_PnlRightMain_DisplayMap();
					}
				}
			});
		}
		#endregion
		#region Vehicle
		private void UpdateGui_UcVehicle_UpdateVehicleInfo(string VehicleName, string StateName, IVehicleInfo VehicleInfo)
		{
			if (VehicleInfo != null)
			{
				ucVehicle1.InvokeIfNecessary(() =>
				{
					if (ucVehicle1.CurrentVehicleName == VehicleName)
					{
						ucVehicle1.UpdateVehicleState(VehicleInfo.mCurrentState);
						ucVehicle1.UpdateVehicleLocation(VehicleInfo.mLocationCoordinate.mX, VehicleInfo.mLocationCoordinate.mY, VehicleInfo.mLocationToward);
						ucVehicle1.UpdateVehicleTarget(VehicleInfo.mCurrentTarget);
						ucVehicle1.UpdateVehicleVelocity(VehicleInfo.mVelocity);
						ucVehicle1.UpdateVehicleLocationScore(VehicleInfo.mLocationScore);
						ucVehicle1.UpdateVehicleBatteryValue(VehicleInfo.mBatteryValue);
						ucVehicle1.UpdateVehicleAlarmMessage(VehicleInfo.mAlarmMessage);
						ucVehicle1.UpdateVehiclePath(string.Join(string.Empty, VehicleInfo.mPath.Select(o => o.ToString())));
						ucVehicle1.UpdateVehicleIpPort(VehicleInfo.mIpPort);
						ucVehicle1.UpdateVehicleMissionId(VehicleInfo.mCurrentMissionId);
						ucVehicle1.UpdateVehicleInterveneCommand(VehicleInfo.mCurrentInterveneCommand);
						ucVehicle1.UpdateVehicleMapName(VehicleInfo.mCurrentMapName);
						ucVehicle1.UpdateVehicleLastUpdateTime(VehicleInfo.mLastUpdated.ToString(Library.Library.TIME_FORMAT));
					}
				});
			}
		}
		#endregion
		#region Mission
		private void UpdateGui_UcMission_AddMission(string MissionId, IMissionState MissionState)
		{
			ucMission1.InvokeIfNecessary(() =>
			{
				ucMission1.AddRow(MissionId, MissionState.ToStringArray());
			});
		}
		private void UpdateGui_UcMission_RemoveMission(string MissionId)
		{
			ucMission1.InvokeIfNecessary(() =>
			{
				ucMission1.RemoveRow(MissionId);
			});
		}
		private void UpdateGui_UcMission_UpdateMission(string MissionId, string StateName, IMissionState MissionState)
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
		private void UpdateGui_PnlLeftMain_DisplayPnlLeftMain(bool Display)
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
		private void UpdateGui_PnlLeftMain_DisplayVehicleOverview()
		{
			if (!pnlLeftMainDisplay || (pnlLeftMainDisplay && pnlLeftSideMarker.Top != btnDisplayVehicleOverview.Top))
			{
				UpdateGui_PnlLeftMain_DisplayPnlLeftMain(true);
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
			else
			{
				UpdateGui_PnlLeftMain_DisplayPnlLeftMain(false);
			}
		}
		private void UpdateGui_PnlLeftMain_DisplayVehicleManualControl()
		{
			if (!pnlLeftMainDisplay || (pnlLeftMainDisplay && pnlLeftSideMarker.Top != btnDisplayVehicleManualControl.Top))
			{
				UpdateGui_PnlLeftMain_DisplayPnlLeftMain(true);
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
			else
			{
				UpdateGui_PnlLeftMain_DisplayPnlLeftMain(false);
			}
		}
		private void UpdateGui_PnlLeftMain_DisplayVehicleApi()
		{
			if (!pnlLeftMainDisplay || (pnlLeftMainDisplay && pnlLeftSideMarker.Top != btnDisplayVehicleApi.Top))
			{
				UpdateGui_PnlLeftMain_DisplayPnlLeftMain(true);
				pnlLeftSideMarker.InvokeIfNecessary(() =>
				{
					pnlLeftSideMarker.Height = btnDisplayVehicleApi.Height;
					pnlLeftSideMarker.Top = btnDisplayVehicleApi.Top;
				});
				ucVehicleApi1.InvokeIfNecessary(() =>
				{
					ucVehicleApi1.BringToFront();
				});
			}
			else
			{
				UpdateGui_PnlLeftMain_DisplayPnlLeftMain(false);
			}
		}
		private void UpdateGui_PnlLeftMain_DisplayAbout()
		{
			if (!pnlLeftMainDisplay || (pnlLeftMainDisplay && pnlLeftSideMarker.Top != btnDisplayAbout.Top))
			{
				UpdateGui_PnlLeftMain_DisplayPnlLeftMain(true);
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
			else
			{
				UpdateGui_PnlLeftMain_DisplayPnlLeftMain(false);
			}
		}
		#region VehicleOverview
		private void UpdateGui_UcVehicleOverview_AddVehicleOverview(string Id, string Battery, string State)
		{
			ucVehicleOverview1.InvokeIfNecessary(() =>
			{
				ucVehicleOverview1.Add(Id, Battery, State);
			});
		}
		private void UpdateGui_UcVehicleOverview_SetVehicleOverview(string Id, Property Property, string Value)
		{
			ucVehicleOverview1.InvokeIfNecessary(() =>
			{
				ucVehicleOverview1.Set(Id, Property, Value);
			});
		}
		private void UpdateGui_UcVehicleOverview_RemoveVehicleOverview(string Id)
		{
			ucVehicleOverview1.InvokeIfNecessary(() =>
			{
				ucVehicleOverview1.Remove(Id);
			});
		}
		#endregion
		#region VehicleApi
		private void UpdateGui_UcVehicleApi_UpdateRemoteMapNameList(string VehicleName)
		{
			UpdateGui_UcVehicleApi_UpdateRemoteMapNameList(VehicleName, GetVehicleInfo(VehicleName).mCurrentMapNameList.ToArray());
		}
		private void UpdateGui_UcVehicleApi_UpdateRemoteMapNameList(string VehicleName, IEnumerable<string> MapNameList)
		{
			ucVehicleApi1.InvokeIfNecessary(() =>
			{
				if (ucVehicleApi1.CurrentVehicleName == VehicleName)
				{
					ucVehicleApi1.UpdateRemoteMapNameList(MapNameList.ToArray());
				}
			});
		}
		private void UpdateGui_UcVehicleApi_UpdateLocalMapNameList()
		{
			ucVehicleApi1.InvokeIfNecessary(() =>
			{
				ucVehicleApi1.UpdateLocalMapNameList(Library.Library.GetLocalMapNameList());
			});
		}
		#endregion
		#endregion

		#region PnlBtm
		private void UpdateGui_PnlBtm_DisplayPnlBtm(bool Display)
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
		private void UpdateGui_PnlBtm_AddSimpleLog(string Date, string Category, string Message)
		{
			ucSimpleLog1.InvokeIfNecessary(() =>
			{
				ucSimpleLog1.AddSimpleLog(Date, Category, Message);
			});
		}
		private void UpdateGui_PnlBtm_ClearSimpleLog()
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

			mCore.VehicleCommunicatorSetConfigOfListenPort(int.Parse(GetConfigurationValue("VehicleCommunicator", "ListenPort")));
			mCore.HostCommunicatorSetConfigOfListenPort(int.Parse(GetConfigurationValue("HostCommunicator", "ListenPort")));

			mCore.VehicleCommunicatorStartListen();
			mCore.CollisionEventDetectorStart();
			mCore.VehicleControlHandlerStart();
			mCore.HostCommunicatorStartListen();
			mCore.MissionDispatcherStart();
		}
		private void Destructor_VehicleManagerProcess()
		{
			mCore.MissionDispatcherStop();
			mCore.VehicleCommunicatorStopListen();
			mCore.CollisionEventDetectorStop();
			mCore.VehicleControlHandlerStop();
			mCore.HostCommunicatorStopListen();

			SetConfigurationValue("HostCommunicator", "ListenPort", mCore.HostCommunicatorGetConfigOfListenPort().ToString());
			SetConfigurationValue("VehicleCommunicator", "ListenPort", mCore.VehicleCommunicatorGetConfigOfListenPort().ToString());

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
			UpdateGui_PnlBtm_AddSimpleLog(OccurTime, Category, Message);
		}
		private void HandleEvent_VehicleManagerProcessVehicleCommunicatorLocalListenStateChagned(DateTime OccurTime, ListenState NewState, int Port)
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
			UpdateGui_UcMap_MapRegisterIconId(VehicleInfo);
			UpdateGui_All_UpdateVehicleNameList();
			UpdateGui_UcVehicleOverview_AddVehicleOverview(VehicleInfo.mName, VehicleInfo.mBatteryValue.ToString("F2"), VehicleInfo.mCurrentState);
			UpdateGui_UpdateControlBackColor(lblConnection, Color.DarkGreen);
			UpdateGui_UpdateControlText(lblConnection, mCore.GetVehicleNameList().Count.ToString());
		}
		private void HandleEvent_VehicleManagerProcessVehicleInfoManagerItemRemoved(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			UpdateGui_UcMap_MapEraseIcon(VehicleInfo);
			UpdateGui_All_UpdateVehicleNameList();
			UpdateGui_UcVehicleOverview_RemoveVehicleOverview(VehicleInfo.mName);
			UpdateGui_UpdateControlBackColor(lblConnection, mCore.GetVehicleCount() > 0 ? Color.DarkGreen : Color.DarkOrange);
			UpdateGui_UpdateControlText(lblConnection, mCore.GetVehicleCount().ToString());
		}
		private void HandleEvent_VehicleManagerProcessVehicleInfoManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleInfo VehicleInfo)
		{
			UpdateGui_UcMap_MapPrintIcon(VehicleInfo);
			UpdateGui_All_UpdateVehicleInfo(Name, StateName, VehicleInfo);
		}
		private void HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventAdded(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			UpdateGui_UcMap_MapRegisterIconId(CollisionPair);
		}
		private void HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventRemoved(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			UpdateGui_UcMap_MapEraseIcon(CollisionPair);
		}
		private void HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventStateUpdated(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			UpdateGui_UcMap_MapPrintIcon(CollisionPair);
		}
		private void HandleEvent_VehicleManagerProcessMissionStateManagerItemAdded(DateTime OccurTime, string MissionId, IMissionState MissionState)
		{
			UpdateGui_UcMission_AddMission(MissionId, MissionState);
		}
		private void HandleEvent_VehicleManagerProcessMissionStateManagerItemRemoved(DateTime OccurTime, string MissionId, IMissionState MissionState)
		{
			UpdateGui_UcMission_RemoveMission(MissionId);
		}
		private void HandleEvent_VehicleManagerProcessMissionStateManagerItemUpdated(DateTime OccurTime, string MissionId, string StateName, IMissionState MissionState)
		{
			UpdateGui_UcMission_UpdateMission(MissionId, StateName, MissionState);
		}

		private string[] GetVehicleNameList()
		{
			return mCore.GetVehicleNameList()?.ToArray();
		}
		private IVehicleInfo GetVehicleInfo(string VehicleName)
		{
			return mCore.GetVehicleInfo(VehicleName);
		}
		private void SendCommandToVehicle(string VehicleName, string Command, params string[] Paras)
		{
			mCore.SendCommand(VehicleName, Command, Paras);
		}
		#endregion

		#region Configuration
		private void Constructor_Configuration()
		{
			LoadConfiguration();
		}
		private void Destructor_Configuration()
		{
			SaveConfiguration();
		}
		private void LoadConfiguration()
		{
			Configuration.Load();
		}
		private void SaveConfiguration()
		{
			Configuration.Save();
		}
		private string GetConfigurationValue(string Category, string Name)
		{
			return Configuration.GetValue(Category, Name);
		}
		private void SetConfigurationValue(string Category, string Name, string Value)
		{
			Configuration.SetValue(Category, Name, Value);
		}
		#endregion
	}
}
