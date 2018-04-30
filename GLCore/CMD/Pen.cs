using Geometry;
using SharpGL;

namespace GLCore
{
    /// <summary>
    /// 畫筆
    /// </summary>
    public class Pen : SingleLine, IGLCore
    {
        /// <summary>
        /// 建構畫筆
        /// </summary>
        public Pen(string styleName) : base(styleName)
        {
        }

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
        /// 完成，並將線段加入障礙點集合中
        /// </summary>
        /// <param name="id">為 <see cref="MultiPair"/> 的識別碼</param>
        public void Finish(int id)
        {
            if (InUse)
            {
                InUse = false;
                GLCMD.SaftyEditMultiGeometry<IPair>(id, true, list =>
                {
                    list.AddRange(Geometry.ToPairs());
                });
            }
        }

        /// <summary>
        /// 設定起點和終點，並將畫筆設為可見
        /// </summary>
        /// <param name="pos"></param>
        public void SetBeginAndEnd(IPair pos)
        {
            Geometry.Begin = new Pair(pos);
            Geometry.End = new Pair(pos);
            InUse = true;
        }

        /// <summary>
        /// 設定畫筆終點，並將畫筆設為可見
        /// </summary>
        /// <param name="pos"></param>
        public void SetEnd(IPair pos)
        {
            Geometry.End = new Pair(pos);
            InUse = true;
        }
    }
}