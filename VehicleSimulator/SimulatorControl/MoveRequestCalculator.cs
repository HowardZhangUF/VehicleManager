using Module.Map.Map;
using Module.Map.MapFactory;
using Module.Pathfinding.PathFinder;
using Module.Pathfinding.PathOptimizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSimulator
{
	public class MoveRequestCalculator : IMoveRequestCalculator
	{
		private IMap mMap = null;
		private IPathFinderUsingAStar mPathFinderUsingAStar = new PathFinderUsingAStar();
		private IPathOptimizer mPathOptimizer = new PathOptimizer();

		public void SetMap(string FilePath)
		{
			if (!File.Exists(FilePath)) return;

			mMap = MapFactory.CreateMapUsingQuadTree(EMapFileType.iTS, FilePath);
			MapFactory.CalculateInfluenceGrid(mMap, 350, 3, 0.2f);
			mPathFinderUsingAStar.Set(mMap);
			mPathOptimizer.Set(mMap);
		}
		public List<MoveRequest> Calculate(Point Start, string TargetName)
		{
			if (mMap.mGoals.Any(o => o.mName == TargetName))
			{
				var Goal = mMap.mGoals.First(o => o.mName == TargetName);
				if (IsGoalRequestToward(Goal))
				{
					return Calculate(Start, new Point(Goal.mX, Goal.mY), (int)Goal.mToward, IsGoalMoveBackward(Goal));
				}
				else
				{
					return Calculate(Start, new Point(Goal.mX, Goal.mY), IsGoalMoveBackward(Goal));
				}
			}
			else
			{
				return null;
			}
		}
		public List<MoveRequest> Calculate(Point Start, Point End, bool IsMoveBackward = false)
		{
			if (mMap == null)
			{
				return new List<MoveRequest>() { new MoveRequest(End.mX, End.mY, IsMoveBackward) };
			}
			else
			{
				mPathFinderUsingAStar.FindPath(new Module.Map.GeometricShape.Point(Start.mX, Start.mY), new Module.Map.GeometricShape.Point(End.mX, End.mY), 700, 350, out PathFindingResult pathfindingResult, out Module.Pathfinding.Object.Path path);
				if (path == null)
				{
					return null;
				}
				else
				{
					path = mPathOptimizer.OptimizePath(path, 700);
					return Convert(path, IsMoveBackward);
				}
			}
		}
		public List<MoveRequest> Calculate(Point Start, Point End, int EndToward, bool IsMoveBackward = false)
		{
			if (mMap == null)
			{
				return new List<MoveRequest>() { new MoveRequest(End.mX, End.mY, EndToward, IsMoveBackward) };
			}
			else
			{
				mPathFinderUsingAStar.FindPath(new Module.Map.GeometricShape.Point(Start.mX, Start.mY), new Module.Map.GeometricShape.Point(End.mX, End.mY), 700, 350, out PathFindingResult pathfindingResult, out Module.Pathfinding.Object.Path path);
				if (path == null)
				{
					return null;
				}
				else
				{
					path = mPathOptimizer.OptimizePath(path, 700);
					return Convert(path, EndToward, IsMoveBackward);
				}
			}
		}

		private List<MoveRequest> Convert(Module.Pathfinding.Object.Path Path, bool IsMoveBackward = false)
		{
			return Path.mPoints.Skip(1).Select(o => new MoveRequest(o.mX, o.mY, IsMoveBackward)).ToList();
		}
		private List<MoveRequest> Convert(Module.Pathfinding.Object.Path Path, int EndToward, bool IsMoveBackward = false)
		{
			List<MoveRequest> result = new List<MoveRequest>();
			for (int i = 1; i < Path.mPoints.Count - 1; ++i)
			{
				result.Add(new MoveRequest(Path.mPoints[i].mX, Path.mPoints[i].mY, IsMoveBackward));
			}
			result.Add(new MoveRequest(Path.mPoints.Last().mX, Path.mPoints.Last().mY, EndToward, IsMoveBackward));
			return result;
		}

		private static bool IsGoalRequestToward(Module.Map.MapObject.Goal Goal)
		{
			return Goal.mToward > 360 ? false : true;
		}
		private static bool IsGoalMoveBackward(Module.Map.MapObject.Goal Goal)
		{
			return Goal.mStyleName == "MovementInverse";
		}
	}
}
