using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.Log
{
	/// <summary>
	/// 主要用於介面顯示
	/// 使用 AddDebugMessage() 方法收集類別的事件(所有的事件，供工程師監看)
	/// 定期發送 DebugMessage 事件
	/// </summary>
	public interface IDebugMessageHandler : ISystemWithLoopTask
	{
		event EventHandler<DebugMessageEventArgs> DebugMessage;

		void AddDebugMessage(string OccurTime, string Category, string SubCategory, string Message);
	}

	public class DebugMessageEventArgs : EventArgs
	{
		public string OccurTime { get; private set; }
		public string Category { get; private set; }
		public string SubCategory { get; private set; }
		public string Message { get; private set; }

		public DebugMessageEventArgs(string OccurTime, string Category, string SubCategory, string Message) : base()
		{
			this.OccurTime = OccurTime;
			this.Category = Category;
			this.SubCategory = SubCategory;
			this.Message = Message;
		}
	}
}
