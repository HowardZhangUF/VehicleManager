namespace GLCore
{
    /// <summary>
    /// 可拖曳的
    /// </summary>
    public interface IDragable : IGLCore
    {
        /// <summary>
        /// 是否可拖曳
        /// </summary>
        bool CanDrag { get; }

        /// <summary>
        /// 移動物件，若無法移動則回傳 false
        /// </summary>
        /// <param name="ctrl">控制點，根據不同種類的物件有所不同</param>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        bool Move(EMoveType ctrl, int x, int y);
    }
}