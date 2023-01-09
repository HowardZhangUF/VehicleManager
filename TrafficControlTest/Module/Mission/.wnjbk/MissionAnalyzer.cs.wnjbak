using System.Collections.Generic;
using System.Linq;
using TrafficControlTest.Library;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Module.Mission
{
	public abstract class MissionAnalyzer : IMissionAnalyzer
	{
		public abstract MissionType mMissionType { get; }
		public virtual string mKeyword { get { return mMissionType.ToString(); } }
		public virtual string mKeyItem { get; } = "Mission";
		public static int mPriorityDefault = 50;
		public static int mPriorityMax = 99;
		public static int mPriorityMin = 1;

		protected abstract string[] mNecessaryItem { get; }
		protected abstract string[] mOptionalItem { get; }

		protected readonly Dictionary<string, string> mItemCollection = new Dictionary<string, string>();

		public MissionAnalyzeResult TryParse(string Message, out IMission Mission, out string AnalyzeFailedDetail)
		{
			MissionAnalyzeResult result = MissionAnalyzeResult.Failed;
			Mission = null;
			AnalyzeFailedDetail = string.Empty;
			if (FillDictionaryUsingString(Message, ref AnalyzeFailedDetail) == MissionAnalyzeResult.Successed)
			{
				if (FillIMissionUsingDictionary(ref Mission, ref AnalyzeFailedDetail) == MissionAnalyzeResult.Successed)
				{
					result = MissionAnalyzeResult.Successed;
				}
				else
				{
					result = MissionAnalyzeResult.Failed;
				}
			}
			else
			{
				result = MissionAnalyzeResult.Failed;
			}
			InitializeDictionaryItemValue();
			return result;
		}

		protected MissionAnalyzer()
		{
			InitializeDictionary();
		}
		/// <summary>初始化字典。建構時執行</summary>
		protected void InitializeDictionary()
		{
			if (mItemCollection.Count > 0) mItemCollection.Clear();

			mItemCollection.Add(mKeyItem, mKeyword);
			if (mNecessaryItem != null && mNecessaryItem.Length > 0)
			{
				foreach (string item in mNecessaryItem)
				{
					mItemCollection.Add(item, string.Empty);
				}
			}
			if (mOptionalItem != null && mOptionalItem.Length > 0)
			{
				foreach (string item in mOptionalItem)
				{
					mItemCollection.Add(item, string.Empty);
				}
			}
		}
		/// <summary>初始化字典的值。每次分析完成後執行</summary>
		protected void InitializeDictionaryItemValue()
		{
			mItemCollection[mKeyItem] = mKeyword;
			if (mNecessaryItem != null && mNecessaryItem.Length > 0)
			{
				foreach (string item in mNecessaryItem)
				{
					mItemCollection[item] = string.Empty;
				}
			}
			if (mOptionalItem != null && mOptionalItem.Length > 0)
			{
				foreach (string item in mOptionalItem)
				{
					mItemCollection[item] = string.Empty;
				}
			}
		}
		/// <summary>從 Message 中擷取資料並填入 Dictionary 中，並確認是否有缺少資料</summary>
		protected MissionAnalyzeResult FillDictionaryUsingString(string Message, ref string AnalyzeFailedDetail)
		{
			MissionAnalyzeResult result = MissionAnalyzeResult.Failed;
			if (Message.Contains($"{mKeyItem}={mKeyword}"))
			{
				if (ConvertToDictionary(Message, out Dictionary<string, string> MessageDictionary))
				{
					// Check Necessary Item
					List<string> lackedNecessaryItem = null;
					foreach (string item in mNecessaryItem)
					{
						if (MessageDictionary.Keys.Contains(item))
						{
							mItemCollection[item] = MessageDictionary[item];
						}
						else
						{
							if (lackedNecessaryItem == null) lackedNecessaryItem = new List<string>();
							lackedNecessaryItem.Add(item);
						}
					}

					if (lackedNecessaryItem == null || lackedNecessaryItem.Count == 0)
					{
						result = MissionAnalyzeResult.Successed;

						// Check Optional Item
						foreach (string item in mOptionalItem)
						{
							if (MessageDictionary.Keys.Contains(item))
							{
								mItemCollection[item] = MessageDictionary[item];
							}
						}
					}
					else
					{
						result = MissionAnalyzeResult.Failed;
						AnalyzeFailedDetail = $"LackOf\"{string.Join(",", lackedNecessaryItem)}\"Parameter";
					}
				}
				else
				{
					result = MissionAnalyzeResult.Failed;
					AnalyzeFailedDetail = $"DataSyntaxError";
				}
			}
			else
			{
				result = MissionAnalyzeResult.Failed;
				AnalyzeFailedDetail = $"UnknownMissionType";
			}
			return result;
		}
		/// <summary>從 Dictionary 中擷取資料並填入 IMission 中，並確認資料格式是否正確</summary>
		protected abstract MissionAnalyzeResult FillIMissionUsingDictionary(ref IMission Mission, ref string AnalyzeFailedDetail);
	}
}
