using System;
using System.Drawing;
using System.Windows.Forms;
using TrafficControlTest.Process;
using TrafficControlTest.Library;
using TrafficControlTest.UserControl;
using TrafficControlTest.Module.Account;

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
			Constructor_VehicleManagerProcess();
			ucMap1.SetStyleFileName("Style.ini");
			ucMap1.Set(mCore.GetReferenceOfIVehicleInfoManager(), mCore.GetReferenceOfICollisionEventManager());
			ucVehicle1.Set(mCore.GetReferenceOfIVehicleInfoManager());
			ucMission1.Set(mCore.GetReferenceOfIMissionStateManager());
			ucSetting1.Set(mCore.GetReferenceOfIConfigurator());
			ucLog1.Set(mCore.GetReferenceOfDatabaseAdapterOfLogRecord(), mCore.GetReferenceOfDatabaseAdapterOfEventRecord());
			ucDashboard1.Set(mCore.GetReferenceOfDatabaseAdapterOfEventRecord());
			ucSystemStatus1.Set(mCore.GetReferenceOfIImportantEventRecorder(), mCore.GetReferenceOfIVehicleCommunicator(), mCore.GetReferenceOfICollisionEventDetector(), mCore.GetReferenceOfIVehicleControlHandler(), mCore.GetReferenceOfIHostCommunicator(), mCore.GetReferenceOfIMissionDispatcher(), mCore.GetReferenceOfIMissionUpdater(), mCore.GetReferenceOfCycleMissionGenerator(), mCore.GetReferenceOfILogExporter(), mCore.GetReferenceOfIMapManager());
			ucVehicleOverview1.Set(mCore.GetReferenceOfIVehicleInfoManager());
			ucVehicleManualControl1.Set(mCore.GetReferenceOfIVehicleCommunicator(), mCore.GetReferenceOfIVehicleInfoManager(), mCore.GetReferenceOfIMapManager());
			ucVehicleApi1.Set(mCore.GetReferenceOfIVehicleInfoManager(), mCore.GetReferenceOfIVehicleCommunicator(), mCore.GetReferenceOfIMapFileManager(), mCore.GetReferenceOfIMapManager());
			ucCycleMission1.Set(mCore.GetReferenceOfIVehicleInfoManager(), mCore.GetReferenceOfCycleMissionGenerator());
			ucSimpleLog1.Set(mCore);
			ucConsoleLog1.Set(mCore);
			ucSystemOverview1.Set(mCore.GetReferenceOfIVehicleCommunicator(), mCore.GetReferenceOfIVehicleInfoManager());
		}
		private void Destructor()
		{
			Destructor_VehicleManagerProcess();
		}
		private void HandleException(Exception Ex)
		{
			string directory = ".\\Exception";
			string file = $".\\Exception\\Exception{DateTime.Now.ToString("yyyyMMdd")}.txt";
			string message = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} - [MainException] - {Ex.ToString()}\r\n";

			if (!System.IO.Directory.Exists(directory)) System.IO.Directory.CreateDirectory(directory);
			if (!System.IO.File.Exists(file)) System.IO.File.Create(file).Close();
			System.IO.File.AppendAllText(file, message);
		}
		private void VehicleManagerGUI_Load(object sender, EventArgs e)
		{
			try
			{
				Constructor();
				VehicleManagerProcessStart();
				UpdateGui_PnlTop_UpdateButtonText();
				UpdateGui_UpdateUsableControlAmount(AccountRank.None);
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
			System.Threading.Tasks.Task.Run(() =>
			{
				VehicleManagerProcessStop();
				Destructor();
				this.InvokeIfNecessary(() => Close());
			});
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
		private void btnDisplayCycleMission_Click(object sender, EventArgs e)
		{
			UpdateGui_PnlLeftMain_DisplayCycleMission();
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
		private void btnDisplayDashboard_Click(object sender, EventArgs e)
		{
			UpdateGui_PnlRightMain_DisplayDashboard();
		}
		private void btnDisplaySystemStatus_Click(object sender, EventArgs e)
		{
			UpdateGui_PnlRightMain_DisplaySystemStatus();
		}
		private void btnDisplaySimpleLog_Click(object sender, EventArgs e)
		{
			UpdateGui_PnlBtm_DisplaySimpleLog();
		}
		private void btnDisplayConsoleLog_Click(object sender, EventArgs e)
		{
			UpdateGui_PnlBtm_DisplayConsoleLog();
		}
		private void ucVehicleOverview1_DoubleClickOnVehicleInfo(string VehicleName)
		{
			UpdateGui_UcMap_MapFocusVehicle(VehicleName);
		}
		private void btnLogin_Click(object sender, EventArgs e)
		{
			if (VehicleManagerProcessIsLoggedIn())
			{
				VehicleManagerProcessLogOut();
			}
			else
			{
				if (CustomMessageBox.InputBox("Please Enter Password:", out string password, '*') == DialogResult.OK)
				{
					if (!VehicleManagerProcessLogIn(password))
					{
						CustomMessageBox.OutputBox("Wrong Password!");
					}
				}
			}
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
		private void UpdateGui_UpdateUsableControlAmount(AccountRank Rank)
		{
			this.InvokeIfNecessary(() =>
			{
				switch (Rank)
				{
					case AccountRank.Software:
					case AccountRank.Service:
						btnDisplayVehicleManualControl.Visible = true;
						btnDisplayVehicleApi.Visible = true;
						btnDisplayCycleMission.Visible = true;
						btnDisplayDashboard.Visible = true;
						btnDisplaySystemStatus.Visible = true;
						btnDisplayConsoleLog.Visible = true;
						ucVehicleManualControl1.Visible = true;
						ucVehicleApi1.Visible = true;
						ucCycleMission1.Visible = true;
						ucLog1.Set(true, true, true);
						ucDashboard1.Visible = true;
						ucSystemStatus1.Visible = true;
						ucConsoleLog1.Visible = true;
						UpdateGui_InitializeMenuState();
						break;
					case AccountRank.Customer:
						btnDisplayVehicleManualControl.Visible = true;
						ucVehicleManualControl1.Visible = true;
						UpdateGui_InitializeMenuState();
						break;
					case AccountRank.None:
						btnDisplayVehicleManualControl.Visible = false;
						btnDisplayVehicleApi.Visible = false;
						btnDisplayCycleMission.Visible = false;
						btnDisplayDashboard.Visible = false;
						btnDisplaySystemStatus.Visible = false;
						btnDisplayConsoleLog.Visible = false;
						ucVehicleManualControl1.Visible = false;
						ucVehicleApi1.Visible = false;
						ucCycleMission1.Visible = false;
						ucLog1.Set(false, true, true);
						ucDashboard1.Visible = false;
						ucSystemStatus1.Visible = false;
						ucConsoleLog1.Visible = false;
						UpdateGui_InitializeMenuState();
						break;
				}
			});
		}
		private void UpdateGui_InitializeMenuState()
		{
			// 左側選單選擇 VehicleOverview 並將選單收起來
			UpdateGui_PnlLeftMain_DisplayVehicleOverview();
			UpdateGui_PnlLeftMain_DisplayPnlLeftMain(false);

			// 主選單選擇 Map
			UpdateGui_PnlRightMain_DisplayMap();

			// 隱藏下方選單
			UpdateGui_PnlBtm_DisplayPnlBtm(false);
		}
		#endregion

		#region PnlTop
		private void UpdateGui_PnlTop_UpdateButtonText()
		{
			btnDisplayMap.Text = "  Map";
			btnDisplayVehicle.Text = "  Vehicle";
			btnDisplayMission.Text = "  Mission";
			btnDisplaySetting.Text = "  Setting";
			btnDisplayLog.Text = "  Log";
			btnDisplayDashboard.Text = "  Dashboard";
			btnDisplaySystemStatus.Text = "  System Status";
		}
		private void UpdateGui_PnlTop_ResetPnlTopMenuButtonBackColor()
		{
			if (btnDisplayMap.BackColor != pnlTop.BackColor) btnDisplayMap.BackColor = pnlTop.BackColor;
			if (btnDisplayVehicle.BackColor != pnlTop.BackColor) btnDisplayVehicle.BackColor = pnlTop.BackColor;
			if (btnDisplayMission.BackColor != pnlTop.BackColor) btnDisplayMission.BackColor = pnlTop.BackColor;
			if (btnDisplaySetting.BackColor != pnlTop.BackColor) btnDisplaySetting.BackColor = pnlTop.BackColor;
			if (btnDisplayLog.BackColor != pnlTop.BackColor) btnDisplayLog.BackColor = pnlTop.BackColor;
			if (btnDisplayDashboard.BackColor != pnlTop.BackColor) btnDisplayDashboard.BackColor = pnlTop.BackColor;
			if (btnDisplaySystemStatus.BackColor != pnlTop.BackColor) btnDisplaySystemStatus.BackColor = pnlTop.BackColor;
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
		private void UpdateGui_PnlRightMain_DisplayDashboard()
		{
			pnlTopMarker.Width = btnDisplayDashboard.Width;
			pnlTopMarker.Left = btnDisplayDashboard.Left;
			ucDashboard1.BringToFront();
			UpdateGui_PnlTop_HighlightPnlTopMenuButton(btnDisplayDashboard);
		}
		private void UpdateGui_PnlRightMain_DisplaySystemStatus()
		{
			pnlTopMarker.Width = btnDisplaySystemStatus.Width;
			pnlTopMarker.Left = btnDisplaySystemStatus.Left;
			ucSystemStatus1.BringToFront();
			UpdateGui_PnlTop_HighlightPnlTopMenuButton(btnDisplaySystemStatus);
		}
		#region Map
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
		private void UpdateGui_PnlLeftMain_DisplayCycleMission()
		{
			if (!pnlLeftMainDisplay || (pnlLeftMainDisplay && pnlLeftSideMarker.Top != btnDisplayCycleMission.Top))
			{
				UpdateGui_PnlLeftMain_DisplayPnlLeftMain(true);
				pnlLeftSideMarker.InvokeIfNecessary(() =>
				{
					pnlLeftSideMarker.Height = btnDisplayCycleMission.Height;
					pnlLeftSideMarker.Top = btnDisplayCycleMission.Top;
				});
				ucCycleMission1.InvokeIfNecessary(() =>
				{
					ucCycleMission1.BringToFront();
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
		#endregion

		#region PnlBtm
		private void UpdateGui_PnlBtm_DisplayPnlBtm(bool Display)
		{
			if (Display)
			{
				if (pnlBtmDisplay == false)
				{
					pnlBtm.InvokeIfNecessary(() => { pnlBtm.Height = pnlBtmDefaultHeight; });
					pnlBtmDisplay = true;
				}
			}
			else
			{
				if (pnlBtmDisplay == true)
				{
					pnlBtm.InvokeIfNecessary(() => { pnlBtm.Height = 0; btnDisplaySimpleLog.BackColor = pnlTop.BackColor; btnDisplayConsoleLog.BackColor = pnlTop.BackColor; });
					pnlBtmDisplay = false;
				}
			}
		}
		private void UpdateGui_PnlBtm_DisplaySimpleLog()
		{
			if (!pnlBtmDisplay || (pnlBtmDisplay && btnDisplaySimpleLog.BackColor != pnlTopMarker.BackColor))
			{
				UpdateGui_PnlBtm_DisplayPnlBtm(true);
				btnDisplaySimpleLog.InvokeIfNecessary(() =>
				{
					btnDisplaySimpleLog.BackColor = pnlTopMarker.BackColor;
					btnDisplayConsoleLog.BackColor = pnlTop.BackColor;
				});
				ucSimpleLog1.InvokeIfNecessary(() =>
				{
					ucSimpleLog1.BringToFront();
				});
			}
			else
			{
				UpdateGui_PnlBtm_DisplayPnlBtm(false);
			}
		}
		private void UpdateGui_PnlBtm_DisplayConsoleLog()
		{
			if (!pnlBtmDisplay || (pnlBtmDisplay && btnDisplayConsoleLog.BackColor != pnlTopMarker.BackColor))
			{
				UpdateGui_PnlBtm_DisplayPnlBtm(true);
				btnDisplayConsoleLog.InvokeIfNecessary(() =>
				{
					btnDisplaySimpleLog.BackColor = pnlTop.BackColor;
					btnDisplayConsoleLog.BackColor = pnlTopMarker.BackColor;
				});
				ucConsoleLog1.InvokeIfNecessary(() =>
				{
					ucConsoleLog1.BringToFront();
				});
			}
			else
			{
				UpdateGui_PnlBtm_DisplayPnlBtm(false);
			}
		}
		#endregion
		#endregion

		#region VehicleManagerProcess
		VehicleManagerProcess mCore;

		private void Constructor_VehicleManagerProcess()
		{
			mCore = new VehicleManagerProcess();
			SubscribeEvent_VehicleManagerProcess(mCore);
		}
		private void Destructor_VehicleManagerProcess()
		{
			UnsubscribeEvent_VehicleManagerProcess(mCore);
			mCore = null;
		}
		private void VehicleManagerProcessStart()
		{
			mCore.Start();
		}
		private void VehicleManagerProcessStop()
		{
			mCore.Stop();
		}
		private bool VehicleManagerProcessIsLoggedIn()
		{
			return mCore.mIsAnyUserLoggedIn;
		}
		private bool VehicleManagerProcessLogIn(string Password)
		{
			return mCore.AccessControlLogIn(Password);
		}
		private bool VehicleManagerProcessLogOut()
		{
			return mCore.AccessControlLogOut();
		}
		private void SubscribeEvent_VehicleManagerProcess(VehicleManagerProcess VehicleManagerProcess)
		{
			if (VehicleManagerProcess != null)
			{
				VehicleManagerProcess.AccessControlUserLogChanged += HandleEvent_VehicleManagerProcessAccessControlUserLogChanged;
			}
		}
		private void UnsubscribeEvent_VehicleManagerProcess(VehicleManagerProcess VehicleManagerProcess)
		{
			if (VehicleManagerProcess != null)
			{
				VehicleManagerProcess.AccessControlUserLogChanged -= HandleEvent_VehicleManagerProcessAccessControlUserLogChanged;
			}
		}
		private void HandleEvent_VehicleManagerProcessAccessControlUserLogChanged(object Sender, UserLogChangedEventArgs Args)
		{
			if (Args.IsLogin)
			{
				UpdateGui_UpdateUsableControlAmount(Args.UserRank);
				UpdateGui_UpdateControlText(btnLogin, Args.UserName);
				UpdateGui_UpdateControlBackColor(btnLogin, Color.DarkOrange);
			}
			else
			{
				UpdateGui_UpdateUsableControlAmount(AccountRank.None);
				UpdateGui_UpdateControlText(btnLogin, string.Empty);
				UpdateGui_UpdateControlBackColor(btnLogin, Color.FromArgb(20, 20, 20));
			}
		}
		#endregion
	}
}
