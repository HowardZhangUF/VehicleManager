﻿using Geometry;
using GLCore;
using GLStyle;
using SerialData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficControlTest
{
	public partial class TrafficControlTestGUI : Form
	{
		private TrafficControlTestProcess process = new TrafficControlTestProcess();

		private Dictionary<string, int> CollisionRegionIcons = new Dictionary<string, int>();

		private Dictionary<string, int> PathPointsIcons = new Dictionary<string, int>();

		public TrafficControlTestGUI()
		{
			InitializeComponent();
			StyleManager.LoadStyle("Style.ini");

			//TrafficController.CalculateCollisionEvent(TrafficController.CreateFakeData());
		}

		private void SubscribeProcessEvent()
		{
			process.DebugMessage += Process_DebugMessage;
			process.AGVAdded += Process_AGVAdded;
			process.AGVRemoved += Process_AGVRemoved;
			process.AGVStatusUpdated += Process_AGVStatusUpdated;
			process.AGVPathUpdated += Process_AGVPathUpdated;
			process.CollisionOccured += Process_CollisionOccured;
			process.CollisionUpdated += Process_CollisionUpdated;
			process.CollisionSolved += Process_CollisionSolved;
		}

		private void UnsubscribeProcessEvent()
		{
			process.DebugMessage -= Process_DebugMessage;
			process.AGVAdded -= Process_AGVAdded;
			process.AGVRemoved -= Process_AGVRemoved;
			process.AGVStatusUpdated -= Process_AGVStatusUpdated;
			process.AGVPathUpdated -= Process_AGVPathUpdated;
			process.CollisionOccured -= Process_CollisionOccured;
			process.CollisionUpdated -= Process_CollisionUpdated;
			process.CollisionSolved -= Process_CollisionSolved;
		}

		/// <summary>註冊圖像 ID</summary>
		private void RegisterIconID(AGVInfo agvInfo)
		{
			agvInfo.AGVIconID = GLCMD.CMD.SerialNumber.Next();
			agvInfo.PathIconID = GLCMD.CMD.AddMultiStripLine("Path", null);
			if (!PathPointsIcons.Keys.Contains(agvInfo.Status.Name))
			{
				int tmp = GLCMD.CMD.AddMultiPair("PathPoint", null);
				PathPointsIcons.Add(agvInfo.Status.Name, tmp);
			}
		}

		/// <summary>印出圖像</summary>
		private void PrintIcon(AGVInfo agvInfo)
		{
			if (agvInfo.Status != null)
			{
				if (agvInfo.AGVIconID != -1)
				{
					GLCMD.CMD.AddAGV(agvInfo.AGVIconID, agvInfo.Status.Name, agvInfo.Status.X, agvInfo.Status.Y, agvInfo.Status.Toward);
				}
			}
			if (agvInfo.Path != null)
			{
				if (agvInfo.PathIconID != -1)
				{
					GLCMD.CMD.SaftyEditMultiGeometry<IPair>(agvInfo.PathIconID, true, (line) =>
					{
						line.Clear();
						line.AddRangeIfNotNull(ConvertAGVPathToPairCollection(agvInfo.Path));
					});
				}
				if (PathPointsIcons.Keys.Contains(agvInfo.Status.Name))
				{
					GLCMD.CMD.SaftyEditMultiGeometry<IPair>(PathPointsIcons[agvInfo.Status.Name], true, (points) =>
					{
						points.Clear();
						points.AddRangeIfNotNull(agvInfo.PathPoints);
					});
				}
			}
		}

		/// <summary>擦掉圖像</summary>
		private void EraseIcon(AGVInfo agvInfo)
		{
			if (agvInfo.AGVIconID != -1) GLCMD.CMD.DeleteAGV(agvInfo.AGVIconID);
			if (agvInfo.PathIconID != -1) GLCMD.CMD.DeleteMulti(agvInfo.PathIconID);
			if (PathPointsIcons.Keys.Contains(agvInfo.Status.Name))
			{
				GLCMD.CMD.DeleteMulti(PathPointsIcons[agvInfo.Status.Name]);
				PathPointsIcons.Remove(agvInfo.Status.Name);
			}
		}

		private void Process_DebugMessage(DateTime timeStamp, string category, string message)
		{
			rtxtDebugMessage.InvokeIfNecessary(() =>
			{
				if (chkRtxtDebugMsgAutoScroll.Checked)
					rtxtDebugMessage.AppendText(new DebugMessage(timeStamp, category, message).ToString() + "\n");
				else
					rtxtDebugMessage.Text += new DebugMessage(timeStamp, category, message).ToString() + "\n";
			});
		}

		private void Process_AGVAdded(DateTime occurTime, string agvName, string ipPort, AGVInfo agvInfo)
		{
			if (agvInfo != null)
			{
				RegisterIconID(agvInfo);
				PrintIcon(agvInfo);
			}
		}

		private void Process_AGVRemoved(DateTime occurTime, string agvName, string ipPort, AGVInfo agvInfo)
		{
			if (agvInfo != null)
			{
				EraseIcon(agvInfo);
			}
		}

		private void Process_AGVStatusUpdated(DateTime occurTime, string agvName, string ipPort, AGVInfo agvInfo)
		{
			if (agvInfo != null)
			{
				PrintIcon(agvInfo);
			}
		}

		private void Process_AGVPathUpdated(DateTime occurTime, string agvName, string ipPort, AGVInfo agvInfo)
		{
			if (agvInfo != null)
			{
				PrintIcon(agvInfo);
			}
		}

		private void Process_CollisionOccured(CollisionPair collisionPair)
		{
			if (collisionPair != null)
			{
				string keyword = collisionPair.AGV1.Status.Name + collisionPair.AGV2.Status.Name;
				if (!CollisionRegionIcons.Keys.Contains(keyword))
				{
					List<Area> tmp = new List<Area>();
					tmp.Add(new Area(collisionPair.CollisionRegion.XMin, collisionPair.CollisionRegion.YMin, collisionPair.CollisionRegion.XMax, collisionPair.CollisionRegion.YMax));
					int id = GLCMD.CMD.AddMultiArea("CollisionArea", tmp);
					CollisionRegionIcons.Add(keyword, id);
				}
			}
		}

		private void Process_CollisionUpdated(CollisionPair collisionPair)
		{
			if (collisionPair != null)
			{
				string keyword = collisionPair.AGV1.Status.Name + collisionPair.AGV2.Status.Name;
				if (CollisionRegionIcons.Keys.Contains(keyword))
				{
					List<Area> tmp = new List<Area>();
					tmp.Add(new Area(collisionPair.CollisionRegion.XMin, collisionPair.CollisionRegion.YMin, collisionPair.CollisionRegion.XMax, collisionPair.CollisionRegion.YMax));

					GLCMD.CMD.SaftyEditMultiGeometry<IArea>(CollisionRegionIcons[keyword], true, (area) =>
					{
						area.Clear();
						area.AddRangeIfNotNull(tmp);
					});
				}
			}
		}

		private void Process_CollisionSolved(CollisionPair collisionPair)
		{
			if (collisionPair != null)
			{
				string keyword = collisionPair.AGV1.Status.Name + collisionPair.AGV2.Status.Name;
				if (CollisionRegionIcons.Keys.Contains(keyword))
				{
					GLCMD.CMD.DeleteMulti(CollisionRegionIcons[keyword]);
					CollisionRegionIcons.Remove(keyword);
				}
			}
		}

		private void TrafficControlTestGUI_Load(object sender, EventArgs e)
		{
			SubscribeProcessEvent();
			process.StartAGVMonitor(8000);
		}

		private void TrafficControlTestGUI_FormClosing(object sender, FormClosingEventArgs e)
		{
			process.StopAGVMonitor();
			UnsubscribeProcessEvent();
		}

		/// <summary>將 AGVPath 轉換成 Pair 集合</summary>
		private IEnumerable<IPair> ConvertAGVPathToPairCollection(AGVPath path)
		{
			for (int ii = 0; ii < path.PathX.Count; ii++)
			{
				yield return new Pair(path.PathX[ii], path.PathY[ii]);
			}
		}

		private void gluiCtrl1_LoadMapEvent(object sender, GLUI.LoadMapEventArgs e)
		{
			// 載入地圖時， GLCMD 裡面記錄的 ID 會被清空，所以需要重新註冊圖像 ID
			// 在註冊之前，需先將之前殘留的圖像擦掉，再進行註冊
			foreach (string agvName in process.GetAGVNames())
			{
				AGVInfo tmp = process.GetAGVInfoByName(agvName);
				EraseIcon(tmp);
				RegisterIconID(tmp);
			}
		}

		private void chkAGVMonitorMsg_CheckedChanged(object sender, EventArgs e)
		{
			if (chkAGVMonitorMsg.Checked)
				process.DisplayAGVMonitorDebugMessage = true;
			else
				process.DisplayAGVMonitorDebugMessage = false;
			rtxtDebugMessage.Focus();
		}

		private void chkAGVInfoManagerMsg_CheckedChanged(object sender, EventArgs e)
		{
			if (chkAGVInfoManagerMsg.Checked)
				process.DisplayAGVInfoManagerDebugMessage = true;
			else
				process.DisplayAGVInfoManagerDebugMessage = false;
			rtxtDebugMessage.Focus();
		}

		private void cmenuItemClearRichTextbox_Click(object sender, EventArgs e)
		{
			rtxtDebugMessage.Clear();
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			// 切換分頁到 DebugMessage 時，自動 Focus 到 RichTextbox 上，好讓其能自動捲動
			if (tabControl1.SelectedTab == tpDebugMsg)
				rtxtDebugMessage.Focus();
		}
	}
}