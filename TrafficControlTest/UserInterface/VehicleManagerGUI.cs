using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TrafficControlTest.Process;
using TrafficControlTest.Interface;
using TrafficControlTest.Library;

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
			ucLog1.Set(mCore.GetReferenceOfDatabaseAdapterOfLogRecord(), mCore.GetReferenceOfDatabaseAdapterOfEventRecord());
			ucVehicleOverview1.Set(mCore.GetReferenceOfIVehicleInfoManager());
			ucVehicleManualControl1.Set(mCore.GetReferenceOfIVehicleCommunicator(), mCore.GetReferenceOfIVehicleInfoManager(), mCore.GetReferenceOfIMapManager());
			ucVehicleApi1.Set(mCore.GetReferenceOfIVehicleInfoManager(), mCore.GetReferenceOfIVehicleCommunicator(), mCore.GetReferenceOfIMapFileManager(), mCore.GetReferenceOfIMapManager());
			ucSimpleLog1.Set(mCore);
		}
		private void Destructor()
		{
			Destructor_VehicleManagerProcess();
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
				VehicleManagerProcessStart();
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
				string password = string.Empty;
				if (InputBox(string.Empty, "Please Enter Password:", ref password, '*') == DialogResult.OK)
				{
					VehicleManagerProcessLogIn(password);
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
						ucVehicleManualControl1.Visible = true;
						ucVehicleApi1.Visible = true;
						ucLog1.Set(3, 1, 1);
						UpdateGui_InitializeMenuState();
						break;
					case AccountRank.Customer:
						// 隱藏左側選單的 VehicleManualControl 頁面
						btnDisplayVehicleManualControl.Visible = true;
						btnDisplayVehicleApi.Visible = false;
						ucVehicleManualControl1.Visible = true;
						ucVehicleApi1.Visible = false;
						// 主選單的 Log 頁面僅顯示 MissionState, HostCommunication 頁面
						ucLog1.Set(0, 1, 1);
						UpdateGui_InitializeMenuState();
						break;
					case AccountRank.None:
						// 隱藏左側選單的 VehicleManualControl, VehicleApi 頁面
						btnDisplayVehicleManualControl.Visible = false;
						btnDisplayVehicleApi.Visible = false;
						ucVehicleManualControl1.Visible = false;
						ucVehicleApi1.Visible = false;
						// 主選單的 Log 頁面僅顯示 MissionState, HostCommunication 頁面
						ucLog1.Set(0, 1, 1);
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
		private static DialogResult InputBox(string caption, string text, ref string value, char passwordChar = '\0')
		{
			Form form = new Form();
			Label lblText = new Label();
			TextBox txtResult = new TextBox();
			Button btnOk = new Button();
			Button btnCancel = new Button();

			form.Text = caption;
			form.BackColor = Color.FromArgb(31, 31, 31);
			form.ForeColor = Color.White;
			lblText.Text = text;
			lblText.AutoSize = true;
			txtResult.BackColor = Color.FromArgb(31, 31, 31);
			txtResult.ForeColor = Color.White;
			btnOk.Text = "Confirm";
			btnOk.DialogResult = DialogResult.OK;
			btnCancel.Text = "Cancel";
			btnCancel.DialogResult = DialogResult.Cancel;
			if (passwordChar != '\0') txtResult.PasswordChar = passwordChar;

			form.Controls.AddRange(new Control[] { lblText, txtResult, btnOk, btnCancel });
			lblText.Location = new System.Drawing.Point(20, 10);
			txtResult.SetBounds(20, lblText.Location.Y + lblText.Size.Height + 10, 160, 20);
			btnOk.SetBounds(20, txtResult.Location.Y + txtResult.Size.Height + 10, 75, 30);
			btnOk.FlatStyle = FlatStyle.Flat;
			btnCancel.SetBounds(btnOk.Right + 10, btnOk.Location.Y, 75, 30);
			btnCancel.FlatStyle = FlatStyle.Flat;

			form.ClientSize = new System.Drawing.Size(Math.Max(btnCancel.Right + 20, lblText.Right + 20), btnCancel.Bottom + 10);
			form.FormBorderStyle = FormBorderStyle.None;
			form.AutoScaleMode = AutoScaleMode.None;
			form.StartPosition = FormStartPosition.CenterParent;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = btnOk;
			form.CancelButton = btnCancel;

			DialogResult dialogResult = form.ShowDialog();
			value = txtResult.Text;
			return dialogResult;
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
				VehicleManagerProcess.AccessControlUserLogIn += HandleEvent_VehicleManagerProcessAccessControlUserLogIn;
				VehicleManagerProcess.AccessControlUserLogOut += HandleEvent_VehicleManagerProcessAccessControlUserLogOut;
				VehicleManagerProcess.VehicleCommunicatorLocalListenStateChagned += HandleEvent_VehicleManagerProcessVehicleCommunicatorLocalListenStateChagned;
				VehicleManagerProcess.VehicleInfoManagerItemAdded += HandleEvent_VehicleManagerProcessVehicleInfoManagerItemAdded;
				VehicleManagerProcess.VehicleInfoManagerItemRemoved += HandleEvent_VehicleManagerProcessVehicleInfoManagerItemRemoved;
			}
		}
		private void UnsubscribeEvent_VehicleManagerProcess(VehicleManagerProcess VehicleManagerProcess)
		{
			if (VehicleManagerProcess != null)
			{
				VehicleManagerProcess.AccessControlUserLogIn -= HandleEvent_VehicleManagerProcessAccessControlUserLogIn;
				VehicleManagerProcess.AccessControlUserLogOut -= HandleEvent_VehicleManagerProcessAccessControlUserLogOut;
				VehicleManagerProcess.VehicleCommunicatorLocalListenStateChagned -= HandleEvent_VehicleManagerProcessVehicleCommunicatorLocalListenStateChagned;
				VehicleManagerProcess.VehicleInfoManagerItemAdded -= HandleEvent_VehicleManagerProcessVehicleInfoManagerItemAdded;
				VehicleManagerProcess.VehicleInfoManagerItemRemoved -= HandleEvent_VehicleManagerProcessVehicleInfoManagerItemRemoved;
			}
		}
		private void HandleEvent_VehicleManagerProcessAccessControlUserLogIn(DateTime OccurTime, string Name, AccountRank Rank)
		{
			UpdateGui_UpdateUsableControlAmount(Rank);
			UpdateGui_UpdateControlText(btnLogin, Name);
			UpdateGui_UpdateControlBackColor(btnLogin, Color.DarkOrange);
		}
		private void HandleEvent_VehicleManagerProcessAccessControlUserLogOut(DateTime OccurTime, string Name, AccountRank Rank)
		{
			UpdateGui_UpdateUsableControlAmount(AccountRank.None);
			UpdateGui_UpdateControlText(btnLogin, string.Empty);
			UpdateGui_UpdateControlBackColor(btnLogin, Color.FromArgb(20, 20, 20));
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
			UpdateGui_UpdateControlBackColor(lblConnection, Color.DarkGreen);
			UpdateGui_UpdateControlText(lblConnection, mCore.GetVehicleNameList().Count.ToString());
		}
		private void HandleEvent_VehicleManagerProcessVehicleInfoManagerItemRemoved(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			UpdateGui_UpdateControlBackColor(lblConnection, mCore.GetVehicleCount() > 0 ? Color.DarkGreen : Color.DarkOrange);
			UpdateGui_UpdateControlText(lblConnection, mCore.GetVehicleCount().ToString());
		}

		private string[] GetVehicleNameList()
		{
			return mCore.GetVehicleNameList()?.ToArray();
		}
		private IVehicleInfo GetVehicleInfo(string VehicleName)
		{
			return mCore.GetVehicleInfo(VehicleName);
		}
		private string[] GetLocalMapNameList()
		{
			return mCore.MapFileManagerGetLocalMapNameList();
		}
		private string[] GetMapGoalNameList()
		{
			return mCore.MapManagerGetGoalNameList();
		}
		private void SendCommandToVehicle(string VehicleName, string Command, params string[] Paras)
		{
			mCore.SendCommand(VehicleName, Command, Paras);
		}
		#endregion
	}
}
