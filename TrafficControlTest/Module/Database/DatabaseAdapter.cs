using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace TrafficControlTest.Library
{
	public abstract class DatabaseAdapter
	{
		public delegate void EventHandlerDateTime(DateTime OccurTime);
		public delegate void SqlConnectionEventHandler(bool Connected);
		public delegate void SqlNonQueryCmdExecutedEventHandler(string SqlCmd, int Result);
		public delegate void SqlQueryCmdExecutedEventHandler(string SqlCmd, DataSet Result);

		public event EventHandlerDateTime SystemStarted;
		public event EventHandlerDateTime SystemStopped;
		public event SqlConnectionEventHandler ConnectStatusChanged;
		public event SqlNonQueryCmdExecutedEventHandler NonQueryCmdExecuted;
		public event SqlQueryCmdExecutedEventHandler QueryCmdExecuted;

		public bool mIsConnected
		{
			get
			{
				return _mIsConnected;
			}
			set
			{
				if (_mIsConnected != value)
				{
					_mIsConnected = value;
					ConnectStatusChanged?.Invoke(_mIsConnected);
				}
			}
		}
		public bool mIsExecuting
		{
			get
			{
				return _IsExecuting;
			}
			private set
			{
				_IsExecuting = value;
				if (_IsExecuting) SystemStarted?.Invoke(DateTime.Now);
				else SystemStopped?.Invoke(DateTime.Now);
			}
		}

		/// <summary>儲存檔案的資料夾名稱</summary>
		public static string mDirectoryNameOfFiles = ".\\Database";
		/// <summary>儲存未執行非查詢類 Sql 指令的檔案名稱</summary>
		public static string mFileNameOfNonQueryCmds = "RemainingNonQueryCommands.txt";
		/// <summary>儲存未執行查詢類 Sql 指令的檔案名稱</summary>
		public static string mFileNameOfQueryCmds = "RemainingQueryCommands.txt";
		/// <summary>儲存例外記錄的檔案名稱</summary>
		public static string mFileNameOfExceptionRecord = "Exception.txt";

		protected bool _mIsConnected = false;
		protected bool _IsExecuting = false;
		/// <summary>是否使用 Ping 來測試連線的旗標</summary>
		protected bool mPingBeforeBuildConnection = false;
		/// <summary>資料庫連線字串</summary>
		protected string mConnectionString = string.Empty;
		/// <summary>資料庫伺服器位址 (IP)</summary>
		protected string mDatabaseServerAddressIp = string.Empty;
		/// <summary>資料庫伺服器位址 (Port)</summary>
		protected string mDatabaseServerAddressPort = string.Empty;
		/// <summary>使用者名稱</summary>
		protected string mUserAccount = string.Empty;
		/// <summary>使用者密碼</summary>
		protected string mUserPassword = string.Empty;
		/// <summary>初始資料庫</summary>
		protected string mInitialDatabase = string.Empty;
		/// <summary>尚未執行 Sql 指令(非查詢類)的檔案名稱</summary>
		protected string mFilePathOfNonQueryCmds = $"{mDirectoryNameOfFiles}\\{mFileNameOfNonQueryCmds}";
		/// <summary>尚未執行 Sql 指令(查詢類)的檔案名稱</summary>
		protected string mFilePathOfQueryCmds = $"{mDirectoryNameOfFiles}\\{mFileNameOfQueryCmds}";
		/// <summary>儲存例外資訊的檔案名稱</summary>
		protected string mFilePathOfExceptionRecord = $"{mDirectoryNameOfFiles}\\{mFileNameOfExceptionRecord}";
		/// <summary>非查詢類 Sql 指令結果預設值</summary>
		protected int mDefaultValueOfNonQueryCmdResult = -1;
		/// <summary>查詢類 Sql 指令結果預設值</summary>
		protected DataSet mDefaultValueOfQueryCmdResult = null;
		/// <summary>執行緒同步鎖</summary>
		protected readonly object mLockOfSqlConnection = new object();
		/// <summary>執行緒同步鎖</summary>
		protected readonly object mLockOfContainerOfNonQueryCmds = new object();
		/// <summary>執行緒同步鎖</summary>
		protected readonly object mLockOfContainerOfQueryCmds = new object();
		/// <summary>非查詢類 Sql 指令容器</summary>
		protected List<string> mContainerOfNonQueryCmds = new List<string>();
		/// <summary>查詢類 Sql 指令容器</summary>
		protected List<string> mContainerOfQueryCmds = new List<string>();
		/// <summary>Sql 指令處理執行緒</summary>
		protected Thread mThdProcessCmds = null;
		/// <summary>Sql 指令處理執行緒的離開旗標</summary>
		protected bool[] mThdProcessCmdsExitFlag = null;
		/// <summary>Sql 指令處理週期 (ms)</summary>
		protected int mPeriodOfProcessCmd = 1000;
		/// <summary>連線狀態測試週期 (ms)</summary>
		protected int mPeriodOfTestConnectStatus = 5000;

		/// <summary>用來執行 Ping 的物件</summary>
		protected static Ping mPing = new Ping();

		public DatabaseAdapter(string DatabaseServerAddressIp, string DatabaseServerAddressPort, string UserAccount, string UserPassword, string InitialDatabase, bool PingBeforeBuildConnection)
		{
			try
			{
				if (!Directory.Exists(mDirectoryNameOfFiles)) Directory.CreateDirectory(mDirectoryNameOfFiles);
				SetDatabaseParameters(DatabaseServerAddressIp, DatabaseServerAddressPort, UserAccount, UserPassword, InitialDatabase);
				mPingBeforeBuildConnection = PingBeforeBuildConnection;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		~DatabaseAdapter()
		{
			try
			{
				Deconstruct();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>解構式</summary>
		public void Deconstruct()
		{
			try
			{
				Stop();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>設定資料庫參數</summary>
		public virtual void SetDatabaseParameters(string DatabaseServerAddressIp, string DatabaseServerAddressPort, string UserAccount, string UserPassword, string InitialDatabase)
		{
			try
			{
				mDatabaseServerAddressIp = DatabaseServerAddressIp;
				mDatabaseServerAddressPort = DatabaseServerAddressPort;
				mUserAccount = UserAccount;
				mUserPassword = UserPassword;
				mInitialDatabase = InitialDatabase;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>從檔案讀取尚未執行的 Sql 指令。執行緒開始執行</summary>
		public virtual void Start()
		{
			ReadRemainingSqlCmdsFromFile();
			InitializeThread();
		}
		/// <summary>執行尚未執行的 Sql 指令或是將尚未執行的 Sql 指令寫入檔案。執行緒停止執行</summary>
		public virtual void Stop()
		{
			if (mIsConnected)
			{
				DequeueAndExecuteSqlCmds();
			}
			else
			{
				WriteRemainingSqlCmdsToFile();
			}
			DestroyThread();
		}
		/// <summary>建立連線，並回傳建立結果</summary>
		public abstract bool Connect();
		/// <summary>排程非查詢類 Sql 指令，受影響的資料行數將由事件回報</summary>
		public virtual void EnqueueNonQueryCommand(string NonQueryCmd)
		{
			try
			{
				if (NonQueryCmd != string.Empty)
				{
					lock (mLockOfContainerOfNonQueryCmds)
					{
						mContainerOfNonQueryCmds.Add(NonQueryCmd);
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>排程查詢類 Sql 指令，查詢結果將由事件回報</summary>
		public virtual void EnqueueQueryCommand(string QueryCmd)
		{
			try
			{
				if (QueryCmd != string.Empty)
				{
					lock (mLockOfContainerOfQueryCmds)
					{
						mContainerOfQueryCmds.Add(QueryCmd);
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>執行非查詢類 Sql 指令，並回傳受影響的資料行數</summary>
		public abstract int ExecuteNonQueryCommand(string NonQueryCmd);
		/// <summary>執行非查詢類 Sql 指令(複數)，並回傳受影響的資料行數(複數)。通常會使用 Transaction 加快執行速度</summary>
		public abstract int[] ExecuteNonQueryCommands(IEnumerable<string> NonQueryCmds);
		/// <summary>執行查詢類 Sql 指令，並回傳查詢結果</summary>
		public abstract DataSet ExecuteQueryCommand(string QueryCmd);
		/// <summary>執行查詢類 Sql 指令(複數)，並回傳查詢結果(複數)。通常會使用 Transaction 加快執行速度</summary>
		public abstract DataSet[] ExecuteQueryCommands(IEnumerable<string> QueryCmds);

		/// <summary>初始化執行緒</summary>
		protected virtual void InitializeThread()
		{
			try
			{
				mThdProcessCmdsExitFlag = new bool[] { false };
				mThdProcessCmds = new Thread(() => TaskProcessCmds(mThdProcessCmdsExitFlag));
				mThdProcessCmds.IsBackground = true;
				mThdProcessCmds.Start();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>解構執行緒</summary>
		protected virtual void DestroyThread()
		{
			try
			{
				if (mThdProcessCmds != null)
				{
					if (mThdProcessCmds.IsAlive)
					{
						mThdProcessCmdsExitFlag[0] = true;
					}
					mThdProcessCmds = null;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>觸發事件 NonQueryCmdExecuted</summary>
		protected virtual void RaiseEvent_NonQueryCmdExecuted(string SqlCmd, int Result)
		{
			NonQueryCmdExecuted?.Invoke(SqlCmd, Result);
		}
		/// <summary>觸發事件 QueryCmdExecuted</summary>
		protected virtual void RaiseEvent_QueryCmdExecuted(string SqlCmd, DataSet Result)
		{
			QueryCmdExecuted?.Invoke(SqlCmd, Result);
		}
		/// <summary>從佇列中取出並執行 Sql 指令</summary>
		protected virtual void DequeueAndExecuteSqlCmds()
		{
			try
			{
				DequeueAndExecuteNonQueryCmd();
				DequeueAndExecuteQueryCmd();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>從佇列中取出並執行 Sql 指令(非查詢類)</summary>
		protected virtual void DequeueAndExecuteNonQueryCmd()
		{
			try
			{
				List<string> tmpNonQueryCmds = null;
				lock (mLockOfContainerOfNonQueryCmds)
				{
					if (mContainerOfNonQueryCmds.Count > 0)
					{
						tmpNonQueryCmds = mContainerOfNonQueryCmds.ToList();
						mContainerOfNonQueryCmds.Clear();
					}
					else
					{
						return;
					}
				}

				int[] tmpResults = ExecuteNonQueryCommands(tmpNonQueryCmds);
				for (int i = 0; i < tmpNonQueryCmds.Count; ++i)
				{
					RaiseEvent_NonQueryCmdExecuted(tmpNonQueryCmds[i], tmpResults[i]);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>從佇列中取出並執行 Sql 指令(查詢類)</summary>
		protected virtual void DequeueAndExecuteQueryCmd()
		{
			try
			{
				List<string> tmpQueryCmds = null;
				lock (mLockOfContainerOfQueryCmds)
				{
					if (mContainerOfQueryCmds.Count > 0)
					{
						tmpQueryCmds = mContainerOfQueryCmds.ToList();
						mContainerOfQueryCmds.Clear();
					}
					else
					{
						return;
					}
				}

				DataSet[] tmpResults = ExecuteQueryCommands(tmpQueryCmds);
				for (int i = 0; i < tmpQueryCmds.Count; ++i)
				{
					RaiseEvent_QueryCmdExecuted(tmpQueryCmds[i], tmpResults[i]);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>從檔案中讀取尚未執行的 Sql 指令</summary>
		protected virtual void ReadRemainingSqlCmdsFromFile()
		{
			try
			{
				ReadRemainingNonQueryCmdsFromFile();
				ReadRemainingQueryCmdsFromFile();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>從檔案中讀取尚未執行的 Sql 指令(非查詢類)</summary>
		protected virtual void ReadRemainingNonQueryCmdsFromFile()
		{
			try
			{
				if (File.Exists(mFilePathOfNonQueryCmds))
				{
					string[] tmp = File.ReadAllLines(mFilePathOfNonQueryCmds);
					if (tmp != null && tmp.Count() > 0)
					{
						lock (mLockOfContainerOfNonQueryCmds)
						{
							mContainerOfNonQueryCmds.AddRange(tmp);
						}
					}
					File.Delete(mFilePathOfNonQueryCmds);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>從檔案中讀取尚未執行的 Sql 指令(查詢類)</summary>
		protected virtual void ReadRemainingQueryCmdsFromFile()
		{
			try
			{
				if (File.Exists(mFilePathOfQueryCmds))
				{
					string[] tmp = File.ReadAllLines(mFilePathOfQueryCmds);
					if (tmp != null && tmp.Count() > 0)
					{
						lock (mLockOfContainerOfQueryCmds)
						{
							mContainerOfQueryCmds.AddRange(tmp);
						}
					}
					File.Delete(mFilePathOfQueryCmds);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>輸出尚未執行的 Sql 指令成檔案</summary>
		protected virtual void WriteRemainingSqlCmdsToFile()
		{
			try
			{
				WriteRemainingNonQueryCmdsToFile();
				WriteRemainingQueryCmdsToFile();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>輸出尚未執行的 Sql 指令(非查詢類)成檔案</summary>
		protected virtual void WriteRemainingNonQueryCmdsToFile()
		{
			try
			{
				lock (mLockOfContainerOfNonQueryCmds)
				{
					if (mContainerOfNonQueryCmds.Count() > 0)
					{
						File.WriteAllLines(mFilePathOfNonQueryCmds, mContainerOfNonQueryCmds);
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>輸出尚未執行的 Sql 指令(查詢類)成檔案</summary>
		protected virtual void WriteRemainingQueryCmdsToFile()
		{
			try
			{
				lock (mLockOfContainerOfQueryCmds)
				{
					if (mContainerOfQueryCmds.Count() > 0)
					{
						File.WriteAllLines(mFilePathOfQueryCmds, mContainerOfQueryCmds);
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>例外處理</summary>
		protected virtual void HandleException(Exception Ex)
		{
			File.AppendAllText(mFilePathOfExceptionRecord, $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")} - {Ex.ToString()}\n");
		}
		/// <summary>例外處理 (Sql)</summary>
		protected virtual void HandleDbException(DbException DbEx)
		{
			mIsConnected = false;
			HandleException(DbEx);
		}
		/// <summary>未連線時，自動連線。連線時，處理佇列中 Sql 指令</summary>
		protected virtual void TaskProcessCmds(bool[] ExitFlag)
		{
			try
			{
				mIsExecuting = true;
				while (!ExitFlag[0])
				{
					try
					{
						if (mIsConnected)
						{
							DequeueAndExecuteSqlCmds();
							Thread.Sleep(mPeriodOfProcessCmd);
						}
						else
						{
							if (!Connect())
							{
								Thread.Sleep(mPeriodOfTestConnectStatus);
							}
						}
					}
					catch (Exception ex)
					{
						HandleException(ex);
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				mIsExecuting = false;
			}
		}

		/// <summary>使用 Ping 進行連線測試</summary>
		public static IPStatus GetPingStatus(string IP)
		{
			if (IPAddress.TryParse(IP, out IPAddress tmp))
			{
				return mPing.Send(IP).Status;
			}
			else
			{
				return IPStatus.Unknown;
			}
		}
	}
}
