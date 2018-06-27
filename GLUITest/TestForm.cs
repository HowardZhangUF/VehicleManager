using AsyncSocket;
using Geometry;
using GLCore;
using GLStyle;
using GLUI;
using PairAStar;
using SerialData;
using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using LittleGhost;
using GLUITest.Properties;
using System.Collections.Concurrent;

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
            cmbSelectType.Items.Add(nameof(SinglePairInfo));
            cmbSelectType.Items.Add(nameof(SingleTowardPairInfo));
            cmbSelectType.Items.Add(nameof(SingleLineInfo));
            cmbSelectType.Items.Add(nameof(SingleAreaInfo));

            // 資料綁定
            var binding = new Binding(nameof(Text), GLCMD.CMD, nameof(GLCMD.MapHash));
            binding.Format += (sender, e) => e.Value = $"Map Editor, Map Hash:{e.Value}";
            DataBindings.Add(binding);

            // 加入事件
            GLUI.LoadMapEvent += GLUI_LoadMapEvent;
            GLUI.PenMapEvent += GLUI_PenMapEvent;
            GLUI.EraserMapEvent += GLUI_EraserMapEvent;
            GLUI.CommandOnClick += GLUI_CommandOnClick;

            // 加入範例 / 測試
            //CMDDemo();
            //AGVDemo();
        }

        private void frmTest_Load(object sender, EventArgs e)
        {
            SocketTest();
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
                        BindingSource singleTowerPairInfoSource = new BindingSource(GLCMD.CMD.SinglePairInfo, null);
                        dgvInfo.DataSource = singleTowerPairInfoSource;
                    }
                    break;

                case nameof(SingleTowardPairInfo):
                    {
                        BindingSource singleTowerPairInfoSource = new BindingSource(GLCMD.CMD.SingleTowerPairInfo, null);
                        dgvInfo.DataSource = singleTowerPairInfoSource;
                    }
                    break;

                case nameof(SingleLineInfo):
                    {
                        BindingSource singleTowerPairInfoSource = new BindingSource(GLCMD.CMD.SingleLineInfo, null);
                        dgvInfo.DataSource = singleTowerPairInfoSource;
                    }
                    break;

                case nameof(SingleAreaInfo):
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
                case nameof(SinglePairInfo):
                    {
                        int x = (int)(dgvInfo[nameof(SinglePairInfo.X), rowIndex].Value);
                        int y = (int)(dgvInfo[nameof(SinglePairInfo.Y), rowIndex].Value);

                        GLUI.Focus(x, y);
                    }
                    break;

                case nameof(SingleTowardPairInfo):
                    {
                        int x = (int)(dgvInfo[nameof(SingleTowardPairInfo.X), rowIndex].Value);
                        int y = (int)(dgvInfo[nameof(SingleTowardPairInfo.Y), rowIndex].Value);

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
                case nameof(SinglePairInfo):
                    {
                        return (int)(dgvInfo[nameof(SinglePairInfo.ID), rowIndex].Value);
                    }

                case nameof(SingleTowardPairInfo):
                    {
                        return (int)(dgvInfo[nameof(SingleTowardPairInfo.ID), rowIndex].Value);
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

                case nameof(SingleTowardPairInfo):
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
		private ManualResetEvent mreSocketEventTask = new ManualResetEvent(false);

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
			}
			mreSocketEventTask.Set();
		}

        private void Server_ListenStatusChangedEvent(object sender, ListenStatusChangedEventArgs e)
        {
            Console.WriteLine($"{e.StatusChangedTime} 監聽狀態改變 >> {e.ListenStatus}");
            lock (socketEventQueue)
            {
                socketEventQueue.Enqueue(e);
			}
			mreSocketEventTask.Set();
		}

        private void ReceivedSerialDataEvent(object sender, ReceivedSerialDataEventArgs e)
        {
            Console.WriteLine($"{e.ReceivedTime.ToString()} 收到來自 {e.RemoteInfo.ToString()} 的訊息 >> {e.Data.GetType()}");
            lock (socketEventQueue)
            {
                socketEventQueue.Enqueue(e);
			}
			mreSocketEventTask.Set();
		}

        /// <summary>
        /// Socket 事件處理區
        /// </summary>
        private void HandleSocketEventArgs()
        {
            while (true)
            {
                EventArgs eventArgs;
                lock (socketEventQueue)
                {
                    if (!socketEventQueue.Any() || !socketEventQueue.TryDequeue(out eventArgs))
                    {
                        eventArgs = null;
					}
				}

                // 把 eventArgs 從 socketEventQueue 拿出來後，就跟 socketEventQueue 無關了
                // 於是不再繼續鎖住
                if (eventArgs != null)
                {
                    if (eventArgs is ConnectStatusChangedEventArgs)
                    {
                        HandleSocketEventArgs(eventArgs as ConnectStatusChangedEventArgs);
                    }
                    else if (eventArgs is ListenStatusChangedEventArgs)
                    {
                        HandleSocketEventArgs(eventArgs as ListenStatusChangedEventArgs);
                    }
                    else if (eventArgs is ReceivedSerialDataEventArgs)
                    {
                        HandleSocketEventArgs(eventArgs as ReceivedSerialDataEventArgs);
                    }
                    else
                    {

                    }
                }
				else
				{
					mreSocketEventTask.WaitOne();
					mreSocketEventTask.Reset();
				}
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
            if (!agvs.Any())
            {
                statusStrip1.InvokeIfNecessary(() => tsslConnectStatus.Image = Resources.circle_yellow);
            }
            else
            {
                statusStrip1.InvokeIfNecessary(() => tsslConnectStatus.Image = Resources.circle_green);
            }
            statusStrip1.InvokeIfNecessary(() => tsslConnectStatus.Text = agvs.Count.ToString());
        }

        /// <summary>
        /// 處理 Socket 監聽狀態變化事件
        /// </summary>
        private void HandleSocketEventArgs(ListenStatusChangedEventArgs e)
        {
            // 更新連線狀態
            if (e.ListenStatus == EListenStatus.Idle)
            {
                statusStrip1.InvokeIfNecessary(() => tsslConnectStatus.Image = Resources.circle_red);
                statusStrip1.InvokeIfNecessary(() => tsslConnectStatus.Text = "0");
            }
            else
            {
                statusStrip1.InvokeIfNecessary(() => tsslConnectStatus.Image = Resources.circle_yellow);
                statusStrip1.InvokeIfNecessary(() => tsslConnectStatus.Text = "0");
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

            // 更新連線狀態
            statusStrip1.InvokeIfNecessary(() => tsslConnectStatus.Image = Resources.circle_green);
            statusStrip1.InvokeIfNecessary(() => tsslConnectStatus.Text = agvs.Count.ToString());
        }

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