using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficControlTest
{
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

		/// <summary>
		/// 擴充功能，使 ComboBox 根據現有的 Items 去調整 DropDownWidth 的數值
		/// </summary>
		public static void AdjustDropDownWidth(this ComboBox ctrl)
		{
			int width = ctrl.Width;
			int tmp = 0;
			foreach (var item in ctrl.Items)
			{
				tmp = TextRenderer.MeasureText(item.ToString(), ctrl.Font).Width;
				if (tmp > width)
				{
					width = tmp;
				}
			}
			ctrl.DropDownWidth = width;
		}
	}
}
