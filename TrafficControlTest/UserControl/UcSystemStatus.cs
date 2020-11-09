using System;
using System.Collections.Generic;
using System.Drawing;
using TrafficControlTest.Module.CycleMission;
using TrafficControlTest.Library;
using TrafficControlTest.Module.Log;
using TrafficControlTest.Module.CollisionEvent;
using TrafficControlTest.Module.InterveneCommand;
using TrafficControlTest.Module.Mission;
using TrafficControlTest.Module.CommunicationVehicle;
using TrafficControlTest.Module.CommunicationHost;
using TrafficControlTest.Module.General;
using System.Windows.Forms;
using TrafficControlTest.Module.Map;

namespace TrafficControlTest.UserControl
{
	public partial class UcSystemStatus : System.Windows.Forms.UserControl
	{
		private IImportantEventRecorder rImportantEventRecorder = null;
		private IVehicleCommunicator rVehicleCommunicator = null;
		private ICollisionEventDetector rCollisionEventDetector = null;
		private IVehicleControlHandler rVehicleControlHandler = null;
		private IHostCommunicator rHostCommunicator = null;
		private IMissionDispatcher rMissionDispatcher = null;
		private IMissionUpdater rMissionUpdater = null;
		private ICycleMissionGenerator rCycleMissionGenerator = null;
		private ILogExporter rLogExporter = null;
		private IMapManager rMapManager = null;

		public UcSystemStatus()
		{
			InitializeComponent();
		}
		public void Set(IImportantEventRecorder ImportantEventRecorder)
		{
			UnsubscribeEvent_IImportantEventRecorder(rImportantEventRecorder);
			rImportantEventRecorder = ImportantEventRecorder;
			SubscribeEvent_IImportantEventRecorder(rImportantEventRecorder);
		}
		public void Set(IVehicleCommunicator VehicleCommunicator)
		{
			UnsubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
			rVehicleCommunicator = VehicleCommunicator;
			SubscribeEvent_IVehicleCommunicator(rVehicleCommunicator);
		}
		public void Set(ICollisionEventDetector CollisionEventDetector)
		{
			UnsubscribeEvent_ICollisionEventDetector(rCollisionEventDetector);
			rCollisionEventDetector = CollisionEventDetector;
			SubscribeEvent_ICollisionEventDetector(rCollisionEventDetector);
		}
		public void Set(IVehicleControlHandler VehicleControlHandler)
		{
			UnsubscribeEvent_IVehicleControlHandler(rVehicleControlHandler);
			rVehicleControlHandler = VehicleControlHandler;
			SubscribeEvent_IVehicleControlHandler(rVehicleControlHandler);
		}
		public void Set(IHostCommunicator HostCommunicator)
		{
			UnsubscribeEvent_IHostCommunicator(rHostCommunicator);
			rHostCommunicator = HostCommunicator;
			SubscribeEvent_IHostCommunicator(rHostCommunicator);
		}
		public void Set(IMissionDispatcher MissionDispatcher)
		{
			UnsubscribeEvent_IMissionDispatcher(rMissionDispatcher);
			rMissionDispatcher = MissionDispatcher;
			SubscribeEvent_IMissionDispatcher(rMissionDispatcher);
		}
		public void Set(IMissionUpdater MissionUpdater)
		{
			UnsubscribeEvent_IMissionUpdater(rMissionUpdater);
			rMissionUpdater = MissionUpdater;
			SubscribeEvent_IMissionUpdater(rMissionUpdater);
		}
		public void Set(ICycleMissionGenerator CycleMissionGenerator)
		{
			UnsubscribeEvent_ICycleMissionGenerator(rCycleMissionGenerator);
			rCycleMissionGenerator = CycleMissionGenerator;
			SubscribeEvent_ICycleMissionGenerator(rCycleMissionGenerator);
		}
		public void Set(ILogExporter LogExporter)
		{
			UnsubscribeEvent_ILogExporter(rLogExporter);
			rLogExporter = LogExporter;
			SubscribeEvent_ILogExporter(rLogExporter);
		}
		public void Set(IMapManager MapManager)
		{
			UnsubscribeEvent_IMapManager(rMapManager);
			rMapManager = MapManager;
			SubscribeEvent_IMapManager(rMapManager);
		}
		public void Set(IImportantEventRecorder ImportantEventRecorder, IVehicleCommunicator VehicleCommunicator, ICollisionEventDetector CollisionEventDetector, IVehicleControlHandler VehicleControlHandler, IHostCommunicator HostCommunicator, IMissionDispatcher MissionDispatcher, IMissionUpdater MissionUpdater, ICycleMissionGenerator CycleMissionGenerator, ILogExporter LogExporter, IMapManager MapManager)
		{
			Set(ImportantEventRecorder);
			Set(VehicleCommunicator);
			Set(CollisionEventDetector);
			Set(VehicleControlHandler);
			Set(HostCommunicator);
			Set(MissionDispatcher);
			Set(MissionUpdater);
			Set(CycleMissionGenerator);
			Set(LogExporter);
			Set(MapManager);
		}
		public new void BringToFront()
		{
			UpdateGui_UpdatePanelEnable(false);
			base.BringToFront();
		}

