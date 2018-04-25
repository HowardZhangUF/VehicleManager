using System;
using System.Windows.Forms;

namespace GLUI
{
    public partial class TextInput : Form
    {
        public TextInput(string title, string hint, string text, Action<string> done)
        {
            InitializeComponent();

            MaximizeBox = false;
            Text = title;
            lblHint.Text = hint;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Done = done;
            txtInput.Text = text;
        }

        private Action<string> Done { get; }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnDone_Click(object sender, EventArgs e)
        {
            Done?.Invoke(txtInput.Text);
            Close();
        }
    }
}