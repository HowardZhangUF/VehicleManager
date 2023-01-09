using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficControlTest.UserControl
{
	public class TextBoxWithHint : TextBox
	{
		public string mHintText { get; private set; }
		private Color mDefaultColor { get; set; }

		public TextBoxWithHint() : base()
		{
			SubscribeEvent();
		}
		public void SetHintText(string HintText)
		{
			mHintText = HintText;
			mDefaultColor = ForeColor;

			ForeColor = Color.Gray;
			Text = mHintText;
		}

		private void SubscribeEvent()
		{
			GotFocus += HandleEvent_GotFocus;
			LostFocus += HandleEvent_LostFocus;
		}
		private void UnsubscribeEvent()
		{
			GotFocus -= HandleEvent_GotFocus;
			LostFocus -= HandleEvent_LostFocus;
		}
		private void HandleEvent_GotFocus(object sender, EventArgs e)
		{
			if (Text == mHintText)
			{
				Text = string.Empty;
				ForeColor = mDefaultColor;
			}
		}
		private void HandleEvent_LostFocus(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(Text) || Text == mHintText)
			{
				ForeColor = Color.Gray;
				Text = mHintText;
			}
		}
	}
}
