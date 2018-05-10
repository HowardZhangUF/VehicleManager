using Geometry;
using GLStyle;

namespace GLCore
{
    /// <summary>
    /// AGV 介面
    /// </summary>
    public interface IAGV : IGLCore, ISingle<ITowardPair, IAGVStyle>
    {
    }
}