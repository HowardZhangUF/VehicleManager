using Geometry;
using GLCore;
using GLStyle;
using GLUI.Language;
using GLUI.Properties;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GLUI
{
    public partial class GLUICtrl : UserControl
    {
        #region 圖示

        private System.Drawing.Image AddImage { get; } = (System.Drawing.Image)Resources.ResourceManager.GetObject("Add");
        private System.Drawing.Image AreaImage { get; } = (System.Drawing.Image)Resources.ResourceManager.GetObject("Area");
        private System.Drawing.Image ChangeStyleImage { get; } = (System.Drawing.Image)Resources.ResourceManager.GetObject("ChangeStyle");
        private System.Drawing.Image DeleteImage { get; } = (System.Drawing.Image)Resources.ResourceManager.GetObject("Delete");
        private System.Drawing.Image EraserImage { get; } = (System.Drawing.Image)Resources.ResourceManager.GetObject("Eraser");
        private System.Drawing.Image LineImage { get; } = (System.Drawing.Image)Resources.ResourceManager.GetObject("Line");
        private System.Drawing.Image MoveImage { get; } = (System.Drawing.Image)Resources.ResourceManager.GetObject("Move");
        private System.Drawing.Image PairImage { get; } = (System.Drawing.Image)Resources.ResourceManager.GetObject("Pair");
        private System.Drawing.Image PenImage { get; } = (System.Drawing.Image)Resources.ResourceManager.GetObject("Pen");
        private System.Drawing.Image RedoImage { get; } = (System.Drawing.Image)Resources.ResourceManager.GetObject("Redo");
        private System.Drawing.Image RenameImage { get; } = (System.Drawing.Image)Resources.ResourceManager.GetObject("Rename");
        private System.Drawing.Image SelectImage { get; } = (System.Drawing.Image)Resources.ResourceManager.GetObject("Select");
        private System.Drawing.Image TowardPairImage { get; } = (System.Drawing.Image)Resources.ResourceManager.GetObject("TowardPair");
        private System.Drawing.Image UndoImage { get; } = (System.Drawing.Image)Resources.ResourceManager.GetObject("Undo");

        #endregion 圖示

        #region 顏色

        /// <summary>
        /// X 軸顏色
        /// </summary>
        public IColor AxisXColor { get; } = new Color(System.Drawing.Color.Crimson);

        /// <summary>
        /// Y 軸顏色
        /// </summary>
        public IColor AxisYColor { get; } = new Color(System.Drawing.Color.DarkOliveGreen);

        /// <summary>
        /// 背景色
        /// </summary>
        public IColor BackgroundColor { get; } = new Color(System.Drawing.Color.Wheat);

        /// <summary>
        /// 網格顏色
        /// </summary>
        public IColor GridColor { get; } = new Color(System.Drawing.Color.Gray);

        #endregion 顏色

        #region 視角

        /// <summary>
        /// 縮放
        /// </summary>
        private double mZoom = 10.0;

        /// <summary>
        /// 平移
        /// </summary>
        public IPair Translate { get; } = new Pair();

        /// <summary>
        /// 縮放
        /// </summary>
        public double Zoom { get { return mZoom; } set { mZoom = Math.Max(0.1, Math.Min(1000.0, value)); } }

        /// <summary>
        /// 設定焦點
        /// </summary>
        public void Focus<T>(T focus) where T : IPair
        {
            Focus(focus.X, focus.Y);
        }

        /// <summary>
        /// 設定焦點
        /// </summary>
        public void Focus(int x, int y)
        {
            Translate.X = -x;
            Translate.Y = -y;
        }

        #endregion 視角

        #region 顯示設定

        /// <summary>
        /// 網格頂點座標陣列
        /// </summary>
        private int[] mGridVertex = new int[] { };

        /// <summary>
        /// 坐標軸大小(mm)
        /// </summary>
        public int AxisLength { get; } = 1000;

        /// <summary>
        /// 網格大小(mm)
        /// </summary>
        public int GridSize { get; } = 1000;

        /// <summary>
        /// 是否畫坐標軸
        /// </summary>
        public bool ShowAxis { get; set; } = true;

        /// <summary>
        /// 是否畫網格
        /// </summary>
        public bool ShowGrid { get; set; } = true;

        #endregion 顯示設定

        #region 控制選單相關

        /// <summary>
        /// 滑鼠控制目標
        /// </summary>
        private int selectTarget = -1;

        /// <summary>
        /// 是否允許顯示物件操作選單
        /// </summary>
        public bool AllowObjectMenu { get; set; } = true;

        /// <summary>
        /// 是否允許顯示復原選單
        /// </summary>
        public bool AllowUndoMenu { get; set; } = true;

        /// <summary>
        /// 移動方式
        /// </summary>
        public EMoveType MoveType { get; private set; } = EMoveType.Stop;

        /// <summary>
        /// 滑鼠控制目標
        /// </summary>
        public int SelectTargetID { get { return selectTarget; } private set { selectTarget = value; GLCMD.DoSelect(value); } }

        /// <summary>
        /// 滑鼠的前一筆位置
        /// </summary>
        private IPair PreGLPosition { get; set; } = new Pair();

        /// <summary>
        /// 滑鼠提示
        /// </summary>
        private ToolTip ToolTip { get; } = new ToolTip();

        /// <summary>
        /// 加入物件右鍵選單觸發
        /// </summary>
        private void MenuAddItemOnClik(object sender, EventArgs e)
        {
            string style = (sender as ToolStripItem).Text;
            IPair center = (sender as ToolStripItem).Tag as IPair;
            switch (StyleManager.GetStyleType(style))
            {
                case nameof(IPairStyle):
                    GLCMD.DoAddSinglePair(style, center.X, center.Y);
                    break;

                case nameof(ITowardPairStyle):
                    GLCMD.DoAddSingleTowardPair(style, center.X, center.Y, 0);
                    break;

                case nameof(ILineStyle):
                    GLCMD.DoAddSingleLine(style, center.X - 500, center.Y, center.X + 500, center.Y);
                    break;

                case nameof(IAreaStyle):
                    GLCMD.DoAddSingleArea(style, center.X - 500, center.Y - 500, center.X + 500, center.Y + 500);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 更改樣式右鍵選單觸發
        /// </summary>
        private void MenuChangeStyleOnClik(object sender, EventArgs e)
        {
            string style = (sender as ToolStripItem).Text;
            GLCMD.DoChangeStyle(SelectTargetID, style);
        }

        /// <summary>
        /// 刪除物件右鍵選單觸發
        /// </summary>
        private void MenuDeleteOnClick(object sender, EventArgs e)
        {
            GLCMD.DoDelete(SelectTargetID);
        }

        /// <summary>
        /// 清除障礙點右鍵選單觸發
        /// </summary>
        private void MenuEraserOnClik(object sender, EventArgs e)
        {
            int size = (int)(sender as ToolStripItem).Tag;
            GLCMD.SetEraser(PreGLPosition, size);
        }

        /// <summary>
        /// 移動起點座標右鍵選單觸發
        /// </summary>
        private void MenuMoveBeginOnClik(object sender, EventArgs e)
        {
            MoveType = EMoveType.Begin;
        }

        /// <summary>
        /// 移動中心點座標右鍵選單觸發
        /// </summary>
        private void MenuMoveCenterOnClik(object sender, EventArgs e)
        {
            MoveType = EMoveType.Center;
        }

        /// <summary>
        /// 移動終點座標右鍵選單觸發
        /// </summary>
        private void MenuMoveEndOnClik(object sender, EventArgs e)
        {
            MoveType = EMoveType.End;
        }

        /// <summary>
        /// 移動最大值座標右鍵選單觸發
        /// </summary>
        private void MenuMoveMaxOnClik(object sender, EventArgs e)
        {
            MoveType = EMoveType.Max;
        }

        /// <summary>
        /// 移動最小值座標右鍵選單觸發
        /// </summary>
        private void MenuMoveMinOnClik(object sender, EventArgs e)
        {
            MoveType = EMoveType.Min;
        }

        /// <summary>
        /// 移動方向角右鍵選單觸發
        /// </summary>
        private void MenuMoveTowardOnClik(object sender, EventArgs e)
        {
            MoveType = EMoveType.Toward;
        }

        /// <summary>
        /// 畫障礙點右鍵選單觸發
        /// </summary>
        private void MenuPenOnClik(object sender, EventArgs e)
        {
            int step = int.Parse((sender as ToolStripItem).Tag.ToString());
            GLCMD.Redo(step);
        }

        /// <summary>
        /// 重做右鍵選單觸發
        /// </summary>
        private void MenuRedoOnClik(object sender, EventArgs e)
        {
            int step = int.Parse((sender as ToolStripItem).Tag.ToString());
            GLCMD.Redo(step);
        }

        /// <summary>
        /// 編輯物件右鍵選單觸發
        /// </summary>
        private void MenuRenameOnClik(object sender, EventArgs e)
        {
            new TextInput(Lang.Rename, Lang.Rename, "New Name", RenameDone).ShowDialog();
        }

        /// <summary>
        /// 選擇物件右鍵選單觸發
        /// </summary>
        private void MenuSelectOnClick(object sender, EventArgs e)
        {
            int id = int.Parse((sender as ToolStripItem).Tag.ToString());
            ShowEditMenu(id);
        }

        /// <summary>
        /// 選擇物件右鍵選單觸發
        /// </summary>
        private void MenuUndoOnClik(object sender, EventArgs e)
        {
            int step = int.Parse((sender as ToolStripItem).Tag.ToString());
            GLCMD.Undo(step);
        }

        /// <summary>
        /// 重新命名
        /// </summary>
        private void RenameDone(string newName)
        {
            GLCMD.DoRename(SelectTargetID, newName);
        }

        /// <summary>
        /// 展開物件編輯選單
        /// </summary>
        private void ShowEditMenu(int select)
        {
            if (!AllowObjectMenu) return;

            SelectTargetID = select;

            // 編輯
            var menu = new ContextMenuStrip();
            menu.Items.Add(Lang.Rename, RenameImage, MenuRenameOnClik);

            // 刪除
            menu.Items.Add(Lang.Delete, DeleteImage, MenuDeleteOnClick);

            // 移動
            string styleType = GLCMD.GetStyleType(SelectTargetID);
            var move = new ToolStripMenuItem(Lang.Move) { Image = MoveImage };
            switch (styleType)
            {
                case nameof(IPairStyle):
                    move.DropDownItems.Add(Lang.MoveCenter, MoveImage, MenuMoveCenterOnClik);
                    break;

                case nameof(ILineStyle):
                    move.DropDownItems.Add(Lang.MoveCenter, MoveImage, MenuMoveCenterOnClik);
                    move.DropDownItems.Add(Lang.MoveBegin, MoveImage, MenuMoveBeginOnClik);
                    move.DropDownItems.Add(Lang.MoveEnd, MoveImage, MenuMoveEndOnClik);
                    break;

                case nameof(IAreaStyle):
                    move.DropDownItems.Add(Lang.MoveCenter, MoveImage, MenuMoveCenterOnClik);
                    move.DropDownItems.Add(Lang.MoveMax, MoveImage, MenuMoveMaxOnClik);
                    move.DropDownItems.Add(Lang.MoveMin, MoveImage, MenuMoveMinOnClik);
                    break;

                case nameof(ITowardPairStyle):
                    move.DropDownItems.Add(Lang.MoveCenter, MoveImage, MenuMoveCenterOnClik);
                    move.DropDownItems.Add(Lang.MoveToward, MoveImage, MenuMoveTowardOnClik);
                    break;
            }
            menu.Items.Add(move);

            // 更改樣式
            ToolStripMenuItem style = new ToolStripMenuItem(Lang.ChangeStyle) { Image = ChangeStyleImage };
            foreach (var name in StyleManager.GetStyleNames(styleType))
            {
                style.DropDownItems.Add(name, ChangeStyleImage, MenuChangeStyleOnClik);
            }
            menu.Items.Add(style);

            menu.Show(MousePosition);
        }

        /// <summary>
        /// 顯示滑鼠在 GL 中的座標
        /// </summary>
        private void ShowMousePosition(IPair pos)
        {
            string msg = pos.ToString();
            string orgMsg = ToolTip.GetToolTip(SharpGLCtrl);
            if (msg != orgMsg)
            {
                ToolTip.SetToolTip(SharpGLCtrl, msg);
            }
        }

        /// <summary>
        /// 顯示選中多個物件時的選擇選單(並清除已選對象)
        /// </summary>
        private void ShowMultItemSelectMenu(IEnumerable<int> selects)
        {
            SelectTargetID = -1;

            if (!AllowObjectMenu) return;

            var menu = new ContextMenuStrip();
            foreach (var id in selects)
            {
                ToolStripItem item = new ToolStripButton() { Text = $"{Lang.Select} {id}", Tag = id, Image = SelectImage };
                item.Click += MenuSelectOnClick;
                menu.Items.Add(item);
            }

            menu.Show(MousePosition);
        }

        /// <summary>
        /// 顯示未選中物件時的選擇選單(並清除已選對象)
        /// </summary>
        private void ShowNoItemSelectMenu()
        {
            SelectTargetID = -1;

            if (!AllowUndoMenu) return;

            // 清除障礙點
            var menu = new ContextMenuStrip();
            ToolStripMenuItem eraser = new ToolStripMenuItem(Lang.Eraser) { Image = EraserImage };
            foreach (var size in new int[] { 10, 50, 100, 500, 1000 })
            {
                ToolStripItem item = new ToolStripButton() { Text = size.ToString(), Tag = size, Width = 30 };
                item.Click += MenuEraserOnClik;
                eraser.DropDownItems.Add(item);
            }
            menu.Items.Add(eraser);

            // 畫障礙點
            menu.Items.Add(Lang.Pen, PenImage, MenuPenOnClik);

            // 加入
            ToolStripMenuItem add = new ToolStripMenuItem(Lang.Add) { Image = AddImage };
            foreach (var typename in StyleManager.GetStyleNames())
            {
                System.Drawing.Image image = null;
                switch (StyleManager.GetStyleType(typename))
                {
                    case nameof(IPairStyle):
                        image = PairImage;
                        break;

                    case nameof(ITowardPairStyle):
                        image = TowardPairImage;
                        break;

                    case nameof(ILineStyle):
                        image = LineImage;
                        break;

                    case nameof(IAreaStyle):
                        image = AreaImage;
                        break;

                    default:
                        break;
                }
                ToolStripItem item = new ToolStripButton() { Text = typename, Tag = PreGLPosition, Image = image, Width = 150 };
                item.Click += MenuAddItemOnClik;
                add.DropDownItems.Add(item);
            }
            menu.Items.Add(add);

            // 復原
            int step = 0;
            ToolStripMenuItem undo = new ToolStripMenuItem(Lang.Undo) { Image = UndoImage };
            foreach (var cmd in GLCMD.GetDoHistory().Reverse())
            {
                ++step;
                ToolStripItem item = new ToolStripButton() { Text = cmd, Tag = step, Width = 250 };
                item.Click += MenuUndoOnClik;
                undo.DropDownItems.Add(item);
            }
            menu.Items.Add(undo);

            // 重做
            step = 0;
            ToolStripMenuItem redo = new ToolStripMenuItem(Lang.Redo) { Image = RedoImage };
            foreach (var cmd in GLCMD.GetUndoHistory())
            {
                ++step;
                ToolStripItem item = new ToolStripButton() { Text = cmd, Tag = step , Width = 250 };
                item.Click += MenuRedoOnClik;
                redo.DropDownItems.Add(item);
            }
            menu.Items.Add(redo);

            menu.Show(MousePosition);
        }

        #endregion 控制選單相關

        /// <summary>
        /// 建構式
        /// </summary>
        public GLUICtrl()
        {
            InitializeComponent();
            SharpGLCtrl.MouseWheel += SharpGLCtrl_MouseWheel; ;
            SharpGLCtrl.Click += SharpGLCtrl_Click;
            SharpGLCtrl.MouseMove += SharpGLCtrl_MouseMove;
            SharpGLCtrl.MouseDown += SharpGLCtrl_MouseDown;
        }

        /// <summary>
        /// 障礙點識別碼
        /// </summary>
        public int ObstaclePointsID { get; set; }

        private OpenGL GL { get { return SharpGLCtrl.OpenGL; } }

        /// <summary>
        /// 實際座標轉螢幕座標
        /// </summary>
        public IPair GLToScreen(int x, int y)
        {
            double mX = (x + Translate.X) / Zoom;
            double mY = (y + Translate.Y) / Zoom;
            return new Pair(mX + Width / 2, Height / 2 - mY);
        }

        /// <summary>
        /// 實際座標轉螢幕座標
        /// </summary>
        public IPair GLToScreen(IPair glPosition)
        {
            return GLToScreen(glPosition.X, glPosition.Y);
        }

        /// <summary>
        /// 螢幕座標轉實際座標
        /// </summary>
        public IPair ScreenToGL(int x, int y)
        {
            double mX = x - Width / 2;
            double mY = Height / 2 - y;
            return new Pair(mX * Zoom - Translate.X, mY * Zoom - Translate.Y);
        }

        /// <summary>
        /// 繪製坐標軸
        /// </summary>
        private void DrawAxis()
        {
            GL.LineWidth(3);

            // 畫XY軸
            GL.Begin(OpenGL.GL_LINES);
            {
                GL.Color(AxisXColor.GetFloats());
                GL.Vertex(0, 0, 0);
                GL.Vertex(AxisLength, 0, 0);
                GL.Color(AxisYColor.GetFloats());
                GL.Vertex(0, 0, 0);
                GL.Vertex(0, AxisLength, 0);
            }
            GL.End();

            // 使用虛線畫負XY軸
            GL.BeginStippleLine(ELinePattern._1111111011111110);
            {
                GL.Begin(OpenGL.GL_LINES);
                {
                    GL.Color(AxisXColor.GetFloats());
                    GL.Vertex(0, 0, 0);
                    GL.Vertex(-AxisLength, 0, 0);
                    GL.Color(AxisYColor.GetFloats());
                    GL.Vertex(0, 0, 0);
                    GL.Vertex(0, -AxisLength, 0);
                }
                GL.End();
            }
            GL.EndStippleLine();
        }

        /// <summary>
        /// 繪製格線
        /// </summary>
        private void DrawGrid()
        {
            if (Zoom > 40) return;

            // 顏色、大小
            GL.Color(GridColor.GetFloats());
            GL.LineWidth(1);

            // 座標設置
            GL.PushMatrix();
            {
                GL.LoadIdentity();
                GL.Translate((Translate.X) % GridSize, (Translate.Y) % GridSize, 0);
                if (mGridVertex.Length != 0)
                {
                    GL.DrawArray(OpenGL.GL_LINES, mGridVertex.Length / 2, mGridVertex);
                }
                else
                {
                    GenGridVertex();
                }
            }
            GL.PopMatrix();
        }

        /// <summary>
        /// 繪圖
        /// </summary>
        private void GDIDraw()
        {
            InitialDraw();
            if (ShowGrid) DrawGrid();
            if (ShowAxis) DrawAxis();
            GLCMD.Draw(GL);
            GLCMD.DrawText(GL, GLToText);
        }

        /// <summary>
        /// 產生網格頂點座標陣列
        /// </summary>
        private void GenGridVertex()
        {
            int rowCount = 20;
            int columonCount = 40;
            mGridVertex = new int[(rowCount * 2 + 1 + columonCount * 2 + 1) * 4];
            int index = 0;
            for (int row = -rowCount; row <= rowCount; ++row)
            {
                mGridVertex[index] = -columonCount * GridSize; // begin.X
                index++;
                mGridVertex[index] = row * GridSize; // begin.Y
                index++;
                mGridVertex[index] = columonCount * GridSize; // end.X
                index++;
                mGridVertex[index] = row * GridSize; // end.Y
                index++;
            }
            for (int column = -columonCount; column <= columonCount; ++column)
            {
                mGridVertex[index] = column * GridSize; // begin.X
                index++;
                mGridVertex[index] = -rowCount * GridSize; // begin.Y
                index++;
                mGridVertex[index] = column * GridSize; // end.X
                index++;
                mGridVertex[index] = rowCount * GridSize; // end.Y
                index++;
            }
        }

        /// <summary>
        /// GL 座標轉文字座標
        /// </summary>
        private IPair GLToText(IPair gl)
        {
            IPair screen = GLToScreen(gl.X, gl.Y);
            return new Pair(screen.X, Height - screen.Y);
        }

        /// <summary>
        /// 初始化畫布
        /// </summary>
        private void InitialDraw()
        {
            GL.ClearColor(BackgroundColor.R / 255.0f, BackgroundColor.G / 255.0f, BackgroundColor.B / 255.0f, BackgroundColor.A / 255.0f);
            GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            // 投影矩陣
            GL.MatrixMode(OpenGL.GL_PROJECTION);
            // MatrixMode 後要執行 LoadIdentity
            GL.LoadIdentity();
            // 畫布的大小（正交）
            GL.Ortho(-Zoom * Width / 2, Zoom * Width / 2, -Zoom * Height / 2, Zoom * Height / 2, -100, 1000);
            // 繪圖矩陣
            GL.MatrixMode(OpenGL.GL_MODELVIEW);
            // MatrixMode 後要執行 LoadIdentity
            GL.LoadIdentity();

            // 線條去鋸齒
            // gl.Enable(OpenGL.GL_LINE_SMOOTH);
            // 點去鋸齒
            // gl.Enable(OpenGL.GL_POINT_SMOOTH);
            // 多邊形去鋸齒
            // gl.Enable(OpenGL.GL_POLYGON_SMOOTH);
            //// 多邊形去鋸齒
            // gl.Enable(OpenGL.GL_SMOOTH);
            // 深度測試
            GL.Enable(OpenGL.GL_DEPTH_TEST);
            // 設定混和模式
            GL.Enable(OpenGL.GL_BLEND);
            GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            // 切除背面
            // GL.CullFace(OpenGL.GL_CCW);
            // 設定座標原點
            GL.Translate(Translate.X, Translate.Y, 0);
        }

        /// <summary>
        /// 將現實座標從 from 移動到 target
        /// </summary>
        private void MoveMap(IPair from, IPair target)
        {
            Translate.X += (target.X - from.X);
            Translate.Y += (target.Y - from.Y);
        }

        /// <summary>
        /// 設定滑鼠位置
        /// </summary>
        /// <param name="glPosition"></param>
        private void SetCursor(IPair glPosition)
        {
            PreGLPosition = glPosition;
            var screenPosition = GLToScreen(glPosition.X, glPosition.Y);
            Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new System.Drawing.Point(screenPosition.X, screenPosition.Y);
        }

        /// <summary>
        /// 點擊事件
        /// </summary>
        private void SharpGLCtrl_Click(object sender, EventArgs e)
        {
            // 結束移動
            if (MoveType != EMoveType.Stop)
            {
                MoveType = EMoveType.Stop;
            }

            // 右鍵選擇物件
            MouseEventArgs mouse = (MouseEventArgs)e;
            if (mouse.Button == MouseButtons.Right)
            {
                var selects = GLCMD.GetAllTargetID(ScreenToGL(mouse.X, mouse.Y));
                if (selects.Count() == 1) ShowEditMenu(selects.ElementAt(0));
                else if (selects.Count() > 1) ShowMultItemSelectMenu(selects);
                else ShowNoItemSelectMenu(); // selects.Count() == 0
            }
        }

        /// <summary>
        /// 鍵盤事件
        /// </summary>
        private void SharpGLCtrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z && e.Control == true)
            {
                if (e.Shift) GLCMD.Redo(1);
                else GLCMD.Undo(1);
            }
        }

        private void SharpGLCtrl_MouseDown(object sender, MouseEventArgs e)
        {
            // 左鍵擦子功能、右鍵取消擦子
            MouseEventArgs mouse = (MouseEventArgs)e;
            if (GLCMD.EraserSize != 0)
            {
                if (mouse.Button == MouseButtons.Left) GLCMD.EraserObstaclePoints(ObstaclePointsID);
                if (mouse.Button == MouseButtons.Right) GLCMD.SetEraser(PreGLPosition, 0);
            }

            // 左鍵完成畫筆、右鍵取消畫筆
            if (mouse.Button == MouseButtons.Left)
            {
                GLCMD.PenFinish(ObstaclePointsID);
            }
            else if (mouse.Button == MouseButtons.Right)
            {
                GLCMD.PenCancel();
            }
        }

        private void SharpGLCtrl_MouseMove(object sender, MouseEventArgs e)
        {
            var newGLPosition = ScreenToGL(e.X, e.Y);
            ShowMousePosition(newGLPosition);

            int x = newGLPosition.X;
            int y = newGLPosition.Y;

            PreGLPosition = newGLPosition;

            switch (MoveType)
            {
                case EMoveType.Center:
                    GLCMD.DoMoveCenter(SelectTargetID, x, y);
                    break;

                case EMoveType.Max:
                    GLCMD.DoMoveMax(SelectTargetID, x, y);
                    break;

                case EMoveType.Min:
                    GLCMD.DoMoveMin(SelectTargetID, x, y);
                    break;

                case EMoveType.Begin:
                    GLCMD.DoMoveBegin(SelectTargetID, x, y);
                    break;

                case EMoveType.End:
                    GLCMD.DoMoveEnd(SelectTargetID, x, y);
                    break;

                case EMoveType.Toward:
                    GLCMD.DoMoveToward(SelectTargetID, x, y);
                    break;
            }

            GLCMD.SetEraser(PreGLPosition);
            GLCMD.SetPenEnd(PreGLPosition);
        }

        /// <summary>
        /// 滑鼠滾輪
        /// </summary>
        private void SharpGLCtrl_MouseWheel(object sender, MouseEventArgs e)
        {
            IPair orgMousePoint = ScreenToGL(e.X, e.Y);
            Focus(orgMousePoint);
            if (e.Delta > 0) Zoom *= 1.2;
            if (e.Delta < 0) Zoom /= 1.2;
            MoveMap(orgMousePoint, ScreenToGL(e.X, e.Y));
        }

        /// <summary>
        /// 重新繪圖
        /// </summary>
        private void SharpGLCtrl_OpenGLDraw(object sender, RenderEventArgs args)
        {
            GDIDraw();
        }
    }
}