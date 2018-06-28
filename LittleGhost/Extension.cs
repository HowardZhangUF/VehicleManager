using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleGhost
{
    /// <summary>
    /// C# 擴充功能
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// 擴充功能，調用控制項，並依照情況調用
        /// </summary>
        public static void InvokeIfNecessary<T>(this T ctrl, Action action) where T : Control
        {
            if (ctrl.InvokeRequired) { ctrl.Invoke(action); }
            else { action(); }
        }

        /// <summary>
        /// 擴充功能，調用控制項，並依照情況調用
        /// </summary>
        public static TResult InvokeIfNecessary<T, TResult>(this T ctrl, Func<T, TResult> action) where T : Control
        {
            if (ctrl.InvokeRequired) { return (TResult)ctrl.Invoke(action, ctrl); }
            else { return action(ctrl); }
        }
    }
}
