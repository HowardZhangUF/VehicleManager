using LibraryForVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator.New
{
	public interface ISimulatorControl : ISystemWithLoopTask
	{
		void Set(ISimulatorInfo ISimulatorInfo);
		void StartMove(int X, int Y);
		void StartMove(int X, int Y, int Toward);
		void StartMove(string Target, int X, int Y);
		void StartMove(string Target, int X, int Y, int Toward);
		void StartMove(string Target, List<MoveRequest> MoveRequests);
		void StopMove();
		void PauseMove();
		void ResumeMove();
	}
}
