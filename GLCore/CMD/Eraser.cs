using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLCore
{
    /// <summary>
    /// 擦子類別
    /// </summary>
    public class Eraser : SingleArea, IGLCore
    {
        /// <summary>
        /// 建構新擦子
        /// </summary>
        public Eraser(string styleName) : base(styleName)
        {
        }

        /// <summary>
        /// 大小
        /// </summary>
        public int Size { get { return Geometry.Max.X - Geometry.Min.X; } }

        /// <summary>
        /// 根據 ID 擦掉障礙點
        /// </summary>
        /// <param name="id">為 <see cref="MultiPair"/> 的識別碼</param>
        public void ClearObstaclePoints(int id)
        {
            GLCMD.SaftyEditMultiGeometry<IPair>(id, true, list => list.RemoveAll(pair => Geometry.Contain(pair)));
        }

        /// <summary>
        /// 設定大小及位置
        /// </summary>
        public void Set(IPair pos, int size)
        {
            Geometry.Set(pos.X - size / 2, pos.Y - size / 2, pos.X + size / 2, pos.Y + size / 2);
        }

        /// <summary>
        /// 設定位置
        /// </summary>
        public void Set(IPair pos)
        {
            Move(EMoveType.Center, pos.X, pos.Y);
        }
    }
}
