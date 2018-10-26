using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControlTest
{
	class DebugMessage
	{
		public const string TIME_FORMAT_DETAIL = "yyyy/MM/dd HH:mm:ss.fff";
		public DateTime TimeStamp = DateTime.Now;
		public string Category = "";
		public string Message = "";

		public DebugMessage(DateTime timeStamp, string category, string message)
		{
			Set(timeStamp, category, message);
		}

		public DebugMessage(string category, string message)
		{
			Set(DateTime.Now, category, message);
		}

		public override string ToString()
		{
			return $"{TimeStamp.ToString(TIME_FORMAT_DETAIL)} - [{Category}] - {Message}";
		}

		private void Set(DateTime timeStamp, string category, string message)
		{
			this.TimeStamp = timeStamp;
			this.Category = category;
			this.Message = message;
		}
	}
}
