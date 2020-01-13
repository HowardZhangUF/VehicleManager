using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficControlTest.UserControl
{
	public class DateTimePickerColorful : DateTimePicker
	{
		public Color BackgroundColor { get; set; } = Color.Black;
		public Color ForeTextColor { get; set; } = Color.White;
		public Color BorderColor { get; set; } = Color.Gray;

		public DateTimePickerColorful() : base()
		{
			SetStyle(ControlStyles.UserPaint, true);
			ValueChanged += (sender, e) => { Refresh(); };
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			using (Brush brush = new SolidBrush(BackgroundColor))
			{
				e.Graphics.FillRectangle(brush, 0, 0, Width, Height);
			}
			using (Pen pen = new Pen(BorderColor))
			{
				e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
			}
			TextRenderer.DrawText(e.Graphics, Value.ToString("yyyy / MM / dd"), Font, System.Drawing.Rectangle.FromLTRB(0, 0, Width - Height, Height), ForeTextColor);
			ComboBoxRenderer.DrawDropDownButton(e.Graphics, System.Drawing.Rectangle.FromLTRB(Width - (int)(Height * 1), 0, Width, Height), System.Windows.Forms.VisualStyles.ComboBoxState.Normal);
		}
	}
}
