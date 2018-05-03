using Geometry;
using GLCore;
using GLStyle;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GLUITest
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();

            StyleManager.LoadStyle("Style.ini");

            cmbSelectType.Items.Add(nameof(SinglePairInfo));
            cmbSelectType.Items.Add(nameof(SingleTowerPairInfo));
            cmbSelectType.Items.Add(nameof(SingleLineInfo));
            cmbSelectType.Items.Add(nameof(SingleAreaInfo));
        }

        /// <summary>
        /// 重新綁定資料
        /// </summary>
        private void CmbSelectType_SelectedValueChanged(object sender, EventArgs e)
        {
            // 綁資料
            switch ((sender as ComboBox).Text)
            {
                case nameof(SinglePairInfo):
                    {
                        BindingSource singleTowerPairInfoSource = new BindingSource(GLCMD.SinglePairInfo, null);
                        dgvInfo.DataSource = singleTowerPairInfoSource;
                    }
                    break;

                case nameof(SingleTowerPairInfo):
                    {
                        BindingSource singleTowerPairInfoSource = new BindingSource(GLCMD.SingleTowerPairInfo, null);
                        dgvInfo.DataSource = singleTowerPairInfoSource;
                    }
                    break;

                case nameof(SingleLineInfo):
                    {
                        BindingSource singleTowerPairInfoSource = new BindingSource(GLCMD.SingleLineInfo, null);
                        dgvInfo.DataSource = singleTowerPairInfoSource;
                    }
                    break;

                case nameof(SingleAreaInfo):
                    {
                        BindingSource singleTowerPairInfoSource = new BindingSource(GLCMD.SingleAreaInfo, null);
                        dgvInfo.DataSource = singleTowerPairInfoSource;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 命令範例
        /// </summary>
        private void CMDDemo()
        {
            // 使用者用指令加入圖形(可復原)
            GLCMD.Do($"Add,{GLCMD.SerialNumber.Next()},ChargingDocking,-2000,0,0");
            GLCMD.Do($"Add,{GLCMD.SerialNumber.Next()},ForbiddenArea2,0,0,3000,3000");
            GLCMD.Do($"Add,{GLCMD.SerialNumber.Next()},ForbiddenArea,4000,4000,7000,7000");

            // 使用者用函式加入圖形(可復原)
            GLCMD.DoAddSingleTowardPair("Goal", 0, 2000, 45);

            // 使用者用函式加入複合圖形(不可復原)
            Random random = new Random();
            List<IPair> list = new List<IPair>();
            for (int ii = 0; ii < 100000; ++ii)
            {
                list.Add(new Pair(random.Next(-10000, 10000), random.Next(-10000, 10000)));
            }
            GLUI.ObstaclePointsID = GLCMD.AddMultiPair("@ObstaclePoints", list);
        }

        /// <summary>
        /// 移動視角
        /// </summary>
        private void DgvInfo_DoubleClick(object sender, EventArgs e)
        {
            int rowIndex = (sender as DataGridView).CurrentRow?.Index ?? -1;
            if (rowIndex < 0 || rowIndex >= dgvInfo.RowCount) return;

            switch (cmbSelectType.Text)
            {
                case nameof(SinglePairInfo):
                    {
                        int x = (int)(dgvInfo[nameof(SinglePairInfo.X), rowIndex].Value);
                        int y = (int)(dgvInfo[nameof(SinglePairInfo.Y), rowIndex].Value);

                        GLUI.Focus(x, y);
                    }
                    break;

                case nameof(SingleTowerPairInfo):
                    {
                        int x = (int)(dgvInfo[nameof(SingleTowerPairInfo.X), rowIndex].Value);
                        int y = (int)(dgvInfo[nameof(SingleTowerPairInfo.Y), rowIndex].Value);

                        GLUI.Focus(x, y);
                    }
                    break;

                case nameof(SingleLineInfo):
                    {
                        int x0 = (int)(dgvInfo[nameof(SingleLineInfo.X0), rowIndex].Value);
                        int y0 = (int)(dgvInfo[nameof(SingleLineInfo.Y0), rowIndex].Value);
                        int x1 = (int)(dgvInfo[nameof(SingleLineInfo.X1), rowIndex].Value);
                        int y1 = (int)(dgvInfo[nameof(SingleLineInfo.Y1), rowIndex].Value);

                        GLUI.Focus((x0 + x1) / 2, (y0 + y1) / 2);
                    }
                    break;

                case nameof(SingleAreaInfo):
                    {
                        int x0 = (int)(dgvInfo[nameof(SingleAreaInfo.MinX), rowIndex].Value);
                        int y0 = (int)(dgvInfo[nameof(SingleAreaInfo.MinY), rowIndex].Value);
                        int x1 = (int)(dgvInfo[nameof(SingleAreaInfo.MaxX), rowIndex].Value);
                        int y1 = (int)(dgvInfo[nameof(SingleAreaInfo.MaxY), rowIndex].Value);

                        GLUI.Focus((x0 + x1) / 2, (y0 + y1) / 2);
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 輸入資料，修改 <see cref="GLCMD"/> 內容
        /// </summary>
        private void DgvInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int rowIndex = (sender as DataGridView).CurrentRow?.Index ?? -1;
                int colIndex = (sender as DataGridView).CurrentCell?.ColumnIndex ?? -1;
                if (rowIndex >= 0) UpdateGLCMD(rowIndex, colIndex);
            }
        }

        /// <summary>
        /// 回傳表格中對應的 ID。若 ID 不存在，則回傳 -1
        /// </summary>
        private int GetTargetID(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvInfo.RowCount) return -1;

            switch (cmbSelectType.Text)
            {
                case nameof(SinglePairInfo):
                    {
                        return (int)(dgvInfo[nameof(SinglePairInfo.ID), rowIndex].Value);
                    }

                case nameof(SingleTowerPairInfo):
                    {
                        return (int)(dgvInfo[nameof(SingleTowerPairInfo.ID), rowIndex].Value);
                    }

                case nameof(SingleLineInfo):
                    {
                        return (int)(dgvInfo[nameof(SingleLineInfo.ID), rowIndex].Value);
                    }

                case nameof(SingleAreaInfo):
                    {
                        return (int)(dgvInfo[nameof(SingleAreaInfo.ID), rowIndex].Value);
                    }

                default:
                    break;
            }

            return -1;
        }

        /// <summary>
        /// 從 <see cref="dgvInfo"/> 中搜尋對應的欄位。若資料不存在則回 <see cref="string.Empty"/>
        /// </summary>
        private string GetValue(int rowIndex, string elementName)
        {
            return dgvInfo[elementName, rowIndex].Value.ToString();
        }

        /// <summary>
        /// 下命令方式更新 <see cref="GLCMD"/> 中的物件座標、名稱等等
        /// </summary>
        private void UpdateGLCMD(int rowIndex, int colIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvInfo.RowCount) return;
            if (colIndex < 0 || colIndex >= dgvInfo.ColumnCount) return;

            string colName = dgvInfo.Columns[colIndex].Name;
            string newValue = dgvInfo[colIndex, rowIndex].Value.ToString();

            switch (cmbSelectType.Text)
            {
                case nameof(SinglePairInfo):
                    {
                        UpdateSinglePairInfo(rowIndex, colName, newValue);
                    }
                    break;

                case nameof(SingleTowerPairInfo):
                    {
                        UpdateSingleTowardPairInfo(rowIndex, colName, newValue);
                    }
                    break;

                case nameof(SingleLineInfo):
                    {
                        UpdateSingleLineInfo(rowIndex, colName, newValue);
                    }
                    break;

                case nameof(SingleAreaInfo):
                    {
                        UpdateSingleAreaInfo(rowIndex, colName, newValue);
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 下命令方式更新 <see cref="GLCMD"/> 中的物件座標、名稱等等
        /// </summary>
        private void UpdateSingleAreaInfo(int rowIndex, string elementName, string newValue)
        {
            int id = GetTargetID(rowIndex);
            switch (elementName)
            {
                case nameof(SingleAreaInfo.Name):
                    {
                        GLCMD.DoRename(id, newValue);
                    }
                    break;

                case nameof(SingleAreaInfo.MinX):
                    {
                        int x = int.Parse(newValue);
                        int y = int.Parse(GetValue(rowIndex, nameof(SingleAreaInfo.MinY)));
                        GLCMD.DoMoveMin(id, x, y);
                        GLCMD.MoveFinish();
                    }
                    break;

                case nameof(SingleAreaInfo.MinY):
                    {
                        int x = int.Parse(GetValue(rowIndex, nameof(SingleAreaInfo.MinX)));
                        int y = int.Parse(newValue);
                        GLCMD.DoMoveMin(id, x, y);
                        GLCMD.MoveFinish();
                    }
                    break;

                case nameof(SingleAreaInfo.MaxX):
                    {
                        int x = int.Parse(newValue);
                        int y = int.Parse(GetValue(rowIndex, nameof(SingleAreaInfo.MaxY)));
                        GLCMD.DoMoveMax(id, x, y);
                        GLCMD.MoveFinish();
                    }
                    break;

                case nameof(SingleAreaInfo.MaxY):
                    {
                        int x = int.Parse(GetValue(rowIndex, nameof(SingleAreaInfo.MaxX)));
                        int y = int.Parse(newValue);
                        GLCMD.DoMoveMax(id, x, y);
                        GLCMD.MoveFinish();
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 下命令方式更新 <see cref="GLCMD"/> 中的物件座標、名稱等等
        /// </summary>
        private void UpdateSingleLineInfo(int rowIndex, string elementName, string newValue)
        {
            int id = GetTargetID(rowIndex);
            switch (elementName)
            {
                case nameof(SingleLineInfo.Name):
                    {
                        GLCMD.DoRename(id, newValue);
                    }
                    break;

                case nameof(SingleLineInfo.X0):
                    {
                        int x = int.Parse(newValue);
                        int y = int.Parse(GetValue(rowIndex, nameof(SingleLineInfo.Y0)));
                        GLCMD.DoMoveBegin(id, x, y);
                        GLCMD.MoveFinish();
                    }
                    break;

                case nameof(SingleLineInfo.Y0):
                    {
                        int x = int.Parse(GetValue(rowIndex, nameof(SingleLineInfo.X0)));
                        int y = int.Parse(newValue);
                        GLCMD.DoMoveBegin(id, x, y);
                        GLCMD.MoveFinish();
                    }
                    break;

                case nameof(SingleLineInfo.X1):
                    {
                        int x = int.Parse(newValue);
                        int y = int.Parse(GetValue(rowIndex, nameof(SingleLineInfo.Y1)));
                        GLCMD.DoMoveEnd(id, x, y);
                        GLCMD.MoveFinish();
                    }
                    break;

                case nameof(SingleLineInfo.Y1):
                    {
                        int x = int.Parse(GetValue(rowIndex, nameof(SingleLineInfo.X1)));
                        int y = int.Parse(newValue);
                        GLCMD.DoMoveEnd(id, x, y);
                        GLCMD.MoveFinish();
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 下命令方式更新 <see cref="GLCMD"/> 中的物件座標、名稱等等
        /// </summary>
        private void UpdateSinglePairInfo(int rowIndex, string elementName, string newValue)
        {
            int id = GetTargetID(rowIndex);
            switch (elementName)
            {
                case nameof(SinglePairInfo.Name):
                    {
                        GLCMD.DoRename(id, newValue);
                    }
                    break;

                case nameof(SinglePairInfo.X):
                    {
                        int x = int.Parse(newValue);
                        int y = int.Parse(GetValue(rowIndex, nameof(SinglePairInfo.Y)));
                        GLCMD.DoMoveCenter(id, x, y);
                        GLCMD.MoveFinish();
                    }
                    break;

                case nameof(SinglePairInfo.Y):
                    {
                        int x = int.Parse(GetValue(rowIndex, nameof(SinglePairInfo.X)));
                        int y = int.Parse(newValue);
                        GLCMD.DoMoveCenter(id, x, y);
                        GLCMD.MoveFinish();
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 下命令方式更新 <see cref="GLCMD"/> 中的物件座標、名稱等等
        /// </summary>
        private void UpdateSingleTowardPairInfo(int rowIndex, string elementName, string newValue)
        {
            int id = GetTargetID(rowIndex);
            switch (elementName)
            {
                case nameof(SingleTowerPairInfo.Name):
                    {
                        GLCMD.DoRename(id, newValue);
                    }
                    break;

                case nameof(SingleTowerPairInfo.X):
                    {
                        int x = int.Parse(newValue);
                        int y = int.Parse(GetValue(rowIndex, nameof(SingleTowerPairInfo.Y)));
                        GLCMD.DoMoveCenter(id, x, y);
                        GLCMD.MoveFinish();
                    }
                    break;

                case nameof(SingleTowerPairInfo.Y):
                    {
                        int x = int.Parse(GetValue(rowIndex, nameof(SingleTowerPairInfo.X)));
                        int y = int.Parse(newValue);
                        GLCMD.DoMoveCenter(id, x, y);
                        GLCMD.MoveFinish();
                    }
                    break;

                case nameof(SingleTowerPairInfo.Toward):
                    {
                        double theta = double.Parse(newValue) * Math.PI / 180.0;
                        int x = int.Parse(GetValue(rowIndex, nameof(SingleTowerPairInfo.X)));
                        int y = int.Parse(GetValue(rowIndex, nameof(SingleTowerPairInfo.Y)));
                        int dx = (int)(Math.Cos(theta) * 10000);
                        int dy = (int)(Math.Cos(theta) * 10000);
                        GLCMD.DoMoveToward(id, x + dx, y + dy);
                        GLCMD.MoveFinish();
                    }
                    break;

                default:
                    break;
            }
        }
    }
}