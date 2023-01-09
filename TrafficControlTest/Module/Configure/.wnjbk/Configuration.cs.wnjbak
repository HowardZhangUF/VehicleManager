using TrafficControlTest.Library;

namespace TrafficControlTest.Module.Configure
{
	public enum ConfigurationType
	{
		Int,
		Double,
		Bool,
		String
	}

	public enum ConfigurationLevel
	{
		Normal,
		Advance
	}

	public class Configuration
	{
		public string mCategory { get; private set; } = string.Empty;
		public string mName { get; private set; } = string.Empty;
		public string mFullName { get { return $"{mCategory}/{mName}"; } }
		public ConfigurationType mType { get; private set; } = ConfigurationType.String;
		public ConfigurationLevel mLevel { get; private set; } = ConfigurationLevel.Normal;
		public string mValue { get; private set; } = string.Empty;
		public string mValueDefault { get; private set; } = string.Empty;
		public string mValueMin { get; private set; } = string.Empty;
		public string mValueMax { get; private set; } = string.Empty;
		public string mDescriptionEnus { get; private set; } = string.Empty;
		public string mDescriptionZhtw { get; private set; } = string.Empty;
		public string mDescriptionZhcn { get; private set; } = string.Empty;

		public Configuration(string Category, string Name, ConfigurationType Type, ConfigurationLevel Level, string ValueDefault, string ValueMin, string ValueMax, string DescriptionEnus, string DescriptionZhtw, string DescriptionZhcn)
		{
			mCategory = Category;
			mName = Name;
			mType = Type;
			mLevel = Level;
			mValueDefault = ValueDefault;
			mValueMin = ValueMin;
			mValueMax = ValueMax;
			mDescriptionEnus = DescriptionEnus;
			mDescriptionZhtw = DescriptionZhtw;
			mDescriptionZhcn = DescriptionZhcn;

			mValue = mValueDefault;
		}
		public string GetDescription(Language Language)
		{
			string result = string.Empty;
			switch (Language)
			{
				case Language.Enus:
					result = mDescriptionEnus;
					break;
				case Language.Zhtw:
					result = mDescriptionZhtw;
					break;
				case Language.Zhcn:
					result = mDescriptionZhcn;
					break;
				default:
					result = mDescriptionEnus;
					break;
			}
			return result;
		}
		public bool SetValue(string Value)
		{
			if (VerifyNewValue(Value))
			{
				mValue = Value;
				return true;
			}
			else
			{
				return false;
			}
		}
		public override string ToString()
		{
			return $"{mFullName}={mValue}";
		}
		public string[] GetDataGridViewRowData(Language Language)
		{
			return new string[] { mCategory, mName, mValue, GetDescription(Language), mValueMin, mValueMax, mValueDefault };
		}

		private bool VerifyNewValue(string NewValue)
		{
			bool result = false;
			switch (mType)
			{
				case ConfigurationType.Int:
					if (int.TryParse(NewValue, out int tmp1) && NewValue != mValue)
					{
						int max = int.Parse(mValueMax);
						int min = int.Parse(mValueMin);
						if (tmp1 >= min && tmp1 <= max)
						{
							result = true;
						}
					}
					break;
				case ConfigurationType.Double:
					if (double.TryParse(NewValue, out double tmp2) && NewValue != mValue)
					{
						double max = double.Parse(mValueMax);
						double min = double.Parse(mValueMin);
						if (tmp2 >= min && tmp2 <= max)
						{
							result = true;
						}
					}
					break;
				case ConfigurationType.Bool:
					if (bool.TryParse(NewValue, out bool tmp3) && NewValue != mValue)
					{
						result = true;
					}
					break;
				case ConfigurationType.String:
					if (NewValue != mValue)
					{
						result = true;
					}
					break;
				default:
					result = false;
					break;
			}
			return result;
		}
	}
}
