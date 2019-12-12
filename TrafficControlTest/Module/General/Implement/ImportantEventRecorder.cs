using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrafficControlTest.Interface;
using TrafficControlTest.Module.General.Interface;
using TrafficControlTest.Module.MissionManager.Interface;
using static TrafficControlTest.Library.DatabaseAdapter;

namespace TrafficControlTest.Module.General.Implement
{
	public class ImportantEventRecorder : IImportantEventRecorder
	{
		public event EventHandlerDateTime SystemStarted;
		public event EventHandlerDateTime SystemStopped;

		public bool mIsExecuting
		{
			get
			{
				return _IsExecuting;
			}
			private set
			{
				_IsExecuting = value;
				if (_IsExecuting) RaiseEvent_SystemStarted();
				else RaiseEvent_SystemStopped();
			}
		}
		public int mPeriodOfTask { get; set; } = 250;
		public int mPeriodOfRecordingHistoryVehicleInfo { get; set; } = 3000;

		private IEventRecorder rEventRecorder = null;
		private IVehicleInfoManager rVehicleInfoManager = null;
		private IMissionStateManager rMissionStateManager = null;
		private Thread mThdRecordHistoryVehicleInfo = null;
		private bool[] mThdRecordHistoryVehicleInfoExitFlag = null;
		private bool _IsExecuting = false;

		public ImportantEventRecorder(IEventRecorder EventRecorder, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager)
		{
			Set(EventRecorder, VehicleInfoManager, MissionStateManager);
		}
		public void Set(IEventRecorder EventRecorder)
		{
			rEventRecorder = EventRecorder;
		}
		public void Set(IVehicleInfoManager VehicleInfoManager)
		{
			UnsubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
			rVehicleInfoManager = VehicleInfoManager;
			SubscribeEvent_IVehicleInfoManager(rVehicleInfoManager);
		}
		public void Set(IMissionStateManager MissionStateManager)
		{
			UnsubscribeEvent_IMissionStateManager(rMissionStateManager);
			rMissionStateManager = MissionStateManager;
			SubscribeEvent_IMissionStateManager(rMissionStateManager);
		}
		public void Set(IEventRecorder EventRecorder, IVehicleInfoManager VehicleInfoManager, IMissionStateManager MissionStateManager)
		{
			Set(EventRecorder);
			Set(VehicleInfoManager);
			Set(MissionStateManager);
		}
		public void Start()
		{
			InitializeThread();
		}
		public void Stop()
		{
			DestroyThread();
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
		private void UnsubscribeEvent_IVehicleInfoManager(IVehicleInfoManager VehicleInfoManager)
		{
			if (VehicleInfoManager != null)
			{
				VehicleInfoManager.ItemAdded -= HandleEvent_VehicleInfoManagerItemAdded;
				VehicleInfoManager.ItemRemoved -= HandleEvent_VehicleInfoManagerItemRemoved;
				VehicleInfoManager.ItemUpdated -= HandleEvent_VehicleInfoManagerItemUpdated;
			}
		}
		private void SubscribeEvent_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemAdded += HandleEvent_MissionStateManagerItemAdded;
				MissionStateManager.ItemUpdated += HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		private void UnsubscribeEvent_IMissionStateManager(IMissionStateManager MissionStateManager)
		{
			if (MissionStateManager != null)
			{
				MissionStateManager.ItemAdded -= HandleEvent_MissionStateManagerItemAdded;
				MissionStateManager.ItemUpdated -= HandleEvent_MissionStateManagerItemUpdated;
			}
		}
		protected virtual void RaiseEvent_SystemStarted(bool Sync = true)
		{
			if (Sync)
			{
				SystemStarted?.Invoke(DateTime.Now);
			}
			else
			{
				Task.Run(() => { SystemStarted?.Invoke(DateTime.Now); });
			}
		}
		protected virtual void RaiseEvent_SystemStopped(bool Sync = true)
		{
			if (Sync)
			{
				SystemStopped?.Invoke(DateTime.Now);
			}
			else
			{
				Task.Run(() => { SystemStopped?.Invoke(DateTime.Now); });
			}
		}
		private void HandleEvent_VehicleInfoManagerItemAdded(DateTime OccurTime, string Name, IVehicleInfo Item)
		{
			rEventRecorder.RecordVehicleInfo(DatabaseDataOperation.Add, Item);
			rEventRecorder.CreateTableOfHistoryVehicleInfo(Name);
		}
		private void HandleEvent_VehicleInfoManagerItemRemoved(DateTime OccurTime, string Name, IVehicleInfo Item)
		{
			rEventRecorder.RecordVehicleInfo(DatabaseDataOperation.Remove, Item);
		}
		private void HandleEvent_VehicleInfoManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IVehicleInfo Item)
		{
			rEventRecorder.RecordVehicleInfo(DatabaseDataOperation.Update, Item);
		}
		private void HandleEvent_MissionStateManagerItemAdded(DateTime OccurTime, string Name, IMissionState Item)
		{
			rEventRecorder.RecordMissionState(DatabaseDataOperation.Add, Item);
		}
		private void HandleEvent_MissionStateManagerItemUpdated(DateTime OccurTime, string Name, string StateName, IMissionState Item)
		{
			rEventRecorder.RecordMissionState(DatabaseDataOperation.Update, Item);
		}
		private void InitializeThread()
		{
			mThdRecordHistoryVehicleInfoExitFlag = new bool[] { false };
			mThdRecordHistoryVehicleInfo = new Thread(() => Task_RecordHistoryVehicleInfo(mThdRecordHistoryVehicleInfoExitFlag));
			mThdRecordHistoryVehicleInfo.IsBackground = true;
			mThdRecordHistoryVehicleInfo.Start();
		}
		private void DestroyThread()
		{
			if (mThdRecordHistoryVehicleInfo != null)
			{
				if (mThdRecordHistoryVehicleInfo.IsAlive)
				{
					mThdRecordHistoryVehicleInfoExitFlag[0] = true;
				}
				mThdRecordHistoryVehicleInfo = null;
			}
		}
		private void Task_RecordHistoryVehicleInfo(bool[] ExitFlag)
		{
			try
			{
				mIsExecuting = true;
				Subtask_RecordHistoryVehicleInfo();
				DateTime lastRecordTimestamp = DateTime.Now;
				while (!ExitFlag[0])
				{
					if (DateTime.Now.Subtract(lastRecordTimestamp).TotalMilliseconds > mPeriodOfRecordingHistoryVehicleInfo)
					{
						Subtask_RecordHistoryVehicleInfo();
						lastRecordTimestamp = DateTime.Now;
					}
					Thread.Sleep(mPeriodOfTask);
				}
			}
			finally
			{
				mIsExecuting = false;
			}
		}
		private void Subtask_RecordHistoryVehicleInfo()
		{
			List<IVehicleInfo> tmpDatas = rVehicleInfoManager.GetItems().ToList();
			DateTime tmpTimestamp = DateTime.Now;
			for (int i = 0; i < tmpDatas.Count; ++i)
			{
				rEventRecorder.RecordHistoryVehicleInfo(DatabaseDataOperation.Add, tmpTimestamp.AddMilliseconds(i), tmpDatas[i]);
			}
		}
	}
}
