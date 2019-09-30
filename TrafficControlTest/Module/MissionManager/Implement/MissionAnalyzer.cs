using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlTest.Module.MissionManager.Interface;
using static TrafficControlTest.Library.Library;

namespace TrafficControlTest.Module.MissionManager.Implement
{
	public abstract class MissionAnalyzer : IMissionAnalyzer
	{
		public abstract string mKeyword { get; }
		public static int mPriorityDefault = 50;
		protected abstract string mKeyItem { get; }
		protected abstract string[] mNecessaryItem { get; }
		protected abstract string[] mOptionalItem { get; }
		protected static int mPriorityMax = 99;
		protected static int mPriorityMin = 1;

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
			if (Message.Contains(mKeyword))
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
						AnalyzeFailedDetail = $"Lack Of \"{string.Join(", ", lackedNecessaryItem)}\" Parameters.";
					}
				}
				else
				{
					result = MissionAnalyzeResult.Failed;
					AnalyzeFailedDetail = $"Data Syntax Error.";
				}
			}
			else
			{
				result = MissionAnalyzeResult.Failed;
				AnalyzeFailedDetail = $"Can Not Find the Command Type.";
			}
			return result;
		}
		/// <summary>從 Dictionary 中擷取資料並填入 IMission 中，並確認資料格式是否正確</summary>
		protected abstract MissionAnalyzeResult FillIMissionUsingDictionary(ref IMission Mission, ref string AnalyzeFailedDetail);
	}
}
