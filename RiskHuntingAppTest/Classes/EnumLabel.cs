using System;

namespace RiskHuntingAppTest
{
	public class EnumLabelAttribute : Attribute
	{
		public string Label { get; private set; }
		public EnumLabelAttribute(string label) { Label = label; }
	}

	public static class EnumLabelExtension
	{
		public static string GetLabel(this Enum @enum)
		{
			string value = null;
			var fieldInfo = @enum.GetType().GetField(@enum.ToString());
			var attrs = fieldInfo.GetCustomAttributes(typeof(EnumLabelAttribute), false) as EnumLabelAttribute[];
			if (attrs != null) value = attrs.Length > 0 ? attrs[0].Label : null;
			return value;
		}
	}
}

