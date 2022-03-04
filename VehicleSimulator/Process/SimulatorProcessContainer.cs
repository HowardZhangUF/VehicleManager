using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	public class SimulatorProcessContainer
	{
		public event EventHandler<SimulatorAddedEventArgs> SimulatorAdded;
		public event EventHandler<SimulatorRemovedEventArgs> SimulatorRemoved;

		private Dictionary<string, SimulatorProcess> mSimulatorProcessCollection = new Dictionary<string, SimulatorProcess>();

		public bool AddSimulatorProcess(string SimulatorName)
		{
			bool successed = false;
			if (!mSimulatorProcessCollection.ContainsKey(SimulatorName))
			{
				mSimulatorProcessCollection.Add(SimulatorName, new SimulatorProcess(SimulatorName));
				mSimulatorProcessCollection[SimulatorName].Start();
				RaiseEvent_SimulatorAdded(SimulatorName, mSimulatorProcessCollection[SimulatorName]);
				successed = true;
			}
			return successed;
		}
		public bool RemoveSimulatorProcess(string SimulatorName)
		{
			bool successed = false;
			if (mSimulatorProcessCollection.ContainsKey(SimulatorName))
			{
				mSimulatorProcessCollection[SimulatorName].Stop();
				RaiseEvent_SimulatorRemoved(SimulatorName, mSimulatorProcessCollection[SimulatorName]);
				mSimulatorProcessCollection.Remove(SimulatorName);
				successed = true;
			}
			return successed;
		}
		public void Clear()
		{
			while (mSimulatorProcessCollection.Any())
			{
				string simulatorName = mSimulatorProcessCollection.Keys.First();
				RemoveSimulatorProcess(simulatorName);
			}
		}
		public SimulatorProcess GetSimulatorProcess(string SimulatorName)
		{
			if (mSimulatorProcessCollection.ContainsKey(SimulatorName))
			{
				return mSimulatorProcessCollection[SimulatorName];
			}
			else
			{
				return null;
			}
		}
		public bool IsSimulatorProcessExist(string SimulatorName)
		{
			return mSimulatorProcessCollection.ContainsKey(SimulatorName);
		}
		public void SetMap(string FilePath, MapData MapData)
		{
			if (System.IO.File.Exists(FilePath) && MapData != null)
			{
				foreach (SimulatorProcess process in mSimulatorProcessCollection.Values)
				{
					process.SetMap(FilePath, MapData);
				}
			}
		}

		protected virtual void RaiseEvent_SimulatorAdded(string SimulatorName, SimulatorProcess SimulatorProcess, bool Sync = true)
		{
			if (Sync)
			{
				SimulatorAdded?.Invoke(this, new SimulatorAddedEventArgs(DateTime.Now, SimulatorName, SimulatorProcess));
			}
			else
			{
				Task.Run(() => { SimulatorAdded?.Invoke(this, new SimulatorAddedEventArgs(DateTime.Now, SimulatorName, SimulatorProcess)); });
			}
		}
		protected virtual void RaiseEvent_SimulatorRemoved(string SimulatorName, SimulatorProcess SimulatorProcess, bool Sync = true)
		{
			if (Sync)
			{
				SimulatorRemoved?.Invoke(this, new SimulatorRemovedEventArgs(DateTime.Now, SimulatorName, SimulatorProcess));
			}
			else
			{
				Task.Run(() => { SimulatorRemoved?.Invoke(this, new SimulatorRemovedEventArgs(DateTime.Now, SimulatorName, SimulatorProcess)); });
			}
		}
	}
}
