using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TrafficControlTest.Library.EventHandlerLibraryOfIVehicleControl;

namespace TrafficControlTest.Interface
{
	public interface IVehicleControlManager
	{
		event EventHandlerIVehicleControl ControlAdded;
		event EventHandlerIVehicleControl ControlRemoved;
		event EventHandlerIVehicleControlStateUpdated ControlStateUpdated;

		/// <summary>取得指定資料</summary>
		IVehicleControl this[string Name] { get; }
		/// <summary>取得資料數量</summary>
		int Count { get; }

		/// <summary>檢查指定資料是否存在</summary>
		bool IsExist(string Name);
		/// <summary>檢查指定資料是否存在</summary>
		bool IsCauseExist(string CauseId);
		/// <summary>取得指定資料</summary>
		IVehicleControl Get(string Name);
		/// <summary>取得指定資料</summary>
		IVehicleControl GetViaCause(string CauseId);
		/// <summary>取得資料名稱清單</summary>
		List<string> GetNames();
		/// <summary>取得資料清單</summary>
		List<IVehicleControl> GetList();
		/// <summary>新增資料</summary>
		void Add(string Name, IVehicleControl Data);
		/// <summary>移除資料</summary>
		void Remove(string Name);
		/// <summary>更新指定資料</summary>
		void UpdateSendState(string Name, SendState SendState);
		/// <summary>更新指定資料</summary>
		void UpdateExecuteState(string Name, ExecuteState ExecuteState);
		/// <summary>更新指定資料</summary>
		void UpdateParameters(string Name, string[] Parameters);
	}
}
