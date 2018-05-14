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
        public static void InvokeIfNecessary(this Control ctrl, Action action)
        {
            if (ctrl.InvokeRequired) { ctrl.Invoke(action); }
            else { action(); }
        }
    }
}
