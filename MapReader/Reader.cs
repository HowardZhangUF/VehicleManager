using Geometry;
using MD5Hash;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MapReader
{
	/// <summary>
	/// 地圖讀取器
	/// </summary>
	public class Reader : IMapReader
    {
        /// <summary>
        /// <para>讀取目標點列表</para>
        /// <para>資料格式如：Goal 2,8421,2264,0,MagneticTracking</para>
        /// </summary>
        private IEnumerable<Goal> ReadGoalList(IEnumerable<string> collection)
        {
            foreach (var data in collection)
            {
                var para = data.Split(',');

                int x = 0, y = 0;
                double toward = 0.0f;
                if (para.Length == 5 && int.TryParse(para[1], out x) && int.TryParse(para[2], out y) && double.TryParse(para[3], out toward))
                {
                    yield return new Goal()
                    {
                        Name = para[0],
                        TowardPair = new TowardPair(x, y, toward),
                        TypeName = para[4],
                    };
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// <para>讀取障礙點</para>
        /// <para>資料格式如：-12794,3803</para>
        /// </summary>
        private IEnumerable<IPair> ReadObstaclePointsList(IEnumerable<string> collection)
        {
            foreach (var data in collection)
            {
                var para = data.Split(',');

                int x = 0, y = 0;
                if (para.Length == 2 && int.TryParse(para[0], out x) && int.TryParse(para[1], out y))
                {
                    yield return new Pair()
                    {
                        X = x,
                        Y = y,
                    };
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 建立地圖讀取器，並且載入地圖
        /// </summary>
        public Reader(string path)
        {
            MapHash = MD5.GetFileHash(path);
            var lines = File.ReadAllLines(path);
            GoalList = ReadGoalList(lines.SkipWhile((line) => line != "Goal List").Skip(1));
            ObstaclePointsList = ReadObstaclePointsList(lines.SkipWhile((line) => line != "Obstacle Points").Skip(1));
        }

        /// <summary>
        /// 目標點列表
        /// </summary>
        public IEnumerable<Goal> GoalList { get; } = null;

        /// <summary>
        /// 地圖檔雜湊值
        /// </summary>
        public string MapHash { get; } = string.Empty;

        /// <summary>
        /// 最大座標
        /// </summary>
        public IPair MaximumPosition { get; private set; } = null;

        /// <summary>
        /// 最小座標
        /// </summary>
        public IPair MinimumPosition { get; private set; } = null;

        /// <summary>
        /// 障礙點
        /// </summary>
        public IEnumerable<IPair> ObstaclePointsList { get; } = null;
    }
}
