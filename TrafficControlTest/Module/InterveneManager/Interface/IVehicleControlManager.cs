using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.General.Implement;

namespace TrafficControlTest.Interface
{
	public interface IVehicleControlManager : IItemManager<IVehicleControl>
	{
		/// <summary>取得指定資料</summary>
		IVehicleControl this[string Name] { get; }

		/// <summary>檢查指定資料是否存在</summary>
		bool IsExistByCauseId(string CauseId);
		/// <summary>取得指定資料</summary>
		IVehicleControl GetItemByCauseId(string CauseId);
		/// <summary>更新指定資料</summary>
		void UpdateSendState(string Name, SendState SendState);
		/// <summary>更新指定資料</summary>
		void UpdateParameters(string Name, string[] Parameters);
	}
}
