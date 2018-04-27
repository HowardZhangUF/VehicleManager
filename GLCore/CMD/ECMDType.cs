namespace GLCore
{
    /// <summary>
    /// 命令種類
    /// </summary>
    public enum ECMDType
    {
        /// <summary>
        /// 移動
        /// </summary>
        Move,

        /// <summary>
        /// 加入
        /// </summary>
        Add,

        /// <summary>
        /// 刪除
        /// </summary>
        Delete,

        /// <summary>
        /// 變更樣式
        /// </summary>
        ChangeStyle,

        /// <summary>
        /// 復原
        /// </summary>
        Undo,

        /// <summary>
        /// 重新命名
        /// </summary>
        Rename,
    }
}