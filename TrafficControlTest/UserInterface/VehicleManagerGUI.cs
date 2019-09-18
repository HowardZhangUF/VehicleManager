using Geometry;
using GLCore;
using GLStyle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TrafficControlTest.Base;
using TrafficControlTest.Interface;

namespace TrafficControlTest.UserInterface
{
	public partial class VehicleManagerGUI : Form
	{
		public VehicleManagerGUI()
		{
			InitializeComponent();

			try
			{
				Constructor();
			}
			catch (Exception Ex)
			{
				HandleException(Ex);
			}
		}
		private void Constructor()
		{
			Constructor_GLUI();
			Constructor_VehicleManagerProcess();
		}
		private void Destructor()
		{
			Destructor_VehicleManagerProcess();
			Destructor_GLUI();
		}
		private void HandleException(Exception Ex)
		{
			Console.WriteLine(Ex.ToString());
		}
		private void VehicleManagerGUI_Load(object sender, EventArgs e)
		{
			try
			{
				btnDisplayVehicleOverview_Click(null, null);
				btnDisplayMap_Click(null, null);
				btnDisplayPnlLeftMain_Click(null, null);
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
				Destructor_VehicleManagerProcess();
			}
			catch (Exception Ex)
			{
				HandleException(Ex);
			}
		}
		private void btnInterveneInsertMovingBuffer_Click(object sender, EventArgs e)
		{
			if (cbVehicleList.SelectedItem != null && !string.IsNullOrEmpty(txtInterveneMovingBuffer.Text))
			{
				SendCommandToVehicle(cbVehicleList.SelectedItem.ToString(), "InsertMovingBuffer", txtInterveneMovingBuffer.Text);
			}
		}
		private void btnInterveneRemoveMovingBuffer_Click(object sender, EventArgs e)
		{
			if (cbVehicleList.SelectedItem != null)
			{
				SendCommandToVehicle(cbVehicleList.SelectedItem.ToString(), "RemoveMovingBuffer");
			}
		}
		private void btnIntervenePauseMoving_Click(object sender, EventArgs e)
		{
			if (cbVehicleList.SelectedItem != null)
			{
				SendCommandToVehicle(cbVehicleList.SelectedItem.ToString(), "PauseMoving");
			}
		}
		private void btnInterveneResumeMoving_Click(object sender, EventArgs e)
		{
			if (cbVehicleList.SelectedItem != null)
			{
				SendCommandToVehicle(cbVehicleList.SelectedItem.ToString(), "ResumeMoving");
			}
		}

		private void btnFormMinimize_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
		}
		private void btnFormClose_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void btnDisplayPnlLeftMain_Click(object sender, EventArgs e)
		{
			UpdateGui_DisplayPnlLeftMain(!pnlLeftMainDisplay);
		}
		private void btnDisplayVehicleOverview_Click(object sender, EventArgs e)
		{
			if (!pnlLeftMainDisplay) UpdateGui_DisplayPnlLeftMain(true);
			pnlLeftSideMarker.Height = btnDisplayVehicleOverview.Height;
			pnlLeftSideMarker.Top = btnDisplayVehicleOverview.Top;

			ucVehicleInfoList1.BringToFront();
		}
		private void btnDisplayManualControl_Click(object sender, EventArgs e)
		{
			if (!pnlLeftMainDisplay) UpdateGui_DisplayPnlLeftMain(true);
			pnlLeftSideMarker.Height = btnDisplayManualControl.Height;
			pnlLeftSideMarker.Top = btnDisplayManualControl.Top;

			ucVehicleManualControl1.BringToFront();
		}
		private void btnDisplayAbout_Click(object sender, EventArgs e)
		{
			if (!pnlLeftMainDisplay) UpdateGui_DisplayPnlLeftMain(true);
			pnlLeftSideMarker.Height = btnDisplayAbout.Height;
			pnlLeftSideMarker.Top = btnDisplayAbout.Top;

			ucAbout1.BringToFront();
		}
		private void btnDisplayMap_Click(object sender, EventArgs e)
		{
			pnlTopMarker.Width = btnDisplayMap.Width;
			pnlTopMarker.Left = btnDisplayMap.Left;
		}
		private void btnDisplayVehicle_Click(object sender, EventArgs e)
		{
			pnlTopMarker.Width = btnDisplayVehicle.Width;
			pnlTopMarker.Left = btnDisplayVehicle.Left;
		}
		private void btnDisplayMission_Click(object sender, EventArgs e)
		{
			pnlTopMarker.Width = btnDisplayMission.Width;
			pnlTopMarker.Left = btnDisplayMission.Left;
		}
		private void btnDisplaySetting_Click(object sender, EventArgs e)
		{
			pnlTopMarker.Width = btnDisplaySetting.Width;
			pnlTopMarker.Left = btnDisplaySetting.Left;
		}
		private void btnDisplayLog_Click(object sender, EventArgs e)
		{
			pnlTopMarker.Width = btnDisplayLog.Width;
			pnlTopMarker.Left = btnDisplayLog.Left;
		}

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
		#endregion

