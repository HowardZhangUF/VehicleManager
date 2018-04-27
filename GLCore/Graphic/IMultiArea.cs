using Geometry;
using GLStyle;

namespace GLCore
{
    /// <summary>
    /// 複合面
    /// </summary>
    public interface IMultiArea : IGLCore, IMulti<IArea, IAreaStyle>
    {
    }
}