		private void SubscribeEvent_IImportantEventRecorder(IImportantEventRecorder ImportantEventRecorder)
		{
			if (ImportantEventRecorder != null)
			{
				ImportantEventRecorder.SystemStatusChanged += HandleEvent_ImportantEventRecorderSystemStatusChanged;
			}
		}
		private void UnsubscribeEvent_IImportantEventRecorder(IImportantEventRecorder ImportantEventRecorder)
		{
			if (ImportantEventRecorder != null)
			{
				ImportantEventRecorder.SystemStatusChanged -= HandleEvent_ImportantEventRecorderSystemStatusChanged;
			}
		}
		private void SubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.LocalListenStateChanged += HandleEvent_VehicleCommunicatorLocalListenStateChanged;
				VehicleCommunicator.SystemStatusChanged += HandleEvent_VehicleCommunicatorSystemStatusChanged;
			}
		}
		private void UnsubscribeEvent_IVehicleCommunicator(IVehicleCommunicator VehicleCommunicator)
		{
			if (VehicleCommunicator != null)
			{
				VehicleCommunicator.LocalListenStateChanged -= HandleEvent_VehicleCommunicatorLocalListenStateChanged;
				VehicleCommunicator.SystemStatusChanged -= HandleEvent_VehicleCommunicatorSystemStatusChanged;
			}
		}
		private void SubscribeEvent_ICollisionEventDetector(ICollisionEventDetector CollisionEventDetector)
		{
			if (CollisionEventDetector != null)
			{
				CollisionEventDetector.SystemStatusChanged += HandleEvent_CollisionEventDetectorSystemStatusChanged;
			}
		}
		private void UnsubscribeEvent_ICollisionEventDetector(ICollisionEventDetector CollisionEventDetector)
		{
			if (CollisionEventDetector != null)
			{
				CollisionEventDetector.SystemStatusChanged -= HandleEvent_CollisionEventDetectorSystemStatusChanged;
			}
		}
		private void SubscribeEvent_IVehicleControlHandler(IVehicleControlHandler VehicleControlHandler)
		{
			if (VehicleControlHandler != null)
			{
				VehicleControlHandler.SystemStatusChanged += HandleEvent_VehicleControlHandlerSystemStatusChanged;
			}
		}
		private void UnsubscribeEvent_IVehicleControlHandler(IVehicleControlHandler VehicleControlHandler)
		{
			if (VehicleControlHandler != null)
			{
				VehicleControlHandler.SystemStatusChanged -= HandleEvent_VehicleControlHandlerSystemStatusChanged;
			}
		}
		private void SubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.LocalListenStateChanged += HandleEvent_HostCommunicatorLocalListenStateChanged;
				HostCommunicator.SystemStatusChanged += HandleEvent_HostCommunicatorSystemStatusChanged;
			}
		}
		private void UnsubscribeEvent_IHostCommunicator(IHostCommunicator HostCommunicator)
		{
			if (HostCommunicator != null)
			{
				HostCommunicator.LocalListenStateChanged -= HandleEvent_HostCommunicatorLocalListenStateChanged;
				HostCommunicator.SystemStatusChanged -= HandleEvent_HostCommunicatorSystemStatusChanged;
			}
		}
		private void SubscribeEvent_IMissionDispatcher(IMissionDispatcher MissionDispatcher)
		{
			if (MissionDispatcher != null)
			{
				MissionDispatcher.SystemStatusChanged += HandleEvent_MissionDispatcherSystemStatusChanged;
			}
		}
		private void UnsubscribeEvent_IMissionDispatcher(IMissionDispatcher MissionDispatcher)
		{
			if (MissionDispatcher != null)
			{
				MissionDispatcher.SystemStatusChanged -= HandleEvent_MissionDispatcherSystemStatusChanged;
			}
		}
		private void SubscribeEvent_IMissionUpdater(IMissionUpdater MissionUpdater)
		{
			if (MissionUpdater != null)
			{
				MissionUpdater.SystemStatusChanged += HandleEvent_MissionUpdaterSystemStatusChanged;
			}
		}
		private void UnsubscribeEvent_IMissionUpdater(IMissionUpdater MissionUpdater)
		{
			if (MissionUpdater != null)
			{
				MissionUpdater.SystemStatusChanged -= HandleEvent_MissionUpdaterSystemStatusChanged;
			}
		}
		private void SubscribeEvent_ICycleMissionGenerator(ICycleMissionGenerator CycleMissionGenerator)
		{
			if (CycleMissionGenerator != null)
			{
				CycleMissionGenerator.SystemStatusChanged += HandleEvent_CycleMissionGeneratorSystemStatusChanged;
			}
		}
		private void UnsubscribeEvent_ICycleMissionGenerator(ICycleMissionGenerator CycleMissionGenerator)
		{
			if (CycleMissionGenerator != null)
			{
				CycleMissionGenerator.SystemStatusChanged -= HandleEvent_CycleMissionGeneratorSystemStatusChanged;
			}
		}
		private void SubscribeEvent_ILogExporter(ILogExporter LogExporter)
		{
			if (LogExporter != null)
			{
				LogExporter.ExportStarted += HandleEvent_LogExporterExportStarted;
				LogExporter.ExportCompleted += HandleEvent_LogExporterExportCompleted;
			}
		}
		private void UnsubscribeEvent_ILogExporter(ILogExporter LogExporter)
		{
			if (LogExporter != null)
			{
				LogExporter.ExportStarted -= HandleEvent_LogExporterExportStarted;
				LogExporter.ExportCompleted -= HandleEvent_LogExporterExportCompleted;
			}
		}
		private void SubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				// do nothing
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				// do nothing
			}
		}
		private void HandleEvent_ImportantEventRecorderSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnImportantEventRecorder, Args.SystemNewStatus ? SwitchState.On : SwitchState.Off);
		}
		private void HandleEvent_VehicleCommunicatorLocalListenStateChanged(object Sender, Module.CommunicationVehicle.LocalListenStateChangedEventArgs Args)
		{
			if (Args.NewState == ListenState.Listening)
			{
				UpdateGui_UpdateSwitchButtonSwitchState(sbtnVehicleCommunicatorServer, SwitchState.On);
			}
			else
			{
				UpdateGui_UpdateSwitchButtonSwitchState(sbtnVehicleCommunicatorServer, SwitchState.Off);
			}
		}
		private void HandleEvent_VehicleCommunicatorSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnVehicleCommunicator, Args.SystemNewStatus ? SwitchState.On : SwitchState.Off);
		}
		private void HandleEvent_CollisionEventDetectorSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnCollisionEventDetector, Args.SystemNewStatus ? SwitchState.On : SwitchState.Off);
		}
		private void HandleEvent_VehicleControlHandlerSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnVehicleControlHandler, Args.SystemNewStatus ? SwitchState.On : SwitchState.Off);
		}
		private void HandleEvent_HostCommunicatorLocalListenStateChanged(object Sender, Module.CommunicationHost.LocalListenStateChangedEventArgs Args)
		{
			if (Args.NewState == ListenState.Listening)
			{
				UpdateGui_UpdateSwitchButtonSwitchState(sbtnHostCommunicatorServer, SwitchState.On);
			}
			else
			{
				UpdateGui_UpdateSwitchButtonSwitchState(sbtnHostCommunicatorServer, SwitchState.Off);
			}
		}
		private void HandleEvent_HostCommunicatorSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnHostCommunicator, Args.SystemNewStatus ? SwitchState.On : SwitchState.Off);
		}
		private void HandleEvent_MissionDispatcherSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnMissionDispatcher, Args.SystemNewStatus ? SwitchState.On : SwitchState.Off);
		}
		private void HandleEvent_MissionUpdaterSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnMissionUpdater, Args.SystemNewStatus ? SwitchState.On : SwitchState.Off);
		}
		private void HandleEvent_CycleMissionGeneratorSystemStatusChanged(object Sender, SystemStatusChangedEventArgs Args)
		{
			UpdateGui_UpdateSwitchButtonSwitchState(sbtnCycleMissionGenerator, Args.SystemNewStatus ? SwitchState.On : SwitchState.Off);
		}
		private void HandleEvent_LogExporterExportStarted(object Sender, LogExportedEventArgs Args)
		{
			btnExportLog.InvokeIfNecessary(() =>
			{
				btnExportLog.Enabled = false;
			});
		}
		private void HandleEvent_LogExporterExportCompleted(object Sender, LogExportedEventArgs Args)
		{
			btnExportLog.InvokeIfNecessary(() =>
			{
				btnExportLog.Enabled = true;
			});
		}
		private void UpdateGui_UpdatePanelEnable(bool Enable)
		{
			btnLockPanel.InvokeIfNecessary(() =>
			{
				if (Enable)
				{
					btnLockPanel.Text = "Unlocked";
					btnLockPanel.BackColor = Color.FromArgb(52, 170, 70);
					btnLockPanel.ForeColor = Color.Black;
					tableLayoutPanel1.Enabled = true;
					//tableLayoutPanel1.BackColor = Color.FromArgb(53, 53, 53);
					//tableLayoutPanel2.BackColor = Color.FromArgb(53, 53, 53);
				}
				else
				{
					btnLockPanel.Text = "Locked";
					btnLockPanel.BackColor = BackColor;
					btnLockPanel.ForeColor = ForeColor;
					tableLayoutPanel1.Enabled = false;
					btnLockPanel.Enabled = true;
					//tableLayoutPanel1.BackColor = Color.FromArgb(202, 202, 202);
					//tableLayoutPanel2.BackColor = Color.FromArgb(202, 202, 202);
				}
			});
		}
		private void UpdateGui_UpdateSwitchButtonSwitchState(SwitchButton SwitchButton, SwitchState SwitchState)
		{
			SwitchButton.InvokeIfNecessary(() =>
			{
				SwitchButton.SwitchState = SwitchState;
			});
		}
		private void btnLockPanel_Click(object sender, EventArgs e)
		{
			if (btnLockPanel.Text == "Locked")
			{
				UpdateGui_UpdatePanelEnable(true);
			}
			else
			{
				UpdateGui_UpdatePanelEnable(false);
			}
		}
		private void btnExportLog_Click(object sender, EventArgs e)
		{
			if (!rLogExporter.mIsExporting)
			{
				rLogExporter.StartExport();
			}
		}
		private void btnLoadMap_Click(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog())
			{
				ofd.Title = "Choose iTS Map File";
				ofd.Filter = "map files (*.map)|*.map";
				ofd.Multiselect = false;
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					rMapManager.LoadMap(ofd.FileName);
				}
			}
		}
		private void sbtnVehicleCommunicatorServer_DoubleClick(object sender, EventArgs e)
		{
			if (sbtnVehicleCommunicatorServer.SwitchState == SwitchState.On)
			{
				rVehicleCommunicator.StopListen();
			}
			else
			{
				rVehicleCommunicator.StartListen();
			}
		}
		private void sbtnHostCommunicatorServer_DoubleClick(object sender, EventArgs e)
		{
			if (sbtnHostCommunicatorServer.SwitchState == SwitchState.On)
			{
				rHostCommunicator.StopListen();
			}
			else
			{
				rHostCommunicator.StartListen();
			}
		}
		private void sbtnImportantEventRecorder_DoubleClick(object sender, EventArgs e)
		{
			if (sbtnImportantEventRecorder.SwitchState == SwitchState.On)
			{
				rImportantEventRecorder.Stop();
			}
			else
			{
				rImportantEventRecorder.Start();
			}
		}
		private void sbtnVehicleCommunicator_DoubleClick(object sender, EventArgs e)
		{
			// 停用，此與 VehicleCommunicator 連動
		}
		private void sbtnCollisionEventDetector_DoubleClick(object sender, EventArgs e)
		{
			if (sbtnCollisionEventDetector.SwitchState == SwitchState.On)
			{
				rCollisionEventDetector.Stop();
			}
			else
			{
				rCollisionEventDetector.Start();
			}
		}
		private void sbtnVehicleControlHandler_DoubleClick(object sender, EventArgs e)
		{
			if (sbtnVehicleControlHandler.SwitchState == SwitchState.On)
			{
				rVehicleControlHandler.Stop();
			}
			else
			{
				rVehicleControlHandler.Start();
			}
		}
		private void sbtnHostCommunicator_DoubleClick(object sender, EventArgs e)
		{
			// 停用，此與 HostCommunicatorServer 連動
		}
		private void sbtnMissionDispatcher_DoubleClick(object sender, EventArgs e)
		{
			if (sbtnMissionDispatcher.SwitchState == SwitchState.On)
			{
				rMissionDispatcher.Stop();
			}
			else
			{
				rMissionDispatcher.Start();
			}
		}
		private void sbtnMissionUpdater_DoubleClick(object sender, EventArgs e)
		{
			if (sbtnMissionUpdater.SwitchState == SwitchState.On)
			{
				rMissionUpdater.Stop();
			}
			else
			{
				rMissionUpdater.Start();
			}
		}
		private void sbtnCycleMissionGenerator_DoubleClick(object sender, EventArgs e)
		{
			if (sbtnCycleMissionGenerator.SwitchState == SwitchState.On)
			{
				rCycleMissionGenerator.Stop();
			}
			else
			{
				rCycleMissionGenerator.Start();
			}
		}
	}
}
