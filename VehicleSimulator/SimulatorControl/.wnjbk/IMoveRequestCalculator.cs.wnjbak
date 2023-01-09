using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	public interface IMoveRequestCalculator
	{
		void SetMap(string FilePath);

		List<MoveRequest> Calculate(Point Start, string TargetName);
		List<MoveRequest> Calculate(Point Start, Point End, bool IsMoveBackward = false);
		List<MoveRequest> Calculate(Point Start, Point End, int EndToward, bool IsMoveBackward = false);
	}
}
