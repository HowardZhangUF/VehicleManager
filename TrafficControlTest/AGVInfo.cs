using SerialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest
{
	/// <summary>AGV 資訊</summary>
	class AGVInfo
	{
		/// <summary>AGV 狀態</summary>
		public AGVStatus Status = null;

		/// <summary>AGV 路徑</summary>
		public AGVPath Path = null;

		/// <summary>AGV 圖像識別碼</summary>
		public int AGVIconID = -1;

		/// <summary>AGV 路徑圖像識別碼</summary>
		public int PathIconID = -1;

		/// <summary>AGV 的 IP 與 Port 。格式為 IP:Port</summary>
		public string IPPort = string.Empty;
	}
}
