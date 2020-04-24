using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficControlTest.UserControl
{
	public enum SwitchState
	{
		On,
		Off
	}

	public class SwitchStateChangedEventArgs : EventArgs
	{
		public SwitchState SwitchState { get; private set; }

		public SwitchStateChangedEventArgs(SwitchState SwitchState) : base()
		{
			this.SwitchState = SwitchState;
		}
	}

	public class SwitchButton : Control
	{
		public event EventHandler SwitchStateChanged;

		public Color ActiveColor { get; set; } = Color.FromArgb(52, 170, 70);
		public Color InactiveColor { get; set; } = Color.FromArgb(223, 25, 32);
		public Color SliderColor { get; set; } = Color.White;
		public Color TextColor { get; set; } = Color.White;
		public string ActiveText { get; set; } = "ON";
		public string InActiveText { get; set; } = "OFF";
		public SwitchState SwitchState
		{
			get
			{
				return _SwitchState;
			}
			set
			{
				if (_SwitchState != value)
				{
					_SwitchState = value;
					Invalidate();
					Refresh();
					RaiseEvent_SwitchStateChanged(_SwitchState);
				}
			}
		}

		private SwitchState _SwitchState = SwitchState.Off;

		public SwitchButton() : base()
		{
			Font = new Font("Microsoft Sans Serif", Font.Size);
		}

		protected void RaiseEvent_SwitchStateChanged(SwitchState SwitchState)
		{
			SwitchStateChanged?.Invoke(this, new SwitchStateChangedEventArgs(SwitchState));
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			MinimumSize = new Size(75, 24);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			// Draw Background
			Color color = SwitchState == SwitchState.On ? ActiveColor : InactiveColor;
			using (Brush brush = new SolidBrush(color))
			{
				e.Graphics.FillRectangle(brush, e.ClipRectangle);
			}

			// Draw Slider
			using (Brush brush = new SolidBrush(SliderColor))
			{
				int margin = 2;
				if (SwitchState == SwitchState.On)
				{
					e.Graphics.FillRectangle(brush, Width / 2 + margin, margin, Width / 2 - margin * 2, Height - margin * 2);
				}
				else
				{
					e.Graphics.FillRectangle(brush, margin, margin, Width / 2 - margin * 2, Height - margin * 2);
				}
			}

			// Draw Text
			using (Brush brush = new SolidBrush(TextColor))
			{
				if (SwitchState == SwitchState.On)
				{
					Size tmpTextSize = TextRenderer.MeasureText(ActiveText, Font);
					e.Graphics.DrawString(ActiveText, Font, brush, new PointF((Width / 2 - tmpTextSize.Width) / 2, (Height - tmpTextSize.Height) / 2));
				}
				else
				{
					Size tmpTextSize = TextRenderer.MeasureText(InActiveText, Font);
					e.Graphics.DrawString(InActiveText, Font, brush, new PointF(Width / 2 + (Width / 2 - tmpTextSize.Width) / 2, (Height - tmpTextSize.Height) / 2));
				}
			}

			base.OnPaint(e);
		}
	}
}
