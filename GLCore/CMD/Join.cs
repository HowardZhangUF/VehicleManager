using Geometry;
using SharpGL;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GLCore
{
	/// <summary>
	/// 插入工具
	/// </summary>
	public class Join : IGLCore
	{
		/// <summary>
		/// 建構子
		/// </summary>
		public Join(string selectRangeStyleName, string obstaclePointsStyleName)
		{
			SelectRange = new SingleArea(selectRangeStyleName, -5000, -5000, 5000, 5000);
			ObstaclePoints = new MultiPair(obstaclePointsStyleName);
		}

		/// <summary>
		/// 使用選擇範圍
		/// </summary>
		public bool EnableSelectRange { get; set; } = false;

		/// <summary>
		/// 使用中(可見)
		/// </summary>
		public bool InUse { get; private set; }

		/// <summary>
		/// 旋轉
		/// </summary>
		public IAngle Rotate { get; private set; } = new Angle();

		/// <summary>
		/// 選擇範圍
		/// </summary>
		public ISingleArea SelectRange { get; private set; }

		/// <summary>
		/// 平移
		/// </summary>
		public IPair Translate { get; private set; } = new Pair();

		/// <summary>
		/// 插入的障礙點
		/// </summary>
		private IMultiPair ObstaclePoints { get; }

		/// <summary>
		/// 清除所有資料
		/// </summary>
		public void ClearAll()
		{
			EnableSelectRange = false;
			InUse = false;
			Translate = new Pair();
			ObstaclePoints.Geometry.SaftyEdit(true, o => o.Clear());
		}

		/// <summary>
		/// 繪圖
		/// </summary>
		public void Draw(OpenGL gl)
		{
			gl.PushMatrix();
			{
				gl.Translate(Translate.X, Translate.Y, 0);
				gl.Rotate(0, 0, -(float)Rotate.Theta);
				ObstaclePoints.Draw(gl);
			}
			gl.PopMatrix();

			if (EnableSelectRange) SelectRange.Draw(gl);
		}

		/// <summary>
		/// 獲得障礙點
		/// </summary>
		public IEnumerable<IPair> GetObstaclePoints()
		{
			if (!InUse) return null;
			if (EnableSelectRange)
			{
				return ObstaclePoints.Geometry.SaftyEdit(list => list
				.Select(pair => pair.Rotate(Rotate).Add(Translate))
				.Where(pair => SelectRange.Geometry.Contain(pair)));
			}
			else
			{
				return ObstaclePoints.Geometry.SaftyEdit(list => list
				.Select(pair => pair.Rotate(Rotate).Add(Translate)));
			}
		}

		/// <summary>
		/// 從檔案插入障礙點
		/// </summary>
		public void LoadJoinMap(string file)
		{
			ClearAll();
			InUse = true;
			var lines = File.ReadAllLines(file);

			for (int ii = 0; ii < lines.Length; ii++)
			{
				switch (lines[ii])
				{
					case "Goal List":
						break;

					case "Obstacle Points":
						ii = ReadObstaclePoints(lines, ii);
						break;

					case "Minimum Position":
						break;

					case "Maximum Position":
						break;
				}
			}
		}

		/// <summary>
		/// <para>讀取障礙點至  <see cref="ObstaclePoints"/> ，並回傳結束的資料行數</para>
		/// <para>資料格式如：-12794,3803</para>
		/// </summary>
		private int ReadObstaclePoints(string[] lines, int begin)
		{
			var data = new List<IPair>();
			for (int ii = begin + 1; ii < lines.Length; ii++)
			{
				var para = lines[ii].Split(',');
				int x = 0, y = 0;
				if (para.Length == 2 && int.TryParse(para[0], out x) && int.TryParse(para[1], out y))
				{
					data.Add(new Pair(x, y));
					begin = ii;
				}
				else
				{
					break;
				}
			}

			if (data.Count != 0)
			{
				ObstaclePoints.Geometry.SaftyEdit(true, o => o.AddRangeIfNotNull(data));
			}
			return begin;
		}
	}
}