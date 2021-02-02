using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Library;
using TrafficControlTest.Module.General;

namespace TrafficControlTest.Module.Log
{
	/// <summary>
	/// 主要用於介面顯示
	/// 使用 AddSignificantMessage() 方法收集類別的事件(重要的事件，供使用者監看)
	/// 定期發送 SignificantMessage 事件
	/// </summary>
	public interface ISignificantMessageHandler : ISystemWithLoopTask
	{
		event EventHandler<SignificantMessageEventArgs> SignificantMessage;

		void AddSignificantMessage(string OccurTime, string Category, string Message);
	}

	public class SignificantMessageEventArgs : EventArgs
	{
		public string OccurTime { get; private set; }
		public string Category { get; private set; }
		public string Message { get; private set; }

		public SignificantMessageEventArgs(string OccurTime, string Category, string Message) : base()
		{
			this.OccurTime = OccurTime;
			this.Category = Category;
			this.Message = Message;
		}
	}
}
