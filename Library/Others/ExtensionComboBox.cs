using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryForVM
{
	public static class ExtensionComboBox
	{
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
