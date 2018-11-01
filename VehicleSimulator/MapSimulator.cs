using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	class MapSimulator
	{
		int BoundaryXMin;
		int BoundaryXMax;
		int BoundaryYMin;
		int BoundaryYMax;
		public List<Pair> ObstaclePoints = new List<Pair>();
		public Dictionary<string, TowardPair> Vehilcles = new Dictionary<string, TowardPair>();

		public void LoadMap()
		{

		}
		
		public void ClearMap()
		{

		}

		public void AddObstaclePoints()
		{

		}

		public void RemoveObstaclePoints()
		{

		}

		public void AddVehicle()
		{

		}

		public void RemoveVehicle()
		{

		}

		public void UpdateVehiclePosition()
		{

		}

		public List<Pair> CalculatePath(VehicleSimulator vehicle, Pair dst)
		{
			return null;
		}
	}
}
