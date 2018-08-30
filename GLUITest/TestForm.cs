using GLCore;
using GLStyle;
using GLUI;
using PairAStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using GLUITest.Properties;
using System.Collections.Concurrent;
using SerialData;
using Geometry;
using Serialization;
using AsyncSocket;
using LittleGhost;

namespace GLUITest
{
	public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();

            // 載入設定檔
            StyleManager.LoadStyle("Style.ini");

			// 加入選單
			cmbSelectType.Items.Add("Point");
			cmbSelectType.Items.Add("Station");
            cmbSelectType.Items.Add("Line");
            cmbSelectType.Items.Add("Area");

			Text = "Vehicle Manager";

            // 加入事件
            GLUI.LoadMapEvent += GLUI_LoadMapEvent;
            GLUI.PenMapEvent += GLUI_PenMapEvent;
            GLUI.EraserMapEvent += GLUI_EraserMapEvent;
            GLUI.CommandOnClick += GLUI_CommandOnClick;

			// 關閉地圖編輯功能
			GLUI.ContextMenuStripMode = false;

			// Map Objects 分頁隱藏
			tabControl1.TabPages.Remove(tabPage2);

            // 加入範例 / 測試
            //CMDDemo();
            //AGVDemo();
        }

        private void frmTest_Load(object sender, EventArgs e)
        {
            GUI_InitializeAGVInfoMonitor();

            SocketTest();
        }

        /// <summary>
        /// 停用顯示 AGV 狀態的 DataGridView 的選取功能
        /// </summary>
        private void dgvAGVInfo_SelectionChanged(object sender, EventArgs e)
        {
            dgvAGVInfo.ClearSelection();
        }

        /// <summary>
        /// 若清單選擇改變時，則更新顯示 AGV 狀態的 DataGridView
        /// </summary>
        private void cbAGVList_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (agvs)
            {
                GUI_UpdateAGVInfoMonitor(agvs[cbAGVList.InvokeIfNecessary((a) => a.SelectedItem.ToString())].Status);
            }
		}

		/// <summary>
		/// 載入地圖
		/// </summary>
		private void btnLoadMap_Click(object sender, EventArgs e)
		{
			GLUI.LoadMap();
		}

		/// <summary>
		/// 初始化地圖，除了 AGV 資訊
		/// </summary>
		private void btnClearMap_Click(object sender, EventArgs e)
		{
			GLCMD.CMD.InitialButAGV();
			GUI_ClearMapInfo();
		}

		/// <summary>
		/// 傳送要求地圖清單的命令
		/// </summary>
		private void btnRequestMapList_Click(object sender, EventArgs e)
		{
			SendRequestMapListCommand();
		}

		/// <summary>
		/// 傳送上傳地圖至 AGV 上的命令
		/// </summary>
		private void btnUploadMapToAGV_Click(object sender, EventArgs e)
		{
			SendUploadMapCommand();
		}

		/// <summary>
		/// 傳送 AGV 更新目前使用地圖的命令
		/// </summary>
		private void btnChangeMap_Click(object sender, EventArgs e)
		{
			SendChangeMapCommand();
		}

		/// <summary>
		/// 重新綁定資料
		/// </summary>
		private void CmbSelectType_SelectedValueChanged(object sender, EventArgs e)
        {
            // 綁資料
            switch ((sender as ComboBox).Text)
            {
                case "Point":
                    {
                        BindingSource singleTowerPairInfoSource = new BindingSource(GLCMD.CMD.SinglePairInfo, null);
                        dgvInfo.DataSource = singleTowerPairInfoSource;
                    }
                    break;

                case "Station":
                    {
                        BindingSource singleTowerPairInfoSource = new BindingSource(GLCMD.CMD.SingleTowerPairInfo, null);
                        dgvInfo.DataSource = singleTowerPairInfoSource;
                    }
                    break;

                case "Line":
                    {
                        BindingSource singleTowerPairInfoSource = new BindingSource(GLCMD.CMD.SingleLineInfo, null);
                        dgvInfo.DataSource = singleTowerPairInfoSource;
                    }
                    break;

                case "Area":
                    {
                        BindingSource singleTowerPairInfoSource = new BindingSource(GLCMD.CMD.SingleAreaInfo, null);
                        dgvInfo.DataSource = singleTowerPairInfoSource;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 輸入資料，修改 <see cref="GLCMD"/> 內容
        /// </summary>
        private void DgvInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) UpdateGLCMD(e.RowIndex, e.ColumnIndex);
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
                case "Point":
                    {
                        int x = (int)(dgvInfo[nameof(SinglePairInfo.X), rowIndex].Value);
                        int y = (int)(dgvInfo[nameof(SinglePairInfo.Y), rowIndex].Value);

                        GLUI.Focus(x, y);
                    }
                    break;

                case "Station":
                    {
                        int x = (int)(dgvInfo[nameof(SingleTowardPairInfo.X), rowIndex].Value);
                        int y = (int)(dgvInfo[nameof(SingleTowardPairInfo.Y), rowIndex].Value);

                        GLUI.Focus(x, y);
                    }
                    break;

                case "Line":
                    {
                        int x0 = (int)(dgvInfo[nameof(SingleLineInfo.X0), rowIndex].Value);
                        int y0 = (int)(dgvInfo[nameof(SingleLineInfo.Y0), rowIndex].Value);
                        int x1 = (int)(dgvInfo[nameof(SingleLineInfo.X1), rowIndex].Value);
                        int y1 = (int)(dgvInfo[nameof(SingleLineInfo.Y1), rowIndex].Value);

                        GLUI.Focus((x0 + x1) / 2, (y0 + y1) / 2);
                    }
                    break;

                case "Area":
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
		/// StatusStrip 的 Log In 標籤被點擊時
		/// </summary>
		private void toolStripStatusLabelLogIn_Click(object sender, EventArgs e)
		{
			string password = "";
			// 若已登入
			if (isLogIn)
			{
				switch (logLevel)
				{
					case 0:
						if (tabControl1.TabPages.Contains(tabPage2))
							tabControl1.TabPages.Remove(tabPage2);
						break;
					case 1:
						break;
					case -1:
						break;
					default:
						break;
				}
				toolStripMenuItemLogIn.Text = toolStripStatusLabelLogIn.Text = "Log In";
				toolStripMenuItemLogIn.BackColor = toolStripStatusLabelLogIn.BackColor = System.Drawing.Color.Transparent;
				LogOut();
			}
			// 若未登入
			else
			{
				if (InputBox("Log In", "Please Enter the Password:", ref password, '*') == DialogResult.OK)
				{
					// 登入成功
					if (LogIn(password))
					{
						switch (logLevel)
						{
							case 0:
								if (!tabControl1.TabPages.Contains(tabPage2))
									tabControl1.TabPages.Add(tabPage2);
								break;
							case 1:
								break;
							case -1:
								break;
							default:
								break;
						}
						toolStripMenuItemLogIn.Text = toolStripStatusLabelLogIn.Text = $"{currentUser} - Log Out";
						toolStripMenuItemLogIn.BackColor = toolStripStatusLabelLogIn.BackColor = System.Drawing.Color.Yellow;
					}
					else
					{
						MessageBox.Show("Wrong Passwrod!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		/// <summary>
		/// ToolStripItem 的 Log In 選單被點擊時
		/// </summary>
		private void logInToolStripMenuItem_Click(object sender, EventArgs e)
		{
			toolStripStatusLabelLogIn_Click(null, null);
		}

		/// <summary>
		/// 傳送命令
		/// </summary>
		private void GLUI_CommandOnClick(object sender, CommandOnClickEventArgs e)
        {
            MessageBox.Show($"這裡向 iM 發送命令:\r\n{e.Command}");
		}

		/// <summary>
		/// 回傳表格中對應的 ID。若 ID 不存在，則回傳 -1
		/// </summary>
		private int GetTargetID(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvInfo.RowCount) return -1;

            switch (cmbSelectType.Text)
            {
                case "Point":
                    {
                        return (int)(dgvInfo[nameof(SinglePairInfo.ID), rowIndex].Value);
                    }

                case "Station":
                    {
                        return (int)(dgvInfo[nameof(SingleTowardPairInfo.ID), rowIndex].Value);
                    }

                case "Line":
                    {
                        return (int)(dgvInfo[nameof(SingleLineInfo.ID), rowIndex].Value);
                    }

                case "Area":
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
                case "Point":
                    {
                        UpdateSinglePairInfo(rowIndex, colName, newValue);
                    }
                    break;

                case "Station":
                    {
                        UpdateSingleTowardPairInfo(rowIndex, colName, newValue);
                    }
                    break;

                case "Line":
                    {
                        UpdateSingleLineInfo(rowIndex, colName, newValue);
                    }
                    break;

                case "Area":
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
                        GLCMD.CMD.DoRename(id, newValue);
                    }
                    break;

                case nameof(SingleAreaInfo.MinX):
                    {
                        int x = int.Parse(newValue);
                        int y = int.Parse(GetValue(rowIndex, nameof(SingleAreaInfo.MinY)));
                        GLCMD.CMD.DoMoveMin(id, x, y);
                        GLCMD.CMD.MoveFinish();
                    }
                    break;

                case nameof(SingleAreaInfo.MinY):
                    {
                        int x = int.Parse(GetValue(rowIndex, nameof(SingleAreaInfo.MinX)));
                        int y = int.Parse(newValue);
                        GLCMD.CMD.DoMoveMin(id, x, y);
                        GLCMD.CMD.MoveFinish();
                    }
                    break;

                case nameof(SingleAreaInfo.MaxX):
                    {
                        int x = int.Parse(newValue);
                        int y = int.Parse(GetValue(rowIndex, nameof(SingleAreaInfo.MaxY)));
                        GLCMD.CMD.DoMoveMax(id, x, y);
                        GLCMD.CMD.MoveFinish();
                    }
                    break;

                case nameof(SingleAreaInfo.MaxY):
                    {
                        int x = int.Parse(GetValue(rowIndex, nameof(SingleAreaInfo.MaxX)));
                        int y = int.Parse(newValue);
                        GLCMD.CMD.DoMoveMax(id, x, y);
                        GLCMD.CMD.MoveFinish();
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
                        GLCMD.CMD.DoRename(id, newValue);
                    }
                    break;

                case nameof(SingleLineInfo.X0):
                    {
                        int x = int.Parse(newValue);
                        int y = int.Parse(GetValue(rowIndex, nameof(SingleLineInfo.Y0)));
                        GLCMD.CMD.DoMoveBegin(id, x, y);
                        GLCMD.CMD.MoveFinish();
                    }
                    break;

                case nameof(SingleLineInfo.Y0):
                    {
                        int x = int.Parse(GetValue(rowIndex, nameof(SingleLineInfo.X0)));
                        int y = int.Parse(newValue);
                        GLCMD.CMD.DoMoveBegin(id, x, y);
                        GLCMD.CMD.MoveFinish();
                    }
                    break;

                case nameof(SingleLineInfo.X1):
                    {
                        int x = int.Parse(newValue);
                        int y = int.Parse(GetValue(rowIndex, nameof(SingleLineInfo.Y1)));
                        GLCMD.CMD.DoMoveEnd(id, x, y);
                        GLCMD.CMD.MoveFinish();
                    }
                    break;

                case nameof(SingleLineInfo.Y1):
                    {
                        int x = int.Parse(GetValue(rowIndex, nameof(SingleLineInfo.X1)));
                        int y = int.Parse(newValue);
                        GLCMD.CMD.DoMoveEnd(id, x, y);
                        GLCMD.CMD.MoveFinish();
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
                        GLCMD.CMD.DoRename(id, newValue);
                    }
                    break;

                case nameof(SinglePairInfo.X):
                    {
                        int x = int.Parse(newValue);
                        int y = int.Parse(GetValue(rowIndex, nameof(SinglePairInfo.Y)));
                        GLCMD.CMD.DoMoveCenter(id, x, y);
                        GLCMD.CMD.MoveFinish();
                    }
                    break;

                case nameof(SinglePairInfo.Y):
                    {
                        int x = int.Parse(GetValue(rowIndex, nameof(SinglePairInfo.X)));
                        int y = int.Parse(newValue);
                        GLCMD.CMD.DoMoveCenter(id, x, y);
                        GLCMD.CMD.MoveFinish();
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
                case nameof(SingleTowardPairInfo.Name):
                    {
                        GLCMD.CMD.DoRename(id, newValue);
                    }
                    break;

                case nameof(SingleTowardPairInfo.X):
                    {
                        int x = int.Parse(newValue);
                        int y = int.Parse(GetValue(rowIndex, nameof(SingleTowardPairInfo.Y)));
                        GLCMD.CMD.DoMoveCenter(id, x, y);
                        GLCMD.CMD.MoveFinish();
                    }
                    break;

                case nameof(SingleTowardPairInfo.Y):
                    {
                        int x = int.Parse(GetValue(rowIndex, nameof(SingleTowardPairInfo.X)));
                        int y = int.Parse(newValue);
                        GLCMD.CMD.DoMoveCenter(id, x, y);
                        GLCMD.CMD.MoveFinish();
                    }
                    break;

                case nameof(SingleTowardPairInfo.Toward):
                    {
                        double theta = double.Parse(newValue) * Math.PI / 180.0;
                        int x = int.Parse(GetValue(rowIndex, nameof(SingleTowardPairInfo.X)));
                        int y = int.Parse(GetValue(rowIndex, nameof(SingleTowardPairInfo.Y)));
                        int dx = (int)(Math.Cos(theta) * 10000);
                        int dy = (int)(Math.Sin(theta) * 10000);
                        GLCMD.CMD.DoMoveToward(id, x + dx, y + dy);
                        GLCMD.CMD.MoveFinish();
                    }
                    break;

                default:
                    break;
            }
		}

		/// <summary>
		/// 自訂輸入視窗
		/// </summary>
		/// <param name="caption">標題</param>
		/// <param name="text">內文</param>
		/// <param name="value">結果值</param>
		/// <returns></returns>
		public static DialogResult InputBox(string caption, string text, ref string value, char passwordChar = '\0')
		{
			Form form = new Form();
			Label lblText = new Label();
			TextBox txtResult = new TextBox();
			Button btnOk = new Button();
			Button btnCancel = new Button();

			form.Text = caption;
			lblText.Text = text;
			lblText.AutoSize = true;
			btnOk.Text = "OK";
			btnOk.DialogResult = DialogResult.OK;
			btnCancel.Text = "Cancel";
			btnCancel.DialogResult = DialogResult.Cancel;
			if (passwordChar != '\0') txtResult.PasswordChar = passwordChar;

			form.Controls.AddRange(new Control[] { lblText, txtResult, btnOk, btnCancel });
			lblText.Location = new System.Drawing.Point(20, 10);
			txtResult.SetBounds(20, lblText.Location.Y + lblText.Size.Height + 10, 160, 20);
			btnOk.SetBounds(20, txtResult.Location.Y + txtResult.Size.Height + 10, 75, 30);
			btnCancel.SetBounds(btnOk.Right + 10, btnOk.Location.Y, 75, 30);

			form.ClientSize = new System.Drawing.Size(Math.Max(btnCancel.Right + 20, lblText.Right + 20), btnCancel.Bottom + 10);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterParent;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = btnOk;
			form.CancelButton = btnCancel;

			DialogResult dialogResult = form.ShowDialog();
			value = txtResult.Text;
			return dialogResult;
		}

		#region 介面操作

		/// <summary>
		/// 初始化顯示 AGV 狀態的 DataGridView
		/// </summary>
		private void GUI_InitializeAGVInfoMonitor()
        {
            // 允許使用者新增列
            dgvAGVInfo.AllowUserToAddRows = false;
            // 允許使用者調整攔寬
            dgvAGVInfo.AllowUserToResizeColumns = false;
            // 允許使用者調整列高
            dgvAGVInfo.AllowUserToResizeRows = false;
            // 首欄是否顯示
            dgvAGVInfo.RowHeadersVisible = false;
            // 首列是否顯示
            dgvAGVInfo.ColumnHeadersVisible = false;
            // 唯讀
            dgvAGVInfo.ReadOnly = true;
            // 選取模式
            dgvAGVInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvAGVInfo.Columns.Add("key", "key");
            dgvAGVInfo.Columns.Add("value", "value");
            dgvAGVInfo.Columns["key"].Width = 80;
            dgvAGVInfo.Columns["key"].DefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
            dgvAGVInfo.Columns["value"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvAGVInfo.Columns["value"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvAGVInfo.Rows.Add(9);
            dgvAGVInfo.Rows[0].Cells[0].Value = "Name";
            dgvAGVInfo.Rows[1].Cells[0].Value = "Status";
            dgvAGVInfo.Rows[2].Cells[0].Value = "Battery (%)";
            dgvAGVInfo.Rows[3].Cells[0].Value = "X (mm)";
            dgvAGVInfo.Rows[4].Cells[0].Value = "Y (mm)";
            dgvAGVInfo.Rows[5].Cells[0].Value = "Toward (deg)";
            dgvAGVInfo.Rows[6].Cells[0].Value = "Target";
            dgvAGVInfo.Rows[7].Cells[0].Value = "Match (%)";
            dgvAGVInfo.Rows[8].Cells[0].Value = "Last Update";

            // 調整 DataGridView 高度
            int Height = 0;
            for (int i = 0; i < dgvAGVInfo.Rows.Count; ++i)
                Height += dgvAGVInfo.Rows[i].Height;
            dgvAGVInfo.Height = (int)(Height * 1.015);
        }

        /// <summary>
        /// 更新顯示 AGV 狀態的 DataGridView ， status 參數給 null 可清空 DataGridView
        /// </summary>
        private void GUI_UpdateAGVInfoMonitor(AGVStatus status)
        {
            dgvAGVInfo.InvokeIfNecessary(() => { if (dgvAGVInfo.Rows[0].Cells[1].Value?.ToString() != status?.Name.ToString()) dgvAGVInfo.Rows[0].Cells[1].Value = status?.Name.ToString(); });
            dgvAGVInfo.InvokeIfNecessary(() => { if (dgvAGVInfo.Rows[1].Cells[1].Value?.ToString() != status?.Description.ToString()) dgvAGVInfo.Rows[1].Cells[1].Value = status?.Description.ToString(); });
            dgvAGVInfo.InvokeIfNecessary(() => { if (dgvAGVInfo.Rows[2].Cells[1].Value?.ToString() != status?.Battery.ToString("F2")) dgvAGVInfo.Rows[2].Cells[1].Value = status?.Battery.ToString("F2"); });
            dgvAGVInfo.InvokeIfNecessary(() => { if (dgvAGVInfo.Rows[3].Cells[1].Value?.ToString() != status?.X.ToString("F2")) dgvAGVInfo.Rows[3].Cells[1].Value = status?.X.ToString("F2"); });
            dgvAGVInfo.InvokeIfNecessary(() => { if (dgvAGVInfo.Rows[4].Cells[1].Value?.ToString() != status?.Y.ToString("F2")) dgvAGVInfo.Rows[4].Cells[1].Value = status?.Y.ToString("F2"); });
            dgvAGVInfo.InvokeIfNecessary(() => { if (dgvAGVInfo.Rows[5].Cells[1].Value?.ToString() != status?.Toward.ToString("F2")) dgvAGVInfo.Rows[5].Cells[1].Value = status?.Toward.ToString("F2"); });
            dgvAGVInfo.InvokeIfNecessary(() => { if (dgvAGVInfo.Rows[6].Cells[1].Value?.ToString() != status?.GoalName.ToString()) dgvAGVInfo.Rows[6].Cells[1].Value = status?.GoalName.ToString(); });
            dgvAGVInfo.InvokeIfNecessary(() => { if (dgvAGVInfo.Rows[7].Cells[1].Value?.ToString() != status?.MapMatch.ToString("F2")) dgvAGVInfo.Rows[7].Cells[1].Value = status?.MapMatch.ToString("F2"); });
            dgvAGVInfo.InvokeIfNecessary(() => { if (dgvAGVInfo.Rows[8].Cells[1].Value?.ToString() != status?.TimeStamp.ToString("yyyy/MM/dd HH:mm:ss.fff")) dgvAGVInfo.Rows[8].Cells[1].Value = status?.TimeStamp.ToString("yyyy/MM/dd HH:mm:ss.fff"); });
        }

        /// <summary>
        /// 更新顯示 AGV 清單的 ComboBox ，使用 add 參數決定為新增或移除
        /// </summary>
        private void GUI_UpdateAGVList(string agvName, bool add = true)
        {
            cbAGVList.InvokeIfNecessary(() =>
            {
                if (add & !cbAGVList.Items.Contains(agvName))
                    cbAGVList.Items.Add(agvName);
                if (!add & cbAGVList.Items.Contains(agvName))
                    cbAGVList.Items.Remove(agvName);
            });
        }

        /// <summary>
        /// 更新顯示 AGV 連線狀態與數量的 ToolStripStatusLabel
        /// </summary>
        private void GUI_UpdateConnectStatusMonitor(System.Drawing.Image img, string text)
        {
            statusStrip1.InvokeIfNecessary(() => tsslConnectStatus.Image = img);
            statusStrip1.InvokeIfNecessary(() => tsslConnectStatus.Text = text);
        }

		/// <summary>
		/// 更新地圖資訊
		/// </summary>
		private void GUI_UpdateMapInfo(string name, string hash, string time)
		{
			lblMapName.InvokeIfNecessary(() => { lblMapName.Text = name; });
			lblMapHash.InvokeIfNecessary(() => { lblMapHash.Text = hash; });
			lblMapLastEditTime.InvokeIfNecessary(() => { lblMapLastEditTime.Text = time; });
		}

		/// <summary>
		/// 清除地圖資訊
		/// </summary>
		private void GUI_ClearMapInfo()
		{
			GUI_UpdateMapInfo("----", "----", "----");
		}

        #endregion

        #region 路徑搜尋

        /// <summary>
        /// A星路徑搜尋
        /// </summary>
        private readonly PairStar aStar = new PairStar();

        /// <summary>
        /// 路徑 ID
        /// </summary>
        private int pathID = -1;

        /// <summary>
        /// 起點座標
        /// </summary>
        private IPair start = new Pair();

        private void GLUI_GLDoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            var end = GLUI.ScreenToGL(mouse.X, mouse.Y);
            var path = aStar.FindPath(start, end)?.ToList();

            if (path != null)
            {
                GLCMD.CMD.SaftyEditMultiGeometry<IPair>(pathID, true, o => { o.Clear(); o.AddRange(path); });
                start = end;
            }
        }

        private void GLUI_LoadMapEvent(object sender, LoadMapEventArgs e)
		{
			// 清除 AGV 殘留圖示與 AGV 資訊
			lock (agvs)
			{
				foreach (var agv in agvs)
				{
					GLCMD.CMD.DeleteAGV(agv.Value.AGVID);
				}
				agvs.Clear();
			}

			// 更新地圖資訊
			System.IO.FileInfo fi = new System.IO.FileInfo(e.MapPath);
			GUI_UpdateMapInfo(fi.Name, GLCMD.CMD.MapHash, fi.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss"));

			// A* 載入地圖，並註冊 A* 路徑編號
			aStar.LoadMap(e.MapPath);
            pathID = GLCMD.CMD.AddMultiStripLine("Path", null);
		}

        private void GLUI_PenMapEvent(object sender, PenMapEventArgs e)
        {
            aStar.Insert(e.Data);
        }

        private void GLUI_EraserMapEvent(object sender, EraserMapEventArgs e)
        {
            aStar.Remove(e.Range.Min, e.Range.Max);
        }
        #endregion 路徑搜尋

        #region 範例 / 測試

        /// <summary>
        /// 命令範例
        /// </summary>
        private void CMDDemo()
        {
            // 使用者用指令加入圖形(可復原)
            GLCMD.CMD.Do($"Add,{GLCMD.CMD.SerialNumber.Next()},ChargingDocking,-2000,0,0");
            GLCMD.CMD.Do($"Add,{GLCMD.CMD.SerialNumber.Next()},ForbiddenArea2,0,0,3000,3000");
            GLCMD.CMD.Do($"Add,{GLCMD.CMD.SerialNumber.Next()},ForbiddenArea,4000,4000,7000,7000");

            // 使用者用函式加入圖形(可復原)
            GLCMD.CMD.DoAddSingleTowardPair("General", 0, 2000, 45);

            // 使用者用函式加入複合圖形(不可復原)
            Random random = new Random();
            List<IPair> list = new List<IPair>();
            for (int ii = 0; ii < 100000; ++ii)
            {
                list.Add(new Pair(random.Next(-10000, 10000), random.Next(-10000, 10000)));
            }
            GLCMD.CMD.SaftyEditMultiGeometry<IPair>(GLCMD.CMD.ObstaclePointsID, true, o => o.AddRangeIfNotNull(list));
        }

        /// <summary>
        /// AGV 範例
        /// </summary>
        private void AGVDemo()
        {
            int agv1 = GLCMD.CMD.SerialNumber.Next();
            int agv2 = GLCMD.CMD.SerialNumber.Next();
            Random random = new Random();
            new Thread(() =>
            {
                while (true)
                {
                    GLCMD.CMD.AddAGV(agv1, "AGV-1", random.Next(0, 5000), random.Next(0, 5000), random.Next(0, 360));
                    GLCMD.CMD.AddAGV(agv2, random.Next(-5000, 0), random.Next(-5000, 0), random.Next(0, 360));
                    Thread.Sleep(1000);
                }
            })
            {
                IsBackground = true
            }.Start();
        }

        #region Socket 測試

        /// <summary>
        /// 通訊用伺服器端
        /// </summary>
        private SerialServer server;

        /// <summary>
        /// 紀錄 AGV 名稱與對應 AGV 資訊包的字典
        /// </summary>
        private readonly Dictionary<string, AGVInfo> agvs = new Dictionary<string, AGVInfo>();

        /// <summary>
        /// 紀錄 Socket 事件資訊的佇列
        /// </summary>
        /// <remarks>
        /// 這裡加 reonly 是因為我把這個成員當執行緒鎖(lock)，所以要保證他記憶體位置不會被修改
        /// </remarks>
        private readonly ConcurrentQueue<EventArgs> socketEventQueue = new ConcurrentQueue<EventArgs>();

        /// <summary>
        /// 處理 Socket 事件的執行緒
        /// </summary>
        private Thread socketEventHandler;

        /// <summary>
        /// 處理 Socket 事件的執行緒鎖
        /// </summary>
        private readonly ManualResetEvent mreSocketEventTask = new ManualResetEvent(false);

        /// <summary>
        /// Socket 範例
        /// </summary>
        private void SocketTest()
        {
            server = new SerialServer();
            server.ConnectStatusChangedEvent += Server_ConnectStatusChangedEvent;
            server.ListenStatusChangedEvent += Server_ListenStatusChangedEvent;
            server.ReceivedSerialDataEvent += ReceivedSerialDataEvent;
            server.StartListening(8000);

            socketEventHandler = new Thread(HandleSocketEventArgs);
            socketEventHandler.IsBackground = true;
            socketEventHandler.Start();
        }

        private void Server_ConnectStatusChangedEvent(object sender, ConnectStatusChangedEventArgs e)
        {
            Console.WriteLine($"{e.StatusChangedTime} 和 {e.RemoteInfo} 的連線狀態改變 >> {e.ConnectStatus}");
            lock (socketEventQueue)
            {
                socketEventQueue.Enqueue(e);
                mreSocketEventTask.Set();
            }
        }

        private void Server_ListenStatusChangedEvent(object sender, ListenStatusChangedEventArgs e)
        {
            Console.WriteLine($"{e.StatusChangedTime} 監聽狀態改變 >> {e.ListenStatus}");
            lock (socketEventQueue)
            {
                socketEventQueue.Enqueue(e);
                mreSocketEventTask.Set();
            }
        }

        private void ReceivedSerialDataEvent(object sender, ReceivedSerialDataEventArgs e)
        {
            Console.WriteLine($"{e.ReceivedTime.ToString()} 收到來自 {e.RemoteInfo.ToString()} 的訊息 >> {e.Data.GetType()}");
            lock (socketEventQueue)
            {
                socketEventQueue.Enqueue(e);
                mreSocketEventTask.Set();
            }
        }

        /// <summary>
        /// 將 <see cref="ConcurrentQueue{T}"/> 所有的內容移除，並以 <see cref="IEnumerable{T}"/> 方式回傳
        /// </summary>
        private IEnumerable<T> DequeueToIEnumer<T>(ConcurrentQueue<T> queqe)
        {
            T item = default(T);
            while (queqe.TryDequeue(out item))
            {
                yield return item;
                item = default(T);
            }
        }

        /// <summary>
        /// 分配 Socket 事件
        /// </summary>
        private void HandleSocketEventArgs(EventArgs e)
        {
            if (e is ConnectStatusChangedEventArgs)
            {
                HandleSocketEventArgs(e as ConnectStatusChangedEventArgs);
            }
            else if (e is ListenStatusChangedEventArgs)
            {
                HandleSocketEventArgs(e as ListenStatusChangedEventArgs);
            }
            else if (e is ReceivedSerialDataEventArgs)
            {
                HandleSocketEventArgs(e as ReceivedSerialDataEventArgs);
            }
        }

        /// <summary>
        /// Socket 事件處理區
        /// </summary>
        private void HandleSocketEventArgs()
        {
            while (true)
            {
                var eventArgsList = new List<EventArgs>();

                // 將 socketEventQueue 資料全部搬移到 eventArgsList，並重置接收資料鎖
                lock (socketEventQueue)
                {
                    // ToList() 是為了讓 QueqeToIEnumer 真的搬移資料
                    eventArgsList = DequeueToIEnumer(socketEventQueue).ToList();
                    mreSocketEventTask.Reset();
                }

                // 這裡不再鎖住，讓程式可以邊執行 HandleSocketEventArgs 邊接收指令
                foreach (var eventArgs in eventArgsList)
                {
                    HandleSocketEventArgs(eventArgs);
                }

                // 執行完指令後等待下一筆指令進來
                mreSocketEventTask.WaitOne();
            }
        }

        /// <summary>
        /// 處理 Socket 連線狀態變化事件
        /// </summary>
        private void HandleSocketEventArgs(ConnectStatusChangedEventArgs e)
        {
            // 當有 AGV 中斷連線時
            if (e.ConnectStatus == EConnectStatus.Disconnect)
            {
                lock (agvs)
                {
                    // 此行最末端 .ToList() 確保 FindAGVNameByIPPort 展開結果
                    // 避免邊刪除(agvs.Remove)邊查(FindAGVNameByIPPort)
                    foreach (var leave in FindAGVNameByIPPort(e.RemoteInfo.ToString()).ToList())
                    {
                        // 若離開的 AGV 正好為清單選擇中的 AGV ，則清空顯示 AGV 狀態的 DataGridView
                        if (cbAGVList.InvokeIfNecessary((a) => a.SelectedItem?.ToString()) == leave)
                            GUI_UpdateAGVInfoMonitor(null);
                        // 更新 AGV 清單
                        GUI_UpdateAGVList(leave, false);
                        // 刪除 AGV 圖像
                        GLCMD.CMD.DeleteAGV(agvs[leave].AGVID);
                        // 刪除 AGV 路徑圖像
                        // 順帶一提 SingleLine 這個類別不是拿來畫路徑用的
                        // 請參考 MultiLine
                        GLCMD.CMD.DeleteMulti(agvs[leave].PathID);
                        // 刪除 AGV 資訊
                        agvs.Remove(leave);
                    }
                }
            }

            // 更新連線狀態
            lock (agvs)
            {
                if (!agvs.Any())
                {
                    GUI_UpdateConnectStatusMonitor(Resources.CircleYellow, agvs.Count.ToString());
                }
                else
                {
                    GUI_UpdateConnectStatusMonitor(Resources.CircleGreen, agvs.Count.ToString());
                }
            }
        }

        /// <summary>
        /// 處理 Socket 監聽狀態變化事件
        /// </summary>
        private void HandleSocketEventArgs(ListenStatusChangedEventArgs e)
        {
            // 更新連線狀態
            if (e.ListenStatus == EListenStatus.Idle)
            {
                GUI_UpdateConnectStatusMonitor(Resources.CircleRed, "0");
            }
            else
            {
                GUI_UpdateConnectStatusMonitor(Resources.CircleYellow, "0");
            }
        }

        /// <summary>
        /// 處理 Socket 接收資料事件
        /// </summary>
        private void HandleSocketEventArgs(ReceivedSerialDataEventArgs e)
        {
            if (e.Data is AGVStatus)
            {
                var status = e.Data as AGVStatus;
                lock (agvs)
                {
                    UpdateAGV(e.RemoteInfo.ToString(), status);
                    // 繪製 AGV
                    GLCMD.CMD.AddAGV(agvs[status.Name].AGVID, agvs[status.Name].Status.Name, agvs[status.Name].Status.X, agvs[status.Name].Status.Y, agvs[status.Name].Status.Toward);
                }
                // 更新 AGV 清單
                GUI_UpdateAGVList(status.Name);
                // 若更新中的 AGV 為清單選擇中的，則更新顯示 AGV 狀態的 DataGridView
                if (cbAGVList.InvokeIfNecessary((a) => a.SelectedItem?.ToString()) == status.Name)
                    GUI_UpdateAGVInfoMonitor(status);
                // 若清單選擇為空，則自動選擇正在更新的 AGV
                else if (cbAGVList.InvokeIfNecessary((a) => a.SelectedIndex < 0))
                    cbAGVList.InvokeIfNecessary(() => cbAGVList.SelectedItem = status.Name);

                // 更新連線狀態
                lock (agvs)
                {
                    GUI_UpdateConnectStatusMonitor(Resources.CircleGreen, agvs.Count.ToString());
                }
            }
            else if (e.Data is AGVPath)
            {
                var path = e.Data as AGVPath;

                int pathID = UpdateAGVPath(path.Name, path);
                if (pathID != -1)
                {
                    GLCMD.CMD.SaftyEditMultiGeometry<IPair>(pathID, true, (line) =>
                    {
                        line.Clear();
                        line.AddRangeIfNotNull(PathToPairCollection(path));
                    });
				}
			}
			else if (e.Data is RequestMapList)
			{
				if ((e.Data as RequestMapList).Response == null) return;
				Thread thd = new Thread(() => 
				{
					var tmp = e.Data as RequestMapList;
					if (tmp.Response.Count > 0)
					{
						string text = "Please Input the Map Index:\r\n";
						for (int i = 0; i < tmp.Response.Count; ++i)
						{
							text += (i + 1).ToString() + ". " + tmp.Response.ElementAt(i).ToString();
							if (i != (tmp.Response.Count - 1)) text += "\r\n";
						}

						string result = "";
						int selectedIndex = -1;
						if (InputBox("Choose Map", text, ref result) == DialogResult.OK)
						{
							if (int.TryParse(result, out selectedIndex))
							{
								if (selectedIndex > 0 && selectedIndex <= tmp.Response.Count)
								{
									// 尾端為 * 符號的，代表是車子當前使用地圖
									string fileName = tmp.Response.ElementAt(selectedIndex - 1);
									if (fileName.EndsWith("*")) fileName.TrimEnd('*');
									SendGetMap(fileName);
								}
							}
						}
					}
				});
				thd.SetApartmentState(ApartmentState.STA);
				thd.Start();
			}
			else if (e.Data is GetMap)
			{
				if ((e.Data as GetMap).Response == null) return;
				Thread thd = new Thread(() =>
				{
					string msg = $"Receive Map File: {(e.Data as GetMap).Response.ToString()}.\r\nSave it?";
					if (MessageBox.Show(msg, "Receive Map File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					{
						FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
						if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
						(e.Data as GetMap).Response.SaveAs(folderBrowserDialog.SelectedPath);
					}
				});
				thd.SetApartmentState(ApartmentState.STA);
				thd.Start();
			}
			else if (e.Data is UploadMapToAGV)
			{
				if ((e.Data as UploadMapToAGV).Response == true)
					Console.WriteLine($"Upload Map ({(e.Data as UploadMapToAGV).Require.Name}) To AGV Success!");
			}
			else if (e.Data is ChangeMap)
			{
				if ((e.Data as ChangeMap).Response == true)
					Console.WriteLine($"Change Map ({(e.Data as ChangeMap).Require}) Success!");
			}
		}

		#region 地圖處理

		/// <summary>
		/// 傳送要求地圖清單的命令
		/// </summary>
		private void SendRequestMapListCommand()
		{
			string agvName = cbAGVList.InvokeIfNecessary((a) => a.Text);
			if (agvs.Keys.Contains(agvName))
			{
				server.Send(agvs[agvName].IPPort, new RequestMapList(null));
			}
		}

		/// <summary>
		/// 傳送要求地圖的命令
		/// </summary>
		/// <param name="fileName">地圖名稱</param>
		private void SendGetMap(string fileName)
		{
			string agvName = cbAGVList.InvokeIfNecessary((a) => a.Text);
			if (agvs.Keys.Contains(agvName))
			{
				server.Send(agvs[agvName].IPPort, new GetMap(fileName));
			}
		}

		/// <summary>
		/// 傳送上傳地圖至 AGV 上的命令
		/// </summary>
		private void SendUploadMapCommand()
		{
			string agvName = cbAGVList.InvokeIfNecessary((a) => a.Text);
			if (agvs.Keys.Contains(agvName))
			{
				string fileName = "";
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.InitialDirectory = Application.StartupPath;
				openFileDialog.Filter = "Map Files|*.map|All Files|*.*";
				openFileDialog.FilterIndex = 1;
				if (openFileDialog.ShowDialog() != DialogResult.OK) return;
				fileName = openFileDialog.FileName;

				server.Send(agvs[agvName].IPPort, new UploadMapToAGV(new FileInfo(fileName)));
			}
		}

		/// <summary>
		/// 傳送 AGV 更新目前使用地圖的命令
		/// </summary>
		private void SendChangeMapCommand()
		{
			string agvName = cbAGVList.InvokeIfNecessary((a) => a.Text);
			if (agvs.Keys.Contains(agvName))
			{
				string fileName = "";
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.InitialDirectory = Application.StartupPath;
				openFileDialog.Filter = "Map Files|*.map|All Files|*.*";
				openFileDialog.FilterIndex = 1;
				if (openFileDialog.ShowDialog() != DialogResult.OK) return;
				fileName = openFileDialog.FileName;

				server.Send(agvs[agvName].IPPort, new ChangeMap(System.IO.Path.GetFileName(fileName)));
			}
		}

		#endregion

		#region AGV 資訊處理

		/// <summary>
		/// 從 <see cref="agvs"/> 中根據 IP:Port 找出所有應的車子名稱。若 IP:Port 不存在則回傳 <see cref="string.Empty"/>
		/// </summary>
		private IEnumerable<string> FindAGVNameByIPPort(string ipport)
        {
            lock (agvs)
            {
                return agvs.Where((agv) => agv.Value.IPPort == ipport).Select((agv) => agv.Key);
            }
        }

        /// <summary>
        /// 根據 AGV 名稱更新 <see cref="agvs"/> 狀態。若對象不存在則改為新增，並在 <see cref="GLCMD"/> 中註冊一個路徑 ID
        /// </summary>
        private void UpdateAGV(string ipport, AGVStatus status)
        {
            lock (agvs)
            {
                // 確認是新增還是更新項目
                if (agvs.Keys.Contains(status.Name))
                {
                    // 更新原有項目
                    agvs[status.Name].Status = status;
                    agvs[status.Name].IPPort = ipport;
                }
                else
                {
                    // 新增項目並記錄 AGV 圖像識別碼
                    AGVInfo agv = new AGVInfo();
                    agv.Status = status;
                    agv.AGVID = GLCMD.CMD.SerialNumber.Next();
                    agv.IPPort = ipport;
                    agv.PathID = GLCMD.CMD.AddMultiStripLine("Path", null);
                    agvs.Add(status.Name, agv);
				}
            }
        }

        /// <summary>
        /// 根據 AGV 名稱更新路徑，若成功更新則回傳 <see cref="AGVInfo.PathID"/> ，若 AGV 名稱不存在 <see cref="agvs"/> 則不更新並回傳 -1
        /// </summary>
        private int UpdateAGVPath(string agvName, AGVPath path)
        {
            lock (agvs)
            {
                if (agvs.Keys.Contains(path.Name))
                {
                    // 更新原有項目
                    agvs[path.Name].Path = path;
                    return agvs[path.Name].PathID;
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// 將 <see cref="AGVPath"/> 轉為 <see cref="IPair"/> 集合
        /// </summary>
        private IEnumerable<IPair> PathToPairCollection(AGVPath path)
        {
            for (int ii = 0; ii < path.PathX.Count; ii++)
            {
                yield return new Pair(path.PathX[ii], path.PathY[ii]);
            }
        }

		#endregion

		#endregion

		#endregion

		#region 權限控制

		/// <summary>
		/// 是否登入
		/// </summary>
		private bool isLogIn = false;

		/// <summary>
		/// 權限等級， -1 為未登入， 0 為最高等級， 99 為最低等級
		/// </summary>
		private int logLevel = -1;

		/// <summary>
		/// 當前登入的使用者
		/// </summary>
		private string currentUser = "";

		private bool LogIn(string password)
		{
			bool result = false;
			if (!isLogIn)
			{
				if (password == "oct27635744")
				{
					currentUser = "CASTEC";
					isLogIn = true;
					logLevel = 0;
					result = true;
				}
				else if (password == "octvmp6ru03")
				{
					currentUser = "TSMC";
					isLogIn = true;
					logLevel = 1;
					result = true;
				}
			}
			return result;
		}

		private void LogOut()
		{
			currentUser = "";
			logLevel = -1;
			isLogIn = false;
		}

		#endregion
	}

	/// <summary>
	/// AGV 資訊
	/// </summary>
	/// <remarks>一個 AGV 狀態對應到一個 AGV 路徑</remarks>
	public class AGVInfo
    {
        /// <summary>
        /// AGV 狀態
        /// </summary>
        public AGVStatus Status;

        /// <summary>
        /// AGV 路徑
        /// </summary>
        public AGVPath Path;

        /// <summary>
        /// AGV 圖像識別碼
        /// </summary>
        public int AGVID;

        /// <summary>
        /// AGV 路徑圖像識別碼
        /// </summary>
        public int PathID;

        /// <summary>
        /// 遠端的 IP 與 Port
        /// </summary>
        /// <remarks>格式為 IP:Port</remarks>
        public string IPPort;
    }
}