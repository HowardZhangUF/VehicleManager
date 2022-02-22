using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LibraryForVM;

namespace VehicleSimulator.New
{
    public partial class UcContentOfSetting : UserControl
	{
		private SimulatorProcessContainer rCore = null;

		public UcContentOfSetting()
        {
            InitializeComponent();
            UpdateGui_InitializeDgvMapFileList();
		}
		public void Set(SimulatorProcessContainer SimulatorProcessContainer)
		{
			if (SimulatorProcessContainer != null)
			{
				rCore = SimulatorProcessContainer;
			}
		}

		private void UpdateGui_InitializeDgvMapFileList()
        {
            dgvMapFileList.InvokeIfNecessary(() =>
            {
                DataGridView dgv = dgvMapFileList;

                dgv.RowHeadersVisible = false;
                dgv.ColumnHeadersVisible = false;
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToResizeRows = false;
                dgv.AllowUserToResizeColumns = false;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.MultiSelect = false;
                dgv.BackgroundColor = Color.FromArgb(28, 28, 28);
                dgv.GridColor = Color.FromArgb(28, 28, 28);
                dgv.BorderStyle = BorderStyle.None;

                dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dgv.DefaultCellStyle.BackColor = Color.FromArgb(28, 28, 28);
                dgv.DefaultCellStyle.ForeColor = Color.FromArgb(255, 255, 255);
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 255);
                dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(0, 0, 0);

                dgv.Columns.Add("MapFileName", "MapFileName");
                dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    column.ReadOnly = true;
                }
            });
        }
		private void UpdateGui_DgvMapFileList_UpdateMapFileList(string MapFileFolderDirectory)
		{
			dgvMapFileList.InvokeIfNecessary(() =>
			{
				dgvMapFileList.ClearSelection();
				dgvMapFileList.Rows.Clear();

				if (Directory.Exists(MapFileFolderDirectory))
				{
					string[] mapFileList = Directory.GetFiles(MapFileFolderDirectory).Where(o => o.EndsWith(".map")).Select(o => o.Replace(MapFileFolderDirectory, string.Empty).TrimStart('\\')).ToArray();
					for (int i = 0; i < mapFileList.Length; ++i)
					{
						dgvMapFileList.Rows.Add(mapFileList[i]);
					}
				}
			});
		}
		private void UpdateGui_PbMapPreview_UpdatePicture(string MapFilePath)
		{
			pbMapPreview.Image = GenerateLoadingImage(pbMapPreview.Width, pbMapPreview.Height);
			Application.DoEvents();

			Task.Run(() =>
			{
				if (GetMapDataFromMapFile(MapFilePath, out MapData mapData))
				{
					pbMapPreview.Image = GenerateMapImage(pbMapPreview.Width, pbMapPreview.Height, mapData);
				}
			});
		}
		private void lblMapFileFolderDirectory_TextChanged(object sender, EventArgs e)
		{
			if (Directory.Exists(lblMapFileFolderDirectory.Text))
			{
				UpdateGui_DgvMapFileList_UpdateMapFileList(lblMapFileFolderDirectory.Text);
			}
			else
			{
				UpdateGui_DgvMapFileList_UpdateMapFileList(string.Empty);
			}
		}
		private void btnSelectMapFileFolderDirectory_Click(object sender, EventArgs e)
		{
			using (var fbd = new FolderBrowserDialog())
			{
				if (!string.IsNullOrEmpty(lblMapFileFolderDirectory.Text) && Directory.Exists(lblMapFileFolderDirectory.Text))
				{
					fbd.SelectedPath = lblMapFileFolderDirectory.Text;
				}
				DialogResult result = fbd.ShowDialog();

				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
				{
					lblMapFileFolderDirectory.Text = fbd.SelectedPath;
				}
			}
		}
		private void dgvMapFileList_SelectionChanged(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(lblMapFileFolderDirectory.Text) && dgvMapFileList.CurrentRow.Index >= 0)
			{
				string mapFilePath = $"{lblMapFileFolderDirectory.Text}\\{dgvMapFileList.CurrentRow.Cells[0].Value.ToString()}";
				UpdateGui_PbMapPreview_UpdatePicture(mapFilePath);
				rCore.SetMap(mapFilePath);
			}
			else
			{
				UpdateGui_PbMapPreview_UpdatePicture(string.Empty);
			}
		}

		private static bool GetMapDataFromMapFile(string MapFilePath, out MapData MapData)
		{
			MapData = new MapData();

			if (File.Exists(MapFilePath))
			{
				string[] mapFileLines = File.ReadAllLines(MapFilePath);
				string[] separator = new string[] { "," };

				// Analyze MinPoint
				//		=> Minimum Position:-25902,-35354
				string minPointString = mapFileLines.First(o => o.StartsWith("Minimum Position:"));
				string[] minPointSplitStrings = minPointString.Replace("Minimum Position:", string.Empty).Split(separator, StringSplitOptions.RemoveEmptyEntries);

				// Analyze MaxPoint
				//		=> Maximum Position:35671,38350
				string maxPointString = mapFileLines.First(o => o.StartsWith("Maximum Position:"));
				string[] maxPointSplitStrings = maxPointString.Replace("Maximum Position:", string.Empty).Split(separator, StringSplitOptions.RemoveEmptyEntries);

				Rectangle tmpRange = new Rectangle();
				if (minPointSplitStrings.Length == 2 && maxPointSplitStrings.Length == 2)
				{
					tmpRange.Set(int.Parse(minPointSplitStrings[0]), int.Parse(minPointSplitStrings[1]), int.Parse(maxPointSplitStrings[0]), int.Parse(maxPointSplitStrings[1]));
				}
				MapData.SetMapFilePath(MapFilePath);
				MapData.SetRange(tmpRange);

				// Analyze ForbiddenRectangle
				//		=> Forbidden Area \r\n ,27498,-34500,32799,-13110
				string[] forbiddenRectangleStrings = mapFileLines.SkipWhile(o => o != "Forbidden Area").Skip(1).TakeWhile(o => o != "Obstacle Points").ToArray();
				List<ForbiddenRectangle> tmpForbiddenRectangles = new List<ForbiddenRectangle>();
				for (int i = 0; i < forbiddenRectangleStrings.Length; ++i)
				{
					if (!string.IsNullOrEmpty(forbiddenRectangleStrings[i]))
					{
						string[] forbiddenRectangleSplitStrings = forbiddenRectangleStrings[i].Split(separator, StringSplitOptions.RemoveEmptyEntries);
						if (forbiddenRectangleSplitStrings.Length >= 4)
						{
							ForbiddenRectangle tmpRectangle = new ForbiddenRectangle(int.Parse(forbiddenRectangleSplitStrings[0]), int.Parse(forbiddenRectangleSplitStrings[1]), int.Parse(forbiddenRectangleSplitStrings[2]), int.Parse(forbiddenRectangleSplitStrings[3]));
							tmpForbiddenRectangles.Add(tmpRectangle);
						}
					}
				}
				MapData.SetForbiddenRectangles(tmpForbiddenRectangles);

				// Analyze OneWayRectangle
				//		=> none
				// do nothing ...
				List<OneWayRectangle> tmpOneWayRectangles = new List<OneWayRectangle>();
				MapData.SetOneWayRectangles(tmpOneWayRectangles);

				// Analyze ObstaclePoint
				//		=> Obstacle Points \r\n 23683,-24244
				string[] obstaclePointStrings = mapFileLines.SkipWhile(o => o != "Obstacle Points").Skip(1).TakeWhile(o => !o.Contains("Position")).ToArray();
				List<ObstaclePoint> tmpObstaclePoints = new List<ObstaclePoint>();
				for (int i = 0; i < obstaclePointStrings.Length; ++i)
				{
					if (!string.IsNullOrEmpty(obstaclePointStrings[i]))
					{
						string[] obstaclePointSplitStrings = obstaclePointStrings[i].Split(separator, StringSplitOptions.RemoveEmptyEntries);
						if (obstaclePointSplitStrings.Length >= 2)
						{
							ObstaclePoint tmpPoint = new ObstaclePoint(int.Parse(obstaclePointSplitStrings[0]), int.Parse(obstaclePointSplitStrings[1]));
							tmpObstaclePoints.Add(tmpPoint);
						}
					}
				}
				MapData.SetObstaclePoints(tmpObstaclePoints);

				return true;
			}
			else
			{
				return false;
			}
		}
		private static Image GenerateMapImage(int CanvasWidth, int CanvasHeight, MapData MapData)
		{
			Color colorOfBackground = Color.FromArgb(15, 15, 15);
			Color colorOfObstaclePoint = Color.FromArgb(240, 240, 240);
			Color colorOfForbiddenRectangle = Color.FromArgb(150, 128, 83, 0);
			Color colorOfOneWayRectangle = Color.FromArgb(150, 0, 128, 128);
			Color colorOfOneWayRectangleDirection = Color.FromArgb(150, 0, 255, 255);
			int lengthOfOneWayArrow = 2000;

			float scaleOfXOfRealToCanvas = (float)CanvasWidth / MapData.mRange.mWidth;
			float scaleOfYOfRealToCanvas = (float)CanvasHeight / MapData.mRange.mHeight;
			float scaleOfRealToCanvas = Math.Min(scaleOfXOfRealToCanvas, scaleOfYOfRealToCanvas);
			int offsetOfXOfRealToCanvas = (int)(0 - MapData.mRange.mMin.mX + ((CanvasWidth / scaleOfRealToCanvas - MapData.mRange.mWidth) / 2));
			int offsetOfYOfRealToCanvas = (int)(0 - MapData.mRange.mMax.mY - ((CanvasHeight / scaleOfRealToCanvas - MapData.mRange.mHeight) / 2));

			Bitmap result = new Bitmap(CanvasWidth, CanvasHeight);
			Graphics canvasGraphics = Graphics.FromImage(result);
			canvasGraphics.Clear(colorOfBackground);

			// Draw Obstacle Point
			using (Brush brush = new SolidBrush(colorOfObstaclePoint))
			{
				for (int i = 0; i < MapData.mObstaclePoints.Count; ++i)
				{
					float xOfCanvas = (MapData.mObstaclePoints[i].mX + offsetOfXOfRealToCanvas) * scaleOfRealToCanvas;
					float yOfCanvas = (MapData.mObstaclePoints[i].mY + offsetOfYOfRealToCanvas) * scaleOfRealToCanvas * (-1);
					canvasGraphics.FillRectangle(brush, xOfCanvas, yOfCanvas, 1, 1);
				}
			}

			// Draw Fobbidden Rectangle
			using (Brush brush = new SolidBrush(colorOfForbiddenRectangle))
			{
				for (int i = 0; i < MapData.mForbiddenRectangles.Count; ++i)
				{
					float xOfCanvas = (MapData.mForbiddenRectangles[i].mLeftTop.mX + offsetOfXOfRealToCanvas) * scaleOfRealToCanvas;
					float yOfCanvas = (MapData.mForbiddenRectangles[i].mLeftTop.mY + offsetOfYOfRealToCanvas) * scaleOfRealToCanvas * (-1);
					float widthOfCanvas = MapData.mForbiddenRectangles[i].mWidth * scaleOfRealToCanvas;
					float heightOfCanvas = MapData.mForbiddenRectangles[i].mHeight * scaleOfRealToCanvas;
					canvasGraphics.FillRectangle(brush, xOfCanvas, yOfCanvas, widthOfCanvas, heightOfCanvas);
				}
			}

			// Draw OneWay Rectangle
			using (Brush brush = new SolidBrush(colorOfOneWayRectangle))
			{
				for (int i = 0; i < MapData.mOneWayRectangles.Count; ++i)
				{
					float xOfCanvas = (MapData.mOneWayRectangles[i].mLeftTop.mX + offsetOfXOfRealToCanvas) * scaleOfRealToCanvas;
					float yOfCanvas = (MapData.mOneWayRectangles[i].mLeftTop.mY + offsetOfYOfRealToCanvas) * scaleOfRealToCanvas * (-1);
					float widthOfCanvas = MapData.mOneWayRectangles[i].mWidth * scaleOfRealToCanvas;
					float heightOfCanvas = MapData.mOneWayRectangles[i].mHeight * scaleOfRealToCanvas;
					canvasGraphics.FillRectangle(brush, xOfCanvas, yOfCanvas, widthOfCanvas, heightOfCanvas);
				}
			}

			// Draw OneWay Rectangle Direction
			using (Brush brush = new SolidBrush(colorOfOneWayRectangleDirection)) // FF00FF
			{
				using (Pen pen = new Pen(brush, 600 * scaleOfRealToCanvas) { EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor })
				{
					for (int i = 0; i < MapData.mOneWayRectangles.Count; ++i)
					{
						Point pointStartOfArrowOfReal = new Point();
						Point pointEndOfArrowOfReal = new Point();
						switch (MapData.mOneWayRectangles[i].mDirection)
						{
							case 0:
								pointStartOfArrowOfReal.Set(MapData.mOneWayRectangles[i].mCenter.mX - lengthOfOneWayArrow / 2, MapData.mOneWayRectangles[i].mCenter.mY);
								pointEndOfArrowOfReal.Set(MapData.mOneWayRectangles[i].mCenter.mX + lengthOfOneWayArrow / 2, MapData.mOneWayRectangles[i].mCenter.mY);
								break;
							case 90:
							case -270:
								pointStartOfArrowOfReal.Set(MapData.mOneWayRectangles[i].mCenter.mX, MapData.mOneWayRectangles[i].mCenter.mY - lengthOfOneWayArrow / 2);
								pointEndOfArrowOfReal.Set(MapData.mOneWayRectangles[i].mCenter.mX, MapData.mOneWayRectangles[i].mCenter.mY + lengthOfOneWayArrow / 2);
								break;
							case 180:
							case -180:
								pointStartOfArrowOfReal.Set(MapData.mOneWayRectangles[i].mCenter.mX + lengthOfOneWayArrow / 2, MapData.mOneWayRectangles[i].mCenter.mY);
								pointEndOfArrowOfReal.Set(MapData.mOneWayRectangles[i].mCenter.mX - lengthOfOneWayArrow / 2, MapData.mOneWayRectangles[i].mCenter.mY);
								break;
							case 270:
							case -90:
								pointStartOfArrowOfReal.Set(MapData.mOneWayRectangles[i].mCenter.mX, MapData.mOneWayRectangles[i].mCenter.mY + lengthOfOneWayArrow / 2);
								pointEndOfArrowOfReal.Set(MapData.mOneWayRectangles[i].mCenter.mX, MapData.mOneWayRectangles[i].mCenter.mY - lengthOfOneWayArrow / 2);
								break;
						}

						float xStartOfArrowOfCanvas = (pointStartOfArrowOfReal.mX + offsetOfXOfRealToCanvas) * scaleOfRealToCanvas;
						float yStartOfArrowOfCanvas = (pointStartOfArrowOfReal.mY + offsetOfYOfRealToCanvas) * scaleOfRealToCanvas * (-1);
						float xEndOfArrowOfCanvas = (pointEndOfArrowOfReal.mX + offsetOfXOfRealToCanvas) * scaleOfRealToCanvas;
						float yEndOfArrowOfCanvas = (pointEndOfArrowOfReal.mY + offsetOfYOfRealToCanvas) * scaleOfRealToCanvas * (-1);
						canvasGraphics.DrawLine(pen, xStartOfArrowOfCanvas, yStartOfArrowOfCanvas, xEndOfArrowOfCanvas, yEndOfArrowOfCanvas);
					}
				}
			}

			return result;
		}
		private static Image GenerateLoadingImage(int CanvasWidth, int CanvasHeight)
		{
			Color colorOfBackground = Color.FromArgb(15, 15, 15);
			Color colorOfPoint = Color.White;
			int pointSize = Math.Min(CanvasWidth / 30, CanvasHeight / 30);
			int intervalBetweeenPoints = CanvasWidth / 3 / 2;
			int startOfXOfPoints = CanvasWidth / 3;
			int startOfYOfPoints = CanvasHeight / 2;

			Bitmap result = new Bitmap(CanvasWidth, CanvasHeight);
			Graphics canvasGraphics = Graphics.FromImage(result);
			canvasGraphics.Clear(colorOfBackground);

			using (Brush brush = new SolidBrush(colorOfPoint))
			{
				canvasGraphics.FillRectangle(brush, startOfXOfPoints, startOfYOfPoints, pointSize, pointSize);
				canvasGraphics.FillRectangle(brush, startOfXOfPoints + intervalBetweeenPoints, startOfYOfPoints, pointSize, pointSize);
				canvasGraphics.FillRectangle(brush, startOfXOfPoints + intervalBetweeenPoints * 2, startOfYOfPoints, pointSize, pointSize);
			}

			return result;
		}
	}
}
