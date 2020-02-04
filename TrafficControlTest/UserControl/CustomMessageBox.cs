using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficControlTest.UserControl
{
	public class CustomMessageBox
	{
		private static int DEFAULT_XBORDER = 30; // 控制項與邊界的距離
		private static int DEFAULT_YBORDER = 30; // 控制項與邊界的距離
		private static int DEFAULT_MARGIN = 15; // 每個控制項之間的距離
		private static int DEFAULT_TEXT_FONT_SIZE = 12;
		private static FontFamily DEFAULT_TEXT_FONT_FAMILY = new FontFamily("新細明體");
		private static Color DEFAULT_FORM_BACKCOLOR = Color.FromArgb(5, 25, 30);
		private static Color DEFAULT_FORM_FORECOLOR = Color.White;
		private static Color DEFAULT_FORM_BORDERCOLOR = Color.Red;
		private static Size DEFAULT_BUTTON_SIZE = new Size(100, 30);

		public static DialogResult InputBox(string text, out string value, char passwordChar = '\0')
		{
			value = string.Empty;

			Form form = new Form();
			Label lblText = new Label();
			TextBox txtResult = new TextBox();
			Button btnOk = new Button();
			Button btnCancel = new Button();

			form.BackColor = DEFAULT_FORM_BACKCOLOR;
			form.ForeColor = DEFAULT_FORM_FORECOLOR;

			lblText.Text = text;
			lblText.AutoSize = true;
			lblText.Font = new Font(DEFAULT_TEXT_FONT_FAMILY, DEFAULT_TEXT_FONT_SIZE, FontStyle.Regular);
			lblText.Size = TextRenderer.MeasureText(lblText.Text, lblText.Font);
			lblText.Location = new Point(DEFAULT_XBORDER, DEFAULT_YBORDER);

			txtResult.BackColor = form.BackColor;
			txtResult.ForeColor = form.ForeColor;
			txtResult.Font = new Font(DEFAULT_TEXT_FONT_FAMILY, DEFAULT_TEXT_FONT_SIZE, FontStyle.Regular);
			txtResult.Width = Math.Max(lblText.Width, DEFAULT_BUTTON_SIZE.Width * 2 + DEFAULT_MARGIN);
			txtResult.Location = new Point(DEFAULT_XBORDER, lblText.Bottom + DEFAULT_MARGIN);
			if (passwordChar != '\0') txtResult.PasswordChar = passwordChar;

			btnOk.Text = "Confirm";
			btnOk.DialogResult = DialogResult.OK;
			btnOk.FlatStyle = FlatStyle.Flat;
			btnOk.Font = new Font(DEFAULT_TEXT_FONT_FAMILY, DEFAULT_TEXT_FONT_SIZE, FontStyle.Regular);
			btnOk.Size = new Size(100, 30);
			btnOk.Location = new Point(DEFAULT_XBORDER + (txtResult.Width - (DEFAULT_BUTTON_SIZE.Width * 2 + DEFAULT_MARGIN)), txtResult.Bottom + DEFAULT_MARGIN);

			btnCancel.Text = "Cancel";
			btnCancel.DialogResult = DialogResult.Cancel;
			btnCancel.FlatStyle = FlatStyle.Flat;
			btnCancel.Font = new Font(DEFAULT_TEXT_FONT_FAMILY, DEFAULT_TEXT_FONT_SIZE, FontStyle.Regular);
			btnCancel.Size = btnOk.Size;
			btnCancel.Location = new Point(btnOk.Right + DEFAULT_MARGIN, btnOk.Location.Y);

			form.Controls.AddRange(GenerateBorderControls());
			form.Controls.AddRange(new Control[] { lblText, txtResult, btnOk, btnCancel });
			form.ClientSize = new Size(Math.Max(btnCancel.Right + DEFAULT_XBORDER, lblText.Right + DEFAULT_XBORDER), btnCancel.Bottom + DEFAULT_YBORDER);
			form.FormBorderStyle = FormBorderStyle.None;
			form.AutoScaleMode = AutoScaleMode.None;
			form.StartPosition = FormStartPosition.CenterParent;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = btnOk;
			form.CancelButton = btnCancel;

			DialogResult dialogResult = form.ShowDialog();
			value = txtResult.Text;
			return dialogResult;
		}
		public static DialogResult OutputBox(string text)
		{
			Form form = new Form();
			Label lblText = new Label();
			Button btnOk = new Button();

			form.BackColor = DEFAULT_FORM_BACKCOLOR;
			form.ForeColor = DEFAULT_FORM_FORECOLOR;

			lblText.Text = text;
			lblText.AutoSize = true;
			lblText.Font = new Font(DEFAULT_TEXT_FONT_FAMILY, DEFAULT_TEXT_FONT_SIZE, FontStyle.Regular);
			lblText.Size = TextRenderer.MeasureText(lblText.Text, lblText.Font);
			lblText.Location = new Point(DEFAULT_XBORDER, DEFAULT_YBORDER);

			btnOk.Text = "OK";
			btnOk.DialogResult = DialogResult.OK;
			btnOk.FlatStyle = FlatStyle.Flat;
			btnOk.Font = new Font(DEFAULT_TEXT_FONT_FAMILY, DEFAULT_TEXT_FONT_SIZE, FontStyle.Regular);
			btnOk.Size = new Size(100, 30);
			btnOk.Location = lblText.Width > btnOk.Width ? new Point(DEFAULT_XBORDER + (lblText.Width - btnOk.Width) / 2, lblText.Bottom + DEFAULT_MARGIN) : new Point(DEFAULT_XBORDER, lblText.Bottom + DEFAULT_MARGIN);

			form.Controls.AddRange(GenerateBorderControls());
			form.Controls.AddRange(new Control[] { lblText, btnOk });
			form.ClientSize = new Size(Math.Max(lblText.Right + DEFAULT_XBORDER, btnOk.Right + DEFAULT_XBORDER), btnOk.Bottom + DEFAULT_YBORDER);
			form.FormBorderStyle = FormBorderStyle.None;
			form.AutoScaleMode = AutoScaleMode.None;
			form.StartPosition = FormStartPosition.CenterParent;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = btnOk;

			DialogResult dialogResult = form.ShowDialog();
			return dialogResult;
		}

		private static Control[] GenerateBorderControls()
		{
			return new Control[]
			{
				new Panel() { BackColor = DEFAULT_FORM_BORDERCOLOR, Height = 1, Dock = DockStyle.Top },
				new Panel() { BackColor = DEFAULT_FORM_BORDERCOLOR, Height = 1, Dock = DockStyle.Bottom },
				new Panel() { BackColor = DEFAULT_FORM_BORDERCOLOR, Width = 1, Dock = DockStyle.Left },
				new Panel() { BackColor = DEFAULT_FORM_BORDERCOLOR, Width = 1, Dock = DockStyle.Right }
			};
		}
	}
}
