using Geometry;
using GLCore;
using GLUI.Language;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GLUI
{
    public partial class JoinMap : Form
    {
        public JoinMap(Action<IEnumerable<IPair>> done)
        {
            InitializeComponent();

            // 語系
            rbtnCenter.Text = Lang.MoveCenter;
            rbtnMax.Text = Lang.MoveMax;
            rbtnMin.Text = Lang.MoveMin;
            gboxSelectRange.Text = Lang.SelectRange;
            gboxTranslate.Text = Lang.Translate;
            gboxRotate.Text = Lang.Rotate;

            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            TopMost = true;
            Done = done;

            UpdateControlPanel();
        }

        private Action<IEnumerable<IPair>> Done { get; }

        /// <summary>
        /// 插入地圖
        /// </summary>
        public void LoadJoinMap()
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "map files (*.map)|*.map";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LoadJoinMap(ofd.FileName);
                }
            }
        }

        /// <summary>
        /// 插入地圖
        /// </summary>
        public void LoadJoinMap(string file)
        {
            GLCMD.CMD.Join.SaftyEdit(true, join => join.LoadJoinMap(file));
            UpdateControlPanel();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            GLCMD.CMD.Join.SaftyEdit(true, join => join.ClearAll());
            Close();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            Done?.Invoke(GLCMD.CMD.Join.SaftyEdit(join => join.GetObstaclePoints()));
            GLCMD.CMD.Join.SaftyEdit(true, join => join.ClearAll());
            Close();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadJoinMap();
        }

        private void btnSelectRange_Click(object sender, EventArgs e)
        {
            GLCMD.CMD.Join.SaftyEdit(true, join => join.EnableSelectRange = !join.EnableSelectRange);
            UpdateControlPanel();
        }

        /// <summary>
        /// 在 UI 的 RadioButton 選擇的角度
        /// </summary>
        private double GetRotateStep()
        {
            if (rbtn01deg.Checked) return 0.1;
            if (rbtn1deg.Checked) return 1;
            if (rbtn5deg.Checked) return 5;
            if (rbtn45deg.Checked) return 45;

            return 0;
        }

        /// <summary>
        /// 在 UI 的 RadioButton 選擇的長度
        /// </summary>
        private int GetTranslateStep()
        {
            if (rbtn1mm.Checked) return 1;
            if (rbtn10mm.Checked) return 10;
            if (rbtn100mm.Checked) return 100;
            if (rbtn1000mm.Checked) return 1000;

            return 0;
        }

        private void RotateBtnMouseDown(object sender, MouseEventArgs e)
        {
            // 由按鈕決定方向
            int dir = 0;
            switch ((sender as Button).Name)
            {
                case nameof(btnRotateRight):
                    dir = -1;
                    break;

                case nameof(btnRotateLeft):
                    dir = 1;
                    break;

                default:
                    break;
            }

            double angle = GetRotateStep();

            if (GLCMD.CMD.Join.SaftyEdit(join => !join.EnableSelectRange))
            {
                // 移動障礙點
                GLCMD.CMD.Join.SaftyEdit(true, join =>
                {
                    join.Rotate.Theta -= dir * angle;
                });
            }
        }

        /// <summary>
        /// 啟用/禁用旋轉相關物件
        /// </summary>
        private void RotateButton(bool enable)
        {
            gboxRotate.Enabled = enable;
            btnRotateRight.Enabled = enable;
            btnRotateLeft.Enabled = enable;
        }

        /// <summary>
        /// 啟用/禁用選擇相關物件
        /// </summary>
        private void SelectRangeButton(bool enable)
        {
            rbtnCenter.Enabled = enable;
            rbtnMax.Enabled = enable;
            rbtnMin.Enabled = enable;
        }

        private void TranslateBtnMouseDown(object sender, MouseEventArgs e)
        {
            // 由按鈕決定方向
            int sx = 0;
            int sy = 0;
            switch ((sender as Button).Name)
            {
                case nameof(btnUp):
                    sy = 1;
                    break;

                case nameof(btnDown):
                    sy = -1;
                    break;

                case nameof(btnLeft):
                    sx = -1;
                    break;

                case nameof(btnRight):
                    sx = 1;
                    break;

                default:
                    break;
            }

            // 獲得移動距離
            int length = GetTranslateStep();

            // 移動
            if (GLCMD.CMD.Join.SaftyEdit(join => !join.EnableSelectRange))
            {
                // 移動障礙點
                GLCMD.CMD.Join.SaftyEdit(true, join =>
                 {
                     join.Translate.X += sx * length;
                     join.Translate.Y += sy * length;
                 });
            }
            else
            {
                // 移動選擇區域
                GLCMD.CMD.Join.SaftyEdit(true, join =>
                {
                    var range = join.SelectRange;

                    // 移動中心點
                    if (rbtnCenter.Checked)
                    {
                        range.Move(EMoveType.Center, range.Geometry.Center().X + sx * length, range.Geometry.Center().Y + sy * length);
                    }
                    else if (rbtnMin.Checked) // 移動最小值座標
                    {
                        range.Move(EMoveType.Min, range.Geometry.Min.X + sx * length, range.Geometry.Min.Y + sy * length);
                    }
                    else if (rbtnMax.Checked) // 移動最大值座標
                    {
                        range.Move(EMoveType.Max, range.Geometry.Max.X + sx * length, range.Geometry.Max.Y + sy * length);
                    }
                });
            }
        }

        /// <summary>
        /// 啟用/禁用平移相關物件
        /// </summary>
        private void TranslateButton(bool enable)
        {
            gboxTranslate.Enabled = enable;
            btnUp.Enabled = enable;
            btnDown.Enabled = enable;
            btnLeft.Enabled = enable;
            btnRight.Enabled = enable;
        }

        /// <summary>
        /// 更新控制項
        /// </summary>
        private void UpdateControlPanel()
        {
            if (GLCMD.CMD.Join.SaftyEdit(join => !join.InUse))
            {
                RotateButton(false);
                TranslateButton(false);
                SelectRangeButton(false);
            }
            else
            {
                if (GLCMD.CMD.Join.SaftyEdit(join => join.EnableSelectRange))
                {
                    RotateButton(false);
                    TranslateButton(true);
                    SelectRangeButton(true);
                }
                else
                {
                    RotateButton(true);
                    TranslateButton(true);
                    SelectRangeButton(false);
                }
            }
        }
    }
}