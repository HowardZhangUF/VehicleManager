using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrafficControlTest.Module.Map;
using TrafficControlTest.Library;

namespace TrafficControlTest.UserControl
{
	public partial class UcMapInfo : System.Windows.Forms.UserControl
	{
		private IMapManager rMapManager = null;

		public UcMapInfo()
		{
			InitializeComponent();
		}
		public void Set(IMapManager MapManager)
		{
			UnsubscribeEvent_IMapManager(rMapManager);
			rMapManager = MapManager;
			SubscribeEvent_IMapManager(rMapManager);
		}
		public new void BringToFront()
		{
			UpdateGui_UpdateMapOperationButtonEnable(false);
			base.BringToFront();
		}

		private void SubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.LoadMapSuccessed += HandleEvent_MapManagerLoadMapSuccessed;
			}
		}
		private void UnsubscribeEvent_IMapManager(IMapManager MapManager)
		{
			if (MapManager != null)
			{
				MapManager.LoadMapSuccessed -= HandleEvent_MapManagerLoadMapSuccessed;
			}
		}
		private void HandleEvent_MapManagerLoadMapSuccessed(object sender, LoadMapSuccessedEventArgs e)
		{
			lblMapFileName.InvokeIfNecessary(() =>
			{
				lblMapFileName.Text = System.IO.Path.GetFileName(rMapManager.mCurrentMapFileName);
				lblMapFileHash.Text = rMapManager.mCurrentMapFileHash;
				lblMapUpdateTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
				lbGoals.Items.Clear();
				lbGoals.Items.AddRange(rMapManager.mTowardPointMapObjects.Select(o => o.mName).OrderBy(o => o).ToArray());
				lblGoalName.Text = string.Empty;
				lblGoalLocation.Text = string.Empty;
				lblGoalType.Text = string.Empty;
				lblGoalParameters.Text = string.Empty;
				lbRegions.Items.Clear();
				lbRegions.Items.AddRange(rMapManager.mRectangleMapObjects.Select(o => o.mName).OrderBy(o => o).ToArray());
				lblRegionName.Text = string.Empty;
				lblRegionRange.Text = string.Empty;
				lblRegionType.Text = string.Empty;
				lblRegionParameters.Text = string.Empty;
			});
		}
		private void UpdateGui_ClearMapInfo()
		{
			lblMapFileName.InvokeIfNecessary(() =>
			{
				lblMapFileName.Text = string.Empty;
				lblMapFileHash.Text = string.Empty;
				lblMapUpdateTime.Text = string.Empty;
				lbGoals.Items.Clear();
				lblGoalName.Text = string.Empty;
				lblGoalLocation.Text = string.Empty;
				lblGoalType.Text = string.Empty;
				lblGoalParameters.Text = string.Empty;
				lbRegions.Items.Clear();
				lblRegionName.Text = string.Empty;
				lblRegionRange.Text = string.Empty;
				lblRegionType.Text = string.Empty;
				lblRegionParameters.Text = string.Empty;
			});
		}
		private void UpdateGui_UpdateMapOperationButtonEnable(bool Enable)
		{
			btnLock.InvokeIfNecessary(() =>
			{
				if (Enable)
				{
					btnLock.Text = "Unlocked";
					btnLock.BackColor = Color.FromArgb(52, 170, 70);
					btnLock.ForeColor = Color.Black;
					btnLoadMap.Enabled = true;
					btnSynchronizeMap.Enabled = true;
				}
				else
				{
					btnLock.Text = "Locked";
					btnLock.BackColor = BackColor;
					btnLock.ForeColor = ForeColor;
					btnLoadMap.Enabled = false;
					btnSynchronizeMap.Enabled = false;
				}
			});
		}
		private void UcMapInfo_Load(object sender, EventArgs e)
		{
			UpdateGui_ClearMapInfo();
			UpdateGui_UpdateMapOperationButtonEnable(false);
		}
		private void btnLock_Click(object sender, EventArgs e)
		{
			if (btnLock.Text == "Locked")
			{
				UpdateGui_UpdateMapOperationButtonEnable(true);
			}
			else
			{
				UpdateGui_UpdateMapOperationButtonEnable(false);
			}
		}
		private void btnLoadMap_Click(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog())
			{
				ofd.Title = "Choose iTS Map File";
				ofd.Filter = "map files (*.map)|*.map";
				ofd.Multiselect = false;
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					rMapManager.LoadMap(ofd.FileName);
				}
			}

			UpdateGui_UpdateMapOperationButtonEnable(false);
		}
		private void btnSynchronizeMap_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(rMapManager.mCurrentMapFileName))
			{
				rMapManager.SynchronizeMapToOnlineVehicles(rMapManager.mCurrentMapFileName);
			}
			UpdateGui_UpdateMapOperationButtonEnable(false);
		}
		private void lbGoals_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lbGoals.SelectedIndex > -1)
			{
				var mapObject = rMapManager.mTowardPointMapObjects.OrderBy(o => o.mName).ToList()[lbGoals.SelectedIndex];
				lblGoalName.Text = mapObject.mName;
				lblGoalLocation.Text = $"({mapObject.mLocation.mX},{mapObject.mLocation.mY})";
				lblGoalType.Text = mapObject.mType.ToString();
				lblGoalParameters.Text = (mapObject.mParameters == null || mapObject.mParameters.Length == 0 ? string.Empty : string.Join(",", mapObject.mParameters));
			}
		}
		private void lbRegions_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lbRegions.SelectedIndex > -1)
			{
				var mapObject = rMapManager.mRectangleMapObjects.OrderBy(o => o.mName).ToList()[lbRegions.SelectedIndex];
				lblRegionName.Text = mapObject.mName;
				lblRegionRange.Text = $"({mapObject.mRange.mMinX},{mapObject.mRange.mMinY})({mapObject.mRange.mMaxX},{mapObject.mRange.mMaxY})";
				lblRegionType.Text = mapObject.mType.ToString();
				lblRegionParameters.Text = (mapObject.mParameters == null || mapObject.mParameters.Length == 0 ? string.Empty : string.Join(",", mapObject.mParameters));
			}
		}
	}
}
