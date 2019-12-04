using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TrafficControlTest.UserControl.UcVehicleInfo;

namespace TrafficControlTest.UserControl
{
	public partial class UCVehicleInfoList : System.Windows.Forms.UserControl
	{
		private object mLock = new object();

		public UCVehicleInfoList()
		{
			InitializeComponent();
		}
		public bool Contain(string Id)
		{
			bool result = false;
			lock (mLock)
			{
				result = Controls.ContainsKey(UcVehicleInfo.PreFix + Id);
			}
			return result;
		}
		public void Add(string Id, string Battery, string State)
		{
			UcVehicleInfo ucVehicleInfo = new UcVehicleInfo() { mId = Id, mBattery = Battery, mState = State, mBorderColor = Color.LightSalmon };
			string tmpName = ucVehicleInfo.Name;
			lock (mLock)
			{
				if (!Controls.ContainsKey(tmpName))
				{
					Controls.Add(ucVehicleInfo);
					Controls[tmpName].Dock = DockStyle.Top;
					Controls[tmpName].Height = UcVehicleInfo.DefaultHeight;
				}
			}
		}
		public UcVehicleInfo Get(string Id)
		{
			UcVehicleInfo result = null;
			string tmpName = UcVehicleInfo.PreFix + Id;
			lock (mLock)
			{
				result = Controls.ContainsKey(tmpName) ? (Controls[tmpName] as UcVehicleInfo) : null;
			}
			return result;
		}
		public void Set(string Id, Property Property, string Value)
		{
			string tmpName = UcVehicleInfo.PreFix + Id;
			lock (mLock)
			{
				if (Controls.ContainsKey(tmpName))
				{
					switch (Property)
					{
						case Property.Id:
							(Controls[tmpName] as UcVehicleInfo).mId = Value;
							break;
						case Property.Battery:
							(Controls[tmpName] as UcVehicleInfo).mBattery = Value;
							break;
						case Property.State:
							(Controls[tmpName] as UcVehicleInfo).mState = Value;
							break;
						default:
							break;
					}
				}
			}
		}
		public void Remove(string Id)
		{
			string tmpName = UcVehicleInfo.PreFix + Id;
			lock (mLock)
			{
				if (Controls.ContainsKey(tmpName))
				{
					Controls.RemoveByKey(tmpName);
				}
			}
		}
	}
}
