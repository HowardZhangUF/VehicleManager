using Geometry;
using SharpGL;

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
        /// 畫線中(可見)
        /// </summary>
        public bool InUse { get; private set; }

        /// <summary>
        /// 取消
        /// </summary>
        public void Cancel()
        {
            InUse = false;
        }

        /// <summary>
        /// 根據 ID 擦掉障礙點
        /// </summary>
        /// <param name="id">為 <see cref="MultiPair"/> 的識別碼</param>
        public void ClearObstaclePoints(int id)
        {
            if (InUse)
            {
                GLCMD.CMD.SaftyEditMultiGeometry<IPair>(id, true, list => list.RemoveAll(pair => Geometry.Contain(pair)));
            }
        }

        /// <summary>
        /// 設定大小及位置，並設為可見
        /// </summary>
        public void Set(IPair pos, int size)
        {
            Geometry.Set(pos.X - size / 2, pos.Y - size / 2, pos.X + size / 2, pos.Y + size / 2);
            InUse = true;
        }

        /// <summary>
        /// 設定位置，並設為可見
        /// </summary>
        public void Set(IPair pos)
        {
            Move(EMoveType.Center, pos.X, pos.Y);
            InUse = true;
        }
    }
}