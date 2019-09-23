using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TrafficControlTest.UserControl.UCVehicleInfo;

namespace TrafficControlTest.UserControl
{
	public partial class UCVehicleOverview : System.Windows.Forms.UserControl
	{
		private object mLock = new object();
		private List<UCVehicleInfo> mControls = new List<UCVehicleInfo>();

		public UCVehicleOverview()
		{
			InitializeComponent();
		}
		public bool Contain(string Id)
		{
			bool result = false;
			lock (mLock)
			{
				result = Controls.ContainsKey(UCVehicleInfo.PreFix + Id);
			}
			return result;
		}
		public void Add(string Id, string Battery, string State)
		{
			UCVehicleInfo ucVehicleInfo = new UCVehicleInfo() { mId = Id, mBattery = Battery, mState = State, mBorderColor = Color.LightSalmon, Dock = DockStyle.Top, Height = UCVehicleInfo.DefaultHeight };
			string tmpName = ucVehicleInfo.Name;
			lock (mLock)
			{
				if (mControls.FirstOrDefault((o) => o.mId == Id) == null && !Controls.ContainsKey(tmpName))
				{
					Visible = false;
					mControls.Add(ucVehicleInfo);
					mControls = mControls.OrderBy((o) => o.mId).ToList();

					Controls.Add(ucVehicleInfo);
					// Child Index 越大， Dock 位置越靠邊
					Controls.SetChildIndex(lblTitle, mControls.Count);
					for (int i = 0; i < mControls.Count; ++i)
					{
						Controls.SetChildIndex(mControls[i], mControls.Count - 1 - i);
					}
					Visible = true;
				}
			}
		}
		public UCVehicleInfo Get(string Id)
		{
			UCVehicleInfo result = null;
			string tmpName = UCVehicleInfo.PreFix + Id;
			lock (mLock)
			{
				result = Controls.ContainsKey(tmpName) ? (Controls[tmpName] as UCVehicleInfo) : null;
			}
			return result;
		}
		public void Set(string Id, Property Property, string Value)
		{
			string tmpName = UCVehicleInfo.PreFix + Id;
			lock (mLock)
			{
				if (Controls.ContainsKey(tmpName))
				{
					switch (Property)
					{
						case Property.Id:
							(Controls[tmpName] as UCVehicleInfo).mId = Value;
							break;
						case Property.Battery:
							(Controls[tmpName] as UCVehicleInfo).mBattery = Value;
							break;
						case Property.State:
							(Controls[tmpName] as UCVehicleInfo).mState = Value;
							break;
						default:
							break;
					}
				}
			}
		}
		public void Remove(string Id)
		{
			string tmpName = UCVehicleInfo.PreFix + Id;
			lock (mLock)
			{
				if (mControls.FirstOrDefault((o) => o.mId == Id) != null && Controls.ContainsKey(tmpName))
				{
					mControls.Remove(mControls.FirstOrDefault((o) => o.mId == Id));
					Controls.RemoveByKey(tmpName);
				}
			}
		}
	}
}