		private void UpdateGui_UpdateVehicleNameList()
		{
			string[] vehicleNames = mCore.GetVehicleNameList()?.ToArray();
			if (vehicleNames == null || vehicleNames.Length == 0)
			{
				UpdateGui_ClearComboBoxItems(cbVehicleList);
			}
			else
			{
				UpdateGui_UpdateComboBoxItems(cbVehicleList, vehicleNames);
				UpdateGui_ClearComboBoxSelectedItem(cbVehicleList);
			}
		}
		private void UpdateGui_DisplayPnlLeftMain(bool Display)
		{
			if (Display)
			{
				if (pnlLeftMainDisplay == false)
				{
					pnlLeftMain.InvokeIfNecessary(() => pnlLeftMain.Width = pnlLeftMainWidth);
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
		private void UpdateGui_AddVehicleOverview(string Id, string Battery, string State)
		{
			ucVehicleInfoList1.InvokeIfNecessary(() => ucVehicleInfoList1.Add(Id, Battery, State));
		}
		private void UpdateGui_SetVehicleOverview(string Id, UserControl.UCVehicleInfoList.Property Property, string Value)
		{
			ucVehicleInfoList1.InvokeIfNecessary(() => ucVehicleInfoList1.Set(Id, Property, Value));
		}
		private void UpdateGui_RemoveVehicleOverview(string Id)
		{
			ucVehicleInfoList1.InvokeIfNecessary(() => ucVehicleInfoList1.Remove(Id));
		}

		bool pnlLeftMainDisplay = true;
		int pnlLeftMainWidth = 400;

		#region VehicleManagerProcess
		VehicleManagerProcess mCore;

		private void Constructor_VehicleManagerProcess()
		{
			mCore = new VehicleManagerProcess();
			mCore.VehicleCommunicatorStartListen(8000);
			mCore.CollisionEventDetectorStart();
			mCore.VehicleControlHandlerStart();
			SubscribeEvent_VehicleManagerProcess(mCore);
		}
		private void Destructor_VehicleManagerProcess()
		{
			UnsubscribeEvent_VehicleManagerProcess(mCore);
			mCore.VehicleCommunicatorStopListen();
			mCore.CollisionEventDetectorStop();
			mCore.VehicleControlHandlerStop();
			mCore = null;
		}
		private void SubscribeEvent_VehicleManagerProcess(VehicleManagerProcess VehicleManagerProcess)
		{
			if (VehicleManagerProcess != null)
			{
				VehicleManagerProcess.VehicleInfoManagerVehicleAdded += HandleEvent_VehicleManagerProcessVehicleInfoManagerVehicleAdded;
				VehicleManagerProcess.VehicleInfoManagerVehicleRemoved += HandleEvent_VehicleManagerProcessVehicleInfoManagerVehicleRemoved;
				VehicleManagerProcess.VehicleInfoManagerVehicleStateUpdated += HandleEvent_VehicleManagerProcessVehicleInfoManagerVehicleStateUpdated;
				VehicleManagerProcess.CollisionEventManagerCollisionEventAdded += HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventAdded;
				VehicleManagerProcess.CollisionEventManagerCollisionEventRemoved += HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventRemoved;
				VehicleManagerProcess.CollisionEventManagerCollisionEventStateUpdated += HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventStateUpdated;
			}
		}
		private void UnsubscribeEvent_VehicleManagerProcess(VehicleManagerProcess VehicleManagerProcess)
		{
			if (VehicleManagerProcess != null)
			{
				VehicleManagerProcess.VehicleInfoManagerVehicleAdded -= HandleEvent_VehicleManagerProcessVehicleInfoManagerVehicleAdded;
				VehicleManagerProcess.VehicleInfoManagerVehicleRemoved -= HandleEvent_VehicleManagerProcessVehicleInfoManagerVehicleRemoved;
				VehicleManagerProcess.VehicleInfoManagerVehicleStateUpdated -= HandleEvent_VehicleManagerProcessVehicleInfoManagerVehicleStateUpdated;
				VehicleManagerProcess.CollisionEventManagerCollisionEventAdded -= HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventAdded;
				VehicleManagerProcess.CollisionEventManagerCollisionEventRemoved -= HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventRemoved;
				VehicleManagerProcess.CollisionEventManagerCollisionEventStateUpdated -= HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventStateUpdated;
			}
		}
		private void HandleEvent_VehicleManagerProcessVehicleInfoManagerVehicleAdded(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			RegisterIconId(VehicleInfo);
			UpdateGui_UpdateVehicleNameList();
			UpdateGui_AddVehicleOverview(VehicleInfo.mName, VehicleInfo.mBattery.ToString("F2"), VehicleInfo.mState);
		}
		private void HandleEvent_VehicleManagerProcessVehicleInfoManagerVehicleRemoved(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			EraseIcon(VehicleInfo);
			UpdateGui_UpdateVehicleNameList();
			UpdateGui_RemoveVehicleOverview(VehicleInfo.mName);
		}
		private void HandleEvent_VehicleManagerProcessVehicleInfoManagerVehicleStateUpdated(DateTime OccurTime, string Name, IVehicleInfo VehicleInfo)
		{
			PrintIcon(VehicleInfo);
			UpdateGui_SetVehicleOverview(VehicleInfo.mName, UserControl.UCVehicleInfoList.Property.Battery, VehicleInfo.mBattery.ToString("F2"));
			UpdateGui_SetVehicleOverview(VehicleInfo.mName, UserControl.UCVehicleInfoList.Property.State, VehicleInfo.mState);
		}
		private void HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventAdded(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			RegisterIconId(CollisionPair);
		}
		private void HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventRemoved(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			EraseIcon(CollisionPair);
		}
		private void HandleEvent_VehicleManagerProcessCollisionEventManagerCollisionEventStateUpdated(DateTime OccurTime, string Name, ICollisionPair CollisionPair)
		{
			PrintIcon(CollisionPair);
		}

		private void SendCommandToVehicle(string VehicleName, string Command, params string[] Paras)
		{
			mCore.SendCommand(VehicleName, Command, Paras);
		}
		#endregion

		#region GLUI
		private Dictionary<string, int> mIconIdsOfVehicle = new Dictionary<string, int>();
		private Dictionary<string, int> mIconIdsOfVehiclePath = new Dictionary<string, int>();
		private Dictionary<string, int> mIconIdsOfVehiclePathPoints = new Dictionary<string, int>();
		private Dictionary<string, int> mIconIdsOfCollisionRegion = new Dictionary<string, int>();

		private void gluiCtrl1_LoadMapEvent(object sender, GLUI.LoadMapEventArgs e)
		{

		}
		private void Constructor_GLUI()
		{
			StyleManager.LoadStyle("Style.ini");
		}
		private void Destructor_GLUI()
		{

		}
		/// <summary>註冊圖像 ID</summary>
		private void RegisterIconId(IVehicleInfo VehicleInfo)
		{
			if (VehicleInfo != null && !string.IsNullOrEmpty(VehicleInfo.mName))
			{
				int VehicleIconId = GLCMD.CMD.SerialNumber.Next();
				int VehiclePathIconId = GLCMD.CMD.AddMultiStripLine("Path", null);
				int VehiclePathPointsIconId = GLCMD.CMD.AddMultiPair("PathPoint", null);

				mIconIdsOfVehicle.Add(VehicleInfo.mName, VehicleIconId);
				mIconIdsOfVehiclePath.Add(VehicleInfo.mName, VehiclePathIconId);
				mIconIdsOfVehiclePathPoints.Add(VehicleInfo.mName, VehiclePathPointsIconId);
			}
		}
		/// <summary>把圖像加入至地圖</summary>
		private void PrintIcon(IVehicleInfo VehicleInfo)
		{
			if (VehicleInfo != null && !string.IsNullOrEmpty(VehicleInfo.mName))
			{
				if (VehicleInfo.mPosition != null)
				{
					GLCMD.CMD.AddAGV(mIconIdsOfVehicle[VehicleInfo.mName], VehicleInfo.mName, VehicleInfo.mPosition.mX, VehicleInfo.mPosition.mY, VehicleInfo.mToward);
				}
				if (VehicleInfo.mPosition != null && VehicleInfo.mPath != null)
				{
					GLCMD.CMD.SaftyEditMultiGeometry<IPair>(mIconIdsOfVehiclePath[VehicleInfo.mName], true, (line) =>
					{
						line.Clear();
						line.AddRangeIfNotNull(GetPath(VehicleInfo));
					});
					GLCMD.CMD.SaftyEditMultiGeometry<IPair>(mIconIdsOfVehiclePathPoints[VehicleInfo.mName], true, (line) =>
					{
						line.Clear();
						line.AddRangeIfNotNull(GetPathDetail(VehicleInfo));
					});
				}
			}
		}
		/// <summary>把圖像從地圖中移除</summary>
		private void EraseIcon(IVehicleInfo VehicleInfo)
		{
			if (VehicleInfo != null && !string.IsNullOrEmpty(VehicleInfo.mName))
			{
				GLCMD.CMD.DeleteAGV(mIconIdsOfVehicle[VehicleInfo.mName]);
				GLCMD.CMD.DeleteMulti(mIconIdsOfVehiclePath[VehicleInfo.mName]);
				GLCMD.CMD.DeleteMulti(mIconIdsOfVehiclePathPoints[VehicleInfo.mName]);

				mIconIdsOfVehicle.Remove(VehicleInfo.mName);
				mIconIdsOfVehiclePath.Remove(VehicleInfo.mName);
				mIconIdsOfVehiclePathPoints.Remove(VehicleInfo.mName);
			}
		}
		private void RegisterIconId(ICollisionPair CollisionPair)
		{
			if (CollisionPair != null && CollisionPair.mCollisionRegion != null && !string.IsNullOrEmpty(CollisionPair.mName))
			{
				int id = GLCMD.CMD.AddMultiArea("CollisionArea", GetRegion(CollisionPair));
				mIconIdsOfCollisionRegion.Add(CollisionPair.mName, id);
			}
		}
		private void PrintIcon(ICollisionPair CollisionPair)
		{
			if (CollisionPair != null && CollisionPair.mCollisionRegion != null && !string.IsNullOrEmpty(CollisionPair.mName))
			{
				GLCMD.CMD.SaftyEditMultiGeometry<IArea>(mIconIdsOfCollisionRegion[CollisionPair.mName], true, (area) =>
				{
					area.Clear();
					area.AddRangeIfNotNull(GetRegion(CollisionPair));
				});
			}
		}
		private void EraseIcon(ICollisionPair CollisionPair)
		{
			if (CollisionPair != null && CollisionPair.mCollisionRegion != null && !string.IsNullOrEmpty(CollisionPair.mName))
			{
				GLCMD.CMD.DeleteMulti(mIconIdsOfCollisionRegion[CollisionPair.mName]);
				mIconIdsOfCollisionRegion.Remove(CollisionPair.mName);
			}
		}
		private IEnumerable<IPair> GetPath(IVehicleInfo VehicleInfo)
		{
			List<IPair> result = null;
			if (VehicleInfo.mPath != null && VehicleInfo.mPath.Count() > 0)
			{
				result = new List<IPair>();
				result.Add(new Pair(VehicleInfo.mPosition.mX, VehicleInfo.mPosition.mY));
				for (int i = 0; i < VehicleInfo.mPath.Count(); ++i)
				{
					result.Add(new Pair(VehicleInfo.mPath.ElementAt(i).mX, VehicleInfo.mPath.ElementAt(i).mY));
				}
			}
			return result;
		}
		private IEnumerable<IPair> GetPathDetail(IVehicleInfo VehicleInfo)
		{
			List<IPair> result = null;
			if (VehicleInfo.mPathDetail != null && VehicleInfo.mPathDetail.Count() > 0)
			{
				result = new List<IPair>();
				result.Add(new Pair(VehicleInfo.mPosition.mX, VehicleInfo.mPosition.mY));
				for (int i = 0; i < VehicleInfo.mPathDetail.Count(); ++i)
				{
					result.Add(new Pair(VehicleInfo.mPathDetail.ElementAt(i).mX, VehicleInfo.mPathDetail.ElementAt(i).mY));
				}
			}
			return result;
		}
		private IEnumerable<IArea> GetRegion(ICollisionPair CollisionPair)
		{
			List<IArea> result = null;
			if (CollisionPair != null && CollisionPair.mCollisionRegion != null)
			{
				result = new List<IArea>();
				result.Add(new Area(CollisionPair.mCollisionRegion.mMinX, CollisionPair.mCollisionRegion.mMinY, CollisionPair.mCollisionRegion.mMaxX, CollisionPair.mCollisionRegion.mMaxY));
			}
			return result;
		}
		#endregion
	}
}
